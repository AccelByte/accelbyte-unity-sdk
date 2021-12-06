// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerUserAccountApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        internal ServerUserAccountApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, nameof(baseUrl) + "is null");
            Assert.IsNotNull(httpClient, nameof(httpClient) + "is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator GetUserData(string userAuthToken, ResultCallback<UserData> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(userAuthToken), "Parameter " + nameof(userAuthToken) + " is null or empty");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/iam/v3/public/users/me")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(userAuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator SearchUserOtherPlatformDisplayName(string namespace_, string accessToken, string platformDisplayName,
                    PlatformType platformType, ResultCallback<PagedUserOtherPlatformInfo> callback, int limit, int offset)
        {
            Assert.IsNotNull(namespace_, nameof(namespace_) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(platformDisplayName, nameof(platformDisplayName) + " cannot be null");

            if (platformType == PlatformType.Nintendo || platformType == PlatformType.Oculus || platformType == PlatformType.Apple)
            {
                Debug.Log("Can not search user using this function. Use SearchUserOtherPlatformUserId instead.");
                yield break;
            }

            string searchBy = "thirdPartyPlatform";
            string platformBy = "platformDisplayName";

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/iam/v3/admin/namespaces/{namespace_}/users/search")
                .WithPathParam("namespace_", namespace_)
                .WithQueryParam("by", searchBy)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("platformBy", platformBy)
                .WithQueryParam("platformId", platformType.ToString().ToLower())
                .WithQueryParam("query", platformDisplayName)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<PagedUserOtherPlatformInfo>();
            callback.Try(result);
        }

        public IEnumerator SearchUserOtherPlatformUserId(string namespace_, string accessToken, string platformUserId,
                    PlatformType platformType, ResultCallback<UserOtherPlatformInfo> callback)
        {
            Assert.IsNotNull(namespace_, nameof(namespace_) + " cannot be null");
            Assert.IsNotNull(accessToken, nameof(accessToken) + " cannot be null");
            Assert.IsNotNull(platformUserId, nameof(platformUserId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/iam/v3/admin/namespaces/{namespace_}/platforms/{platformId}/users/{platformUserId}")
                .WithPathParam("namespace_", namespace_)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithPathParam("platformUserId", platformUserId)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserOtherPlatformInfo>();
            callback.Try(result);
        }
    }
}