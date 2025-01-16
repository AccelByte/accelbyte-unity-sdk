// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Utils
{
    [Preserve, DataContract]
    internal class ServiceLog
    {
        [DataMember(Name = "message_id")] public string MessageId;
        [DataMember(Name = "timestamp")] public string Timestamp;
        [DataMember(Name = "type")] public int Type;
        [DataMember(Name = "direction")] public int Direction;
        [DataMember(Name = "data")] public ServiceData Data;
    }

    [Preserve, DataContract]
    internal abstract class ServiceData
    {
        [DataMember(Name = "verb"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Verb;
        [DataMember(Name = "url"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string Url;
        [DataMember(Name = "header"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public IDictionary<string,string> Header;
        [DataMember(Name = "payload"), JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public object Payload;
    }
    
    [Preserve, DataContract]
    internal class ServiceRequestData : ServiceData
    {
    }

    [Preserve, DataContract]
    internal class ServiceResponseData : ServiceData
    {
        [DataMember(Name = "status")] public long Status;
        [DataMember(Name = "content_type")] public string ContentType = string.Empty;
    }
    
    [Preserve, DataContract]
    internal class WebsocketData : ServiceData
    {
    }

    internal enum DataDirection
    {
        None,
        Sending,
        Receiving
    }
    
    internal enum DataType
    {
        None,
        Request,
        WebsocketNotification
    }
}