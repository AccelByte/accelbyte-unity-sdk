// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Xml;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

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

        public IEnumerator GetAllGameSessions(
            ResultCallback<SessionV2GameSessionPagingResponse> callback
            , SessionV2DsStatus statusV2 = SessionV2DsStatus.None
            , string sessionId = ""
            , string matchPool = ""
            , string gameMode = ""
            , SessionV2Joinability joinability = SessionV2Joinability.None
            , string memberId = ""
            , string configurationName = ""
            , DateTime fromTime = default
            , DateTime toTime = default
            , string dsPodName = ""
            , bool isSoftDeleted = false
            , bool isPersistent = false
            , SessionV2AttributeOrderBy orderBy = SessionV2AttributeOrderBy.createdAt
            , SessionV2AttributeOrder order = SessionV2AttributeOrder.Desc
            , int offset = 0
            , int limit = 20
        )
        {
            var httpBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/gamesessions")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("statusV2", statusV2 != SessionV2DsStatus.None ? statusV2.ToString() : string.Empty)
                .WithQueryParam("sessionID", sessionId)
                .WithQueryParam("matchPool", matchPool)
                .WithQueryParam("gameMode", gameMode)
                .WithQueryParam("joinability", joinability != SessionV2Joinability.None ? joinability.ToString() : string.Empty)
                .WithQueryParam("memberID", memberId)
                .WithQueryParam("configurationName", configurationName)
                .WithQueryParam("dsPodName", dsPodName)
                .WithQueryParam("isSoftDeleted", isSoftDeleted.ToString())
                .WithQueryParam("isPersistent", isPersistent.ToString())
                .WithQueryParam("orderBy", orderBy.ToString())
                .WithQueryParam("order", order.ToString().ToLower())
                .WithQueryParam("offset", offset > 0 ? offset.ToString() : "0")
                .WithQueryParam("limit", limit > 0 ? limit.ToString() : "20");

            if (fromTime != default)
            {
                httpBuilder.WithQueryParam("fromTime", XmlConvert.ToString(fromTime, XmlDateTimeSerializationMode.Local));
            }

            if (toTime != default)
            {
                httpBuilder.WithQueryParam("toTime", XmlConvert.ToString(toTime, XmlDateTimeSerializationMode.Local));
            }

            var request = httpBuilder.GetResult();
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<SessionV2GameSessionPagingResponse>();

            callback?.Try(result);
        }

        public IEnumerator GetGameSessionDetails(string sessionId
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
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/gamesessions/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<SessionV2GameSession>();

            callback?.Try(result);
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
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParse();

            callback?.Try(result);
        }

        public IEnumerator UpdateGameSession(string sessionId,
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
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<SessionV2GameSession>();

            callback?.Try(result);
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
                .CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/gamesessions/{sessionId}/members/{memberId}/kick")
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

        public IEnumerator SendDSSessionReady(string sessionId,
            bool isDsSessionReady,
            ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            SessionV2DsSessionReadyRequest data = new SessionV2DsSessionReadyRequest
            {
                Ready = isDsSessionReady
            };

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/gamesessions/{sessionId}/ds")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithBody(data.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParse();

            callback?.Try(result);
        }

        public void GetMemberActiveSession(string userId
            , string configurationName
            , ResultCallback<SessionV2MemberActiveSession> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(userId
                , configurationName
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/configurations/{name}/memberactivesession/{userId}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", ServerConfig.Namespace)
                .WithPathParam("name", configurationName)
                .WithPathParam("userId", userId);

            IHttpRequest request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<SessionV2MemberActiveSession>();
                callback?.Try(result);
            });
        }

        public void ReconcileMaxActiveSession(string userId
            , string configurationName
            , ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(userId
                , configurationName
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/configurations/{name}/reconcile")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", ServerConfig.Namespace)
                .WithPathParam("name", configurationName)
                .Accepts(MediaType.ApplicationJson);

            var body = new
            {
                UserId = userId
            };

            builder.WithBody(body.ToUtf8Json());
            IHttpRequest request = builder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        internal void GetPartySessionStorage(string partyId
            , ResultCallback<GetPartySessionStorageResult> callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(partyId
                , AuthToken
                , Namespace_);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/parties/{partyId}/storage")
                .WithBearerAuth(AuthToken)
                .WithPathParam("namespace", ServerConfig.Namespace)
                .WithPathParam("partyId", partyId)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GetPartySessionStorageResult>();
                callback?.Try(result);
            });
        }
    }
}