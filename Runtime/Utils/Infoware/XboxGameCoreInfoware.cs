// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using System;

namespace AccelByte.Utils.Infoware
{
    public class XboxGameCoreInfoware : InfowareUtils
    {
        internal override string GetDeviceID()
        {
            logger?.LogWarning("Incomplete implementation for Xbox Game Core platform");
            return string.Empty;
        }

        internal override string GetMacAddress()
        {
            return string.Empty;
        }

        public virtual void GetAccelByteXboxToken(string baseUrl, Action<GetXstsTokenResult> onGetPlatformTokenFinished)
        {
            GetXstsTokenResult result;
#if !UNITY_EDITOR && UNITY_GAMECORE
            result = new GetXstsTokenResult(new Error(ErrorCode.ErrorFromException, "AccelByte Game Core Extension is not installed. Please contact AccelByte support."));
#else
            result = new GetXstsTokenResult(new Error(ErrorCode.ErrorFromException, "Xbox xsts generator only available in xbox game package."));
#endif
            onGetPlatformTokenFinished?.Invoke(result);
        }
    }

    public class GetXstsTokenResult
    {
        public string XstsToken;
        public Core.Error Error;

        public GetXstsTokenResult(string xstsToken)
        {
            XstsToken = xstsToken;
            Error = null;
        }

        public GetXstsTokenResult(Error error)
        {
            XstsToken = null;
            Error = error;
        }

        public GetXstsTokenResult(string xstsToken, Error error)
        {
            XstsToken = xstsToken;
            Error = error;
        }
    }
}