// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class ServiceVersionInfo
    {
        [DataMember] public string BuildDate;
        [DataMember] public string GitHash;
        [DataMember] public string Name;
        [DataMember] public string Realm;
        [DataMember] public string Version;
        [DataMember] public string VersionRoleSeeding;
    }
}
