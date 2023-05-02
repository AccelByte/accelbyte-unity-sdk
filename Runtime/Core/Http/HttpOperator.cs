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
        public abstract void SendRequest(IHttpRequest request, System.Action<IHttpResponse> response);
    }

    public class HttpAsyncOperator : HttpOperator
    {
        IHttpClient httpClient;
        public HttpAsyncOperator(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async override void SendRequest(IHttpRequest request, System.Action<IHttpResponse> response)
        {
            HttpSendResult result = await httpClient.SendRequestAsync(request);
            response?.Invoke(result.CallbackResponse);
        }
    }

    public class HttpCoroutineOperator : HttpOperator
    {
        IHttpClient httpClient;
        CoroutineRunner runner;

        public HttpCoroutineOperator(IHttpClient httpClient, CoroutineRunner runner)
        {
            this.httpClient = httpClient;
            this.runner = runner;
        }

        public override void SendRequest(IHttpRequest request, System.Action<IHttpResponse> response)
        {
            runner.Run(httpClient.SendRequest(request, response));
        }
    }
}
