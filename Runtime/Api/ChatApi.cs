// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;

namespace AccelByte.Api
{
    public class ChatApi : ApiBase
    {
        internal ChatApi(IHttpClient inHttpClient
            , Config inConfig
            , ISession inSession) : base(inHttpClient, inConfig, inConfig.BaseUrl, inSession)
        {
        }

        public Config GetConfig()
        {
            return Config;
        }

        public void OnBanNotificationReceived(Action<string> onHttpBearerRefreshed)
        {
            Report.GetFunctionLog(GetType().Name);
            ((AccelByteHttpClient)HttpClient).OnBearerAuthRejected(onHttpBearerRefreshed);
        }
    }
}