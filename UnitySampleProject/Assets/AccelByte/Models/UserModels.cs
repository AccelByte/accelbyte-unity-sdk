// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    public enum AuthenticationType
    {
        EMAILPASSWD,
        PHONEPASSWD
    }

    [DataContract]
    public class TokenData
    {
        [DataMember] public string access_token { get; set; }
        [DataMember] public string refresh_token { get; set; }
        [DataMember] public int expires_in { get; set; }
        [DataMember] public string token_type { get; set; }
        [DataMember] public string user_id { get; set; }
        [DataMember] public string display_name { get; set; }
        [DataMember] public string @namespace { get; set; }
    }

    [DataContract]
    public class UserData
    {
        [DataMember] public string Namespace { get; set; }
        [DataMember] public string UserId { get; set; }
        [DataMember] public string DisplayName { get; set; }
        [DataMember] public string AuthType { get; set; }
        [DataMember] public string LoginId { get; set; }
        [DataMember] public string EmailAddress { get; set; }
        [DataMember] public string OldEmailAddress { get; set; }
//        [DataMember] public DateTime CreatedAt { get; set; }
        [DataMember] public bool PhoneVerified { get; set; }
        [DataMember] public bool EmailVerified { get; set; }
        [DataMember] public bool Enabled { get; set; }
//        [DataMember] public DateTime LastEnabledChangedTime { get; set; }
        [DataMember] public string Country { get; set; }
    }

    [DataContract]
    public class RegisterUserRequest
    {
        [DataMember] public AuthenticationType AuthType { get; set; }
        [DataMember] public string DisplayName { get; set; }
        [DataMember(Name = "LoginId")] public string UserName { get; set; }
        [DataMember] public string Password { get; set; }
    }

    [DataContract]
    public class UpdateUserRequest
    {
        [DataMember] public string Country { get; set; }
        [DataMember] public string DisplayName { get; set; }
        [DataMember] public string EmailAddress { get; set; }
        [DataMember] public string LanguageTag { get; set; }
    }

    [DataContract]
    public class ResetPasswordRequest
    {
        [DataMember] public string Code { get; set; }
        [DataMember] public string LoginID { get; set; }
        [DataMember] public string NewPassword { get; set; }
    }

    public enum PlatformType
    {
        Steam,
        Google,
        Facebook,
        Twitch,
        Oculus,
        Twitter,
        Device
    }

    [DataContract]
    public class PlatformLink
    {
        [DataMember] public string PlatformId { get; set; }
        [DataMember] public string PlatformUserId { get; set; }
        [DataMember] public string Namespace { get; set; }
        [DataMember] public string UserId { get; set; }
    }
}