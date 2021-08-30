// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;

namespace Tests
{
    public class UgcTestAdmin
    {
        private readonly AccelByteHttpClient httpClient;
        private readonly CoroutineRunner coroutineRunner;
        private readonly Config config;

        public UgcTestAdmin()
        {
            this.config = new Config
            {
                BaseUrl = Environment.GetEnvironmentVariable("ADMIN_BASE_URL"),
                ClientId = Environment.GetEnvironmentVariable("ADMIN_CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("ADMIN_CLIENT_SECRET"),
                PublisherNamespace = Environment.GetEnvironmentVariable("PUBLISHER_NAMESPACE")
            };
            this.config.Expand();

            this.coroutineRunner = new CoroutineRunner();
            this.httpClient = new AccelByteHttpClient();
            this.httpClient.SetCredentials(this.config.ClientId, this.config.ClientSecret);
        }

        public void CreateType(string namespace_, string accessToken, string type, string[] subtype, ResultCallback<UGCTypeResponse> callback)
        {
            this.coroutineRunner.Run(CreateTypeAsync(namespace_, accessToken, type, subtype, callback));
        }

        public void CreateTags(string namespace_, string accessToken, string tags, ResultCallback<UGCTagResponse> callback)
        {
            this.coroutineRunner.Run(CreateTagsAsync(namespace_, accessToken, tags, callback));
        }

        public void DeleteType(string namespace_, string accessToken, string typeId, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteTypeAsync(namespace_, accessToken, typeId, callback));
        }

        public void DeleteTag(string namespace_, string accessToken, string tagId, ResultCallback callback)
        {
            this.coroutineRunner.Run(DeleteTagAsync(namespace_, accessToken, tagId, callback));
        }

        public IEnumerator CreateTypeAsync(string namespace_, string accessToken, string type, string[] subtype, ResultCallback<UGCTypeResponse> callback)
        {
            string subTypeString = "";
            foreach (string stype in subtype)
            {
                if (subTypeString != "")
                {
                    subTypeString += ",";
                }
                subTypeString += "\"" + stype + "\"";
            }

            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.UGCServerUrl + "/v1/admin/namespaces/{namespace}/types")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"subtype\": [{0}], \"type\": \"{1}\" }}", subTypeString, type))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UGCTypeResponse>();
            callback.Try(result);
        }

        public IEnumerator CreateTagsAsync(string namespace_, string accessToken, string tags, ResultCallback<UGCTagResponse> callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreatePost(this.config.UGCServerUrl + "/v1/admin/namespaces/{namespace}/tags")
                .WithPathParam("namespace", namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"tag\": \"{0}\" }}", tags))
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UGCTagResponse>();
            callback.Try(result);
        }

        public IEnumerator DeleteTypeAsync(string namespace_, string accessToken, string typeId, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.UGCServerUrl + "/v1/admin/namespaces/{namespace}/types/{typeId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("typeId", typeId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator DeleteTagAsync(string namespace_, string accessToken, string tagId, ResultCallback callback)
        {
            IHttpRequest request = HttpRequestBuilder
                .CreateDelete(this.config.UGCServerUrl + "/v1/admin/namespaces/{namespace}/tags/{tagId}")
                .WithPathParam("namespace", namespace_)
                .WithPathParam("tagId", tagId)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;
            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

    }
}
