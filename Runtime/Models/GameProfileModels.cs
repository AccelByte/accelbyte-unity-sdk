// Copyright (c) 2019 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class GameProfile
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string profileId;
        [DataMember] public string userId;
        [DataMember] public Dictionary<string, string> attributes;
        [DataMember] public string avatarUrl;
        [DataMember] public string label;
        [DataMember] public string profileName;
        [DataMember] public string[] tags;
    }

    [DataContract, Preserve]
    public class GameProfileRequest
    {
        [DataMember] public Dictionary<string, string> attributes;
        [DataMember] public string avatarUrl;
        [DataMember] public string label;
        [DataMember] public string profileName;
        [DataMember] public string[] tags;

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

    [DataContract, Preserve]
    public class GameProfileAttribute
    {
        [DataMember] public string name;
        [DataMember] public string value;
    }

    [DataContract, Preserve]
    public class GameProfilePublicInfo
    {
        [DataMember] public string profileId;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string profileName;
        [DataMember] public string avatarUrl;
    }

    [DataContract, Preserve]
    public class UserGameProfiles
    {
        [DataMember] public string userId;
        [DataMember] public GameProfilePublicInfo[] gameProfiles;
    }
}