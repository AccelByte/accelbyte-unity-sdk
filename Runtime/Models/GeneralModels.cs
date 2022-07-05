// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    [DataContract]
    public class Paging
    {
        [DataMember] public string first { get; set; }
        [DataMember] public string last { get; set; }
        [DataMember] public string next { get; set; }
        [DataMember] public string previous { get; set; }
    }

    [DataContract]
    public class PagingCursor
    {
        [DataMember] public string next { get; set; }
        [DataMember] public string previous { get; set; }
    }

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum PaginationType
    {
        FIRST,
        NEXT,
        PREVIOUS
    }

    public enum SettingsEnvironment
    {
        Development,
	    Certification,
	    Production,
	    Default
    };
}