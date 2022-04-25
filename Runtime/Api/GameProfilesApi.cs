// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class GameProfilesApi
    {
        #region Fields 

        private readonly string baseUrl;
        private readonly IHttpClient httpClient;
        #endregion

        #region Constructor

        internal GameProfilesApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        #endregion

        #region Public Methods

        public IEnumerator BatchGetGameProfiles(string @namespace, ICollection<string> userIds, string accessToken,
            ResultCallback<UserGameProfiles[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get all game profiles! namespace parameter is null!");
            Assert.IsNotNull(userIds, "Can't get all game profiles! userIds parameter is null!");
            Assert.IsNotNull(accessToken, "Can't all game profiles! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/profiles")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("userIds", userIds)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserGameProfiles[]>();

            callback.Try(result);
        }

        public IEnumerator GetAllGameProfiles(string @namespace, string userId, string accessToken,
            ResultCallback<GameProfile[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get all game profiles! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get all game profiles! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't all game profiles! accessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameProfile[]>();

            callback.Try(result);
        }

        public IEnumerator CreateGameProfile(string @namespace, string userId, string accessToken,
            GameProfileRequest gameProfile, ResultCallback<GameProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't create a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create a game profile! accessToken parameter is null!");
            Assert.IsNotNull(gameProfile, "Can't create a game profile! gameProfile parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(gameProfile.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }

        public IEnumerator GetGameProfile(string @namespace, string userId, string accessToken, string profileId,
            ResultCallback<GameProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get a game profile! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get a game profile! profileId parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }

        public IEnumerator UpdateGameProfile(string @namespace, string userId, string accessToken, string profileId,
            GameProfileRequest gameProfile, ResultCallback<GameProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't update a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't update a game profile! accessToken parameter is null!");
            Assert.IsNotNull(gameProfile, "Can't update a game profile! gameProfile parameter is null!");
            Assert.IsNotNull(profileId, "Can't update a game profile! gameProfile.profileId is null!");

            var request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(gameProfile.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }

        public IEnumerator DeleteGameProfile(string @namespace, string userId, string accessToken, string profileId,
            ResultCallback callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't delete a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't delete a game profile! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't delete a game profile! fileSection parameter is null!");


            var request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator GetGameProfileAtrribute(string @namespace, string userId, string accessToken,
            string profileId, string attributeName, ResultCallback<GameProfileAttribute> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get a game profile attribute! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get a game profile attribute! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get a game profile attribute! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get a game profile attribute! profileId parameter is null!");
            Assert.IsNotNull(attributeName, "Can't get a game profile attribute! attributeName parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(
                    this.baseUrl +
                    "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/attributes/{attributeName}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithPathParam("attributeName", attributeName)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameProfileAttribute>();

            callback.Try(result);
        }

        public IEnumerator UpdateGameProfileAtrribute(string @namespace, string userId, string accessToken,
            string profileId, GameProfileAttribute attribute, ResultCallback<GameProfile> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't update a game profile attribute! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a game profile attribute! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't update a game profile attribute! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't update a game profile attribute! profileId parameter is null!");
            Assert.IsNotNull(attribute, "Can't update a game profile attribute! attribute parameter is null!");
            Assert.IsNotNull(attribute.name, "Can't update a game profile attribute! attribute.name is null!");

            var request = HttpRequestBuilder
                .CreatePut(
                    this.baseUrl +
                    "/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/attributes/{attributeName}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithPathParam("attributeName", attribute.name)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(attribute.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<GameProfile>();

            callback.Try(result);
        }

        #endregion
    }
}