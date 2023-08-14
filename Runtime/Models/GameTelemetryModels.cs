// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class TelemetryBody
    {
        [DataMember] public string EventName;
        [DataMember] public string EventNamespace;
        [DataMember] public object Payload;
        [DataMember] public DateTime ClientTimestamp;

        public TelemetryBody() 
        {
            ClientTimestamp = DateTime.Now;
        }
    }
}