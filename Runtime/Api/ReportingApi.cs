// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class ReportingApi
    {
        private readonly string baseUrl;
        private readonly string @namespace;
        private readonly ISession session;
        private readonly IHttpClient httpWorker;

        internal ReportingApi(string baseUrl, IHttpClient httpWorker)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpWorker = httpWorker;
        }
        
        public IEnumerator GetReasonGroups(string @namespace, string accessToken, ResultCallback<ReportingReasonGroupsResponse> callback, int offset, int limit)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't create content! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create content! AccessToken parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/reasonGroups")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<ReportingReasonGroupsResponse>();
            callback.Try(result);
        }
        
        public IEnumerator GetReasons(string @namespace, string accessToken, string reasonGroup, ResultCallback<ReportingReasonsResponse> callback, int offset, int limit)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't create content! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create content! AccessToken parameter is null!");
            Assert.IsNotNull(reasonGroup, "Can't create content! reasonGroup parameter is null!");

            var request = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/reasons")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("group", reasonGroup)
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("offset", offset.ToString())
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<ReportingReasonsResponse>();
            callback.Try(result);
        }
        
        public IEnumerator SubmitReport(string @namespace, string accessToken, ReportingSubmitData reportData, ResultCallback<ReportingSubmitResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't create content! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't create content! AccessToken parameter is null!");
            Assert.IsNotNull(reportData, "Can't create content! reasonGroup parameter is null!");

            var request = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/v1/public/namespaces/{namespace}/reports")
                .WithPathParam("namespace", @namespace)
                .WithContentType(MediaType.ApplicationJson)
                .WithBearerAuth(accessToken)
                .WithBody(reportData.ToUtf8Json())
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<ReportingSubmitResponse>();
            callback.Try(result);
        }
    }
}
