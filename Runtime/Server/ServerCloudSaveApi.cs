// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerCloudSaveApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==CloudSaveServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ServerCloudSaveApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.CloudSaveServerUrl, session )
        {
        }

        #region Private Methods 

        const string metaDataFieldKey = "__META";

        private Dictionary<string, object> AddMetaDataJsonAdminRecord
            (AdminRecordMetadataOptionalParams optionalParams
            , Dictionary<string, object> requestToInject)
        {
            if (requestToInject.ContainsKey(metaDataFieldKey))
            {
                AccelByteDebug.LogWarning(
                    $"{metaDataFieldKey} field for AdminGameRecord request overriden. " +
                    $"Please use optionalParams instead to populate metadata");
            }

            requestToInject[metaDataFieldKey] = new SaveAdminRecordMetaData
            {
                TTLConfig = optionalParams.TTLConfig,
                Tags = optionalParams.Tags
            };

            return requestToInject;
        }

        private Dictionary<string, object> AddMetaDataJsonGameRecord
            (GameRecordMetadataOptionalParams optionalParams
            , Dictionary<string, object> RequestToInject)
        {
            if (RequestToInject.ContainsKey(metaDataFieldKey))
            {
                AccelByteDebug.LogWarning(
                    $"{metaDataFieldKey} field for GameRecord request overriden. " +
                    $"Please use optionalParams instead to populate metadata");
            }

            RequestToInject[metaDataFieldKey] = new SaveGameRecordMetaData
            {
                SetBy = optionalParams.SetBy,
                TTLConfig = optionalParams.TTLConfig,
                Tags = optionalParams.Tags
            };

            return RequestToInject;
        }

        private Dictionary<string, object> AddMetaDataJsonUserRecord( RecordSetBy setBy
            , bool isPublic
            , Dictionary<string, object> RequestToInject )
        {
            RequestToInject[metaDataFieldKey] = new
            {
                set_by = setBy.GetString(), 
                is_public = isPublic,
            };
            
            return RequestToInject;
        }
        #endregion /Private Methods

        public void SaveGameRecord(
            string key
            , Dictionary<string, object> recordRequest
            , GameRecordMetadataOptionalParams optionalParams
            , ResultCallback<GameRecord> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, key, recordRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            recordRequest = AddMetaDataJsonGameRecord(optionalParams, recordRequest);

            SaveGameRecord(key, recordRequest, callback);
        }

        public void SaveGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback<GameRecord> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, key, recordRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameRecord>();
                callback?.Try(result);
            });
        }

        public void SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool setPublic
            , ResultCallback callback )
        {
            recordRequest = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            SaveUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                isPublic: false);
        } 
        
        public void GetUserRecord( string userId
            , string key
            , bool isPublic
            , ResultCallback<UserRecord> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string url = "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
                url += "/public";

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserRecord>();
                callback?.Try(result);
            });
        }

        public void ReplaceUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, userId, recordRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string url = "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
                url += "/public";

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void ReplaceGameRecord(string key
            , Dictionary<string, object> recordRequest
            , GameRecordMetadataOptionalParams optionalParams
            , ResultCallback<GameRecord> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, key, recordRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            recordRequest = AddMetaDataJsonGameRecord(optionalParams, recordRequest);

            ReplaceGameRecord(key, recordRequest, callback);
        }

        public void ReplaceGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback<GameRecord> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, key, recordRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string url = "/v1/admin/namespaces/{namespace}/records/{key}"; 

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameRecord>();
                callback?.Try(result);
            });
        }
        
        public void ReplaceGameRecord( string key
            , ConcurrentReplaceRequest data
            , ResultCallback callback
            , Action callbackOnConflictedData = null )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, key, data);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/concurrent/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToJsonString())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();

                bool hasErr =
                    result.IsError &&
                    result.Error.Code == ErrorCode.GameRecordPreconditionFailed &&
                    callbackOnConflictedData != null;

                if (hasErr)
                {
                    callbackOnConflictedData?.Invoke();
                }
                else
                {
                    callback?.Try(result);
                }
            });
        }

        public void DeleteGameRecord( string key
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        #region User Record 
        public void SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, userId, key, recordRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string url = "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
                url += "/public";

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void ReplaceUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool setPublic
            , ResultCallback callback )
        {
            recordRequest = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            ReplaceUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                false);
        }

        public void DeleteUserRecord( string userId
            , string key
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, userId, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void RetrieveGameRecordsKey(
            ResultCallback<GameRecordList> callback
            ,string query
            ,int offset
            ,int limit )
        {
            string url = "/v1/admin/namespaces/{namespace}/records";

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + url)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("query", query != "{}" ? query : "")
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit >= 0) ? limit.ToString() : "")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameRecordList>();
                callback?.Try(result);
            });
        }

        #endregion

        public void GetGameRecords(string key, ResultCallback<GameRecord> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string url = "/v1/admin/namespaces/{namespace}/records/{key}";

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameRecord>();
                callback?.Try(result);
            });
        }

        public void CreateAdminGameRecord(string key
            , Dictionary<string, object> recordRequest
            , AdminRecordMetadataOptionalParams optionalParams
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            recordRequest = AddMetaDataJsonAdminRecord(optionalParams, recordRequest);

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<AdminGameRecord>();
                callback?.Try(result);
            });
        }

        public void QueryAdminGameRecordsByKey(string key
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<AdminGameRecord>();
                callback?.Try(result);
            });
        }

        public void BulkQueryAdminGameRecordsByKey(string[] keys,
            ResultCallback<BulkAdminGameRecordResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, keys);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (keys.Length > 20)
            {
                error = new Error(ErrorCode.InvalidRequest, message: "Number of keys cannot be greater than 20.");
                callback?.TryError(error);
                return;
            }

            var requestBody = new BulkQueryAdminGameRecordRequest()
            {
                Keys = keys
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/adminrecords/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<BulkAdminGameRecordResponse>();
                callback?.Try(result);
            });
        }

        public void QueryAdminGameRecordKeys(ResultCallback<GameRecordList> callback
            , int limit
            , int offset)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/adminrecords")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GameRecordList>();
                callback?.Try(result);
            });
        }

        public void ReplaceAdminGameRecord(string key
            , Dictionary<string, object> recordRequest
            , AdminRecordMetadataOptionalParams optionalParams
            , ResultCallback<AdminGameRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            recordRequest = AddMetaDataJsonAdminRecord(optionalParams, recordRequest);

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<AdminGameRecord>();
                callback?.Try(result);
            });
        }

        public void DeleteAdminGameRecord(string key
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void CreateAdminUserRecord(string key
            , string userId
            , Dictionary<string, object> recordRequest
            , ResultCallback<AdminUserRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<AdminUserRecord>();
                callback?.Try(result);
            });
        }

        public void QueryAdminUserRecordsByKey(string key
            , string userId
            , ResultCallback<AdminUserRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<AdminUserRecord>();
                callback?.Try(result);
            });
        }

        public void QueryAdminUserRecordKeys(string userId
            , ResultCallback<PaginatedGetAdminUserRecordKeys> callback
            , int limit = 20
            , int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/adminrecords")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("limit", limit >= 0 ? limit.ToString() : string.Empty)
                .WithQueryParam("offset", offset >= 0 ? offset.ToString() : string.Empty)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PaginatedGetAdminUserRecordKeys>();
                callback?.Try(result);
            });
        }

        public void ReplaceAdminUserRecord(string key
            , string userId
            , Dictionary<string, object> recordRequest
            , ResultCallback<AdminUserRecord> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<AdminUserRecord>();
                callback?.Try(result);
            });
        }

        public void DeleteAdminUserRecord(string key
            , string userId
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/adminrecords/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void DeleteAdminGameRecordTTLConfig(string key, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/adminrecords/{key}/ttl")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void DeleteGameRecordTTLConfig(string key, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/records/{key}/ttl")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }
    }
}
