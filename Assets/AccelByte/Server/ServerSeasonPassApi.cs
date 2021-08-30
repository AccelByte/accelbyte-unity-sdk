// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerSeasonPassApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        internal ServerSeasonPassApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, nameof(baseUrl) + "is null");
            Assert.IsNotNull(httpClient, nameof(httpClient) + "is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator GrantExpToUser(string @namespace, string accessToken, string userId, int exp,
            ResultCallback<UserSeasonInfoWithoutReward> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Grant Exp! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Grant Exp! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't Grant Exp! UserId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/seasonpass/admin/namespaces/{namespace}/users/{userId}/seasons/current/exp")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{ \"exp\": {0} }}", exp))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserSeasonInfoWithoutReward>();
            callback.Try(result);
        }
    }
}
