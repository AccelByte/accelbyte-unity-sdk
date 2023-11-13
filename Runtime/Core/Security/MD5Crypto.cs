// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AccelByte.Core
{
	public class MD5Crypto
    {
        private readonly string hashKey;

        public MD5Crypto(string key)
        {
            hashKey = key;
            if(hashKey == null)
            {
                hashKey = string.Empty;
            }
        }

        public string Encrypt(string plainText)
        {
            const bool async = false;
            string retval = null;
            EncryptImp(plainText, async, (result) =>
            {
                retval = result;
            });
            return retval;
        }

        public string Decrypt(string cipherText)
        {
            byte[] bData = Convert.FromBase64String(cipherText);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashKey));
            tripleDES.Mode = CipherMode.ECB;

            string plainText = cipherText;
            try
            {
                ICryptoTransform trnsfrm = tripleDES.CreateDecryptor();
                byte[] result = trnsfrm.TransformFinalBlock(bData, 0, bData.Length);
                plainText = UTF8Encoding.UTF8.GetString(result);
            }
            catch(Exception ex)
            {
                AccelByteDebug.LogWarning($"Decryption failed, {ex.Message}");
            }
            return plainText;
        }

        public void EncryptAsync(string plainText, Action<string> callback)
        {
            const bool async = true;
            EncryptImp(plainText, async, callback);
        }

        private async void EncryptImp(string plainText, bool async, Action<string> callback)
        {
            byte[] bData = UTF8Encoding.UTF8.GetBytes(plainText);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashKey));
            tripleDES.Mode = CipherMode.ECB;

            var stream = new MemoryStream();
            using (var transform = tripleDES.CreateEncryptor())
            {
                using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        if (async)
                        {
                            await writer.WriteAsync(plainText);
                        }
                        else
                        {
                            writer.Write(plainText);
                        }
                    }
                }
            }

            byte[] result = stream.ToArray();
            string cipherText = Convert.ToBase64String(result);
            callback?.Invoke(cipherText);
        }
    }
}
