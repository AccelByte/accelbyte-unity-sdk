// Copyright (c) 2019 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class GameProfilesApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==GameProfileServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal GameProfilesApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.GameProfileServerUrl, session )
        {
        }

        public IEnumerator BatchGetGameProfiles( ICollection<string> userIds
            , ResultCallback<UserGameProfiles[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userIds, AuthToken);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("userIds", userIds)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<UserGameProfiles[]>();

            callback.Try(result);
        }

        public IEnumerator GetAllGameProfiles( string userId
            , ResultCallback<GameProfile[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameProfile[]>();

            callback.Try(result);
        }

        public IEnumerator CreateGameProfile( string userId
            , GameProfileRequest gameProfile
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken, gameProfile);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(gameProfile.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }

        public IEnumerator GetGameProfile( string userId
            , string profileId
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken, profileId);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }

        public IEnumerator UpdateGameProfile( string userId
            , string profileId
            , GameProfileRequest gameProfile
            , ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , userId
                , AuthToken
                , gameProfile
                , profileId
            );
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(gameProfile.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }

        public IEnumerator DeleteGameProfile( string userId
            , string profileId
            , ResultCallback callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, userId, AuthToken, profileId);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }


            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator GetGameProfileAtrribute( string userId
            , string profileId
            , string attributeName
            , ResultCallback<GameProfileAttribute> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , userId
                , AuthToken
                , profileId
                , attributeName
            );
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(
                    BaseUrl +
                    "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/attributes/{attributeName}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithPathParam("attributeName", attributeName)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameProfileAttribute>();

            callback.Try(result);
        }

        public IEnumerator UpdateGameProfileAtrribute( string userId
            , string profileId
            , GameProfileAttribute attribute
            ,  ResultCallback<GameProfile> callback )
        {
            Report.GetFunctionLog(GetType().Name);
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , userId
                , AuthToken
                , profileId
                , attribute
                , attribute?.name
            );
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(
                    BaseUrl +
                    "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/attributes/{attributeName}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithPathParam("attributeName", attribute.name)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(attribute.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }
    }
}
