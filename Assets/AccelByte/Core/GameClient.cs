// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using AccelByte.Api;
using AccelByte.Models;

namespace AccelByte.Core
{
    public class GameClient
    {
        public IUserSession Session => this.session;

        public GameClient(Config config, IHttpClient httpClient)
        {
            this.coroutineRunner = new CoroutineRunner();

            this.httpClient = httpClient;
            this.httpClient.SetBaseUri(new Uri(config.BaseUrl));
            this.httpClient.SetCredentials(config.ClientId, config.ClientSecret);

            this.session = new LoginSession(
                config.BaseUrl,
                config.Namespace,
                config.RedirectUri,
                httpClient,
                this.coroutineRunner,
                config.UsePlayerPrefs);

            this.httpApis = new HttpApiContainer(httpClient);
        }

        public T GetHttpApi<T>() where T : HttpApiBase
        {
            return this.httpApis.GetApi<T>();
        }

        public void ConfigureHttpApi<T>(params object[] args) where T : HttpApiBase
        {
            this.httpApis.ConfigureApi<T>(args);
        }

        private readonly CoroutineRunner coroutineRunner;
        private readonly IHttpClient httpClient;
        private readonly HttpApiContainer httpApis;
        private readonly IUserSession session;
    }
}