// Copyright (c) 2018 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    #region enum
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum AuthenticationType { EMAILPASSWD, PHONEPASSWD }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum SearchType { ALL, DISPLAYNAME, USERNAME, THIRDPARTYPLATFORM, UNIQUEDISPLAYNAME }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum TwoFAFactorType
    {
        [Description("authenticator")]
        AUTHENTICATOR,
        [Description("backupCode")]
        BACKUPCODE
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum SearchPlatformType { None, PlatformDisplayName }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum AccountAvailabilityField
    {
        [EnumMember(Value = "displayName")] DisplayName,
        [EnumMember(Value = "uniqueDisplayName")] UniqueDisplayName,
        [EnumMember(Value = "userName")] UserName
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
    
    [DataContract, Preserve]
    public class TokenBans
    {
        [DataMember] public string Ban;
        [DataMember] public DateTime DisabledDate;
        [DataMember] public bool Enabled;
        [DataMember] public DateTime EndDate;
        [DataMember] public string TargetedNamespace;
    }

    [DataContract, Preserve]
    public class TokenData
    {
        [DataMember] public string access_token;
        [DataMember] public string auth_trust_id;
        [DataMember(Name = "bans")] public TokenBans[] Bans;
        [DataMember] public string display_name;
        [DataMember(Name = "device_id")] public string DeviceId;
        [DataMember] public int expires_in;
        [DataMember] public bool is_comply;
        [DataMember(Name = "jflgs")] public float Jflgs;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "namespace_roles")] public NamespaceRoles[] NamespaceRoles;
        [DataMember(Name = "permissions")] public Permission[] Permissions;
        [DataMember] public string platform_id;
        [DataMember] public string platform_user_id;
        [DataMember] public int refresh_expires_in;
        [DataMember] public string refresh_token;
        [DataMember(Name = "roles")] public string[] Roles;
        [DataMember(Name = "scope")] public string Scope;
        [DataMember(Name = "simultaneous_platform_id")] public string SimultaneousPlatformId;
        [DataMember(Name = "simultaneous_platform_user_id")] public string SimultaneousPlatformUserId;
        [DataMember] public string token_type;
        [DataMember(Name = "unique_display_name")] public string UniqueDisplayName;
        [DataMember] public string user_id;
        [DataMember(Name = "xuid")] public string Xuid;
    }

    [DataContract, Preserve]
    public class SessionData
    {
        [DataMember] public string session_id;
        [DataMember] public int expires_in;
        [DataMember] public string refresh_id;
    }

    [DataContract, Preserve, Serializable]
    public class RefreshTokenData
    {
        [DataMember] public string refresh_token;
        [DataMember] public int expiration_date;
    }

    [DataContract, Preserve]
    public class Ban
    {
        [DataMember] public string ban;
        [DataMember] public string banId;
        [DataMember] public DateTime endDate;
    }

    [DataContract, Preserve]
    public class Permission
    {
        [DataMember] public int action;
        [DataMember] public string resource;
        [DataMember] public int schedAction;
        [DataMember] public string schedCron;
        [DataMember] public string[] schedRange;
    }

    [DataContract, Preserve]
    public class NamespaceRoles
    {
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string roleId;
    }

    [DataContract, Preserve]
    public class UserData
    {
        [DataMember] public string authType;
        [DataMember] public string avatarUrl;
        [DataMember] public Ban[] bans;
        [DataMember] public string country;
        [DataMember] public DateTime createdAt;
        [DataMember] public string dateOfBirth;
        [DataMember(Name = "deletionDate")] public string DeletionDate;
        [DataMember] public bool deletionStatus;
        [DataMember] public string displayName;
        [DataMember] public string emailAddress;
        [DataMember] public bool emailVerified;
        [DataMember] public bool enabled;
        [DataMember] public DateTime lastDateOfBirthChangedTime;
        [DataMember] public DateTime lastEnabledChangedTime;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public NamespaceRoles[] namespaceRoles;
        [DataMember] public string newEmailAddress;
        [DataMember] public string oldEmailAddress;
        [DataMember] public Permission[] permissions;
        [DataMember] public string phoneNumber;
        [DataMember] public bool phoneVerified;
        [DataMember] public string platformAvatarUrl;
        [DataMember] public string platformDisplayName;
        [DataMember] public string platformId;
        [DataMember(Name = "platformInfos")] public AccountUserPlatformInfo[] PlatformInfos;
        [DataMember] public string platformUserId;
        [DataMember] public string[] roles;
        [DataMember(Name = "testAccount")] public bool TestAccount;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string userId;
        [DataMember] public string userName;
        [DataMember] public bool eligible = true;
    }

    [DataContract, Preserve]
    public class PublicUserInfo
    {
        [DataMember] public DateTime createdAt;
        [DataMember] public string displayName;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string userId;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string userName;
    }

    [DataContract, Preserve]
    public class PagedPublicUsersInfo
    {
        [DataMember] public PublicUserInfo[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class PolicyAcceptance
    {
        [DataMember] public bool isAccepted;
        [DataMember] public string localizedPolicyVersionId;
        [DataMember] public string policyId;
        [DataMember] public string policyVersionId;
    }

    /// <summary>
    /// Optional parameters for Register and RegisterV4 endpoints. Can be null.
    /// </summary>
    public class RegisterUserOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Verification code sent to email. Use only when Mandatory Email Verification is enabled on the namespace.
        /// </summary>
        public string Code;
    }

    [DataContract, Preserve]
    public class RegisterUserRequest
    {
        [DataMember] public AuthenticationType authType;
        [DataMember] public string country;
        [DataMember] public string dateOfBirth;
        [DataMember] public string displayName;
        [DataMember] public string emailAddress;
        [DataMember] public string password;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public List<PolicyAcceptance> acceptedPolicies;
        [DataMember(Name = "code"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Code;
    }
    
    [DataContract, Preserve]
    public class RegisterUserRequestv2
    {
        [DataMember] public AuthenticationType authType = AuthenticationType.EMAILPASSWD;
        [DataMember] public string country;
        [DataMember] public string dateOfBirth;
        [DataMember] public string displayName;
        [DataMember] public string emailAddress;
        [DataMember] public string password;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string username;
        [DataMember] public List<PolicyAcceptance> acceptedPolicies;
        [DataMember(Name = "code"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Code;
    }

    [DataContract, Preserve]
    public class RegisterUserResponse
    {
        [DataMember] public AuthenticationType authType;
        [DataMember] public string country;
        [DataMember] public string dateOfBirth;
        [DataMember] public string displayName;
        [DataMember] public string emailAddress;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string userId;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string username;
    }

    [DataContract, Preserve]
    public class UpdateUserRequest
    {
        [DataMember] public string country; // Country use ISO3166-1 alpha-2 two letter, e.g. US.
        [DataMember] public string dateOfBirth; // Date of Birth format : YYYY-MM-DD, e.g. 2019-04-29.
        [DataMember] public string displayName;
        [DataMember] public string emailAddress; // Able to be used in publisher namespace 
        [DataMember] public string languageTag;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string username;
        [DataMember] public string avatarUrl;
    }
    
    [Preserve]
    public class LoginPlatformType
    {
        internal readonly string PlatformId;

        public LoginPlatformType(PlatformType typeEnum)
        {
            PlatformId = typeEnum.ToString().ToLower();
        }
        
        public LoginPlatformType(string platformId)
        {
            PlatformId = platformId;
        }
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum PlatformGroup
    {
        None,
        SteamNetwork,
        Psn,
        Xbox,
        OculusGroup,
        Google
    }

    [Preserve]
    public class GetUserPlatformType
    {
        internal readonly string PlatformId;

        internal bool IsValid()
        {
            return !string.IsNullOrEmpty(PlatformId);
        }

        public GetUserPlatformType(PlatformType typeEnum)
        {
            PlatformId = typeEnum.ToString().ToLower();
        }

        public GetUserPlatformType(PlatformGroup typeEnum)
        {
            PlatformId = typeEnum.ToString().ToLower();
        }
        
        public GetUserPlatformType(string platformId)
        {
            bool ignoreCase = true;
            if (Enum.TryParse(platformId, ignoreCase, out PlatformType _))
            {
                PlatformId = platformId.ToLower();
                return;
            }

            if (Enum.TryParse(platformId, ignoreCase, out PlatformGroup _))
            {
                PlatformId = platformId.ToLower();
                return;
            }
        }
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum PlatformType
    {
        None,
        Steam,
        EpicGames,
        PS4,
        Live,
        Google,
        Apple,
        Facebook,
        Twitch,
        Oculus,
        Twitter,
        Device, 
        Android,
        iOS,
        Nintendo,
        awscognito,
        PS5,
        Netflix,
        EAOrigin,
        Discord,
        Snapchat,
        PS4Web,
        GooglePlayGames,
    }

    [DataContract, Preserve]
    public class PlatformLink
    {
        [DataMember(Name = "accountGroup")] public string AccountGroup;
        [DataMember] public string displayName;
        [DataMember] public string emailAddress;
        [DataMember] public string linkedAt;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string originNamespace;
        [DataMember] public string platformId;
        [DataMember] public string platformUserId;
        [DataMember] public string userId;
        [DataMember(Name = "xuid")] public string Xuid;
    }

    [DataContract, Preserve]
    public class PagedPlatformLinks
    {
        [DataMember] public PlatformLink[] data;
        [DataMember] public Paging paging;
    }

    [DataContract, Preserve]
    public class BulkPlatformUserIdRequest
    {
        [DataMember] public string[] platformUserIDs;
        [DataMember(Name = "pidType"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string PidType = null;
    }

    public class BulkPlatformUserIdParameter
    {
        public string PlatformId;
        public bool? RawPuid;
    }

    [DataContract, Preserve]
    public class PlatformUserIdMap
    {
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class BulkGetUserByOtherPlatformUserIdsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// When enabled, return un-encrypted platform user id.
        /// </summary>
        public bool? RawPuid = null;
        /// <summary>
        /// The type of the platform user ids.
        /// </summary>
        public PidType PidType = null;
    }

    public class PidType
    {
        private string pid;
        
        public PidType(BulkGetUserPlatformUserIdType typeEnum)
        {
            pid = Utils.ConverterUtils.EnumToDescription(typeEnum);
        }

        public string ConvertToString()
        {
            return pid;
        }
    }
    
    public enum BulkGetUserPlatformUserIdType
    {
        [Description("OCULUS_APP_USER_ID")] OculusAppUserId
    }

    [DataContract, Preserve]
    public class BulkPlatformUserIdResponse
    {
        [DataMember] public PlatformUserIdMap[] userIdPlatforms;
    }

    [DataContract, Preserve]
    public class CountryInfo
    {
        [DataMember] public string countryCode;
        [DataMember] public string countryName;
        [DataMember] public string state;
        [DataMember] public string city;
    }

    [DataContract, Preserve]
    public class Country
    {
        [DataMember]  public string code;
        [DataMember] public string name;
    
    }

    [DataContract, Preserve]
    public class UpgradeAndVerifyHeadlessRequest
    {
        [DataMember] public string code;
        [DataMember] public string country; //optional
        [DataMember] public string dateOfBirth; //optional
        [DataMember] public string displayName; //optional
        [DataMember] public string emailAddress;
        [DataMember] public string password;
        [DataMember] public bool reachMinimumAge = true; //optional. If user input DOB, BE will not check this field
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string username;
        [DataMember] public bool validateOnly = false;
    }

    [DataContract, Preserve]
    public class AccountLinkedPlatform
    {
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string platformUserId;
    }

    [DataContract, Preserve]
    public class UnlinkPlatformAccountRequest
    {
        [DataMember] public string platformNamespace;
    }

    public class UnlinkPlatformAccountParameter
    {
        public string PlatformId;
    }

    [DataContract, Preserve]
    public class AccountLinkPublisherAccount
    {
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public AccountLinkedPlatform[] linkedPlatforms;

    }

    [DataContract, Preserve]
    public class AccountLinkConfictMessageVariables
    {
        [DataMember] public string platformUserID;
        [DataMember] public AccountLinkPublisherAccount[] publisherAccounts;
    }

    [DataContract, Preserve]
    public class LinkPlatformAccountRequest
    {
        [DataMember] public string platformId;
        [DataMember] public string platformUserId;
    }

    public class LinkPlatformAccountParameter
    {
        public string UserId;
    }

    [DataContract, Preserve]
    public class UpdateEmailRequest
    {
        [DataMember] public string code;
        [DataMember] public string emailAddress;
    }

    [DataContract, Preserve]
    public class UpgradeRequest
    {
        [DataMember(Name = "emailAddress")] public string EmailAddress;
        [DataMember(Name = "password")] public string Password;
    }

    public class UpgradeParameter
    {
        public bool NeedVerificationCode;
    }

    [DataContract, Preserve]
    public class UpgradeV2Request
    {
        [DataMember(Name = "emailAddress")] public string EmailAddress;
        [DataMember(Name = "password")] public string Password;
        [DataMember(Name = "username")] public string Username;
    }

    [DataContract, Preserve]
    public class SendVerificationCodeRequest
    {
        [DataMember(Name = "emailAddress")] public string EmailAddress;
        [DataMember(Name = "context")] public string Context;
    }

    [DataContract, Preserve]
    public class VerifyRequest
    {
        [DataMember(Name = "code")] public string VerificationCode;
        [DataMember(Name = "contactType")] public string ContactType;
    }

    [DataContract, Preserve]
    public class SendPasswordResetCodeRequest
    {
        [DataMember(Name = "emailAddress")] public string EmailAddress;
    }


    [DataContract, Preserve]
    public class ResetPasswordRequest
    {
        [DataMember(Name = "code")] public string ResetCode;
        [DataMember(Name = "newPassword")] public string NewPassword;
        [DataMember(Name = "emailAddress")] public string EmailAddress;
    }

    [DataContract, Preserve]
    public class LinkOtherPlatformRequest
    {
        [DataMember(Name = "platformId")] public string PlatformId;
    }

    public class LinkOtherPlatformParameter
    {
        public string Ticket;
    }

    public class GetPlatformLinkRequest
    {
        public string UserId;
    }

    public class SearchUsersRequest
    {
        public string Query;
        public SearchType SearchBy;
        public int Offset;
        public int Limit;
        public string PlatformId;
        public SearchPlatformType PlatformBy;
        public readonly string[] FilterType = { "", "displayName", "username", "thirdPartyPlatform" , "uniqueDisplayName" };
    }

    public class GetUserByUserIdRequest
    {
        public string UserId;
    }

    public class GetUserByOtherPlatformUserIdRequest
    {
        public string PlatformId;
        public string PlatformUserId;
    }

    #region Ban
    /// <summary>
    /// Type of Ban that available
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum BanType { LOGIN, CHAT_SEND, CHAT_ALL, ORDER_AND_PAYMENT, STATISTICS, LEADERBOARD, MATCHMAKING, UGC_CREATE_UPDATE }

    /// <summary>
    /// Type of Ban reason that available
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
    public enum BanReason { VIOLENCE, HARASSMENT, HATEFUL_CONDUCT, OFFENSIVE_USERNAME, IMPERSONATION, 
        MALICIOUS_CONTENT, SEXUALLY_SUGGESTIVE, SEXUAL_VIOLENCE, EXTREME_VIOLENCE, UNDERAGE_USER, CHEATING, TOS_VIOLATION }

    /// <summary>
    /// Information about user that performing a ban
    /// </summary>
    /// <param name="displayName"> display name of the User</param>
    /// <param name="userId"> user ID of the User</param>
    [DataContract, Preserve]
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
    [DataContract, Preserve]
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
    [DataContract, Preserve]
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

    [DataContract, Preserve]
    public class UserEnableBan
    {
        [DataMember] public bool enabled;
    }

    [DataContract, Preserve]
    public class UserBanPagedList
    {
        [DataMember] public UserBanResponseV3[] data;
        [DataMember] public Paging paging;
    }
    #endregion

    [DataContract, Preserve]
    public class ListBulkUserInfoRequest
    {
        [DataMember] public string[] userIds;
    }

    public class Change2FAFactorParameter
    {
        public string MfaToken;
        public string Factor;
    }

    public class Enable2FAAuthenticatorParameter
    {
        public string Code;
    }

    public class Make2FAFactorDefaultParameter
    {
        public string FactorType;
    }

    public class GetInputValidationsParameter
    {
        public string LanguageCode;
        public bool DefaultOnEmpty;
    }

    public class GetPublisherUserParameter
    {
        public string UserId;
    }

    public class GetUserInformationParameter
    {
        public string UserId;
    }

    [DataContract, Preserve]
    public class BaseUserInfo
    {
        [DataMember] public string avatarUrl;
        [DataMember] public string displayName;
        [DataMember] public Dictionary<string, string> platformUserIds; 
        [DataMember] public string userId;
        [DataMember] public string publisherAvatarUrl;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string username;
    }

    [DataContract, Preserve]
    public class NamespaceRole
    {
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string roleId;
    }

    [DataContract, Preserve]
    public class UserOtherPlatformInfo
    {
        [DataMember] public string authType;
        [DataMember(Name = "avatarUrl")] public string AvatarUrl;
        [DataMember] public Ban[] bans;
        [DataMember] public string country;
        [DataMember] public string createdAt;
        [DataMember] public string dateOfBirth;
        [DataMember(Name = "deletionDate")] public string DeletionDate;
        [DataMember] public bool deletionStatus;
        [DataMember] public string displayName;
        [DataMember] public string emailAddress;
        [DataMember] public bool emailVerified;
        [DataMember] public bool enabled;
        [DataMember] public string lastDateOfBirthChangedTime;
        [DataMember] public string lastEnabledChangedTime;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public NamespaceRole[] namespaceRoles;
        [DataMember] public string newEmailAddress;
        [DataMember] public string oldEmailAddress;
        [DataMember] public Permission[] permissions;
        [DataMember] public string phoneNumber;
        [DataMember] public bool phoneVerified;
        [DataMember(Name = "platformAvatarUrl")] public string PlatformAvatarUrl;
        [DataMember] public string platformDisplayName; //optional
        [DataMember] public string platformId; //optional
        [DataMember] public string platformUserId; //optional
        [DataMember] public string[] roles;
        [DataMember] public string userId;
        [DataMember] public string username; //optional
    }

    [DataContract, Preserve]
    public class PagedUserOtherPlatformInfo
    {
        [DataMember] public UserOtherPlatformInfo[] data;
        [DataMember] public Paging paging;
        [DataMember] public int totalData;
    }

    [DataContract, Preserve]
    public class TwoFACode
    {
        [DataMember] public int generatedAt;
        [DataMember] public string[] invalidCode;
        [DataMember] public string[] validCodes;
    }

    [DataContract, Preserve]
    public class SecretKey3rdPartyApp
    {
        [DataMember] public string secretKey;
        [DataMember] public string uri;
    }

    [DataContract, Preserve]
    public class Enable2FAFactors
    { 
        [DataMember(Name = "default")] public string default_;
        [DataMember] public string[] enabled;
    }

    [DataContract, Preserve]
    public class ValidationDescription
    {
        [DataMember] public string language;
        [DataMember] public string[] message;
    }

    [DataContract, Preserve]
    public class Validation
    {
        [DataMember] public bool allowDigit;
        [DataMember] public bool allowLetter;
        [DataMember] public bool allowSpace;
        [DataMember] public bool allowUnicode;
        [DataMember] public ValidationDescription description;
        [DataMember] public bool isCustomRegex;
        [DataMember] public string letterCase;
        [DataMember] public int maxLength;
        [DataMember] public int maxRepeatingAlphaNum;
        [DataMember] public int maxRepeatingSpecialCharacter;
        [DataMember] public int minCharType;
        [DataMember] public int minLength;
        [DataMember] public string regex;
        [DataMember] public string specialCharacterLocation;
        [DataMember] public string[] specialCharacters;
    }

    [DataContract, Preserve]
    public class DataInputValidation
    {
        [DataMember] public string field;
        [DataMember] public Validation validation;

    }

    [DataContract, Preserve]
    public class InputValidation
    {
        [DataMember] public DataInputValidation[] data; 
        [DataMember] public int version;
    }
    
    [DataContract, Preserve]
    public class GetPublisherUserResponse
    {
        [DataMember(Name = "namespace")] public string namespace_; 
        [DataMember] public string userId;
    }
    
    [DataContract, Preserve]
    public class PlatformUserInformation
    {
        [DataMember] public string displayName;
        [DataMember] public string emailAddress;
        [DataMember] public string linkedAt;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string platformId;
        [DataMember] public string platformUserId;
        [DataMember] public string xboxUserId;
    }
    
    [DataContract, Preserve]
    public class GetUserInformationResponse
    {
        [DataMember] public string country;
        [DataMember] public string displayName;
        [DataMember] public string[] emailAddresses;
        [DataMember] public string phoneNumber;
        [DataMember] public PlatformUserInformation[] platformUsers;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember] public string username;
        [DataMember] public string xboxUserId;
    }
    
    [DataContract, Preserve]
    public class GeneratedOneTimeCode
    {
        [DataMember] public string oneTimeLinkCode;
        [DataMember(Name = "oneTimeLinkUrl")] public string oneTimeLinkURL;
        [DataMember] public int exp;
    }

    [DataContract, Preserve]
    public class CodeForTokenExchangeResponse
    {
        [DataMember(Name = "code")] public string Code;
    }

    [DataContract, Preserve]
    public class ListUserDataRequest
    {
        [DataMember(Name = "userIds")] public string[] UserIds;
    }

    [DataContract, Preserve]
    public class UserDataResponse
    {
        [DataMember(Name = "displayName")] public string DisplayName;
        [DataMember(Name = "emailAddress")] public string EmailAddress;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember(Name = "userId")] public string UserId;
    }

    [DataContract, Preserve]
    public class ListUserDataResponse
    {
        [DataMember(Name = "data")] public UserDataResponse[] Data;
    }

    [DataContract, Preserve]
    public class GetBulkUserByEmailAddressRequest
    {
        [DataMember(Name = "listEmailAddressRequest")] public string[] ListEmailAddressRequest;
    }

    [DataContract, Preserve]
    public class GetBulkUserByEmailAddressResponse
    {
        [DataMember(Name = "data")] public UserData[] Data;
    }

    [DataContract, Preserve]
    public class LinkHeadlessAccountRequest
    {
        [DataMember] public string[] ChosenNamespaces;
        [DataMember] public string OneTimeLinkCode;
    }

    public class GetConflictResultWhenLinkHeadlessAccountToFullAccountRequest
    {
        public string OneTimeLinkCode;
    }

    [DataContract, Preserve]
    public class AccountProgressionInfo
    {
        [DataMember] public string DisplayName;
        [DataMember] public string Email;
        [DataMember] public string[] LinkedGames;
        [DataMember] public string UserName;
    }
    
    [DataContract, Preserve]
    public class ConflictLinkHeadlessAccountResult
    {
        [DataMember] public AccountProgressionInfo CurrentAccount;
        [DataMember] public AccountProgressionInfo HeadlessAccount;
        [DataMember] public bool PlatformAlreadyLinked;
        [DataMember] public string PlatformId;
        [DataMember] public bool PlatformLinkConflict;
    }

    [Preserve]
    internal class RefreshTokenCache
    {
        public string RefreshToken;
        public DateTime ExpiredDate;

        public RefreshTokenCache()
        {

        }
    }

    [Preserve]
    internal class LoginAdditionalInfo
    {
        internal string PlatformId;
    }

    public static class MaximumUserIds
    {
        /** Attributes that limit the user ids allowed to proceed with the request */
        public const int UserIdsLimit = 40;
    }

    [DataContract, Preserve]
    public class ThirdPartyPlatformTokenData
    {
        [DataMember(Name = "platform_token")] public string PlatformToken;
        [DataMember(Name = "platform_token_expires_at")] public int ExpiredAt;
        [DataMember(Name = "sand_box_id")] public string SanboxId; 
    }

    [DataContract, Preserve]
    public class AccountUserPlatformInfo
    {
        [DataMember(Name = "platformAvatarUrl")] public string PlatformAvatarUrl;
        [DataMember(Name = "platformDisplayName")] public string PlatformDisplayName;
        [DataMember(Name = "platformGroup")] public string PlatformGroup;
        [DataMember(Name = "platformId")] public string PlatformId;
        [DataMember(Name = "platformUserId")] public string PlatformUserId;
    }

    [DataContract, Preserve]
    public class AccountUserPlatformData
    {
        [DataMember(Name = "avatarUrl")] public string AvatarUrl;
        [DataMember(Name = "displayName")] public string DisplayName;
        [DataMember(Name = "platformInfos")] public List<AccountUserPlatformInfo> PlatformInfos;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember(Name = "userId")] public string UserId;
    }

    [DataContract, Preserve]
    public class PlatformAccountInfoRequest
    {
        [DataMember(Name = "platformId")] public string PlatformId;
        [DataMember(Name = "userIds")] public string[] UserIds;
    }

    [DataContract, Preserve]
    public class AccountUserPlatformInfosResponse
    {
        [DataMember(Name = "data")] public AccountUserPlatformData[] Data;
    }

    [DataContract, Preserve]
    public class SendVerificationCodeToNewUserRequest
    {
        [DataMember(Name = "emailAddress")] public string EmailAddress;
        [DataMember(Name = "languageTag")
            , JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string LanguageTag;
    }

    /// <summary>
    /// Request verification code to email address optional parameters. Can be null.
    /// </summary>
    public class SendVerificationCodeToNewUserOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Language of email to be sent if configured.
        /// </summary>
        public string LanguageTag = null;
    }

    /// <summary>
    /// Holds fields to be validated against input validation and profanity filter (if enabled). Unused fields can be left null
    /// </summary>
    [DataContract, Preserve]
    public class ValidateInputRequest
    {
        [DataMember(Name = "displayName")] public string DisplayName = null;
        [DataMember(Name = "password")] public string Password = null;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName = null;
        [DataMember(Name = "username")] public string Username = null;
    }

    [Preserve]
    public class ValidateUserInputOptionalParameters : OptionalParametersBase
    {
        
    }

    [DataContract, Preserve]
    public class ValidateInputResponse
    {
        [DataMember(Name = "message")] public string Message; 
        [DataMember(Name = "valid")] public bool IsValid; 
    }

    [Preserve]
    public class LoginWithUsernameV3OptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Set it to true to extend the refresh token expiration time
        /// </summary>
        public bool? RememberMe = false;
    };

    [Preserve]
    public class LoginWithOtherPlatformOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Set it to true  because it doesn't have username yet
        /// </summary>
        public bool? CreateHeadless = true;

        /// <summary>
        /// (Early-access: for PS5 only currently)Used to validate PSN app when AppId is set on Admin Portal for PS4/PS5
        /// </summary>
        public string ServiceLabel = null;

        /// <summary>
        /// Include mac Address information for PSN and Xbox ban reporting
        /// </summary>
        public LoginWithMacAddress LoginWithMacAddress = null;

        /// <summary>
        /// If need skip set cookie. Default is false
        /// </summary>
        public bool SkipSetCookie;
    }

    [Preserve]
    public class LoginWithLauncherOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class LoginWithDeviceIdOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class LoginWithRefreshTokenOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class LogoutOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class CreateHeadlessAccountAndResponseTokenOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class AuthenticationWithPlatformLinkOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class Verify2FACodeOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Will record device token when true
        /// </summary>
        public bool? RememberDevice = false;
    }

    [Preserve]
    public class VerifyTokenOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GenerateCodeForPublisherTokenExchangeOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class GenerateGameTokenOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class RetrieveUserThirdPartyPlatformTokenOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class GetOrRefreshDataOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Set to true to also retrieve data from all user's linked platform accounts.
        /// </summary>
        public bool? IsIncludeAllPlatforms = false;
    }

    [Preserve]
    public class UpdateUserDataOptionalParameters : OptionalParametersBase
    {
        
    }

    [Preserve]
    public class UpdateEmailOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UpgradeHeadlessAccountOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Set to true to require verification code that will be sent to user's registered email
        /// </summary>
        public bool NeedVerificationCode = false;
    }

    [Preserve]
    public class UpgradeHeadlessAccountV2OptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UpgradeAndVerifyHeadlessAccountOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class SendUpgradeVerificationCodeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class VerifyUserEmailOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class SendPasswordResetCodeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class ResetPasswordOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class LinkOtherPlatformOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class ForceLinkOtherPlatformOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UnlinkOtherPlatformOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class UnlinkAllOtherPlatformOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetPlatformLinksOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class SearchUsersOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 100;

        /// <summary>
        /// Specify platform type, string type of this field makes support OpenID Connect (OIDC).
        /// </summary>
        public string PlatformId = null;

        /// <summary>
        /// Filter the responded PagedPublicUsersInfo by SearchPlatformType.
        /// </summary>
        public SearchPlatformType? SearchPlatformType = Models.SearchPlatformType.None;
    }

    [Preserve]
    public class GetUserByOtherPlatformUserIdV4OptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetCountryFromIPOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetCountryGroupV3OptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class Change2FAFactorOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class Disable2FAAuthenticatorOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class Enable2FAAuthenticatorOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GenerateSecretKeyFor3rdPartyAuthenticateAppOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GenerateBackUpCodeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class Disable2FABackupCodesOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class Enable2FABackupCodesOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetBackUpCodeOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetUserEnabledFactorsOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class Make2FAFactorDefaultOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetInputValidationsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Should return default language if languageCode is empty or language not available if set to true.
        /// </summary>
        public bool? DefaultOnEmpty = true;
    }

    [Preserve]
    public class UpdateUserOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetPublisherUserOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetUserInformationOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class LinkHeadlessAccountToCurrentFullAccountOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetConflictResultWhenLinkHeadlessAccountToFullAccountOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class CheckUserAccountAvailabilityByFieldNameOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetConfigValueOptionalParameters : OptionalParametersBase
    {

    }

    [Preserve]
    public class GetUserOtherPlatformBasicPublicInfoOptionalParameters : OptionalParametersBase
    {

    }

    [DataContract, Preserve]
    internal class ConfigValueResponse<T>
    {
        [DataMember(Name = "result")] public T Result;
    }

    [DataContract, Preserve]
    internal class UniqueDisplayNameEnabledResponse
    {
        [DataMember(Name = "uniqueDisplayNameEnabled")] public bool UniqueDisplayNameEnabled;
    }

    [DataContract, Preserve]
    internal class DisplayNameDisabledResponse
    {
        [DataMember(Name = "usernameDisabled")] public bool UserNameDisabled;
    }

