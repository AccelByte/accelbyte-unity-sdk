// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

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
        private string baseUrl;
        private IHttpClient httpClient;

        internal ServerCloudSaveApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        string ConvertPlayerRecordSetByToString(UserRecordSetBy setBy)
        {
            switch (setBy)
            {
                case UserRecordSetBy.SERVER:
                    return "SERVER";
                case UserRecordSetBy.CLIENT:
                    return "CLIENT";
                default:
                    return "CLIENT";
            }
        }

        Dictionary<string, object> AddMetaDataJson(bool isPublic, UserRecordSetBy setBy, Dictionary<string, object> RequestToInject)
        {
            ServerMetaRequest serverMetaRequest = new ServerMetaRequest
            {
                is_public = isPublic,
                set_by = ConvertPlayerRecordSetByToString(setBy)
            };
            
            RequestToInject["META"] = serverMetaRequest;

            return RequestToInject;
        }

        public IEnumerator SaveUserRecord(string @namespace, string userId, string accessToken, string key, Dictionary<string, object> recordRequest, bool isPublic,
            ResultCallback callback)
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
            UserRecordSetBy setBy, bool isPublic, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't save user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't save user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't save user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't save user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't save user record! recordRequest parameter is null!");

            Dictionary<string, object> requestToSend = AddMetaDataJson(isPublic, setBy, recordRequest);

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestToSend.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
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

        public IEnumerator ReplaceUserRecord(string @namespace, string userId, string accessToken, string key, Dictionary<string, object> recordRequest, bool isPublic,
            ResultCallback callback)
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
            UserRecordSetBy setBy, bool isPublic, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't replace user record! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't replace user record! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't replace user record! AccessToken parameter is null!");
            Assert.IsNotNull(key, "Can't replace user record! Key parameter is null!");
            Assert.IsNotNull(recordRequest, "Can't replace user record! recordRequest parameter is null!");

            Dictionary<string, object> requestToSend = AddMetaDataJson(isPublic, setBy, recordRequest);

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/records/{key}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("key", key)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestToSend.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
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
    }
}
