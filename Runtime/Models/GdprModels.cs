// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{ 
    [DataContract, Preserve]
    public class SubmitAccountDeletionResponse
    {
        [DataMember] public string Namespace;
        [DataMember(Name = "UserID")] public string UserId;
    }

    [DataContract, Preserve]
    public class AccountDeletionStatusResponse
    {
        [DataMember] public string DeletionDate; // can be empty string, since the format dd MMMM yyyy  
        [DataMember] public bool DeletionStatus;
        [DataMember] public string DisplayName;
        [DataMember] public DateTime ExecutionDate;
        [DataMember] public string Status;
        [DataMember(Name = "UserID")] public string UserId;
    }
}