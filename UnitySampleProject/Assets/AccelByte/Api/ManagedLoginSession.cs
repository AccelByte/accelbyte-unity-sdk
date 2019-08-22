// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class ManagedLoginSession : ILoginSession
    {
        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirecUri;
        private readonly IHttpWorker httpWorker;
        private readonly object syncObject = new object();

        private string sessionId;

        internal ManagedLoginSession(string baseUrl, string @namespace, string clientId, string clientSecret, string redirecUri,
            IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(@namespace, "Creating " + GetType().Name + " failed. Namespace parameter is null!");
            Assert.IsNotNull(clientId, "Creating " + GetType().Name + " failed. ClientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Creating " + GetType().Name + " failed. ClientSecret parameter is null!");
            Assert.IsNotNull(redirecUri, "Creating " + GetType().Name + " failed. RedirectUri parameter is null!");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.@namespace = @namespace;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirecUri = redirecUri;
            this.httpWorker = httpWorker;
        }

        public string AuthorizationToken
        {
            get
            {
                lock (this.syncObject)
                {
                    if (string.IsNullOrEmpty(this.sessionId))
                    {
                        throw new InvalidOperationException("Getting access token while account is not logged in.");
                    }

                    return this.sessionId;
                }
            }
            
            private set
            {
                lock (this.syncObject)
                {
                    this.sessionId = value;
                }
            }
        }

        public string UserId { get { return "me"; } }

        public IEnumerator LoginWithUsername(string username, string password, ResultCallback callback)
        {
            Assert.IsNotNull(username, "Username parameter is null.");
            Assert.IsNotNull(password, "Password parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v1/login/password")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            TrySetAccessToken(callback, response);
        }

        public IEnumerator LoginWithDeviceId(ResultCallback callback)
        {
            DeviceProvider deviceProvider = DeviceProvider.GetFromSystemInfo();

            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v1/login/platforms/{platformId}")
                .WithPathParam("platformId", deviceProvider.DeviceType)
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("device_id", deviceProvider.DeviceId)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            TrySetAccessToken(callback, response);
        }

        public IEnumerator LoginWithOtherPlatform(PlatformType platformType, string platformToken,
            ResultCallback callback)
        {
            Assert.IsNotNull(platformToken, "PlatformToken parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v1/login/platforms/{platformId}")
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("platform_token", platformToken)
                .WithFormParam("namespace", this.@namespace)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            TrySetAccessToken(callback, response);
        }

        public IEnumerator LoginWithAuthorizationCode(string code, ResultCallback callback)
        {
            Assert.IsNotNull(code, "Code parameter is null.");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v1/login/code")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "authorization_code")
                .WithFormParam("code", code)
                .WithFormParam("redirect_uri", this.redirecUri)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            TrySetAccessToken(callback, response);
        }

        public IEnumerator Logout(ResultCallback callback)
        {
            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/v1/logout")
                .WithBearerAuth(this.AuthorizationToken)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("token", this.AuthorizationToken)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            this.AuthorizationToken = null;
            var result = response.TryParse();
            callback.Try(result);
        }

        private void TrySetAccessToken(ResultCallback callback, IHttpResponse response)
        {
            var result = response.TryParseJson<SessionData>();

            if (result.IsError)
            {
                callback.TryError(result.Error);

                return;
            }

            this.AuthorizationToken = result.Value.session_id;
            callback.TryOk();
        }

    }
}