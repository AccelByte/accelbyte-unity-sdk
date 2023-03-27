// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Runtime.Serialization;
using AccelByte.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
        eventNewSystemMessage,
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
        sendChatResponse,
        actionQuerySystemMessage,
        actionUpdateSystemMessages,
        actionDeleteSystemMessages,
        actionGetSystemMessageStats
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

    [DataContract]
    public class SystemMessageNotifMessage
    {
        [DataMember(Name = "title")] public string Title;
        [DataMember(Name = "body")] public string Body;
        [DataMember(Name = "gift")] public JObject Gift;
    }

    [DataContract]
    public class QuerySystemMessageResponseItem
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "category")] public string Category;
        [DataMember(Name = "message")] public string Message;

        [DataMember(Name = "createdAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updatedAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "expiredAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ExpiredAt { get; set; }

        [DataMember(Name = "readAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ReadAt { get; set; }

        /// <summary>
        /// Parse Message to SystemMessageNotifMessage
        /// </summary>
        /// <param name="outFormattedMessage">Message in SystemMessageNotifMessage</param>
        /// <returns>true when parsing success</returns>
        public SystemMessageNotifMessage GetSystemMessageData()
        {
            return JsonConvert.DeserializeObject<SystemMessageNotifMessage>(Message);
        }
    }

    [DataContract]
    public class SystemMessageNotif
    {
        [DataMember(Name = "messageId")] public string MessageId;
        [DataMember(Name = "category")] public string Category;
        [DataMember(Name = "message")] public string Message;

        [DataMember(Name = "createdAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "expiredAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ExpiredAt { get; set; }

        /// <summary>
        /// Parse Message to SystemMessageNotifMessage
        /// </summary>
        /// <param name="outFormattedMessage">Message in SystemMessageNotifMessage</param>
        /// <returns>true when parsing success</returns>
        public SystemMessageNotifMessage GetSystemMessageData()
        {
            return JsonConvert.DeserializeObject<SystemMessageNotifMessage>(Message);
        }
    }

    /// <summary>
    /// Optional parameter for query system messages
    /// </summary>
    [DataContract]
    public class QuerySystemMessageRequest
    {
        /// <summary>
        /// Query only unread messages
        /// </summary>
        [DataMember(Name = "unreadOnly")]
        public bool UnreadOnly = false;

        /// <summary>
        /// Query messages from specified date and time
        /// </summary>
        [DataMember(Name = "startCreatedAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime StartCreatedAt { get; set; } = default;

        /// <summary>
        /// Query messages up to specified date and time
        /// </summary>
        [DataMember(Name = "endCreatedAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime EndCreatedAt { get; set; } = default;
        
        /// <summary>
        /// Category of system message
        /// </summary>
        [DataMember(Name = "category")] public string Category;
        
        /// <summary>
        /// Query only unread messages
        /// </summary>
        [DataMember(Name = "offset")] public int Offset { get; set; } = 0;

        /// <summary>
        /// number of maximum messages to query
        /// </summary>
        [DataMember(Name = "limit")] public int Limit { get; set; } = 20;
    }

    [DataContract]
    public class QuerySystemMessagesResponse
    {
        [DataMember(Name = "data")] public List<QuerySystemMessageResponseItem> Data;
    }

    [DataContract]
    public enum OptionalBoolean
    {
        [EnumMember(Value = "NONE")] None = 0,
        [EnumMember(Value = "YES")] Yes = 1,
        [EnumMember(Value = "NO")] No = 2
    }

    [DataContract]
    public class ActionUpdateSystemMessage
    {
        [DataMember(Name = "id")] public string Id { get; set; }
        [DataMember(Name = "read")] public OptionalBoolean Read { get; set; }
        [DataMember(Name = "keep")] public OptionalBoolean Keep { get; set; }
    }

    [DataContract]
    public class UpdateSystemMessagesRequest
    {
        [DataMember(Name = "data")] public HashSet<ActionUpdateSystemMessage> Data;
    }

    [DataContract]
    public class UpdateSystemMessagesResponse
    {
        [DataMember(Name = "processed"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Processed { get; set; }
    }

    [DataContract]
    public class DeleteSystemMessageRequest
    {
        [DataMember(Name = "messageIds")] public HashSet<string> MessageIds;
    }

    [DataContract]
    public class DeleteSystemMessagesResponse
    {
        [DataMember(Name = "processed"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Processed { get; set; }
    }

    [DataContract]
    public class GetSystemMessageStatsRequest
    {
        
    }

    [DataContract]
    public class GetSystemMessageStatsResponse
    {
        [DataMember(Name = "oldestUnread"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime OldestUnread;
        
        [DataMember(Name = "unread")] public Int32 Unread;
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