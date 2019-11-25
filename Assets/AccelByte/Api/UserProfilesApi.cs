// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class UserProfilesApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal UserProfilesApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        public IEnumerator GetUserProfile(string @namespace, string userAccessToken,
            ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get user profile! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user profile! UserAccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();

            callback.Try(result);
        }

        public IEnumerator CreateUserProfile(string @namespace, string userAccessToken,
            CreateUserProfileRequest createRequest, ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't create user profile! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't create user profile! UserAccessToken parameter is null!");
            Assert.IsNotNull(createRequest, "Can't create user profile! CreateRequest parameter is null!");
            Assert.IsNotNull(
                createRequest.language,
                "Can't create user profile! CreateRequest.language parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(createRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();

            callback.Try(result);
        }

        public IEnumerator UpdateUserProfile(string @namespace, string userAccessToken,
            UpdateUserProfileRequest updateRequest, ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't update user profile! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't update user profile! UserAccessToken parameter is null!");
            Assert.IsNotNull(updateRequest, "Can't update user profile! ProfileRequest parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(updateRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();
            callback.Try(result);
        }

        public IEnumerator GetUserProfilePublicInfo(string @namespace, string userId, string userAccessToken,
            ResultCallback<PublicUserProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get user profile public info! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user profile public info! userId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user profile public info! UserAccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/users/{user_id}/profiles/public")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("user_id", userId)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PublicUserProfile>();
            callback.Try(result);
        }

        public IEnumerator GetUserProfilePublicInfosByIds(string @namespace, string userAccessToken, string[] userIds,
            ResultCallback<PublicUserProfile[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get user profile public info by ids! Namespace parameter is null!");
            Assert.IsNotNull(
                userAccessToken,
                "Can't get user profile public info by ids! UserAccessToken parameter is null!");

            Assert.IsNotNull(userIds, "Can't get user profile info by ids! userIds parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/profiles/public")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("userIds", string.Join(",", userIds))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PublicUserProfile[]>();
            callback.Try(result);
        }

        public IEnumerator GetTimeZones(string @namespace, string userAccessToken, ResultCallback<string[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get time zones! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get time zones! UserAccessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/misc/timezones")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<string[]>();
            callback.Try(result);
        }
    }
}