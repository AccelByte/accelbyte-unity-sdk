// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using AccelByte.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public class PresenceBroadcastEventScheduler : AnalyticsEventScheduler
    {
        private readonly PresenceBroadcastEvent pbeWrapper = null;
        private readonly PresenceBroadcastEventGatherer gatherer;
        private PresenceBroadcastEventPayload presenceBroadcastPayloadModel = null;
        
        private ResultCallback onPresenceBroadcastEventResponse = null;
        internal PresenceEventRuntimeConfig Config = null;

        protected override int defaultEventIntervalInlMs 
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

        public PresenceBroadcastEventScheduler(PresenceBroadcastEvent pbeWrapper) : base(pbeWrapper)
        {
            eventIntervalInlMs = defaultEventIntervalInlMs;
            this.pbeWrapper = pbeWrapper;
            RunValidator();

            if (pbeWrapper.GetConfig().PresenceBroadcastEventInterval <= 0)
            {
                pbeWrapper.GetConfig().PresenceBroadcastEventInterval = (int)Utils.TimeUtils.MillisecondsToSeconds(defaultEventIntervalInlMs);
            }

            Config = new PresenceEventRuntimeConfig(
                (PresenceBroadcastEventGameState)pbeWrapper.GetConfig().PresenceBroadcastEventGameState, 
                pbeWrapper.GetConfig().EnablePresenceBroadcastEvent,
                Utils.TimeUtils.SecondsToMilliseconds(pbeWrapper.GetConfig().PresenceBroadcastEventInterval),
                pbeWrapper.GetConfig().PresenceBroadcastEventGameStateDescription);

            gatherer = new PresenceBroadcastEventGatherer();
        }

        internal void SetPresenceBroadcastEventCallback(ResultCallback onPBEResponse)
        {
            onPresenceBroadcastEventResponse = onPBEResponse;
        }

        internal void SetPresenceBroadcastEventEnabled(bool enabled, int intervalInMs = 0)
        {
            bool enablePbe = false;
#if !UNITY_SERVER
            enablePbe = enabled;
#endif

            SetEventEnabled(enablePbe, intervalInMs);
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

        public void SetPresenceBroadcastEventInterval(int newIntervalInMs)
        {
            if (maintainer != null)
            {
                AccelByteDebug.LogWarning("Presence Broadcast Event is still running, please stop it before changing the interval");
                return;
            }
            Config.IntervalInMs = newIntervalInMs;
            SetInterval(newIntervalInMs);
        }

        public void SetPresenceBroadcastEventInterval(float newIntervalInS)
        {
            SetPresenceBroadcastEventInterval(UnityEngine.Mathf.RoundToInt(newIntervalInS) * 1000);
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
            Config.GameState = gameState;
        }

        public void SetGameStateDescription(string gameStateDescription)
        {
            Config.GameStateDescription = gameStateDescription;
        }

        #region Data Preparation
        internal TelemetryBody PrepareData()
        {
            ProvidePBEPayloadData();

            TelemetryBody body = new TelemetryBody()
            {
                EventNamespace = pbeWrapper.GetSession().Namespace,
                EventName = PresenceBroadcastEventGatherer.EventName,
                Payload = presenceBroadcastPayloadModel
            };

            return body;
        }

        internal TelemetryBody PrepareData(IAccelByteTelemetryEvent telemetryEvent)
        {
            ProvidePBEPayloadData();

            string eventNamespace = pbeWrapper.GetSession().Namespace;
            if(string.IsNullOrEmpty(eventNamespace))
            {
                eventNamespace = pbeWrapper.GetConfig().Namespace;
            }

            TelemetryBody body = new TelemetryBody(telemetryEvent)
            {
                EventNamespace = eventNamespace,
                EventName = PresenceBroadcastEventGatherer.EventName,
                Payload = presenceBroadcastPayloadModel
            };

            return body;
        }

        private void ProvidePBEPayloadData()
        {
            presenceBroadcastPayloadModel = new PresenceBroadcastEventPayload
            {
                FlightId = PresenceBroadcastEventGatherer.GetFlightID(),
                GameState = GetGameState(),
                GameContext = GetGameContext(),
                PlatformInfo = gatherer.GetPlatformName(),
                AdditionalData = gatherer.GetAdditionalData()
            };
        }

        public new void Dispose()
        {
            keepValidating = false;
            base.Dispose();
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
            pbeWrapper.SendData(PrepareData(), onPresenceBroadcastEventResponse);
        }

        #endregion

        #region Data Gathering
        private string GetGameState()
        {
            var rawState = Config.GameState;
            return Utils.JsonUtils.SerializeWithStringEnum(rawState);
        }

        private string GetGameContext()
        {
            return Config.GameStateDescription;
        }

        #endregion
    }
}