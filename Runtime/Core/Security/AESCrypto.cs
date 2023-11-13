// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.Assertions.Must;

namespace AccelByte.Core
{
    public class AESCrypto
    {
        /** Key size is 256 bits(32 bytes) (AES-256) */
        private const int defaultKeySizeInbits = 256;

        /** block size is 128 bits(16 bytes) (AES-256) */
        private int blockSizeInbits = 128;

        private CipherMode cipherMode = CipherMode.CBC;

        /** padding string consists of random data before the length. */
        private PaddingMode paddingMode = PaddingMode.ISO10126;

        /* Key Instance of the AES. */
        private AesCryptoServiceProvider provider;

        public AESCrypto() { }
        ~AESCrypto() { Clear(); }

        public bool GenerateKey(int inKeySize = defaultKeySizeInbits)
        {
            Clear();
            if ((provider = new AesCryptoServiceProvider()) != null)
            {
                provider.KeySize = inKeySize;
                provider.BlockSize = blockSizeInbits;
                provider.Padding = paddingMode;
                provider.Mode = cipherMode;

                provider.GenerateKey();
                provider.GenerateIV();

                return true;
            }
            throw new Exception($"AES: GenerateKey: the provider allocation failed.");
        }

        public bool ImportKey(byte[] inKey, byte[] inIV)
        {
            Clear();
            if ((provider = new AesCryptoServiceProvider()) != null)
            {
                provider.KeySize = inKey.Length * 8;
                provider.BlockSize = blockSizeInbits;
                provider.Padding = paddingMode;
                provider.Mode = cipherMode;

                SetKeyBytes(inKey);
                SetIVBytes(inIV);

                return true;
            }
            throw new Exception($"AES: ImportKey: the provider allocation failed.");
        }

        public byte[] Encrypt(byte[] plainText)
        {
            // Check arguments.
            if ((plainText is null) || (plainText.Length <= 0))
            {
                AccelByteDebug.LogWarning("plain text is null or empty.");
                return null;
            }

            if (provider != null)
            {
                // Create an encryptor to perform the stream transform.
                using (ICryptoTransform encryptor = provider?.CreateEncryptor(provider.Key, provider.IV))
                {
                    return PerformCryptography(plainText, encryptor);
                }
            }
            return plainText;
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            // Check arguments.
            if ((cipherText is null) || (cipherText.Length <= 0))
            {
                AccelByteDebug.LogWarning("cipher text is null or empty.");
                return null;
            }

            if (provider != null)
            {
                // Create a decryptor to perform the stream transform.
                using (ICryptoTransform decryptor = provider?.CreateDecryptor(provider.Key, provider.IV))
                {
                    var outArray = PerformCryptography(cipherText, decryptor);
                    return outArray;
                }
            }
            return cipherText;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                AccelByteDebug.LogWarning("plain text is null or empty.");
                return string.Empty;
            }

            var array = ABCryptoUtilities.StringToBytesWithoutNull(plainText);

            if (provider != null)
            {
                // Create an encryptor to perform the stream transform.
                using (ICryptoTransform encryptor = provider?.CreateEncryptor(provider.Key, provider.IV))
                {
                    var outArray = PerformCryptography(array, encryptor);
                    return ABCryptoUtilities.BytesToString(outArray);
                }
            }
            return plainText;
        }

        public string Decrypt(string cipherText)
        {
            var array = ABCryptoUtilities.StringToBytes(cipherText);

            if (provider != null)
            {
                // Create a decryptor to perform the stream transform.
                using (ICryptoTransform decryptor = provider.CreateDecryptor(provider.Key, provider.IV))
                {
                    var outArray = PerformCryptography(array, decryptor);
                    return ABCryptoUtilities.BytesToStringWithoutNull(outArray);
                }
            }
            return cipherText;
        }

        private byte[] PerformCryptography(byte[] inData, ICryptoTransform cryptoTransform)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(inData, 0, inData.Length);
                    cryptoStream.FlushFinalBlock();

                    var outArray = memoryStream.ToArray();

                    cryptoStream.Dispose();
                    memoryStream.Dispose();

                    return outArray;
                }
            }
        }

        public void Clear()
        {
            provider?.Clear();
            provider = null;
        }

        /** get the AES key */
        public byte[] GetKeyBytes() { return (provider?.Key); }

        /** get the initialization vector */
        public byte[] GetIVBytes() { return (provider?.IV); }

        /** set a AES key */
        public void SetKeyBytes(byte[] inKey) { provider.Key = (inKey); }

        /** set a initialization vector */
        public void SetIVBytes(byte[] inIv) { provider.IV = (inIv); }

        public int GetBlockSize()
        {
            if (provider is null)
            {
                return 0;
            }
            return provider.BlockSize;
        }

        public int GetKeySize()
        {
            if (provider is null)
            {
                return 0;
            }
            return provider.KeySize;
        }

        public bool IsActive()
        {
            return (provider != null);
        }
    }
}
