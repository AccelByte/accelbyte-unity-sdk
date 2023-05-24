// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class TelemetryEventTag
    {
        [DataMember] public uint AppId;
        [DataMember] public uint Id;
        [DataMember] public uint Level;
        [DataMember] public uint Type;
        [DataMember] public uint UX;
    }
}