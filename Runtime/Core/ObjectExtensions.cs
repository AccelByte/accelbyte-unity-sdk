// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace AccelByte.Core
{
    public static class ObjectExtensions
    {
        public static T ToObject<T>( this IDictionary<string, object> source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance )
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach( var item in source )
            {
                PropertyInfo asPropInfo = someObjectType.GetProperties( bindingAttr )
                    .Where( propInfo => Attribute.IsDefined( propInfo, typeof( DataMemberAttribute ) ) )
                    .SingleOrDefault( propInfo => ( (DataMemberAttribute) Attribute.GetCustomAttribute( propInfo, typeof( DataMemberAttribute ) ) )
                                            .Name == item.Key );
                if( asPropInfo == null )
                {
                    Type itemValueType = someObjectType.GetProperty(item.Key).PropertyType;
                    // number handler
                    if (itemValueType == typeof(int)||
                        itemValueType == typeof(uint)||
                        itemValueType == typeof(double)||
                        itemValueType == typeof(float)||
                        itemValueType == typeof(short)||
                        itemValueType == typeof(ushort)||
                        itemValueType == typeof(long)||
                        itemValueType == typeof(ulong)||
                        itemValueType == typeof(Single)||
                        itemValueType == typeof(Decimal))
                    {
                        var castedValue = Convert.ChangeType(item.Value, itemValueType);
                        someObjectType
                            .GetProperty(item.Key)
                            .SetValue(someObject, castedValue, null);
                    }
                    else
                    {
                        someObjectType
                                 .GetProperty( item.Key )
                                 .SetValue( someObject, item.Value, null );
                    }
                    
                }
                else
                {
                    asPropInfo.SetValue( someObject, item.Value, null );
                }
            }

            return someObject;
        }
        
        public static T[] ToArray<T>( this object source,
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance )
            where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            var list = ((IEnumerable)source).Cast<Dictionary<string, Object>>().ToList();
            var result = new List<T>();

            foreach( var item in list)
            {
                var object_ = item.ToObject<T>();
                result.Add(object_);
            }

            return result.ToArray();
        }

        public static IDictionary<string, object> AsDictionary( this object source, 
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance )
        {
            return source.GetType().GetProperties( bindingAttr ).ToDictionary
            (
                propInfo => Attribute.IsDefined( propInfo, typeof( DataMemberAttribute ) ) ?
                    ( (DataMemberAttribute) Attribute.GetCustomAttribute( propInfo, typeof( DataMemberAttribute ) ) ).Name
                    : propInfo.Name,
                propInfo => propInfo.GetValue( source, null )
            );
        }
    }
}
