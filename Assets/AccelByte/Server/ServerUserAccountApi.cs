﻿// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerUserAccountApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;

        internal ServerUserAccountApi(string baseUrl, IHttpWorker httpWorker)
        {
            Assert.IsNotNull(baseUrl, nameof(baseUrl) + "is null");
            Assert.IsNotNull(httpWorker, nameof(httpWorker) + "is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }

        /// <summary>
        /// Get User Data by Authorization Token
        /// </summary>
        /// <param name="userAuthToken">User's authorization token</param>
        /// <param name="callback">Returns a UserData via callback when completed.</param>
        /// <returns></returns>
        public IEnumerator GetUserData(string userAuthToken, ResultCallback<UserData> callback)
        {
            Assert.IsFalse(string.IsNullOrEmpty(userAuthToken), "Parameter " + nameof(userAuthToken) + " is null or empty");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/iam/v3/public/users/me")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBearerAuth(userAuthToken)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserData>();
            callback.Try(result);
        }
    }
}