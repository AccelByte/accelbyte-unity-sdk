// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;

namespace AccelByte.Core
{
    public class AccelByteNotificationSender
    {
        private AccelByteMessagingSystem messagingSystem;

        internal AccelByteNotificationSender(ref AccelByteMessagingSystem inMessagingSystem)
        {
            messagingSystem = inMessagingSystem;
        }

        public void SendLobbyNotification(string message)
        {
            messagingSystem?.SendMessage(AccelByteMessagingTopic.NotificationSenderLobby, message);
        }
    }

    internal class AccelByteNotificationSenderUtility
    {
        public static string ComposeMMv2Notification(string topic, string jsonStringContent, bool isEncoded = false)
        {
            string payloadString = isEncoded ? jsonStringContent.ToBytes().ToBase64() : jsonStringContent;

            string retVal = 
                $"type: messageNotif\n" +
                $"topic: {topic}\n" +
                $"payload: {payloadString}\n" +
                $"sentAt: {DateTime.UtcNow.ToString("o")}"; ;

            return retVal;
        }

        public static string ComposeMMv2Notification(string topic, string jsonStringContent, int sequenceNumber, string sequenceId, bool isEncoded = false)
        {
            string payloadString = isEncoded ? jsonStringContent.ToBytes().ToBase64() : jsonStringContent;

            string retVal =
                $"type: messageNotif\n" +
                $"topic: {topic}\n" +
                $"payload: {payloadString}\n" +
                $"sequenceID: {sequenceId}\n" +
                $"sequenceNumber: {sequenceNumber}\n" +
                $"sentAt: {DateTime.UtcNow.ToString("o")}"; ;

            return retVal;
        }

        public static string ComposeSessionNotification(string topic, string jsonStringContent, bool isEncoded = false)
        {
            string payloadString = isEncoded ? jsonStringContent.ToBytes().ToBase64() : jsonStringContent;

            string retVal =
                $"type: messageSessionNotif\n" +
                $"topic: {topic}\n" +
                $"payload: {payloadString}\n" +
                $"sentAt: {DateTime.UtcNow.ToString("o")}";

            return retVal;
        }

        public static string ComposeSessionNotification(string topic, string jsonStringContent, int sequenceNumber, string sequenceId, bool isEncoded = false)
        {
            string payloadString = isEncoded ? jsonStringContent.ToBytes().ToBase64() : jsonStringContent;

            string retVal =
                $"type: messageSessionNotif\n" +
                $"topic: {topic}\n" +
                $"payload: {payloadString}\n" +
                $"sequenceID: {sequenceId}\n" +
                $"sequenceNumber: {sequenceNumber}\n" +
                $"sentAt: {DateTime.UtcNow.ToString("o")}";

            return retVal;
        }

        public static string ComposeConnectedNotification(string lobbySessionId, string loginType, int reconnectFromCode)
        {
            string retVal =
                $"type: connectNotif\n" +
                $"loginType: {loginType}\n" +
                $"reconnectFromCode: {reconnectFromCode}\n" +
                $"lobbySessionID: {lobbySessionId}\n" +
                $"sentAt: {DateTime.UtcNow.ToString("o")}"; ;

            return retVal;
        }
    }
}