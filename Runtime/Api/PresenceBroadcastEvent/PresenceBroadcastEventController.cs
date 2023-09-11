// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace AccelByte.Api
{
    public class PresenceBroadcastEventController : IDisposable
    {
        PresenceBroadcastEvent pbeWrapper = null;
        AccelByteHeartBeat maintainer = null;
        ISession session = null;
        
        private PresenceBroadcastEventPayload presenceBroadcastPayloadModel = null;
        private PresenceBroadcastEventGatherer gatherer = null;
        private ResultCallback onPresenceBroadcastEventResponse = null;
        private bool keepValidating = true;
        private const int defaultPresenceBroadcastIntervalInlMs = 10 * 60 * 1000;
        
        internal PresenceEventRuntimeConfig Config = null;

        public PresenceBroadcastEventController(PresenceBroadcastEvent pbeWrapper)
        {
            this.pbeWrapper = pbeWrapper;
            this.session = pbeWrapper.GetSession();
            ValidateLoginSession();

            if (pbeWrapper.GetConfig().PresenceBroadcastEventInterval <= 0)
            {
                pbeWrapper.GetConfig().PresenceBroadcastEventInterval = (int)Utils.TimeUtils.MillisecondsToSeconds(defaultPresenceBroadcastIntervalInlMs);
            }

            Config = new PresenceEventRuntimeConfig(
                (PresenceBroadcastEventGameState)pbeWrapper.GetConfig().PresenceBroadcastEventGameState, 
                pbeWrapper.GetConfig().EnablePresenceBroadcastEvent,
                Utils.TimeUtils.SecondsToMilliseconds(pbeWrapper.GetConfig().PresenceBroadcastEventInterval),
                pbeWrapper.GetConfig().PresenceBroadcastEventGameStateDescription);

            gatherer = new PresenceBroadcastEventGatherer();
        }

        public int PresenceBroadcastEventIntervalMs
        {
            get
            {
                bool success = int.TryParse(Config.IntervalInMs.ToString(), out int result);
                if (success)
                {
                    return result;
                }
                else
                {
                    AccelByteDebug.LogError("Failed to parse PresenceBroadcastEvent interval");
                    return defaultPresenceBroadcastIntervalInlMs;
                }
            }
        }

        internal bool KeepValidating
        {
            get 
            { 
                return keepValidating; 
            }
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
            if (intervalInMs == 0)
            {
                bool success = int.TryParse(Config.IntervalInMs.ToString(), out int result);
                if (success)
                {
                    intervalInMs = result;
                }
                else
                {
                    AccelByteDebug.LogError("Failed to parse PresenceBroadcastEvent interval");
                    intervalInMs = defaultPresenceBroadcastIntervalInlMs;
                }
            }

            if (enablePbe)
            {
                StartPresenceBroadcastEventScheduler(intervalInMs);
            }
            else
            {
                StopPresenceBroadcastEventScheduler();
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

        public void SetPresenceBroadcastEventInterval(int newIntervalInMs)
        {
            if (maintainer != null)
            {
                AccelByteDebug.LogWarning("Presence Broadcast Event is still running, please stop it before changing the interval");
                return;
            }
            Config.IntervalInMs = newIntervalInMs;
        }

        public void SetPresenceBroadcastEventInterval(float newIntervalInS)
        {
            SetPresenceBroadcastEventInterval(UnityEngine.Mathf.RoundToInt(newIntervalInS) * 1000);
        }

        private void StartPresenceBroadcastEventScheduler(int intervalMs)
        {
            Report.GetFunctionLog(GetType().Name);

            if (maintainer != null)
            {
                maintainer.Stop();
            }
            maintainer = new AccelByteHeartBeat(intervalMs);
            maintainer.OnHeartbeatTrigger += () =>
            {
                pbeWrapper.SendData(PrepareData(), onPresenceBroadcastEventResponse);
            };
        }

        private void StopPresenceBroadcastEventScheduler()
        {
            if (maintainer != null)
            {
                maintainer.Stop();
                maintainer = null;
            }
        }

        private async void ValidateLoginSession()
        {
            Report.GetFunctionLog(GetType().Name);

            bool currentSessionValidity = session.IsValid();

            while (keepValidating)
            {
                if (maintainer != null)
                {
                    if(currentSessionValidity != session.IsValid())
                    {
                        if (session.IsValid())
                        {
                            if (!maintainer.IsHeartBeatJobRunning)
                            {
                                maintainer.Start();
                            }
                            else
                            {
                                maintainer.UnPause();
                            }
                        }
                        else
                        {
                            maintainer.Pause();
                        }

                        currentSessionValidity = session.IsValid();
                    }
                }

                await System.Threading.Tasks.Task.Delay(AccelByteHttpHelper.HttpDelayOneFrameTimeMs);
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

        public void AddSendData(string key, string data)
        {
            gatherer.AddAdditionalData(key, data);
        }

        public bool RemoveSendData(string key)
        {
            return gatherer.RemoveAdditionalData(key);
        }

        public void Dispose()
        {
            StopPresenceBroadcastEventScheduler();
            keepValidating = false;
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
                EventNamespace = pbeWrapper.GetConfig().PublisherNamespace,
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