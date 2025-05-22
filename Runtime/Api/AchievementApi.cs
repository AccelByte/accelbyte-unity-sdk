// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
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

        public IEnumerator QueryAchievements(string language
            , string tags
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicAchievement> callback
            , int offset
            , int limit
            , bool isGlobal)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new QueryAchievementsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                IsGlobal = isGlobal,
                Limit = limit,
                Offset = offset,
                Tags = tags
            };

            QueryAchievements(language, sortBy, optionalParameters, callback);

            yield return null;
        }

        internal void QueryAchievements(string language
            , AchievementSortBy sortBy
            , QueryAchievementsOptionalParameters optionalParameters
            , ResultCallback<PaginatedPublicAchievement> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var tags = optionalParameters?.TagBuilder is null ? optionalParameters?.Tags : optionalParameters.TagBuilder.Build();

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/achievements";
            var requestBuilder = HttpRequestBuilder
                .CreateGet(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("language", language)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset", optionalParameters.Offset == null ? "0" : optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit == null ? "20" : optionalParameters.Limit.ToString())
                    .WithQueryParam("global", optionalParameters.IsGlobal == null ? "false" : optionalParameters.IsGlobal.ToString());

                if (!string.IsNullOrEmpty(tags))
                {
                    requestBuilder.WithQueryParam("tags", tags);
                }
            }

            var request = requestBuilder.GetResult();
            
            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<PaginatedPublicAchievement>();
                    callback?.Try(result);
                });
        }

        public IEnumerator GetAchievement(string achievementCode
            , ResultCallback<MultiLanguageAchievement> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetAchievementOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetAchievement(achievementCode, optionalParameters, callback);

            yield return null;
        }

        internal void GetAchievement(string achievementCode
            , GetAchievementOptionalParameters optionalParameters
            , ResultCallback<MultiLanguageAchievement> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/achievements/{achievementCode}";
            var request = HttpRequestBuilder.CreateGet(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<MultiLanguageAchievement>();
                    callback?.Try(result);
                });
        }

        public IEnumerator QueryUserAchievements(string userId
            , string tags
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedUserAchievement> callback
            , int offset
            , int limit
            , bool preferUnlocked)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new QueryUserAchievementsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Tags = tags,
                Limit = limit,
                Offset = offset,
                PreferUnlocked = preferUnlocked
            };

            QueryUserAchievements(userId, sortBy, optionalParameters, callback);

            yield return null;
        }

        internal void QueryUserAchievements(string userId
            , AchievementSortBy sortBy
            , QueryUserAchievementsOptionalParameters optionalParameters
            , ResultCallback<PaginatedUserAchievement> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var tags = optionalParameters?.TagBuilder is null ? optionalParameters?.Tags : optionalParameters.TagBuilder.Build();

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements";
            var requestBuilder = HttpRequestBuilder
                .CreateGet(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset", optionalParameters.Offset == null ? "0" : optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit == null ? "20" : optionalParameters.Limit.ToString())
                    .WithQueryParam("preferUnlocked", optionalParameters.PreferUnlocked == null ? "true" : optionalParameters.PreferUnlocked.ToString());

                if (!string.IsNullOrEmpty(tags))
                {
                    requestBuilder.WithQueryParam("tags", tags);
                }
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<PaginatedUserAchievement>();
                    callback?.Try(result);
                });
        }

        public IEnumerator UnlockAchievement(string userId
            , string AuthToken
            , string achievementCode
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UnlockAchievementOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UnlockAchievement(userId, AuthToken, achievementCode, optionalParameters, callback);

            yield return null;
        }

        internal void UnlockAchievement(string userId
            , string AuthToken
            , string achievementCode
            , UnlockAchievementOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements/{achievementCode}/unlock";
            var request = HttpRequestBuilder
                .CreatePut(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParse();
                    callback?.Try(result);
                });
        }

        public void BulkUnlockAchievement(string[] achievementCodes, ResultCallback<BulkUnlockAchievementResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkUnlockAchievementOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkUnlockAchievement(achievementCodes, optionalParameters, callback);
        }

        internal void BulkUnlockAchievement(string[] achievementCodes
            , BulkUnlockAchievementOptionalParameters optionalParameters
            , ResultCallback<BulkUnlockAchievementResponse[]> callback)
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

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/achievements/bulkUnlock";
            var request = HttpRequestBuilder
                .CreatePut(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", Session.UserId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
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

            var optionalParameters = new QueryGlobalAchievementsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Tags = tags,
                Offset = offset,
                Limit = limit
            };

            QueryGlobalAchievements(achievementCode, achievementStatus, sortBy, optionalParameters, callback);

            yield return null;
        }

        internal void QueryGlobalAchievements(string achievementCode
            , GlobalAchievementStatus achievementStatus
            , GlobalAchievementListSortBy sortBy
            , QueryGlobalAchievementsOptionalParameters optionalParameters
            , ResultCallback<PaginatedUserGlobalAchievement> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var tags = optionalParameters?.TagBuilder is null ? optionalParameters?.Tags : optionalParameters.TagBuilder.Build();

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/global/achievements";
            var requestBuilder = HttpRequestBuilder
                .CreateGet(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("achievementCode", achievementCode)
                .WithQueryParam("status", ConvertGlobalAchievementTypeToString(achievementStatus))
                .WithQueryParam("sortBy", ConvertGlobalAchievementSortByToString(sortBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset", optionalParameters.Offset == null ? "0" : optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit == null ? "20" : optionalParameters.Limit.ToString());

                if (!string.IsNullOrEmpty(tags))
                {
                    requestBuilder.WithQueryParam("tags", tags);
                }
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<PaginatedUserGlobalAchievement>();
                    callback?.Try(result);
                });
        }

        public IEnumerator QueryGlobalAchievementContributors(string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , ResultCallback<PaginatedGlobalAchievementContributors> callback
            , int offset
            , int limit
            )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new QueryGlobalAchievementContributorsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Offset = offset,
                Limit = limit
            };

            QueryGlobalAchievementContributors(achievementCode, sortBy, optionalParameters, callback);

            yield return null;
        }

        internal void QueryGlobalAchievementContributors(string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , QueryGlobalAchievementContributorsOptionalParameters optionalParameters
            , ResultCallback<PaginatedGlobalAchievementContributors> callback
            )
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/global/achievements/{achievementCode}/contributors";
            var requestBuilder = HttpRequestBuilder
                .CreateGet(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("achievementCode", achievementCode)
                .WithQueryParam("sortBy", ConvertGlobalAchievementContributorsSortByToString(sortBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset", optionalParameters.Offset == null ? "0" : optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit == null ? "20" : optionalParameters.Limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<PaginatedGlobalAchievementContributors>();
                    callback?.Try(result);
                });
        }

        public IEnumerator QueryGlobalAchievementUserContributed(string userId
            , string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , ResultCallback<PaginatedGlobalAchievementUserContributed> callback
            , int offset
            , int limit
            )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new QueryGlobalAchievementUserContributedOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Offset = offset,
                Limit = limit
            };

            QueryGlobalAchievementUserContributed(userId, achievementCode, sortBy, optionalParameters, callback);

            yield return null;
        }

        internal void QueryGlobalAchievementUserContributed(string userId
            , string achievementCode
            , GlobalAchievementContributorsSortBy sortBy
            , QueryGlobalAchievementUserContributedOptionalParameters optionalParameters
            , ResultCallback<PaginatedGlobalAchievementUserContributed> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/global/achievements";
            var requestBuilder = HttpRequestBuilder
                .CreateGet(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("achievementCode", achievementCode)
                .WithQueryParam("sortBy", ConvertGlobalAchievementContributorsSortByToString(sortBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset", optionalParameters.Offset == null ? "0" : optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit == null ? "20" : optionalParameters.Limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<PaginatedGlobalAchievementUserContributed>();
                    callback?.Try(result);
                });
        }

        public IEnumerator ClaimGlobalAchievement(string userId
            , string AuthToken
            , string achievementCode
            , ResultCallback callback
            )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ClaimGlobalAchievementOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
            };

            ClaimGlobalAchievement(userId, AuthToken, achievementCode, optionalParameters, callback);

            yield return null;
        }

        internal void ClaimGlobalAchievement(string userId
            , string AuthToken
            , string achievementCode
            , ClaimGlobalAchievementOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, achievementCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/global/achievements/{achievementCode}/claim";
            var request = HttpRequestBuilder
                .CreatePost(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("achievementCode", achievementCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParse();
                    callback?.Try(result);
                });
        }

        public IEnumerator GetTags(string name
            , AchievementSortBy sortBy
            , ResultCallback<PaginatedPublicTag> callback
            , int offset
            , int limit
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetTagsAchievementOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset
            };

            GetTags(name, sortBy, optionalParameters, callback);

            yield return null;
        }

        internal void GetTags(string name
            , AchievementSortBy sortBy
            , GetTagsAchievementOptionalParameters optionalParameters
            , ResultCallback<PaginatedPublicTag> callback
        )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string requestUrlFormat = BaseUrl + "/v1/public/namespaces/{namespace}/tags";
            var requestBuilder = HttpRequestBuilder
                .CreateGet(requestUrlFormat)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("name", name)
                .WithQueryParam("sortBy", ConvertAchievementSortByToString(sortBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset", optionalParameters.Offset == null ? "0" : optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit == null ? "20" : optionalParameters.Limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<PaginatedPublicTag>();
                    callback?.Try(result);
                });
        }
        #endregion
    }
}
