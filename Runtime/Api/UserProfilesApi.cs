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
    public class UserProfilesApi : ApiBase
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

        public IEnumerator GetUserProfilePublicInfoByPublicId(string publicId, ResultCallback<PublicUserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(publicId, "publicId parameter is null!");
            Assert.IsNotNull(Namespace_, "Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/profiles/public/byPublicId")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("publicId", publicId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PublicUserProfile>();
            callback.Try(result);
        }

        public IEnumerator CreateUserProfile(string userId
            , string language
            , Dictionary<string, object> customAttributes
            , string timezone
            , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't create user profile! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update user profile! userId parameter is null!");
            Assert.IsNotNull(language, "Can't update user profile! language parameter is null!");
            Assert.IsNotNull(customAttributes, "Can't update user profile! customAttributes parameter is null!");
            Assert.IsNotNull(timezone, "Can't update user profile! timezone parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create user profile! accessToken parameter is null!"); 

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(new {
                    language = language,
                    customAttributes = customAttributes,
                    timezone = timezone,
                }.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();

            callback.Try(result);
        }

        public IEnumerator UpdateUserProfile(string userId
            , string language 
            , string timezone
            , Dictionary<string, object> customAttributes
            , string zipCode 
            , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't update user profile! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update user profile! userId parameter is null!");
            Assert.IsNotNull(language, "Can't update user profile! language parameter is null!");
            Assert.IsNotNull(timezone, "Can't update user profile! timezone parameter is null!");
            Assert.IsNotNull(customAttributes, "Can't update user profile! customAttributes parameter is null!");
            Assert.IsNotNull(zipCode, "Can't update user profile! zipCode parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't update user profile! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(new {
                    language = language,
                    timezone = timezone,
                    customAttributes = customAttributes,
                    zipCode = zipCode
                }.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();
            callback.Try(result);
        }


        public IEnumerator GetUserProfile(string userId 
          , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't update user profile! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update user profile! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't update user profile! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson) 
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<UserProfile>();
            callback.Try(result);
        }

        public IEnumerator GenerateUploadURL(string folder
          , FileType filetype
          , ResultCallback<GenerateUploadURLResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Generate Upload URL! Namespace parameter is null!");
            Assert.IsNotNull(folder, "Can't Generate Upload URL! folder parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Generate Upload URL! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/folders/{folder}/files")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("folder", folder)
                .WithQueryParam("fileType", filetype.ToString().ToLower())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GenerateUploadURLResult>();

            callback.Try(result);
        }

        public IEnumerator GenerateUploadURLForUserContent(string userId
            , FileType filetype
            , ResultCallback<GenerateUploadURLResult> callback
            , UploadCategory category = UploadCategory.DEFAULT)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't Generate Upload URL for User Content! Namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't Generate Upload URL for User Content! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't Generate Upload URL for User Content! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/files")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithQueryParam("fileType", filetype.ToString().ToLower())
                .WithQueryParam("category", category.ToString().ToLower())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<GenerateUploadURLResult>();

            callback.Try(result);
        }
        
        public IEnumerator GetPrivateCustomAttributes(
            ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get custom attributes! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get custom attributes! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles/privateCustomAttributes")
                .WithPathParam("namespace", Namespace_)
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
        
        public IEnumerator UpdatePrivateCustomAttributes(Dictionary<string, object> updates
            , ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(Namespace_, "Can't get custom attributes! Namespace parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get custom attributes! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles/privateCustomAttributes")
                .WithPathParam("namespace", Namespace_)
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
    }
}
