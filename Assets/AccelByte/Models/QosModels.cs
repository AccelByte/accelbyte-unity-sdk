// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class QosServerList
    {
        [DataMember] public QosServer[] servers { get; set; }
    }

    [DataContract]
    public class QosServer
    {
        [DataMember] public string ip { get; set; }
        [DataMember] public int port { get; set; }
        [DataMember] public string region { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string last_update { get; set; }
    }
}