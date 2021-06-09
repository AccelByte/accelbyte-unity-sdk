// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using AccelByte.Models;


namespace AccelByte.Core
{
    public static class JsonWebToken
    {
        static string publicKey;

        /// <summary>
        /// Try to decode the json web token, verify it, and return the object of the payload when successful.
        /// </summary>
        /// <typeparam name="T"> A class of the payload object. </typeparam>
        /// <param name="publicKey"> Public key to verify the token. </param>
        /// <param name="token"> Token that will be decoded. </param>
        /// <param name="result"> Payload object that will be return if the token is valid. </param>
        /// <param name="verifyPublicKey"> Do verification on public key. Set False to skip this. </param>
        /// <param name="verifyExpiration"> Do verification on expiration. Set False to skip this. </param>
        /// <returns></returns>
        public static bool TryDecodeToken<T>(string publicKey, string token, out T result, bool verifyPublicKey = true, bool verifyExpiration = true)
        {
            result = default(T);

            try
            {
                SetPublicKey(publicKey);
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(publicKey))
                {
                    return false;
                }

                if (!SplitToken(token, out string[] parts))
                {
                    return false;
                }

                var decodedPayload = ConvertStringToBase64(parts[(int)JsonWebTokenIndex.Payload]);
                var payloadData = decodedPayload.ToObject<T>();

                if (verifyPublicKey)
                {
                    if (!VerifySignature(parts))
                    {
                        return false;
                    }
                }
                if (verifyExpiration)
                {
                    if (!VerifyExpiration(decodedPayload))
                    {
                        return false;
                    }
                }

                result = payloadData;
                return true;
            }
            catch (Exception e)
            {
                // Ignored
            }

            return false;
        }

        static bool VerifyExpiration(byte[] decodedPayload)
        {
            var payloadData = decodedPayload.ToObject<Dictionary<string, object>>();

            if ((!payloadData.ContainsKey("exp") || payloadData["exp"] == null) 
                || (!payloadData.ContainsKey("iat") || payloadData["iat"] == null))
            {
                return false;
            }

            int expiration = Convert.ToInt32(payloadData["exp"]);
            int issuedAt = Convert.ToInt32(payloadData["iat"]);

            DateTime exp = DateTimeOffset.FromUnixTimeSeconds(expiration).DateTime;
            DateTime iat = DateTimeOffset.FromUnixTimeSeconds(issuedAt).DateTime;
            DateTime now = DateTime.UtcNow;

            // The current DateTime should be between Expiration and IssuedAt DateTime
            // The IssuedAt DateTime sometimes late and have a higher value around 1-3 seconds than current DateTime. 
            // Therefore, add 10 seconds deviation to anticipate that
            if (now <= exp && (now - iat).TotalSeconds > -10)
            {
                return true;
            }

            return false;
        }

        static bool VerifySignature(string[] parts)
        {
            var bytesToSign = Encoding.UTF8.GetBytes(string.Concat(parts[(int)JsonWebTokenIndex.Header], ".", parts[(int)JsonWebTokenIndex.Payload]));

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            var publicKeyBytes = Convert.FromBase64String(publicKey);

            byte[] exponentData = new byte[3];
            byte[] modulusData = new byte[256];

            Array.Copy(publicKeyBytes, publicKeyBytes.Length - exponentData.Length, exponentData, 0, exponentData.Length);
            Array.Copy(publicKeyBytes, publicKeyBytes.Length - exponentData.Length - 2 - modulusData.Length, modulusData, 0, modulusData.Length);

            rsa.ImportParameters(new RSAParameters()
            {
                Modulus = modulusData,
                Exponent = exponentData
            });

            var sha = SHA256.Create();
            var signature = sha.ComputeHash(bytesToSign);

            RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
            rsaDeformatter.SetHashAlgorithm("SHA256");
            if (rsaDeformatter.VerifySignature(signature, ConvertStringToBase64(parts[(int)JsonWebTokenIndex.Signature])))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void SetPublicKey(string key)
        {
            key = key.Replace("-----BEGIN PUBLIC KEY-----", "")
                .Replace("-----END PUBLIC KEY-----", "");

            publicKey = key;
        }

        static bool SplitToken(string token, out string[] parts)
        {
            parts = default;

            parts = token.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            return true;
        }

        static byte[] ConvertStringToBase64(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            string output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');

            switch (output.Length % 4) 
            {
                case 0: break;
                case 2: output += "=="; break;
                case 3: output += "="; break;
                default: return null;
            }

            return Convert.FromBase64String(output);
        }
    }
}