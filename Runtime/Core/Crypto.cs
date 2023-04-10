// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.Assertions.Must;

namespace AccelByte.Core
{
    #region RSA_CRYPTO
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

        public RSACrypto() {}
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
            if ( provider is null )
            {
                return 0;
            }

            if ( UseOaep )
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
    #endregion RSA_CRYPTO

    #region AES_CRYPTO
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

        public AESCrypto() {}
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
    #endregion AES_CRYPTO
}
