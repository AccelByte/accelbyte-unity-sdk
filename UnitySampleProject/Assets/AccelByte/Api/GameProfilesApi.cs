// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.Net;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class GameProfilesApi
    {
        private readonly string baseUrl;

        internal GameProfilesApi(string baseUrl)
        {
            Assert.IsNotNull(baseUrl, "Can not construct GameProfile service; baseUrl is null!");
            this.baseUrl = baseUrl;
        }

        public IEnumerator<ITask> BatchGetGameProfiles(string @namespace, ICollection<string> userIds, string accessToken,
            ResultCallback<UserGameProfiles[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get all game profiles! namespace parameter is null!");
            Assert.IsNotNull(userIds, "Can't get all game profiles! userIds parameter is null!");
            Assert.IsNotNull(accessToken, "Can't all game profiles! accessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/profiles")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("userIds", userIds)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<UserGameProfiles[]>.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result<UserGameProfiles[]> result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        UserGameProfiles[] gameProfiles = SimpleJson.SimpleJson.DeserializeObject<UserGameProfiles[]>(responseText);
                        result = Result<UserGameProfiles[]>.CreateOk(gameProfiles);
                    }
                    catch (ArgumentException ex)
                    {
                        result = Result<UserGameProfiles[]>.CreateError(ErrorCode.InvalidResponse,
                            "Batch get game profiles failed to deserialize response body: " + ex.Message);
                    }

                    break;

                case HttpStatusCode.NotFound:
                    result = Result<UserGameProfiles[]>.CreateError(ErrorCode.CategoryNotFound,
                        "Batch get game profiles failed due to the resource not found");
                    break;

                default:
                    result = Result<UserGameProfiles[]>.CreateError((ErrorCode)response.StatusCode,
                        "Batch get game profiles failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> GetAllGameProfiles(string @namespace, string userId, string accessToken,
            ResultCallback<GameProfile[]> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get all game profiles! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get all game profiles! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't all game profiles! accessToken parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<GameProfile[]>.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result<GameProfile[]> result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        GameProfile[] gameProfiles = SimpleJson.SimpleJson.DeserializeObject<GameProfile[]>(responseText);
                        result = Result<GameProfile[]>.CreateOk(gameProfiles);
                    }
                    catch (ArgumentException ex)
                    {
                        result = Result<GameProfile[]>.CreateError(ErrorCode.InvalidResponse,
                            "Get game profiles failed to deserialize response body: " + ex.Message);
                    }

                    break;

                case HttpStatusCode.NotFound:
                    result = Result<GameProfile[]>.CreateError(ErrorCode.CategoryNotFound,
                        "Get game profiles failed due to the resource not found");
                    break;

                default:
                    result = Result<GameProfile[]>.CreateError((ErrorCode)response.StatusCode,
                        "Get game profiles failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> CreateGameProfile(string @namespace, string userId, string accessToken,
            GameProfileRequest gameProfile, ResultCallback<GameProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't create a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't create a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create a game profile! accessToken parameter is null!");
            Assert.IsNotNull(gameProfile, "Can't create a game profile! gameProfile parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/users/{userId}/profiles")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(SimpleJson.SimpleJson.SerializeObject(gameProfile))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result<GameProfile> result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    try
                    {
                        GameProfile gameProfileResult = SimpleJson.SimpleJson.DeserializeObject<GameProfile>(responseText);
                        result = Result<GameProfile>.CreateOk(gameProfileResult);
                    }
                    catch (ArgumentException ex)
                    {
                        result = Result<GameProfile>.CreateError(ErrorCode.InvalidResponse,
                            "Create game profile failed to deserialize response body: " + ex.Message);
                    }
                    break;

                case HttpStatusCode.NotFound:
                    result = Result<GameProfile>.CreateError(ErrorCode.NotFound,
                        "Create game profile failed due to the resource not found");
                    break;

                default:
                    result = Result<GameProfile>.CreateError((ErrorCode)response.StatusCode,
                        "Create game profile failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> GetGameProfile(string @namespace, string userId, string accessToken,
            string profileId, ResultCallback<GameProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get a game profile! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get a game profile! profileId parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result<GameProfile> result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        GameProfile gameProfile = SimpleJson.SimpleJson.DeserializeObject<GameProfile>(responseText);
                        result = Result<GameProfile>.CreateOk(gameProfile);
                    }
                    catch (ArgumentException ex)
                    {
                        result = Result<GameProfile>.CreateError(ErrorCode.InvalidResponse,
                            "Get game profile failed to deserialize response body: " + ex.Message);
                    }

                    break;

                case HttpStatusCode.NotFound:
                    result = Result<GameProfile>.CreateError(ErrorCode.CategoryNotFound,
                        "Get game profile failed due to the resource not found");
                    break;

                default:
                    result = Result<GameProfile>.CreateError((ErrorCode)response.StatusCode,
                        "Get game profile failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> UpdateGameProfile(string @namespace, string userId, string accessToken,
            string profileId, GameProfileRequest gameProfile, ResultCallback<GameProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't update a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't update a game profile! accessToken parameter is null!");
            Assert.IsNotNull(gameProfile, "Can't update a game profile! gameProfile parameter is null!");
            Assert.IsNotNull(profileId, "Can't update a game profile! gameProfile.profileId is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(SimpleJson.SimpleJson.SerializeObject(gameProfile))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result<GameProfile> result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        GameProfile gameProfileResult = SimpleJson.SimpleJson.DeserializeObject<GameProfile>(responseText);
                        result = Result<GameProfile>.CreateOk(gameProfileResult);
                    }
                    catch (ArgumentException ex)
                    {
                        result = Result<GameProfile>.CreateError(ErrorCode.InvalidResponse,
                            "Update game profile failed to deserialize response body: " + ex.Message);
                    }

                    break;

                case HttpStatusCode.NotFound:
                    result = Result<GameProfile>.CreateError(ErrorCode.NotFound,
                        "Update game profile failed due to the resource not found");
                    break;

                default:
                    result = Result<GameProfile>.CreateError((ErrorCode)response.StatusCode,
                        "Update game profile failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> DeleteGameProfile(string @namespace, string userId, string accessToken,
            string profileId, ResultCallback callback)
        {
            Assert.IsNotNull(@namespace, "Can't delete a game profile! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't delete a game profile! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't delete a game profile! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't delete a game profile! fileSection parameter is null!");


            HttpWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                    result = Result.CreateOk();
                    break;

                case HttpStatusCode.NotFound:
                    result = Result.CreateError(ErrorCode.NotFound,
                        "Delete game profile failed due to the resource not found");
                    break;

                default:
                    result = Result.CreateError((ErrorCode)response.StatusCode,
                        "Delete game profile failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> GetGameProfileAtrribute(string @namespace, string userId, string accessToken,
            string profileId, string attributeName, ResultCallback<GameProfileAttribute> callback)
        {
            Assert.IsNotNull(@namespace, "Can't get a game profile attribute! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't get a game profile attribute! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get a game profile attribute! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't get a game profile attribute! profileId parameter is null!");
            Assert.IsNotNull(attributeName, "Can't get a game profile attribute! attributeName parameter is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/attributes/{attributeName}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithPathParam("attributeName", attributeName)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<GameProfileAttribute>.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result<GameProfileAttribute> result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        GameProfileAttribute gameProfile = SimpleJson.SimpleJson.DeserializeObject<GameProfileAttribute>(responseText);
                        result = Result<GameProfileAttribute>.CreateOk(gameProfile);
                    }
                    catch (ArgumentException ex)
                    {
                        result = Result<GameProfileAttribute>.CreateError(ErrorCode.InvalidResponse,
                            "Get game profile attribute failed to deserialize response body: " + ex.Message);
                    }

                    break;

                case HttpStatusCode.NotFound:
                    result = Result<GameProfileAttribute>.CreateError(ErrorCode.CategoryNotFound,
                        "Get game profile attribute failed due to the resource not found");
                    break;

                default:
                    result = Result<GameProfileAttribute>.CreateError((ErrorCode)response.StatusCode,
                        "Get game profile attribute failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }

        public IEnumerator<ITask> UpdateGameProfileAtrribute(string @namespace, string userId, string accessToken,
            string profileId, GameProfileAttribute attribute, ResultCallback<GameProfile> callback)
        {
            Assert.IsNotNull(@namespace, "Can't update a game profile attribute! namespace parameter is null!");
            Assert.IsNotNull(userId, "Can't update a game profile attribute! userId parameter is null!");
            Assert.IsNotNull(accessToken, "Can't update a game profile attribute! accessToken parameter is null!");
            Assert.IsNotNull(profileId, "Can't update a game profile attribute! profileId parameter is null!");
            Assert.IsNotNull(attribute, "Can't update a game profile attribute! attribute parameter is null!");
            Assert.IsNotNull(attribute.name, "Can't update a game profile attribute! attribute.name is null!");

            HttpWebRequest request = HttpRequestBuilder
                .CreatePut(this.baseUrl + "/soc-profile/public/namespaces/{namespace}/users/{userId}/profiles/{profileId}/attributes/{attributeName}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("userId", userId)
                .WithPathParam("profileId", profileId)
                .WithPathParam("attributeName", attribute.name)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(SimpleJson.SimpleJson.SerializeObject(attribute))
                .ToRequest();

            HttpWebResponse response = null;

            yield return Task.Await(request, rsp => response = rsp);

            if (response == null)
            {
                callback.Try(Result<GameProfile>.CreateError(ErrorCode.NetworkError, "There is no response"));
                yield break;
            }

            string responseText = response.GetBodyText();
            response.Close();
            Result<GameProfile> result;

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    try
                    {
                        GameProfile gameProfile = SimpleJson.SimpleJson.DeserializeObject<GameProfile>(responseText);
                        result = Result<GameProfile>.CreateOk(gameProfile);
                    }
                    catch (ArgumentException ex)
                    {
                        result = Result<GameProfile>.CreateError(ErrorCode.InvalidResponse,
                            "Update game profile attribute failed to deserialize response body: " + ex.Message);
                    }

                    break;

                case HttpStatusCode.NotFound:
                    result = Result<GameProfile>.CreateError(ErrorCode.CategoryNotFound,
                        "Update game profile attribute failed due to the resource not found");
                    break;

                default:
                    result = Result<GameProfile>.CreateError((ErrorCode)response.StatusCode,
                        "Update game profile attribute failed with status: " + response.StatusCode);
                    break;
            }

            callback.Try(result);
        }
    }
}