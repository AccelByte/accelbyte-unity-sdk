// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public enum EJwtResult
    {
        [EnumMember] Ok,
        [EnumMember] MalformedJwt,
        [EnumMember] MalformedPublicKey,
        [EnumMember] AlgorithmMismatch,
        [EnumMember] SignatureMismatch
    }

    [DataContract, Preserve]
    public class JwkSet
    {
        [DataMember(Name = "keys")]
        public List<JObject> keys;
    }
}