#region Strong Typed Variables
    
    public class LoginWithMacAddress
    {
        private bool isSendMacAddress;
        
        public LoginWithMacAddress(bool useMacAddress)
        {
            isSendMacAddress = useMacAddress;
        }

        public LoginWithMacAddress()
        {
            isSendMacAddress = true;
        }
        
        internal bool IsLoginWithMacAddress()
        {
#if (!UNITY_PS4 && !UNITY_PS5 && !UNITY_GAMECORE_XBOXSERIES && !UNITY_GAMECORE_XBOXONE) || UNITY_EDITOR
            return false;
#else
            return isSendMacAddress;
#endif
        }
    }

    #endregion

    #region V4
    [DataContract, Preserve]
    public class LoginQueueTicket
    {
        [DataMember(Name = "estimatedWaitingTimeInSeconds")] public int EstimatedWaitingTimeInSeconds;
        [DataMember(Name = "playerPollingTimeInSeconds")] public int PlayerPollingTimeInSeconds;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "position")] public int Position;
        [DataMember(Name = "reconnectExpiredAt")] public int ReconnectExpiredAt;
        [DataMember(Name = "ticket")] public string Ticket;
        [DataMember] public string Identifier;
    }

    [DataContract, Preserve]
    public class TokenDataV4 : TokenData
    {
        [DataMember] public LoginQueueTicket Queue;
    }
    
    [DataContract, Preserve]
    public class GetUserPublicInfoOptionalParameters : OptionalParametersBase
    {
    }

    [DataContract, Preserve]
    public class GetUserPublicInfoResponse
    {
        [DataMember(Name = "avatarUrl")] public string AvatarUrl;
        [DataMember(Name = "displayName")] public string DisplayName;
        [DataMember(Name = "uniqueDisplayName")] public string UniqueDisplayName;
        [DataMember(Name = "userId")] public string UserId;
    }

    [DataContract, Preserve]
    public class GetPublicSystemConfigValueResponse
    {
        [DataMember(Name = "searchQueryMaxLength")] public uint SearchQueryMaxLength;
        [DataMember(Name = "searchQueryMaxLengthForEmail")] public uint SearchQueryMaxLengthForEmail;
        [DataMember(Name = "searchQueryMinLength")] public uint SearchQueryMinLength;
    }

    [Preserve]
    public class GetPublicSystemConfigValueOptionalParameters : OptionalParametersBase
    {

    }

    [DataContract, Preserve]
    public class GetLinkedPlatformAccountsOptionalParams : OptionalParametersBase
    {
        public PlatformType PlatformId;
        public string TargetNamespace;
    }

    [DataContract, Preserve]
    internal class LoginQueueTicketResponse : LoginQueueTicket
    {
        [DataMember(Name = "refresh")] internal TicketEndpointAction Refresh;
        [DataMember(Name = "cancel")] internal TicketEndpointAction Cancel;
    }

    [DataContract, Preserve]
    internal class TicketEndpointAction
    {
        [DataMember(Name = "action")] public string Action;
        [DataMember(Name = "href")] public string Href;
    };

    [Preserve]
    public class GenerateOneTimeCodeOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// A Redirect Uri that will be called as DeepLinking uri
        /// It have to be registered through IAM Client settings on Admin Portal
        /// </summary>
        public string RedirectUri;
        
        /// <summary>
        /// A string that will be given back to the game client
        /// It can be used for a security validation
        /// </summary>
        public string State;
    }

    [Preserve]
    public class RequestTokenByOneTimeLinkCodeOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// A flag to produce transient token
        /// </summary>
        public bool? IsTransient = false;
        
        /// <summary>
        /// Additional info, it will be passed to login success event.
        /// </summary>
        public string AdditionalData;
    }

    [Preserve]
    public class LoginV4OptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Cancelation token that can be used to cancel the queue process
        /// </summary>
        public System.Threading.CancellationToken? CancellationToken;

        /// <summary>
        /// Set the the login queue timeout
        /// </summary>
        public LoginV4Timeout LoginTimeout = new LoginV4Timeout(120 * 1000);

        /// <summary>
        /// Returns the queue refreshed token update periodically
        /// </summary>
        public Action<LoginQueueTicket> OnQueueUpdatedEvent;

        /// <summary>
        /// Return when cancellation queue ticket is triggered
        /// </summary>
        public Action OnCancelledEvent;
    }

    public class LoginV4Timeout
    {
        private const uint minTime = 30000; 
        public readonly uint TimeoutMs;

        public LoginV4Timeout(uint timeoutMs)
        {
            var selectedTimeoutTime = System.Math.Max(timeoutMs, minTime);
            TimeoutMs = selectedTimeoutTime;
        }
    }

    [Preserve]
    public class LoginWithEmailV4OptionalParameters : LoginV4OptionalParameters
    {
        /// <summary>
        /// Set it to true to extend the refresh token expiration time
        /// </summary>
        public bool RememberMe = false;
    }

    [Preserve]
    public class LoginWithDeviceIdV4OptionalParameters : LoginV4OptionalParameters { }

    [Preserve]
    public class LoginWithOtherPlatformV4OptionalParameters : LoginV4OptionalParameters
    {
        /// <summary>
        /// If directly create new account when not linked yet
        /// </summary>
        public bool CreateHeadless = true;

        /// <summary>
        /// (Early-access: for PS5 only currently)Used to validate PSN app when AppId is set on Admin Portal for PS4/PS5
        /// </summary>
        public string ServiceLabel = null;

        /// <summary>
        /// Include mac Address information for PSN and Xbox ban reporting
        /// </summary>
        public LoginWithMacAddress LoginWithMacAddress = null;

        /// <summary>
        /// If need skip set cookie. Default is false
        /// </summary>
        public bool SkipSetCookie;
    }

    [Preserve]
    public class LoginWithRefreshTokenV4OptionalParameters : LoginV4OptionalParameters { }

    [Preserve]
    public class CreateHeadlessAccountAndResponseTokenV4OptionalParameters : LoginV4OptionalParameters { }

    [Preserve]
    public class AuthenticationWithPlatformLinkAndLoginV4OptionalParameters : LoginV4OptionalParameters { }

    [Preserve]
    public class GenerateGameTokenV4OptionalParameters : LoginV4OptionalParameters { }

    [Preserve]
    public class Verify2FACodeV4OptionalParameters : LoginV4OptionalParameters
    {
        /// <summary>
        /// Record device token when true
        /// </summary>
        public bool RememberDevice = false;
    }
    #endregion
};
