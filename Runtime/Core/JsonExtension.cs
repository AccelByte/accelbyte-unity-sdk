// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Core
{
    public class JsonGenericDictionaryOrArrayConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
        {
            throw new NotImplementedException();
        }


        public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
        {
            return ReadValue( reader );
        }

        private object ReadValue( JsonReader reader )
        {
            while( reader.TokenType == JsonToken.Comment )
            {
                if( !reader.Read() )
                    throw new JsonSerializationException( "Unexpected Token when converting IDictionary<string, object>" );
            }

            switch( reader.TokenType )
            {
                case JsonToken.StartObject:
                    return ReadObject( reader );
                case JsonToken.StartArray:
                    return ReadArray( reader );
                case JsonToken.Integer:
                case JsonToken.Float:
                case JsonToken.String:
                case JsonToken.Boolean:
                case JsonToken.Undefined:
                case JsonToken.Null:
                case JsonToken.Date:
                case JsonToken.Bytes:
                    return reader.Value;
                default:
                    throw new JsonSerializationException
                        ( string.Format( "Unexpected token when converting IDictionary<string, object>: {0}", reader.TokenType ) );
            }
        }

        private object ReadArray( JsonReader reader )
        {
            IList<object> list = new List<object>();

            while( reader.Read() )
            {
                switch( reader.TokenType )
                {
                    case JsonToken.Comment:
                        break;
                    default:
                        var v = ReadValue( reader );

                        list.Add( v );
                        break;
                    case JsonToken.EndArray:
                        return list.ToArray();
                }
            }

            throw new JsonSerializationException( "Unexpected end when reading IDictionary<string, object>" );
        }

        private object ReadObject( JsonReader reader )
        {
            var obj = new Dictionary<string, object>();

            while( reader.Read() )
            {
                switch( reader.TokenType )
                {
                    case JsonToken.PropertyName:
                        var propertyName = reader.Value.ToString();

                        if( !reader.Read() )
                        {
                            throw new JsonSerializationException( "Unexpected end when reading IDictionary<string, object>" );
                        }

                        var v = ReadValue( reader );

                        obj[ propertyName ] = v;
                        break;
                    case JsonToken.Comment:
                        break;
                    case JsonToken.EndObject:
                        return obj;
                }
            }

            throw new JsonSerializationException( "Unexpected end when reading IDictionary<string, object>" );
        }

        public override bool CanConvert( Type objectType ) { return typeof( IDictionary<string, object> ).IsAssignableFrom( objectType ); }
    }

    public static class JsonExtension
    {
        public static string ToJsonString<T>( this T obj, Formatting format = Formatting.None )
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add( new StringEnumConverter() );
            return JsonConvert.SerializeObject( obj, format, jsonSerializerSettings );
        }

        public static byte[] ToUtf8Json<T>( this T obj, Formatting format = Formatting.None )
        {
            return System.Text.Encoding.UTF8.GetBytes( obj.ToJsonString() );
        }

        public static T ToObject<T>( this string data )
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add( new JsonGenericDictionaryOrArrayConverter() );
            return JsonConvert.DeserializeObject<T>( data, jsonSerializerSettings );
        }

        public static T ToObject<T>( this byte[] data )
        {
            return System.Text.Encoding.UTF8.GetString( data ).ToObject<T>();
        }
    }
}