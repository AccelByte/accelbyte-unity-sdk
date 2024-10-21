// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

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
        [UnityEngine.Scripting.Preserve]
        internal ServerQosManagerApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.QosManagerServerUrl, session )
        {
        }
        
        public void RequestGetAllQosServers(ResultCallback<QosServerList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/qos")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<QosServerList>();
                callback?.Try(result);
            });
        }
        
        public void RequestGetQosServers(ResultCallback<QosServerList> callback)
        {
            GetQosServerOptionalParameters optionalParams = new GetQosServerOptionalParameters();
            optionalParams.Status = QosStatus.Active;

            RequestGetPublicQosServers(optionalParams, callback);
        }

        internal void RequestGetPublicQosServers(GetQosServerOptionalParameters optionalParams, ResultCallback<QosServerList> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var httpBuilder = HttpRequestBuilder
                .CreateGet(BaseUrl + "/public/namespaces/{namespace}/qos")
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson);

            if (optionalParams != null)
            {
                if (optionalParams.Status != null)
                {
                    httpBuilder.WithQueryParam("status", ConverterUtils.EnumToDescription(optionalParams.Status));
                }
            }

            var request = httpBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<QosServerList>();
                callback?.Try(result);
            });
        }
    }
}