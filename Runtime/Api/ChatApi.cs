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
        [UnityEngine.Scripting.Preserve]
        internal ChatApi(IHttpClient inHttpClient
            , Config config
            , ISession session)
            : this(inHttpClient, config, session, null)
        {
        }

        [UnityEngine.Scripting.Preserve]
        internal ChatApi(IHttpClient httpClient
            , Config config
            , ISession session
            , HttpOperator httpOperator)
            : base(httpClient, config, config.BaseUrl, session, httpOperator)
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