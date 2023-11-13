// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
	public class AccelByteGameStandardEventCacheImp : AccelByteGameStandardEventCacheImpTemplate
    {
        private string saveDirectory;
        private string saveFile;
        private bool saveAsync;
        private bool loadAsync;
        internal AccelByteFileCacheImplementation FileCacheImp;
        protected MD5Crypto cryptoImp;

        public AccelByteGameStandardEventCacheImp(string saveDirectory, string saveFile, string encryptionKey)
        {
            this.saveDirectory = saveDirectory;
            this.saveFile = saveFile;
            saveAsync = false;
            loadAsync = true;
            FileCacheImp = GetFileCacheImp(saveDirectory);
            UpdateKey(encryptionKey);
        }

        public void SetSaveAsync(bool async)
        {
            saveAsync = async;
        }

        public void SetLoadAsync(bool async)
        {
            loadAsync = async;
        }

        internal void UpdateKey(string newKey)
        {
            cryptoImp = GetCryptoImp(newKey);
        }

        protected override void EncryptCache(string content, Action<string> callback)
        {
            if (saveAsync)
            {
                cryptoImp.EncryptAsync(content, callback);
            }
            else
            {
                string cipherText = cryptoImp.Encrypt(content);
                callback?.Invoke(cipherText);
            }
        }

        protected override void DecryptCache(string content, Action<string> callback)
        {
            string plainText = cryptoImp.Decrypt(content);
            callback?.Invoke(plainText);
        }

        protected override void CacheTelemetryEvents(string content, string environment, Action<bool> callback)
        {
            string cacheDirectory = saveDirectory;
            string cacheFileName = environment + saveFile;
            if (saveAsync)
            {
                FileCacheImp.EmplaceAsync(cacheFileName, content, callback);
            }
            else
            {
                bool result = FileCacheImp.Emplace(cacheFileName, content);
                callback?.Invoke(result);
            }
        }

        protected override void LoadTelemetryEventsCache(string environment, Action<string> callback)
        {
            string cacheDirectory = saveDirectory;
            string cacheFileName = environment + saveFile;

            if (loadAsync)
            {
                callback += (content) =>
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        FileCacheImp.Remove(cacheFileName);
                    }
                };
                FileCacheImp.RetrieveAsync(cacheFileName, callback);
            }
            else
            {
                string content = FileCacheImp.Retrieve(cacheFileName);
                if (!string.IsNullOrEmpty(content))
                {
                    FileCacheImp.Remove(cacheFileName);
                }
                callback?.Invoke(content);
            }
        }

        internal virtual AccelByteFileCacheImplementation GetFileCacheImp(string cacheDirectory)
        {
            var retval = new AccelByteFileCacheImplementation(cacheDirectory);
            return retval;
        }

        internal virtual MD5Crypto GetCryptoImp(string key)
        {
            var retval = new MD5Crypto(key);
            return retval;
        }
    }
}