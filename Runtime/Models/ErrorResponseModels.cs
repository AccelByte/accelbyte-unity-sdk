// Copyright (c) 2018 - 2024 AccelByte Inc. All Rights Reserved.
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
        [DataMember(Name = "devStackTrace")] public string DevStackTrace;
        [DataMember(Name = "requiredPermission")] public ServiceErrorPermission RequiredPermission;
        [DataMember(Name = "name")] public string Name;
        [DataMember(Name = "attributes")] public System.Collections.Generic.Dictionary<string,string> Attributes;
    }
    
    [DataContract, Preserve]
    public class ServiceErrorPermission
    {
        [DataMember(Name = "requiredPermission")] public string RequiredPermission;
        [DataMember(Name = "action")] public float Action;
    }

    [DataContract, Preserve]
    public class UserBan
    {
        [DataMember] public string comment;
        [DataMember] public string endDate;
        [DataMember] public BanReason reason;
    }

    [DataContract, Preserve]
    public class LoginQueueTicketErrorAction
    {
        [DataMember(Name = "action")] public string Action;
        [DataMember(Name = "href")] public string Href;
    }
    
    [DataContract, Preserve]
    public class LoginQueueTicketErrorResponse
    {
        [DataMember(Name = "cancel")] public LoginQueueTicketErrorAction Cancel;
        [DataMember(Name = "estimatedWaitingTimeInSeconds")] public double EstimatedWaitingTimeInSeconds;
        [DataMember(Name = "playerPollingTimeInSeconds")] public double PlayerPollingTimeInSeconds;
        [DataMember(Name = "position")] public double Position;
        [DataMember(Name = "reconnectExpiredAt")] public double ReconnectExpiredAt;
        [DataMember(Name = "refresh")] public LoginQueueTicketErrorAction Refresh;
        [DataMember(Name = "ticket")] public string Ticket;
    }

    [DataContract, Preserve]
    public class OAuthError
    {
        [DataMember] public string clientId;
        [DataMember] public string default_factor;
        [DataMember] public string email;
        [DataMember] public string error;
        [DataMember] public string error_description;
        [DataMember] public string error_uri;
        [DataMember] public string[] factors;
        [DataMember] public string linkingToken;
        [DataMember(Name = "login_queue_ticket")] public LoginQueueTicketErrorResponse LoginQueueTicket;
        [DataMember] public object messageVariables { get;set; }
        [DataMember] public string mfa_token;
        [DataMember(Name = "platformId")] public string PlatformId;
        [DataMember(Name = "remainingBackupCodeCount")] public double RemainingBackupCodeCount;
        [DataMember] public UserBan userBan;
    }
}
