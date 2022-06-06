// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class UserProfile
    {
        [DataMember] public string userId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string firstName { get; set; }
        [DataMember] public string lastName { get; set; }
        [DataMember] public string avatarSmallUrl { get; set; }
        [DataMember] public string avatarUrl { get; set; }
        [DataMember] public string avatarLargeUrl { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public string timeZone { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
        [DataMember] public string publicId { get; set; }
        [DataMember] public Dictionary<string, object> privateCustomAttributes { get; set; }

    }

    [DataContract]
    public class PublicUserProfile
    {
        [DataMember] public string userId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string timeZone { get; set; }
        [DataMember] public string avatarSmallUrl { get; set; }
        [DataMember] public string avatarUrl { get; set; }
        [DataMember] public string avatarLargeUrl { get; set; }
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
        [DataMember] public string publicId { get; set; }
    }

    [DataContract]
    public class CreateUserProfileRequest
    {
        [DataMember] public string firstName { get; set; }
        [DataMember] public string lastName { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public string avatarSmallUrl { get; set; }
        [DataMember] public string avatarUrl { get; set; }
        [DataMember] public string avatarLargeUrl { get; set; }
        [DataMember] public string timeZone { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
    }

    [DataContract]
    public class UpdateUserProfileRequest
    {
        [DataMember] public string firstName { get; set; }
        [DataMember] public string lastName { get; set; }
        [DataMember] public string language { get; set; }
        [DataMember] public string avatarSmallUrl { get; set; }
        [DataMember] public string avatarUrl { get; set; }
        [DataMember] public string avatarLargeUrl { get; set; }
        [DataMember] public string timeZone { get; set; }
        [DataMember] public string dateOfBirth { get; set; }
        [DataMember] public Dictionary<string, object> customAttributes { get; set; }
    }

    [DataContract]
    public class Time
    {
        [DataMember] public System.DateTime currentTime;
    }
}