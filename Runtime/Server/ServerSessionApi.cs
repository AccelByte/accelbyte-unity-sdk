// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerSessionApi : ServerApiBase
    {
        /// <summary>
        /// Api class for server Session service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==SessionServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        public ServerSessionApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.SessionServerUrl, session)
        {
        }

        public IEnumerator GetGameSessionDetails(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(sessionId, nameof(sessionId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2GameSession>();

            callback.Try(result);
        }

        public IEnumerator DeleteGameSession(string sessionId
            , ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(sessionId, nameof(sessionId) + " cannot be null");

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator UpdateGameSession(string sessionId,
            SessionV2GameSessionUpdateRequest data
            , ResultCallback<SessionV2GameSession> callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(sessionId, nameof(sessionId) + " cannot be null");
            Assert.IsNotNull(data, "SessionV2GameSessionUpdateRequest cannot be null");

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBody(data.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2GameSession>();

            callback.Try(result);
        }
    }
}