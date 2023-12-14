// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public abstract class PredefinedEventPayload : IAccelByteTelemetryPayload
    {
        internal const string EventType = "PredefinedEvent";

        [DataMember(Name = "predefinedEventName")] public string PredefinedEventName;

        public PredefinedEventPayload()
        {
            PredefinedEventName = GetPredefinedModelName();
        }

        public string GetEventName()
        {
            return EventType;
        }

        internal abstract string GetPredefinedModelName();
    }

    #region Core Game
    [DataContract, Preserve]
    public class PredefinedSDKInitializedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "version")] public string Version;

        internal override string GetPredefinedModelName()
        {
            return "SDK_Initialized";
        }

        public PredefinedSDKInitializedPayload(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameLaunchedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "gameTitle")] public string GameTitle;
        [DataMember(Name = "gameVersion")] public string GameVersion;

        internal override string GetPredefinedModelName()
        {
            return "Game_Launched";
        }

        public PredefinedGameLaunchedPayload(string gameTitle, string gameVersion)
        {
            GameTitle = gameTitle;
            GameVersion = gameVersion;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameExitedPayload : PredefinedGameModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "Game_Exited";
        }

        public PredefinedGameExitedPayload(string gameTitle, string gameVersion, string reason)
            : base(gameTitle, gameVersion, reason)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGamePausedPayload : PredefinedGameModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "Game_Paused";
        }

        public PredefinedGamePausedPayload(string gameTitle, string gameVersion, string reason)
            : base(gameTitle, gameVersion, reason)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "gameTitle")] public string GameTitle;
        [DataMember(Name = "gameVersion")] public string GameVersion;
        [DataMember(Name = "reason")] public string Reason;

        public PredefinedGameModelBase(string gameTitle, string gameVersion, string reason)
        {
            GameTitle = gameTitle;
            GameVersion = gameVersion;
            Reason = reason;
        }

        internal override string GetPredefinedModelName()
        {
            return null;
        }
    }
    #endregion

    #region Access
    [DataContract, Preserve]
    public class PredefinedLoginSucceededPayload : PredefinedLoginModelBase
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "platformUserId")] public string PlatformUserId;
        [DataMember(Name = "deviceId")] public string DeviceId;

        internal override string GetPredefinedModelName()
        {
            return "Login_Succeeded";
        }

        public PredefinedLoginSucceededPayload(string @namespace, string userId, string platformId, string platformUserId, string deviceId)
            : base(@namespace, platformId)
        {
            UserId = userId;
            PlatformUserId = platformUserId;
            DeviceId = deviceId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLoginFailedPayload : PredefinedLoginModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "Login_Failed";
        }

        public PredefinedLoginFailedPayload(string @namespace, string platformId)
            : base(@namespace, platformId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedLoginModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "platformId")] public string PlatformId;

        public PredefinedLoginModelBase(string @namespace, string platformId)
        {
            Namespace = @namespace;
            PlatformId = platformId;
        }

        internal override string GetPredefinedModelName()
        {
            return null;
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementAcceptedPayload : PredefinedAgreementDocumentBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserAgreement_Accepted";
        }

        public PredefinedAgreementAcceptedPayload(List<PredefinedAgreementDocument> agreementDocuments)
            : base(agreementDocuments)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementNotAcceptedPayload : PredefinedAgreementDocumentBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserAgreement_NotAccepted";
        }

        public PredefinedAgreementNotAcceptedPayload(List<PredefinedAgreementDocument> agreementDocuments)
            : base(agreementDocuments)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementDocumentBase : PredefinedEventPayload
    {
        [DataMember(Name = "agreementDocuments")] public List<PredefinedAgreementDocument> AgreementDocuments;

        public PredefinedAgreementDocumentBase(List<PredefinedAgreementDocument> agreementDocuments)
        {
            AgreementDocuments = agreementDocuments;
        }

        internal override string GetPredefinedModelName()
        {
            return null;
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementDocument
    {
        [DataMember(Name = "localizedPolicyVersionId")] public string LocalizedPolicyVersionId;
        [DataMember(Name = "policyVersionId")] public string PolicyVersionId;
        [DataMember(Name = "policyId")] public string PolicyId;

        public PredefinedAgreementDocument(string localizedPolicyVersionId, string policyVersionId, string policyId)
        {
            LocalizedPolicyVersionId = localizedPolicyVersionId;
            PolicyVersionId = policyVersionId;
            PolicyId = policyId;
        }
    }
    #endregion

    #region Storage
    [DataContract, Preserve]
    public class PredefinedUserProfileCreatedPayload : PredefinedUserProfileModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserProfile_Created";
        }

        public PredefinedUserProfileCreatedPayload(UserProfile updatedFields)
            : base(updatedFields)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserProfileUpdatedPayload : PredefinedUserProfileModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserProfile_Updated";
        }

        public PredefinedUserProfileUpdatedPayload(UserProfile updatedFields)
            : base(updatedFields)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserProfileModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "updatedFields")] public UserProfile UpdatedFields;

        public PredefinedUserProfileModelBase(UserProfile updatedFields)
        {
            UpdatedFields = updatedFields;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemCreatedPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserStatItem_Created";
        }

        public PredefinedUserStatItemCreatedPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemUpdatedPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserStatItem_Updated";
        }

        public PredefinedUserStatItemUpdatedPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemResetPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserStatItem_Reset";
        }

        public PredefinedUserStatItemResetPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatGetItemsByCodesPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserStatItem_GetItemsByCodes";
        }

        public PredefinedUserStatGetItemsByCodesPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatGetSameItemsFromUsersPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserStatItem_GetSameItemsFromUsers";
        }

        public PredefinedUserStatGetSameItemsFromUsersPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatGetItemsPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserStatItem_GetItems";
        }

        public PredefinedUserStatGetItemsPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalStatGetItemByCodePayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "statCode")] public string StatCode;

        internal override string GetPredefinedModelName()
        {
            return "GlobalStatItem_GetItemByCode";
        }

        public PredefinedGlobalStatGetItemByCodePayload(string userId, string statCode)
        {
            UserId = userId;
            StatCode = statCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedStatCycleItemModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "cycleId")] public string CycleId;

        public PredefinedStatCycleItemModelBase(string userId, string cycleId)
        {
            UserId = userId;
            CycleId = cycleId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedStatCycleGetItemListPayload : PredefinedStatCycleItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "StatCycleItem_GetItemList";
        }

        public PredefinedStatCycleGetItemListPayload(string userId, string cycleId) : base (userId, cycleId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedStatCycleGetConfigByCycleIdPayload : PredefinedStatCycleItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "StatCycleItem_GetConfigByCycleId";
        }

        public PredefinedStatCycleGetConfigByCycleIdPayload(string userId, string cycleId)
            : base(userId, cycleId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedStatCycleGetListCyclePayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "StatCycleItem_GetListCycle";
        }

        public PredefinedStatCycleGetListCyclePayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedStatCycleGetCycleItemsPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "cycleId")] public string CycleId;
        [DataMember(Name = "statCodes")] public List<string> StatCodes;

        internal override string GetPredefinedModelName()
        {
            return "StatCycleItem_GetCycleItems";
        }

        public PredefinedStatCycleGetCycleItemsPayload(string userId, string cycleId, List<string> statCodes)
        {
            UserId = userId;
            CycleId = cycleId;
            StatCodes = statCodes;
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatGetItemListPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "StatCycleItem_GetItemList";
        }

        public PredefinedUserStatGetItemListPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemDeletedPayload : PredefinedUserStatItemModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "UserStatItem_Deleted";
        }

        public PredefinedUserStatItemDeletedPayload(string userId, List<string> statCode)
            : base(userId, statCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "statCodes")] public List<string> StatCodes;

        public PredefinedUserStatItemModelBase(string userId, List<string> statCodes)
        {
            UserId = userId;
            this.StatCodes = statCodes;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordUpdatedPayload : PredefinedPlayerRecordModelBase
    {
        [DataMember(Name = "isPublic")] public bool IsPublic;
        [DataMember(Name = "strategy")] public string Strategy;
        [DataMember(Name = "setBy")] public string SetBy;
        [DataMember(Name = "value")] public Dictionary<string, object> Value;

        internal override string GetPredefinedModelName()
        {
            return "PlayerRecord_Updated";
        }

        public PredefinedPlayerRecordUpdatedPayload(string key, string userId, bool isPublic, string strategy, string setBy, Dictionary<string, object> value)
            : base(key, userId)
        {
            IsPublic = isPublic;
            Strategy = strategy;
            SetBy = setBy;
            Value = value;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordDeletedPayload : PredefinedPlayerRecordModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "PlayerRecord_Deleted";
        }

        public PredefinedPlayerRecordDeletedPayload(string key, string userId)
            : base(key, userId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordGetRecordsPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "keys")] public string[] Keys;

        internal override string GetPredefinedModelName()
        {
            return "PlayerRecord_GetRecords";
        }

        public PredefinedPlayerRecordGetRecordsPayload(string userId, string[] keys)
        {
            UserId = userId;
            Keys = keys;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPublicPlayerRecordGetSameRecordsFromUsersPayload : PredefinedEventPayload
    {
        [DataMember(Name = "key")] public string Key;
        [DataMember(Name = "userIds")] public List<string> UserIds;

        internal override string GetPredefinedModelName()
        {
            return "PublicPlayerRecord_GetSameRecordsFromUsers";
        }

        public PredefinedPublicPlayerRecordGetSameRecordsFromUsersPayload(string key, List<string> userIds)
        {
            Key = key;
            UserIds = userIds;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPublicPlayerRecordGetRecordPayload : PredefinedPlayerRecordModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "PublicPlayerRecord_GetRecord";
        }

        public PredefinedPublicPlayerRecordGetRecordPayload(string userId, string key) : base (userId, key)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPublicPlayerRecordUpdatedPayload : PredefinedPlayerRecordModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "PublicPlayerRecord_Updated";
        }

        public PredefinedPublicPlayerRecordUpdatedPayload(string userId, string key) : base(userId, key)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPublicPlayerRecordGetOtherUserKeysPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "PublicPlayerRecord_GetOtherUserKeys";
        }

        public PredefinedPublicPlayerRecordGetOtherUserKeysPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPublicPlayerRecordGetOtherUserRecordsPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "keys")] public string[] Keys;

        internal override string GetPredefinedModelName()
        {
            return "PublicPlayerRecord_GetOtherUserRecords";
        }

        public PredefinedPublicPlayerRecordGetOtherUserRecordsPayload(string userId, string[] keys)
        {
            UserId = userId;
            Keys = keys;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordCreatedPayload : PredefinedPlayerRecordModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "GameRecord_Created";
        }

        public PredefinedGameRecordCreatedPayload(string userId, string key) : base(userId, key)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordGetRecordPayload : PredefinedPlayerRecordModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "GameRecord_GetRecord";
        }

        public PredefinedGameRecordGetRecordPayload(string userId, string key) : base(userId, key)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordGetRecordsPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "keys")] public string[] Keys;

        internal override string GetPredefinedModelName()
        {
            return "GameRecord_GetRecords";
        }

        public PredefinedGameRecordGetRecordsPayload(string userId, string[] keys)
        {
            UserId = userId;
            Keys = keys;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerBinaryRecordCreatedPayload : PredefinedPlayerRecordModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "PlayerBinaryRecord_Created";
        }

        public PredefinedPlayerBinaryRecordCreatedPayload(string userId, string key) : base(userId, key)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "key")] public string Key;

        public PredefinedPlayerRecordModelBase(string userId, string key)
        {
            UserId = userId;
            Key = key;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordUpdatedPayload : PredefinedGameRecordModelBase
    {
        [DataMember(Name = "setBy")] public string SetBy;
        [DataMember(Name = "strategy")] public string Strategy;
        [DataMember(Name = "value")] public Dictionary<string, object> Values;

        internal override string GetPredefinedModelName()
        {
            return "GameRecord_Updated";
        }

        public PredefinedGameRecordUpdatedPayload(string key, string setBy, string strategy, Dictionary<string, object> values)
            : base(key)
        {
            SetBy = setBy;
            Values = values;
            Strategy = strategy;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordDeletedPayload : PredefinedPlayerRecordModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "GameRecord_Deleted";
        }

        public PredefinedGameRecordDeletedPayload(string userId, string key)
            : base(userId, key)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "key")] public string Key;

        public PredefinedGameRecordModelBase(string key)
        {
            Key = key;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Engagement

    [DataContract, Preserve]
    public class PredefinedGroupModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "groupId")] public string GroupId;

        public PredefinedGroupModelBase(string groupId)
        {
            GroupId = groupId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupCreatedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "configurationCode")] public string ConfigurationCode;
        [DataMember(Name = "groupMaxMember")] public int GroupMaxMember;
        [DataMember(Name = "groupName")] public string GroupName;
        [DataMember(Name = "groupRegion")] public string GroupRegion;

        public PredefinedGroupCreatedPayload(GroupInformation groupInformation) : base(groupInformation.groupId)
        {
            ConfigurationCode = groupInformation.configurationCode;
            GroupMaxMember = groupInformation.groupMaxMember;
            GroupName = groupInformation.groupName;
            GroupRegion = groupInformation.groupRegion;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_Created";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupUpdatedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "configurationCode")] public string ConfigurationCode;
        [DataMember(Name = "groupMaxMember")] public int GroupMaxMember;
        [DataMember(Name = "groupName")] public string GroupName;
        [DataMember(Name = "groupRegion")] public string GroupRegion;

        public PredefinedGroupUpdatedPayload(GroupInformation groupInformation) : base(groupInformation.groupId)
        {
            ConfigurationCode = groupInformation.configurationCode;
            GroupMaxMember = groupInformation.groupMaxMember;
            GroupName = groupInformation.groupName;
            GroupRegion = groupInformation.groupRegion;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_Updated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupJoinedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "status")] public JoinGroupStatus Status;

        public PredefinedGroupJoinedPayload(JoinGroupResponse joinGroupResponse) : base(joinGroupResponse.groupId)
        {
            UserId = joinGroupResponse.userId;
            Status = joinGroupResponse.status;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_Joined";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupDeletedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupDeletedPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_Deleted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupLeftPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupLeftPayload(GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            UserId = groupResponse.userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_Left";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteAcceptedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupInviteAcceptedPayload(GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            UserId = groupResponse.userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_InvitationAccepted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteRejectedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupInviteRejectedPayload(GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            UserId = groupResponse.userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_InvitationRejected";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteCanceledPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "canceledUserId")] public string CanceledUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupInviteCanceledPayload(string adminUserId, GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            CanceledUserId = groupResponse.userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_InvitationCanceled";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupJoinRequestAcceptedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "acceptedUserId")] public string AcceptedUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupJoinRequestAcceptedPayload(string adminUserId, GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            AcceptedUserId = groupResponse.userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_JoinRequestAccepted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupJoinRequestRejectedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "rejectedUserId")] public string RejectedUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupJoinRequestRejectedPayload(string adminUserId, GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            RejectedUserId = groupResponse.userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_JoinRequestRejected";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupMemberKickedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "kickedUserId")] public string KickedUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupMemberKickedPayload(string adminUserId, KickMemberResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            KickedUserId = groupResponse.kickedUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_MemberKicked";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupMemberRoleUpdatedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "memberRoleId")] public string MemberRoleId;
        [DataMember(Name = "updatedUserId")] public string UpdatedUserId;

        public PredefinedGroupMemberRoleUpdatedPayload(string updatedUserId, string memberRoleId, string groupId) : base(groupId)
        {
            MemberRoleId = memberRoleId;
            UpdatedUserId = updatedUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_MemberRoleUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupMemberRoleDeletedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "memberRoleId")] public string MemberRoleId;
        [DataMember(Name = "updatedUserId")] public string UpdatedUserId;

        public PredefinedGroupMemberRoleDeletedPayload(string userId, string memberRoleId, string groupId = null) : base(groupId)
        {
            MemberRoleId = memberRoleId;
            UpdatedUserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_MemberRoleDeleted";
        }
    }


    [DataContract, Preserve]
    public class PredefinedGroupCustomAttributesUpdatedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;

        public PredefinedGroupCustomAttributesUpdatedPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            AdminUserId = adminUserId;
            CustomAttributes = groupInformation.customAttributes;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_CustomAttributesUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupCustomRuleUpdatedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;
        [DataMember(Name = "groupRules")] public GroupRules GroupRules;

        public PredefinedGroupCustomRuleUpdatedPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            AdminUserId = adminUserId;
            GroupRules = groupInformation.groupRules;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_CustomRuleUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupPredefinedRuleUpdatedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupPredefinedRuleUpdatedPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            AdminUserId = adminUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_PredefinedRuleUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupPredefinedRuleDeletedPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupPredefinedRuleDeletedPayload(string groupId, string adminUserId) : base(groupId)
        {
            AdminUserId = adminUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_PredefinedRuleDeleted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetInformationPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetInformationPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            UserId = adminUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_GetInformation";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupFindPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "groupName")] public string GroupName;
        [DataMember(Name = "groupRegion")] public string GroupRegion;

        public PredefinedGroupFindPayload(string userId, string groupName, string groupRegion)
        {
            UserId = userId;
            GroupName = groupName;
            GroupRegion = groupRegion;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_Find";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupFindByIdsPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "groupIds")] public string[] GroupIds;

        public PredefinedGroupFindByIdsPayload(string userId, string[] groupIds)
        {
            UserId = userId;
            GroupIds = groupIds;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_FindByIds";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteUserPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;
        [DataMember(Name = "invitedUserId")] public string InvitedUserId;

        public PredefinedGroupInviteUserPayload(UserInvitationResponse userInvitationResponse, string adminUserId) : base(userInvitationResponse.groupId)
        {
            AdminUserId = adminUserId;
            InvitedUserId = userInvitationResponse.userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_UserInvited";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetInvitationListPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetInvitationListPayload(string userId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_GetInvitationList";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupCancelJoinRequestPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupCancelJoinRequestPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_JoinRequestCanceled";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetJoinRequestPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetJoinRequestPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_GetJoinRequest";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetGroupMemberPayload : PredefinedGroupModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetGroupMemberPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Group_GetGroupMember";
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassClaimRewardPayload : PredefinedSeasonPassModelBase
    {
        [DataMember(Name = "passCode")] public string PassCode;
        [DataMember(Name = "tierIndex")] public int TierIndex;
        [DataMember(Name = "rewardCode")] public string RewardCode;

        internal override string GetPredefinedModelName()
        {
            return "SeasonPass_RewardClaimed";
        }

        public PredefinedSeasonPassClaimRewardPayload(string userId, string passCode, int tierIndex, string rewardCode) : base(userId)
        {
            PassCode = passCode;
            TierIndex = tierIndex;
            RewardCode = rewardCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassBulkClaimRewardPayload : PredefinedSeasonPassModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "SeasonPass_BulkRewardClaimed";
        }

        public PredefinedSeasonPassBulkClaimRewardPayload(string userId) : base(userId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassGetCurrentSeasonPayload : PredefinedSeasonPassModelBase
    {
        [DataMember(Name = "language")] public string Language;

        public PredefinedSeasonPassGetCurrentSeasonPayload(string userId, string language) : base(userId)
        {
            Language = language;
        }

        internal override string GetPredefinedModelName()
        {
            return "SeasonPass_GetCurrentSeason";
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedSeasonPassModelBase(string userId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassGetUserSeasonPayload : PredefinedSeasonPassModelBase
    {
        [DataMember(Name = "seasonId")] public string SeasonId;

        internal override string GetPredefinedModelName()
        {
            return "SeasonPass_GetUserSpecificSeasonData";
        }

        public PredefinedSeasonPassGetUserSeasonPayload(string userId, string seasonId) : base(userId)
        {
            SeasonId = seasonId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassGetCurrentUserSeasonPayload : PredefinedSeasonPassModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "SeasonPass_GetUserCurrentSeasonData";
        }

        public PredefinedSeasonPassGetCurrentUserSeasonPayload(string userId) : base(userId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "achievementCode")] public string AchievementCode;

        public PredefinedAchievementModelBase(string achievementCode)
        {
            AchievementCode = achievementCode;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementUnlockedPayload : PredefinedAchievementModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "Achievement_Unlocked";
        }

        public PredefinedAchievementUnlockedPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetAllPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "Achievement_GetAll";
        }

        public PredefinedAchievementGetAllPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetSpecificPayload : PredefinedAchievementModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "Achievement_GetSpecific";
        }

        public PredefinedAchievementGetSpecificPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetUserAchievementsPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "Achievement_GetUserAchievements";
        }

        public PredefinedAchievementGetUserAchievementsPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementGetPayload : PredefinedAchievementModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "GlobalAchievement_Get";
        }

        public PredefinedGlobalAchievementGetPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementGetContributorsPayload : PredefinedAchievementModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "GlobalAchievement_GetContributors";
        }

        public PredefinedGlobalAchievementGetContributorsPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementGetContributedPayload : PredefinedAchievementModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "GlobalAchievement_GetContributed";
        }

        public PredefinedGlobalAchievementGetContributedPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementClaimedPayload : PredefinedAchievementModelBase
    {
        internal override string GetPredefinedModelName()
        {
            return "GlobalAchievement_Claimed";
        }

        public PredefinedGlobalAchievementClaimedPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetTagsPayload : PredefinedEventPayload
    {
        [DataMember(Name = "name")] public string Name;

        internal override string GetPredefinedModelName()
        {
            return "Achievement_GetTags";
        }

        public PredefinedAchievementGetTagsPayload(string name)
        {
            Name = name;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetRankingsPayload : PredefinedLeaderboardBaseModel
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "Leaderboard_GetRankings";
        }

        public PredefinedLeaderboardGetRankingsPayload(string leaderboardCode, string userId) : base(leaderboardCode)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetUserRankingPayload : PredefinedLeaderboardBaseModel
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "Leaderboard_GetUserRanking";
        }

        public PredefinedLeaderboardGetUserRankingPayload(string leaderboardCode, string userId) : base(leaderboardCode)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetLeaderboardsPayload :  PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "Leaderboard_GetLeaderboards";
        }

        public PredefinedLeaderboardGetLeaderboardsPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetRankingByCycleIdPayload : PredefinedLeaderboardBaseModel
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "cycleId")] public string CycleId;

        internal override string GetPredefinedModelName()
        {
            return "Leaderboard_GetRankingByCycleId";
        }

        public PredefinedLeaderboardGetRankingByCycleIdPayload(string leaderboardCode, string userId, string cycleId) : base(leaderboardCode)
        {
            UserId = userId;
            CycleId = cycleId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetUserRankingsPayload : PredefinedLeaderboardBaseModel
    {
        [DataMember(Name = "userIds")] public string[] UserIds;
        [DataMember(Name = "requesterUserId")] public string RequesterUserId;

        internal override string GetPredefinedModelName()
        {
            return "Leaderboard_GetUsersRankings";
        }

        public PredefinedLeaderboardGetUserRankingsPayload(string leaderboardCode, string[] userIds, string requesterUserId) : base(leaderboardCode)
        {
            UserIds = userIds;
            RequesterUserId = requesterUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardBaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "leaderboardCode")] public string LeaderboardCode;

        public PredefinedLeaderboardBaseModel(string leaderboardCode)
        {
            LeaderboardCode = leaderboardCode;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedRewardGetRewardByCodePayload : PredefinedRewardBaseModel
    {
        [DataMember(Name = "rewardCode")] public string RewardCode;

        public PredefinedRewardGetRewardByCodePayload(string userId, string rewardCode) : base(userId)
        {
            RewardCode = rewardCode;
        }

        internal override string GetPredefinedModelName()
        {
            return "Reward_GetRewardByCode";
        }
    }

    [DataContract, Preserve]
    public class PredefinedRewardGetRewardByIdPayload : PredefinedRewardBaseModel
    {
        [DataMember(Name = "rewardId")] public string RewardId;

        public PredefinedRewardGetRewardByIdPayload(string userId, string rewardId) : base(userId)
        {
            RewardId = rewardId;
        }

        internal override string GetPredefinedModelName()
        {
            return "Reward_GetRewardById";
        }
    }

    [DataContract, Preserve]
    public class PredefinedRewardGetAllRewardPayload : PredefinedRewardBaseModel
    {
        [DataMember(Name = "eventTopic")] public string EventTopic;

        public PredefinedRewardGetAllRewardPayload(string userId, string eventTopic) : base(userId)
        {
            EventTopic = eventTopic;
        }

        internal override string GetPredefinedModelName()
        {
            return "Reward_GetAllReward";
        }
    }

    [DataContract, Preserve]
    public class PredefinedRewardBaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedRewardBaseModel(string userId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcChannelCreatedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "channelName")] public string ChannelName;

        public PredefinedUgcChannelCreatedPayload(string userId, string channelName) : base(userId)
        {
            ChannelName = channelName;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ChannelCreated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcChannelUpdatedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "channelId")] public string ChannelId;
        [DataMember(Name = "channelName")] public string ChannelName;

        public PredefinedUgcChannelUpdatedPayload(string userId, string channelId, string channelName) 
            : base(userId)
        {
            ChannelId = channelId;
            ChannelName = channelName;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ChannelUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcChannelDeletedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "channelId")] public string ChannelId;

        public PredefinedUgcChannelDeletedPayload(string userId, string channelId)
            : base(userId)
        {
            ChannelId = channelId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ChannelDeleted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcContentCreatedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "channelId")] public string ChannelId;

        public PredefinedUgcContentCreatedPayload(string userId, string channelId)
            : base(userId)
        {
            ChannelId = channelId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ContentCreated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcContentUpdatedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "channelId")] public string ChannelId;
        [DataMember(Name = "contentId")] public string ContentId;

        public PredefinedUgcContentUpdatedPayload(string userId, string channelId, string contentId)
            : base(userId)
        {
            ChannelId = channelId;
            ContentId = contentId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ContentUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcContentDeletedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "channelId")] public string ChannelId;
        [DataMember(Name = "contentId")] public string ContentId;

        public PredefinedUgcContentDeletedPayload(string userId, string channelId, string contentId)
            : base(userId)
        {
            ChannelId = channelId;
            ContentId = contentId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ContentDeleted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcContentLikedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "contentId")] public string ContentId;
        [DataMember(Name = "likeStatus")] public bool LikeStatus;

        public PredefinedUgcContentLikedPayload(string userId, string contentId, bool likeStatus)
            : base(userId)
        {
            ContentId = contentId;
            LikeStatus = likeStatus;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ContentLiked";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcCreatorFollowedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "followStatus")] public bool FollowStatus;

        public PredefinedUgcCreatorFollowedPayload(string userId, bool followStatus)
            : base(userId)
        {
            FollowStatus = followStatus;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_CreatorFollowed";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetListFollowersPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "targetUserId")] public string TargetUserId;

        public PredefinedUgcGetListFollowersPayload(string userId, string targetUserId)
            : base(userId)
        {
            TargetUserId = targetUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetListFollowers";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetUserContentsPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "targetUserId")] public string TargetUserId;

        public PredefinedUgcGetUserContentsPayload(string userId, string targetUserId)
            : base(userId)
        {
            TargetUserId = targetUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetUserContents";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcContentScreenshotUploadedPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "contentId")] public string ContentId;

        public PredefinedUgcContentScreenshotUploadedPayload(string userId, string contentId)
            : base(userId)
        {
            ContentId = contentId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_ContentScreenshotUploaded";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetFollowedContentPayload : PredefinedUgcBaseModel
    {
        public PredefinedUgcGetFollowedContentPayload(string userId)
            : base(userId)
        {
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetFollowedContent";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetFollowedUsersPayload : PredefinedUgcBaseModel
    {
        public PredefinedUgcGetFollowedUsersPayload(string userId)
            : base(userId)
        {
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetFollowedUsers";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetLikedContentsPayload : PredefinedUgcBaseModel
    {
        public PredefinedUgcGetLikedContentsPayload(string userId)
            : base(userId)
        {
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetLikedContents";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetCreatorPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "targetUserId")] public string TargetUserId;

        public PredefinedUgcGetCreatorPayload(string userId, string targetUserId)
            : base(userId)
        {
            TargetUserId = targetUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetCreator";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetGroupsPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "targetUserId")] public string TargetUserId;

        public PredefinedUgcGetGroupsPayload(string userId, string targetUserId)
            : base(userId)
        {
            TargetUserId = targetUserId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetGroups";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcSearchContentsChannelPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "channelId")] public string ChannelId;

        public PredefinedUgcSearchContentsChannelPayload(string userId, string channelId)
            : base(userId)
        {
            ChannelId = channelId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_SearchContentsChannel";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcSearchContentsPayload : PredefinedUgcBaseModel
    {
        public PredefinedUgcSearchContentsPayload(string userId)
            : base(userId)
        {
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_SearchContents";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetChannelsPayload : PredefinedUgcBaseModel
    {
        public PredefinedUgcGetChannelsPayload(string userId)
            : base(userId)
        {
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetChannels";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetTypesPayload : PredefinedUgcBaseModel
    {
        public PredefinedUgcGetTypesPayload(string userId)
            : base(userId)
        {
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetTypes";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetTagsPayload : PredefinedUgcBaseModel
    {
        public PredefinedUgcGetTagsPayload(string userId)
            : base(userId)
        {
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetTags";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetPreviewPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "contentId")] public string ContentId;

        public PredefinedUgcGetPreviewPayload(string userId, string contentId)
            : base(userId)
        {
            ContentId = contentId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetPreview";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetContentByShareCodePayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "shareCode")] public string ShareCode;

        public PredefinedUgcGetContentByShareCodePayload(string userId, string shareCode)
            : base(userId)
        {
            ShareCode = shareCode;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetContentByShareCode";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetContentByContentIdPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "contentId")] public string ContentId;

        public PredefinedUgcGetContentByContentIdPayload(string userId, string contentId)
            : base(userId)
        {
            ContentId = contentId;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetContentByContentId";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcGetContentBulkPayload : PredefinedUgcBaseModel
    {
        [DataMember(Name = "contentIds")] public List<string> ContentIds;

        public PredefinedUgcGetContentBulkPayload(string userId, List<string> contentIds)
            : base(userId)
        {
            ContentIds = contentIds;
        }

        internal override string GetPredefinedModelName()
        {
            return "UGC_GetContentBulk";
        }
    }

    [DataContract, Preserve]
    public class PredefinedUgcBaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedUgcBaseModel(string userId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region Monetization
    [DataContract, Preserve]
    public class PredefinedStoreOpenedPayload : PredefinedStoreModelBase
    {
        private readonly string eventName = "Store_Opened";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedStoreOpenedPayload(string storeId, string storeName, string category)
            : base(storeId, storeName, category)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedStoreClosedPayload : PredefinedStoreModelBase
    {
        private readonly string eventName = "Store_Closed";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedStoreClosedPayload(string storeId, string storeName, string category)
            : base(storeId, storeName, category)
        {
        }
    }

    [DataContract, Preserve]

    public class PredefinedStoreModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "storeId")] public string StoreId;
        [DataMember(Name = "storeName")] public string StoreName;
        [DataMember(Name = "category")] public string Category;

        public PredefinedStoreModelBase(string storeId, string storeName, string category)
        {
            StoreId = storeId;
            StoreName = storeName;
            Category = category;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemInspectOpenedPayload : PredefinedItemInspectModelBase
    {
        private readonly string eventName = "ItemInspect_Opened";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedItemInspectOpenedPayload(string itemId, string itemNamespace, string storeId, string language)
            : base(itemId, itemNamespace, storeId, language)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemInspectClosedPayload : PredefinedItemInspectModelBase
    {
        private readonly string eventName = "ItemInspect_Closed";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedItemInspectClosedPayload(string itemId, string itemNamespace, string storeId, string language)
            : base(itemId, itemNamespace, storeId, language)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemInspectModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "itemNamespace")] public string ItemNamespace;
        [DataMember(Name = "storeId")] public string StoreId;
        [DataMember(Name = "language")] public string Language;

        public PredefinedItemInspectModelBase(string itemId, string itemNamespace, string storeId, string language)
        {
            ItemId = itemId;
            ItemNamespace = itemNamespace;
            StoreId = storeId;
            Language = language;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedCurrencyUpdatedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "walletId")] public string WalletId;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        private readonly string eventName = "Currency_Updated";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedCurrencyUpdatedPayload(string walletId, string currencyCode)
        {
            WalletId = walletId;
            CurrencyCode = currencyCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedEntitlementGrantedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "entitlements")] public List<PredefinedEntitlements> Entitlements;
        private readonly string eventName = "Entitlement_Granted";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedEntitlementGrantedPayload(List<PredefinedEntitlements> entities)
        {
            Entitlements = entities;
        }
    }

    [DataContract, Preserve]
    public class PredefinedEntitlementRevokedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "affected")] public object Affected;
        private readonly string eventName = "Entitlement_Revoked";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedEntitlementRevokedPayload(object affectedObject)
        {
            Affected = affectedObject;
        }
    }

    [DataContract, Preserve]
    public class PredefinedEntitlements
    {
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "itemNamespace")] public string ItemNamespace;
        [DataMember(Name = "storeId")] public string StoreId;
        [DataMember(Name = "grantedCode")] public string GrantedCode;
        [DataMember(Name = "source")] public string Source;

        public PredefinedEntitlements(string itemId, string itemNamespace, string storeId, string grantedCode, string source)
        {
            ItemId = itemId;
            ItemNamespace = itemNamespace;
            StoreId = storeId;
            GrantedCode = grantedCode;
            Source = source;
        }
    }

    [DataContract, Preserve]
    public class PredefinedCampaignCodeRedeemedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userID")] public string UserId;
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "entitlementSummaries")] public List<PredefinedEntitlementSummary> EntitlementSummaries;
        [DataMember(Name = "creditSummaries")] public List<PredefinedCreditSummary> CreditSummaries;
        [DataMember(Name = "subscriptionSummaries")] public List<PredefinedSubscriptionSummary> SubscriptionSummaries;

        private readonly string eventName = "CampaignCode_Redeemed";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedCampaignCodeRedeemedPayload(
            string userId,
            string code,
            List<PredefinedEntitlementSummary> entitlementSummaries,
            List<PredefinedCreditSummary> creditSummaries,
            List<PredefinedSubscriptionSummary> subscriptionSummaries)
        {
            UserId = userId;
            Code = code;
            EntitlementSummaries = entitlementSummaries;
            CreditSummaries = creditSummaries;
            SubscriptionSummaries = subscriptionSummaries;
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemFulfilledPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userID")] public string UserId;
        [DataMember(Name = "entitlementSummaries")] public List<PredefinedEntitlementSummary> EntitlementSummaries;
        [DataMember(Name = "creditSummaries")] public List<PredefinedCreditSummary> CreditSummaries;
        [DataMember(Name = "subscriptionSummaries")] public List<PredefinedSubscriptionSummary> SubscriptionSummaries;

        private readonly string eventName = "Item_Fulfilled";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedItemFulfilledPayload(
            string userId,
            List<PredefinedEntitlementSummary> entitlementSummaries,
            List<PredefinedCreditSummary> creditSummaries,
            List<PredefinedSubscriptionSummary> subscriptionSummaries)
        {
            UserId = userId;
            EntitlementSummaries = entitlementSummaries;
            CreditSummaries = creditSummaries;
            SubscriptionSummaries = subscriptionSummaries;
        }
    }

    [DataContract, Preserve]
    public class PredefinedEntitlementSummary
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "type")] public string Type;
        [DataMember(Name = "clazz")] public string Clazz;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "storeId")] public string StoreId;

        public PredefinedEntitlementSummary(string id, string name, string type, string clazz, string itemId, string storeId)
        {
            Id = id;
            Name = name;
            Type = type;
            Clazz = clazz;
            ItemId = itemId;
            StoreId = storeId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedCreditSummary
    {
        [DataMember(Name = "walletId")] public string WalletId;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "amount")] public int Amount;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;

        public PredefinedCreditSummary(string walletId, string userId, int amount, string currencyCode)
        {
            WalletId = walletId;
            UserId = userId;
            Amount = amount;
            CurrencyCode = currencyCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedSubscriptionSummary
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "sku")] public string Sku;
        [DataMember(Name = "status")] public string Status;
        [DataMember(Name = "subscribedBy")] public string SubscribedBy;

        public PredefinedSubscriptionSummary(string id, string itemId, string userId, string sku, string status, string subscribedBy)
        {
            Id = id;
            UserId = userId;
            ItemId = itemId;
            Sku = sku;
            Status = status;
            SubscribedBy = subscribedBy;
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemRewardedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "success")] public bool Success;

        private readonly string eventName = "Item_Rewarded";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedItemRewardedPayload(bool success)
        {
            Success = success;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPaymentSucceededPayload : PredefinedEventPayload
    {
        [DataMember(Name = "data")] public List<PredefinedPaymentModel> Data;

        private readonly string eventName = "Payment_Succeeded";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedPaymentSucceededPayload(List<PredefinedPaymentModel> data)
        {
            Data = data;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPaymentFailedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "data")] public List<PredefinedPaymentModel> Data;

        private readonly string eventName = "Payment_Failed";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedPaymentFailedPayload(List<PredefinedPaymentModel> data)
        {
            Data = data;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPaymentModel
    {
        [DataMember(Name = "orderNo")] public string OrderNo;
        [DataMember(Name = "paymentOrderNo")] public string PaymentOrderNo;
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "itemId")] public string ItemId;
        [DataMember(Name = "price")] public int Price;
        [DataMember(Name = "status")] public string Status;

        public PredefinedPaymentModel(string orderNo, string paymentOrderNo, string userId, string itemId, int price, string status)
        {
            OrderNo = orderNo;
            PaymentOrderNo = paymentOrderNo;
            UserId = userId;
            ItemId = itemId;
            Price = price;
            Status = status;
        }
    }

    [DataContract, Preserve]
    public class PredefinedWalletCreditedPayload : PredefinedWalletModelBase
    {
        private readonly string eventName = "Wallet_Credited";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedWalletCreditedPayload(
            string userId,
            string currencyCode,
            string currencySymbol,
            int balance,
            string balanceOrigin,
            int totalPermanentBalance,
            int totalTimeLimitBalance)
            : base(userId, currencyCode, currencySymbol, balance, balanceOrigin, totalPermanentBalance, totalTimeLimitBalance)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedWalletDebitedPayload : PredefinedWalletModelBase
    {
        private readonly string eventName = "Wallet_Debited";

        internal override string GetPredefinedModelName()
        {
            return eventName;
        }

        public PredefinedWalletDebitedPayload(
            string userId,
            string currencyCode,
            string currencySymbol,
            int balance,
            string balanceOrigin,
            int totalPermanentBalance,
            int totalTimeLimitBalance)
            : base(userId, currencyCode, currencySymbol, balance, balanceOrigin, totalPermanentBalance, totalTimeLimitBalance)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedWalletModelBase : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        [DataMember(Name = "currecncySymbol")] public string CurrencySymbol;
        [DataMember(Name = "balance")] public int Balance;
        [DataMember(Name = "balanceOrigin")] public string BalanceOrigin;
        [DataMember(Name = "totlaPermanentBalance")] public int TotalPermanentBalance;
        [DataMember(Name = "totalTimeLimitedBalance")] public int TotalTimeLimitedBalance;

        public PredefinedWalletModelBase(
            string userId,
            string currencyCode,
            string currencySymbol,
            int balance,
            string balanceOrigin,
            int totalPermanentBalance,
            int totalTimeLimitedBalance)
        {
            UserId = userId;
            CurrencyCode = currencyCode;
            CurrencySymbol = currencySymbol;
            Balance = balance;
            BalanceOrigin = balanceOrigin;
            TotalPermanentBalance = totalPermanentBalance;
            TotalTimeLimitedBalance = totalTimeLimitedBalance;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region Play

    [DataContract, Preserve]
    public class PredefinedSessionV2BaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedSessionV2BaseModel(string userId)
        {
            UserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedSessionLeaderPromotedV2BaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "promotedUserId")] public string PromotedUserId;

        public PredefinedSessionLeaderPromotedV2BaseModel(string userId)
        {
            PromotedUserId = userId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2BaseModel : PredefinedSessionV2BaseModel
    {
        [DataMember(Name = "gameSessionId")] public string GameSessionId;

        public PredefinedGameSessionV2BaseModel(string userId, string sessionId) : base(userId)
        {
            GameSessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2BaseModel : PredefinedSessionV2BaseModel
    {
        [DataMember(Name = "partySessionId")] public string PartySessionId;

        public PredefinedPartySessionV2BaseModel(string userId, string sessionId) : base(userId)
        {
            PartySessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2CreatedPayload : PredefinedGameSessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_GameSession_Created";
        }

        public PredefinedGameSessionV2CreatedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2InvitedPayload : PredefinedGameSessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_GameSession_Invited";
        }

        public PredefinedGameSessionV2InvitedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2JoinedPayload : PredefinedGameSessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_GameSession_Joined";
        }

        public PredefinedGameSessionV2JoinedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2LeftPayload : PredefinedGameSessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_GameSession_Left";
        }

        public PredefinedGameSessionV2LeftPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2LeaderPromotedPayload : PredefinedSessionLeaderPromotedV2BaseModel
    {
        [DataMember(Name = "gameSessionId")] public string GameSessionId;

        internal override string GetPredefinedModelName()
        {
            return "MPV2_GameSession_LeaderPromoted";
        }

        public PredefinedGameSessionV2LeaderPromotedPayload(string userId, string sessionId) : base(userId)
        {
            GameSessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2CreatedPayload : PredefinedPartySessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_PartySession_Created";
        }

        public PredefinedPartySessionV2CreatedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2InvitedPayload : PredefinedPartySessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_PartySession_Invited";
        }

        public PredefinedPartySessionV2InvitedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2JoinedPayload : PredefinedPartySessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_PartySession_Joined";
        }

        public PredefinedPartySessionV2JoinedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2LeftPayload : PredefinedPartySessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_PartySession_Left";
        }

        public PredefinedPartySessionV2LeftPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2KickedPayload : PredefinedPartySessionV2BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV2_PartySession_Kicked";
        }

        public PredefinedPartySessionV2KickedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2LeaderPromotedPayload : PredefinedSessionLeaderPromotedV2BaseModel
    {
        [DataMember(Name = "partySessionId")] public string PartySessionId;

        internal override string GetPredefinedModelName()
        {
            return "MPV2_PartySession_LeaderPromoted";
        }

        public PredefinedPartySessionV2LeaderPromotedPayload(string userId, string sessionId) : base(userId)
        {
            PartySessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingRequestedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchPool")] public string MatchPool;
        [DataMember(Name = "partySessionId")] public string PartySessionId;
        [DataMember(Name = "attributes")] public Dictionary<string, object> Attributes;
        [DataMember(Name = "matchTicketId")] public string MatchTicketId;
        [DataMember(Name = "queueTime")] public int QueueTime;

        internal override string GetPredefinedModelName()
        {
            return "MPV2_Matchmaking_Requested";
        }

        public PredefinedMatchmakingRequestedPayload(string userId, string matchPool, string partySessionId, Dictionary<string, object> attributes, string matchTicketId, int queueTime)
        {
            UserId = userId;
            MatchPool = matchPool;
            PartySessionId = partySessionId;
            Attributes = attributes;
            MatchTicketId = matchTicketId;
            QueueTime = queueTime;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingStartedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchTicketId")] public string MatchTicketId;
        [DataMember(Name = "partySessionId")] public string PartySessionId;
        [DataMember(Name = "matchPool")] public string MatchPool;

        internal override string GetPredefinedModelName()
        {
            return "MPV2_Matchmaking_Started";
        }

        public PredefinedMatchmakingStartedPayload(string userId, string matchPool, string partySessionId, string matchTicketId)
        {
            UserId = userId;
            MatchPool = matchPool;
            PartySessionId = partySessionId;
            MatchTicketId = matchTicketId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingCanceledPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchTicketId")] public string MatchTicketId;

        internal override string GetPredefinedModelName()
        {
            return "MPV2_Matchmaking_Canceled";
        }

        public PredefinedMatchmakingCanceledPayload(string userId, string matchTicketId)
        {
            UserId = userId;
            MatchTicketId = matchTicketId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV1CreatedPayload : PredefinedGameSessionV1BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV1_GameSession_Created";
        }

        public PredefinedGameSessionV1CreatedPayload(string userId, string sessionId)
            : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV1JoinedPayload : PredefinedGameSessionV1BaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "MPV1_GameSession_Joined";
        }

        public PredefinedGameSessionV1JoinedPayload(string userId, string sessionId)
            : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV1BaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "gameSessionId")] public string GameSessionId;

        public PredefinedGameSessionV1BaseModel(string userId, string gameSessionId)
        {
            UserId = userId;
            GameSessionId = gameSessionId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1StartedPayload :  PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "gameMode")] public string GameMode;
        [DataMember(Name = "serverName")] public string ServerName;
        [DataMember(Name = "clientVersion")] public string ClientVersion;
        [DataMember(Name = "latencies")] public Dictionary<string, int> Latencies;
        [DataMember(Name = "partyAttribute")] public Dictionary<string, object> PartyAttribute;

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Matchmaking_Started";
        }

        public PredefinedMatchmakingV1StartedPayload(string userId, string gameMode, string serverName,
            string clientVersion, Dictionary<string, int> latencies, Dictionary<string, object> partyAttribute)
        {
            UserId = userId;
            GameMode = gameMode;
            ServerName = serverName;
            ClientVersion = clientVersion;
            Latencies = latencies;
            PartyAttribute = partyAttribute;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1NotifReceivedPayload : PredefinedMatchmakingV1BaseModel
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "matchPool")] public string MatchPool;
        [DataMember(Name = "teams")] public string Teams;
        [DataMember(Name = "tickets")] public string Tickets;

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Matchmaking_MatchNotif_Received";
        }

        public PredefinedMatchmakingV1NotifReceivedPayload(string userId, string matchId, string nameSpace,
            string matchPool, string teams, string tickets) : base(userId, matchId)
        {
            Namespace = nameSpace;
            MatchPool = matchPool;
            Teams = teams;
            Tickets = tickets;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1ReadyConsentPayload : PredefinedMatchmakingV1BaseModel
    {

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Matchmaking_ReadyConsent";
        }

        public PredefinedMatchmakingV1ReadyConsentPayload(string userId, string matchId) : base(userId, matchId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1RejectMatchPayload : PredefinedMatchmakingV1BaseModel
    {

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Matchmaking_RejectMatch";
        }

        public PredefinedMatchmakingV1RejectMatchPayload(string userId, string matchId) : base(userId, matchId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1CanceledPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "gameMode")] public string GameMode;
        [DataMember(Name = "isTempParty")] public bool IsTempParty;

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Matchmaking_Canceled";
        }

        public PredefinedMatchmakingV1CanceledPayload(string userId, string gameMode, bool isTempParty)
        {
            UserId = userId;
            GameMode = gameMode;
            IsTempParty = isTempParty;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1BaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchId")] public string MatchId;

        public PredefinedMatchmakingV1BaseModel(string userId, string matchId)
        {
            UserId = userId;
            MatchId = matchId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1CreatedPayload : PredefinedPartyV1BaseModel
    {

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Party_Created";
        }

        public PredefinedPartyV1CreatedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1InvitedPayload : PredefinedPartyV1BaseModel
    {

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Party_Invited";
        }

        public PredefinedPartyV1InvitedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1JoinedPayload : PredefinedPartyV1BaseModel
    {

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Party_Joined";
        }

        public PredefinedPartyV1JoinedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1LeftPayload : PredefinedPartyV1BaseModel
    {

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Party_Left";
        }

        public PredefinedPartyV1LeftPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1KickedPayload : PredefinedPartyV1BaseModel
    {

        internal override string GetPredefinedModelName()
        {
            return "MPV1_Party_Kicked";
        }

        public PredefinedPartyV1KickedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1BaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "partyId")] public string PartyId;

        public PredefinedPartyV1BaseModel(string userId, string partyId)
        {
            UserId = userId;
            PartyId = partyId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSConnectedPayload : PredefinedDSBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "DS_DSHub_Connected";
        }

        public PredefinedDSConnectedPayload(string podName) : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSDisconnectedPayload : PredefinedDSBaseModel
    {
        [DataMember(Name = "statusCode")] public WsCloseCode StatusCode;

        internal override string GetPredefinedModelName()
        {
            return "DS_DSHub_Disconnected";
        }

        public PredefinedDSDisconnectedPayload(string podName, WsCloseCode statusCode) : base(podName)
        {
            StatusCode = statusCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSRegisteredPayload : PredefinedDSBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "DS_Registered";
        }

        public PredefinedDSRegisteredPayload(string podName) : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSUnregisteredPayload : PredefinedDSBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "DS_Unregistered";
        }

        public PredefinedDSUnregisteredPayload(string podName) : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSMemberChangedPayload : PredefinedDSBaseModel
    {
        [DataMember(Name = "gameSessionId")] public string GameSessionId;
        [DataMember(Name = "members")] public SessionV2MemberData[] Members;
        [DataMember(Name = "teams")] public SessionV2TeamData[] Teams;

        internal override string GetPredefinedModelName()
        {
            return "DS_MemberChangedNotif_Received";
        }

        public PredefinedDSMemberChangedPayload(string podName, SessionV2GameSession gameSession) : base(podName)
        {
            GameSessionId = gameSession.id;
            Members = gameSession.members;
            Teams = gameSession.teams;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSClientJoinedPayload : PredefinedDSBaseModel
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "DS_GameClient_Joined";
        }

        public PredefinedDSClientJoinedPayload(string podName, string userId) : base(podName)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSClientLeftPayload : PredefinedDSBaseModel
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "DS_GameClient_Left";
        }

        public PredefinedDSClientLeftPayload(string podName, string userId) : base(podName)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSClaimedPayload : PredefinedDSBaseModel
    {
        [DataMember(Name = "gameSessionId")] public string GameSessionId;

        internal override string GetPredefinedModelName()
        {
            return "DS_Claimed";
        }

        public PredefinedDSClaimedPayload(string podName, string gameSessionId) : base(podName)
        {
            GameSessionId = gameSessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillReceivedPayload : PredefinedDSBackfillBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "DS_BackfillProposal_Received";
        }

        public PredefinedDSBackfillReceivedPayload(string podName, MatchmakingV2BackfillProposalNotification backfillNotif) : base(podName, backfillNotif)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillAcceptedPayload : PredefinedDSBackfillBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "DS_BackfillProposal_Accepted";
        }

        public PredefinedDSBackfillAcceptedPayload(string podName, MatchmakingV2BackfillProposalNotification backfillNotif) : base(podName, backfillNotif)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillRejectedPayload : PredefinedDSBackfillBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "DS_BackfillProposal_Rejected";
        }

        public PredefinedDSBackfillRejectedPayload(string podName, MatchmakingV2BackfillProposalNotification backfillNotif) : base(podName, backfillNotif)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "podName")] public string PodName;


        public PredefinedDSBaseModel(string podName)
        {
            PodName = podName;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillBaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "podName")] public string PodName;
        [DataMember(Name = "backfillTicketId")] public string BackfillTicketId;
        [DataMember(Name = "proposalId")] public string ProposalId;
        [DataMember(Name = "matchPool")] public string MatchPool;
        [DataMember(Name = "gameSessionId")] public string GameSessionId;
        [DataMember(Name = "proposedTeams")] public SessionV2TeamData[] ProposedTeams;
        [DataMember(Name = "addedTickets")] public MatchmakingV2Ticket[] AddedTickets;

        public PredefinedDSBackfillBaseModel(string podName, MatchmakingV2BackfillProposalNotification backfillNotif)
        {
            PodName = podName;
            BackfillTicketId = backfillNotif.backfillTicketId;
            ProposalId = backfillNotif.proposalId;
            MatchPool = backfillNotif.matchPool;
            GameSessionId = backfillNotif.matchSessionId;
            ProposedTeams = backfillNotif.proposedTeams;
            AddedTickets = backfillNotif.addedTickets;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserRequestDSPayload : PredefinedEventPayload
    {
        [DataMember(Name = "matchId")] public string MatchId;
        [DataMember(Name = "gameMode")] public string GameMode;

        internal override string GetPredefinedModelName()
        {
            return "User_RequestDS";
        }

        public PredefinedUserRequestDSPayload(string matchId, string gameMode)
        {
            MatchId = matchId;
            GameMode = gameMode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserBannedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "banType")] public string BanType;
        [DataMember(Name = "endDate")] public string EndDate;
        [DataMember(Name = "reason")] public string Reason;

        internal override string GetPredefinedModelName()
        {
            return "User_Banned";
        }

        public PredefinedUserBannedPayload(string banType, string endDate, string reason)
        {
            BanType = banType;
            EndDate = endDate;
            Reason = reason;
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserUnbannedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "banType")] public string BanType;

        internal override string GetPredefinedModelName()
        {
            return "User_Unbanned";
        }

        public PredefinedUserUnbannedPayload(string banType)
        {
            BanType = banType;
        }
    }
    #endregion

    #region Social
    [DataContract, Preserve]
    public class PredefinedPartyJoinedPayload : PredefinedPartyBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "Party_Joined";
        }

        public PredefinedPartyJoinedPayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyLeavePayload : PredefinedPartyBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "Party_Leave";
        }

        public PredefinedPartyLeavePayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyInvitePayload : PredefinedPartyBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "Party_Invite";
        }

        public PredefinedPartyInvitePayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyKickPayload : PredefinedPartyBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "Party_Kick";
        }

        public PredefinedPartyKickPayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyBaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "partyId")] public string PartyId;

        public PredefinedPartyBaseModel(string partyId)
        {
            PartyId = partyId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedLobbyConnectedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "Lobby_Connected";
        }

        public PredefinedLobbyConnectedPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLobbyDisconnectedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "statusCode")] public WsCloseCode StatusCode;

        internal override string GetPredefinedModelName()
        {
            return "Lobby_Disconnected";
        }

        public PredefinedLobbyDisconnectedPayload(string userId, WsCloseCode statusCode)
        {
            UserId = userId;
            StatusCode = statusCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestBase : PredefinedEventPayload
    {
        [DataMember(Name = "senderId")] public string SenderId;
        [DataMember(Name = "receiverId")] public string ReceiverId;

        public PredefinedFriendRequestBase(string senderId, string receiverId)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestSentPayload : PredefinedFriendRequestBase
    {
        internal override string GetPredefinedModelName()
        {
            return "FriendRequest_Sent";
        }

        public PredefinedFriendRequestSentPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestCancelledPayload : PredefinedFriendRequestBase
    {
        internal override string GetPredefinedModelName()
        {
            return "FriendRequest_Cancelled";
        }

        public PredefinedFriendRequestCancelledPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestAcceptedPayload : PredefinedFriendRequestBase
    {
        internal override string GetPredefinedModelName()
        {
            return "FriendRequest_Accepted";
        }

        public PredefinedFriendRequestAcceptedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestRejectedPayload : PredefinedFriendRequestBase
    {
        internal override string GetPredefinedModelName()
        {
            return "FriendRequest_Rejected";
        }

        public PredefinedFriendRequestRejectedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserBlockedPayload : PredefinedFriendRequestBase
    {
        internal override string GetPredefinedModelName()
        {
            return "User_Blocked";
        }

        public PredefinedUserBlockedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserUnblockedPayload : PredefinedFriendRequestBase
    {
        internal override string GetPredefinedModelName()
        {
            return "User_Unblocked";
        }

        public PredefinedUserUnblockedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendUnfriendedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "friendId")] public string FriendId;

        internal override string GetPredefinedModelName()
        {
            return "Friend_Unfriended";
        }

        public PredefinedFriendUnfriendedPayload(string userId, string friendId)
        {
            UserId = userId;
            FriendId = friendId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserPresenceStatusUpdatedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "status")] public string Status;

        internal override string GetPredefinedModelName()
        {
            return "UserPresence_StatusUpdated";
        }

        public PredefinedUserPresenceStatusUpdatedPayload(string userId, string status)
        {
            UserId = userId;
            Status = status;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ConnectedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_Connected";
        }

        public PredefinedChatV2ConnectedPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2DisconnectedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "statusCode")] public WsCloseCode StatusCode;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_Disconnected";
        }

        public PredefinedChatV2DisconnectedPayload(string userId, WsCloseCode statusCode)
        {
            UserId = userId;
            StatusCode = statusCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicBaseModel : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "topicId")] public string TopicId;

        public PredefinedChatV2TopicBaseModel(string userId, string topicId)
        {
            UserId = userId;
            TopicId = topicId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicJoinedPayload : PredefinedChatV2TopicBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "ChatV2_Topic_Joined";
        }

        public PredefinedChatV2TopicJoinedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicQuitPayload : PredefinedChatV2TopicBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "ChatV2_Topic_Quit";
        }

        public PredefinedChatV2TopicQuitPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicUserAddedPayload : PredefinedChatV2TopicBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "ChatV2_Topic_UserAdded";
        }

        public PredefinedChatV2TopicUserAddedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicUserRemovedPayload : PredefinedChatV2TopicBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "ChatV2_Topic_UserRemoved";
        }

        public PredefinedChatV2TopicUserRemovedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicDeletedPayload : PredefinedChatV2TopicBaseModel
    {
        internal override string GetPredefinedModelName()
        {
            return "ChatV2_Topic_Deleted";
        }

        public PredefinedChatV2TopicDeletedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2UserBlockedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "blockedUserId")] public string BlockedUserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_User_Blocked";
        }

        public PredefinedChatV2UserBlockedPayload(string userId, string blockedUserId)
        {
            UserId = userId;
            BlockedUserId = blockedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2UserUnblockedPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "blockedUserId")] public string BlockedUserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_User_Unblocked";
        }

        public PredefinedChatV2UserUnblockedPayload(string userId, string blockedUserId)
        {
            UserId = userId;
            BlockedUserId = blockedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2CreateTopicPayload : PredefinedEventPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "targetUserId")] public string TargetUserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_PersonalTopic_Created";
        }

        public PredefinedChatV2CreateTopicPayload(string userId, string targetUserId)
        {
            UserId = userId;
            TargetUserId = targetUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorBase : PredefinedEventPayload
    {
        [DataMember(Name = "groudId")] public string GroupId;
        [DataMember(Name = "moderatorId")] public string ModeratorId;

        public PredefinedChatV2ModeratorBase(string groupId, string moderatorId)
        {
            GroupId = groupId;
            ModeratorId = moderatorId;
        }

        internal override string GetPredefinedModelName()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorMutedPayload : PredefinedChatV2ModeratorBase
    {
        [DataMember(Name = "mutedUserId")] public string MutedUserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_GroupChat_ModeratorMutedUser";
        }

        public PredefinedChatV2ModeratorMutedPayload(string groupId, string moderatorId, string mutedUserId) : base(groupId, moderatorId)
        {
            MutedUserId = mutedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorUnmutedPayload : PredefinedChatV2ModeratorBase
    {
        [DataMember(Name = "unmutedUserId")] public string UnmutedUserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_GroupChat_ModeratorUnmutedUser";
        }

        public PredefinedChatV2ModeratorUnmutedPayload(string groupId, string moderatorId, string unmutedUserId) : base(groupId, moderatorId)
        {
            UnmutedUserId = unmutedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorBannedPayload : PredefinedChatV2ModeratorBase
    {
        [DataMember(Name = "bannedUserId")] public string BannedUserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_GroupChat_ModeratorBannedUser";
        }

        public PredefinedChatV2ModeratorBannedPayload(string groupId, string moderatorId, string bannedUserId) : base(groupId, moderatorId)
        {
            BannedUserId = bannedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorUnbannedPayload : PredefinedChatV2ModeratorBase
    {
        [DataMember(Name = "unbannedUserId")] public string UnbannedUserId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_GroupChat_ModeratorUnbannedUser";
        }

        public PredefinedChatV2ModeratorUnbannedPayload(string groupId, string moderatorId, string unbannedUserId) : base(groupId, moderatorId)
        {
            UnbannedUserId = unbannedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorDeletedPayload : PredefinedChatV2ModeratorBase
    {
        [DataMember(Name = "chatId")] public string ChatId;

        internal override string GetPredefinedModelName()
        {
            return "ChatV2_GroupChat_ModeratorDeletedGroupChat";
        }

        public PredefinedChatV2ModeratorDeletedPayload(string groupId, string moderatorId, string chatId) : base(groupId, moderatorId)
        {
            ChatId = chatId;
        }
    }

    #endregion
}