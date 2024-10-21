// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
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
        [DataMember(Name = "comply")] public bool Comply;
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
        [DataMember(Name = "isHiddenPublic")] public bool IsHiddenPublic;
    }

    [DataContract, Preserve]
    public class BasePolicy
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
        [DataMember(Name = "readableId")] public string ReadableId;
        [DataMember(Name = "policyName")] public string PolicyName;
        [DataMember(Name = "countryCode")] public string CountryCode;
        [DataMember(Name = "countryGroupCode")] public string CountryGroupCode;
        [DataMember(Name = "shouldNotifyOnUpdate")] public bool ShouldNotifyOnUpdate;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "isMandatory")] public bool IsMandatory;
        [DataMember(Name = "isDefaultOpted")] public bool IsDefaultOpted;
        [DataMember(Name = "isDefaultSelection")] public bool IsDefaultSelection;
    }

    [DataContract, Preserve]
    public class GetBasePolicyResponse
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "policies")] public BasePolicy[] Policies;
        [DataMember(Name = "basePolicyName")] public string BasePolicyName;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "isHidden")] public bool IsHidden;
        [DataMember(Name = "isHiddenPublic")] public bool IsHiddenPublic;
        [DataMember(Name = "affectedClientIds")] public string[] AffectedClientIds;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "policyTypeName")] public string PolicyTypeName;
        [DataMember(Name = "policyTypeId")] public string PolicyTypeId;
    }

    [DataContract, Preserve]
    public class BasePolicyOperationRequestBase
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "basePolicyName")] public string BasePolicyName;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "affectedCountries")] public string[] AffectedCountries;
        [DataMember(Name = "isHidden")] public bool IsHidden;
        [DataMember(Name = "isHiddenPublic")] public bool IsHiddenPublic;
        [DataMember(Name = "affectedClientIds")] public string[] AffectedClientIds;
        [DataMember(Name = "tags")] public string[] Tags;
    }

    [DataContract, Preserve]
    public class CreateBasePolicyRequest : BasePolicyOperationRequestBase
    {
        [DataMember(Name = "typeId")] public string TypeId;
    }

    [DataContract, Preserve]
    public class UpdateBasePolicyRequest : BasePolicyOperationRequestBase
    {
    }

    [DataContract, Preserve]
    public class BasePolicyOperationResponseBase
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "createdAt")] public DateTime CreatedAt;
        [DataMember(Name = "updatedAt")] public DateTime UpdatedAt;
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember(Name = "policyId")] public string PolicyId;
        [DataMember(Name = "typeId")] public string TypeId;
        [DataMember(Name = "globalPolicyName")] public string GlobalPolicyName;
        [DataMember(Name = "description")] public string Description;
        [DataMember(Name = "affectedCountries")] public string[] AffectedCountries;
        [DataMember(Name = "affectedClientIds")] public string[] AffectedClientIds;
        [DataMember(Name = "tags")] public string[] Tags;
        [DataMember(Name = "isHidden")] public bool IsHidden;
        [DataMember(Name = "isHiddenPublic")] public bool IsHiddenPublic;
    }

    [DataContract, Preserve]
    public class CreateBasePolicyResponse : BasePolicyOperationResponseBase
    {
    }
    
    [DataContract, Preserve]
    public class UpdateBasePolicyResponse : BasePolicyOperationResponseBase
    {
    }

    [DataContract, Preserve]
    internal class GetPolicyTypeIdResponse
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "policyTypeName")] public string PolicyTypeName;
    }
}