// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Security.Cryptography;

namespace AccelByte.Core
{
    public class RSACrypto
    {
        /** Size of the RSA key in bytes
         * Maximum plain text length that can be encrypted with public RSA key
         */

        /** Encryption size is 256 bytes (maxExponentBits / 8) */
        private const int maxExponentBits = 2048;

        /* Key Instance of the RSA. */
        private RSACryptoServiceProvider provider = null;
        private RSAParameters publicKey;

        public bool UseOaep { get; set; }

        public RSACrypto() { }
        ~RSACrypto()
        {
            Clear();
        }

        public byte[] ExportPublicKeyModulus()
        {
            return publicKey.Modulus;
        }

        public byte[] ExportPublicKeyExponent()
        {
            return publicKey.Exponent;
        }

        public int GetMaxDataSize()
        {
            if (provider is null)
            {
                return 0;
            }

            if (UseOaep)
            {
                return GetBlockSize() - 42;
            }
            else
            {
                return GetBlockSize() - 11;
            }
        }

        public bool ImportPublicKey(byte[] modulus, byte[] exponent, bool oaepInit = true)
        {
            Clear();
            if ((provider = new RSACryptoServiceProvider(maxExponentBits)) != null)
            {
                publicKey.Modulus = modulus;
                publicKey.Exponent = exponent;
                UseOaep = oaepInit;

                provider.ImportParameters(publicKey);

                return true;
            }

            throw new Exception($"RSA: ImportPublicKey: the provider allocation failed.");
        }

        public bool GenerateKey(bool oaepInit = true)
        {
            Clear();
            if ((provider = new RSACryptoServiceProvider(maxExponentBits)) != null)
            {
                UseOaep = oaepInit;
                publicKey = provider.ExportParameters(false);

                return true;
            }
            throw new Exception($"RSA: GenerateKey: the provider allocation failed.");
        }

        public byte[] Encrypt(byte[] plainText)
        {
            // Check arguments.
            if ((plainText is null) || (plainText.Length <= 0))
            {
                AccelByteDebug.LogWarning("plain text is null or empty.");
                return plainText;
            }

            // apply pkcs#1.5 padding and encrypt our data
            if (provider != null)
            {
                byte[] outArray = provider.Encrypt(plainText, UseOaep);
                return outArray;
            }

            return plainText;
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                AccelByteDebug.LogWarning("plain text is null or empty.");
                return string.Empty;
            }

            // for encryption, always handle bytes...
            var bytesPlainText = ABCryptoUtilities.StringToBytesWithoutNull(plainText);

            if (provider != null)
            {
                var outArray = provider.Encrypt(bytesPlainText, UseOaep);

                //we might want a string representation of our cypher text... base64 will do
                return ABCryptoUtilities.BytesToString(outArray);
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
                byte[] outArray = provider.Decrypt(cipherText, UseOaep);
                return outArray;
            }
            return cipherText;
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                AccelByteDebug.LogWarning("cipher text is null or empty.");
                return string.Empty;
            }

            // first, get our bytes back from the base64 string ...
            var bytesCypherText = ABCryptoUtilities.StringToBytes(cipherText);

            if (provider != null)
            {
                // decrypt and strip pkcs#1.5 padding
                var outArray = provider.Decrypt(bytesCypherText, UseOaep);

                // get our original plainText back...
                return ABCryptoUtilities.BytesToStringWithoutNull(outArray);
            }
            return cipherText;
        }

        public void Clear()
        {
            provider?.Clear();
            provider = null;
        }

        public int GetKeySize()
        {
            if (provider is null)
            {
                return 0;
            }
            return provider.KeySize;
        }

        public int GetBlockSize()
        {
            if (provider is null)
            {
                return 0;
            }
            return provider.KeySize / 8;
        }

        public bool IsActive()
        {
            return (provider != null);
        }
    }
}
