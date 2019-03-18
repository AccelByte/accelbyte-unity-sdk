// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Net;
using System.Threading;
using UnityEngine;

namespace AccelByte.Core
{
    public class HttpTask : ITask
    {
        private readonly HttpWebRequest request;
        private readonly Action<HttpWebResponse> responseCallback;
        private readonly ManualResetEvent awaiter;
        private IAsyncResult asyncResult;

        internal HttpTask(HttpWebRequest request, Action<HttpWebResponse> responseCallback)
        {
            this.request = request;
            this.responseCallback = responseCallback;
            this.awaiter = new ManualResetEvent(false);
        }

        public event Action Completed;

        public WaitHandle GetAwaiter() { return this.awaiter; }

        public bool Execute()
        {
            if (this.asyncResult == null)
            {
                this.asyncResult = this.request.BeginGetResponse(result => this.awaiter.Set(), null);
            }
            
            if (!this.asyncResult.IsCompleted) return true;

            HttpWebResponse response;
                
            try
            {
                response = (HttpWebResponse) this.request.EndGetResponse(this.asyncResult);
            }
            catch (WebException e)
            {
                response = (HttpWebResponse) e.Response;
                    
                if (response == null)
                {
                    Debug.Log("Response is null");
                    Debug.Log(e.StackTrace);
                }
            }
                

            this.responseCallback(response);
                
            return false;
        }
    }
}