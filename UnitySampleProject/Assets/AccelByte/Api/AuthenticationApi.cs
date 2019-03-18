// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Net;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class AuthenticationApi
    {
        private readonly string baseUrl;

        internal AuthenticationApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Cannot construct Oauth2! baseUrl parameter is null!");
            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> GetClientToken(string clientId, string clientSecret,
            ResultCallback<TokenData> callback)
        {
            Assert.IsNotNull(clientId, "Can't generate client token; ClientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't generate client token; ClientSecret parameter is null!");

            HttpWebRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/oauth/token")
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJsonBody<TokenData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserToken(string @namespace, string clientId, string clientSecret, string username,
            string password, ResultCallback<TokenData> callback)
        {
            Assert.IsNotNull(clientId, "Can't generate token! ClientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't generate token! ClientSecret parameter is null!");
            Assert.IsNotNull(username, "Can't generate token! UserName parameter is null!");
            Assert.IsNotNull(password, "Can't generate token! Password parameter is null!");

            HttpWebRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/oauth/token")
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "password")
                .WithFormParam("username", username)
                .WithFormParam("password", password)
                .WithFormParam("namespace", @namespace)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJsonBody<TokenData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserTokenWithDeviceId(string @namespace, string clientId, string clientSecret,
            string deviceType, string deviceId, ResultCallback<TokenData> callback)
        {
            Assert.IsNotNull(clientId, "Can't generate device token! ClientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't generate device token! ClientSecret parameter is null!");
            Assert.IsNotNull(deviceType, "Can't generate device token! DeviceType parameter is null!");
            Assert.IsNotNull(deviceId, "Can't generate device token! DeviceId parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/oauth/platforms/{platformId}/token")
                .WithPathParam("platformId", deviceType)
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("device_id", deviceId)
                .WithFormParam("namespace", @namespace)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJsonBody<TokenData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserTokenWithOtherPlatform(string @namespace, string clientId, string clientSecret,
            PlatformType platformType, string platformToken, ResultCallback<TokenData> callback)
        {
            Assert.IsNotNull(@namespace, "Can't generate platform token! Namespace parameter is null!");
            Assert.IsNotNull(clientId, "Can't generate platform token! ClientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't generate platform token! ClientSecret parameter is null!");
            Assert.IsNotNull(platformToken, "Can't generate platform token! PlatformToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/iam/oauth/platforms/{platformId}/token")
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("platform_token", platformToken)
                .WithFormParam("namespace", @namespace)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJsonBody<TokenData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserTokenWithAuthorizationCode(string clientId, string clientSecret, string code,
            string redirectUri, ResultCallback<TokenData> callback)
        {
            Assert.IsNotNull(clientId, "Can't generate token from authorization code! clientId parameter is null!");
            Assert.IsNotNull(
                clientSecret,
                "Can't generate token from authorization code! ClientSecret parameter is null!");
            Assert.IsNotNull(code, "Can't generate token from authorization code! Code parameter is null!");
            Assert.IsNotNull(
                redirectUri,
                "Can't generate token from authorization code! RedirectUri parameter is null!");

            HttpWebRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/oauth/token")
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "authorization_code")
                .WithFormParam("code", code)
                .WithFormParam("redirect_uri", redirectUri)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJsonBody<TokenData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserTokenWithRefreshToken(string clientId, string clientSecret,
            string refreshToken, ResultCallback<TokenData> callback)
        {
            Assert.IsNotNull(clientId, "Can't refresh token! clientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't refresh token! ClientSecret parameter is null!");
            Assert.IsNotNull(refreshToken, "Can't refresh token! RefreshToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/oauth/token")
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "refresh_token")
                .WithFormParam("refresh_token", refreshToken)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<TokenData> result = response.TryParseJsonBody<TokenData>();
            callback.Try(result);
        }

        public IEnumerator<ITask> RevokeToken(string clientId, string clientSecret, string token,
            ResultCallback callback)
        {
            Assert.IsNotNull(clientId, "Can't revoke token! clientId parameter is null!");
            Assert.IsNotNull(clientSecret, "Can't revoke token! ClientSecret parameter is null!");
            Assert.IsNotNull(token, "Can't revoke token! Token parameter is null!");

            HttpWebRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/oauth/revoke/token")
                .WithBasicAuth(clientId, clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("token", token)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result result = response.TryParse();
            callback.Try(result);
        }
    }
}