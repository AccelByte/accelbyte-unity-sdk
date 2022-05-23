// Copyright (c) 2019 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
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
            Assert.IsNotNull(Namespace_, "Can't get all game profiles! namespace parameter is null!");
            Assert.IsNotNull(userIds, "Can't get all game profiles! userIds parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't all game profiles! accessToken parameter is null!");

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
            Assert.IsNotNull(Namespace_, "Can't get all game profiles! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get all game profiles! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't all game profiles! accessToken parameter is null!");

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
            Assert.IsNotNull(Namespace_, "Can't create a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a game profile! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't create a game profile! accessToken parameter is null!");
            Assert.IsNotNull(gameProfile, "Can't create a game profile! gameProfile parameter is null!");

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
            Assert.IsNotNull(Namespace_, "Can't get a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get a game profile! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get a game profile! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get a game profile! profileId parameter is null!");

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
            Assert.IsNotNull(Namespace_, "Can't update a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a game profile! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't update a game profile! accessToken parameter is null!");
            Assert.IsNotNull(gameProfile, "Can't update a game profile! gameProfile parameter is null!");
            Assert.IsNotNull(profileId, "Can't update a game profile! gameProfile.profileId is null!");

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
            Assert.IsNotNull(Namespace_, "Can't delete a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete a game profile! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't delete a game profile! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't delete a game profile! fileSection parameter is null!");


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
            Assert.IsNotNull(Namespace_, "Can't get a game profile attribute! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get a game profile attribute! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't get a game profile attribute! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get a game profile attribute! profileId parameter is null!");
            Assert.IsNotNull(attributeName, "Can't get a game profile attribute! attributeName parameter is null!");

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
            Assert.IsNotNull(Namespace_, "Can't update a game profile attribute! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a game profile attribute! userId parameter is null!");
            Assert.IsNotNull(AuthToken, "Can't update a game profile attribute! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't update a game profile attribute! profileId parameter is null!");
            Assert.IsNotNull(attribute, "Can't update a game profile attribute! attribute parameter is null!");
            Assert.IsNotNull(attribute.name, "Can't update a game profile attribute! attribute.name is null!");

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
