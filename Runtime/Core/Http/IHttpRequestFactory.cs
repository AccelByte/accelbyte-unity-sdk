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
        private readonly System.Collections.Generic.List<IHttpRequestSender> createdHttpRequestSenderRecord;
        public AccelByteSDKHttpRequestFactory()
        {
            createdHttpRequestSenderRecord = new System.Collections.Generic.List<IHttpRequestSender>();
        }

        public IHttpRequestSender CreateHttpRequestSender()
        {
            var httpSenderScheduler = new WebRequestSchedulerAsync();
            var sdkHttpSender = new UnityHttpRequestSender(httpSenderScheduler);
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