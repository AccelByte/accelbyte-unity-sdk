﻿// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;

namespace AccelByte.Api
{
    public class AchievementApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==AchievementServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal AchievementApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.AchievementServerUrl, session )
        {
        }

        #region Methods 

        /// <summary>
        /// Convert Achievement Sort By Enum to String Value
        /// </summary>
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

        /// <summary>
        /// Convert Global Achievement Sort By Enum to String Value
        /// </summary>
        private string ConvertGlobalAchievementSortByToString( GlobalAchievementListSortBy sortBy )
        {
            switch (sortBy)
            {
                case GlobalAchievementListSortBy.AchievedAt:
                    return "achievedAt";
                case GlobalAchievementListSortBy.AchievedAtAsc:
                    return "achievedAt:asc";
                case GlobalAchievementListSortBy.AchievedAtDesc:
                    return "achievedAt:desc";
                case GlobalAchievementListSortBy.CreatedAt:
                    return "createdAt";
                case GlobalAchievementListSortBy.CreatedAtAsc:
                    return "createdAt:asc";
                case GlobalAchievementListSortBy.CreatedAtDesc:
                    return "createdAt:desc";
            }
            return "";
        }

        /// <summary>
        /// Convert Global Achievement Type Enum to String Value
        /// </summary>
        private string ConvertGlobalAchievementTypeToString( GlobalAchievementStatus type )
        {
            switch (type)
            {
                case GlobalAchievementStatus.InProgress:
                    return "in_progress";
                case GlobalAchievementStatus.Unlocked:
                    return "unlocked";
            }
            return "";
        }

        /// <summary>
        /// Convert Global Achievement Contributors Sort By Enum to String Value
        /// </summary>
        private string ConvertGlobalAchievementContributorsSortByToString(GlobalAchievementContributorsSortBy sortBy)
        {
            switch (sortBy)
            {
                case GlobalAchievementContributorsSortBy.ContributedValue:
                    return "contributedValue";
                case GlobalAchievementContributorsSortBy.ContributedValueAsc:
                    return "contributedValue:asc";
                case GlobalAchievementContributorsSortBy.ContributedValueDesc:
                    return "contributedValue:desc";
            }
            return "";
        }

        public IEnumerator QueryAchievements( string language
            , string tags
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicAchievement> callback
            , int offset
            , int limit
            , bool isGlobal)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/achievements")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("language", language)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("global", isGlobal.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(tags))
            {
                requestBuilder.WithQueryParam("tags", tags);
            }

            var request = requestBuilder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedPublicAchievement>();
            callback?.Try(result);
        }
        public IEnumerator GetAchievement(string achievementCode
            , ResultCallback<MultiLanguageAchievement> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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
            callback?.Try(result);
        }

        public IEnumerator QueryUserAchievements( string userId
            , string tags
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedUserAchievement> callback
            , int offset
            , int limit
            , bool preferUnlocked )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("preferUnlocked", preferUnlocked.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(tags))
            {
                requestBuilder.WithQueryParam("tags", tags);
            }

            var request = requestBuilder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedUserAchievement>();
            callback?.Try(result);
        }

        public IEnumerator UnlockAchievement( string userId
            , string AuthToken
            , string achievementCode
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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
            callback?.Try(result);
        }

        public void BulkUnlockAchievement(string[] achievementCodes, ResultCallback<BulkUnlockAchievementResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, Session.UserId, achievementCodes);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBody = new BulkUnlockAchievementRequest()
            {
                AchievementCodes = achievementCodes
            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements/bulkUnlock")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<BulkUnlockAchievementResponse[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator QueryGlobalAchievements(string achievementCode
            , GlobalAchievementStatus achievementStatus
            , GlobalAchievementListSortBy sortBy
            , ResultCallback<PaginatedUserGlobalAchievement> callback
            , int offset
            , int limit
            , string tags
            ) 
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/global/achievements")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("achievementCode", achievementCode)
                .WithQueryParam("status", ConvertGlobalAchievementTypeToString(achievementStatus))
                .WithQueryParam("sortBy", ConvertGlobalAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(tags))
            {
                requestBuilder.WithQueryParam("tags", tags);
            }

            var request = requestBuilder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedUserGlobalAchievement>();
            callback?.Try(result);
        }

        public IEnumerator QueryGlobalAchievementContributors(string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , ResultCallback<PaginatedGlobalAchievementContributors> callback
            , int offset
            , int limit
            )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/global/achievements/{achievementCode}/contributors")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("achievementCode", achievementCode)
                .WithQueryParam("sortBy", ConvertGlobalAchievementContributorsSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            var request = requestBuilder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGlobalAchievementContributors>();
            callback?.Try(result);
        }

        public IEnumerator QueryGlobalAchievementUserContributed( string userId
            , string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , ResultCallback<PaginatedGlobalAchievementUserContributed> callback
            , int offset
            , int limit
            )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/global/achievements")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("achievementCode", achievementCode)
                .WithQueryParam("sortBy", ConvertGlobalAchievementContributorsSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            var request = requestBuilder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedGlobalAchievementUserContributed>();
            callback?.Try(result);
        }

        public IEnumerator ClaimGlobalAchievement( string userId
            , string AuthToken
            , string achievementCode
            , ResultCallback callback
            ) 
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/global/achievements/{achievementCode}/claim")
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
            callback?.Try(result);
        }

        public IEnumerator GetTags(string name
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicTag> callback
            , int offset
            , int limit
        )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/tags")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("name",name)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);
            
            var result = response.TryParseJson<PaginatedPublicTag>();
            
            callback?.Try(result);
        }
        #endregion
    }
}
