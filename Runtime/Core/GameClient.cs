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
        public UserSession Session => this.session;
        public User User => this.user;

        public GameClient(OAuthConfig oAuthConfig, Config config, IHttpClient httpClient)
        {
            this.coroutineRunner = new CoroutineRunner();

            this.httpClient = httpClient;
            this.httpClient.SetBaseUri(new Uri(config.BaseUrl));
            this.httpClient.SetCredentials(oAuthConfig.ClientId, oAuthConfig.ClientSecret);

            this.session = new UserSession(
                httpClient,
                this.coroutineRunner,
                config.PublisherNamespace,
                config.UsePlayerPrefs);
            this.user = new User(
                    new UserApi(
                        this.httpClient,
                        config,
                        this.session),
                    this.session,
                    this.coroutineRunner);
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
        private readonly UserSession session;
        private readonly User user;
    }
}