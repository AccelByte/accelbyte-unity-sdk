// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
	public class AccelByteGameStandardEventCacheImp : AccelByteGameStandardEventCacheImpTemplate
    {
        private string tableName;
        private bool saveAsync;
        private bool loadAsync;
        protected MD5Crypto cryptoImp;
        internal IAccelByteDataStorage DataStorage; 

        public AccelByteGameStandardEventCacheImp(string saveDirectory, string saveFile, string encryptionKey)
        {
            this.tableName = $"{saveDirectory}/{saveFile}";
            
            DataStorage = new Core.AccelByteDataStorageBinaryFile(AccelByteSDK.Implementation.FileStream);
            
            saveAsync = false;
            loadAsync = true;
            UpdateKey(encryptionKey);
        }
        
        internal AccelByteGameStandardEventCacheImp(string tableName, IAccelByteDataStorage dataStorage, string encryptionKey)
        {
            this.tableName = tableName;            
            DataStorage = dataStorage;
            
            saveAsync = false;
            loadAsync = true;
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

        internal override void DeleteCache(string environment)
        {
            UnityEngine.Assertions.Assert.IsNotNull(DataStorage, "Data storage cache shouldn't null");
            DataStorage.DeleteItem(environment, onDone: null, tableName: tableName);
        }

        protected override void DecryptCache(string content, Action<string> callback)
        {
            string plainText = cryptoImp.Decrypt(content);
            callback?.Invoke(plainText);
        }

        protected override void CacheTelemetryEvents(string content, string environment, Action<bool> callback)
        {
            UnityEngine.Assertions.Assert.IsNotNull(DataStorage, "Data storage cache shouldn't null");
            DataStorage.SaveItem(environment, content, callback, tableName);
        }

        protected override void LoadTelemetryEventsCache(string environment, Action<string> callback)
        {
            UnityEngine.Assertions.Assert.IsNotNull(DataStorage, "Data storage cache shouldn't null");
            DataStorage.GetItem<string>(environment, tableName: tableName, onDone: (success, content) =>
            {
                if (!string.IsNullOrEmpty(content))
                {
                    DataStorage?.DeleteItem(key: environment, onDone: null, tableName);
                }
                callback?.Invoke(content);
            });
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