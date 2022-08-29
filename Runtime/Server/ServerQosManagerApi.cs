// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Server
{
    /// <summary>
    /// QoS == Quality of Service (Latencies, Pings, Regions, etc)
    /// </summary>
    internal class ServerQosManagerApi : ServerApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==QosManagerServerUrl</param>
        /// <param name="session"></param>
        internal ServerQosManagerApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.QosManagerServerUrl, session )
        {
        }

        public IEnumerator GetAllQosServers( ResultCallback<QosServerList> callback )
        {
            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/qos")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);
            
            var result = response.TryParseJson<QosServerList>();
            callback.Try(result);
        }

        public IEnumerator GetQosServers(ResultCallback<QosServerList> callback)
        {
            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/qos")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("status", "ACTIVE")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp => response = rsp);

            var result = response.TryParseJson<QosServerList>();
            callback.Try(result);
        }
    }
}