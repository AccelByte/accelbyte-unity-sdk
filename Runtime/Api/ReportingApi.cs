// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Api
{
    internal class ReportingApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==ReportingServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal ReportingApi( IHttpClient httpClient
            , Config config
            , ISession session )
            : base( httpClient, config, config.ReportingServerUrl, session )
        {
        }
        
        public IEnumerator GetReasonGroups( ResultCallback<ReportingReasonGroupsResponse> callback
            , int offset
            , int limit )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/reasonGroups")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<ReportingReasonGroupsResponse>();
            callback?.Try(result);
        }
        
        public IEnumerator GetReasons( string reasonGroup
            , ResultCallback<ReportingReasonsResponse> callback
            , int offset
            , int limit
            , string title)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, reasonGroup);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/reasons")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("group", reasonGroup)
                .WithQueryParam("title", title)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<ReportingReasonsResponse>();
            callback?.Try(result);
        }
        
        public IEnumerator SubmitReport( ReportingSubmitData reportData
            , ResultCallback<ReportingSubmitResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, reportData);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/reports")
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithBody(reportData.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<ReportingSubmitResponse>();
            callback?.Try(result);
        }

        public IEnumerator SubmitChatReport(ReportingSubmitDataChat reportData
            , ResultCallback<ReportingSubmitResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, reportData);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder
                .CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/reports")
                .WithPathParam("namespace", Namespace_)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(AuthToken)
                .WithBody(reportData.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<ReportingSubmitResponse>();
            callback?.Try(result);
        }
    }
}
