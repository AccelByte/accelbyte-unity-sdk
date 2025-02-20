// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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

    [Preserve]
    public class MissionStartedOptionalParameters
    {
        public string MissionType;
        public string MissionName;
        public string MissionLocation;
        public string MissionDifficulty;
    }
    
    [DataContract, Preserve]
    internal class MissionStartedEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")] public string UserId;
        [DataMember(Name = "missionID")] public string MissionId;
        [DataMember(Name = "missionInstanceID")] public string MissionInstanceId;
        [DataMember(Name = "missionType"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MissionType;
        [DataMember(Name = "missionName"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MissionName;
        [DataMember(Name = "missionLocation"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MissionLocation;
        [DataMember(Name = "missionDifficulty"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MissionDifficulty;

        internal MissionStartedEventPayload(
            AccelByteUserId userId
            , MissionId missionId
            , MissionInstanceId missionInstanceId
            , MissionStartedOptionalParameters optionalParameters)
        {
            UserId = userId.ToString();
            MissionId = missionId.ToString();
            MissionInstanceId = missionInstanceId.ToString();
            this.CopyField(optionalParameters);
        }

        protected override string GetGameStandardModelName()
        {
            return "mission_Started";
        }
    }

    [Preserve]
    public class MissionStepEndedOptionalParameters
    {
        public string MissionStepName;
    }

    [DataContract, Preserve]
    internal class MissionStepEndedEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")]
        public string UserId;
        [DataMember(Name = "missionID")]
        public string MissionId;
        [DataMember(Name = "missionInstanceID")]
        public string MissionInstanceId;
        [DataMember(Name = "missionStep")]
        public int MissionStep;
        [DataMember(Name = "missionStepName"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MissionStepName;

        internal MissionStepEndedEventPayload(
            AccelByteUserId userId
            , MissionId missionId
            , MissionInstanceId missionInstanceId
            , MissionStep missionStep
            , MissionStepEndedOptionalParameters optionalParameters)
        {
            UserId = userId.ToString();
            MissionId = missionId.ToString();
            MissionInstanceId = missionInstanceId.ToString();
            MissionStep = missionStep.GetValue();
            this.CopyField(optionalParameters);
        }

        protected override string GetGameStandardModelName()
        {
            return "mission_Step_Ended";
        }
    }

    [Preserve]
    public class MissionEndedOptionalParameters
    {
        public string MissionOutcome;
    }

    [DataContract, Preserve]
    internal class MissionEndedEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")]
        public string UserId;
        [DataMember(Name = "missionID")]
        public string MissionId;
        [DataMember(Name = "missionInstanceID")]
        public string MissionInstanceId;
        [DataMember(Name = "missionSuccess")]
        public bool MissionSuccess;
        [DataMember(Name = "missionOutcome"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MissionOutcome;

        internal MissionEndedEventPayload(
            AccelByteUserId userId
            , MissionId missionId
            , MissionInstanceId missionInstanceId
            , MissionSuccess missionSuccess
            , MissionEndedOptionalParameters optionalParameters)
        {
            UserId = userId.ToString();
            MissionId = missionId.ToString();
            MissionInstanceId = missionInstanceId.ToString();
            MissionSuccess = missionSuccess.GetValue();
            this.CopyField(optionalParameters);
        }

        protected override string GetGameStandardModelName()
        {
            return "mission_Ended";
        }
    }

    [Preserve]
    public class MatchInfoOptionalParameters
    {
        public string MatchId;
        public string GameMode;
        public string MatchDifficulty;
    }

    [DataContract, Preserve]
    internal class MatchInfoEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "matchinfoID")]
        public string MatchInfoId;
        [DataMember(Name = "matchID"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MatchId;
        [DataMember(Name = "gameMode"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GameMode;
        [DataMember(Name = "matchDifficulty"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MatchDifficulty;

        internal MatchInfoEventPayload(MatchInfoId matchInfoId
            , MatchInfoOptionalParameters optionalParameters)
        {
            MatchInfoId = matchInfoId.ToString();
            this.CopyField(optionalParameters);
        }

        protected override string GetGameStandardModelName()
        {
            return "matchinfo";
        }
    }

    [Preserve]
    public class MatchInfoPlayerOptionalParameters
    {
        public string MatchId;
        public string Team;
        public string Class;
        public string Rank;
    }

    [DataContract, Preserve]
    internal class MatchInfoPlayerEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")]
        public string UserId;
        [DataMember(Name = "matchinfoID")]
        public string MatchInfoId;
        [DataMember(Name = "matchID"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MatchId;
        [DataMember(Name = "team"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Team;
        [DataMember(Name = "class"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Class;
        [DataMember(Name = "rank"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Rank;

        internal MatchInfoPlayerEventPayload(
            AccelByteUserId userId
            , MatchInfoId matchInfoId
            , MatchInfoPlayerOptionalParameters optionalParameters)
        {
            UserId = userId.ToString();
            MatchInfoId = matchInfoId.ToString();
            this.CopyField(optionalParameters);
        }

        protected override string GetGameStandardModelName()
        {
            return "matchinfo_Player";
        }
    }

    [Preserve]
    public class MatchInfoEndedOptionalParameters
    {
        public string MatchId;
        public string Winner;
    }

    [DataContract, Preserve]
    internal class MatchInfoEndedEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "matchinfoID")]
        public string MatchInfoId;
        [DataMember(Name = "endReason")]
        public string EndReason;
        [DataMember(Name = "matchID"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MatchId;
        [DataMember(Name = "winner"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Winner;

        internal MatchInfoEndedEventPayload(
            MatchInfoId matchInfoId
            , MatchEndReason matchEndReason
            , MatchInfoEndedOptionalParameters optionalParameters)
        {
            MatchInfoId = matchInfoId.ToString();
            EndReason = matchEndReason.ToString();
            this.CopyField(optionalParameters);
        }

        protected override string GetGameStandardModelName()
        {
            return "matchinfo_Ended";
        }
    }

    [Preserve]
    public class PopupAppearOptionalParameters
    {
        public string PopupType;
        public string PopupName;
    }

    [DataContract, Preserve]
    internal class PopupAppearEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")]
        public string UserId;
        [DataMember(Name = "popupID")]
        public string PopupId;
        [DataMember(Name = "popupType"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PopupType;
        [DataMember(Name = "popupName"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PopupName;

        internal PopupAppearEventPayload(
            AccelByteUserId userId
            , PopupId popupId
            , PopupAppearOptionalParameters optionalParameters)
        {
            UserId = userId.ToString();
            PopupId = popupId.ToString();
            this.CopyField(optionalParameters);
        }

        protected override string GetGameStandardModelName()
        {
            return "popup_Appear";
        }
    }

    [Preserve]
    public class EntityLeveledOptionalParameters
    {
        public AccelByteUserId UserId;
        public EntityId EntityId;
        public string LevelStat;
        public int? LevelChange;
        public int? LevelCurrent;
    }

    [DataContract, Preserve]
    internal class EntityLeveledEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "entityType")]
        public string EntityType;
        [DataMember(Name = "userID"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserId;
        [DataMember(Name = "entityID"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntityId;
        [DataMember(Name = "levelStat"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LevelStat;
        [DataMember(Name = "levelChange"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? LevelChange;
        [DataMember(Name = "levelCurrent"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? LevelCurrent;

        internal EntityLeveledEventPayload(
            EntityType entityType
            , EntityLeveledOptionalParameters optionalParameters)
        {
            EntityType = entityType.ToString();
            
            if (optionalParameters == null)
            {
                return;
            }

            if (optionalParameters.UserId != null && optionalParameters.UserId.IsValid())
            {
                UserId = optionalParameters.UserId.ToString();
            }

            if (optionalParameters.EntityId != null && optionalParameters.EntityId.IsValid())
            {
                EntityId = optionalParameters.EntityId.ToString();
            }
            
            LevelStat = optionalParameters.LevelStat;
            LevelChange = optionalParameters.LevelChange;
            LevelCurrent = optionalParameters.LevelCurrent;
        }

        protected override string GetGameStandardModelName()
        {
            return "entity_Leveled";
        }
    }

    [Preserve]
    public class EntityDeadOptionalParameters
    {
        public AccelByteUserId UserId;
        public EntityId EntityId;
        public int? DeathDay;
        public string DeathLocation;
        public string DeathType;
        public string DeathSource;
    }

    [DataContract, Preserve]
    internal class EntityDeadEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "entityType")]
        public string EntityType;
        [DataMember(Name = "userID"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserId;
        [DataMember(Name = "entityID"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EntityId;
        [DataMember(Name = "deathDay"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? DeathDay;
        [DataMember(Name = "deathLocation"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeathLocation;
        [DataMember(Name = "deathType"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeathType;
        [DataMember(Name = "deathSource"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeathSource;

        internal EntityDeadEventPayload(
            EntityType entityType
            , EntityDeadOptionalParameters optionalParameters)
        {
            EntityType = entityType.ToString();

            if (optionalParameters == null)
            {
                return;
            }

            if (optionalParameters.UserId != null && optionalParameters.UserId.IsValid())
            {
                UserId = optionalParameters.UserId.ToString();
            }

            if (optionalParameters.EntityId != null && optionalParameters.EntityId.IsValid())
            {
                EntityId = optionalParameters.EntityId.ToString();
            }

            DeathDay = optionalParameters.DeathDay;
            DeathLocation = optionalParameters.DeathLocation;
            DeathType = optionalParameters.DeathType;
            DeathSource = optionalParameters.DeathSource;
        }

        protected override string GetGameStandardModelName()
        {
            return "entity_Dead";
        }
    }

    [DataContract, Preserve]
    internal class ResourceFlowEventPayload : GameStandardEventPayload, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")] public string UserId;
        [DataMember(Name = "flowType")] public string FlowType;
        [DataMember(Name = "transactionID")] public string TransactionId;
        [DataMember(Name = "transactionType")] public string TransactionType;
        [DataMember(Name = "resourceName")] public string ResourceName;
        [DataMember(Name = "amount")] public string Amount;
        [DataMember(Name = "endBalance")] public string EndBalance;

        internal ResourceFlowEventPayload(
            AccelByteUserId userId
            , ResourceFlowType flowType
            , TransactionId transactionId
            , TransactionType transactionType
            , ResourceName resourceName
            , ResourceAmount amount
            , ResourceEndBalance endBalance)
        {
            UserId = userId.ToString();
            FlowType = flowType.ToString();
            TransactionId = transactionId.ToString();
            TransactionType = transactionType.ToString();
            ResourceName = resourceName.ToString();
            Amount = amount.ToString();
            EndBalance = endBalance.ToString();
        }

        protected override string GetGameStandardModelName()
        {
            return "resource_Flow";
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum FlowType
    {
        [EnumMember(Value = "sink")]
        Sink,
        [EnumMember(Value = "source")]
        Source
    }

    public class AccelByteUserId : AccelByteId
    {
        public AccelByteUserId(string id)
            : base(id) { }
    }

    public class MissionInstanceId : AccelByteId
    {
        public MissionInstanceId(string id)
            : base(id) { }
    }

    public class EntityId : AccelByteId
    {
        public EntityId(string id)
            : base(id) { }
    }

    public class MissionId : FreeFormData
    {
        public MissionId(string id)
            : base(id) { }
    }

    public class MissionStep : NumberData
    {
        public MissionStep(int step)
            : base(step)
        {
            if (step < 1) // invalid step
            {
                data = null;
            }
        }

        internal int GetValue()
        {
            return base.ToInt();
        }
    }

    public class MissionSuccess : NumberData
    {
        public MissionSuccess(bool success)
            : base(success) { }

        public MissionSuccess(string success)
            : base(success) { }

        internal bool GetValue()
        {
            return base.ToBoolean();
        }
    }

    public class MatchInfoId : FreeFormData
    {
        public MatchInfoId(string matchInfoId)
            : base(matchInfoId) { }
    }

    public class MatchEndReason : FreeFormData
    {
        public MatchEndReason(string matchEndReason)
            : base(matchEndReason) { }
    }

    public class PopupId : FreeFormData
    {
        public PopupId(string popupId)
            : base(popupId) { }
    }

    public class EntityType : FreeFormData
    {
        public EntityType(string entityType)
            : base(entityType) { }
    }

    public class TransactionId : AccelByteId
    {
        public TransactionId(string transactionId)
            : base(transactionId) { }
    }

    public class ResourceFlowType : FreeFormData
    {
        public ResourceFlowType(string resourceFlowType)
            : base(null)
        {
            if (Enum.TryParse(resourceFlowType, out FlowType _))
            {
                data = resourceFlowType;
            }
        }

        public ResourceFlowType(FlowType resourceFlowType)
            : base(resourceFlowType.ToString())
        {
        }
    }

    public class TransactionType : FreeFormData
    {
        public TransactionType(string transactionType)
            : base(transactionType) { }
    }

    public class ResourceName : FreeFormData
    {
        public ResourceName(string resourceName)
            : base(resourceName) { }
    }

    public class ResourceAmount : StringNumberData
    {
        public ResourceAmount(string resourceAmount)
            : base(resourceAmount) { }

        public ResourceAmount(int resourceAmount)
            : base(resourceAmount) { }

        public ResourceAmount(float resourceAmount)
            : base(resourceAmount) { }
    }

    public class ResourceEndBalance : StringNumberData
    {
        public ResourceEndBalance(string resourceEndBalance)
            : base(resourceEndBalance) { }

        public ResourceEndBalance(int resourceEndBalance)
            : base(resourceEndBalance) { }

        public ResourceEndBalance(float resourceEndBalance)
            : base(resourceEndBalance) { }
    }
}