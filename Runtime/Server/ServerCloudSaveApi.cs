// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
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
        internal ServerCloudSaveApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.CloudSaveServerUrl, session )
        {
        }

        #region Private Methods 
        private Dictionary<string, object> AddMetaDataJsonGameRecord
            ( RecordSetBy setBy
            , Dictionary<string, object> RequestToInject )
        {
            RequestToInject["__META"] = new { set_by = setBy.GetString() };
            return RequestToInject;
        }

        private Dictionary<string, object> AddMetaDataJsonUserRecord( RecordSetBy setBy
            , bool isPublic
            , Dictionary<string, object> RequestToInject )
        {
            RequestToInject["__META"] = new
            {
                set_by = setBy.GetString(), 
                is_public = isPublic,
            };
            
            return RequestToInject;
        }
        #endregion /Private Methods
        
        public IEnumerator SaveGameRecord( string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , ResultCallback callback )
        {
            recordRequest = AddMetaDataJsonGameRecord(setBy, recordRequest);
            yield return SaveGameRecord(key, recordRequest, callback);
        }
  
        public IEnumerator SaveGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! recordRequest parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/records/{key}")
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

        public IEnumerator SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool setPublic
            , ResultCallback callback )
        {
            recordRequest = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            yield return SaveUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                isPublic: false);
        } 
        
        public IEnumerator GetUserRecord( string userId
            , string key
            , bool isPublic
            , ResultCallback<UserRecord> callback )
        {
            Assert.IsNotNull(Namespace_, "Can't get user record! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get user record! AuthToken parameter is null!");
            Assert.IsNotNull(userId, "Can't get user record! userId parameter is null!");
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

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

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserRecord>();
            callback.Try(result);
        }

        public IEnumerator ReplaceUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic )
        {
            Assert.IsNotNull(userId, "Can't replace user record! userId parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't replace user record! recordRequest parameter is null!");

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

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
        
        public IEnumerator ReplaceGameRecord( string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , ResultCallback callback )
        {
            recordRequest = AddMetaDataJsonGameRecord(setBy, recordRequest);
            yield return ReplaceGameRecord(key, recordRequest, callback);
        }
        
        public IEnumerator ReplaceGameRecord( string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback )
        {
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't replace user record! recordRequest parameter is null!");

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
            Assert.IsNotNull(key, nameof(key) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null"); 

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/concurrent/records/{key}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("key", key)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToJsonString())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            bool hasErr = 
                result.IsError && 
                result.Error.Code == ErrorCode.GameRecordPreconditionFailed && 
                callbackOnConflictedData != null;
            
            if (hasErr)
                callbackOnConflictedData?.Invoke();
            else
                callback.Try(result);
        }

        public IEnumerator DeleteGameRecord( string key
            , ResultCallback callback )
        {
            Assert.IsNotNull(AuthToken, "Can't delete user record! AuthToken parameter is null!");
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/records/{key}")
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

        #region User Record 
        public IEnumerator SaveUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , ResultCallback callback
            , bool isPublic )
        {
            Assert.IsNotNull(userId, "Can't save user record! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't save user record! AuthToken parameter is null!");
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! recordRequest parameter is null!");

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

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ReplaceUserRecord( string userId
            , string key
            , Dictionary<string, object> recordRequest
            , RecordSetBy setBy
            , bool setPublic
            , ResultCallback callback )
        {
            recordRequest = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            yield return ReplaceUserRecord(
                userId,
                key,
                recordRequest,
                callback,
                false);
        }

        public IEnumerator DeleteUserRecord( string userId
            , string key
            , ResultCallback callback )
        {
            Assert.IsNotNull(Namespace_, "Can't delete user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete user record! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't delete user record! AuthToken parameter is null!");
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}")
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

        #endregion
    }
}
