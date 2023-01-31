// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;
using AccelByte.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatMessageMethod
    {
        empty,
        eventConnected,
        eventDisconnected,
        eventAddedToTopic,
        eventBanChat,
        eventNewChat,
        eventReadChat,
        eventRemovedFromTopic,
        eventServerShutdown,
        eventTopicBanChat,
        eventTopicDeleted,
        eventTopicUnbanChat,
        eventTopicUpdated,
        eventUnbanChat,
        eventUserBanned,
        actionAddUserToTopic,
        actionAddUserToTopicResponse,
        actionBlockUser,
        actionBlockUserResponse,
        actionCreateTopic,
        actionCreateTopicResponse,
        actionDeleteTopic,
        actionDeleteTopicResponse,
        actionDisconnect,
        actionDisconnectResponse,
        actionJoinTopic,
        actionJoinTopicResponse,
        actionQueryGroupTopic,
        actionQueryGroupTopicResponse,
        actionQueryPersonalTopic,
        actionQueryPersonalTopicResponse,
        actionQueryPublicGroupTopic,
        actionQueryPublicGroupTopicResponse,
        actionQueryTopic,
        actionQueryTopicById,
        actionQueryTopicByIdResponse,
        actionQueryTopicResponse,
        actionQuitTopic,
        actionQuitTopicResponse,
        actionRefreshToken,
        actionRefreshTokenResponse,
        actionRemoveUserFromTopic,
        actionRemoveUserFromTopicResponse,
        actionUnblockUser,
        actionUnblockUserResponse,
        actionUpdateTopic,
        actionUpdateTopicResponse,
        queryChat,
        queryChatResponse,
        readChat,
        readChatResponse,
        sendChat,
        sendChatResponse
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatTopicType
    {
        empty,
        personal,
        group
    }
    
    [DataContract]
    public class ChatWsMessage
    {
        [DataMember] public string jsonrpc { get; set; }
        [DataMember] public ChatMessageMethod method { get; set; }
        [DataMember] public string id { get; set; }
        [DataMember] public Error error { get; set; }
    }
    
    [DataContract]
    public class ChatWsMessage<T> : ChatWsMessage
    {
        [DataMember, JsonProperty("params")]
        public T params_ { get; set; }
    }
    
    [DataContract]
    public class ChatWsMessageResponse<T> : ChatWsMessage
    {
        [DataMember]
        public T result { get; set; }
    }

    [DataContract]
    public class RefreshTokenRequest
    {
        [DataMember] public string token { get; set; }
    }

    [DataContract]
    public class ChatActionTopicRequest
    {
        [DataMember(Name="namespace")] public string Namespace { get; set; }
        [DataMember] public string topicId { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string type { get; set; }
        [DataMember] public bool isJoinable { get; set; }
        [DataMember] public bool isChannel { get; set; }
        [DataMember] public int shardLimit { get; set; }
        [DataMember] public string[] members { get; set; }
        [DataMember] public string[] admins { get; set; }
    }

    [DataContract]
    public class ChatActionTopicResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime processed { get; set; }
        [DataMember] public string topicId { get; set; }
    }

    [DataContract]
    public class SendChatRequest
    {
        [DataMember] public string topicId { get; set; }
        [DataMember] public string message { get; set; }
    }

    [DataContract]
    public class SendChatResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime processed { get; set; }
        [DataMember] public string topicId { get; set; }
        [DataMember] public string chatId { get; set; }
    }

    [DataContract]
    public class QueryTopicByIdRequest
    {
        [DataMember] public string topicId { get; set; }
    }

    [DataContract]
    public class QueryTopicRequest
    {
        [DataMember(Name="namespace")] public string Namespace { get; set; }
        [DataMember] public string keyword { get; set; }
        [DataMember] public int offset { get; set; }
        [DataMember] public int limit { get; set; }
    }
    
    public enum TopicType
    {
        NONE = 0,
        PERSONAL = 1,
        GROUP = 2
    };
    
    [DataContract]
    public class QueryTopicResponseData
    {
        [DataMember] public string topicId { get; set; }
        [DataMember] public TopicType type { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string[] members { get; set; }
        
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime updatedAt { get; set; }
    }

    [DataContract]
    public class QueryTopicResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime processed { get; set; }
        
        [DataMember] public QueryTopicResponseData[] data { get; set; } 
    }

    [DataContract]
    public class QueryTopicByIdResponseData : QueryTopicResponseData
    {
        [DataMember] public bool isChannel { get; set; }
    }
    
    [DataContract]
    public class QueryTopicByIdResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime processed { get; set; }
        
        [DataMember] public QueryTopicByIdResponseData[] data { get; set; } 
    }

    [DataContract]
    public class QueryChatRequest
    {
        [DataMember] public string topicId;
        [DataMember] public int limit;
        [DataMember] public DateTime lastChatCreatedAt;
    }
    
    [DataContract]
    public class QueryChatResponseData
    {
        [DataMember] public string chatId;
        [DataMember] public string message;
        [DataMember] public string topicId;
        [DataMember] public string from;
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime createdAt { get; set; }
    }

    [DataContract]
    public class QueryChatResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime processed { get; set; }
        
        [DataMember] public QueryChatResponseData[] data { get; set; } 
    }

    [DataContract]
    public class BlockUnblockRequest
    {
        [DataMember] public string userId { get; set; }
    }

    [DataContract]
    public class BlockUnblockResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime processed { get; set; }

        [DataMember] public string userId { get; set; }
    }

    #region event models
    [DataContract]
    public class EventConnected
    {
        [DataMember] public string sessionId { get; set; }
    }
    
    [DataContract]
    public class EventTopicUpdated
    {
        [DataMember] public string name { get; set; }
        [DataMember] public string topicId { get; set; }
        [DataMember] public string senderId { get; set; }
    }

    [DataContract]
    public class EventAddRemoveFromTopic
    {
        [DataMember] public ChatTopicType type { get; set; }
        [DataMember] public string name { get; set; }
        [DataMember] public string topicId { get; set; }
        [DataMember] public string senderId { get; set; }
        [DataMember] public bool isChannel { get; set; }
    }

    [DataContract]
    public class EventNewChat
    {
        [DataMember] public string chatId { get; set; }
        [DataMember] public string message { get; set; }
        [DataMember] public string topicId { get; set; }
        [DataMember] public string from { get; set; }
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime createdAt { get; set; }
    }
    
    
    public class EventBanUnban
    {
        [DataMember] public bool enable { get; set; }
        [DataMember] public string userId { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string reason { get; set; }
        [DataMember] public string ban { get; set; }
        
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))] 
        public DateTime endDate { get; set; }
    }
    #endregion
}