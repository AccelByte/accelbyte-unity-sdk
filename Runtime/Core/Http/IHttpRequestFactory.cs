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
        private CoreHeartBeat coreHeartBeat;
        private readonly System.Collections.Generic.List<IHttpRequestSender> createdHttpRequestSenderRecord;
        public AccelByteSDKHttpRequestFactory(CoreHeartBeat coreHeartBeat)
        {
            this.coreHeartBeat = coreHeartBeat;
            createdHttpRequestSenderRecord = new System.Collections.Generic.List<IHttpRequestSender>();
        }

        public IHttpRequestSender CreateHttpRequestSender()
        {
#if !UNITY_WEBGL
            var httpSenderScheduler = new WebRequestSchedulerAsync();
#else
            var httpSenderScheduler = new WebRequestSchedulerCoroutine(new CoroutineRunner());
#endif
            var sdkHttpSender = new UnityHttpRequestSender(httpSenderScheduler);
            sdkHttpSender.SetHeartBeat(coreHeartBeat);
            createdHttpRequestSenderRecord.Add(sdkHttpSender);
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