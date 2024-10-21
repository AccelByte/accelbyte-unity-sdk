// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine.Scripting;
using System.ComponentModel;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QosStatus
    {
        [Description("UNREACHABLE"), EnumMember(Value = "UNREACHABLE")] Unreachable,
        [Description("INACTIVE"), EnumMember(Value = "ACTIVE")] Inactive,
        [Description("ACTIVE"), EnumMember(Value = "ACTIVE")] Active
    }

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

    [Preserve]
    public class GetQosServerOptionalParameters
    {
        internal QosStatus? Status;
        internal ILatencyCalculator LatencyCalculator;
    }
}