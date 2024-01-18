// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    public class PS5Wrapper : IPlatformWrapper
    {
        const string platformId = "ps5";
        private string clientId;

        public string PlatformId
        {
            get
            {
                return platformId;
            }
        }

        public PS5Wrapper(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                AccelByteDebug.LogWarning($"clientId is null. Please check {nameof(PS5Wrapper)} constructor value");
            }
            this.clientId = clientId;
        }

        public void SetClientId(string clientId)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                AccelByteDebug.LogWarning($"clientId is null");
            }

            this.clientId = clientId;
        }

        public string GetClientId()
        {
            return clientId;
        }

        public void FetchPlatformToken(Action<string> callback)
        {
            try
            {
                if (string.IsNullOrEmpty(clientId))
                {
                    throw new Exception("client ID is empty");
                }

                GetInfoware().GetAccelBytePS5Token(clientId, result =>
                {
                    callback?.Invoke(result);
                });
            }
            catch (Exception e)
            {
                string messageBuilt = Utils.PlatformHandlerUtils.SerializeErrorMessage(((int)ErrorCode.GenerateAuthCodeFailed).ToString(), e.Message);
                callback?.Invoke(messageBuilt);
            }
        }

        protected virtual Utils.Infoware.PlayStation5Infoware GetInfoware()
        {
            return AccelByte.Core.PS5Main.InfowareGetter();
        }
    }
}