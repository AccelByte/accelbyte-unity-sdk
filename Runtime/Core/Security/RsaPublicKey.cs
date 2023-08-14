// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Security.Cryptography;
using AccelByte.Utils;

namespace AccelByte.Core
{
    public class RsaPublicKey
    {
        private const int rsaModulusBase64Length = 342;
        private const int rsaExponentBase64Length = 4;

        public string ModulusB64Url { get; }
        public string ExponentB64Url { get; }

        public RsaPublicKey(string modulusB64Url, string exponentB64Url)
        {
            ModulusB64Url = modulusB64Url;
            ExponentB64Url = exponentB64Url;
        }

        public bool IsValid()
        {
            return ModulusB64Url != null &&
                   ExponentB64Url != null &&
                   ModulusB64Url.Length == rsaModulusBase64Length &&
                   ExponentB64Url.Length == rsaExponentBase64Length;
        }

        public RSAParameters GetRSAParameters()
        {
            if(!IsValid())
            {
                return default;
            }

            string unescapedModulus = ModulusB64Url;
            string unescapedExponent = ExponentB64Url;

            JwtUtils.UnescapeB64Url(ref unescapedModulus);
            JwtUtils.UnescapeB64Url(ref unescapedExponent);

            byte[] modulusBytes = Convert.FromBase64String(unescapedModulus);
            byte[] exponentBytes = Convert.FromBase64String(unescapedExponent);

            RSAParameters parameters = new RSAParameters
            {
                Modulus = modulusBytes,
                Exponent = exponentBytes
            };

            return parameters;
        }

        public string ToPem()
        {
            if (!IsValid())
            {
                return default;
            }

            string unarmoredPem = string.Format($"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA{ModulusB64Url}ID{ExponentB64Url}");

            JwtUtils.UnescapeB64Url(ref unarmoredPem);

            return string.Format("-----BEGIN PUBLIC KEY-----\n{0}\n-----END PUBLIC KEY-----", unarmoredPem);
        }
    }
}
