// Copyright (c) 2018 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class UserProfilesApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BasicServerUrl</param>
        /// <param name="session"></param>
        internal UserProfilesApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.BasicServerUrl, session )
        {
        }

        public IEnumerator GetUserProfile( ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get user profile! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get user profile! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();

            callback.Try(result);
        }

        public IEnumerator CreateUserProfile( CreateUserProfileRequest createRequest
            , ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create user profile! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create user profile! accessToken parameter is null!");
            Assert.IsNotNull(createRequest, "Can't create user profile! CreateRequest parameter is null!");
            Assert.IsNotNull(
                createRequest.language,
                "Can't create user profile! CreateRequest.language parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(createRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();

            callback.Try(result);
        }

        public IEnumerator UpdateUserProfile( UpdateUserProfileRequest updateRequest
            , ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't update user profile! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't update user profile! accessToken parameter is null!");
            Assert.IsNotNull(updateRequest, "Can't update user profile! ProfileRequest parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(updateRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();
            callback.Try(result);
        }

        public IEnumerator GetUserProfilePublicInfo( string userId
            , ResultCallback<PublicUserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get user profile public info! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get user profile public info! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get user profile public info! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PublicUserProfile>();
            callback.Try(result);
        }

        public IEnumerator GetUserProfilePublicInfosByIds( string[] userIds
            , ResultCallback<PublicUserProfile[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get user profile public info by ids! Namespace parameter is null!");
            Assert.IsNotNull(
                AuthToken,
                "Can't get user profile public info by ids! accessToken parameter is null!");

            Assert.IsNotNull(userIds, "Can't get user profile info by ids! userIds parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/profiles/public")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithQueryParam("userIds", string.Join(",", userIds))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<PublicUserProfile[]>();
            callback.Try(result);
        }

        public IEnumerator GetCustomAttributes( string userId
            , ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get custom attributes! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get custom attributes! userId is null!");
            Assert.IsNotNull(AuthToken, "Can't get custom attributes! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles/customAttributes")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Dictionary<string, object>>();
            callback.Try(result);
        }

        public IEnumerator UpdateCustomAttributes( string userId
            , Dictionary<string, object> updates
            , ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get custom attributes! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get custom attributes! userId is null!");
            Assert.IsNotNull(AuthToken, "Can't get custom attributes! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles/customAttributes")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(updates.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<Dictionary<string, object>>();
            callback.Try(result);
        }

        public IEnumerator GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get time zones! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get time zones! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/misc/timezones")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<string[]>();
            callback.Try(result);
        }
		
    }
}
