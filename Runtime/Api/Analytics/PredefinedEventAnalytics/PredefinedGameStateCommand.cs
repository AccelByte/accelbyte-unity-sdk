// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Models;

namespace AccelByte.Core
{
    internal class PredefinedGameStateCommand
    {
        internal static PredefinedGameStateCommand GlobalGameStateCommand
        {
            get;
            private set;
        }

        static bool eventControllerSet;
        private static PredefinedEventScheduler targetEventScheduler;
        private static bool acceptEvent;

        public PredefinedGameStateCommand(bool setAsSDKGlobal)
        {
            if (setAsSDKGlobal)
            {
                GlobalGameStateCommand = this;
            }
        }

        internal void StartAcceptEvent()
        {
            acceptEvent = true;
        }

        internal void StopAcceptEvent()
        {
            acceptEvent = false;
        }

        internal void ExecGameStartEvent(PredefinedEventOnGameStartParameter param)
        {
            var gameLaunchedEvent = new PredefinedGameLaunchedPayload(param.ProductName, param.ProductVersion);
            SendEvent(gameLaunchedEvent);
        }

        internal void ExecSdkInitializedEvent(PredefinedEventSdkInitializedParameter param, System.Action onSent)
        {
            var sdkInitializedEvent = new PredefinedSDKInitializedPayload("", param.SDKVersion);
            SendEvent(sdkInitializedEvent, onSent);
        }

        internal void SetPredefinedEventScheduler(ref PredefinedEventScheduler eventController)
        {
            if (!eventControllerSet)
            {
                targetEventScheduler = eventController;
                eventControllerSet = true;
            }
        }

        private async void SendEvent(IAccelByteTelemetryPayload newPayload, System.Action onSent = null)
        {
            var telemetryEvent = new AccelByteTelemetryEvent(newPayload);

            while (targetEventScheduler == null && acceptEvent)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }

            if (!acceptEvent)
            {
                return;
            }

            targetEventScheduler.SendEvent(telemetryEvent, null);
            onSent?.Invoke();
        }
    }

    internal struct PredefinedEventOnGameStartParameter
    {
        internal string ProductName;
        internal string ProductVersion;

        public PredefinedEventOnGameStartParameter(string productName, string productVersion)
        {
            ProductName = productName;
            ProductVersion = productVersion;
        }
    }

    internal struct PredefinedEventOnGamePauseParameter
    {
        internal string ProductName;
        internal string ProductVersion;
        internal string Reason;

        public PredefinedEventOnGamePauseParameter(string productName, string productVersion, string reason)
        {
            ProductName = productName;
            ProductVersion = productVersion;
            Reason = reason;
        }
    }

    internal struct PredefinedEventSdkInitializedParameter
    {
        internal const string LowLevelEventName = "";

        internal string Name;
        internal string SDKVersion;

        public PredefinedEventSdkInitializedParameter(string name, string sDKVersion)
        {
            Name = name;
            SDKVersion = sDKVersion;
        }
    }
}
