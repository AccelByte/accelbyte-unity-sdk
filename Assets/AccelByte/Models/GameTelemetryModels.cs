// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class TelemetryBody
    {
        [DataMember] public string EventName { get; set; }
        [DataMember] public string EventNamespace { get; set; }
        [DataMember] public object Payload { get; set; }
    }
}