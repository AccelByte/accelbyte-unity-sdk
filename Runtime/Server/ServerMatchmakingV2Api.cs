// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
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
        [UnityEngine.Scripting.Preserve]
        public ServerMatchmakingV2Api(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.MatchmakingV2ServerUrl, session)
        {
        }


        [Obsolete("This interface is deprecated, and will be removed on AGS 3.82. Please access the api from Api.AcceptBackfillProposal(backfillProposal, acceptedTicketIds, stopBackfilling, callback)")]
        public IEnumerator AcceptBackfillProposal(MatchmakingV2BackfillProposalNotification backfillProposal, bool stopBackfilling, ResultCallback callback)
        {
            var optionalParams = new AcceptBackfillProposalOptionalParams()
            {
                StopBackfilling = stopBackfilling,
            };

            AcceptBackfillProposal(backfillProposal, optionalParams, result =>
            {
                if (result.IsError)
                {
                    callback?.TryError(result.Error);
                }
                else
                {
                    callback?.TryOk();
                }
            });

            yield break;
        }

        public void AcceptBackfillProposal(MatchmakingV2BackfillProposalNotification backfillProposal
            , AcceptBackfillProposalOptionalParams optionalParams
            , ResultCallback<AcceptBackfillProposalResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , AuthToken
                , backfillProposal
                , backfillProposal?.proposalId
                , backfillProposal?.backfillTicketId);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBody = new ServerMatchmakingV2BackfillRequest()
            {
                proposalId = backfillProposal.proposalId,
                isStoppingBackfill = false
            };

            if (optionalParams != null)
            {
                if (optionalParams.StopBackfilling.HasValue)
                {
                    requestBody.isStoppingBackfill = optionalParams.StopBackfilling.Value;
                }
                if (optionalParams.AcceptedTicketIds != null && optionalParams.AcceptedTicketIds.Length > 0)
                {
                    requestBody.AcceptedTicketIds = optionalParams.AcceptedTicketIds;
                }
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/namespaces/{namespace}/backfill/{backfillID}/proposal/accept")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("backfillID", backfillProposal.backfillTicketId)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<AcceptBackfillProposalResponse>();
                callback?.Try(result);
            });
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