// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;

namespace AccelByte.Core
{
    public class ClientAnalyticsEventScheduler : AutoLoginAnalyticsEventScheduler
    {
        public const int ClientAnalyticsMiniumAllowedIntervalInlMs = 1 * 1000;

        internal const string ClientAnalyticMainEventNamespace = "io.accelbyte.intelligence.analyticscontrollerevents";

        private string defaultEventNamespace = ClientAnalyticMainEventNamespace;

        protected override int defaultEventIntervalInlMs
        {
            get
            {
                const int defaultIntervalMs = 1 * 60 * 1000;
                return defaultIntervalMs;
            }
        }

        protected override int defaultMinimumEventIntervalInlMs
        {
            get
            {
                return ClientAnalyticsMiniumAllowedIntervalInlMs;
            }
        }

        public ClientAnalyticsEventScheduler(IAccelByteAnalyticsWrapper analyticsWrapper) : base(analyticsWrapper)
        {

        }

        public void SetDefaultNamespace(string newDefaultNamespace)
        {
            defaultEventNamespace = newDefaultNamespace;
        }

        public override void SendEvent(IAccelByteTelemetryEvent telemetryEvent, ResultCallback callback)
        {
            SendEvent(telemetryEvent, defaultEventNamespace, callback);
        }

        public void SendEvent(IAccelByteTelemetryEvent telemetryEvent, string eventNamespace, ResultCallback callback)
        {
            executeValidator = true;

            TelemetryBody eventBody = new TelemetryBody(telemetryEvent)
            {
                EventNamespace = eventNamespace
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
