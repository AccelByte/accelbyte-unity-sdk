// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public enum ProfanityEntrySortBy
    {
        None,
        [EnumMember(Value = "word:desc")] WordDescending,
        [EnumMember(Value = "word:asc")] WordAscending,
    }

    [DataContract, Preserve]
    public enum ProfanityGroupSortBy
    {
        None,
        [EnumMember(Value = "name:desc")] NameDescending,
        [EnumMember(Value = "name:asc")] NameAscending,
        [EnumMember(Value = "count:desc")] CountDescending,
        [EnumMember(Value = "count:asc")] CountAscending,
    }

    /// <summary>
    /// QueryProfanityWords optional parameters. Can be null.
    /// </summary>
    [Preserve]
    public class QueryProfanityWordsOptionalParameters
    {
        /// <summary>
        /// Query profanity words starting with set value. Can be null.
        /// </summary>
        public string StartsWith = null;

        /// <summary>
        /// True if should include false negatives and positives. Can be null.
        /// </summary>
        public bool? IncludeChildren = null;

        /// <summary>
        /// Query profanity words via filter mask value. Can be null.
        /// </summary>
        public string FilterMask = null;

        /// <summary>
        /// Sort returned results by selection. Default value is None.
        /// </summary>
        public ProfanityEntrySortBy SortBy = ProfanityEntrySortBy.None;

        /// <summary>
        /// Offset of displayed results by page. Can be null.
        /// </summary>
        public int? Page = null;

        /// <summary>
        /// Amount of displayed results per page. Can be null.
        /// </summary>
        public int? PageSize = null;
    }

    /// <summary>
    /// GetProfanityWordGroups optional parameters. Can be null.
    /// </summary>
    [Preserve]
    public class GetProfanityWordGroupsOptionalParameters
    {
        /// <summary>
        /// Sort returned results by selection. Default value is None.
        /// </summary>
        public ProfanityGroupSortBy SortBy = ProfanityGroupSortBy.None;

        /// <summary>
        /// Offset of displayed results by page. Can be null.
        /// </summary>
        public int? Page = null;

        /// <summary>
        /// Amount of displayed results per page. Can be null.
        /// </summary>
        public int? PageSize = null;
    }

    [DataContract, Preserve]
    public class ProfanityFilterPagination
    {
        [DataMember(Name = "page")] public long Page;
        [DataMember(Name = "pageSize")] public long PageSize;
        [DataMember(Name = "total")] public long Total;
    }

    [DataContract, Preserve]
    public class ProfanityDictionaryEntry
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "word")] public string Word;
    }

    [DataContract, Preserve]
    public class ProfanityDictionaryEntryWithChildren : ProfanityDictionaryEntry
    {
        [DataMember(Name = "falseNegatives")] public string[] FalseNegatives;
        [DataMember(Name = "falsePositives")] public string[] FalsePositives;
    }

    [DataContract, Preserve]
    public class QueryProfanityWordsResponse
    {
        [DataMember(Name = "data")] public ProfanityDictionaryEntryWithChildren[] Data;
        [DataMember(Name = "pagination")] public ProfanityFilterPagination Pagination;
    }

    [DataContract, Preserve]
    public class CreateProfanityWordRequest
    {
        [DataMember(Name = "falseNegatives")] public string[] FalseNegatives;
        [DataMember(Name = "falsePositives")] public string[] FalsePositives;
        [DataMember(Name = "word")] public string Word;
    }

    [DataContract, Preserve]
    public class BulkCreateProfanityWordRequest
    {
        [DataMember(Name = "dictionaries")] public CreateProfanityWordRequest[] Dictionaries;
    }

    [DataContract, Preserve]
    public class ProfanityWordGroup
    {
        [DataMember(Name = "count")] public long Count;
        [DataMember(Name = "name")] public string Name;
    }

    [DataContract, Preserve]
    public class ProfanityWordGroupResponse
    {
        [DataMember(Name = "data")] public ProfanityWordGroup[] Data;
        [DataMember(Name = "pagination")] public ProfanityFilterPagination Pagination;
    }
}