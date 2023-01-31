// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    [DataContract]
    public class ServiceVersionInfo
    {
        [DataMember] public string BuildDate { get; set; }
        [DataMember] public string GitHash { get; set; }
        [DataMember] public string Name { get; set; }
        [DataMember] public string Realm { get; set; }
        [DataMember] public string Version { get; set; }
        [DataMember] public string VersionRoleSeeding { get; set; }
    }
}
