// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
    internal class AccelByteFileStreamFactory : IFileStreamFactory
    {
        public IFileStream CreateFileStream()
        {
            AccelByteDebug.LogVerbose("Create AccelByte Default FileStream");
#if UNITY_SWITCH && !UNITY_EDITOR
            var retval = new NullFileStream();
#elif UNITY_WEBGL && !UNITY_EDITOR
            var retval = new PlayerPrefsFileStream();
#else
            var retval = new AccelByteFileStream();
#endif
            return retval;
        }
    }
}
