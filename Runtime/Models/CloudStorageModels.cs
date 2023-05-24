// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class Slot
    {
        [DataMember] public string checksum;
        [DataMember] public string customAttribute;
        [DataMember] public DateTime dateAccessed;
        [DataMember] public DateTime dateCreated;
        [DataMember] public DateTime dateModified;
        [DataMember] public string label;
        [DataMember] public string mimeType;
        [DataMember] public string namespaceId;
        [DataMember] public string originalName;
        [DataMember] public string slotId;
        [Obsolete("Status property is deprecated on new cloudstorage service.")]
        [DataMember] public string status;
        [DataMember] public string storedName;
        [DataMember] public string[] tags;
        [DataMember] public string userId;
        
    }

    [DataContract, Preserve]
    public class UpdateMedataRequest
    {
        [DataMember] public string label;
        [DataMember] public string[] tags;
        [DataMember] public string customAttribute;

    }
}
