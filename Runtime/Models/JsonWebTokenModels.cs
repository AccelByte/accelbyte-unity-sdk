// Copyright (c) 2021 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum JsonWebTokenIndex
    {
        Header,
        Payload,
        Signature
    }

    [DataContract, Preserve]
    public class JsonWebTokenDefaultHeader
    {
        [DataMember] public string alg;
        [DataMember] public string kid;
        [DataMember] public string typ;
    }
}
