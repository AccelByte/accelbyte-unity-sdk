// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    internal class GetPartySessionStorageResult
    {
        [DataMember(Name = "reserved")] public Dictionary<string, PartySessionStorageReservedData> Reserved;
    }

    [DataContract, Preserve]
    internal class PartySessionStorageReservedData
    {
        [DataMember(Name = "pastSessionIds")] public string[] PastSessionIds;

        public PartySessionStorageReservedData()
        {
        }

        public PartySessionStorageReservedData(IEnumerable<string> pastSessionIds)
        {
            PastSessionIds = pastSessionIds.ToArray();
        }
    }
}