// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
using System.Text;
#endif

namespace AccelByte.Core
{
    internal class AccelByteFileCacheImplementation : IAccelByteCacheImplementation<string>
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
        readonly string cacheDirectory = string.Empty;
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

        public bool Emplace(string key, string item)
        {
            if (!System.IO.Directory.Exists(cacheDirectory))
            {
                System.IO.Directory.CreateDirectory(cacheDirectory);
            }
            System.IO.File.WriteAllBytes(GetFileFullPath(key), Encoding.ASCII.GetBytes(item));
            return true;
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

        public string Retrieve(string key)
        {
            if (!Contains(key))
            {
                return null;
            }
            string retval = System.IO.File.ReadAllText(GetFileFullPath(key));
            return retval;
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
        public bool Contains(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool Emplace(string key, string item)
        {
            throw new System.NotImplementedException();
        }

        public void Empty()
        {
            throw new System.NotImplementedException();
        }

        public string Peek(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new System.NotImplementedException();
        }

        public string Retrieve(string key)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(string key, string item)
        {
            throw new System.NotImplementedException();
        }
#endif
    }
}