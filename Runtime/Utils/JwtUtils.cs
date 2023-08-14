// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Text;

namespace AccelByte.Utils
{
    public class JwtUtils
    {
        public static void UnescapeB64Url(ref string base64)
        {
            if (base64 == null)
            {
                throw new ArgumentNullException(nameof(base64));
            }

            base64 = base64.Replace('-', '+').Replace('_', '/');
            int mod4 = base64.Length % 4;
            if (mod4 > 0)
            {
                base64 += new string('=', 4 - mod4);
            }
        }

        public static string Base64UrlDecode(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            string base64 = input.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(base64);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                return string.Empty;
            }
        }
    }
}
