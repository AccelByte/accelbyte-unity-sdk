// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class CloudSaveApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==CloudSaveServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal CloudSaveApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.CloudSaveServerUrl, session )
        {
        }

        #region Private Methods 

        Dictionary<string, object> AddMetaDataJsonGameRecord( RecordSetBy setBy
            , Dictionary<string, object> RequestToInject )
        {
            RequestToInject["__META"] = new { set_by = setBy.GetString() };
            return RequestToInject;
        }

        Dictionary<string, object> AddMetaDataJsonUserRecord( RecordSetBy setBy
            , bool isPublic
            , Dictionary<string, object> RequestToInject )
        {
            RequestToInject["__META"] = new { set_by = setBy.GetString(), is_public = isPublic };
            return RequestToInject;
        }
        
        private const int maxBulkRecords = 20; 

        #endregion

        public IEnumerator SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , bool isPublic
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't save user record! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't save user record! AccessToken parameter is null!");
            Assert.IsNotNull(userId, "Can't save user record! userId parameter is null!");
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! recordRequest parameter is null!");

            string url = "/v1/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
            {
                url += "/public"; //POST method for endpoint using this suffix will be deprecated in the future, please pay attention to declaration warning
            }

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

            IHttpResponse response = null;
            
            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);
            
            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SaveUserRecord( string namespace_
            , string userId
            , string accessToken
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool setPublic
            , ResultCallback callback )
        {
            recordRequest = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            yield return SaveUserRecord(userId, key, recordRequest, callback, false);
        }

        public IEnumerator SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, key, recordRequest);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            string url = "/v1/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
            {
                url += "/public"; //POST method for endpoint using this suffix will be deprecated in the future, please pay attention to declaration warning
            }

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

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
                
        public IEnumerator GetUserRecord( string userId
            , string key
            , ResultCallback<UserRecord> callback
            , bool isPublic )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, key);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            string url = "/v1/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
            {
                url += "/public";
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + url)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserRecord>();
            callback.Try(result);
        }


        public IEnumerator ReplaceUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool setPublic
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, key, recordRequest);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            Dictionary<string, object> requestToSend = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            yield return ReplaceUserRecord(userId, key, recordRequest, callback, isPublic:false);
        }

        public IEnumerator ReplaceUserRecord
            ( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, key, recordRequest);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            string url = "/v1/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
            {
                url += "/public"; //PUT method for endpoint using this suffix will be deprecated in the future, please pay attention to declaration warning
            }

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

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
        
        public IEnumerator ReplaceUserRecord( string userId
            , string key
            , ConcurrentReplaceRequest data
            , ResultCallback callback
            , Action callbackOnConflictedData = null )
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(key) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(data.ToString()))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(data) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/users/{userID}/concurrent/records/{key}/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userID", userId)
                .WithPathParam("key", key)
                .WithQueryParam("responseBody", "false")
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            if (result.IsError && 
                result.Error.Code == ErrorCode.PlayerRecordPreconditionFailed && 
                callbackOnConflictedData != null)
            {
                callbackOnConflictedData?.Invoke();
            }
            else
            {
                callback.Try(result);
            }
        }
        
        public IEnumerator ReplaceUserRecord( string userId
            , string key
            , ConcurrentReplaceRequest data
            , ResultCallback<ConcurrentReplaceUserRecordResponse> callback
            , Action callbackOnConflictedData = null )
        {
            if (string.IsNullOrEmpty(Namespace_))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(Namespace_) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(userId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(AuthToken))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(AuthToken) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(key))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(key) + " cannot be null or empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(data.ToString()))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, nameof(data) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/users/{userID}/concurrent/records/{key}/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userID", userId)
                .WithPathParam("key", key)
                .WithQueryParam("responseBody", "true")
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<ConcurrentReplaceUserRecordResponse>();

            if (result.IsError && 
                result.Error.Code == ErrorCode.PlayerRecordPreconditionFailed && 
                callbackOnConflictedData != null)
            {
                callbackOnConflictedData?.Invoke();
            }
            else
            {
                callback.Try(result);
            }
        }

        public IEnumerator DeleteUserRecord( string userId
            , string key
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId, key);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
        
        public IEnumerator SaveGameRecord( string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, recordRequest, key);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            Dictionary<string, object> requestToSend = AddMetaDataJsonGameRecord(setBy, recordRequest);
            yield return SaveGameRecord(key, recordRequest, callback);
        }

        public IEnumerator SaveGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, recordRequest, key);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetGameRecord( string key
            , ResultCallback<GameRecord> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameRecord>();
            callback.Try(result);
        }

        public IEnumerator ReplaceGameRecord( string namespace_
            , string accessToken
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(namespace_, accessToken, key, recordRequest);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            Dictionary<string, object> requestToSend = AddMetaDataJsonGameRecord(setBy, recordRequest);
            yield return ReplaceGameRecord(key, recordRequest, callback);
        }

        public IEnumerator ReplaceGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, recordRequest);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ReplaceGameRecord( string key
            , ConcurrentReplaceRequest data
            , ResultCallback callback
            , Action callbackOnConflictedData = null )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key, data);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/concurrent/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            if (result.IsError && 
                result.Error.Code == ErrorCode.GameRecordPreconditionFailed && 
                callbackOnConflictedData != null)
            {
                callbackOnConflictedData?.Invoke();
            }
            else
            {
                callback.Try(result);
            }
        }

        public IEnumerator DeleteGameRecord( string key
            , ResultCallback callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, key);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
        
        
        public IEnumerator BulkGetUserRecords( BulkGetRecordsByKeyRequest data
            , ResultCallback<UserRecords> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, data, data?.keys);
            if (error == null && data.keys.Length == 0)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , "Can't bulk get user records! data.keys parameter is empty!"
                );
            }
            if (error == null && data.keys.Length > maxBulkRecords)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , $"Can't bulk get user records! data.keys exceeding {maxBulkRecords}!"
                );
            }
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/me/records/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserRecords>();
            callback.Try(result);
        }

        public IEnumerator BulkGetPublicUserRecords(string key
            , BulkGetPublicUserRecordsByUserIdsRequest data
            , ResultCallback<UserRecords> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, data, data?.UserIds);
            if (error == null && data.UserIds.Length == 0)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , "Can't bulk get user records! data.userId parameter is empty!"
                );
            }
            if (error == null && data.UserIds.Length > maxBulkRecords)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , $"Can't bulk get user records! data.keys exceeding {maxBulkRecords}!"
                );
            }
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/bulk/records/{key}/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                result =>
                {
                    response = result;
                });

            var result = response.TryParseJson<UserRecords>();
            callback.Try(result);
        }

        public IEnumerator BulkGetGameRecords( BulkGetRecordsByKeyRequest data
            , ResultCallback<GameRecords> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, data, data?.keys);
            if (error == null && data.keys.Length == 0)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , "Can't bulk get game records! data.keys parameter is empty!"
                );
            }
            if (error == null && data.keys.Length > maxBulkRecords)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , $"Can't bulk get game records! data.keys exceeding {maxBulkRecords}!"
                );
            }
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/records/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameRecords>();
            callback.Try(result);
        }
        
        public IEnumerator BulkGetOtherPlayerPublicRecordKeys( string userId
            , ResultCallback<PaginatedBulkGetPublicUserRecordKeys> callback
            , int offset
            , int limit)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userId);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/records/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => 
                {
                    response = rsp;
                });

            var result = response.TryParseJson<PaginatedBulkGetPublicUserRecordKeys>();
            callback.Try(result);
        }
        
        public IEnumerator BulkGetOtherPlayerPublicRecords( string userId
            , BulkGetRecordsByKeyRequest data
            , ResultCallback<UserRecords> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, data, data?.keys);
            if (error == null && data.keys.Length == 0)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , "Can't bulk get user records! data.keys parameter is empty!"
                );
            }
            if (error == null && data.keys.Length > maxBulkRecords)
            {
                error = new Error(ErrorCode.InvalidRequest
                    , $"Can't bulk get user records! data.keys exceeding {maxBulkRecords}!"
                );
            }
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }
            
            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/users/{userId}/records/public/bulk")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UserRecords>();
            callback.Try(result);
        }
    }
}
