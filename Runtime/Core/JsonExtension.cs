// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using Newtonsoft.Json;

namespace AccelByte.Core
{
    public static class JsonExtension
    {
        public static byte[] ToUtf8Json<T>(this T obj)
        {
            return System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
        }

        public static string ToJsonString<T>(this T obj,Formatting format = Formatting.None)
        {
            return JsonConvert.SerializeObject(obj,format);
        }

        public static T ToObject<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static T ToObject<T>(this byte[] data)
        {
            return JsonConvert.DeserializeObject<T>(System.Text.Encoding.UTF8.GetString(data));
        }
    }
}