// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Models;
using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public class PresenceBroadcastEventScheduler : AnalyticsEventScheduler
    {
        private readonly PresenceBroadcastEventGatherer gatherer;
        private PresenceBroadcastEventPayload presenceBroadcastPayloadModel = null;

        private string defaultNamespace = null;
        PresenceBroadcastEventGameState currentGameState;
        string currentGameStateDescription;

        protected override int defaultEventIntervalInMs 
        {
            get
            {
                const int defaultIntervalMs = 10 * 60 * 1000;
                return defaultIntervalMs;
            }
        }

        public int PresenceBroadcastEventIntervalMs
        {
            get
            {
                return eventIntervalInlMs;
            }
        }

        public bool IsPresenceBroadcastEventJobRunning
        {
            get
            {
                if (maintainer == null)
                {
                    return false;
                }

                return maintainer.IsHeartBeatJobRunning;
            }
        }

        public bool IsPresenceBroadcastEventJobEnabled
        {
            get
            {
                if (maintainer == null)
                {
                    return false;
                }

                return maintainer.IsHeartBeatEnabled;
            }
        }

        public bool IsPresenceBroadcastEventPaused
        {
            get
            {
                if (maintainer == null)
                {
                    return false;
                }

                return maintainer.IsHeartBeatPaused;
            }
        }

        public PresenceBroadcastEventPayload PresenceBroadcastEventPayloadModel
        {
            get
            {
                return presenceBroadcastPayloadModel;
            }
        }

        public Dictionary<string, object> PresenceBroadcastEventAdditionalData
        {
            get
            {
                return gatherer.GetAdditionalData();
            }
        }

        public PresenceBroadcastEventScheduler(AnalyticsService analyticsWrapper, string defaultNamespace, PresenceBroadcastEventGameState initialGameState, string initialGameStateDescription) : base(analyticsWrapper)
        {
            eventIntervalInlMs = defaultEventIntervalInMs;

            currentGameState = initialGameState;
            currentGameStateDescription = initialGameStateDescription;
            this.defaultNamespace = defaultNamespace;
            gatherer = new PresenceBroadcastEventGatherer();
        }

        [Obsolete("Please set presence interval from SDK config")]
        public void SetPresenceBroadcastEventInterval(int newIntervalInMs)
        {
        }

        [Obsolete("Please set presence interval from SDK config")]
        public void SetPresenceBroadcastEventInterval(float newIntervalInS)
        {
        }

        public void AddSendData(string key, string data)
        {
            gatherer.AddAdditionalData(key, data);
        }

        public bool RemoveSendData(string key)
        {
            return gatherer.RemoveAdditionalData(key);
        }

        public void SetGameState(PresenceBroadcastEventGameState gameState)
        {
            currentGameState = gameState;
        }

        public void SetGameStateDescription(string gameStateDescription)
        {
            currentGameStateDescription = gameStateDescription;
        }

        public PresenceBroadcastEventGameState GetGameState()
        {
            PresenceBroadcastEventGameState retval = currentGameState;
            return retval;
        }

        public string GetGameStateDescription()
        {
            string retval = currentGameStateDescription;
            return retval;
        }

        #region Internal Implementation
        internal void StartPresenceEvent(int intervalInMs = 0)
        {
            bool enablePbe = true;
#if UNITY_SERVER
            enablePbe = false;
#endif

            SetEventEnabled(enablePbe, intervalInMs);
        }

        internal void StopPresenceEvent()
        {
            const bool enablePbe = false;
            SetEventEnabled(enablePbe);
        }

        internal TelemetryBody PrepareData()
        {
            ProvidePBEPayloadData();

            TelemetryBody body = new TelemetryBody()
            {
                EventName = PresenceBroadcastEventGatherer.EventName,
                Payload = presenceBroadcastPayloadModel
            };
            if (SharedMemory != null && SharedMemory.TimeManager != null)
            {
                AccelByteGameTelemetryApi.TryAssignTelemetryBodyClientTimestamps(ref body, ref SharedMemory.TimeManager);
            }

            return body;
        }

        internal TelemetryBody PrepareData(IAccelByteTelemetryEvent telemetryEvent)
        {
            ProvidePBEPayloadData();

            TelemetryBody body = new TelemetryBody(telemetryEvent)
            {
                EventName = PresenceBroadcastEventGatherer.EventName,
                Payload = presenceBroadcastPayloadModel
            };
            if (SharedMemory != null && SharedMemory.TimeManager != null)
            {
                AccelByteGameTelemetryApi.TryAssignTelemetryBodyClientTimestamps(ref body, ref SharedMemory.TimeManager);
            }

            return body;
        }

        private void ProvidePBEPayloadData()
        {
            presenceBroadcastPayloadModel = new PresenceBroadcastEventPayload
            {
                FlightId = PresenceBroadcastEventGatherer.GetFlightID(),
                GameState = GetSerializedGameState(currentGameState),
                GameContext = GetGameStateDescription(),
                PlatformInfo = gatherer.GetPlatformName(),
                AdditionalData = gatherer.GetAdditionalData()
            };
        }

        public override void SendEvent(IAccelByteTelemetryEvent telemetryEvent, ResultCallback callback)
        {
            var eventBody = PrepareData(telemetryEvent);

            jobQueue.Enqueue(new Tuple<TelemetryBody, ResultCallback>(eventBody, callback));
            OnTelemetryEventAdded?.Invoke(eventBody);
        }

        protected override void RunValidator()
        {
            RunCommonValidator();
        }

        protected override void TriggerSend()
        {
            TelemetryBody telemetry = PrepareData();
            string eventNamespace = analyticsWrapper.GetSession().Namespace;
            if (string.IsNullOrEmpty(eventNamespace))
            {
                eventNamespace = defaultNamespace;
            }
            telemetry.EventNamespace = eventNamespace;
            analyticsWrapper.SendData(telemetry, null);
        }

        internal static string GetSerializedGameState(PresenceBroadcastEventGameState state)
        {
            var rawState = state;
            string retval = Utils.JsonUtils.SerializeWithStringEnum(rawState);
            return retval;
        }
        #endregion
    }
}