// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
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
        internal static readonly string RootPath = $"{Application.persistentDataPath}/AccelByte/{Application.productName}";

        private readonly AccelByteFileStream accelByteFileStream;
        private readonly BinaryFormatter formatter;

        public AccelByteDataStorageBinaryFile(AccelByteFileStream accelByteFileStream)
        {
            UnityEngine.Assertions.Assert.IsNotNull(accelByteFileStream, "AccelByte File Stream can't null");
            this.accelByteFileStream = accelByteFileStream;

            formatter = new BinaryFormatter();
        }

        public void DeleteItem(string key, Action<bool> onDone, string tableName = "DefaultKeyValueTable")
        {
            Action<bool, object> onReadDone = (isSuccess, result) =>
            {
                if(result == null)
                {
                    onDone?.Invoke(false);
                    return;
                }

                if(result is Dictionary<string, object>)
                {
                    var storage = result as Dictionary<string, object>;
                    bool isRemoveSuccess = storage.Remove(key);
                    if(!isRemoveSuccess)
                    {
                        onDone?.Invoke(false);
                        return;
                    }
                    const bool instantWrite = true;
                    this.accelByteFileStream.WriteFile(formatter, storage, GetPath(tableName), onDone, instantWrite);
                }
                else
                {
                    onDone?.Invoke(false);
                }
            };
            accelByteFileStream.ReadFile(formatter, GetPath(tableName), onReadDone);
        }

        public void GetItem<T>(string key, Action<bool, T> onDone, string tableName = "DefaultKeyValueTable")
        {
            Action<bool, object> onReadDone = (isSuccess, result) =>
            {
                if (result == null)
                {
                    onDone?.Invoke(false, default(T));
                    return;
                }

                if (result is Dictionary<string, object>)
                {
                    var storage = result as Dictionary<string, object>;
                    storage.TryGetValue(key, out object value);
                    if(value is T)
                    {
                        onDone?.Invoke(true, (T)value);
                    }
                    else
                    {
                        onDone?.Invoke(false, default(T));
                    }
                }
                else
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
            Action<bool, object> onReadDone = (isSuccess, result) =>
            {
                Dictionary<string, object> storage = null;
                if (result == null)
                {
                    storage = new Dictionary<string, object>();
                }
                else
                {
                    if (result is Dictionary<string, object>)
                    {
                        storage = result as Dictionary<string, object>;
                    }
                    else
                    {
                        storage = new Dictionary<string, object>();
                    }
                }

                foreach (Tuple<string, object> keyItemPair in keyItems)
                {
                    storage[keyItemPair.Item1] = keyItemPair.Item2;
                }
                const bool instantWrite = true;
                accelByteFileStream.WriteFile(formatter, storage, GetPath(tableName), onDone, instantWrite);
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