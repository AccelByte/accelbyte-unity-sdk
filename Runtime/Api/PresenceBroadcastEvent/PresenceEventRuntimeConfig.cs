// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    public class AnalyticEventRuntimeConfig
    {
        private const float minimumAllowedIntervalInMs = 5 * 1000;

        public AnalyticEventRuntimeConfig(bool isEnabled, float intervalInMs = minimumAllowedIntervalInMs)
        {
            Enabled = isEnabled;
            this.intervalInMs = intervalInMs;
        }

        public float IntervalInMs
        {
            get
            {
                return intervalInMs;
            }
            set
            {
                if (value <= 0)
                {
                    if(intervalInMs != 0)
                    {
                        AccelByteDebug.LogWarning($"The interval must be more than 0!. Reverting to previous interval {intervalInMs}ms");
                    }
                    else
                    {
                        AccelByteDebug.LogWarning($"The interval must be more than 0!. Reverting to minimum allowed interval {minimumAllowedIntervalInMs}ms");
                        intervalInMs = minimumAllowedIntervalInMs;
                    }
                }
                else
                {
                    intervalInMs = value;
                }
            }
        }
        public bool Enabled;

        private float intervalInMs;
    }

    public class PresenceEventRuntimeConfig : AnalyticEventRuntimeConfig
    {
        internal const string EventName = "enhanced_presence";

        internal PresenceBroadcastEventGameState GameState;
        internal string GameStateDescription;

        public PresenceEventRuntimeConfig(
            PresenceBroadcastEventGameState initialState, 
            bool isEnabled, 
            float intervalInMs,
            string gameStateDescription)
            : base (isEnabled, intervalInMs) 
        {
            GameState = initialState;
            GameStateDescription = gameStateDescription;
        }
    }
}