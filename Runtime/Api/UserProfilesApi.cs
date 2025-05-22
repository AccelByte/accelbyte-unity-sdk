// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Api
{
    public class UserProfilesApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BasicServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal UserProfilesApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.BasicServerUrl, session )
        {
        }

        public IEnumerator GetUserProfile( ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            GetUserProfile(optionalParameters: null, callback);
            yield break;
        }

        internal void GetUserProfile(GetUserProfileOptionalParameters optionalParameters, ResultCallback<UserProfile> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<UserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator GetUserProfile(string userId
          , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            GetUserProfile(userId, optionalParameters: null, callback);
            yield break;
        }

        internal void GetUserProfile(string userId, GetUserProfileOptionalParameters optionalParameters, ResultCallback<UserProfile> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<UserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator CreateUserProfile( CreateUserProfileRequest createRequest
            , ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            CreateUserProfile(createRequest, optionalParameters: null, callback);
            yield break;
        }

        internal void CreateUserProfile(
            CreateUserProfileRequest createRequest
            , CreateUserProfileOptionalParameters optionalParameters
            , ResultCallback<UserProfile> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, createRequest, createRequest?.language);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(createRequest.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<UserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator CreateUserProfile(string userId
            , CreateUserProfileRequest createRequest
            , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            CreateUserProfile(userId, createRequest, optionalParameters: null, callback);
            yield break;
        }
        
        internal void CreateUserProfile(string userId
            , CreateUserProfileRequest createRequest
            , CreateUserProfileOptionalParameters optionalParameters
            , ResultCallback<UserProfile> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , userId
                , createRequest
                , createRequest.language
                , createRequest.customAttributes
                , createRequest.timeZone
                , AuthToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(createRequest.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<UserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator UpdateUserProfile( UpdateUserProfileRequest updateRequest
            , ResultCallback<UserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            UpdateUserProfile(updateRequest, optionalParameters: null, callback);
            yield break;
        }

        internal void UpdateUserProfile(UpdateUserProfileRequest updateRequest, UpdateUserProfileOptionalParameters optionalParameters, ResultCallback<UserProfile> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, updateRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(updateRequest.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<UserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator UpdateUserProfile(string userId
            , UpdateUserProfileRequest updateRequest
            , ResultCallback<UserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            UpdateUserProfile(userId, updateRequest, optionalParameters: null, callback);
            yield break;
        }

        internal void UpdateUserProfile(string userId, UpdateUserProfileRequest updateRequest, UpdateUserProfileOptionalParameters optionalParameters, ResultCallback<UserProfile> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , userId
                , updateRequest
                , updateRequest.language
                , updateRequest.timeZone
                , updateRequest.customAttributes
                , updateRequest.zipCode
                , AuthToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(updateRequest.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<UserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator GetUserProfilePublicInfo( string userId
            , ResultCallback<PublicUserProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            GetUserProfilePublicInfo(userId, optionalParameters: null, callback);
            yield break;
        }

        internal void GetUserProfilePublicInfo(string userId, GetPublicUserProfileOptionalParameter optionalParameters, ResultCallback<PublicUserProfile> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles/public")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<PublicUserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator GetUserProfilePublicInfosByIds( string[] userIds
            , ResultCallback<PublicUserProfile[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, userIds);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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
            callback?.Try(result);
        }

        public IEnumerator GetCustomAttributes( string userId
            , ResultCallback<Dictionary<string, object>> callback )
        {
            var error = new Dictionary<string, object>
            {
                { "This endpoint is not able to use since give a security issue for other player/user", "use GetUserProfilePublicInfo instead" }
            };
            callback?.TryOk(error);

            yield break;
        }

        public IEnumerator UpdateCustomAttributes( string userId
            , Dictionary<string, object> updates
            , ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            UpdateCustomAttributes(userId, updates, optionalParameters: null, callback);
            yield break;
        }

        internal void UpdateCustomAttributes(string sessionUserId
            , Dictionary<string, object> updates
            , UpdateCustomAttributesOptionalParameters optionalParameters
            , ResultCallback<Dictionary<string, object>> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, sessionUserId, updates, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/{userId}/profiles/customAttributes")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", sessionUserId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(updates.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<Dictionary<string, object>>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator GetTimeZones( ResultCallback<string[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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
            callback?.Try(result);
        }

        public IEnumerator GetUserProfilePublicInfoByPublicId(string publicId, ResultCallback<PublicUserProfile> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            GetUserProfilePublicInfoByPublicId(publicId, optionalParameters: null, callback);
            yield break;
        }

        internal void GetUserProfilePublicInfoByPublicId(string publicId, GetUserProfilePublicInfoByPublicIdOptionalParameter optionalParameters, ResultCallback<PublicUserProfile> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(publicId, Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/profiles/public/byPublicId")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("publicId", publicId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<PublicUserProfile>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator GenerateUploadURL(string folder
          , FileType filetype
          , ResultCallback<GenerateUploadURLResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            GenerateUploadURL(folder, filetype, optionalParameters: null, callback);
            yield break;
        }

        internal void GenerateUploadURL(string folder, FileType fileType, GenerateUploadURLOptionalParameter optionalParameters, ResultCallback<GenerateUploadURLResult> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, folder, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/folders/{folder}/files")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("folder", folder)
                .WithQueryParam("fileType", fileType.ToString().ToLower())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<GenerateUploadURLResult>();
                    callback?.Try(result);   
                });
        }

        public IEnumerator GenerateUploadURLForUserContent(string userId
            , FileType filetype
            , ResultCallback<GenerateUploadURLResult> callback
            , UploadCategory category = UploadCategory.DEFAULT)
        {
            Report.GetFunctionLog(GetType().Name);
            GenerateUploadURLForUserContent(userId, filetype, category, optionalParameters: null, callback);
            yield break;
        }

        internal void GenerateUploadURLForUserContent(string userId
            , FileType filetype
            , UploadCategory category
            , GenerateUploadURLForUserContentOptionalParameter optionalParameters
            , ResultCallback<GenerateUploadURLResult> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }
            
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

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<GenerateUploadURLResult>();
                    callback?.Try(result);
                });
        }
        
        public IEnumerator GetPrivateCustomAttributes(
            ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            
            GetPrivateCustomAttributes(optionalParameters: null, callback: callback);
            yield break;
        }
        
        internal void GetPrivateCustomAttributes(
            GetPrivateCustomAttributesOptionalParameter optionalParameters
            , ResultCallback<Dictionary<string, object>> callback
            )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles/privateCustomAttributes")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<Dictionary<string, object>>();
                    callback?.Try(result); 
                });
        }
        
        public IEnumerator UpdatePrivateCustomAttributes(Dictionary<string, object> updates
            , ResultCallback<Dictionary<string, object>> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            
            UpdatePrivateCustomAttributes(updates, optionalParameters: null, callback: callback);
            yield break;
        }
        
        internal void UpdatePrivateCustomAttributes(
            Dictionary<string, object> updates
            , UpdatePrivateCustomAttributesOptionalParameter optionalParameters
            , ResultCallback<Dictionary<string, object>> callback )
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(updates, Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/profiles/privateCustomAttributes")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(updates.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                ,response =>
                {
                    var result = response.TryParseJson<Dictionary<string, object>>();
                    callback?.Try(result);   
                });
        }
    }
}
