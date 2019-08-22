// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using Utf8Json;

namespace AccelByte.Core {
    public static class JsonExtension
    {
        public static byte[] ToUtf8Json<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static string ToJsonString<T>(this T obj)
        {
            return JsonSerializer.ToJsonString(obj);
        }

        public static T ToObject<T>(this string data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }

        public static T ToObject<T>(this byte[] data)
        {
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}