// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class GameTelemetryApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==GameTelemetryServerUrl</param>
        /// <param name="session"></param>
        public GameTelemetryApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.GameTelemetryServerUrl, session )
        {
        }
        
        public IEnumerator SendProtectedEvents( List<TelemetryBody> events
            , ResultCallback callback )
        {
            Assert.IsNotNull(events, nameof(events) + " is null.");

                var request = HttpRequestBuilder
                    .CreatePost(BaseUrl + "/v1/protected/events")
                    .WithContentType(MediaType.ApplicationJson)
                    .WithBody(events.ToUtf8Json())
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .GetResult();

                IHttpResponse response = null;

                yield return HttpClient.SendRequest(request, 
                    rsp => response = rsp);

                var result = response.TryParse();
                callback.Try(result);
        }

    }
}
