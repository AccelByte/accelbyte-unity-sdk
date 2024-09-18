// Copyright (c) 2020 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    #region enum
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum AgreementPolicyType
    {
        EMPTY,
        LEGAL_DOCUMENT_TYPE,
        MARKETING_PREFERENCE_TYPE
    }
    #endregion enum

    [DataContract, Preserve]
    public class IneligibleUser
    {
        [DataMember] public string emailAddress;
        [DataMember] public string country;
        [DataMember] public bool eligible = true;
        [DataMember] public bool deletionStatus;
    }

    [DataContract, Preserve]
    public class LocalizedPolicyVersionObject
    {
        [DataMember] public string id;
        [DataMember] public string createdAt;
        [DataMember] public string updatedAt;
        [DataMember] public string localeCode;
        [DataMember] public string contentType;
        [DataMember] public string attachmentLocation;
        [DataMember] public string attachmentChecksum;
        [DataMember] public string attachmentVersionIdentifier;
        [DataMember] public string description;
        [DataMember] public string status;
        [DataMember] public string publishedDate;
        [DataMember] public bool isDefaultSelection;
    }

    [DataContract, Preserve]
    public class PolicyVersionWithLocalizedVersionObject
    {
        [DataMember] public string id;
        [DataMember] public string createdAt;
        [DataMember] public string updatedAt; 
        [DataMember] public string displayVersion;
        [DataMember] public string description; 
        [DataMember] public string status;
        [DataMember] public string publishedDate; 
        [DataMember] public LocalizedPolicyVersionObject[] localizedPolicyVersions; 
        [DataMember] public bool isCommitted;
        [DataMember] public bool isCrucial;
        [DataMember] public bool isInEffect;
    }
    
    [DataContract, Preserve]
    public class PublicPolicy
    {
        [DataMember] public string id;
        [DataMember] public string createdAt;
        [DataMember] public string updatedAt;
        [DataMember] public string readableId;
        [DataMember] public string policyName;
        [DataMember] public string policyType;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string countryCode;
        [DataMember] public string countryGroupCode;
        [DataMember] public string[] baseUrls ;
        [DataMember] public bool shouldNotifyOnUpdate;
        [DataMember] public PolicyVersionWithLocalizedVersionObject[] policyVersions;
        [DataMember] public string description;
        [DataMember] public bool isMandatory;
        [DataMember] public bool isDefaultOpted;
        [DataMember] public bool isDefaultSelection;
    }
    
    [DataContract, Preserve]
    public class AcceptAgreementRequest
    {
        [DataMember] public string localizedPolicyVersionId;
        [DataMember] public string policyVersionId;
        [DataMember] public string policyId;
        [DataMember] public bool isAccepted = false;
    }

    [DataContract, Preserve]
    public class ChangeAgreementRequest : AcceptAgreementRequest
    {
        [DataMember (Name = "isNeedToSendEventMarketing")] public bool IsNeedToSendEventMarketing;
    }
    
    [DataContract, Preserve]
    public class AcceptAgreementResponse
    {
        [DataMember] public bool proceed;
    }

    [DataContract, Preserve]
    public class RetrieveUserEligibilitiesResponse
    {
        [DataMember] public string readableId;
        [DataMember] public string policyName;
        [DataMember] public string policyType;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string countryCode;
        [DataMember] public string countryGroupCode;
        [DataMember] public string[] baseUrls;
        [DataMember] public PolicyVersionWithLocalizedVersionObject[] policyVersions;
        [DataMember] public string description;
        [DataMember] public string policyId;
        [DataMember] public bool isMandatory;
        [DataMember] public bool isAccepted;
    }
}