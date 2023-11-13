// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public abstract class GameStandardEventPayload : IAccelByteTelemetryPayload
    {
        internal const string EventType = "GameStandardEvent";
        [DataMember(Name = "gameStandardEventName")] public string GameStandardEventName;

        public GameStandardEventPayload()
        {
            GameStandardEventName = GetGameStandardModelName();
        }

        public string GetEventName()
        {
            return EventType;
        }

        protected abstract string GetGameStandardModelName();
    }

    [DataContract, Preserve]
    public abstract class GameStandardResourceEventPayloadBase : GameStandardEventPayload
    {
        [DataMember(Name = "resourceName")] public string ResourceName;
        [DataMember(Name = "resourceID")] public string ResourceID;
        [DataMember(Name = "resourceCategory")] public string ResourceCategory;
        [DataMember(Name = "resourceRating")] public string ResourceRating;
        [DataMember(Name = "resourceSource")] public string ResourceSource;
        [DataMember(Name = "resourceLevelRequirement")] public string ResourceLevelRequirement;
        [DataMember(Name = "resourceRarity")] public int ResourceRarity;
    }

    [DataContract, Preserve]
    public class ResourceSourcedEventPayload : GameStandardResourceEventPayloadBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "sourcedAmount")] public string SourcedAmount;

        protected override string GetGameStandardModelName()
        {
            return "resource_Sourced";
        }
    }

    [DataContract, Preserve]
    public class ResourceSinkedEventPayload : GameStandardResourceEventPayloadBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "sinkAmount")] public string SinkAmount;
        [DataMember(Name = "sinkedReason")] public string SinkedReason;

        protected override string GetGameStandardModelName()
        {
            return "resource_Sinked";
        }
    }

    [DataContract, Preserve]
    public class ResourceUpgradedEventPayload : GameStandardResourceEventPayloadBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "upgradeType")] public string UpgradeType;
        [DataMember(Name = "upgradeSource")] public string UpgradeSource;
        [DataMember(Name = "upgradeLevel")] public string UpgradeLevel;

        protected override string GetGameStandardModelName()
        {
            return "resource_Upgraded";
        }
    }

    [DataContract, Preserve]
    public class ResourceActionedEventPayload : GameStandardResourceEventPayloadBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "equipLocation")] public string EquipLocation;
        [DataMember(Name = "actionName")] public string ActionName;
        [DataMember(Name = "actionTarget")] public string ActionTarget;

        protected override string GetGameStandardModelName()
        {
            return "resource_Actioned";
        }
    }

    [DataContract, Preserve]
    public abstract class GameStandardQuestEventBasePayload : GameStandardEventPayload
    {
        [DataMember(Name = "questName")] public string QuestName;
        [DataMember(Name = "questID")] public string QuestID;
        [DataMember(Name = "questType")] public string QuestType;
    }

    [DataContract, Preserve]
    public class QuestStartedEventPayload : GameStandardQuestEventBasePayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "questDifficulty")] public string QuestDifficulty;

        protected override string GetGameStandardModelName()
        {
            return "quest_Started";
        }
    }

    [DataContract, Preserve]
    public class QuestEndedEventPayload : GameStandardQuestEventBasePayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "questOutcome")] public string QuestOutcome;
        [DataMember(Name = "questDifficulty")] public string QuestDifficulty;

        protected override string GetGameStandardModelName()
        {
            return "quest_Ended";
        }
    }

    [DataContract, Preserve]
    public class PlayerLeveledEventPayload : GameStandardEventPayload
    {
        [DataMember(Name = "levelStatus")] public string LevelStatus;
        [DataMember(Name = "levelID")] public string LevelID;
        [DataMember(Name = "levelName")] public string LevelName;
        [DataMember(Name = "levelDifficulty")] public string LevelDifficulty;
        [DataMember(Name = "levelSubject")] public string LevelSubject;

        protected override string GetGameStandardModelName()
        {
            return "player_Leveled";
        }
    }

    [DataContract, Preserve]
    public class PlayerDeadEventPayload : GameStandardEventPayload
    {
        [DataMember(Name = "deathTimeStamp")] public string DeathTimeStamp;
        [DataMember(Name = "deathType")] public string DeathType;
        [DataMember(Name = "deathCause")] public string deathCause;
        [DataMember(Name = "deathLocation")] public string DeathLocation;

        protected override string GetGameStandardModelName()
        {
            return "player_Dead";
        }
    }

    [DataContract, Preserve]
    public class RewardCollectedEventPayload : GameStandardEventPayload
    {
        [DataMember(Name = "rewardName")] public string RewardName;
        [DataMember(Name = "rewardType")] public string RewardType;
        [DataMember(Name = "rewardAmount")] public string RewardAmount;

        protected override string GetGameStandardModelName()
        {
            return "reward_Collected";
        }
    }
}