// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;

namespace AccelByte.Core
{
    public class PredefinedEventScheduler : AnalyticsEventScheduler
    {
        protected override int defaultEventIntervalInMs
        {
            get
            {
                const int defaultIntervalMs = 1 * 60 * 1000;
                return defaultIntervalMs;
            }
        }

        public PredefinedEventScheduler(IAccelByteAnalyticsWrapper analyticsWrapper) : base(analyticsWrapper)
        {
        }

        public override void SendEvent(IAccelByteTelemetryEvent telemetryEvent, ResultCallback callback)
        {
            TelemetryBody eventBody = new TelemetryBody(telemetryEvent);
            if(analyticsWrapper != null && analyticsWrapper.GetSession().IsValid())
            {
                eventBody.EventNamespace = analyticsWrapper.GetSession().Namespace;
            }

            jobQueue.Enqueue(new System.Tuple<TelemetryBody, ResultCallback>(eventBody, callback));
            OnTelemetryEventAdded?.Invoke(eventBody);
        }

        internal System.Tuple<TelemetryBody, ResultCallback>[] GetJobQueue()
        {
            System.Tuple<TelemetryBody, ResultCallback>[] retval = jobQueue.ToArray();
            return retval;
        }

        protected override void TriggerSend()
        {
            const bool overrideEventNamespaceOnSend = true;
            CommonTriggerSend(overrideEventNamespaceOnSend);
        }

        protected override void RunValidator()
        {
            RunCommonValidator();
        }
    }
}