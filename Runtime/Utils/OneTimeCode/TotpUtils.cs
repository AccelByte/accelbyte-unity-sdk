// Copyright (c) 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AccelByte.Utils
{
    public class TotpUtils
    {
        /// <summary>
        /// Can be used for game client to generate several digit of totp
        /// It is used when the session config's enable secret is true
        /// </summary>
        /// <param name="secretKey">player's onJoinSession secret received from lobby notification</param>
        public string GenerateTotp(string secretKey)
        {
            return GenerateTotp(secretKey, null);
        }

        /// <summary>
        /// Can be used for game client to generate several digit of totp
        /// It is used when the session config's enable secret is true
        /// </summary>
        /// <param name="secretKey">player's onJoinSession secret received from lobby notification</param>
        /// <param name="optionalParameter">optional parameter that can be set</param>
        /// <returns></returns>
        public string GenerateTotp(string secretKey, GenerateTotpOptionalParameter optionalParameter)
        {
            if (optionalParameter == null)
            {
                optionalParameter = new GenerateTotpOptionalParameter();
            }

            string result = null;
            var serverTime = AccelByteSDK.TimeManager.GetCachedServerTime().ServerTime;
            long currentTime = ((DateTimeOffset)serverTime).ToUnixTimeSeconds();

            result = GenerateTOTPMain(currentTime, secretKey, optionalParameter.CodeLength, optionalParameter.TimeStep);
            UnityEngine.Debug.Log($"unity");
            return result;
        }

        private string GenerateTOTPMain(long currentTime, string secretKey, uint codeLength, uint timeStep)
        {
            long timeStepCount = currentTime / timeStep;
            byte[] timeBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(timeStepCount));
            byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

            using (var hmac = new HMACSHA1(secretBytes))
            {
                string result = string.Empty;
                byte[] hash = hmac.ComputeHash(timeBytes);

                //Dynamic hash
                int offset = hash[hash.Length - 1] & 0x0F;
                int binaryCode =
                    ((hash[offset] & 0x7f) << 24)
                  | ((hash[offset + 1] & 0xff) << 16)
                  | ((hash[offset + 2] & 0xff) << 8)
                  | (hash[offset + 3] & 0xff);

                int otp = binaryCode % (int)Math.Pow(10, codeLength);
                result = otp.ToString().PadLeft((int)codeLength, '0');
                return result;
            }
        }

        /// <summary>
        /// Can be used for Dedicated Server (DS) for secure Handshaking
        /// </summary>
        /// <param name="secret">secret received from DSHub notification</param>
        /// <param name="receivedTotp">player's Totp</param>
        /// <param name="userId">player's userId</param>
        /// <returns>reutrn whether it sitll a valid totp</returns>
        public bool ValidateTotp(string secret, string receivedTotp, string userId)
        {
            bool isValid = false;
            var acceptableTotp = GenerateAcceptableTotp(secret, userId);

            foreach (var item in acceptableTotp)
            {
                UnityEngine.Debug.Log($"+++ acceptable totp: {item}");
            }

            isValid = acceptableTotp.Contains(receivedTotp);
            return isValid;
        }

        private List<string> GenerateAcceptableTotp(string secret, string userId)
        {
            var optionalParameter = new GenerateTotpOptionalParameter();
            uint acceptableWindow = optionalParameter.TimeStep;
            uint codeLength = optionalParameter.CodeLength;

            List<string> acceptableTOTPs = new List<string>();
            string hashString = GenerateHashString(secret + userId);

            var serverTime = AccelByteSDK.TimeManager.GetCachedServerTime().ServerTime;
            long currentTime = ((DateTimeOffset)serverTime).ToUnixTimeSeconds();

            for (int i = 0; i < acceptableWindow; i++)
            {
                string totp = GenerateTOTPMain(currentTime - i, hashString, codeLength, 30);
                if (!acceptableTOTPs.Contains(totp))
                {
                    acceptableTOTPs.Add(totp);
                }
            }

            return acceptableTOTPs;
        }

        private string GenerateHashString(string message)
        {
            using (var sha = SHA1.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);
                byte[] hash = sha.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}