// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    public enum AccelByteMessagingTopic
    {
        None,
        LobbyConnected,
        QosRegionLatenciesUpdated,
        NotificationSenderLobby,
        NotificationBufferSent,
        MatchmakingStarted,
        MatchFoundOnPoll,
        MatchFoundOnGetSessionDetails,
        OnCreatedPartyNotification,
        OnJoinedPartyNotification,
        OnLeftPartyNotification,
        OnJoinedGameSessionNotification,
        OnLeftGameSessionNotification,
    }

    [DataContract, Preserve]
    public struct MessagingSystemMessage
    {
        [DataMember(Name = "topic")] public AccelByteMessagingTopic Topic;
        [DataMember(Name = "payload")] public string Payload;
    }
    
    public struct AccelByteMessage
    {
        public AccelByteMessagingTopic Topic;
        public string Message;
    }
}