// Copyright (c) 2022 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using Newtonsoft.Json.Linq;

namespace AccelByte.Api
{
    public class SessionApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==BaseUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        public SessionApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.SessionServerUrl, session)
        {
        }

        #region PartySession

        public IEnumerator GetPartyDetails(string partyId
            , ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator UpdateParty(string partyId, SessionV2PartySessionUpdateRequest data
            , ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , data
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator PatchUpdateParty(string partyId, SessionV2PartySessionUpdateRequest data
            , ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , data
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator InviteUserToParty(string partyId, string userId
            , ResultCallback callback)
        {
            InviteUserToParty(partyId, userId, callback);
            yield break;
        }
        
        internal void InviteUserToParty(string partyId, string userId, InviteUserToPartyOptionalParameters optionalParameters, ResultCallback<InviteUserToPartyResponse> callback)
        {
            var error = AccelByte.Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, partyId, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var data = new SessionV2SessionInviteRequest
            {
                Metadata = optionalParameters?.Metadata,
                PlatformId = optionalParameters?.PlatformId?.Id,
                userId = userId
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/invite")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            
            var additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);
            HttpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParseJson<InviteUserToPartyResponse>();
                callback?.Try(result);
            });
        }
        
        public IEnumerator PromoteUserToPartyLeader(string partyId, string leaderId
            , ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , leaderId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }
            var data = new SessionV2PartySessionPromoteLeaderRequest()
            {
                leaderId = leaderId
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/leader")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator JoinParty(string partyId, ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/users/me/join")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator LeaveParty(string partyId, ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/users/me/leave")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();

            callback?.Try(result);
        }

        public IEnumerator RejectPartyInvitation(string partyId, ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/users/me/reject")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParse();

            callback?.Try(result);
        }

        public IEnumerator KickUserFromParty(string partyId, string userId,
            ResultCallback<SessionV2PartySessionKickResponse> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , userId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/users/{userId}/kick")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2PartySessionKickResponse>();

            callback?.Try(result);
        }

        public IEnumerator CreateParty(SessionV2PartySessionCreateRequest data
            , ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(data
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/party")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator GetUserParties(ResultCallback<PaginatedResponse<SessionV2PartySession>> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/parties")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedResponse<SessionV2PartySession>>();

            callback?.Try(result);
        }

        public IEnumerator JoinPartyByCode(string code
            , ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(code
                 , AuthToken
                 , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            SessionV2JoinByCodeRequest body = new SessionV2JoinByCodeRequest { Code = code };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/parties/users/me/join/code")
                .WithPathParam("namespace", Namespace_)
                .WithBody(body.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator GenerateNewPartyCode(string partyId
            , ResultCallback<SessionV2PartySession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/code")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SessionV2PartySession>();

            callback?.Try(result);
        }

        public IEnumerator RevokePartyCode(string partyId
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/code")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();

            callback?.Try(result);
        }

        internal void CancelPartyInvitation(string partyId, string userId, ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/parties/{partyId}/users/{userId}/cancel")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("partyId", partyId)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        #endregion

        #region GameSession

        public IEnumerator CreateGameSession(SessionV2GameSessionCreateRequest data
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(data
                , data?.configurationName
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/gamesession")
                .WithPathParam("namespace", Namespace_)
                .WithBody(data.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2GameSession>();

            callback?.Try(result);
        }

        public IEnumerator QueryGameSession(Dictionary<string, object> data
            , ResultCallback<PaginatedResponse<SessionV2GameSession>> callback)
        {
            QueryGameSession(data, null, callback);
            yield break;
        }
        
        public void QueryGameSession(Dictionary<string, object> data
            , QueryGameSessionOptionalParameters optionalParameters
            , ResultCallback<PaginatedResponse<SessionV2GameSession>> callback)
        {
            if (optionalParameters != null)
            {
                if (data == null)
                {
                    data = new Dictionary<string, object>();
                }
                
                if (optionalParameters.Offset != null)
                {
                    if (!data.TryAdd("offset", optionalParameters.Offset))
                    {
                        data["offset"] = optionalParameters.Offset;
                    }
                }

                if (optionalParameters.Limit != null)
                {
                    if (!data.TryAdd("limit", optionalParameters.Limit))
                    {
                        data["limit"] = optionalParameters.Limit;
                    }
                }

                if (optionalParameters.Availability != null)
                {
                    string availability = optionalParameters.Availability.ToString().ToLower();
                    if (!data.TryAdd("availability", availability))
                    {
                        data["availability"] = availability;
                    }
                }
            }
            
            var error = ApiHelperUtils.CheckForNullOrEmpty(data
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions")
                .WithPathParam("namespace", Namespace_)
                .WithBody(data.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);
            HttpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParseJson<PaginatedResponse<SessionV2GameSession>>();
                callback?.Try(result); 
            });
        }

        public IEnumerator GetGameSessionDetailsByPodName(string podName
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(podName
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/servers/{podName}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("podName", podName)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionV2GameSession>();

            callback?.Try(result);
        }

        public IEnumerator GetGameSessionDetailsBySessionId(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            GetGameSessionDetailsBySessionIdInternal(sessionId, null, callback);
            yield break;
        }
        
        internal void GetGameSessionDetailsBySessionIdInternal(string sessionId
            , OptionalParametersBase optionalParameters
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, sessionId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);
            HttpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParseJson<SessionV2GameSession>();
                callback?.Try(result); 
            });
        }

        public IEnumerator DeleteGameSession(string sessionId
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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

            callback?.Try(result);
        }

        public IEnumerator PatchGameSession(string sessionId,
            SessionV2GameSessionUpdateRequest data
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , data
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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

            callback?.Try(result);
        }

        public IEnumerator InviteUserToGameSession(string sessionId, string userId
            , ResultCallback callback)
        {
            InviteUserToGameSession(sessionId, userId, callback);
            yield break;
        }
        
        internal void InviteUserToGameSession(string sessionId, string userId, InviteUserToGameSessionOptionalParameters optionalParameters, ResultCallback<InviteUserToGameSessionResponse> callback)
        {
            var error = AccelByte.Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, sessionId, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var data = new SessionV2SessionInviteRequest
            {
                Metadata = optionalParameters?.Metadata,
                PlatformId = optionalParameters?.PlatformId,
                userId = userId
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/invite")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBody(data.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters), request, response =>
            {
                var result = response.TryParseJson<InviteUserToGameSessionResponse>();
                callback?.Try(result);
            });
        }

        public void KickUserFromGameSession(string sessionId, string userId, ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId, userId, AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/members/{memberId}/kick")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithPathParam("memberId", userId)
                .WithBearerAuth(AuthToken)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public IEnumerator JoinGameSession(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            JoinGameSession(sessionId, null, callback);
            yield break;
        }

        internal void JoinGameSession(string sessionId
            , JoinGameSessionOptionalParameters optionalParameters
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/join")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters), request, response =>
            {
                var result = response.TryParseJson<SessionV2GameSession>();
                callback?.Try(result);
            });
        }

        public IEnumerator LeaveGameSession(string sessionId
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/leave")
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

            callback?.Try(result);
        }

        public IEnumerator RejectGameSessionInvitation(string sessionId
            , ResultCallback callback)
        {
            RejectGameSessionInvitation(sessionId, null, callback);
            yield break;
        }
        
        public void RejectGameSessionInvitation(string sessionId
            , RejectSessionInvitationOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/reject")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters), request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public IEnumerator GetUserGameSessions(SessionV2StatusFilter? statusFilter,
            SessionV2AttributeOrderBy? orderBy, bool? sortDesc,
            ResultCallback<PaginatedResponse<SessionV2GameSession>> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var queryDict = new Dictionary<string, string>();

            if (statusFilter != null) queryDict.Add("status", statusFilter.ToString());
            if (orderBy != null) queryDict.Add("orderBy", orderBy.ToString());
            if (sortDesc != null) queryDict.Add("order", sortDesc == true ? "desc" : "asc");

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/gamesessions")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParams(queryDict)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<PaginatedResponse<SessionV2GameSession>>();

            callback?.Try(result);
        }

        public IEnumerator JoinGameSessionByCode(string code
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(code
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            SessionV2JoinByCodeRequest body = new SessionV2JoinByCodeRequest { Code = code };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/join/code")
                .WithPathParam("namespace", Namespace_)
                .WithBody(body.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SessionV2GameSession>();

            callback?.Try(result);
        }

        public IEnumerator GenerateNewGameSessionCode(string sessionId
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/code")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SessionV2GameSession>();

            callback?.Try(result);
        }

        public IEnumerator RevokeGameSessionCode(string sessionId
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/code")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();

            callback?.Try(result);
        }

        public IEnumerator PromoteUserToGameSessionLeader(string sessionId, string leaderId
            , ResultCallback<SessionV2GameSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , leaderId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var data = new SessionV2GameSessionPromoteLeaderRequest()
            {
                LeaderId = leaderId
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}/leader")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SessionV2GameSession>();

            callback?.Try(result);
        }

        #endregion

        #region SessionStorage

        public IEnumerator UpdateLeaderStorage(string sessionId, JObject data, ResultCallback<JObject> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/public/namespaces/{namespace}/sessions/{sessionId}/storage/leader")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<JObject>();

            callback?.Try(result);
        }

        public IEnumerator UpdateMemberStorage(string userId, string sessionId, JObject data, ResultCallback<JObject> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePatch(BaseUrl + "/v1/public/namespaces/{namespace}/sessions/{sessionId}/storage/users/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithPathParam("userId", userId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(data.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<JObject>();

            callback?.Try(result);
        }

        #endregion

#region Player
        internal void GetPlayerAttributes(ResultCallback<PlayerAttributesResponseBody> callback)
        {
            var error = AccelByte.Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/attributes")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PlayerAttributesResponseBody>();
                callback?.Try(result);
            });
        }

        internal void StorePlayerAttributes(PlayerAttributesRequestBody requestBody, ResultCallback<PlayerAttributesResponseBody> callback)
        {
            var error = AccelByte.Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, requestBody);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/attributes")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(requestBody.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<PlayerAttributesResponseBody>();
                callback?.Try(result);
            });
        }

        internal void RemovePlayerAttributes(ResultCallback callback)
        {
            var error = AccelByte.Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/attributes")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        internal void GetRecentPlayers(GetRecentPlayersOptionalParameters optionalParameters, ResultCallback<SessionV2RecentPlayers> callback)
        {
            var error = AccelByte.Utils.ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            uint limit = 20; // Default value
            if (optionalParameters?.Limit != null)
            {
                limit = optionalParameters.Limit.Value;
                if (limit > 200)
                {
                    AccelByteDebug.LogWarning("GetRecentPlayers limit exceeds 200, clamping to 200");
                    limit = 200;
                }
            }

            var requestBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/recent-player")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParameters?.Limit != null)
            {
                requestBuilder.WithQueryParam("limit", limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<SessionV2RecentPlayers>();
                callback?.Try(result);
            });
        }
#endregion
    }
}