// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;

namespace AccelByte.Core
{
    public class SteamWrapper : IPlatformWrapper
    {
        const string platformId = "steam";
        private string authTicketIdentity;

        public string PlatformId
        {
            get
            {
                return platformId;
            }
        }

        public SteamWrapper() 
        {
        }

        public SteamWrapper(string authTicketIdentity)
        {
            this.authTicketIdentity = authTicketIdentity;
        }

        public void SetAuthTicketIdentity(string authTicketIdentity)
        {
            this.authTicketIdentity = authTicketIdentity;
        }

        public string GetAuthTicketIdentity()
        {
            return authTicketIdentity;
        }

        public void FetchPlatformToken(Action<string> callback)
        {
            try
            {
                Models.AccelByteResult<ThirdParties.Steam.GetSteamTokenResult, Error> result = null; 
                if (string.IsNullOrEmpty(authTicketIdentity))
                {
                    result = ThirdParties.Steam.AccelByteSteam.GetAccelByteSteamToken();
                }
                else
                {
                    result = ThirdParties.Steam.AccelByteSteam.GetAccelByteSteamToken(authTicketIdentity);
                }

                result
                .OnSuccess(getResult =>
                {
                    callback?.Invoke(getResult.AccelByteSteamToken);
                })
                .OnFailed(error =>
                {
                    AccelByteDebug.LogWarning(error.Message);
                    callback?.Invoke(null);
                });
            }
            catch (Exception e)
            {
                string messageBuilt = Utils.PlatformHandlerUtils.SerializeErrorMessage(((int)ErrorCode.GenerateAuthCodeFailed).ToString(), e.Message);
                callback?.Invoke(messageBuilt);
            }
        }

        protected virtual ThirdParties.Steam.ISteamWrapper GetImplementation()
        {
            return ThirdParties.Steam.AccelByteSteam.SteamImpGetter();
        }
    }
}