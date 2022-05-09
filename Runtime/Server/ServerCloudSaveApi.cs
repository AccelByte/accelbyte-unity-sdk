// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerCloudSaveApi
    {
        #region Fields 

        private string baseUrl;
        private IHttpClient httpClient;
        
        #endregion

        #region Constructor
        
        internal ServerCloudSaveApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        #endregion

        #region Private Methods 

        Dictionary<string, object> AddMetaDataJsonGameRecord(RecordSetBy setBy, Dictionary<string, object> RequestToInject)
        {
            RequestToInject["__META"] = new { set_by = setBy.GetString() };
            return RequestToInject;
        }

        Dictionary<string, object> AddMetaDataJsonUserRecord(RecordSetBy setBy, bool isPublic, Dictionary<string, object> RequestToInject)
        {
            RequestToInject["__META"] = new { set_by = setBy.GetString(), is_public = isPublic };
            return RequestToInject;
        }
        
        #endregion

        #region Public Methods

        #region Game Record
        public IEnumerator SaveGameRecord(string @namespace, string accessToken, string key, Dictionary<string, object> recordRequest,
          RecordSetBy setBy, ResultCallback callback)
        {
            recordRequest = AddMetaDataJsonGameRecord(setBy, recordRequest);

            yield return SaveGameRecord(@namespace, accessToken, key, recordRequest, callback);
        }

        public IEnumerator SaveGameRecord(string @namespace, string accessToken, string key, Dictionary<string, object> recordRequest, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't save user record! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't save user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! recordRequest parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/admin/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator GetGameRecords(string @namespace, string userId, string accessToken, string key, ResultCallback<GameRecordList> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            string url = "/v1/admin/namespaces/{namespace}/records"; 

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + url)
                .WithPathParam("namespace", @namespace)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameRecordList>();
            callback.Try(result);
        }
        
        public IEnumerator GetGameRecord(string @namespace, string userId, string accessToken, string key, ResultCallback<GameRecord> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            string url = "/v1/admin/namespaces/{namespace}/records/{key}";

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + url)
                .WithPathParam("namespace", @namespace)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameRecord>();
            callback.Try(result);
        }

        public IEnumerator ReplaceGameRecord(string @namespace, string accessToken, string key, Dictionary<string, object> recordRequest,
            RecordSetBy setBy, ResultCallback callback)
        {
            recordRequest = AddMetaDataJsonGameRecord(setBy, recordRequest);
            yield return ReplaceGameRecord(@namespace, accessToken, key, recordRequest, callback);
        }

        public IEnumerator ReplaceGameRecord(string @namespace, string accessToken, string key, Dictionary<string, object> recordRequest,
            ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't replace user record! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't replace user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't replace user record! recordRequest parameter is null!");

            string url = "/v1/admin/namespaces/{namespace}/records/{key}"; 

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + url)
                .WithPathParam("namespace", @namespace)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ReplaceGameRecord(string @namespace, string accessToken, string key, 
            ConcurrentReplaceRequest data, ResultCallback callback, Action callbackOnConflictedData = null)
        {
            Assert.IsNotNull(@namespace, nameof(@namespace) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(key, nameof(key) + " cannot be null");
            Assert.IsNotNull(data, nameof(data) + " cannot be null"); 

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/admin/namespaces/{namespace}/concurrent/records/{key}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .WithBody(data.ToJsonString())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();

            if (result.IsError && result.Error.Code == ErrorCode.GameRecordPreconditionFailed && callbackOnConflictedData != null)
            {
                callbackOnConflictedData?.Invoke();
            }
            else
            {
                callback.Try(result);
            }
        }

        public IEnumerator DeleteGameRecord(string @namespace, string accessToken, string key,
            ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't delete user record! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't delete user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v1/admin/namespaces/{namespace}/records/{key}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        #endregion

        #region User Record 
        public IEnumerator SaveUserRecord(string @namespace, string userId, string accessToken, string key, Dictionary<string, object> recordRequest, 
            ResultCallback callback, bool isPublic)
        {
            Assert.IsNotNull(@namespace, "Can't save user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't save user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't save user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! recordRequest parameter is null!");

            string url = "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
            {
                url += "/public";
            }

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + url)
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SaveUserRecord(string @namespace, string userId, string accessToken, string key, Dictionary<string, object> recordRequest,
            RecordSetBy setBy, bool setPublic, ResultCallback callback)
        {
            recordRequest = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            yield return SaveUserRecord(@namespace, userId, accessToken, key, recordRequest, callback, false);
        }      

        public IEnumerator GetUserRecord(string @namespace, string userId, string accessToken, string key, bool isPublic,
            ResultCallback<UserRecord> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't get user record! Key parameter is null!");

            string url = "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
            {
                url += "/public";
            }

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + url)
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserRecord>();
            callback.Try(result);
        }

        public IEnumerator ReplaceUserRecord(string @namespace, string userId, string accessToken, string key, Dictionary<string, object> recordRequest,
            ResultCallback callback, bool isPublic)
        {
            Assert.IsNotNull(@namespace, "Can't replace user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't replace user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't replace user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't replace user record! recordRequest parameter is null!");

            string url = "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}";
            if (isPublic)
            {
                url += "/public";
            }

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + url)
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(recordRequest.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ReplaceUserRecord(string @namespace, string userId, string accessToken, string key, Dictionary<string, object> recordRequest,
            RecordSetBy setBy, bool setPublic, ResultCallback callback)
        {
            recordRequest = AddMetaDataJsonUserRecord(setBy, setPublic, recordRequest);
            yield return ReplaceUserRecord(@namespace, userId, accessToken, key, recordRequest, callback, false);
        }

        public IEnumerator DeleteUserRecord(string @namespace, string userId, string accessToken, string key,
            ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't delete user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't delete user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't delete user record! Key parameter is null!");

            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        #endregion

        #endregion
    }
}
