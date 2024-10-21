// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Runtime.Serialization;
using AccelByte.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine.Scripting;

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
        actionGetUserConfig,
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
        actionSetUserConfig,
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
        actionGetSystemMessageStats,
        eventUserMuted,
        eventUserUnmuted
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ChatTopicType
    {
        empty,
        personal,
        group
    }

    [DataContract, Preserve]
    public class ChatWsMessage
    {
        [DataMember] public string jsonrpc;
        [DataMember] public ChatMessageMethod method;
        [DataMember] public string id;
        [DataMember] public Error error;
    }

    [DataContract, Preserve]
    public class ChatWsMessage<T> : ChatWsMessage
    {
        [DataMember, JsonProperty(PropertyName = "params", DefaultValueHandling = DefaultValueHandling.Populate)]
        public T params_;
    }

    [DataContract, Preserve]
    public class ChatWsMessageResponse<T> : ChatWsMessage
    {
        [DataMember]
        public T result;
    }

    [DataContract, Preserve]
    public class RefreshTokenRequest
    {
        [DataMember] public string token;
    }

    [DataContract, Preserve]
    public class ChatActionTopicRequest
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string topicId;
        [DataMember] public string name;
        [DataMember] public string type;
        [DataMember] public bool isJoinable;
        [DataMember] public bool isChannel;
        [DataMember] public int shardLimit;
        [DataMember] public string[] members;
        [DataMember] public string[] admins;
    }

    [DataContract, Preserve]
    public class ChatActionTopicResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime processed;
        [DataMember] public string topicId;
    }

    [DataContract, Preserve]
    public class SendChatRequest
    {
        [DataMember] public string topicId;
        [DataMember] public string message;
    }

    [DataContract, Preserve]
    public class SendChatResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime processed;
        [DataMember] public string topicId;
        [DataMember] public string chatId;
    }

    [DataContract, Preserve]
    public class QueryTopicByIdRequest
    {
        [DataMember] public string topicId;
    }

    [DataContract, Preserve]
    public class QueryTopicRequest
    {
        [DataMember(Name = "namespace")] public string Namespace;
        [DataMember] public string keyword;
        [DataMember] public int offset;
        [DataMember] public int limit;
    }

    public enum TopicType
    {
        NONE = 0,
        PERSONAL = 1,
        GROUP = 2
    };

    [DataContract, Preserve]
    public class QueryTopicResponseData
    {
        [DataMember] public string topicId;
        [DataMember] public TopicType type;
        [DataMember] public string name;
        [DataMember] public string[] members;

        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime updatedAt;
    }

    [DataContract, Preserve]
    public class QueryTopicResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime processed;

        [DataMember] public QueryTopicResponseData[] data;
    }

    [DataContract, Preserve]
    public class QueryTopicByIdResponseData : QueryTopicResponseData
    {
        [DataMember] public bool isChannel;
    }

    [DataContract, Preserve]
    public class QueryTopicByIdResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime processed;

        [DataMember] public QueryTopicByIdResponseData[] data;
    }

    [DataContract, Preserve]
    public class QueryChatRequest
    {
        [DataMember] public string topicId;
        [DataMember] public int limit;
        [DataMember] public DateTime lastChatCreatedAt;
    }

    [DataContract, Preserve]
    public class QueryChatResponseData
    {
        [DataMember] public string chatId;
        [DataMember] public string message;
        [DataMember] public string topicId;
        [DataMember] public string from;
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime createdAt;
    }

    [DataContract, Preserve]
    public class QueryChatResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime processed;

        [DataMember] public QueryChatResponseData[] data;
    }

    [DataContract, Preserve]
    public class BlockUnblockRequest
    {
        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class BlockUnblockResponse
    {
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime processed;

        [DataMember] public string userId;
    }

    [DataContract, Preserve]
    public class SystemMessageNotifMessage
    {
        [DataMember(Name = "title")] public string Title;
        [DataMember(Name = "body")] public string Body;
        [DataMember(Name = "gift")] public JObject Gift;
    }

    [DataContract, Preserve]
    public class QuerySystemMessageResponseItem
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "category")] public string Category;
        [DataMember(Name = "message")] public string Message;

        [DataMember(Name = "createdAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt;

        [DataMember(Name = "updatedAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime UpdatedAt;

        [DataMember(Name = "expiredAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ExpiredAt;

        [DataMember(Name = "readAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ReadAt;

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

    [DataContract, Preserve]
    public class SystemMessageNotif
    {
        [DataMember(Name = "messageId")] public string MessageId;
        [DataMember(Name = "category")] public string Category;
        [DataMember(Name = "message")] public string Message;

        [DataMember(Name = "createdAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt;

        [DataMember(Name = "expiredAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ExpiredAt;

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
    [DataContract, Preserve]
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
        public DateTime StartCreatedAt = default;

        /// <summary>
        /// Query messages up to specified date and time
        /// </summary>
        [DataMember(Name = "endCreatedAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime EndCreatedAt = default;

        /// <summary>
        /// Category of system message
        /// </summary>
        [DataMember(Name = "category")] public string Category;

        /// <summary>
        /// Query only unread messages
        /// </summary>
        [DataMember(Name = "offset")] public int Offset = 0;

        /// <summary>
        /// number of maximum messages to query
        /// </summary>
        [DataMember(Name = "limit")] public int Limit = 20;
    }

    [DataContract, Preserve]
    public class QuerySystemMessagesResponse
    {
        [DataMember(Name = "data")] public List<QuerySystemMessageResponseItem> Data;
    }

    [DataContract, Preserve]
    public enum OptionalBoolean
    {
        [EnumMember(Value = "NONE")] None = 0,
        [EnumMember(Value = "YES")] Yes = 1,
        [EnumMember(Value = "NO")] No = 2
    }

    [DataContract, Preserve]
    public class ActionUpdateSystemMessage
    {
        [DataMember(Name = "id")] public string Id;
        [DataMember(Name = "read")] public OptionalBoolean Read;
        [DataMember(Name = "keep")] public OptionalBoolean Keep;
    }

    [DataContract, Preserve]
    public class UpdateSystemMessagesRequest
    {
        [DataMember(Name = "data")] public HashSet<ActionUpdateSystemMessage> Data;
    }

    [DataContract, Preserve]
    public class UpdateSystemMessagesResponse
    {
        [DataMember(Name = "processed"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Processed;
    }

    [DataContract, Preserve]
    public class DeleteSystemMessageRequest
    {
        [DataMember(Name = "messageIds")] public HashSet<string> MessageIds;
    }

    [DataContract, Preserve]
    public class DeleteSystemMessagesResponse
    {
        [DataMember(Name = "processed"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Processed;
    }

    [DataContract, Preserve]
    public class GetSystemMessageStatsRequest
    {

    }

    [DataContract, Preserve]
    public class GetSystemMessageStatsResponse
    {
        [DataMember(Name = "oldestUnread"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime OldestUnread;

        [DataMember(Name = "unread")] public Int32 Unread;
    }

    #region event models
    [DataContract, Preserve]
    public class EventConnected
    {
        [DataMember] public string sessionId;
    }

    [DataContract, Preserve]
    public class EventTopicUpdated
    {
        [DataMember] public string name;
        [DataMember] public string topicId;
        [DataMember] public string senderId;
    }

    [DataContract, Preserve]
    public class EventAddRemoveFromTopic
    {
        [DataMember] public ChatTopicType type;
        [DataMember] public string name;
        [DataMember] public string topicId;
        [DataMember] public string senderId;
        [DataMember] public bool isChannel;
    }

    [DataContract, Preserve]
    public class EventNewChat
    {
        [DataMember] public string chatId;
        [DataMember] public string message;
        [DataMember] public string topicId;
        [DataMember] public string from;
        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime createdAt;
    }


    public class EventBanUnban
    {
        [DataMember] public bool enable;
        [DataMember] public string userId;
        [DataMember(Name = "namespace")] public string namespace_;
        [DataMember] public string reason;
        [DataMember] public string ban;

        [DataMember, JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime endDate;
    }
    #endregion

    #region Group Chat As Moderator

    [DataContract, Preserve]
    public class MuteGroupChatRequest
    {
        [DataMember(Name = "userId")]
        public string UserId;

        [DataMember(Name = "duration")]
        public int Duration;
    }

    [DataContract, Preserve]
    public class UnmuteGroupChatRequest
    {
        [DataMember(Name = "userId")]
        public string UserId;
    }

    [DataContract, Preserve]
    public class SnapshotMessage
    {
        [DataMember(Name = "chatId")]
        public string ChatId;

        [DataMember(Name = "createdAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt;

        [DataMember(Name = "message")]
        public string Message;

        [DataMember(Name = "senderId")]
        public string SenderId;
    }

    [DataContract, Preserve]
    public class ChatSnapshotResponse
    {
        [DataMember(Name = "chatId")]
        public string ChatId;

        [DataMember(Name = "createdAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt;

        [DataMember(Name = "joinedTopics")]
        public List<string> JoinedTopics;

        [DataMember(Name = "messages")]
        public List<SnapshotMessage> Messages;

        [DataMember(Name = "namespace")]
        public string Namespace;

        [DataMember(Name = "senderId")]
        public string SenderId;

        [DataMember(Name = "ticketId")]
        public string TicketId;

        [DataMember(Name = "topicId")]
        public string TopicId;
    }

    [DataContract, Preserve]
    public class BanGroupChatRequest
    {
        [DataMember(Name = "userIDs")]
        public List<string> UserIds;
    }

    [DataContract, Preserve]
    public class BanGroupChatResponse
    {
        [DataMember(Name = "userIDs")]
        public List<string> UserIds;
    }

    [DataContract, Preserve]
    public class UnbanGroupChatRequest
    {
        [DataMember(Name = "userIDs")]
        public List<string> UserIds;
    }

    [DataContract, Preserve]
    public class UnbanGroupChatResponse
    {
        [DataMember(Name = "userIDs")]
        public List<string> UserIds;
    }

    #endregion

    [DataContract, Preserve]
    public class ChatMutedNotif
    {
        [DataMember(Name = "topicId")]
        public string TopicId;

        [DataMember(Name = "remainingTime")]
        public int RemainingTime;

        [DataMember(Name = "expiredAt"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ExpiredAt;
    }

    [DataContract, Preserve]
    public class ChatUnmutedNotif
    {
        [DataMember(Name = "topicId")]
        public string TopicId;
    }

    [DataContract, Preserve]
    public class UserChatConfiguration
    {
        [DataMember(Name = "profanityDisabled")]  public bool ProfanityDisabled;
    }

    [DataContract, Preserve]
    public class GetUserChatConfigurationResponse
    {
        [DataMember(Name = "config")] public UserChatConfiguration Config;
        [DataMember(Name = "processed"), JsonConverter(typeof(UnixDateTimeConverter))] public DateTime Processed;
    }

    [DataContract, Preserve]
    public class SetUserChatConfigurationRequest
    {
        [DataMember(Name = "config")] public UserChatConfiguration Config;
    }

    [DataContract, Preserve]
    public class SetUserChatConfigurationResponse
    {
        [DataMember(Name = "processed"), JsonConverter(typeof(UnixDateTimeConverter))] public DateTime Processed;
    }
}