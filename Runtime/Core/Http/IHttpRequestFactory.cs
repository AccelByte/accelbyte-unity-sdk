// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

namespace AccelByte.Core
{
	internal interface IHttpRequestSenderFactory
    {
        IHttpRequestSender CreateHttpRequestSender();
        void ResetHttpRequestSenders();
    }

    internal class AccelByteSDKHttpRequestFactory : IHttpRequestSenderFactory
    {
        public System.Action<WebRequestScheduler> OnWebRequestSchedulerCreated;
        private CoreHeartBeat coreHeartBeat;
        private readonly System.Collections.Generic.List<IHttpRequestSender> createdHttpRequestSenderRecord;
        public AccelByteSDKHttpRequestFactory(CoreHeartBeat coreHeartBeat)
        {
            this.coreHeartBeat = coreHeartBeat;
            createdHttpRequestSenderRecord = new System.Collections.Generic.List<IHttpRequestSender>();
        }

        public IHttpRequestSender CreateHttpRequestSender()
        {
            var httpSenderScheduler = new WebRequestScheduler();
            var sdkHttpSender = new UnityHttpRequestSender(httpSenderScheduler);
            sdkHttpSender.SetHeartBeat(coreHeartBeat);
            createdHttpRequestSenderRecord.Add(sdkHttpSender);
            OnWebRequestSchedulerCreated?.Invoke(httpSenderScheduler);
            
            return sdkHttpSender;
        }

        public void ResetHttpRequestSenders()
        {
            foreach(var httpRequestSender in createdHttpRequestSenderRecord)
            {
                httpRequestSender.ClearTasks();
            }
        }
    }
}