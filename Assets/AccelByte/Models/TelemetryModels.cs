// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class TelemetryEventTag
    {
        [DataMember] public uint AppId { get; set; }
        [DataMember] public uint Id { get; set; }
        [DataMember] public uint Level { get; set; }
        [DataMember] public uint Type { get; set; }
        [DataMember] public uint UX { get; set; }
    }
}