// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using System.Linq;
using AccelByte.Utils;

namespace AccelByte.Server
{
    public class ServerStatisticApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==StatisticServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerStatisticApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.StatisticServerUrl, session)
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

        public IEnumerator CreateUserStatItems(string userId
            , CreateStatItemRequest[] statItems
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateUserStatItems(userId, statItems, optionalParameters, callback);

            yield return null;
        }

        internal void CreateUserStatItems(string userId
            , CreateStatItemRequest[] statItems
            , CreateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, statItems, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
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

        public IEnumerator GetUserStatItems(string userId
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

            GetUserStatItems(userId, optionalParameters, callback);

            yield return null;
        }

        internal void GetUserStatItems(string userId
            , GetUserStatItemsOptionalParam optionalParameters
            , ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
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

        public IEnumerator IncrementUserStatItems(string userId
            , StatItemIncrement[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new IncrementUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            IncrementUserStatItems(userId, data, optionalParameters, callback);

            yield return null;
        }

        internal void IncrementUserStatItems(string userId
            , StatItemIncrement[] data
            , IncrementUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken, data);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator IncrementManyUsersStatItems(UserStatItemIncrement[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new IncrementManyUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            IncrementManyUsersStatItems(data, optionalParameters, callback);

            yield return null;
        }

        internal void IncrementManyUsersStatItems(UserStatItemIncrement[] data
            , IncrementManyUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, data, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/admin/namespaces/{namespace}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator ResetUserStatItems(string userId
            , StatItemReset[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ResetUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ResetUserStatItems(userId, data, optionalParameters, callback);

            yield return null;
        }

        internal void ResetUserStatItems(string userId
            , StatItemReset[] data
            , ResetUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken, data);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator ResetManyUsersStatItems(UserStatItemReset[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ResetManyUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ResetManyUsersStatItems(data, optionalParameters, callback);

            yield return null;
        }

        internal void ResetManyUsersStatItems(UserStatItemReset[] data
            , ResetManyUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, data, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator UpdateUserStatItems(string userId
            , string additionalKey
            , StatItemUpdate[] data
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                AdditionalKey = additionalKey
            };

            UpdateUserStatItems(userId, data, optionalParameters, callback);

            yield return null;
        }

        internal void UpdateUserStatItems(string userId
            , StatItemUpdate[] data
            , UpdateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken, data);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                builder.WithQueryParam("additionalKey", optionalParameters.AdditionalKey);
            }

            var request = builder.GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator UpdateManyUsersStatItems( UserStatItemUpdate[] data
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateManyUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateManyUsersStatItems(data, optionalParameters, callback);

            yield return null;
        }

        internal void UpdateManyUsersStatItems(UserStatItemUpdate[] data
            , UpdateManyUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, data, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<StatItemOperationResult[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator BulkFetchUserStatItemValues(string statCode
           , string[] userIds
           , string additionalKey
           , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkFetchUserStatItemsValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                AdditionalKey = additionalKey
            };

            BulkFetchUserStatItemValues(statCode, userIds, optionalParameters, callback);

            yield return null;
        }

        internal void BulkFetchUserStatItemValues(string statCode
           , string[] userIds
           , BulkFetchUserStatItemsValueOptionalParameters optionalParameters
           , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, statCode, userIds, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v2/admin/namespaces/{namespace}/statitems/value/bulk/getOrDefault")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithQueryParam("statCode", statCode)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);
            foreach (var userId in userIds)
            {
                builder.WithQueryParam("userIds", userId);
            }

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
                var result = response.TryParseJson<FetchUser[]>();
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

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, statCode, userIds, AuthToken);
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
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/statitems/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithQueryParam("statCode", statCode)
                .WithQueryParam("userIds", string.Join(",", processedUserIds))
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

        public IEnumerator BulkUpdateMultipleUserStatItemsValue(UpdateUserStatItem[] bulkUpdateMultipleUserStatItem
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkUpdateMultipleUserStatItemsValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkUpdateMultipleUserStatItemsValue(bulkUpdateMultipleUserStatItem, optionalParameters, callback);

            yield return null;
        }

        internal void BulkUpdateMultipleUserStatItemsValue(UpdateUserStatItem[] bulkUpdateMultipleUserStatItem
           , BulkUpdateMultipleUserStatItemsValueOptionalParameters optionalParameters
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, bulkUpdateMultipleUserStatItem, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(bulkUpdateMultipleUserStatItem.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UpdateUserStatItemsResponse[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator BulkResetUserStatItemsValues(string userId
            , string additionalKey
            , UserStatItem[] bulkUserStatItems
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkResetUserStatItemsValuesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkResetUserStatItemsValues(userId, additionalKey, bulkUserStatItems, optionalParameters, callback);

            yield return null;
        }

        internal void BulkResetUserStatItemsValues(string userId
            , string additionalKey
            , UserStatItem[] bulkUserStatItems
            , BulkResetUserStatItemsValuesOptionalParameters optionalParameters
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(userId, bulkUserStatItems, Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/statitems/value/reset/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(bulkUserStatItems.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(additionalKey))
            {
                builder.WithQueryParam("additionalKey", additionalKey);
            }

            var request = builder.GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UpdateUserStatItemsResponse[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator BulkUpdateUserStatItemValue(string userId
           , string additionalKey
           , UpdateUserStatItemWithStatCode[] bulkUpdateUserStatItem
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkUpdateUserStatItemValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkUpdateUserStatItemValue(userId, additionalKey, bulkUpdateUserStatItem, optionalParameters, callback);

            yield return null;
        }

        internal void BulkUpdateUserStatItemValue(string userId
           , string additionalKey
           , UpdateUserStatItemWithStatCode[] bulkUpdateUserStatItem
           , BulkUpdateUserStatItemValueOptionalParameters optionalParameters
           , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(userId, bulkUpdateUserStatItem, Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/statitems/value/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(bulkUpdateUserStatItem.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(additionalKey))
            {
                builder.WithQueryParam("additionalKey", additionalKey);
            }

            var request = builder.GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UpdateUserStatItemsResponse[]>();
                callback?.Try(result);
            });
        }

        public IEnumerator UpdateUserStatItemValue(string userId, string statCode
           , string additionalKey
           , UpdateUserStatItem updateUserStatItemValue
           , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            this.UpdateUserStatItemValue(userId, statCode, additionalKey, updateUserStatItemValue, callback);

            yield return null;
        }

        internal void UpdateUserStatItemValue(string userId, string statCode
           , string additionalKey
           , UpdateUserStatItem updateUserStatItemValue
           , UpdateUserStatItemValueOptionalParameters optionalParameters
           , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, statCode, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
            .CreatePut(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems/value")
            .WithPathParam("namespace", Namespace_)
            .WithBearerAuth(AuthToken)
            .WithPathParam("userId", userId)
            .WithPathParam("statCode", statCode)
            .WithBody(updateUserStatItemValue.ToUtf8Json())
            .WithContentType(MediaType.ApplicationJson)
            .Accepts(MediaType.ApplicationJson);

            if (!string.IsNullOrEmpty(additionalKey))
            {
                builder.WithQueryParam("additionalKey", additionalKey);
            }

            var request = builder.GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UpdateUserStatItemValueResponse>();
                callback?.Try(result);
            });
        }

        public IEnumerator DeleteUserStatItems(string userId, string statCode
           , string additionalKey
           , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteUserStatItems(userId, statCode, additionalKey, optionalParameters, callback);

            yield return null;
        }

        internal void DeleteUserStatItems(string userId, string statCode
           , string additionalKey
           , DeleteUserStatItemsOptionalParameters optionalParameters
           , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, statCode, additionalKey, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
            .CreateDelete(BaseUrl + "/v2/admin/namespaces/{namespace}/users/{userId}/stats/{statCode}/statitems")
            .WithPathParam("namespace", Namespace_)
            .WithBearerAuth(AuthToken)
            .WithPathParam("userId", userId)
            .WithPathParam("statCode", statCode)
            .WithQueryParam("additionalKey", additionalKey)
            .WithContentType(MediaType.ApplicationJson)
            .Accepts(MediaType.ApplicationJson)
            .GetResult();

            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public IEnumerator GetGlobalStatItemsByStatCode(string statCode, ResultCallback<GlobalStatItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetGlobalStatItemsByStatCodeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetGlobalStatItemsByStatCode(statCode, optionalParameters, callback);

            yield return null;
        }

        internal void GetGlobalStatItemsByStatCode(string statCode, GetGlobalStatItemsByStatCodeOptionalParameters optionalParameters, ResultCallback<GlobalStatItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(statCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/globalstatitems/{statCode}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("statCode", statCode)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GlobalStatItem>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetListStatCycleConfigs(
            StatisticCycleType type,
            StatisticCycleStatus status,
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

            GetListStatCycleConfigs(optionalParameters, callback);

            yield return null;
        }

        internal void GetListStatCycleConfigs(
            GetListStatCycleConfigsOptionalParameters optionalParameters
            , ResultCallback<PagedStatCycleConfigs> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            HttpRequestBuilder builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/statCycles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
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
                    queryParams.Add("cycleType", optionalParameters.Type.ToString());
                }
                if (optionalParameters.Status != null && optionalParameters.Status != StatisticCycleStatus.None)
                {
                    queryParams.Add("status", optionalParameters.Status.ToString());
                }

                builder.WithQueries(queryParams);
            }

            IHttpRequest request = builder.GetResult();
            
            var additionalParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<PagedStatCycleConfigs>();
                callback?.Try(result);
            });
        }
    }        
}