// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;

namespace AccelByte.Api
{
    public class GameStandardAnalyticsClientService : Core.GameStandardAnalyticsServiceBase<AnalyticsService>
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
        
        public bool SendMissionStartedEvent(
            AccelByteUserId userId
            , MissionId missionId
            , MissionInstanceId missionInstanceId
            , MissionStartedOptionalParameters optionalParameters = null)
        {
            if(!ValidateParams(userId, missionId, missionInstanceId))
            {
                return false;
            }

            var payload = new MissionStartedEventPayload(userId, missionId, missionInstanceId, optionalParameters);
            SendEvent(payload);
            return true;
        }

        public bool SendMissionStepEndedEvent(
            AccelByteUserId userId
            , MissionId missionId
            , MissionInstanceId missionInstanceId
            , MissionStep missionStep
            , MissionStepEndedOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(userId, missionId, missionInstanceId, missionStep))
            {
                return false;
            }

            var payload = new MissionStepEndedEventPayload(userId, missionId, missionInstanceId, missionStep, optionalParameters);
            SendEvent(payload);
            return true;
        }

        public bool SendMissionEndedEvent(
            AccelByteUserId userId
            , MissionId missionId
            , MissionInstanceId missionInstanceId
            , MissionSuccess missionSuccess
            , MissionEndedOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(userId, missionId, missionInstanceId, missionSuccess))
            {
                return false;
            }

            var payload = new MissionEndedEventPayload(userId, missionId, missionInstanceId, missionSuccess, optionalParameters);
            SendEvent(payload);
            return true;
        }

        public bool SendMatchInfoEvent(MatchInfoId matchInfoId
            , MatchInfoOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(matchInfoId))
            {
                return false;
            }

            var payload = new MatchInfoEventPayload(matchInfoId, optionalParameters);
            SendEvent(payload);
            return true;
        }

        public bool SendMatchInfoPlayerEvent(
            AccelByteUserId userId
            , MatchInfoId matchInfoId
            , MatchInfoPlayerOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(userId, matchInfoId))
            {
                return false;
            }

            var payload = new MatchInfoPlayerEventPayload(userId
                , matchInfoId
                , optionalParameters);

            SendEvent(payload);
            return true;
        }

        public bool SendMatchInfoEndedEvent(
            MatchInfoId matchInfoId
            , MatchEndReason matchEndReason
            , MatchInfoEndedOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(matchInfoId, matchEndReason))
            {
                return false;
            }

            var payload = new MatchInfoEndedEventPayload(matchInfoId
                , matchEndReason
                , optionalParameters);

            SendEvent(payload);
            return true;
        }

        public bool SendPopupAppearEvent(
            AccelByteUserId userId
            , PopupId popupId
            , PopupAppearOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(userId, popupId))
            {
                return false;
            }

            var payload = new PopupAppearEventPayload(userId, popupId, optionalParameters);
            SendEvent(payload);
            return true;
        }

        public bool SendEntityLeveledEvent(
            EntityType entityType
            , EntityLeveledOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(entityType))
            {
                return false;
            }

            var payload = new EntityLeveledEventPayload(entityType, optionalParameters);
            SendEvent(payload);
            return true;
        }

        public bool SendEntityDeadEvent(
            EntityType entityType
            , EntityDeadOptionalParameters optionalParameters = null)
        {
            if (!ValidateParams(entityType))
            {
                return false;
            }

            var payload = new EntityDeadEventPayload(entityType, optionalParameters);
            SendEvent(payload);
            return true;
        }

        public bool SendResourceFlowEvent(
            AccelByteUserId userId
            , ResourceFlowType flowType
            , TransactionId transactionId
            , TransactionType transactionType
            , ResourceName resourceName
            , ResourceAmount resourceAmount
            , ResourceEndBalance resourceEndBalance)
        {
            if (!ValidateParams(userId
                , flowType
                , transactionId
                , transactionType
                , resourceName
                , resourceAmount
                , resourceEndBalance))
            {
                return false;
            }

            var payload = new ResourceFlowEventPayload(userId
                , flowType
                , transactionId
                , transactionType
                , resourceName
                , resourceAmount
                , resourceEndBalance);

            SendEvent(payload);
            return true;
        }

        private bool ValidateParams(params BaseAnalyticsData[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return false;
            }

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] == null || !parameters[i].IsValid())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
