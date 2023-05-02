﻿// Copyright (c) 2019-2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using System.Linq;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class StatisticApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==StatisticServerUrl</param>
        /// <param name="session"></param>
        internal StatisticApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.StatisticServerUrl, session )
        {
        }

        /// <summary>
        /// Convert Statistic Sort By Enum to String Value
        /// </summary>
        private string ConvertStatisticSortByToString(StatisticSortBy sortBy)
        {
            switch (sortBy)
            {
                case StatisticSortBy.StatCode:
                    return "StatCode";
                case StatisticSortBy.StatCodeAsc:
                    return "StatCode:asc";
                case StatisticSortBy.StatCodeDesc:
                    return "StatCode:desc";
                case StatisticSortBy.CreatedAt:
                    return "createdAt";
                case StatisticSortBy.CreatedAtAsc:
                    return "createdAt:asc";
                case StatisticSortBy.CreatedAtDesc:
                    return "createdAt:desc";
                case StatisticSortBy.UpdatedAt:
                    return "updatedAt";
                case StatisticSortBy.UpdatedAtAsc:
                    return "updatedAt:asc";
                case StatisticSortBy.UpdatedAtDesc:
                    return "updatedAt:desc";
            }
            return "";
        }

        public IEnumerator CreateUserStatItems( string userId
            , string accessToken
            , CreateStatItemRequest[] statItems
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(statItems, nameof(statItems) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(statItems.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator GetUserStatItems( string userId
            , string accessToken
            , ICollection<string> statCodes
            , ICollection<string> tags
            , ResultCallback<PagedStatItems> callback
            , int offset
            , int limit
            , StatisticSortBy sortBy)
        {
            Assert.IsNotNull(Namespace_, "Can't get stat items! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get stat items! userIds parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get stat items! accessToken parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(
                    BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (statCodes != null && statCodes.Count > 0)
            {
                builder.WithQueryParam("statCodes", string.Join(",", statCodes));
            }

            if (tags != null && tags.Count > 0)
            {
                builder.WithQueryParam("tags", string.Join(",", tags));
            }

            if (sortBy != StatisticSortBy.None)
            {
                builder.WithQueryParam("sortBy", ConvertStatisticSortByToString(sortBy));
            }

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PagedStatItems>();

            callback.Try(result);
        }

        public IEnumerator IncrementUserStatItems( string userId
            , StatItemIncrement[] increments
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Assert.IsNotNull(Namespace_, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(increments.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator ResetUserStatItems( string userId
            , StatItemReset[] resets
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback ) 
        {
            Assert.IsNotNull(Namespace_, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(resets.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator UpdateUserStatItems( string userId
            , string additionalKey
            , StatItemUpdate[] updates
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback ) 
        {
            Assert.IsNotNull(Namespace_, "Can't add stat item value! namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't add stat item value! userId parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updates.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemOperationResult[]>();

            callback.Try(result);
        }

        public IEnumerator ListUserStatItems(string userId
            , string[] statCodes
            , string[] tags
            , string additionalKey
            , string accessToken
            , ResultCallback<FetchUser[]> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(statCodes, nameof(statCodes) + " cannot be null");
            Assert.IsNotNull(tags, nameof(tags) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);
            foreach (var statCode in statCodes)
            {
                builder.WithQueryParam("statCodes", statCode);
            }
            foreach (var tag in tags)
            {
                builder.WithQueryParam("tags", tag);
            }
            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<FetchUser[]>();

            callback.Try(result);
        } 

        public IEnumerator UpdateUserStatItemsValue(string userId
            , string statCode 
            , string additionalKey
            , PublicUpdateUserStatItem updateUserStatItem
            , string accessToken
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(statCode, nameof(statCode) + " cannot be null");
            Assert.IsNotNull(additionalKey, nameof(additionalKey) + " cannot be null");
            Assert.IsNotNull(updateUserStatItem, nameof(updateUserStatItem) + " cannot be null");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("statCode", statCode)
                .WithQueryParam("additionalKey", additionalKey)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserStatItem.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UpdateUserStatItemValueResponse>();

            callback.Try(result);
        }

        public IEnumerator BulkFetchStatItemsValue(string statCode
           , string[] userIds
           , ResultCallback<FetchUserStatistic> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(statCode, nameof(statCode) + " cannot be null");
            Assert.IsNotNull(userIds, nameof(userIds) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");

            string[] processedUserIds = new string[0];
            string[] notProcessedUserIds = new string[0];
            if (userIds.Length > MaximumUserIds.UserIdsLimit)
            {
                for (int i = 0; i < userIds.Length; i++)
                {
                    if (i <= MaximumUserIds.UserIdsLimit)
                    {
                        processedUserIds.Append(userIds[i]);
                    }
                    else
                    {
                        notProcessedUserIds.Append(userIds[i]);
                    }
                }
            }
            else
            {
                processedUserIds = userIds;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/statitems/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithQueryParam("statCode", statCode)
                .WithQueryParam("userIds", string.Join(",", userIds))
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<StatItemValue[]>();

            FetchUserStatistic finalResult = new FetchUserStatistic
            {
                UserStatistic = result.Value,
                NotProcessedUserIds = notProcessedUserIds
            };

            if (result.IsError)
            {
                callback.Try(Result<FetchUserStatistic>.CreateError(result.Error.Code, result.Error.Message));
            }
            else
            {
                callback.Try(Result<FetchUserStatistic>.CreateOk(finalResult));
            }
        }

        public IEnumerator GetGlobalStatItemsByStatCode(string statCode,string accessToken, ResultCallback<GlobalStatItem> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(statCode), nameof(statCode) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/globalstatitems/{statCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("statCode", statCode)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GlobalStatItem>();

            callback.Try(result);
        }
        
        public IEnumerator GetStatCycleConfig(string cycleId
            , string accessToken
            , ResultCallback<StatCycleConfig> callback)
        {
            Assert.IsNotNull(cycleId, "Can't get statistic cycle config! CycleId is null or empty");
            Assert.IsNotNull(accessToken, "Can't get statistic cycle config! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/statCycles/{cycleId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("cycleId", cycleId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<StatCycleConfig>();

            callback.Try(result);
        }

        public IEnumerator GetListStatCycleConfigs(
            StatisticCycleType type,
            StatisticCycleStatus status,
            string accessToken,
            ResultCallback<PagedStatCycleConfigs> callback,
            int offset,
            int limit )
        {
            Assert.IsNotNull(accessToken, "Can't get list cycle config! accessToken parameter is null!");

            HttpRequestBuilder builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/statCycles")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (type != StatisticCycleType.None)
            {
                builder.WithQueryParam("cycleType", type.ToString().ToUpper());
            }

            if (status != StatisticCycleStatus.None)
            {
                builder.WithQueryParam("status", status.ToString().ToUpper());
            }

            IHttpRequest request = builder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<PagedStatCycleConfigs>();

            callback.Try(result);
        }

        public IEnumerator GetListUserStatCycleItem(string cycleId,
            string[] statCodes,
            string userId, 
            string accessToken, 
            ResultCallback<PagedStatCycleItem> callback, 
            int offset, 
            int limit)
        {
            Assert.IsNotNull(cycleId, "Can't add stat item value! cycle id parameter is null!");
            Assert.IsNotNull(accessToken, "Can't add stat item value! accessToken parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statCycles/{cycleId}/statCycleitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("cycleId", cycleId)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (statCodes.Length > 0)
            {
                builder.WithQueryParam("statCodes", string.Join(",", statCodes));
            }
            
            IHttpRequest request = builder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<PagedStatCycleItem>();

            callback.Try(result);
        }
    }
}
