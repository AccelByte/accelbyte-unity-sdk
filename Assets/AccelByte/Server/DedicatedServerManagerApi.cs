// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;
using UnityEngine;

namespace AccelByte.Server
{
    internal class DedicatedServerManagerApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;
        private readonly string namespace_;

        internal DedicatedServerManagerApi(string baseUrl, string namespace_, IHttpWorker httpWorker)
        {
            Debug.Log("ServerApi init serverapi start");
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsFalse(
                string.IsNullOrEmpty(namespace_),
                "Creating " + GetType().Name + " failed. Parameter namespace is null.");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.namespace_ = namespace_;
            this.httpWorker = httpWorker;
        }

        public IEnumerator RegisterServer(RegisterServerRequest registerRequest, string accessToken,
            ResultCallback callback)
        {
            Assert.IsNotNull(registerRequest, "Register failed. registerserverRequest is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/servers/register")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(registerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator ShutdownServer(ShutdownServerRequest shutdownServerRequest, string accessToken,
            ResultCallback callback)
        {
            Assert.IsNotNull(shutdownServerRequest, "Register failed. shutdownServerNotif is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/servers/shutdown")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(shutdownServerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }

        public IEnumerator SendHeartBeat(string name, string accessToken, ResultCallback<MatchRequest> callback)
        {
            Assert.IsNotNull(name, "Deregister failed. name is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/servers/heartbeat")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"name\": \"{0}\"}}", name))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            if (response.BodyBytes == null || response.BodyBytes.Length == 0)
            {
                callback.Try(null);
            }
            else
            {
                var result = response.TryParseJson<MatchRequest>();
                callback.Try(result);
            }
        }

        public IEnumerator RegisterLocalServer(RegisterLocalServerRequest registerRequest, string accessToken,
            ResultCallback callback)
        {
            Assert.IsNotNull(registerRequest, "Register failed. registerRequest is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/servers/local/register")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(registerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
        
        public IEnumerator DeregisterLocalServer(string name, string accessToken, ResultCallback callback)
        {
            Assert.IsNotNull(name, "Deregister failed. name is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/servers/local/deregister")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"name\": \"{0}\"}}", name))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            callback.Try(result);
        }
    }
}
