// Copyright (c) 2022 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

namespace AccelByte.Api
{
    public class MatchmakingV2Api : ApiBase
    {
        /// <summary>
        /// Api class for MatchmakingV2 service
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==MatchmakingV2ServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        public MatchmakingV2Api(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.MatchmakingV2ServerUrl, session)
        {
        }

        public IEnumerator CreateMatchmakingTicket(string matchPoolName
            , [CanBeNull] MatchmakingV2CreateTicketRequestOptionalParams optionalParams
            , ResultCallback<MatchmakingV2CreateTicketResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , matchPoolName
            );
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var requestBody = new MatchmakingV2CreateTicketRequest()
            {
                matchPool = matchPoolName
            };

            if (optionalParams != null)
            {
                if (optionalParams.attributes != null)
                {
                    requestBody.attributes = optionalParams.attributes;
                }
                if (optionalParams.latencies != null)
                {
                    requestBody.latencies = optionalParams.latencies;
                }
                if (!string.IsNullOrEmpty(optionalParams.sessionId))
                {
                    requestBody.sessionId = optionalParams.sessionId;
                }
                if (optionalParams.ExcludedGameSessionIds != null)
                {
                    requestBody.ExcludedSessions = optionalParams.ExcludedGameSessionIds;
                }
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/namespaces/{namespace}/match-tickets")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParams?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParseJson<MatchmakingV2CreateTicketResponse>();
                callback?.Try(result);
            });

            yield break;
        }

        public IEnumerator GetMatchmakingTicket(string ticketId
            , ResultCallback<MatchmakingV2MatchTicketStatus> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMatchmakingTicketOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetMatchmakingTicket(ticketId, optionalParameters, callback);

            yield return null;
        }

        internal void GetMatchmakingTicket(string ticketId
            , GetMatchmakingTicketOptionalParameters optionalParameters
            , ResultCallback<MatchmakingV2MatchTicketStatus> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);
            
            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , ticketId
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/match-tickets/{ticketId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("ticketId", ticketId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request,
                response =>
                {
                    var result = response.TryParseJson<MatchmakingV2MatchTicketStatus>();
                    callback.Try(result);
                });
        }

        public IEnumerator DeleteMatchmakingTicket(string ticketId, ResultCallback callback)
        {
            return DeleteMatchmakingTicket(ticketId, null, callback);
        }
        
        internal IEnumerator DeleteMatchmakingTicket(string ticketId, DeleteMatchmakingV2OptionalParams optionalParams, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , ticketId
            );
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/namespaces/{namespace}/match-tickets/{ticketId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("ticketId", ticketId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            
            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParams?.Logger
            };

            yield return HttpClient.SendRequest(additionalParams, request,
                rsp => response = rsp);

            var result = response.TryParse();

            callback?.Try(result);
        }

        public IEnumerator GetMatchmakingMetrics(string matchPool, ResultCallback<MatchmakingV2Metrics> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMatchmakingMetricsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetMatchmakingMetrics(matchPool, optionalParameters, callback);

            yield return null;
        }

        internal void GetMatchmakingMetrics(string matchPool, GetMatchmakingMetricsOptionalParameters optionalParameters, ResultCallback<MatchmakingV2Metrics> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
                , matchPool
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/match-pools/{matchPool}/metrics")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("matchPool", matchPool)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<MatchmakingV2Metrics>();
                callback.Try(result);
            });
        }

        public IEnumerator GetUserMatchmakingTickets(ResultCallback<MatchmakingV2ActiveTickets> callback
            , string matchPool, int offset, int limit)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserMatchmakingTicketsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                MatchPool = matchPool,
                Offset = offset
            };

            GetUserMatchmakingTickets(optionalParameters, callback);

            yield return null;
        }

        internal void GetUserMatchmakingTickets(GetUserMatchmakingTicketsOptionalParameters optionalParameters
            , ResultCallback<MatchmakingV2ActiveTickets> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(
                Namespace_
                , AuthToken
            );
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/namespaces/{namespace}/match-tickets/me")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters != null)
            {
                var queryParams = new Dictionary<string, string>();

                if (!string.IsNullOrEmpty(optionalParameters.MatchPool))
                {
                    queryParams.Add("matchPool", optionalParameters?.MatchPool);
                }
                if (optionalParameters.Offset != null && optionalParameters.Offset >= 0)
                {
                    queryParams.Add("offset", optionalParameters.Offset.ToString());
                }
                if (optionalParameters.Limit != null && optionalParameters.Limit >= 1)
                {
                    queryParams.Add("limit", optionalParameters.Limit.ToString());
                }

                builder.WithQueries(queryParams);
            }

            var request = builder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<MatchmakingV2ActiveTickets>();
                callback?.Try(result);
            });
        }
    }
}