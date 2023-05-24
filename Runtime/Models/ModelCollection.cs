// Copyright (c) 2019 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class Collection
    {
        [DataMember] public Slot[] slots;
        [DataMember] public UserGameProfiles[] userGameProfiles;
        [DataMember] public GameProfile[] gameProfiles;
        [DataMember] public OrderHistoryInfo[] orderHistories;
        [DataMember] public PlatformLink[] platformLinks;
        [DataMember] public PublicUserProfile[] publicUserProfiles;
    }
}
