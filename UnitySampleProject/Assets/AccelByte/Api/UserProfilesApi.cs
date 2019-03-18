// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Net;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class UserProfilesApi
    {
        private readonly string baseUrl;

        internal UserProfilesApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can't construct user profile service! BaseUrl parameter is null!");

            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> GetUserProfile(string @namespace, string userAccessToken,
            ResultCallback<UserProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user profile! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user profile! UserAccessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/basic/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserProfile> result = response.TryParseJsonBody<UserProfile>();
            callback.Try(result);
        }

        public IEnumerator<ITask> CreateUserProfile(string @namespace, string userAccessToken,
            CreateUserProfileRequest createRequest, ResultCallback<UserProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't create user profile! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't create user profile! UserAccessToken parameter is null!");
            Assert.IsNotNull(createRequest, "Can't create user profile! CreateRequest parameter is null!");
            Assert.IsNotNull(
                createRequest.language,
                "Can't create user profile! CreateRequest.language parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/basic/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(SimpleJson.SimpleJson.SerializeObject(createRequest))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserProfile> result = response.TryParseJsonBody<UserProfile>();
            callback.Try(result);
        }

        public IEnumerator<ITask> UpdateUserProfile(string @namespace, string userAccessToken,
            UpdateUserProfileRequest updateRequest, ResultCallback<UserProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't update user profile! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't update user profile! UserAccessToken parameter is null!");
            Assert.IsNotNull(updateRequest, "Can't update user profile! ProfileRequest parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/basic/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(SimpleJson.SimpleJson.SerializeObject(updateRequest))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<UserProfile> result = response.TryParseJsonBody<UserProfile>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserProfilePublicInfo(string @namespace, string userId, string userAccessToken,
            ResultCallback<PublicUserProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user profile public info! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user profile public info! userId parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get user profile public info! UserAccessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/basic/public/namespaces/{namespace}/users/{user_id}/profiles/public")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("user_id", userId)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<PublicUserProfile> result = response.TryParseJsonBody<PublicUserProfile>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetUserProfilePublicInfosByIds(string @namespace, string userAccessToken,
            string[] userIds, ResultCallback<PublicUserProfile[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get user profile public info by ids! Namespace parameter is null!");
            Assert.IsNotNull(
                userAccessToken,
                "Can't get user profile public info by ids! UserAccessToken parameter is null!");
            Assert.IsNotNull(userIds, "Can't get user profile info by ids! userIds parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/basic/public/namespaces/{namespace}/profiles/public")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("userIds", string.Join(",", userIds))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<PublicUserProfile[]> result = response.TryParseJsonBody<PublicUserProfile[]>();
            callback.Try(result);
        }

        public IEnumerator<ITask> GetTimeZones(string @namespace, string userAccessToken,
            ResultCallback<string[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get time zones! Namespace parameter is null!");
            Assert.IsNotNull(userAccessToken, "Can't get time zones! UserAccessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/basic/public/namespaces/{namespace}/misc/timezones")
                .WithPathParam("namespace", @namespace)
                .WithBearerAuth(userAccessToken)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            Result<string[]> result = response.TryParseJsonBody<string[]>();
            callback.Try(result);
        }
    }
}