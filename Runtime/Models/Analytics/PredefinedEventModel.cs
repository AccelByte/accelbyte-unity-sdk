// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

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
            return "Agreement_Accepted";
        }

        public PredefinedAgreementAcceptedPayload(List<PredefinedAgreementDocument> agreementDocuments)
            : base(agreementDocuments)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedAgreementRefusedPayload : PredefinedAgreementDocumentBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Agreement_Refused";
        }

        public PredefinedAgreementRefusedPayload(List<PredefinedAgreementDocument> agreementDocuments)
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
    public class PredefinedUserProfileUpdatedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "updatedFields")] public List<string> UpdatedFields;

        public string GetEventName()
        {
            return "UserProfile_Updated";
        }

        public PredefinedUserProfileUpdatedPayload(List<string> updatedFields) 
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

        public PredefinedUserStatItemCreatedPayload(object statCodes)
            : base(statCodes)
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

        public PredefinedUserStatItemUpdatedPayload(object statCodes)
            :base(statCodes)
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

        public PredefinedUserStatItemDeletedPayload(object statCodes)
            :base(statCodes)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedUserStatItemModelBase
    {
        [DataMember(Name = "statCodes")] public object StatCodes;

        public PredefinedUserStatItemModelBase(object statCodes)
        {
            StatCodes = statCodes;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordUpdatedPayload : PredefinedGameRecordModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "isPublic")] public bool IsPublic;
        [DataMember(Name = "strategy")] public string Strategy;

        public string GetEventName()
        {
            return "PlayerRecord_Updated";
        }

        public PredefinedPlayerRecordUpdatedPayload(string key, bool isPublic, string strategy)
            : base(key)
        {
            IsPublic = isPublic;
            Strategy = strategy;
        }
    }

    [DataContract, Preserve]
    public class PredefinedPlayerRecordDeletedPayload : PredefinedGameRecordModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "isPublic")] public bool IsPublic;

        public string GetEventName()
        {
            return "PlayerRecord_Deleted";
        }

        public PredefinedPlayerRecordDeletedPayload(string key, bool isPublic)
            : base(key)
        {
            IsPublic = isPublic;
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameRecordUpdatedPayload : PredefinedGameRecordModelBase, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "strategy")] public string Strategy;

        public string GetEventName()
        {
            return "GameRecord_Updated";
        }

        public PredefinedGameRecordUpdatedPayload(string key, string strategy)
            : base(key)
        {
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

    #region Monetization
    [DataContract, Preserve]
    public class PredefinedStoreOpenedPayload : PredefinedStoreModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Store_Opened";
        }

        public PredefinedStoreOpenedPayload(string storeId, string storeName, string category)
            : base(storeId, storeName, category)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedStoreClosedPayload : PredefinedStoreModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "Store_Closed";
        }

        public PredefinedStoreClosedPayload(string storeId, string storeName, string category)
            :base(storeId, storeName, category)
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
        public string GetEventName()
        {
            return "ItemInspect_Opened";
        }

        public PredefinedItemInspectOpenedPayload(string itemId, string itemNamespace, string storeId, string language)
            : base(itemId, itemNamespace, storeId, language)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedItemInspectClosedPayload : PredefinedItemInspectModelBase, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "ItemInspect_Closed";
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

        public string GetEventName()
        {
            return "Currency_Updated";
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

        public string GetEventName()
        {
            return "Entitlement_Granted";
        }

        public PredefinedEntitlementGrantedPayload(List<PredefinedEntitlements> entities)
        {
            Entitlements = entities;
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
        [DataMember(Name = "code")] public string Code;

        public string GetEventName()
        {
            return "CampaignCode_Redeemed";
        }

        public PredefinedCampaignCodeRedeemedPayload(string code)
        {
            Code = code;
        }
    }
    #endregion

    #region Play
    [DataContract, Preserve]
    public class PredefinedGameSessionCreatedPayload : PredefinedGameSessionPlayerBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GameSession_Created";
        }

        public PredefinedGameSessionCreatedPayload(string sessionId)
            : base(sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionJoinedPayload : PredefinedGameSessionPlayerBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GameSession_Joined";
        }

        public PredefinedGameSessionJoinedPayload(string sessionId)
            : base(sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionPlayerAddedPayload : PredefinedGameSessionPlayerBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GameSession_PlayerAdded";
        }

        public PredefinedGameSessionPlayerAddedPayload(string sessionId)
            : base(sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionPlayerRemovedPayload : PredefinedGameSessionPlayerBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "GameSession_PlayerRemoved";
        }

        public PredefinedGameSessionPlayerRemovedPayload(string sessionId)
            : base(sessionId)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedGameSessionPlayerBaseModel
    {
        [DataMember(Name = "sessionId")] public string SessionId;

        public PredefinedGameSessionPlayerBaseModel(string sessionId)
        {
            SessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSRegisterServerPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_RegisterServer";
        }

        public PredefinedDSRegisterServerPayload(string podName)
            : base(podName)
        {
            PodName = podName;
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSDeRegisterServerPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_DeRegisterServer";
        }

        public PredefinedDSDeRegisterServerPayload(string podName)
            : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSShutdownServerPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        public string GetEventName()
        {
            return "DS_ShutdownServer";
        }

        public PredefinedDSShutdownServerPayload(string podName)
            : base(podName)
        {
        }
    }

    [DataContract, Preserve]
    public class PredefinedDSClaimedPayload : PredefinedDSBaseModel, IAccelByteTelemetryPayload
    {
        [DataMember(Name = "sessionId")] public string SessionId;

        public string GetEventName()
        {
            return "DS_Claimed";
        }

        public PredefinedDSClaimedPayload(string sessionId, string podName)
            : base(podName)
        {
            SessionId = sessionId;
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
    public class PredefinedMatchmakingV1RequestedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "gameMode")] public string GameMode;
        [DataMember(Name = "optionalParams")] public object OptionalParams;

        public string GetEventName()
        {
            return "Matchmaking_V1_Requested";
        }

        public PredefinedMatchmakingV1RequestedPayload(string gameMode, object optionalParams)
        {
            GameMode = gameMode;
            OptionalParams = optionalParams;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1StartedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "sessionId")] public string SessionId;

        public string GetEventName()
        {
            return "Matchmaking_V1_Started";
        }

        public PredefinedMatchmakingV1StartedPayload(string sessionId)
        {
            SessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1ReadyConsentPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "matchId")] public string MatchId;

        public string GetEventName()
        {
            return "Matchmaking_V1_ReadyConsent";
        }

        public PredefinedMatchmakingV1ReadyConsentPayload(string matchId)
        {
            MatchId = matchId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1RejectMatchPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "sessionId")] public string SessionId;

        public string GetEventName()
        {
            return "Matchmaking_V1_RejectMatch";
        }

        public PredefinedMatchmakingV1RejectMatchPayload(string sessionId)
        {
            SessionId = sessionId;
        }
    }

    [DataContract, Preserve]
    public class PredefinedMatchmakingV1CanceledPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "gameMode")] public string GameMode;

        public string GetEventName()
        {
            return "Matchmaking_V1_Canceled";
        }

        public PredefinedMatchmakingV1CanceledPayload(string gameMode)
        {
            GameMode = gameMode;
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
    public class PredefinedUserPresenceStatusUpdatedPayload : IAccelByteTelemetryPayload
    {
        [DataMember(Name = "status")] public string Status;

        public string GetEventName()
        {
            return "UserPresence_StatusUpdated";
        }

        public PredefinedUserPresenceStatusUpdatedPayload(string status)
        {
            Status = status;
        }
    }
    #endregion
}