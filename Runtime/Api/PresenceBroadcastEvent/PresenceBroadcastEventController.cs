// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
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
        private PresenceBroadcastEventGatherer gatherer;
        private ResultCallback onPresenceBroadcastEventResponse = null;
        private int presenceBroadcastIntervalInlMs = Utils.TimeUtils.SecondsToMilliseconds(defaultPresenceBroadcastIntervalInlMs);
        private bool keepValidating = true;

        private const int defaultPresenceBroadcastIntervalInlMs = 10 * 60 * 1000;

        public PresenceBroadcastEventController(PresenceBroadcastEvent pbeWrapper)
        {
            this.pbeWrapper = pbeWrapper;
            this.session = pbeWrapper.GetSession();
            ValidateLoginSession();

            gatherer = new PresenceBroadcastEventGatherer(pbeWrapper.GetConfig());
        }

        public int PresenceBroadcastEventIntervalMs
        {
            get
            {
                return presenceBroadcastIntervalInlMs;
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
                intervalInMs = presenceBroadcastIntervalInlMs;
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

        public void SetPresenceBroadcastEventInterval(int newIntervalMs)
        {
            if (maintainer != null)
            {
                AccelByteDebug.LogWarning("Presence Broadcast Event is still running, please stop it before changing the interval");
                return;
            }
            presenceBroadcastIntervalInlMs = newIntervalMs;
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

        private void ProvidePBEPayloadData()
        {
            presenceBroadcastPayloadModel = new PresenceBroadcastEventPayload
            {
                FlightId = PresenceBroadcastEventGatherer.GetFlightID(),
                GameState = gatherer.GetGameState(),
                GameContext = gatherer.GetGameContext(),
                PlatformInfo = gatherer.GetPlatformName(),
                AdditionalData = gatherer.GetAdditionalData()
            };
        }

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

        public void Dispose()
        {
            StopPresenceBroadcastEventScheduler();
            keepValidating = false;
        }
    }
}