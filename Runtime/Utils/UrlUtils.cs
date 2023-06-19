// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Text.RegularExpressions;
using AccelByte.Core;


namespace AccelByte.Utils
{
    internal static class UrlUtils
    {
        public static string SanitizeBaseUrl(string baseURL)
        {
            string retval = string.Empty;
            if (!string.IsNullOrEmpty(baseURL))
            {
                var regexStr = "^https?|wss?:\\/\\/(?:www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$";
                if (Regex.IsMatch(baseURL, regexStr))
                {
                    retval = baseURL.TrimEnd('/');
                }
                else
                {
                    throw new System.ArgumentException(baseURL);
                }
            }
            return retval;
        }
    }
}
