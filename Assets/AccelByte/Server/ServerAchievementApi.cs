// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerAchievementApi
    {
        private string baseUrl;
        private IHttpWorker httpWorker;

        internal ServerAchievementApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator UnlockAchievement(string @namespace, string userId, string accessToken, string achievementCode,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't unlock achievement! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't unlock achievement! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't unlock achievement! UserId parameter is null!");
            Assert.IsNotNull(achievementCode, "Can't unlock achievement! AchievementCode parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/achievements/{achievementCode}/unlock")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
    }
}
