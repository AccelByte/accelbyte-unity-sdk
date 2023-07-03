// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Utils
{
    public static class JsonUtils
    {
        public static string SerializeWithStringEnum(object obj)
        {
            var converter = new StringEnumConverter();
            return JsonConvert.SerializeObject(obj, converter).Trim('\"');
        }
    }
}