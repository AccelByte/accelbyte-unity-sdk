// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    /// <summary>
    /// Provide function to let Challenge service wrapper to connect to endpoint.
    /// </summary>
    internal class ServerChallengeApi : ServerApiBase
    {
        readonly private IHttpClient httpClient;

        [UnityEngine.Scripting.Preserve]
        internal ServerChallengeApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.ChallengeServerUrl, session)
        {
            this.httpClient = httpClient;
        }

        public async void EvaluateChallengeProgress(ChallengeEvaluatePlayerProgressionRequest requestBody
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/progress/evaluate")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", serverConfig.Namespace)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpSendResult sendResult = await httpClient.SendRequestAsync(request);
            IHttpResponse response = sendResult.CallbackResponse;
            var result = response.TryParse();

            callback.Try(result);
        }
    }
}