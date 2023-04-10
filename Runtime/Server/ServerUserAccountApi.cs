// Copyright (c) 2021 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal class ServerUserAccountApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param> // TODO: Should this base BaseUrl?
        /// <param name="session"></param>
        internal ServerUserAccountApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.BaseUrl, session )
        {
        }

        public IEnumerator GetUserData( string userAuthToken
            , ResultCallback<UserData> callback )
        {
            Assert.IsFalse(string.IsNullOrEmpty(userAuthToken), 
                "Parameter " + nameof(userAuthToken) + " is null or empty");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/public/users/me")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(userAuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator SearchUserOtherPlatformDisplayName( string platformDisplayName
            , PlatformType platformType
            , ResultCallback<PagedUserOtherPlatformInfo> callback
            , int limit
            , int offset )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(platformDisplayName, nameof(platformDisplayName) + " cannot be null");

            if (platformType == PlatformType.Nintendo || platformType == PlatformType.Oculus || platformType == PlatformType.Apple)
            {
                Debug.Log("Can not search user using this function. Use SearchUserOtherPlatformUserId instead.");
                yield break;
            }

            string searchBy = "thirdPartyPlatform";
            string platformBy = "platformDisplayName";

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace_}/users/search")
                .WithPathParam("namespace_", Namespace_)
                .WithQueryParam("by", searchBy)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("platformBy", platformBy)
                .WithQueryParam("platformId", platformType.ToString().ToLower())
                .WithQueryParam("query", platformDisplayName)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PagedUserOtherPlatformInfo>();
            callback.Try(result);
        }

        public IEnumerator SearchUserOtherPlatformUserId( string platformUserId
            , PlatformType platformType
            , ResultCallback<UserOtherPlatformInfo> callback )
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(platformUserId, nameof(platformUserId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace_}/platforms/{platformId}/users/{platformUserId}")
                .WithPathParam("namespace_", Namespace_)
                .WithPathParam("platformId", platformType.ToString().ToLower())
                .WithPathParam("platformUserId", platformUserId)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserOtherPlatformInfo>();
            callback.Try(result);
        }

        public IEnumerator BanUser(string userId, BanCreateRequest banRequest, ResultCallback<UserBanResponseV3> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(banRequest, nameof(banRequest) + " cannot be null");

            var request = HttpRequestBuilder
               .CreatePost(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}/bans")
               .WithPathParam("namespace", Namespace_)
               .WithPathParam("userId", userId)
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(banRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanResponseV3>();
            callback.Try(result);
        }

        public IEnumerator ChangeUserBanStatus(string userId, string banId, bool enabled, ResultCallback<UserBanResponseV3> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");
            Assert.IsNotNull(banId, nameof(banId) + " cannot be null");

            UserEnableBan changeRequest = new UserEnableBan { enabled = enabled };

            var request = HttpRequestBuilder
               .CreatePatch(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}/bans/{banId}")
               .WithPathParam("namespace", Namespace_)
               .WithPathParam("userId", userId)
               .WithPathParam("banId", banId)
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(changeRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanResponseV3>();
            callback.Try(result);
        }

        public IEnumerator GetUserBanInfo(string userId, bool activeOnly, ResultCallback<UserBanPagedList> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");

            var request = HttpRequestBuilder
               .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/{userId}/bans")
               .WithPathParam("namespace", Namespace_)
               .WithPathParam("userId", userId)
               .WithQueryParam("activeOnly", activeOnly ? "true" : "false")
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanPagedList>();
            callback.Try(result);
        }

        public IEnumerator GetUserBannedList(bool activeOnly, BanType banType, int offset, int limit, ResultCallback<UserBanPagedList> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null"); 

            var request = HttpRequestBuilder
               .CreateGet(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/bans/users")
               .WithPathParam("namespace", Namespace_)
               .WithQueryParam("activeOnly", activeOnly ? "true" : "false")
               .WithQueryParam("banType", banType.ToString())
               .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
               .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<UserBanPagedList>();
            callback.Try(result);
        }

        public IEnumerator GetUserByUserId(string userId 
            , ResultCallback<UserData> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(userId, nameof(userId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/iam/v3/namespaces/{namespace}/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }

        public IEnumerator ListUserByUserId(ListUserDataRequest listUserDataRequest, ResultCallback<ListUserDataResponse> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(listUserDataRequest, nameof(listUserDataRequest) + " cannot be null");

            var request = HttpRequestBuilder
               .CreatePost(BaseUrl + "/iam/v3/admin/namespaces/{namespace}/users/bulk")
               .WithPathParam("namespace", Namespace_)
               .WithBearerAuth(AuthToken)
               .WithContentType(MediaType.ApplicationJson)
               .Accepts(MediaType.ApplicationJson)
               .WithBody(listUserDataRequest.ToUtf8Json())
               .GetResult();

            IHttpResponse response = null;
            yield return HttpClient.SendRequest(request, rsp => response = rsp);
            var result = response.TryParseJson<ListUserDataResponse>();
            callback.Try(result);
        }
    }
}