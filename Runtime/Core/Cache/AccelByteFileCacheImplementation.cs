// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
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
            var retval = System.IO.File.Exists(GetFileFullPath(key));
            return retval;
        }

        public virtual bool Emplace(string key, string item)
        {
            try
            {
                if (!System.IO.Directory.Exists(cacheDirectory))
                {
                    System.IO.Directory.CreateDirectory(cacheDirectory);
                }
                System.IO.File.WriteAllBytes(GetFileFullPath(key), Encoding.ASCII.GetBytes(item));
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
            if (!System.IO.Directory.Exists(cacheDirectory))
            {
                System.IO.Directory.CreateDirectory(cacheDirectory);
            }

            string path = GetFileFullPath(key);
            bool writeSuccess = false;
            while (!writeSuccess)
            {
                try
                {
                    using (var outputFile = new System.IO.StreamWriter(path))
                    {
                        await outputFile.WriteAsync(item);
                        writeSuccess = true;
                    }
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
                if (System.IO.Directory.Exists(cacheDirectory))
                {
                    System.IO.Directory.Delete(cacheDirectory, true);
                }
            }
            catch (System.Exception exception)
            {
                AccelByteDebug.LogWarning($"Failed to delete cache directory: {cacheDirectory}.\n{exception.Message}");
            }
        }

        public virtual string Retrieve(string key)
        {
            if (!Contains(key))
            {
                return null;
            }

            string retval = null;

            try
            {
                retval = System.IO.File.ReadAllText(GetFileFullPath(key));
            }
            catch (System.Exception ex)
            {
                AccelByteDebug.LogWarning(ex.Message);
            }
            return retval;
        }

        public virtual async void RetrieveAsync(string key, System.Action<string> callback)
        {
            if (callback != null)
            {
                if (!Contains(key))
                {
                    callback?.Invoke(null);
                    return;
                }

                string fileText = null;
                bool loadSuccess = false;
                string path = GetFileFullPath(key);
                while (!loadSuccess)
                {
                    try
                    {
                        using (var reader = System.IO.File.OpenText(path))
                        {
                            fileText = await reader.ReadToEndAsync();
                            loadSuccess = true;
                        }
                    }
                    catch (System.Exception)
                    {
                        await System.Threading.Tasks.Task.Delay(readWriteAsyncWaitMs);
                    }
                }
                callback?.Invoke(fileText);
            }
        }

        public string Peek(string key)
        {
            return Retrieve(key);
        }

        public bool Remove(string key)
        {
            if (!Contains(key))
            {
                return false;
            }
            System.IO.File.Delete(GetFileFullPath(key));
            return true;
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