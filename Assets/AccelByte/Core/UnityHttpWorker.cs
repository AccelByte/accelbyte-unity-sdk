// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

namespace AccelByte.Core
{
    public class UnityHttpWorker : IHttpWorker
    {
        public event Action<UnityWebRequest> ServerErrorOccured;
        
        public event Action<UnityWebRequest> NetworkErrorOccured;

        public UnityHttpWorker()
        {
            SetRetryParameters();
        }

        public IEnumerator SendRequest(IHttpRequest request, Action<IHttpResponse> requestDoneCallback)
        {
            var rand = new Random();
            uint nextDelay = this.initialDelay;
            var stopwatch = new Stopwatch();
            UnityWebRequest unityWebRequest;
            stopwatch.Start();

            if (request == null)
            {
                if (requestDoneCallback != null)
                {
                    requestDoneCallback(null);
                }
                
                yield break;
            }

            do
            {
                unityWebRequest = request.GetUnityWebRequest();
                unityWebRequest.timeout = (int)(this.totalTimeout / 1000);

                Report.GetHttpRequest(request, unityWebRequest);

                yield return unityWebRequest.SendWebRequest();

                Report.GetHttpResponse(unityWebRequest);

                if (unityWebRequest.isNetworkError)
                {
                    Action<UnityWebRequest> netErrorHandler = this.NetworkErrorOccured;

                    if (netErrorHandler != null)
                    {
                        netErrorHandler(unityWebRequest);
                    }

                    if (requestDoneCallback != null)
                    {
                        requestDoneCallback(unityWebRequest.GetHttpResponse());
                    }
                    
                    yield break;
                }

                switch ((HttpStatusCode) unityWebRequest.responseCode)
                {
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    Action<UnityWebRequest> serverErrorHandler = this.ServerErrorOccured;

                    if (serverErrorHandler != null)
                    {
                        serverErrorHandler(unityWebRequest);
                    }
                    
                    float delaySeconds = (float) (0.75f * nextDelay + 0.5 * rand.NextDouble() * nextDelay) / 1000f;

                    yield return new WaitForSeconds(delaySeconds);

                    nextDelay *= 2;

                    if (nextDelay > this.maxDelay)
                    {
                        nextDelay = this.maxDelay;
                    }

                    break;

                default:
                    if (requestDoneCallback != null)
                    {
                        requestDoneCallback(unityWebRequest.GetHttpResponse());
                    }
                    
                    yield break;
                }
            }
            while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(this.totalTimeout));
            
            if (requestDoneCallback != null)
            {
                requestDoneCallback(unityWebRequest.GetHttpResponse());
            }
        }

        public void SetRetryParameters(uint totalTimeout = 60000, uint initialDelay = 1000, uint maxDelay = 30000)
        {
            this.totalTimeout = totalTimeout;
            this.initialDelay = initialDelay;
            this.maxDelay = maxDelay;
        }

        private uint totalTimeout;
        private uint initialDelay;
        private uint maxDelay;
    }
}