// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class AchievementApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal AchievementApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        string ConvertAchievementSortByToString(AchievementSortBy sortBy)
        {
            switch (sortBy)
            {
                case AchievementSortBy.LISTORDER:
                    return "listOrder";
                case AchievementSortBy.LISTORDER_ASC:
                    return "listOrder:asc";
                case AchievementSortBy.LISTORDER_DESC:
                    return "listOrder:desc";
                case AchievementSortBy.CREATED_AT:
                    return "createdAt";
                case AchievementSortBy.CREATED_AT_ASC:
                    return "createdAt:asc";
                case AchievementSortBy.CREATED_AT_DESC:
                    return "createdAt:desc";
                case AchievementSortBy.UPDATED_AT:
                    return "updatedAt";
                case AchievementSortBy.UPDATED_AT_ASC:
                    return "updatedAt:asc";
                case AchievementSortBy.UPDATED_AT_DESC:
                    return "updatedAt:desc";
            }
            return "";
        }

        public IEnumerator QueryAchievements(string @namespace, string accessToken, string language, AchievementSortBy sortBy,
            ResultCallback<PaginatedPublicAchievement> callback, int offset, int limit)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't query achievements! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't query achievements! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/achievements")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("language", language)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedPublicAchievement>();
            callback.Try(result);
        }

        public IEnumerator GetAchievement(string @namespace, string accessToken, string achievementCode,
            ResultCallback<MultiLanguageAchievement> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get achievement! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get achievement! AccessToken parameter is null!");
            Assert.IsNotNull(achievementCode, "Can't get achievement! AchievementCode parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/achievements/{achievementCode}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<MultiLanguageAchievement>();
            callback.Try(result);
        }

        public IEnumerator QueryUserAchievements(string @namespace, string userId, string accessToken, AchievementSortBy sortBy,
            ResultCallback<PaginatedUserAchievement> callback, int offset, int limit, bool PreferUnlocked)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't query user achievements! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't query user achievements! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("preferUnlocked", PreferUnlocked.ToString())
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PaginatedUserAchievement>();
            callback.Try(result);
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
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements/{achievementCode}/unlock")
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
