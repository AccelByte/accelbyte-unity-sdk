// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;

namespace AccelByte.Api
{

    public class TurnManagerApi : AccelByte.Core.ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        public TurnManagerApi(IHttpClient inHttpClient, Config inConfig, ISession inSession)
            : base(inHttpClient, inConfig, inConfig.TurnManagerServerUrl, inSession)
        {
        }

        public IEnumerator GetTurnServers(ResultCallback<TurnServerList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/turn")
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<TurnServerList>();
            callback.Try(result);
        }

        public IEnumerator GetTurnServerCredential(string region
            , string ip
            , int port
            , ResultCallback<TurnServerCredential> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (string.IsNullOrEmpty(region))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "region parameter is null or empty!"));
                yield break;
            }
            if (string.IsNullOrEmpty(ip))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "ip parameter is null or empty!"));
                yield break;
            }

            if (port < 1 || port > 65535)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "port is not between 1-65535!"));
                yield break;
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

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<TurnServerCredential>();
            callback.Try(result);
        }
        
        public IEnumerator SendMetric(string turnServerRegion, P2PConnectionType connectionType
            , ResultCallback callback)
        {
            if (Namespace_ == null)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                yield break;
            }
            
            if (AuthToken == null)
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                yield break;
            }
            
            if (string.IsNullOrEmpty(turnServerRegion))
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "region is null or empty!"));
                yield break;
            }
            
            if (connectionType == P2PConnectionType.None)
            {
                callback.TryError(new Error(ErrorCode.InvalidRequest, "Connection type cannot be None"));
                yield break;
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

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParse();

            callback.Try(result);
        }

        public void Disconnect(ResultCallback callback)
        {
            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var data = new DisconnectTurnServerRequest()
            {
                UserId = Session.UserId
            };

            var httpBuilder = HttpRequestBuilder
                .CreatePost(BaseUrl + "/metrics/namespaces/{namespace}/disconnected")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithBody(data.ToUtf8Json())
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
            .GetResult();

            IHttpRequest request = httpBuilder;
            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }
    }
}