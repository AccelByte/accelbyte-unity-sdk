// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace AccelByte.Core
{
    public static class FormEncoding
    {
        public static string ToForm<T>(this T obj)
        {
            if (obj == null) throw new ArgumentException("Can't form-encode null");

            var properties = typeof(T).GetProperties();

            if (properties.Length == 0) throw new ArgumentException("Can't form-encode type without public properties");

            Dictionary<string, string> formParams = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var name = FormEncoding.GetMemberName(property);

                if (FormEncoding.IsPropertyIgnored(property)) continue;

                object value = property.GetValue(obj);

                if (FormEncoding.TryEncodeProperty(property, value, out var encodedValue))
                {
                    formParams.Add(Uri.EscapeDataString(name), Uri.EscapeDataString(encodedValue));
                }
            }

            return string.Join("&", formParams.Select(p => $"{p.Key}={p.Value}").ToArray());
        }
        
        private static string GetMemberName(MemberInfo member)
        {
            string name = member.Name;
            DataMemberAttribute nameAttribute = member.GetCustomAttribute<DataMemberAttribute>();

            if (nameAttribute != null && !string.IsNullOrEmpty(nameAttribute.Name))
            {
                name = nameAttribute.Name;
            }

            return name;
        }

        private static bool TryEncodeProperty(PropertyInfo property, object value, out string encodedValue)
        {
            Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            if (value == null)
            {
                encodedValue = null;
                
                return false;
            }

            if (type == typeof(DateTime))
            {
                encodedValue = (value is DateTime d ? d : default).ToString("O");
                
                return true;
            }

            if (type == typeof(string) || type.IsPrimitive)
            {
                encodedValue = value.ToString();
                
                return true;
            }

            if (type.IsEnum)
            {
                encodedValue = FormEncoding.GetMemberName(type.GetMember(Enum.GetName(type, value) ?? "")[0]);
                
                return true;
            }

            throw new ArgumentException("Can't form-encode unsupported type: " + property.PropertyType.Name);
        }

        private static bool IsPropertyIgnored(MemberInfo property)
        {
            return property.GetCustomAttribute<IgnoreDataMemberAttribute>() != null;
        }
    }
}