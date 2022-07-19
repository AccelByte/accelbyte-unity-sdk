// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using AccelByte.Models;
using UnityEngine.TestTools;

namespace AccelByte.Core
{
    /// <summary>
    /// Awesome Format: AccelByte Web Socket Messaging Format
    /// </summary>
    public static class AwesomeFormat
    {
        public static void WriteHeader(StringWriter writer, MessageType type, long id = -1, ErrorCode? code = null)
        {
            writer.NewLine = "\n";
            writer.Write("type: {0}", type);

            if (id != -1)
            {
                writer.WriteLine();
                writer.Write("id: {0}", id);
            }

            if (code != null)
            {
                writer.WriteLine();
                writer.Write("code: {0:D}", code);
            }
        }

        public static void WritePayload<T>(StringWriter writer, T inputPayload) where T : class
        {
            writer.NewLine = "\n";

            foreach (var fieldInfo in typeof(T).GetFields().Where(x => x.IsDefined(typeof(DataMemberAttribute), false)))
            {
                writer.WriteLine();

                string fieldName = ((DataMemberAttribute) fieldInfo.GetCustomAttribute(typeof(DataMemberAttribute))).Name; 
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName = fieldInfo.Name;
                }

                if (fieldInfo.FieldType.IsArray)
                {
                    writer.Write("{0}: [", fieldName);

                    Array items = (Array) fieldInfo.GetValue(inputPayload);

                    for (int i = 0; i < items.Length - 1; i++)
                    {
                        writer.Write(items.GetValue(i) + ",");
                    }

                    writer.Write(items.GetValue(items.Length - 1));
                    writer.Write("]");
                }
                else if (fieldInfo.FieldType.IsPrimitive || fieldInfo.FieldType == typeof(string))
                {
                    writer.Write("{0}: {1}", fieldName, fieldInfo.GetValue(inputPayload));
                }
                else if (fieldInfo.FieldType == typeof(DateTime))
                {
                    DateTime inputValue = ((DateTime) fieldInfo.GetValue(inputPayload)).ToUniversalTime();
                    writer.Write("{0}: {1:O}", fieldName, inputValue);
                }
                else
                {
                    writer.Write("{0}: {1:G}", fieldName, fieldInfo.GetValue(inputPayload));
                }
            }
        }

        public static ErrorCode ReadHeader(string message, out MessageType type, out long id)
        {
            type = MessageType.unknown;
            id = -1;
            StringReader reader = new StringReader(message);
            string strType = reader.ReadLine();

            if (string.IsNullOrEmpty(strType) || !strType.StartsWith("type: "))
            {
                return ErrorCode.MessageFormatInvalid;
            }

            try
            {
                strType = strType.Substring(strType.IndexOf(": ") + 2);
                type = (MessageType) Enum.Parse(typeof(MessageType), strType);
            }
            catch (Exception)
            {
                return ErrorCode.MessageTypeNotSupported;
            }

            string strId = reader.ReadLine();

            if (string.IsNullOrEmpty(strId) || !strId.StartsWith("id: "))
            {
                return ErrorCode.None;
            }

            if (!long.TryParse(strId.Substring(strId.IndexOf(": ") + 2), out id))
            {
                return ErrorCode.MessageFieldConversionFailed;
            }

            string strCode = reader.ReadLine();

            if (string.IsNullOrEmpty(strCode) || !strCode.StartsWith("code: "))
            {
                return ErrorCode.None;
            }

            uint code;

            if (!uint.TryParse(strCode.Substring(strCode.IndexOf(": ") + 2), out code))
            {
                return ErrorCode.MessageFieldConversionFailed;
            }

            return (ErrorCode) code;
        }

        public static ErrorCode ReadPayload<T>(string message, out T payload) where T : class, new()
        {
            payload = default(T);
            var fields = AwesomeFormat.ParseFields(message);

            if (fields == null)
            {
                return ErrorCode.MessageFormatInvalid;
            }

            return AwesomeFormat.MakePayloadFromFields(fields, out payload);
        }

        private static Dictionary<string, string> ParseFields(string inputString)
        {
            var fields = new Dictionary<string, string>();

            foreach (string line in inputString.TrimEnd('\n').Split('\n'))
            {
                int splitIndex = line.IndexOf(": ");

                if (splitIndex == -1)
                {
                    return null;
                }

                var key = line.Substring(0, splitIndex).Trim(' ');
                var value = line.Substring(splitIndex + 2).Trim(' ');

                if (string.IsNullOrEmpty(key))
                {
                    return null;
                }

                if (fields.ContainsKey(key))
                {
                    return null;
                }

                fields[key] = value;
            }

            return fields;
        }

