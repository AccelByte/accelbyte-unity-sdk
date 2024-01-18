// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;

namespace AccelByte.Core
{
    public class XboxWrapper : IPlatformWrapper
    {
        const string platformId = "live";
        private string baseUrl;

        public string PlatformId
        {
            get
            {
                return platformId;
            }
        }

        public XboxWrapper(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                AccelByteDebug.LogWarning($"baseUrl is null. Please check {nameof(XboxWrapper)} constructor value");
            }

            this.baseUrl = baseUrl;
        }

        public void SetBaseUrl(string baseUrl)
        {
            if (string.IsNullOrEmpty(baseUrl))
            {
                AccelByteDebug.LogWarning($"baseUrl is null. Please check {nameof(XboxWrapper)} constructor value");
            }

            this.baseUrl = baseUrl;
        }

        public string GetBaseUrl()
        {
            return this.baseUrl;
        }

        public void FetchPlatformToken(Action<string> callback)
        {
            try
            {
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new Exception("baseUrl is empty");
                }

                GetInfoware().GetAccelByteXboxToken(baseUrl, result =>
                {
                    callback?.Invoke(result.XstsToken);
                });
            }
            catch (Exception e)
            {
                string messageBuilt = Utils.PlatformHandlerUtils.SerializeErrorMessage(((int)ErrorCode.GenerateAuthCodeFailed).ToString(), e.Message);
                callback?.Invoke(messageBuilt);
            }
        }

        protected virtual Utils.Infoware.XboxGameCoreInfoware GetInfoware()
        {
            return AccelByte.Core.GameCoreMain.InfowareGetter();
        }
    }
}