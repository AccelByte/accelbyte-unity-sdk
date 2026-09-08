// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Linq;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class GdprApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==GdprServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal GdprApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.GdprServerUrl, session)
        {
        }

        public IEnumerator SubmitAccountDeletion(string accessToken
          , string userId
          , string password
          , ResultCallback<SubmitAccountDeletionResponse> callback)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(accessToken) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(userId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(userId) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(password))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(password) + " cannot be null or empty"));
                yield break;
            } 

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/deletions")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithFormParam("password", password)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<SubmitAccountDeletionResponse>();

            callback?.Try(result);
        } 

        public IEnumerator SubmitAccountDeletionPlatformId(string accessToken
           , string platformId
           , string platformToken
           , ResultCallback<SubmitAccountDeletionResponse> callback)
        { 
            if (string.IsNullOrEmpty(accessToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(accessToken) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(platformId))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(platformId) + " cannot be null or empty"));
                yield break;
            }

            if (string.IsNullOrEmpty(platformToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(platformToken) + " cannot be null or empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/users/me/deletions")
                .WithFormParam("platformId", platformId)
                .WithFormParam("platformToken", platformToken)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<SubmitAccountDeletionResponse>();

            callback?.Try(result);
        }

        public IEnumerator CancelAccountDeletion(string accessToken
           , ResultCallback callback)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(accessToken) + " cannot be null or empty"));
                yield break;
            } 

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/public/users/me/deletions")
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParse();

            callback?.Try(result);
        }

        public IEnumerator GetAccountDeletionStatus(string accessToken
       , ResultCallback<AccountDeletionStatusResponse> callback)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                callback?.TryError(new Error(ErrorCode.BadRequest, nameof(accessToken) + " cannot be null or empty"));
                yield break;
            }
             
            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/users/me/deletions/status")
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => 
                { 
                    response = rsp; 
                });

            var result = response.TryParseJson<AccountDeletionStatusResponse>();

            callback?.Try(result);
        }

        internal void SubmitMyPersonalDataRequest(string platformId
            , string platformToken
            , SubmitMyPersonalDataRequestOptionalParameters optionalParameters
            , ResultCallback<SubmitPersonalDataRequestResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, platformId, platformToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/users/me/requests")
                .WithFormParam("platformId", platformId)
                .WithFormParam("platformToken", platformToken)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                if (!string.IsNullOrEmpty(optionalParameters.Email))
                {
                    requestBuilder.WithFormParam("email", optionalParameters.Email);
                }

                if (!string.IsNullOrEmpty(optionalParameters.LanguageTag))
                {
                    requestBuilder.WithFormParam("languageTag", optionalParameters.LanguageTag);
                }
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<SubmitPersonalDataRequestResponse>();
                    callback?.Try(result);
                });
        }

        internal void GetMyPersonalDataRequests(GetMyPersonalDataRequestsOptionalParameters optionalParameters
            , ResultCallback<PersonalDataRequestsResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/users/me/requests")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset"
                        , optionalParameters.Offset == null ? "0" : optionalParameters.Offset.ToString())
                    .WithQueryParam("limit"
                        , optionalParameters.Limit == null ? "20" : optionalParameters.Limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParseJson<PersonalDataRequestsResponse>();
                    callback?.Try(result);
                });
        }

        internal void CancelMyPersonalDataRequest(DateTime requestDate
            , CancelMyPersonalDataRequestOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            // WithPathParam Uri-escapes the value, so pass the raw ISO 8601 string.
            string requestDateIso = requestDate
                .ToUniversalTime()
                .ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/public/users/me/requests/{requestDate}")
                .WithPathParam("requestDate", requestDateIso)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
                AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
                , request
                , response =>
                {
                    var result = response.TryParse();
                    callback?.Try(result);
                });
        }
    }
}
