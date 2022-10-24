// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerMatchmakingV2Api : ServerApiBase
    {
        /// <summary>
        /// Api class for server MatchmakingV2 service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==MatchmakingV2ServerUrl</param>
        /// <param name="session"></param>
        public ServerMatchmakingV2Api(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.MatchmakingV2ServerUrl, session)
        {
        }

        public IEnumerator AcceptBackfillProposal(MatchmakingV2BackfillProposalNotification backfillProposal, bool stopBackfilling, ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(backfillProposal, nameof(backfillProposal) + " cannot be null");

            var requestBody = new ServerMatchmakingV2BackfillRequest()
            {
                proposalId = backfillProposal.proposalId,
                isStoppingBackfill = stopBackfilling
            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/backfill/{backfillTicketId}/proposal/accept")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("backfillTicketId", backfillProposal.backfillTicketId)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();

            callback.Try(result);
        }

        public IEnumerator RejectBackfillProposal(MatchmakingV2BackfillProposalNotification backfillProposal, bool stopBackfilling, ResultCallback callback)
        {
            Assert.IsNotNull(Namespace_, nameof(Namespace_) + " cannot be null");
            Assert.IsNotNull(AuthToken, nameof(AuthToken) + " cannot be null");
            Assert.IsNotNull(backfillProposal, nameof(backfillProposal) + " cannot be null");

            var requestBody = new ServerMatchmakingV2BackfillRequest()
            {
                proposalId = backfillProposal.proposalId,
                isStoppingBackfill = stopBackfilling
            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/backfill/{backfillTicketId}/proposal/reject")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("backfillTicketId", backfillProposal.backfillTicketId)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
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