﻿// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class PublicIp
    {
        [DataMember] public string ip;
    }
}
