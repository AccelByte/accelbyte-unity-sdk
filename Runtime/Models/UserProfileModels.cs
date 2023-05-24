// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class UserProfile
    {
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string firstName;
        [DataMember] public string lastName;
        [DataMember] public string avatarSmallUrl;
        [DataMember] public string avatarUrl;
        [DataMember] public string avatarLargeUrl;
        [DataMember] public string status;
        [DataMember] public string language;
        [DataMember] public string timeZone;
        [DataMember] public string dateOfBirth;
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public string publicId;
        [DataMember] public string zipCode;
        [DataMember] public Dictionary<string, object> privateCustomAttributes; // This fiels will not appear when call public user profile 
    }

    [DataContract, Preserve]
    public class PublicUserProfile
    {
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string timeZone;
        [DataMember] public string avatarSmallUrl;
        [DataMember] public string avatarUrl;
        [DataMember] public string avatarLargeUrl;
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public string publicId;
    }

    [DataContract, Preserve]
    public class CreateUserProfileRequest
    {
        [DataMember] public string firstName;
        [DataMember] public string lastName;
        [DataMember] public string language;
        [DataMember] public string avatarSmallUrl;
        [DataMember] public string avatarUrl;
        [DataMember] public string avatarLargeUrl;
        [DataMember] public string timeZone;
        [DataMember] public string dateOfBirth;
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public Dictionary<string, object> privateCustomAttributes;
    }

    [DataContract, Preserve]
    public class UpdateUserProfileRequest
    {
        [DataMember] public string firstName;
        [DataMember] public string lastName;
        [DataMember] public string language;
        [DataMember] public string avatarSmallUrl;
        [DataMember] public string avatarUrl;
        [DataMember] public string avatarLargeUrl;
        [DataMember] public string timeZone;
        [DataMember] public string dateOfBirth;
        [DataMember] public Dictionary<string, object> customAttributes;
        [DataMember] public string zipCode;
        [DataMember] public Dictionary<string, object> privateCustomAttributes;
    }
};
