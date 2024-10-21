// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace AccelByte.Core
{
	public class PlayerPrefsFileStream : IFileStream
    {
        Dictionary<string, string> saveMem;
        private string cacheKey;
        
        public PlayerPrefsFileStream()
        {
            saveMem = new Dictionary<string, string>();
            cacheKey = $"{Application.companyName}/{Application.productName}";

            bool hasKey = PlayerPrefs.HasKey(cacheKey);
            if (!hasKey)
            {
                saveMem = new Dictionary<string, string>();
            }
            else
            {
                string cache = PlayerPrefs.GetString(cacheKey);
                try
                {
                    saveMem = cache.ToObject<Dictionary<string, string>>();
                }
                catch (System.Exception)
                {
                    saveMem = new Dictionary<string, string>();
                }
            }
        }

        public void DeleteDirectory(string directory, Action<bool> onDone)
        {
            if (saveMem.Count == 0)
            {
                onDone?.Invoke(false);
                return;
            }

            bool isDirectoryFound = false;
            List<string> keys = new List<string>();
            foreach(string key in saveMem.Keys)
            {
                keys.Add(key);
            }
            foreach (string key in keys)
            {
                if (key.StartsWith(directory))
                {
                    isDirectoryFound = true;
                    saveMem.Remove(key);
                }
            }

            if (isDirectoryFound)
            {
                PlayerPrefs.SetString(cacheKey, saveMem.ToJsonString());
                PlayerPrefs.Save();
            }

            onDone?.Invoke(isDirectoryFound);
        }

        public void DeleteFile(string path, Action<bool> onDone, bool instantDelete = false)
        {
            if (!IsFileExist(path))
            {
                onDone?.Invoke(false);
                return;
            }

            bool removeSuccess = saveMem.Remove(path);

            if (removeSuccess)
            {
                PlayerPrefs.SetString(cacheKey, saveMem.ToJsonString());
                PlayerPrefs.Save();
            }
            
            onDone?.Invoke(removeSuccess);
        }

        public bool IsFileExist(string path)
        {
            bool keyExist = saveMem.ContainsKey(path);
            return keyExist;
        }

        public bool IsDirectoryExist(string path)
        {
            foreach (string key in saveMem.Keys)
            {
                if (key.StartsWith(path))
                {
                    return true;
                }
            }
            return false;
        }

        public void ReadFile(IFormatter formatter, string path, Action<bool, string> onDone, bool instantRead = false)
        {
            if (!IsFileExist(path))
            {
                onDone?.Invoke(false, null);
                return;
            }

            string output = saveMem[path];
            onDone?.Invoke(true, output);
        }

        public void ReadFileAsync(string path, Action<bool, string> onDone)
        {
            if (!IsFileExist(path))
            {
                onDone?.Invoke(false, null);
                return;
            }

            string output = saveMem[path];
            onDone?.Invoke(true, output);
        }

        public void WriteFile(IFormatter formatter, string content, string path, Action<bool> onDone, bool instantWrite = false)
        {
            saveMem[path] = content;
            PlayerPrefs.SetString(cacheKey, saveMem.ToJsonString());
            PlayerPrefs.Save();
            onDone?.Invoke(true);
        }

        public void WriteFileAsync(string content, string path, Action<bool> onDone)
        {
            saveMem[path] = content;
            PlayerPrefs.SetString(cacheKey, saveMem.ToJsonString());
            PlayerPrefs.Save();
            onDone?.Invoke(true);
        }

        void IFileStream.Dispose()
        {

        }

        void IFileStream.AddOnPop(Action action)
        {

        }

        void IFileStream.Pop()
        {

        }

        void IFileStream.RemoveOnPop(Action action)
        {

        }
    }
}
