// Copyright (c) 2019-2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AccelByte.Core
{
    public abstract class HttpOperator
    {
        public abstract IHttpClient HttpClient
        {
            get;
        }

        public abstract void SendRequest(IHttpRequest request, System.Action<IHttpResponse> response);

        public abstract void SendRequest(IHttpRequest request, System.Action<IHttpResponse, Error> response);
    }

    public class HttpAsyncOperator : HttpOperator
    {
        IHttpClient httpClient;
        public override IHttpClient HttpClient
        {
            get
            {
                return httpClient;
            }
        }

        public HttpAsyncOperator(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async override void SendRequest(IHttpRequest request, System.Action<IHttpResponse> response)
        {
            HttpSendResult result = await httpClient.SendRequestAsync(request);
            response?.Invoke(result.CallbackResponse);
        }

        public async override void SendRequest(IHttpRequest request, System.Action<IHttpResponse, Error> response)
        {
            HttpSendResult result = await httpClient.SendRequestAsync(request);
            response?.Invoke(result.CallbackResponse, result.CallbackError);
        }
    }

    public class HttpCoroutineOperator : HttpOperator
    {
        IHttpClient httpClient;
        CoroutineRunner runner;

        public override IHttpClient HttpClient
        {
            get
            {
                return httpClient;
            }
        }

        public HttpCoroutineOperator(IHttpClient httpClient, CoroutineRunner runner)
        {
            this.httpClient = httpClient;
            this.runner = runner;
        }

        public override void SendRequest(IHttpRequest request, System.Action<IHttpResponse> response)
        {
            runner.Run(httpClient.SendRequest(request, response));
        }

        public override void SendRequest(IHttpRequest request, System.Action<IHttpResponse, Error> response)
        {
            runner.Run(httpClient.SendRequest(request, response));
        }
    }
}
