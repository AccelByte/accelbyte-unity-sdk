// Copyright (c) 2019 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class Paging
    {
        [DataMember] public string first;
        [DataMember] public string last;
        [DataMember] public string next;
        [DataMember] public string previous;
    }

    [DataContract, Preserve]
    public class PagingCursor
    {
        [DataMember] public string next;
        [DataMember] public string previous;
    }
    
    [DataContract, Preserve]
    public class PaginatedResponse<T> where T : class, new()
    {
        [DataMember] public T[] data;
        [DataMember] public Paging paging;
    }

    [JsonConverter(typeof(StringEnumConverter)), System.Serializable]
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
	    Default,
        Sandbox,
        QA,
        Integration
    };
}