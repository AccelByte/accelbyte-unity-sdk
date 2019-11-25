// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class GameProfile
    {
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string profileId { get; set; }
        [DataMember] public string userId { get; set; }

        [DataMember] public Dictionary<string, string> attributes { get; set; }
        [DataMember] public string avatarUrl { get; set; }
        [DataMember] public string label { get; set; }
        [DataMember] public string profileName { get; set; }
        [DataMember] public string[] tags { get; set; }
    }

    [DataContract]
    public class GameProfileRequest
    {
        [DataMember] public Dictionary<string, string> attributes { get; set; }
        [DataMember] public string avatarUrl { get; set; }
        [DataMember] public string label { get; set; }
        [DataMember] public string profileName { get; set; }
        [DataMember] public string[] tags { get; set; }

        public static implicit operator GameProfileRequest(GameProfile gameProfile)
        {
            return new GameProfileRequest
            {
                attributes = gameProfile.attributes,
                avatarUrl = gameProfile.avatarUrl,
                label = gameProfile.label,
                profileName = gameProfile.profileName,
                tags = gameProfile.tags,
            };
        }
    }

    [DataContract]
    public class GameProfileAttribute
    {
        [DataMember] public string name { get; set; }
        [DataMember] public string value { get; set; }
    }

    [DataContract]
    public class GameProfilePublicInfo
    {
        [DataMember] public string profileId { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string profileName { get; set; }
        [DataMember] public string avatarUrl { get; set; }
    }

    [DataContract]
    public class UserGameProfiles
    {
        [DataMember] public string userId { get; set; }
        [DataMember] public GameProfilePublicInfo[] gameProfiles { get; set; }
    }
}