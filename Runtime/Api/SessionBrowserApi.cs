// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace AccelByte.Api
{
    public class SessionBrowserApi : AccelByte.Core.ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        public SessionBrowserApi(IHttpClient inHttpClient, Config inConfig, ISession inSession)
            : base(inHttpClient, inConfig, inConfig.SessionBrowserServerUrl, inSession)
        {
        }

        public IEnumerator CreateGameSession(SessionBrowserCreateRequest createRequest, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(createRequest);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

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

            var error = ApiHelperUtils.CheckForNullOrEmpty(updateRequest);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            if (string.IsNullOrEmpty(sessionId))
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

            if (string.IsNullOrEmpty(sessionId))
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

            var error = ApiHelperUtils.CheckForNullOrEmpty(filter);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

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

        public IEnumerator GetGameSessionsByUserIds(string[] userIds, ResultCallback<SessionBrowserGetByUserIdsResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(userIds);
            if (error != null)
            {
                callback.TryError(error);
                yield break;
            }

            if (userIds.Length == 0)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Empty userIds"));
                yield break;
            }

            string url = BaseUrl + "/namespaces/" + Namespace_ + "/gamesession/bulk";

            string userIdsQueryString = string.Join(",", userIds);

            var request = HttpRequestBuilder
                .CreateGet(url)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithQueryParam("user_ids", userIdsQueryString)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<SessionBrowserGetByUserIdsResult>();
            callback.Try(result);
        }

        public IEnumerator GetGameSession(string sessionId, ResultCallback<SessionBrowserData> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(sessionId);
            if (error != null)
            {
                callback.TryError(error);
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

            if (string.IsNullOrEmpty(sessionId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(targetedUserId))
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

            if (string.IsNullOrEmpty(sessionId))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "sessionId can't be empty"));
                yield break;
            }
            if (string.IsNullOrEmpty(targetedUserId))
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

            if (string.IsNullOrEmpty(targetedUserId))
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

            if (string.IsNullOrEmpty(sessionId))
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

        internal string GetHashSessionPassword(string password)
        {
            string retval = string.Empty;
            using (MD5 md5 = MD5.Create())
            {
                var bytesPassword = Encoding.UTF8.GetBytes(password);
                byte[] computeHash = md5.ComputeHash(bytesPassword);
                retval = System.BitConverter.ToString(computeHash);
            }
            return retval;
        }
    }
}
