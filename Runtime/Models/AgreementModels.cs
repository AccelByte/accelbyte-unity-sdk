// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;

namespace AccelByte.Models
{
    #region enum
    public enum AgreementPolicyType
    {
        EMPTY,
        LEGAL_DOCUMENT_TYPE,
        MARKETING_PREFERENCE_TYPE
    }
    #endregion enum

    [DataContract]
    public class IneligibleUser
    {
        [DataMember] public string emailAddress { get; set; }
        [DataMember] public string country { get; set; }
        [DataMember] public bool eligible { get; set; } = true;
        [DataMember] public bool deletionStatus { get; set; }
    }

    [DataContract]
    public class LocalizedPolicyVersionObject
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string createdAt { get; set; }
        [DataMember] public string updatedAt { get; set; }
        [DataMember] public string localeCode { get; set; }
        [DataMember] public string contentType { get; set; }
        [DataMember] public string attachmentLocation { get; set; }
        [DataMember] public string attachmentChecksum { get; set; }
        [DataMember] public string attachmentVersionIdentifier { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public string publishedDate { get; set; }
        [DataMember] public bool isDefaultSelection { get; set; }
    }

    [DataContract]
    public class PolicyVersionWithLocalizedVersionObject
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string createdAt { get; set; }
        [DataMember] public string updatedAt { get; set; } 
        [DataMember] public string displayVersion { get; set; }
        [DataMember] public string description { get; set; } 
        [DataMember] public string status { get; set; }
        [DataMember] public string publishedDate { get; set; } 
        [DataMember] public LocalizedPolicyVersionObject[] localizedPolicyVersions { get; set; } 
        [DataMember] public bool isCommitted { get; set; }
        [DataMember] public bool isCrucial { get; set; }
        [DataMember] public bool isInEffect { get; set; }
    }
    
    [DataContract]
    public class PublicPolicy
    {
        [DataMember] public string id { get; set; }
        [DataMember] public string createdAt { get; set; }
        [DataMember] public string updatedAt { get; set; }
        [DataMember] public string readableId { get; set; }
        [DataMember] public string policyName { get; set; }
        [DataMember] public string policyType { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string countryCode { get; set; }
        [DataMember] public string countryGroupCode { get; set; }
        [DataMember] public string[] baseUrls  { get; set; }
        [DataMember] public bool shouldNotifyOnUpdate { get; set; }
        [DataMember] public PolicyVersionWithLocalizedVersionObject[] policyVersions { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public bool isMandatory { get; set; }
        [DataMember] public bool isDefaultOpted { get; set; }
        [DataMember] public bool isDefaultSelection { get; set; }
    }
    
    [DataContract]
    public class AcceptAgreementRequest
    {
        [DataMember] public string localizedPolicyVersionId { get; set; }
        [DataMember] public string policyVersionId { get; set; }
        [DataMember] public string policyId { get; set; }
        [DataMember] public bool isAccepted { get; set; }
    }
    
    [DataContract]
    public class AcceptAgreementResponse
    {
        [DataMember] public bool proceed { get; set; }
    }

    [DataContract]
    public class RetrieveUserEligibilitiesResponse
    {
        [DataMember] public string readableId { get; set; }
        [DataMember] public string policyName { get; set; }
        [DataMember] public string policyType { get; set; }
        [DataMember(Name = "namespace")] public string Namespace { get; set; }
        [DataMember] public string countryCode { get; set; }
        [DataMember] public string countryGroupCode { get; set; }
        [DataMember] public string[] baseUrls { get; set; }
        [DataMember] public PolicyVersionWithLocalizedVersionObject[] policyVersions { get; set; }
        [DataMember] public string description { get; set; }
        [DataMember] public string policyId { get; set; }
        [DataMember] public bool isMandatory { get; set; }
        [DataMember] public bool isAccepted { get; set; }
    }
}