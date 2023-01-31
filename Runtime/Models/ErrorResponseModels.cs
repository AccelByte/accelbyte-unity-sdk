using System;
using System.Runtime.Serialization;

namespace AccelByte.Models
{
    [DataContract]
    public class ServiceError
    {
        [DataMember] public long code { get; set; }
        [DataMember] public long errorCode { get; set; }
        [DataMember] public int numericErrorCode { get; set; }
        [DataMember] public string errorMessage { get; set; }
        [DataMember] public string message { get; set; }
        [DataMember] public object messageVariables { get; set; }        
        [DataMember] public string error { get; set; }
        [DataMember] public string error_description { get; set; }
    }

    [DataContract]
    public class UserBan
    {
        [DataMember] public string comment { get; set; }
        [DataMember] public string endDate { get; set; }
        [DataMember] public BanReason reason { get; set; }
    }

    [DataContract]
    public class OAuthError
    {
        [DataMember] public string error { get; set; }
        [DataMember] public string error_description { get; set; }
        [DataMember] public string error_uri { get; set; }
        [DataMember] public string default_factor { get; set; }
        [DataMember] public string[] factors { get; set; }
        [DataMember] public string mfa_token { get; set; }
        [DataMember] public string linkingToken { get; set; }
        [DataMember] public string clientId { get; set; }
        [DataMember] public string email { get; set; }
        [DataMember] public object messageVariables { get;set; }
        [DataMember] public UserBan userBan { get; set; }
    }
}
