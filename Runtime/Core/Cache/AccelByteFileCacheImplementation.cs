// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
#if !UNITY_WEBGL
using System.Text;
#endif

namespace AccelByte.Core
{
    internal class AccelByteFileCacheImplementation : IAccelByteCacheImplementation<string>
    {
#if !UNITY_WEBGL
        readonly string cacheDirectory = string.Empty;
        const int readWriteAsyncWaitMs = 100;

        public AccelByteFileCacheImplementation(string cacheDirectory)
        {
            if (string.IsNullOrEmpty(cacheDirectory))
            {
                throw new System.InvalidOperationException("Cache directory is empty.");
            }
            this.cacheDirectory = cacheDirectory;
        }

        public bool Contains(string key)
        {
            string itemPath = GetFileFullPath(key);
            var retval = AccelByteSDK.FileStream.IsFileExist(itemPath);
            return retval;
        }

        public virtual bool Emplace(string key, string item)
        {
            try
            {
                string itemSavePath = GetFileFullPath(key);
                AccelByteSDK.FileStream.WriteFile(formatter: null, item, path: itemSavePath, onDone: null, instantWrite: true);
            }
            catch (System.Exception ex)
            {
                AccelByteDebug.LogWarning(ex.Message);
                return false;
            }
            return true;
        }

        public virtual async void EmplaceAsync(string key, string item, System.Action<bool> callback = null)
        {
            string filePath = GetFileFullPath(key);
            bool writeSuccess = false;
            while (!writeSuccess)
            {
                try
                {
                    AccelByteSDK.FileStream.WriteFileAsync(item, filePath, (success) =>
                    {
                        writeSuccess = success;
                    });
                }
                catch (System.Exception)
                {
                    await System.Threading.Tasks.Task.Delay(readWriteAsyncWaitMs);
                }
            }
            callback?.Invoke(true);
        }

        public bool Update(string key, string item)
        {
            if (!Contains(key))
            {
                return false;
            }
            return Emplace(key, item);
        }

        public void Empty()
        {
            try
            {
                AccelByteSDK.FileStream.DeleteDirectory(cacheDirectory, onDone: null);
            }
            catch (System.Exception exception)
            {
                AccelByteDebug.LogWarning($"Failed to delete cache directory: {cacheDirectory}.\n{exception.Message}");
            }
        }

        public virtual string Retrieve(string key)
        {
            string filePath = GetFileFullPath(key);
            string retval = null;

            try
            {
                AccelByteSDK.FileStream.ReadFile(formatter: null, filePath, instantRead: true, onDone: (isSucess, readResult)=>
                {
                    if(isSucess)
                    {
                        retval = (string) readResult;
                    }
                });
            }
            catch (System.Exception ex)
            {
                AccelByteDebug.LogWarning(ex.Message);
            }
            return retval;
        }

        public virtual void RetrieveAsync(string key, System.Action<string> callback)
        {
            if (callback != null)
            {
                string filePath = GetFileFullPath(key);
                AccelByteSDK.FileStream.ReadFileAsync(filePath, (success, readResult) =>
                {
                    callback?.Invoke(readResult);
                });
            }
        }

        public string Peek(string key)
        {
            return Retrieve(key);
        }

        public bool Remove(string key)
        {
            bool result = false;
            try
            {
                string filePath = GetFileFullPath(key);
                AccelByteSDK.FileStream.DeleteFile(filePath, instantDelete: true, onDone: (isSuccess) =>
                {
                    result = isSuccess;
                });
            }
            catch(System.Exception ex)
            {
                AccelByteDebug.LogWarning($"Failed to delete cache file.\n{ex.Message}");
            }
            return result;
        }

        string GetFileFullPath(string key)
        {
            string retval = $"{cacheDirectory}/{key}";
            return retval;
        }
#else
        public AccelByteFileCacheImplementation(string cacheDirectory)
        {

        }

        public bool Contains(string key)
        {
            return false;
        }

        public virtual bool Emplace(string key, string item)
        {
            return false;
        }

        public virtual void EmplaceAsync(string key, string item, System.Action<bool> callback = null)
        {
            callback?.Invoke(false);
        }

        public void Empty()
        {
            
        }

        public string Peek(string key)
        {
            return null;
        }

        public bool Remove(string key)
        {
            return false;
        }

        public virtual string Retrieve(string key)
        {
            return null;
        }

        public virtual void RetrieveAsync(string key, System.Action<string> callback)
        {
            callback?.Invoke(string.Empty);
        }

        public bool Update(string key, string item)
        {
            return false;
        }
#endif
    }
}