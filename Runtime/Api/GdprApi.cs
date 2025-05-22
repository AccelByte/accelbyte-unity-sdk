// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
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
    }
}
