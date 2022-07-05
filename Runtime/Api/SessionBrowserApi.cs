// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft;

namespace AccelByte.Api
{
    public class SessionBrowserApi : AccelByte.Core.ApiBase
    {
        public SessionBrowserApi(IHttpClient inHttpClient, Config inConfig, ISession inSession)
            : base(inHttpClient, inConfig, inConfig.SessionBrowserServerUrl, inSession)
        {
        }

        public IEnumerator CreateGameSession(SessionBrowserCreateRequest createRequest, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(createRequest, "Can't create game session! request parameter is null!");

            if (createRequest.session_type == SessionType.none)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Wrong session type"));
                yield break;
            }

            if (createRequest.game_session_setting.max_player == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Max players must greather then 0"));
                yield break;
            }

            if (createRequest.game_session_setting.mode.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Game mode can't be empty"));
                yield break;
            }

            if (createRequest.game_session_setting.password.Length > 0)
            {
                createRequest.game_session_setting.password = GetHashSessionPassword(createRequest.game_session_setting.password);
            }

            var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
            jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/namespaces/{namespace}/gamesession")
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Newtonsoft.Json.JsonConvert.SerializeObject(createRequest, jsonSerializerSettings))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserData>();
            callback.Try(result);
        }

        public IEnumerator UpdateGameSession(string sessionId, SessionBrowserUpdateSessionRequest updateRequest, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "Can't update game session! sessionId parameter is null!");
            Assert.IsNotNull(updateRequest, "Can't update game session! updateRequest parameter is null!");

            if (sessionId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }

            if (updateRequest.game_max_player == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "game_max_player must greather then 0"));
                yield break;
            }

            if (updateRequest.game_max_player < updateRequest.game_current_player)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "game_max_player should NOT be less than game_current_player"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePut(BaseUrl + "/namespaces/{namespace}/gamesession/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Newtonsoft.Json.JsonConvert.SerializeObject(updateRequest))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserData>();
            callback.Try(result);
        }

        public IEnumerator RemoveGameSession(string sessionId, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "Can't update game session! sessionId parameter is null!");

            if (sessionId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/namespaces/{namespace}/gamesession/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserData>();
            callback.Try(result);
        }

        public IEnumerator GetGameSessions(SessionBrowserQuerySessionFilter filter, ResultCallback<SessionBrowserGetResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(filter, "Can't get game sessions! filter parameter is null!");

            if (filter.sessionType == SessionType.none)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Wrong session type"));
                yield break;
            }

            string url = BaseUrl + "/namespaces/" + Namespace_ + "/gamesession";

            string queryParam = "?session_type=" + System.Enum.GetName(typeof(SessionType), filter.sessionType);
            queryParam += "&joinable=true";
            if (filter.gameMode != null && filter.gameMode.Length > 0)
            {
                queryParam += "&game_mode=" + filter.gameMode;
            }
            if (filter.limit != null)
            {
                queryParam += "&limit=" + filter.limit;
            }
            if (filter.offset != null)
            {
                queryParam += "&offset=" + filter.offset;
            }
            if (filter.matchExist != null)
            {
                queryParam += "&match_exist=" + (filter.matchExist.GetValueOrDefault() ? "true" : "false");
            }
            url = url + queryParam;

            var request = HttpRequestBuilder
                .CreateGet(url)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserGetResult>();
            callback.Try(result);
        }

        public IEnumerator GetGameSession(string sessionId, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "Can't get game session! sessionId parameter is null!");

            if (sessionId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/namespaces/{namespace}/gamesession/{sessionId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserData>();
            callback.Try(result);
        }

        public IEnumerator RegisterPlayer(string sessionId, string targetedUserId, bool asSpectator, ResultCallback<SessionBrowserAddPlayerResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (sessionId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }
            if (targetedUserId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "targetedUserId can't be empty"));
                yield break;
            }

            SessionBrowserAddPlayerRequest registerRequest = new SessionBrowserAddPlayerRequest();
            registerRequest.user_id = targetedUserId;
            registerRequest.as_spectator = asSpectator;
            
            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/namespaces/{namespace}/gamesession/{sessionId}/player")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithBody(Newtonsoft.Json.JsonConvert.SerializeObject(registerRequest))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserAddPlayerResponse>();
            callback.Try(result);
        }

        public IEnumerator UnregisterPlayer(string sessionId, string targetedUserId, ResultCallback<SessionBrowserRemovePlayerResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (sessionId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }
            if (targetedUserId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "targetedUserId can't be empty"));
                yield break;
            }
            
            var request = HttpRequestBuilder
                .CreateDelete(BaseUrl + "/namespaces/{namespace}/gamesession/{sessionId}/player/{userId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithPathParam("userId", targetedUserId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserRemovePlayerResponse>();
            callback.Try(result);
        }

        public IEnumerator GetRecentPlayer(string targetedUserId, uint offset, uint limit, ResultCallback<SessionBrowserRecentPlayerGetResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(targetedUserId, "Can't get recent player! targetedUserId parameter is null!");

            if (targetedUserId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "targetedUserId can't be empty"));
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/namespaces/{namespace}/recentplayer/{targetedUserId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("targetedUserId", targetedUserId)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserRecentPlayerGetResponse>();
            callback.Try(result);
        }

        public IEnumerator JoinSession(string sessionId, string password, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(sessionId, "Can't join session! sessionId parameter is null!");

            if (sessionId.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }

            var joinRequest = new SessionBrowserJoinSessionRequest();
            joinRequest.password = GetHashSessionPassword(password);

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/namespaces/{namespace}/gamesession/{sessionId}/join")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("sessionId", sessionId)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(Newtonsoft.Json.JsonConvert.SerializeObject(joinRequest))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserData>();
            callback.Try(result);
        }

        private string GetHashSessionPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                var bytesPassword = Encoding.UTF8.GetBytes(password);
                byte[] computeHash = md5.ComputeHash(bytesPassword);
                return System.BitConverter.ToString(computeHash);
            }
            return "";
        }
    }
}
