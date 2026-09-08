// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{ 
    [DataContract, Preserve]
    public class SubmitAccountDeletionResponse
    {
        [DataMember] public string Namespace;
        [DataMember(Name = "UserID")] public string UserId;
    }

    [DataContract, Preserve]
    public class AccountDeletionStatusResponse
    {
        [DataMember] public string DeletionDate; // can be empty string, since the format dd MMMM yyyy  
        [DataMember] public bool DeletionStatus;
        [DataMember] public string DisplayName;
        [DataMember] public DateTime ExecutionDate;
        [DataMember] public string Status;
        [DataMember(Name = "UserID")] public string UserId;
    }

    [DataContract, Preserve]
    public class SubmitPersonalDataRequestResponse
    {
        [DataMember] public string Namespace;
        [DataMember] public DateTime RequestDate;
        [DataMember(Name = "UserID")] public string UserId;
    }

    [DataContract, Preserve]
    public class PersonalDataRequest
    {
        [DataMember] public DateTime DataExpirationDate;
        [DataMember] public DateTime RequestDate;
        [DataMember] public string Status; // One of: Pending, In-Progress, Completed, Canceled, Failed, Expired
    }

    [DataContract, Preserve]
    public class PersonalDataRequestsResponse
    {
        [DataMember] public PersonalDataRequest[] Data;
        [DataMember] public Paging Paging;
    }

    public class SubmitMyPersonalDataRequestOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Email address that receives the notification and the download link once the request
        /// completes. There is no other way to retrieve the completed download link for the
        /// headless flow, so a request submitted without an email has no retrieval path.
        /// </summary>
        public string Email;

        /// <summary>
        /// Language tag for the notification email.
        /// </summary>
        public string LanguageTag;
    }

    public class GetMyPersonalDataRequestsOptionalParameters : OptionalParametersBase
    {
        /// <summary>
        /// Amount of entries to offset / traverse on the pagination system.
        /// </summary>
        public int? Offset = 0;

        /// <summary>
        /// Amount of entries to display per page on the pagination system.
        /// </summary>
        public int? Limit = 20;
    }

    public class CancelMyPersonalDataRequestOptionalParameters : OptionalParametersBase
    {
    }
}