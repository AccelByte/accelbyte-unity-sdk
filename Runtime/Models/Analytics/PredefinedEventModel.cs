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
    #region Core Game
    [DataContract, Preserve]
    public class PredefinedSDKInitializedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "version")] public string Version;

        public string GetEventName()
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
    public class PredefinedGameLaunchedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "gameTitle")] public string GameTitle;
        [DataMember(Name = "gameVersion")] public string GameVersion;

        public string GetEventName()
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
    public class PredefinedGameExitedPayload : PredefinedGameModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Game_Exited";
        }

        public PredefinedGameExitedPayload(string gameTitle, string gameVersion, string reason)
            : base(gameTitle, gameVersion, reason)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGamePausedPayload : PredefinedGameModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Game_Paused";
        }

        public PredefinedGamePausedPayload(string gameTitle, string gameVersion, string reason)
            : base(gameTitle, gameVersion, reason)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameModelBase
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
    }
    #endregion

    #region Access
    [DataContract, Preserve]
    public class PredefinedLoginSucceededPayload : PredefinedLoginModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "platformUserId")] public string PlatformUserId;
        [DataMember(Name = "deviceId")] public string DeviceId;

        public string GetEventName()
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
    public class PredefinedLoginFailedPayload : PredefinedLoginModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Login_Failed";
        }

        public PredefinedLoginFailedPayload(string @namespace, string platformId)
            : base(@namespace, platformId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedLoginModelBase
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "platformId")] public string PlatformId;

        public PredefinedLoginModelBase(string @namespace, string platformId)
        {
            Namespace = @namespace;
            PlatformId = platformId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementAcceptedPayload : PredefinedAgreementDocumentBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "UserAgreement_Accepted";
        }

        public PredefinedAgreementAcceptedPayload(List<PredefinedAgreementDocument> agreementDocuments)
            : base(agreementDocuments)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementNotAcceptedPayload : PredefinedAgreementDocumentBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "UserAgreement_NotAccepted";
        }

        public PredefinedAgreementNotAcceptedPayload(List<PredefinedAgreementDocument> agreementDocuments)
            : base(agreementDocuments)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementDocumentBase
    {
        [DataMember(Name = "agreementDocuments")] public List<PredefinedAgreementDocument> AgreementDocuments;

        public PredefinedAgreementDocumentBase(List<PredefinedAgreementDocument> agreementDocuments)
        {
            AgreementDocuments = agreementDocuments;
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
    public class PredefinedUserProfileCreatedPayload : PredefinedUserProfileModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "UserProfile_Created";
        }

        public PredefinedUserProfileCreatedPayload(UserProfile updatedFields)
            : base(updatedFields)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserProfileUpdatedPayload : PredefinedUserProfileModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "UserProfile_Updated";
        }

        public PredefinedUserProfileUpdatedPayload(UserProfile updatedFields)
            : base(updatedFields)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserProfileModelBase
    {
        [DataMember(Name = "updatedFields")] public UserProfile UpdatedFields;

        public PredefinedUserProfileModelBase(UserProfile updatedFields)
        {
            UpdatedFields = updatedFields;
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemCreatedPayload : PredefinedUserStatItemModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "UserStatItem_Created";
        }

        public PredefinedUserStatItemCreatedPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemUpdatedPayload : PredefinedUserStatItemModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "UserStatItem_Updated";
        }

        public PredefinedUserStatItemUpdatedPayload(string userId, List<string> statCodes)
            : base(userId, statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemDeletedPayload : PredefinedUserStatItemModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "UserStatItem_Deleted";
        }

        public PredefinedUserStatItemDeletedPayload(string userId, List<string> statCode)
            : base(userId, statCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemModelBase
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "statCodes")] public List<string> StatCodes;

        public PredefinedUserStatItemModelBase(string userId, List<string> statCodes)
        {
            UserId = userId;
            this.StatCodes = statCodes;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordUpdatedPayload : PredefinedPlayerRecordModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "isPublic")] public bool IsPublic;
        [DataMember(Name = "strategy")] public string Strategy;
        [DataMember(Name = "setBy")] public string SetBy;
        [DataMember(Name = "value")] public Dictionary<string, object> Value;

        public string GetEventName()
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
    public class PredefinedPlayerRecordDeletedPayload : PredefinedPlayerRecordModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "PlayerRecord_Deleted";
        }

        public PredefinedPlayerRecordDeletedPayload(string key, string userId)
            : base(key, userId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordModelBase
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "key")] public string Key;

        public PredefinedPlayerRecordModelBase(string userId, string key)
        {
            UserId = userId;
            Key = key;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordUpdatedPayload : PredefinedGameRecordModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "setBy")] public string SetBy;
        [DataMember(Name = "strategy")] public string Strategy;
        [DataMember(Name = "value")] public Dictionary<string, object> Values;

        public string GetEventName()
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
    public class PredefinedGameRecordDeletedPayload : PredefinedGameRecordModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GameRecord_Deleted";
        }

        public PredefinedGameRecordDeletedPayload(string key)
            : base(key)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordModelBase
    {
        [DataMember(Name = "key")] public string Key;

        public PredefinedGameRecordModelBase(string key)
        {
            Key = key;
        }
    }
    #endregion

    #region Engagement

    [DataContract, Preserve]
    public class PredefinedGroupModelBase
    {
        [DataMember(Name = "groupId")] public string GroupId;

        public PredefinedGroupModelBase(string groupId)
        {
            GroupId = groupId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupCreatedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
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

        public string GetEventName()
        {
            return "Group_Created";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupUpdatedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
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

        public string GetEventName()
        {
            return "Group_Updated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupJoinedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "status")] public JoinGroupStatus Status;

        public PredefinedGroupJoinedPayload(JoinGroupResponse joinGroupResponse) : base(joinGroupResponse.groupId)
        {
            UserId = joinGroupResponse.userId;
            Status = joinGroupResponse.status;
        }

        public string GetEventName()
        {
            return "Group_Joined";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupDeletedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupDeletedPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        public string GetEventName()
        {
            return "Group_Deleted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupLeftPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupLeftPayload(GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            UserId = groupResponse.userId;
        }

        public string GetEventName()
        {
            return "Group_Left";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteAcceptedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupInviteAcceptedPayload(GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            UserId = groupResponse.userId;
        }

        public string GetEventName()
        {
            return "Group_InvitationAccepted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteRejectedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupInviteRejectedPayload(GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            UserId = groupResponse.userId;
        }

        public string GetEventName()
        {
            return "Group_InvitationRejected";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteCanceledPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "canceledUserId")] public string CanceledUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupInviteCanceledPayload(string adminUserId, GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            CanceledUserId = groupResponse.userId;
        }

        public string GetEventName()
        {
            return "Group_InvitationCanceled";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupJoinRequestAcceptedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "acceptedUserId")] public string AcceptedUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupJoinRequestAcceptedPayload(string adminUserId, GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            AcceptedUserId = groupResponse.userId;
        }

        public string GetEventName()
        {
            return "Group_JoinRequestAccepted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupJoinRequestRejectedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "rejectedUserId")] public string RejectedUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupJoinRequestRejectedPayload(string adminUserId, GroupGeneralResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            RejectedUserId = groupResponse.userId;
        }

        public string GetEventName()
        {
            return "Group_JoinRequestRejected";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupMemberKickedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "kickedUserId")] public string KickedUserId;
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupMemberKickedPayload(string adminUserId, KickMemberResponse groupResponse) : base(groupResponse.groupId)
        {
            AdminUserId = adminUserId;
            KickedUserId = groupResponse.kickedUserId;
        }

        public string GetEventName()
        {
            return "Group_MemberKicked";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupMemberRoleUpdatedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "memberRoleId")] public string MemberRoleId;
        [DataMember(Name = "updatedUserId")] public string UpdatedUserId;

        public PredefinedGroupMemberRoleUpdatedPayload(string updatedUserId, string memberRoleId, string groupId) : base(groupId)
        {
            MemberRoleId = memberRoleId;
            UpdatedUserId = updatedUserId;
        }

        public string GetEventName()
        {
            return "Group_MemberRoleUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupMemberRoleDeletedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "memberRoleId")] public string MemberRoleId;
        [DataMember(Name = "updatedUserId")] public string UpdatedUserId;

        public PredefinedGroupMemberRoleDeletedPayload(string userId, string memberRoleId, string groupId = null) : base(groupId)
        {
            MemberRoleId = memberRoleId;
            UpdatedUserId = userId;
        }

        public string GetEventName()
        {
            return "Group_MemberRoleDeleted";
        }
    }


    [DataContract, Preserve]
    public class PredefinedGroupCustomAttributesUpdatedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;
        [DataMember(Name = "customAttributes")] public Dictionary<string, object> CustomAttributes;

        public PredefinedGroupCustomAttributesUpdatedPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            AdminUserId = adminUserId;
            CustomAttributes = groupInformation.customAttributes;
        }

        public string GetEventName()
        {
            return "Group_CustomAttributesUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupCustomRuleUpdatedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;
        [DataMember(Name = "groupRules")] public GroupRules GroupRules;

        public PredefinedGroupCustomRuleUpdatedPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            AdminUserId = adminUserId;
            GroupRules = groupInformation.groupRules;
        }

        public string GetEventName()
        {
            return "Group_CustomRuleUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupPredefinedRuleUpdatedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupPredefinedRuleUpdatedPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            AdminUserId = adminUserId;
        }

        public string GetEventName()
        {
            return "Group_PredefinedRuleUpdated";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupPredefinedRuleDeletedPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;

        public PredefinedGroupPredefinedRuleDeletedPayload(string groupId, string adminUserId) : base(groupId)
        {
            AdminUserId = adminUserId;
        }

        public string GetEventName()
        {
            return "Group_PredefinedRuleDeleted";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetInformationPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetInformationPayload(GroupInformation groupInformation, string adminUserId) : base(groupInformation.groupId)
        {
            UserId = adminUserId;
        }

        public string GetEventName()
        {
            return "Group_GetInformation";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupFindPayload : IAccelByteTelemetryPayload
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

        public string GetEventName()
        {
            return "Group_Find";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupFindByIdsPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "groupIds")] public string[] GroupIds;

        public PredefinedGroupFindByIdsPayload(string userId, string[] groupIds)
        {
            UserId = userId;
            GroupIds = groupIds;
        }

        public string GetEventName()
        {
            return "Group_FindByIds";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupInviteUserPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "adminUserId")] public string AdminUserId;
        [DataMember(Name = "invitedUserId")] public string InvitedUserId;

        public PredefinedGroupInviteUserPayload(UserInvitationResponse userInvitationResponse, string adminUserId) : base(userInvitationResponse.groupId)
        {
            AdminUserId = adminUserId;
            InvitedUserId = userInvitationResponse.userId;
        }

        public string GetEventName()
        {
            return "Group_UserInvited";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetInvitationListPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetInvitationListPayload(string userId)
        {
            UserId = userId;
        }

        public string GetEventName()
        {
            return "Group_GetInvitationList";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupCancelJoinRequestPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupCancelJoinRequestPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        public string GetEventName()
        {
            return "Group_JoinRequestCanceled";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetJoinRequestPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetJoinRequestPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        public string GetEventName()
        {
            return "Group_GetJoinRequest";
        }
    }

    [DataContract, Preserve]
    public class PredefinedGroupGetGroupMemberPayload : PredefinedGroupModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedGroupGetGroupMemberPayload(string groupId, string userId) : base(groupId)
        {
            UserId = userId;
        }

        public string GetEventName()
        {
            return "Group_GetGroupMember";
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassClaimRewardPayload : PredefinedSeasonPassModelBase , IAccelByteTelemetryPayload
    {
        [DataMember(Name = "passCode")] public string PassCode;
        [DataMember(Name = "tierIndex")] public int TierIndex;
        [DataMember(Name = "rewardCode")] public string RewardCode;

        public string GetEventName()
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
    public class PredefinedSeasonPassBulkClaimRewardPayload : PredefinedSeasonPassModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "SeasonPass_BulkRewardClaimed";
        }

        public PredefinedSeasonPassBulkClaimRewardPayload(string userId) : base(userId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassGetCurrentSeasonPayload : PredefinedSeasonPassModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "language")] public string Language;

        public PredefinedSeasonPassGetCurrentSeasonPayload(string userId, string language) : base(userId)
        {
            Language = language;
        }

        public string GetEventName()
        {
            return "SeasonPass_GetCurrentSeason";
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassModelBase
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedSeasonPassModelBase(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassGetUserSeasonPayload : PredefinedSeasonPassModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "seasonId")] public string SeasonId;

        public string GetEventName()
        {
            return "SeasonPass_GetUserSpecificSeasonData";
        }

        public PredefinedSeasonPassGetUserSeasonPayload(string userId, string seasonId) : base(userId)
        {
            SeasonId = seasonId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedSeasonPassGetCurrentUserSeasonPayload : PredefinedSeasonPassModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "SeasonPass_GetUserCurrentSeasonData";
        }

        public PredefinedSeasonPassGetCurrentUserSeasonPayload(string userId) : base(userId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementModelBase
    {
        [DataMember(Name = "achievementCode")] public string AchievementCode;

        public PredefinedAchievementModelBase(string achievementCode)
        {
            AchievementCode = achievementCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementUnlockedPayload : PredefinedAchievementModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Achievement_Unlocked";
        }

        public PredefinedAchievementUnlockedPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetAllPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "Achievement_GetAll";
        }

        public PredefinedAchievementGetAllPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetSpecificPayload : PredefinedAchievementModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Achievement_GetSpecific";
        }

        public PredefinedAchievementGetSpecificPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetUserAchievementsPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "Achievement_GetUserAchievements";
        }

        public PredefinedAchievementGetUserAchievementsPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementGetPayload : PredefinedAchievementModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GlobalAchievement_Get";
        }

        public PredefinedGlobalAchievementGetPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementGetContributorsPayload : PredefinedAchievementModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GlobalAchievement_GetContributors";
        }

        public PredefinedGlobalAchievementGetContributorsPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementGetContributedPayload : PredefinedAchievementModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GlobalAchievement_GetContributed";
        }

        public PredefinedGlobalAchievementGetContributedPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGlobalAchievementClaimedPayload : PredefinedAchievementModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GlobalAchievement_Claimed";
        }

        public PredefinedGlobalAchievementClaimedPayload(string achievementCode) : base(achievementCode)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAchievementGetTagsPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "name")] public string Name;

        public string GetEventName()
        {
            return "Achievement_GetTags";
        }

        public PredefinedAchievementGetTagsPayload(string name)
        {
            Name = name;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetRankingsPayload : PredefinedLeaderboardBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "Leaderboard_GetRankings";
        }

        public PredefinedLeaderboardGetRankingsPayload(string leaderboardCode, string userId) : base(leaderboardCode)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetUserRankingPayload : PredefinedLeaderboardBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "Leaderboard_GetUserRanking";
        }

        public PredefinedLeaderboardGetUserRankingPayload(string leaderboardCode, string userId) : base(leaderboardCode)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetLeaderboardsPayload :  IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "Leaderboard_GetLeaderboards";
        }

        public PredefinedLeaderboardGetLeaderboardsPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLeaderboardGetRankingByCycleIdPayload : PredefinedLeaderboardBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "cycleId")] public string CycleId;

        public string GetEventName()
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
    public class PredefinedLeaderboardGetUserRankingsPayload : PredefinedLeaderboardBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userIds")] public string[] UserIds;
        [DataMember(Name = "requesterUserId")] public string RequesterUserId;

        public string GetEventName()
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
    public class PredefinedLeaderboardBaseModel
    {
        [DataMember(Name = "leaderboardCode")] public string LeaderboardCode;

        public PredefinedLeaderboardBaseModel(string leaderboardCode)
        {
            LeaderboardCode = leaderboardCode;
        }
    }

    #endregion

    #region Monetization
    [DataContract, Preserve]
    public class PredefinedStoreOpenedPayload : PredefinedStoreModelBase, IAccelByteTelemetryPayload
    {
        private readonly string eventName = "Store_Opened";

        public string GetEventName()
        {
            return eventName;
        }

        public PredefinedStoreOpenedPayload(string storeId, string storeName, string category)
            : base(storeId, storeName, category)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedStoreClosedPayload : PredefinedStoreModelBase, IAccelByteTelemetryPayload
    {
        private readonly string eventName = "Store_Closed";

        public string GetEventName()
        {
            return eventName;
        }

        public PredefinedStoreClosedPayload(string storeId, string storeName, string category)
            : base(storeId, storeName, category)
        {
        }
    }

    [DataContract, Preserve]

    public class PredefinedStoreModelBase
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
    }

    [DataContract, Preserve]
    public class PredefinedItemInspectOpenedPayload : PredefinedItemInspectModelBase, IAccelByteTelemetryPayload
    {
        private readonly string eventName = "ItemInspect_Opened";

        public string GetEventName()
        {
            return eventName;
        }

        public PredefinedItemInspectOpenedPayload(string itemId, string itemNamespace, string storeId, string language)
            : base(itemId, itemNamespace, storeId, language)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemInspectClosedPayload : PredefinedItemInspectModelBase, IAccelByteTelemetryPayload
    {
        private readonly string eventName = "ItemInspect_Closed";

        public string GetEventName()
        {
            return eventName;
        }

        public PredefinedItemInspectClosedPayload(string itemId, string itemNamespace, string storeId, string language)
            : base(itemId, itemNamespace, storeId, language)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemInspectModelBase
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
    }

    [DataContract, Preserve]
    public class PredefinedCurrencyUpdatedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "walletId")] public string WalletId;
        [DataMember(Name = "currencyCode")] public string CurrencyCode;
        private readonly string eventName = "Currency_Updated";

        public string GetEventName()
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
    public class PredefinedEntitlementGrantedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "entitlements")] public List<PredefinedEntitlements> Entitlements;
        private readonly string eventName = "Entitlement_Granted";

        public string GetEventName()
        {
            return eventName;
        }

        public PredefinedEntitlementGrantedPayload(List<PredefinedEntitlements> entities)
        {
            Entitlements = entities;
        }
    }

    [DataContract, Preserve]
    public class PredefinedEntitlementRevokedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "affected")] public object Affected;
        private readonly string eventName = "Entitlement_Revoked";

        public string GetEventName()
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
    public class PredefinedCampaignCodeRedeemedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")] public string UserId;
        [DataMember(Name = "code")] public string Code;
        [DataMember(Name = "entitlementSummaries")] public List<PredefinedEntitlementSummary> EntitlementSummaries;
        [DataMember(Name = "creditSummaries")] public List<PredefinedCreditSummary> CreditSummaries;
        [DataMember(Name = "subscriptionSummaries")] public List<PredefinedSubscriptionSummary> SubscriptionSummaries;

        private readonly string eventName = "CampaignCode_Redeemed";

        public string GetEventName()
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
    public class PredefinedItemFulfilledPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userID")] public string UserId;
        [DataMember(Name = "entitlementSummaries")] public List<PredefinedEntitlementSummary> EntitlementSummaries;
        [DataMember(Name = "creditSummaries")] public List<PredefinedCreditSummary> CreditSummaries;
        [DataMember(Name = "subscriptionSummaries")] public List<PredefinedSubscriptionSummary> SubscriptionSummaries;

        private readonly string eventName = "Item_Fulfilled";

        public string GetEventName()
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
    public class PredefinedItemRewardedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "success")] public bool Success;

        private readonly string eventName = "Item_Rewarded";

        public string GetEventName()
        {
            return eventName;
        }

        public PredefinedItemRewardedPayload(bool success)
        {
            Success = success;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPaymentSucceededPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "data")] public List<PredefinedPaymentModel> Data;

        private readonly string eventName = "Payment_Succeeded";

        public string GetEventName()
        {
            return eventName;
        }

        public PredefinedPaymentSucceededPayload(List<PredefinedPaymentModel> data)
        {
            Data = data;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPaymentFailedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "data")] public List<PredefinedPaymentModel> Data;

        private readonly string eventName = "Payment_Failed";

        public string GetEventName()
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
    public class PredefinedWalletCreditedPayload : PredefinedWalletModelBase, IAccelByteTelemetryPayload
    {
        private readonly string eventName = "Wallet_Credited";

        public string GetEventName()
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
    public class PredefinedWalletDebitedPayload : PredefinedWalletModelBase, IAccelByteTelemetryPayload
    {
        private readonly string eventName = "Wallet_Debited";

        public string GetEventName()
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
    public class PredefinedWalletModelBase
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
    }
    #endregion

    #region Play

    [DataContract, Preserve]
    public class PredefinedSessionV2BaseModel
    {
        [DataMember(Name = "userId")] public string UserId;

        public PredefinedSessionV2BaseModel(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedSessionLeaderPromotedV2BaseModel
    {
        [DataMember(Name = "promotedUserId")] public string PromotedUserId;

        public PredefinedSessionLeaderPromotedV2BaseModel(string userId)
        {
            PromotedUserId = userId;
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
    public class PredefinedGameSessionV2CreatedPayload : PredefinedGameSessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_GameSession_Created";
        }

        public PredefinedGameSessionV2CreatedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2InvitedPayload : PredefinedGameSessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_GameSession_Invited";
        }

        public PredefinedGameSessionV2InvitedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2JoinedPayload : PredefinedGameSessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_GameSession_Joined";
        }

        public PredefinedGameSessionV2JoinedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2LeftPayload : PredefinedGameSessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_GameSession_Left";
        }

        public PredefinedGameSessionV2LeftPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV2LeaderPromotedPayload : PredefinedSessionLeaderPromotedV2BaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "gameSessionId")] public string GameSessionId;

        public string GetEventName()
        {
            return "MPV2_GameSession_LeaderPromoted";
        }

        public PredefinedGameSessionV2LeaderPromotedPayload(string userId, string sessionId) : base(userId)
        {
            GameSessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2CreatedPayload : PredefinedPartySessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_PartySession_Created";
        }

        public PredefinedPartySessionV2CreatedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2InvitedPayload : PredefinedPartySessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_PartySession_Invited";
        }

        public PredefinedPartySessionV2InvitedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2JoinedPayload : PredefinedPartySessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_PartySession_Joined";
        }

        public PredefinedPartySessionV2JoinedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2LeftPayload : PredefinedPartySessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_PartySession_Left";
        }

        public PredefinedPartySessionV2LeftPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2KickedPayload : PredefinedPartySessionV2BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV2_PartySession_Kicked";
        }

        public PredefinedPartySessionV2KickedPayload(string userId, string sessionId) : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartySessionV2LeaderPromotedPayload : PredefinedSessionLeaderPromotedV2BaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "partySessionId")] public string PartySessionId;

        public string GetEventName()
        {
            return "MPV2_PartySession_LeaderPromoted";
        }

        public PredefinedPartySessionV2LeaderPromotedPayload(string userId, string sessionId) : base(userId)
        {
            PartySessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingRequestedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchPool")] public string MatchPool;
        [DataMember(Name = "partySessionId")] public string PartySessionId;
        [DataMember(Name = "attributes")] public Dictionary<string, object> Attributes;
        [DataMember(Name = "matchTicketId")] public string MatchTicketId;
        [DataMember(Name = "queueTime")] public int QueueTime;

        public string GetEventName()
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
    public class PredefinedMatchmakingStartedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchTicketId")] public string MatchTicketId;
        [DataMember(Name = "partySessionId")] public string PartySessionId;
        [DataMember(Name = "matchPool")] public string MatchPool;

        public string GetEventName()
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
    public class PredefinedMatchmakingCanceledPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchTicketId")] public string MatchTicketId;

        public string GetEventName()
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
    public class PredefinedGameSessionV1CreatedPayload : PredefinedGameSessionV1BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV1_GameSession_Created";
        }

        public PredefinedGameSessionV1CreatedPayload(string userId, string sessionId)
            : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV1JoinedPayload : PredefinedGameSessionV1BaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "MPV1_GameSession_Joined";
        }

        public PredefinedGameSessionV1JoinedPayload(string userId, string sessionId)
            : base(userId, sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionV1BaseModel
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "gameSessionId")] public string GameSessionId;

        public PredefinedGameSessionV1BaseModel(string userId, string gameSessionId)
        {
            UserId = userId;
            GameSessionId = gameSessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1StartedPayload :  IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "gameMode")] public string GameMode;
        [DataMember(Name = "serverName")] public string ServerName;
        [DataMember(Name = "clientVersion")] public string ClientVersion;
        [DataMember(Name = "latencies")] public Dictionary<string, int> Latencies;
        [DataMember(Name = "partyAttribute")] public Dictionary<string, object> PartyAttribute;

        public string GetEventName()
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
    public class PredefinedMatchmakingV1NotifReceivedPayload : PredefinedMatchmakingV1BaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "matchPool")] public string MatchPool;
        [DataMember(Name = "teams")] public string Teams;
        [DataMember(Name = "tickets")] public string Tickets;

        public string GetEventName()
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
    public class PredefinedMatchmakingV1ReadyConsentPayload : PredefinedMatchmakingV1BaseModel, IAccelByteTelemetryPayload
    {

        public string GetEventName()
        {
            return "MPV1_Matchmaking_ReadyConsent";
        }

        public PredefinedMatchmakingV1ReadyConsentPayload(string userId, string matchId) : base(userId, matchId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1RejectMatchPayload : PredefinedMatchmakingV1BaseModel, IAccelByteTelemetryPayload
    {

        public string GetEventName()
        {
            return "MPV1_Matchmaking_RejectMatch";
        }

        public PredefinedMatchmakingV1RejectMatchPayload(string userId, string matchId) : base(userId, matchId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1CanceledPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "gameMode")] public string GameMode;
        [DataMember(Name = "isTempParty")] public bool IsTempParty;

        public string GetEventName()
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
    public class PredefinedMatchmakingV1BaseModel
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "matchId")] public string MatchId;

        public PredefinedMatchmakingV1BaseModel(string userId, string matchId)
        {
            UserId = userId;
            MatchId = matchId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1CreatedPayload : PredefinedPartyV1BaseModel, IAccelByteTelemetryPayload
    {

        public string GetEventName()
        {
            return "MPV1_Party_Created";
        }

        public PredefinedPartyV1CreatedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1InvitedPayload : PredefinedPartyV1BaseModel, IAccelByteTelemetryPayload
    {

        public string GetEventName()
        {
            return "MPV1_Party_Invited";
        }

        public PredefinedPartyV1InvitedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1JoinedPayload : PredefinedPartyV1BaseModel, IAccelByteTelemetryPayload
    {

        public string GetEventName()
        {
            return "MPV1_Party_Joined";
        }

        public PredefinedPartyV1JoinedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1LeftPayload : PredefinedPartyV1BaseModel, IAccelByteTelemetryPayload
    {

        public string GetEventName()
        {
            return "MPV1_Party_Left";
        }

        public PredefinedPartyV1LeftPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1KickedPayload : PredefinedPartyV1BaseModel, IAccelByteTelemetryPayload
    {

        public string GetEventName()
        {
            return "MPV1_Party_Kicked";
        }

        public PredefinedPartyV1KickedPayload(string userId, string partyId) : base(userId, partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyV1BaseModel
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "partyId")] public string PartyId;

        public PredefinedPartyV1BaseModel(string userId, string partyId)
        {
            UserId = userId;
            PartyId = partyId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSConnectedPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_DSHub_Connected";
        }

        public PredefinedDSConnectedPayload(string podName) : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSDisconnectedPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "statusCode")] public WsCloseCode StatusCode;

        public string GetEventName()
        {
            return "DS_DSHub_Disconnected";
        }

        public PredefinedDSDisconnectedPayload(string podName, WsCloseCode statusCode) : base(podName)
        {
            StatusCode = statusCode;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSRegisteredPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_Registered";
        }

        public PredefinedDSRegisteredPayload(string podName) : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSUnregisteredPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_Unregistered";
        }

        public PredefinedDSUnregisteredPayload(string podName) : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSMemberChangedPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "gameSessionId")] public string GameSessionId;
        [DataMember(Name = "members")] public SessionV2MemberData[] Members;
        [DataMember(Name = "teams")] public SessionV2TeamData[] Teams;

        public string GetEventName()
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
    public class PredefinedDSClientJoinedPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "DS_GameClient_Joined";
        }

        public PredefinedDSClientJoinedPayload(string podName, string userId) : base(podName)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSClientLeftPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "DS_GameClient_Left";
        }

        public PredefinedDSClientLeftPayload(string podName, string userId) : base(podName)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSClaimedPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "gameSessionId")] public string GameSessionId;

        public string GetEventName()
        {
            return "DS_Claimed";
        }

        public PredefinedDSClaimedPayload(string podName, string gameSessionId) : base(podName)
        {
            GameSessionId = gameSessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillReceivedPayload : PredefinedDSBackfillBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_BackfillProposal_Received";
        }

        public PredefinedDSBackfillReceivedPayload(string podName, MatchmakingV2BackfillProposalNotification backfillNotif) : base(podName, backfillNotif)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillAcceptedPayload : PredefinedDSBackfillBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_BackfillProposal_Accepted";
        }

        public PredefinedDSBackfillAcceptedPayload(string podName, MatchmakingV2BackfillProposalNotification backfillNotif) : base(podName, backfillNotif)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillRejectedPayload : PredefinedDSBackfillBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_BackfillProposal_Rejected";
        }

        public PredefinedDSBackfillRejectedPayload(string podName, MatchmakingV2BackfillProposalNotification backfillNotif) : base(podName, backfillNotif)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBaseModel
    {
        [DataMember(Name = "podName")] public string PodName;


        public PredefinedDSBaseModel(string podName)
        {
            PodName = podName;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSBackfillBaseModel
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
    }

    [DataContract, Preserve]
    public class PredefinedUserRequestDSPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "matchId")] public string MatchId;
        [DataMember(Name = "gameMode")] public string GameMode;

        public string GetEventName()
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
    public class PredefinedUserBannedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "banType")] public string BanType;
        [DataMember(Name = "endDate")] public string EndDate;
        [DataMember(Name = "reason")] public string Reason;

        public string GetEventName()
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
    public class PredefinedUserUnbannedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "banType")] public string BanType;

        public string GetEventName()
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
    public class PredefinedPartyJoinedPayload : PredefinedPartyBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Party_Joined";
        }

        public PredefinedPartyJoinedPayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyLeavePayload : PredefinedPartyBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Party_Leave";
        }

        public PredefinedPartyLeavePayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyInvitePayload : PredefinedPartyBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Party_Invite";
        }

        public PredefinedPartyInvitePayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyKickPayload : PredefinedPartyBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Party_Kick";
        }

        public PredefinedPartyKickPayload(string partyId)
            : base(partyId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedPartyBaseModel
    {
        [DataMember(Name = "partyId")] public string PartyId;

        public PredefinedPartyBaseModel(string partyId)
        {
            PartyId = partyId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLobbyConnectedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "Lobby_Connected";
        }

        public PredefinedLobbyConnectedPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedLobbyDisconnectedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "statusCode")] public WsCloseCode StatusCode;

        public string GetEventName()
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
    public class PredefinedFriendRequestBase
    {
        [DataMember(Name = "senderId")] public string SenderId;
        [DataMember(Name = "receiverId")] public string ReceiverId;

        public PredefinedFriendRequestBase(string senderId, string receiverId)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestSentPayload : PredefinedFriendRequestBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "FriendRequest_Sent";
        }

        public PredefinedFriendRequestSentPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestCancelledPayload : PredefinedFriendRequestBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "FriendRequest_Cancelled";
        }

        public PredefinedFriendRequestCancelledPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestAcceptedPayload : PredefinedFriendRequestBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "FriendRequest_Accepted";
        }

        public PredefinedFriendRequestAcceptedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendRequestRejectedPayload : PredefinedFriendRequestBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "FriendRequest_Rejected";
        }

        public PredefinedFriendRequestRejectedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserBlockedPayload : PredefinedFriendRequestBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "User_Blocked";
        }

        public PredefinedUserBlockedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserUnblockedPayload : PredefinedFriendRequestBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "User_Unblocked";
        }

        public PredefinedUserUnblockedPayload(string senderId, string receiverId) : base(senderId, receiverId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedFriendUnfriendedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "friendId")] public string FriendId;

        public string GetEventName()
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
    public class PredefinedUserPresenceStatusUpdatedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "status")] public string Status;

        public string GetEventName()
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
    public class PredefinedChatV2ConnectedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;

        public string GetEventName()
        {
            return "ChatV2_Connected";
        }

        public PredefinedChatV2ConnectedPayload(string userId)
        {
            UserId = userId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2DisconnectedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "statusCode")] public WsCloseCode StatusCode;

        public string GetEventName()
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
    public class PredefinedChatV2TopicBaseModel
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "topicId")] public string TopicId;

        public PredefinedChatV2TopicBaseModel(string userId, string topicId)
        {
            UserId = userId;
            TopicId = topicId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicJoinedPayload : PredefinedChatV2TopicBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "ChatV2_Topic_Joined";
        }

        public PredefinedChatV2TopicJoinedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicQuitPayload : PredefinedChatV2TopicBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "ChatV2_Topic_Quit";
        }

        public PredefinedChatV2TopicQuitPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicUserAddedPayload : PredefinedChatV2TopicBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "ChatV2_Topic_UserAdded";
        }

        public PredefinedChatV2TopicUserAddedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicUserRemovedPayload : PredefinedChatV2TopicBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "ChatV2_Topic_UserRemoved";
        }

        public PredefinedChatV2TopicUserRemovedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2TopicDeletedPayload : PredefinedChatV2TopicBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "ChatV2_Topic_Deleted";
        }

        public PredefinedChatV2TopicDeletedPayload(string userId, string topicId) : base(userId, topicId)
        {
        }

    }

    [DataContract, Preserve]
    public class PredefinedChatV2UserBlockedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "blockedUserId")] public string BlockedUserId;

        public string GetEventName()
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
    public class PredefinedChatV2UserUnblockedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "blockedUserId")] public string BlockedUserId;

        public string GetEventName()
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
    public class PredefinedChatV2CreateTopicPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "userId")] public string UserId;
        [DataMember(Name = "targetUserId")] public string TargetUserId;

        public string GetEventName()
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
    public class PredefinedChatV2ModeratorBase
    {
        [DataMember(Name = "groudId")] public string GroupId;
        [DataMember(Name = "moderatorId")] public string ModeratorId;

        public PredefinedChatV2ModeratorBase(string groupId, string moderatorId)
        {
            GroupId = groupId;
            ModeratorId = moderatorId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorMutedPayload : PredefinedChatV2ModeratorBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "mutedUserId")] public string MutedUserId;

        public string GetEventName()
        {
            return "ChatV2_GroupChat_ModeratorMutedUser";
        }

        public PredefinedChatV2ModeratorMutedPayload(string groupId, string moderatorId, string mutedUserId) : base(groupId, moderatorId)
        {
            MutedUserId = mutedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorUnmutedPayload : PredefinedChatV2ModeratorBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "unmutedUserId")] public string UnmutedUserId;

        public string GetEventName()
        {
            return "ChatV2_GroupChat_ModeratorUnmutedUser";
        }

        public PredefinedChatV2ModeratorUnmutedPayload(string groupId, string moderatorId, string unmutedUserId) : base(groupId, moderatorId)
        {
            UnmutedUserId = unmutedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorBannedPayload : PredefinedChatV2ModeratorBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "bannedUserId")] public string BannedUserId;

        public string GetEventName()
        {
            return "ChatV2_GroupChat_ModeratorBannedUser";
        }

        public PredefinedChatV2ModeratorBannedPayload(string groupId, string moderatorId, string bannedUserId) : base(groupId, moderatorId)
        {
            BannedUserId = bannedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorUnbannedPayload : PredefinedChatV2ModeratorBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "unbannedUserId")] public string UnbannedUserId;

        public string GetEventName()
        {
            return "ChatV2_GroupChat_ModeratorUnbannedUser";
        }

        public PredefinedChatV2ModeratorUnbannedPayload(string groupId, string moderatorId, string unbannedUserId) : base(groupId, moderatorId)
        {
            UnbannedUserId = unbannedUserId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedChatV2ModeratorDeletedPayload : PredefinedChatV2ModeratorBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "chatId")] public string ChatId;

        public string GetEventName()
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