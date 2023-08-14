// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using AccelByte.Utils;
using AccelByte.Models;

namespace AccelByte.Core
{
    public class Jwt
    {
        private const int rs256SignatureLength = 342;
        private const int expectedJwtPartsCount = 3;

        private string jwtString { get; }
        private int headerEnd { get; }
        private int payloadEnd { get; }
        private JObject headerJsonPtr { get; }
        private JObject payloadJsonPtr { get; }

        public Jwt(string inJwtString)
        {
            if (string.IsNullOrEmpty(inJwtString))
            {
                AccelByteDebug.LogWarning($"Jwt: JwtString is null or empty.");
                return;
            }

            jwtString = inJwtString;

            // Parse the JWT and extract header and payload
            string[] jwtParts = jwtString.Split('.');
            if (jwtParts.Length == expectedJwtPartsCount)
            {
                string headerJsonString = JwtUtils.Base64UrlDecode(jwtParts[0]);
                string payloadJsonString = JwtUtils.Base64UrlDecode(jwtParts[1]);

                try
                {
                    headerJsonPtr = JObject.Parse(headerJsonString);
                    payloadJsonPtr = JObject.Parse(payloadJsonString);

                    // Set headerEnd and payloadEnd values
                    headerEnd = jwtParts[0].Length;
                    payloadEnd = headerEnd + jwtParts[1].Length + 1;
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    AccelByteDebug.LogWarning($"Jwt: Failed to parse header or payload JSON.");
                }
            }
        }

        public EJwtResult VerifyWith(RsaPublicKey key)
        {
            if (!IsValid())
            {
                AccelByteDebug.LogWarning($"Jwt: FAIL: MalformedJwt.");
                return EJwtResult.MalformedJwt;
            }

            if (!key.IsValid())
            {
                AccelByteDebug.LogWarning($"Jwt: FAIL: MalformedPublicKey.");
                return EJwtResult.MalformedPublicKey;
            }

            if (!headerJsonPtr.ContainsKey("alg") || headerJsonPtr["alg"].ToString() != "RS256")
            {
                AccelByteDebug.LogWarning($"Jwt: FAIL: AlgorithmMismatch.");
                return EJwtResult.AlgorithmMismatch;
            }

            RSAParameters rsaParams = key.GetRSAParameters();

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);

                byte[] jwtBytes = Encoding.ASCII.GetBytes(jwtString);
                byte[] payloadBytes = new byte[payloadEnd];
                Buffer.BlockCopy(jwtBytes, 0, payloadBytes, 0, payloadEnd);

                string signatureB64 = jwtString.Substring(payloadEnd + 1);
                JwtUtils.UnescapeB64Url(ref signatureB64);
                byte[] signatureBytes = Convert.FromBase64String(signatureB64);

                using (var sha256 = SHA256.Create())
                {
                    byte[] hashedPayload = sha256.ComputeHash(payloadBytes);

                    bool verifyResult = rsa.VerifyHash(hashedPayload, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                    if (!verifyResult)
                    {
                        AccelByteDebug.LogWarning($"Jwt: FAIL: SignatureMismatch.");
                        return EJwtResult.SignatureMismatch;
                    }
                }
            }

            return EJwtResult.Ok;
        }

        public bool IsValid()
        {
            return headerJsonPtr != null &&
                   payloadJsonPtr != null &&
                   jwtString != null &&
                   headerEnd != -1 &&
                   payloadEnd != -1 &&
                   headerEnd != payloadEnd &&
                   jwtString.Length - payloadEnd - 1 == rs256SignatureLength;
        }

        public JObject Header()
        {
            return headerJsonPtr;
        }

        public JObject Payload()
        {
            return payloadJsonPtr;
        }
    }
}
