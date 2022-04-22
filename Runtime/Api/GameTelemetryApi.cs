// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class GameTelemetryApi
    {
        #region Fields 

        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        #endregion

        #region Public Methods

        public GameTelemetryApi(string baseUrl, IHttpClient httpClient)
        {
            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }
        
        public IEnumerator SendProtectedEvents(List<TelemetryBody> events, string accessToken, ResultCallback callback)
        {
            Assert.IsNotNull(events, nameof(events) + " is null.");

                var request = HttpRequestBuilder
                    .CreatePost(this.baseUrl + "/v1/protected/events")
                    .WithContentType(MediaType.ApplicationJson)
                    .WithBody(events.ToUtf8Json())
                    .WithBearerAuth(accessToken)
                    .Accepts(MediaType.ApplicationJson)
                    .GetResult();

                IHttpResponse response = null;

                yield return this.httpClient.SendRequest(request, rsp => response = rsp);

                var result = response.TryParse();
                callback.Try(result);
        }

        #endregion
    }
}