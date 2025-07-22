// Copyright (c) 2022 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;

namespace AccelByte.Api
{

    public class TurnManagerApi : ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        public TurnManagerApi(IHttpClient inHttpClient, Config inConfig, ISession inSession)
            : base(inHttpClient, inConfig, inConfig.TurnManagerServerUrl, inSession)
        {
        }

        public IEnumerator GetTurnServers(ResultCallback<TurnServerList> callback)
        {
            RequestGetTurnServers(null, callback);
            yield break;
        }

        internal void RequestGetTurnServers(RequestGetTurnServersOptionalParam optionalParameters, ResultCallback<TurnServerList> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/turn")
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(
            AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters)
            , request
            , response =>
            {
                var result = response.TryParseJson<TurnServerList>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetTurnServerCredential(string region
            , string ip
            , int port
            , ResultCallback<TurnServerCredential> callback)
        {
            RequestGetTurnServerCredential(region, ip, port, optionalParameters: null, callback);
            yield break;
        }

        internal void RequestGetTurnServerCredential(string region
            , string ip
            , int port
            , RequestGetTurnServerCredentialOptionalParameters optionalParameters
            , ResultCallback<TurnServerCredential> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, region, ip);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (port < 1 || port > 65535)
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "port is not between 1-65535!"));
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/turn/secret/{region}/{ip}/{port}")
                .WithPathParam("region", region)
                .WithPathParam("ip", ip)
                .WithPathParam("port", port.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            var additionalHttpParameters = AdditionalHttpParameters.CreateFromOptionalParameters(optionalParameters);
            HttpOperator.SendRequest(additionalHttpParameters, request, response =>
            {
                var result = response.TryParseJson<TurnServerCredential>();
                callback?.Try(result);
            });
        }

        public IEnumerator SendMetric(string turnServerRegion, P2PConnectionType connectionType
            , ResultCallback callback)
        {
            RequestSendMetric(turnServerRegion, connectionType, callback);
            yield break;
        }

        internal void RequestSendMetric(string turnServerRegion, P2PConnectionType connectionType
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, turnServerRegion);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (connectionType == P2PConnectionType.None)
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "Connection type cannot be None"));
                return;
            }

            var data = new TurnServerMetricRequest()
            {
                Region = turnServerRegion,
                Type = connectionType
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/metrics/namespaces/{namespace}/connected")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        public void Disconnect(ResultCallback callback)
        {
            RequestDisconnect(callback);
        }

        internal void RequestDisconnect(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var data = new DisconnectTurnServerRequest()
            {
                UserId = Session.UserId
            };

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/metrics/namespaces/{namespace}/disconnected")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
            .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }
    }
}