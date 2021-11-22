// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

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

    public enum PaginationType
    {
        FIRST,
        NEXT,
        PREVIOUS
    }
}