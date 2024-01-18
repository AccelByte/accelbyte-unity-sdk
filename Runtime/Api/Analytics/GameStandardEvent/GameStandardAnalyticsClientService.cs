// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;

namespace AccelByte.Api
{
    public class GameStandardAnalyticsClientService : GameStandardAnalyticsServiceBase<AnalyticsService>
    {
        internal const string DefaultCacheFileName = "GameStandardClientEvent.cache";
        Config config;

        public GameStandardAnalyticsClientService(AnalyticsService inApi, Config config) : base(inApi)
        {
            this.config = config;
        }

        internal void Initialize(AnalyticsService inApi, Config config)
        {
            this.config = config;
            Initialize(inApi);
        }

        internal Config GetConfig()
        {
            return config;
        }

        public void SendResourceSourcedEvent(ResourceSourcedEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendResourceSinkedEvent(ResourceSinkedEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendResourceUpgradedEvent(ResourceUpgradedEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendResourceActionedEvent(ResourceActionedEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendQuestStartedEvent(QuestStartedEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendQuestEndedEvent(QuestEndedEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendPlayerLeveledEvent(PlayerLeveledEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendPlayerDeadEvent(PlayerDeadEventPayload payload)
        {
            SendEvent(payload);
        }

        public void SendRewardCollectedEvent(RewardCollectedEventPayload payload)
        {
            SendEvent(payload);
        }
    }
}