        private static ErrorCode MakePayloadFromFields<T>(Dictionary<string, string> fields, out T payload)
            where T : class, new()
        {
            payload = new T();
            var fieldInfos = typeof(T).GetFields().Where(x => x.IsDefined(typeof(DataMemberAttribute), false));

            foreach (var fieldInfo in fieldInfos)
            {
                string fieldValue;

                if (!fields.TryGetValue(fieldInfo.Name, out fieldValue))
                {
                    continue;
                }

                if (fieldInfo.FieldType.IsArray)
                {
                    var trimmedString = fieldValue.Trim('[', ']', ',', ' ');

                    if (String.IsNullOrEmpty(trimmedString))
                    {
                        fieldInfo.SetValue(payload, Array.CreateInstance(fieldInfo.FieldType.GetElementType(), 0));
                    }
                    else
                    {
                        var strItems = trimmedString.Split(',');
                        var array = Array.CreateInstance(fieldInfo.FieldType.GetElementType(), strItems.Length);

                        try
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                if (fieldInfo.FieldType.GetElementType().IsEnum)
                                {
                                    var obj = Activator.CreateInstance(fieldInfo.FieldType.GetElementType());
                                    obj = Enum.Parse(fieldInfo.FieldType.GetElementType(), Uri.UnescapeDataString((string)strItems[i]), true);
                                    array.SetValue(obj, i);
                                }
                                else
                                {
                                    var parsedValue = Convert.ChangeType(strItems[i].Trim(' '), fieldInfo.FieldType.GetElementType());

                                    if (fieldInfo.FieldType.GetElementType() == typeof(string))
                                    {
                                        array.SetValue(Uri.UnescapeDataString((string)parsedValue), i);
                                    }
                                    else
                                    {
                                        array.SetValue(parsedValue, i);
                                    }
                                }
                            }

                            fieldInfo.SetValue(payload, array);
                        }
                        catch (Exception e)
                        {
                            AccelByteDebug.Log($"Error parsing field {fieldInfo.Name}\n{e}");
                            return ErrorCode.MessageFieldConversionFailed;
                        }
                    }
                }
                else if (fieldInfo.FieldType == typeof(string))
                {
                    fieldInfo.SetValue(payload, Uri.UnescapeDataString(fieldValue));
                }
                else if (fieldInfo.FieldType.IsPrimitive)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(fieldValue, fieldInfo.FieldType);
                        fieldInfo.SetValue(payload, convertedValue);
                    }
                    catch (Exception)
                    {
                        return ErrorCode.MessageFieldConversionFailed;
                    }
                }
                else if (fieldInfo.FieldType == typeof(DateTime))
                {
                    DateTime convertedValue;

                    if (DateTime.TryParse(
                        fieldValue,
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal,
                        out convertedValue))
                    {
                        fieldInfo.SetValue(payload, convertedValue);
                    }
                    else
                    {
                        return ErrorCode.MessageFieldConversionFailed;
                    }
                }
                else if (fieldInfo.FieldType.IsEnum)
                {
                    try
                    {
                        var obj = Activator.CreateInstance(fieldInfo.FieldType);
                        obj = Enum.Parse(fieldInfo.FieldType, fieldValue, true);
                        fieldInfo.SetValue(payload, obj);
                    }
                    catch (Exception)
                    {
                        return ErrorCode.MessageFieldConversionFailed;
                    }
                }
                else if (fieldInfo.FieldType == typeof(Dictionary<string, object>)) // Used by party storage
                {
                    try
                    {
                        //var convertedValue = Utf8Json.JsonSerializer.Deserialize<Dictionary<string, object>>(fieldValue);
                        var convertedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(fieldValue);
                        fieldInfo.SetValue(payload, convertedValue);
                    }
                    catch (Exception)
                    {
                        return ErrorCode.MessageFieldConversionFailed;
                    }
                }
                else if (fieldInfo.FieldType == typeof(Dictionary<string, int>)) // Used dsnotif
                {
                    try
                    {
                        var convertedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(fieldValue);
                        fieldInfo.SetValue(payload, convertedValue);
                    }
                    catch (Exception)
                    {
                        return ErrorCode.MessageFieldConversionFailed;
                    }
                }
                else if (fieldInfo.FieldType == typeof(Dictionary<string, string>)) // Used session attribute
                {
                    try
                    {
                        var convertedValue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(fieldValue);
                        fieldInfo.SetValue(payload, convertedValue);
                    }
                    catch (Exception)
                    {
                        return ErrorCode.MessageFieldConversionFailed;
                    }
                }
                else if (fieldValue.StartsWith("{"))
                {
                    try
                    {
                        var convertedValue = Newtonsoft.Json.JsonConvert.DeserializeObject(fieldValue,fieldInfo.FieldType);
                        fieldInfo.SetValue(payload, convertedValue);
                    }
                    catch (Exception)
                    {
                        return ErrorCode.MessageFieldConversionFailed;
                    }
                }
                else
                {
                    return ErrorCode.MessageFieldTypeNotSupported;
                }
            }

            return ErrorCode.None;
        }
    }
}
