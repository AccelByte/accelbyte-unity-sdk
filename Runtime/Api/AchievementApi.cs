// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class AchievementApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==AchievementServerUrl</param>
        /// <param name="session"></param>
        internal AchievementApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.AchievementServerUrl, session )
        {
        }

        #region Methods 

        private string ConvertAchievementSortByToString( AchievementSortBy sortBy )
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

        public IEnumerator QueryAchievements( string language
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicAchievement> callback
            , int offset
            , int limit )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't query achievements! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't query achievements! AuthToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/achievements")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("language", language)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedPublicAchievement>();
            callback.Try(result);
        }

        public IEnumerator GetAchievement( string achievementCode
            , ResultCallback<MultiLanguageAchievement> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get achievement! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get achievement! AuthToken parameter is null!");
            Assert.IsNotNull(achievementCode, "Can't get achievement! AchievementCode parameter is null!");

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/achievements/{achievementCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<MultiLanguageAchievement>();
            callback.Try(result);
        }

        public IEnumerator QueryUserAchievements( string userId
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedUserAchievement> callback
            , int offset
            , int limit
            , bool PreferUnlocked )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't query user achievements! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't query user achievements! AuthToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("preferUnlocked", PreferUnlocked.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedUserAchievement>();
            callback.Try(result);
        }

        public IEnumerator UnlockAchievement( string userId
            , string AuthToken
            , string achievementCode
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't unlock achievement! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't unlock achievement! AuthToken parameter is null!");
            Assert.IsNotNull(userId, "Can't unlock achievement! UserId parameter is null!");
            Assert.IsNotNull(achievementCode, "Can't unlock achievement! AchievementCode parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements/{achievementCode}/unlock")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        #endregion
    }
}
