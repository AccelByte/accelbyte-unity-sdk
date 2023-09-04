// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;

namespace AccelByte.Core
{
    public class ClientAnalyticsEventScheduler : AutoLoginAnalyticsEventScheduler
    {
        internal const string ClientAnalyticMainEventNamespace = "io.accelbyte.intelligence.analyticscontrollerevents";

        protected override int defaultEventIntervalInlMs
        {
            get
            {
                const int defaultIntervalMs = 1 * 60 * 1000;
                return defaultIntervalMs;
            }
        }

        public ClientAnalyticsEventScheduler(IAccelByteAnalyticsWrapper analyticsWrapper) : base(analyticsWrapper)
        {

        }

        public override void SendEvent(IAccelByteTelemetryEvent telemetryEvent, ResultCallback callback)
        {
            executeValidator = true;

            TelemetryBody eventBody = new TelemetryBody(telemetryEvent)
            {
                EventNamespace = ClientAnalyticMainEventNamespace
            };

            jobQueue.Enqueue(new System.Tuple<TelemetryBody, ResultCallback>(eventBody, callback));
            OnTelemetryEventAdded?.Invoke(eventBody);
        }

        protected override void TriggerSend()
        {
            CommonTriggerSend();
        }

        protected override void RunValidator()
        {
            RunAutoLoginValidator();
        }
    }
}
