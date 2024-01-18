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
                if (string.IsNullOrEmpty(authTicketIdentity))
                {
                    GetInfoware().GetAccelByteSteamToken(result =>
                    {
                        callback?.Invoke(result.AccelByteSteamToken);
                    });
                }
                else
                {
                    GetInfoware().GetAccelByteSteamToken(authTicketIdentity, result =>
                    {
                        callback?.Invoke(result.AccelByteSteamToken);
                    });
                }
            }
            catch (Exception e)
            {
                string messageBuilt = Utils.PlatformHandlerUtils.SerializeErrorMessage(((int)ErrorCode.GenerateAuthCodeFailed).ToString(), e.Message);
                callback?.Invoke(messageBuilt);
            }
        }

        protected virtual ThirdParties.Steam.ISteamWrapper GetInfoware()
        {
            return ThirdParties.Steam.AccelByteSteam.SteamWrapperGetter();
        }
    }
}