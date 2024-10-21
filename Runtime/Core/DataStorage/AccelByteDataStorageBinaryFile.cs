// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace AccelByte.Core
{
    internal class AccelByteDataStorageBinaryFile : IAccelByteDataStorage
    {
#if UNITY_SWITCH && !UNITY_EDITOR
        internal static readonly string RootPath = "AccelByte/";
#else
        internal static readonly string RootPath = $"{GameCommonInfo.PersistentPath}/AccelByte/{GameCommonInfo.ProductName}";
#endif

        private readonly IFileStream accelByteFileStream;
        private readonly BinaryFormatter formatter;

        public AccelByteDataStorageBinaryFile(IFileStream accelByteFileStream)
        {
            UnityEngine.Assertions.Assert.IsNotNull(accelByteFileStream, "AccelByte File Stream can't null");
            this.accelByteFileStream = accelByteFileStream;

            formatter = new BinaryFormatter();
        }

        public void DeleteItem(string key, Action<bool> onDone, string tableName = "DefaultKeyValueTable")
        {
            Action<bool, string> onReadDone = (isSuccess, resultJson) =>
            {
                if (string.IsNullOrEmpty(resultJson))
                {
                    onDone?.Invoke(false);
                    return;
                }

                try
                {
                    Dictionary<string, object> storage = resultJson.ToObject<Dictionary<string, object>>();
                    bool isRemoveSuccess = storage.Remove(key);
                    if (!isRemoveSuccess)
                    {
                        onDone?.Invoke(false);
                        return;
                    }
                    const bool instantWrite = true;
                    string storageJson = storage.ToJsonString();
                    this.accelByteFileStream.WriteFile(formatter, storageJson, GetPath(tableName), onDone, instantWrite);
                }
                catch (Exception)
                {
                    onDone?.Invoke(false);
                }
            };
            accelByteFileStream.ReadFile(formatter, GetPath(tableName), onReadDone);
        }

        public void GetItem<T>(string key, Action<bool, T> onDone, string tableName = "DefaultKeyValueTable")
        {
            Action<bool, string> onReadDone = (isSuccess, resultJson) =>
            {
                if (string.IsNullOrEmpty(resultJson))
                {
                    onDone?.Invoke(false, default(T));
                    return;
                }

                try
                {
                    Dictionary<string, object> storage = resultJson.ToObject<Dictionary<string, object>>();
                    storage.TryGetValue(key, out object value);
                    if(value == null)
                    {
                        onDone?.Invoke(false, default(T));
                        return;
                    }
                    string valueJson = value.ToJsonString();
                    var trueTypeValue = valueJson.ToObject<T>();
                    onDone?.Invoke(true, trueTypeValue);
                }
                catch(Exception)
                {
                    onDone?.Invoke(false, default(T));
                }
            };
            accelByteFileStream.ReadFile(formatter, GetPath(tableName), onReadDone);
        }

        public void Reset(Action<bool> result, string tableName = "DefaultKeyValueTable")
        {
            this.accelByteFileStream.DeleteFile(GetPath(tableName), result);
        }

        public void SaveItem(string key, object item, Action<bool> onDone, string tableName = "DefaultKeyValueTable")
        {
            List<Tuple<string, object>> keyItems = new List<Tuple<string, object>>();
            keyItems.Add(new Tuple<string, object>(key, item));
            SaveItems(keyItems, onDone, tableName);
        }

        public void SaveItems(List<Tuple<string, object>> keyItems, Action<bool> onDone, string tableName = "DefaultKeyValueTable")
        {
            Action<bool, string> onReadDone = (isSuccess, resultJson) =>
            {
                Dictionary<string, object> storage = null;
                if (string.IsNullOrEmpty(resultJson))
                {
                    storage = new Dictionary<string, object>();
                }
                else
                {
                    try
                    {
                        storage = resultJson.ToObject<Dictionary<string, object>>();
                    }
                    catch(Exception)
                    {
                        storage = new Dictionary<string, object>();
                    }
                }

                foreach (Tuple<string, object> keyItemPair in keyItems)
                {
                    storage[keyItemPair.Item1] = keyItemPair.Item2;
                }
                const bool instantWrite = true;
                string storageJson = storage.ToJsonString();
                accelByteFileStream.WriteFile(formatter, storageJson, GetPath(tableName), onDone, instantWrite);
            };
            accelByteFileStream.ReadFile(formatter, GetPath(tableName), onReadDone);
        }

        private string GetPath(string tableName)
        {
            string retval = $"{RootPath}/{tableName}";
            return retval;
        }
    }
}