// Copyright (c) 2019-2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AccelByte.Api
{
    public class StatisticApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==StatisticServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateUserStatItems(userId, accessToken, statItems, optionalParameters, callback);

            yield return null;
        }

        internal void CreateUserStatItems(string userId
            , string accessToken
            , CreateStatItemRequest[] statItems
            , CreateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, accessToken, statItems);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(statItems.ToUtf8Json())
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserStatItemsOptionalParam()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                sortBy = sortBy,
                StatCodes = statCodes?.ToArray(),
                Tags = tags?.ToArray()
            };

            GetUserStatItems(userId, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetUserStatItems(string userId
            , string accessToken
            , GetUserStatItemsOptionalParam optionalParameters
            , ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, accessToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(
                    BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                var queryParams = new Dictionary<string, string>();

                if (optionalParameters.Limit >= 1)
                {
                    queryParams.Add("limit", optionalParameters.Limit.ToString());
                }    
                if (optionalParameters.Offset >= 0)
                {
                    queryParams.Add("offset", optionalParameters.Offset.ToString());
                }
                if (optionalParameters.StatCodes != null && optionalParameters.StatCodes.Length > 0)
                {
                    queryParams.Add("statCodes", string.Join(",", optionalParameters.StatCodes));
                }
                if (optionalParameters.Tags != null && optionalParameters.Tags.Length > 0)
                {
                    queryParams.Add("tags", string.Join(",", optionalParameters.Tags));
                }
                if (optionalParameters.sortBy != StatisticSortBy.None)
                {
                    queryParams.Add("sortBy", ConvertStatisticSortByToString(optionalParameters.sortBy));
                }

                builder.WithQueries(queryParams);
            }
            var request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedStatItems>();
                callback?.Try(result);
            });
        }

        public IEnumerator IncrementUserStatItems( string userId
            , StatItemIncrement[] increments
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new IncrementUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            IncrementUserStatItems(userId, increments, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void IncrementUserStatItems(string userId
            , StatItemIncrement[] increments
            , string accessToken
            , IncrementUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, accessToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(increments.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator ResetUserStatItems( string userId
            , StatItemReset[] resets
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback ) 
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ResetUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ResetUserStatItems(userId, resets, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void ResetUserStatItems(string userId
            , StatItemReset[] resets
            , string accessToken
            , ResetUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, accessToken, resets);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(resets.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback.Try(result);
            });
        }

        public IEnumerator UpdateUserStatItems( string userId
            , string additionalKey
            , StatItemUpdate[] updates
            , string accessToken
            , ResultCallback<StatItemOperationResult[]> callback ) 
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                AdditionalKey = additionalKey
            };

            UpdateUserStatItems(userId, updates, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void UpdateUserStatItems(string userId
            , StatItemUpdate[] updates
            , string accessToken
            , UpdateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, accessToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updates.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                var queryParams = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(optionalParameters.AdditionalKey))
                {
                    queryParams.Add("additionalKey", optionalParameters.AdditionalKey);
                }

                builder.WithQueries(queryParams);
            }

            var request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator ListUserStatItems(string userId
            , string[] statCodes
            , string[] tags
            , string additionalKey
            , string accessToken
            , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ListUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                AdditionalKey = additionalKey,
                StatCodes = statCodes,
                Tags = tags
            };

            ListUserStatItems(userId, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void ListUserStatItems(string userId
            , string accessToken
            , ListUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , userId
                , accessToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                if (!string.IsNullOrEmpty(optionalParameters.AdditionalKey))
                {
                    builder.WithQueryParam("additionalKey", optionalParameters.AdditionalKey);
                }

                if (optionalParameters.StatCodes != null && optionalParameters.StatCodes.Length > 0)
                {
                    foreach (var statCode in optionalParameters.StatCodes)
                    {
                        builder.WithQueryParam("statCodes", statCode);
                    }
                }

                if (optionalParameters.Tags != null && optionalParameters.Tags.Length > 0)
                {
                    foreach (var tag in optionalParameters.Tags)
                    {
                        builder.WithQueryParam("tags", tag);
                    }
                }
            }
            
            var request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<FetchUser[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator UpdateUserStatItemsValue(string userId
            , string statCode 
            , string additionalKey
            , PublicUpdateUserStatItem updateUserStatItem
            , string accessToken
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemsValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                AdditionalKey = additionalKey
            };

            UpdateUserStatItemsValue(userId, statCode, updateUserStatItem, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void UpdateUserStatItemsValue(string userId
            , string statCode
            , PublicUpdateUserStatItem updateUserStatItem
            , string accessToken
            , UpdateUserStatItemsValueOptionalParameters optionalParameters
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , userId
                , statCode
                , updateUserStatItem
                , accessToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/public/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("statCode", statCode)
                
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(updateUserStatItem.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                if (!string.IsNullOrEmpty(optionalParameters.AdditionalKey))
                {
                    builder.WithQueryParam("additionalKey", optionalParameters.AdditionalKey);
                }
            }

            var request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UpdateUserStatItemValueResponse>();
                callback?.Try(result);
            });
        }

        public IEnumerator BulkFetchStatItemsValue(string statCode
           , string[] userIds
           , ResultCallback<FetchUserStatistic> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkFetchStatItemsValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkFetchStatItemsValue(statCode, userIds, optionalParameters, callback);

            yield return null;
        }

        internal void BulkFetchStatItemsValue(string statCode
           , string[] userIds
           , BulkFetchStatItemsValueOptionalParameters optionalParameters
           , ResultCallback<FetchUserStatistic> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , statCode
                , userIds
                , AuthToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

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
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemValue[]>();

                FetchUserStatistic finalResult = new FetchUserStatistic
                {
                    UserStatistic = result.Value,
                    NotProcessedUserIds = notProcessedUserIds
                };

                if (result.IsError)
                {
                    callback?.Try(Result<FetchUserStatistic>.CreateError(result.Error.Code, result.Error.Message));
                }
                else
                {
                    callback?.Try(Result<FetchUserStatistic>.CreateOk(finalResult));
                }
            });
        }

        public IEnumerator GetGlobalStatItemsByStatCode(string statCode,string accessToken, ResultCallback<GlobalStatItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetGlobalStatItemsByStatCodeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetGlobalStatItemsByStatCode(statCode, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetGlobalStatItemsByStatCode(string statCode, string accessToken, GetGlobalStatItemsByStatCodeOptionalParameters optionalParameters, ResultCallback<GlobalStatItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , statCode
                , accessToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/globalstatitems/{statCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("statCode", statCode)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GlobalStatItem>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetStatCycleConfig(string cycleId
            , string accessToken
            , ResultCallback<StatCycleConfig> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetStatCycleConfigOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetStatCycleConfig(cycleId, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetStatCycleConfig(string cycleId
            , string accessToken
            , GetStatCycleConfigOptionalParameters optionalParameters
            , ResultCallback<StatCycleConfig> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , cycleId
                , accessToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/statCycles/{cycleId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("cycleId", cycleId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatCycleConfig>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetListStatCycleConfigs(
            StatisticCycleType type,
            StatisticCycleStatus status,
            string accessToken,
            ResultCallback<PagedStatCycleConfigs> callback,
            int offset,
            int limit )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetListStatCycleConfigsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                Status = status,
                Type = type
            };

            GetListStatCycleConfigs(accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetListStatCycleConfigs(
            string accessToken
            , GetListStatCycleConfigsOptionalParameters optionalParameters
            , ResultCallback<PagedStatCycleConfigs> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(accessToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            HttpRequestBuilder builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/statCycles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                var queryParams = new Dictionary<string, string>();

                if (optionalParameters.Limit != null && optionalParameters.Limit >= 1)
                {
                    queryParams.Add("limit", optionalParameters.Limit.ToString());
                }
                if (optionalParameters.Offset != null && optionalParameters.Offset >= 0)
                {
                    queryParams.Add("offset", optionalParameters.Offset.ToString());
                }
                if (optionalParameters.Type != null && optionalParameters.Type != StatisticCycleType.None)
                {
                    queryParams.Add("cycleType", optionalParameters.Type.ToString().ToUpper());
                }
                if (optionalParameters.Status != null && optionalParameters.Status != StatisticCycleStatus.None)
                {
                    queryParams.Add("status", optionalParameters.Status.ToString().ToUpper());
                }

                builder.WithQueries(queryParams);
            }

            var request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedStatCycleConfigs>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetListUserStatCycleItem(string cycleId,
            string[] statCodes,
            string userId, 
            string accessToken, 
            ResultCallback<PagedStatCycleItem> callback, 
            int offset, 
            int limit)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetListUserStatCycleItemOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                StatCodes = statCodes
            };

            GetListUserStatCycleItem(cycleId, userId, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetListUserStatCycleItem(string cycleId
            , string userId
            , string accessToken
            , GetListUserStatCycleItemOptionalParameters optionalParameters
            , ResultCallback<PagedStatCycleItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , cycleId
                , accessToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/statCycles/{cycleId}/statCycleitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("cycleId", cycleId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                var queryParams = new Dictionary<string, string>();

                if (optionalParameters.Limit != null && optionalParameters.Limit >= 1)
                {
                    queryParams.Add("limit", optionalParameters.Limit.ToString());
                }
                if (optionalParameters.Offset != null && optionalParameters.Offset >= 0)
                {
                    queryParams.Add("offset", optionalParameters.Offset.ToString());
                }
                if (optionalParameters.StatCodes != null && optionalParameters.StatCodes.Length > 0)
                {
                    queryParams.Add("statCodes", string.Join(",", optionalParameters.StatCodes));
                }

                builder.WithQueries(queryParams);
            }

            IHttpRequest request = builder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedStatCycleItem>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetMyStatItems(
            IEnumerable<string> statCodes, 
            IEnumerable<string> tags, 
            string accessToken, 
            ResultCallback<PagedStatItems> callback,
            int limit, 
            int offset)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMyStatItemsOptionalParam()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                StatCodes = statCodes?.ToArray(),
                Tags = tags?.ToArray()
            };

            GetMyStatItems(accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetMyStatItems(
            string accessToken
            , GetMyStatItemsOptionalParam optionalParameters
            , ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, accessToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/statitems")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                if (optionalParameters.Limit >= 1)
                {
                    requestBuilder.WithQueryParam("limit", optionalParameters.Limit.ToString());
                }
                if (optionalParameters.Offset >= 0)
                {
                    requestBuilder.WithQueryParam("offset", optionalParameters.Offset.ToString());
                }
                if (optionalParameters.StatCodes != null && optionalParameters.StatCodes.Length > 0)
                {
                    foreach (string statCode in optionalParameters.StatCodes)
                    {
                        requestBuilder.WithQueryParam("statCodes", statCode);
                    }
                }
                if (optionalParameters.Tags != null && optionalParameters.Tags.Length > 0)
                {
                    foreach (string tag in optionalParameters.Tags)
                    {
                        requestBuilder.WithQueryParam("tags", tag);
                    }
                }
            }

            var request = requestBuilder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedStatItems>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetMyStatItemValues(
            IEnumerable<string> statCodes,
            IEnumerable<string> tags,
            string additionalKey,
            string accessToken,
            ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMyStatItemValuesOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                AdditionalKey = additionalKey,
                StatCodes = statCodes?.ToArray(),
                Tags = tags?.ToArray()
            };

            GetMyStatItemValues(accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetMyStatItemValues(
            string accessToken
            , GetMyStatItemValuesOptionalParameters optionalParameters
            , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, accessToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                if (!string.IsNullOrEmpty(optionalParameters.AdditionalKey))
                {
                    requestBuilder.WithQueryParam("additionalKey", optionalParameters.AdditionalKey);
                }
                if (optionalParameters.StatCodes != null && optionalParameters.StatCodes.Length > 0)
                {
                    foreach (string statCode in optionalParameters.StatCodes)
                    {
                        requestBuilder.WithQueryParam("statCodes", statCode);
                    }
                }
                if (optionalParameters.Tags != null && optionalParameters.Tags.Length > 0)
                {
                    foreach (string tag in optionalParameters.Tags)
                    {
                        requestBuilder.WithQueryParam("tags", tag);
                    }
                }
            }

            var request = requestBuilder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<FetchUser[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetMyStatCycleItems(
            string cycleId,
            IEnumerable<string> statCodes,
            string accessToken,
            int limit,
            int offset,
            ResultCallback<PagedStatCycleItem> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMyStatCycleItemsOptionalParam()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                StatCodes = statCodes?.ToArray()
            };

            GetMyStatCycleItems(cycleId, accessToken, optionalParameters, callback);

            yield return null;
        }

        internal void GetMyStatCycleItems(
            string cycleId
            , string accessToken
            , GetMyStatCycleItemsOptionalParam optionalParameters
            , ResultCallback<PagedStatCycleItem> callback
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, accessToken, cycleId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/statCycles/{cycleId}/statCycleitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("cycleId", cycleId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                if (optionalParameters.Limit >= 1)
                {
                    requestBuilder.WithQueryParam("limit", optionalParameters.Limit.ToString());
                }
                if (optionalParameters.Offset >= 0)
                {
                    requestBuilder.WithQueryParam("offset", optionalParameters.Offset.ToString());
                }
                if (optionalParameters.StatCodes != null && optionalParameters.StatCodes.Length > 0)
                {
                    foreach (string statCode in optionalParameters.StatCodes)
                    {
                        requestBuilder.WithQueryParam("statCodes", statCode);
                    }
                }
            }

            var request = requestBuilder.GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedStatCycleItem>();
                callback?.Try(result);
            });
        }
    }
}
