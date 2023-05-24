// Copyright (c) 2018 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public class ServiceError
    {
        [DataMember] public long code;
        [DataMember] public long errorCode;
        [DataMember] public int numericErrorCode;
        [DataMember] public string errorMessage;
        [DataMember] public string message;
        [DataMember] public object messageVariables;        
        [DataMember] public string error;
        [DataMember] public string error_description;
    }

    [DataContract, Preserve]
    public class UserBan
    {
        [DataMember] public string comment;
        [DataMember] public string endDate;
        [DataMember] public BanReason reason;
    }

    [DataContract, Preserve]
    public class OAuthError
    {
        [DataMember] public string error;
        [DataMember] public string error_description;
        [DataMember] public string error_uri;
        [DataMember] public string default_factor;
        [DataMember] public string[] factors;
        [DataMember] public string mfa_token;
        [DataMember] public string linkingToken;
        [DataMember] public string clientId;
        [DataMember] public string email;
        [DataMember] public object messageVariables { get;set; }
        [DataMember] public UserBan userBan;
    }
}
