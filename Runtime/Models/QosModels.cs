// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class QosServerList
    {
        [DataMember] public QosServer[] servers;
    }

    [DataContract, Preserve]
    public class QosServer
    {
        [DataMember] public string ip;
        [DataMember] public int port;
        [DataMember] public string region;
        [DataMember] public string status;
        [DataMember] public string last_update;
    }
}