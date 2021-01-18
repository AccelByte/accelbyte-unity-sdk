// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    public enum AuthenticationType { EMAILPASSWD, PHONEPASSWD }

    public enum SearchType { ALL, EMAILADDRESS, DISPLAYNAME, USERNAME }

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
    public class PublicUserInfo
    {
        [DataMember] public string country { get; set; }
        [DataMember] public DateTime createdAt { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string phoneNumber { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember] public string userName { get; set; }
    }

    [DataContract]
    public class PagedPublicUsersInfo
    {
        public PublicUserInfo[] data { get; set; }
        public Paging paging { get; set; }
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
    }

    [DataContract]
    public class RegisterUserRequestv2
    {
        [DataMember] public AuthenticationType authType { get; set; }
        [DataMember] public string country { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public string displayName { get; set; }
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public string password { get; set; }
        [DataMember] public string username { get; set; }
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

    public enum PlatformType { Steam, PS4, Live, Stadia, Google, Facebook, Twitch, Oculus, Twitter, Device }

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
        [DataMember] public string CountryCode { get; set; }
        [DataMember] public string CountryName { get; set; }
        [DataMember] public string State { get; set; }
        [DataMember] public string City { get; set; }
    }

    [DataContract]
    public class UpgradeUserRequest
    {
        [DataMember] public string temporary_session_id { get; set; }
    }

    [DataContract]
    public class AccountLinkedPlatform
    {
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string platformUserId { get; set; }
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
}
