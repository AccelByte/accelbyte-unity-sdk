// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Api
{
    internal class LoginQueueApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BasicServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal LoginQueueApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.LoginQueueServerUrl, session)
        {
        }

        [UnityEngine.Scripting.Preserve]
        public LoginQueueApi(IHttpClient httpClient
            , Config config
            , ISession session
            , HttpOperator httpOperator)
            : base(httpClient, config, config.LoginQueueServerUrl, session, httpOperator)
        {
        }

        public void RefreshTicket(string ticketId, string namespace_, ResultCallback<RefreshTicketResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(ticketId, namespace_);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/ticket")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(ticketId)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<RefreshTicketResponse>();
                callback.Try(result);
            });
        }

        public void CancelTicket(string ticketId, string namespace_, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(ticketId, namespace_);
            if (error != null)
            {
                callback.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/namespaces/{namespace}/ticket")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(ticketId)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback.Try(result);
            });
        }
    }
}
