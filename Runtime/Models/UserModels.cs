// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
	#region enum
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum AuthenticationType { EMAILPASSWD, PHONEPASSWD }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum SearchType { ALL, DISPLAYNAME, USERNAME }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum TwoFAFactorType
    {
        [Description("authenticator")]
        AUTHENTICATOR,
        [Description("backupCode")]
        BACKUPCODE
    }

    public static class TwoFAFactorTypeExtensions
    {
        public static string GetString(this TwoFAFactorType me)
        {
            switch (me)
            {
                case TwoFAFactorType.AUTHENTICATOR:
                    return "authenticator";
                case TwoFAFactorType.BACKUPCODE:
                    return "backupCode"; 
                default:
                    return "NO VALUE GIVEN";
            }
        }
    }

    #endregion enum

    [DataContract]
    public class TokenData
    {
        [DataMember] public string access_token { get; set; }
        [DataMember] public string refresh_token { get; set; }
        [DataMember] public int expires_in { get; set; }
        [DataMember] public string token_type { get; set; }
        [DataMember] public string user_id { get; set; }
        [DataMember] public string display_name { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public bool is_comply { get; set; }
    }

    [DataContract]
    public class SessionData
    {
        [DataMember] public string session_id { get; set; }
        [DataMember] public int expires_in { get; set; }
        [DataMember] public string refresh_id { get; set; }
    }

    [DataContract]
    public class Ban
    {
        [DataMember] public string ban { get; set; }
        [DataMember] public string banId { get; set; }
        [DataMember] public DateTime endDate { get; set; }
    }

    [DataContract]
    public class Permission
    {
        [DataMember] public int action { get; set; }
        [DataMember] public string resource { get; set; }
        [DataMember] public int schedAction { get; set; }
        [DataMember] public string schedCron { get; set; }
        [DataMember] public string[] schedRange { get; set; }
    }

    [DataContract]
    public class UserData
    {
        [DataMember] public string authType { get; set; }
        [DataMember] public Ban[] bans { get; set; }
        [DataMember] public string country { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public bool deletionStatus { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public bool emailVerified { get; set; }
        [DataMember] public bool enabled { get; set; }
        [DataMember] public DateTime lastDateOfBirthChangedTime { get; set; }
        [DataMember] public DateTime lastEnabledChangedTime { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string newEmailAddress { get; set; }
        [DataMember] public string oldEmailAddress { get; set; }
        [DataMember] public Permission[] permissions { get; set; }
        [DataMember] public string phoneNumber { get; set; }
        [DataMember] public bool phoneVerified { get; set; }
        [DataMember] public string platformId { get; set; }
        [DataMember] public string platformUserId { get; set; }
        [DataMember] public string[] roles { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string userName { get; set; }
        [DataMember] public bool eligible { get; set; } = true;
    }

    [DataContract]
    public class PublicUserData
    {
        [DataMember] public string authType { get; set; }
        [DataMember] public Ban[] bans { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public bool deletionStatus { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public bool emailVerified { get; set; }
        [DataMember] public bool enabled { get; set; }
        [DataMember] public DateTime lastDateOfBirthChangedTime { get; set; }
        [DataMember] public DateTime lastEnabledChangedTime { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public Permission[] permissions { get; set; }
        [DataMember] public bool phoneVerified { get; set; }
        [DataMember] public string platformId { get; set; }
        [DataMember] public string platformUserId { get; set; }
        [DataMember] public string[] roles { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string userName { get; set; }
        [DataMember] public bool eligible { get; set; } = true;
    }

    [DataContract]
    public class PublicUserInfo
    {
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string userName { get; set; }
    }

    [DataContract]
    public class PagedPublicUsersInfo
    {
        [DataMember] public PublicUserInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class PolicyAcceptance
    {
        [DataMember] public bool isAccepted { get; set; }
        [DataMember] public string localizedPolicyVersionId { get; set; }
        [DataMember] public string policyId { get; set; }
        [DataMember] public string policyVersionId { get; set; }
    }

    [DataContract]
    public class RegisterUserRequest
    {
        [DataMember] public AuthenticationType authType { get; set; }
        [DataMember] public string country { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public string password { get; set; }
        [DataMember] public List<PolicyAcceptance> acceptedPolicies { get; set; }
    }
    
    [DataContract]
    public class RegisterUserRequestv2
    {
        [DataMember] public AuthenticationType authType { get; set; } = AuthenticationType.EMAILPASSWD;
        [DataMember] public string country { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public string password { get; set; }
        [DataMember] public string username { get; set; }
        [DataMember] public List<PolicyAcceptance> acceptedPolicies { get; set; }
    }

    [DataContract]
    public class RegisterUserResponse
    {
        [DataMember] public AuthenticationType authType { get; set; }
        [DataMember] public string country { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string username { get; set; }
    }

    [DataContract]
    public class UpdateUserRequest
    {
        [DataMember] public string country { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public string languageTag { get; set; }
        [DataMember] public string username { get; set; }
    }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum PlatformType { Steam, EpicGames, PS4, Live, Stadia, Google, Apple, Facebook, Twitch, Oculus, Twitter, Device, Android, iOS, Nintendo, awscognito, PS5 }

    [DataContract]
    public class PlatformLink
    {
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public string linkedAt { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string originNamespace { get; set; }
        [DataMember] public string platformId { get; set; }
        [DataMember] public string platformUserId { get; set; }
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class PagedPlatformLinks
    {
        [DataMember] public PlatformLink[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
    }

    [DataContract]
    public class BulkPlatformUserIdRequest
    {
        [DataMember] public  string[] platformUserIDs { get; set; }
    }

    [DataContract]
    public class PlatformUserIdMap
    {
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class BulkPlatformUserIdResponse
    {
        [DataMember] public PlatformUserIdMap[] userIdPlatforms { get; set; }
    }

    public class CountryInfo
    {
        [DataMember] public string countryCode { get; set; }
        [DataMember] public string countryName { get; set; }
        [DataMember] public string state { get; set; }
        [DataMember] public string city { get; set; }
    }

    public class Country
    {
        [DataMember]  public string code { get; set; }
        [DataMember] public string name { get; set; }
    
    }

    [DataContract]
    public class UpgradeUserRequest
    {
        [DataMember] public string temporary_session_id { get; set; }
    }

    [DataContract]
    public class UpgradeAndVerifyHeadlessRequest
    {
        [DataMember] public string code { get; set; }
        [DataMember] public string country { get; set; } //optional
        [DataMember] public string dateOfBirth { get; set; } //optional
        [DataMember] public string displayName { get; set; } //optional
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public string password { get; set; }
        [DataMember] public bool reachMinimumAge { get; set; } = true; //optional. If user input DOB, BE will not check this field
        [DataMember] public string username { get; set; }
        [DataMember] public bool validateOnly { get; set; } = false;
    }

    [DataContract]
    public class AccountLinkedPlatform
    {
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string platformUserId { get; set; }
    }

    [DataContract]
    public class UnlinkPlatformAccountRequest
    {
        [DataMember] public string platformNamespace { get; set; }
    }

    [DataContract]
    public class AccountLinkPublisherAccount
    {
        [DataMember] public string userId { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public AccountLinkedPlatform[] linkedPlatforms { get; set; }

    }

    [DataContract]
    public class AccountLinkConfictMessageVariables
    {
        [DataMember] public string platformUserID { get; set; }
        [DataMember] public AccountLinkPublisherAccount[] publisherAccounts { get; set; }
    }

    [DataContract]
    public class LinkPlatformAccountRequest
    {
        [DataMember] public string platformId { get; set; }
        [DataMember] public string platformUserId { get; set; }
    }

    [DataContract]
    public class UpdateEmailRequest
    {
        [DataMember] public string code;
        [DataMember] public string emailAddress;
    }

    #region Ban
    /// <summary>
    /// Type of Ban that available
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum BanType { LOGIN, CHAT_SEND, CHAT_ALL, ORDER_AND_PAYMENT, STATISTICS, LEADERBOARD, MATCHMAKING, UGC_CREATE_UPDATE }

    /// <summary>
    /// Type of Ban reason that available
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum BanReason { VIOLENCE, HARASSMENT, HATEFUL_CONDUCT, OFFENSIVE_USERNAME, IMPERSONATION, 
        MALICIOUS_CONTENT, SEXUALLY_SUGGESTIVE, SEXUAL_VIOLENCE, EXTREME_VIOLENCE, UNDERAGE_USER, CHEATING, TOS_VIOLATION }

    /// <summary>
    /// Information about user that performing a ban
    /// </summary>
    /// <param name="displayName"> display name of the User</param>
    /// <param name="userId"> user ID of the User</param>
    [DataContract]
    public class BannedByV3
    {
        [DataMember] public string displayName;
        [DataMember] public string userId;
    }

    /// <summary>
    /// Template for making a Ban request
    /// </summary>
    /// <param name="ban">The type of Ban</param>
    /// <param name="comment">The detail or comment about the banning</param>
    /// <param name="endDate">The date when the ban is lifted with format "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffzzz"</param>
    /// <param name="reason">The reason of Banning</param>
    /// <param name="skipNotif">Notify user via email or not</param>
    [DataContract]
    public class BanCreateRequest
    {
        [DataMember] public string ban;
        [DataMember] public string comment;
        [DataMember] public string endDate;
        [DataMember] public string reason;
        [DataMember] public bool skipNotif;
    }

    /// <summary>
    /// Template for Ban response
    /// </summary>
    /// <param name="ban">The type of Ban</param>
    /// <param name="banId">The Ban ID</param>
    /// <param name="bannedBy">Information about user that performing a ban</param>
    /// <param name="comment">The detail or comment about the banning</param>
    /// <param name="createdAt">The date when the ban is created</param>
    /// <param name="disabledAt">The date when user got disabled</param>
    /// <param name="enabled">is the ban still going for the user</param>
    /// <param name="endDate">The date when the ban is lifted</param>
    /// <param name="Namespace">Namespace that user got banned</param>
    /// <param name="reason">The reason of Banning</param>
    /// <param name="userId">The user ID that got banned</param>
    [DataContract]
    public class UserBanResponseV3
    {
        [DataMember] public BanType ban;
        [DataMember] public string banId;
        [DataMember] public BannedByV3 bannedBy;
        [DataMember] public string comment;
        [DataMember] public DateTime createdAt;
        [DataMember] public DateTime disabledDate;
        [DataMember] public bool enabled;
        [DataMember] public DateTime endDate;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public BanReason reason;
        [DataMember] public string userId;
    }

    [DataContract]
    public class UserEnableBan
    {
        [DataMember] public bool enabled;
    }

    [DataContract]
    public class UserBanPagedList
    {
        [DataMember] public UserBanResponseV3[] data;
        [DataMember] public Paging paging;
    }
    #endregion

    [DataContract]
    public class ListBulkUserInfoRequest
    {
        [DataMember] public string[] userIds;
    }

    [DataContract]
    public class BaseUserInfo
    {
        [DataMember] public string avatarUrl { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public Dictionary<string, string> platformUserIds { get; set; } 
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class ListBulkUserInfoResponse
    {
        [DataMember] public BaseUserInfo[] data { get; set; }
    }

    [DataContract]
    public class NamespaceRole
    {
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string roleId { get; set; }
    }

    [DataContract]
    public class UserOtherPlatformInfo
    {
        [DataMember] public string authType { get; set; }
        [DataMember] public Ban[] bans { get; set; }
        [DataMember] public string country { get; set; }
        [DataMember] public string createdAt { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public bool deletionStatus { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public bool emailVerified { get; set; }
        [DataMember] public bool enabled { get; set; }
        [DataMember] public string lastDateOfBirthChangedTime { get; set; }
        [DataMember] public string lastEnabledChangedTime { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public NamespaceRole[] namespaceRoles { get; set; }
        [DataMember] public string newEmailAddress { get; set; }
        [DataMember] public string oldEmailAddress { get; set; }
        [DataMember] public Permission[] permissions { get; set; }
        [DataMember] public string phoneNumber { get; set; }
        [DataMember] public bool phoneVerified { get; set; }
        [DataMember] public string platformDisplayName { get; set; } //optional
        [DataMember] public string platformId { get; set; } //optional
        [DataMember] public string platformUserId { get; set; } //optional
        [DataMember] public string[] roles { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string username { get; set; } //optional
    }

    [DataContract]
    public class PagedUserOtherPlatformInfo
    {
        [DataMember] public UserOtherPlatformInfo[] data { get; set; }
        [DataMember] public Paging paging { get; set; }
        [DataMember] public int totalData { get; set; }
    }

    [DataContract]
    public class TwoFACode
    {
        [DataMember] public int generatedAt { get; set; }
        [DataMember] public string[] invalidCode { get; set; }
        [DataMember] public string[] validCodes { get; set; }
    }

    [DataContract]
    public class SecretKey3rdPartyApp
    {
        [DataMember] public string secretKey { get; set; }
        [DataMember] public string uri { get; set; }
    }

    [DataContract]
    public class Enable2FAFactors
    { 
        [DataMember(Name = "default")] public string default_ { get; set; }
        [DataMember] public string[] enabled { get; set; }
    }
    [DataContract]
    public class ValidationDescription
    {
        [DataMember] public string language { get; set; }
        [DataMember] public string[] message { get; set; }
    }

    [DataContract]
    public class Validation
    {
        [DataMember] public bool allowDigit { get; set; }
        [DataMember] public bool allowLetter { get; set; }
        [DataMember] public bool allowSpace { get; set; }
        [DataMember] public bool allowUnicode { get; set; }
        [DataMember] public ValidationDescription description { get; set; }
        [DataMember] public bool isCustomRegex { get; set; }
        [DataMember] public string letterCase { get; set; }
        [DataMember] public int maxLength { get; set; }
        [DataMember] public int maxRepeatingAlphaNum { get; set; }
        [DataMember] public int maxRepeatingSpecialCharacter { get; set; }
        [DataMember] public int minCharType { get; set; }
        [DataMember] public int minLength { get; set; }
        [DataMember] public string regex { get; set; }
        [DataMember] public string specialCharacterLocation { get; set; }
        [DataMember] public string[] specialCharacters { get; set; }
    }

    [DataContract]
    public class DataInputValidation
    {
        [DataMember] public string field { get; set; }
        [DataMember] public Validation validation { get; set; }

    }

    [DataContract]
    public class InputValidation
    {
        [DataMember] public DataInputValidation[] data { get; set; } 
        [DataMember] public int version { get; set; }
    }
};
