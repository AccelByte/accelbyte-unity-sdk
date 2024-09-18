// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using System;
using System.Collections.Generic;

namespace AccelByte.Core
{
    public class AccelByteMessagingSystem
    {
        private AccelByteHeartBeat poller;
        private const int pollingIntervalMs = 500;

        private Queue<MessagingSystemMessage> messages = new Queue<MessagingSystemMessage>();
        internal Queue<MessagingSystemMessage> Messages => messages;

        private Dictionary<AccelByteMessagingTopic, Action<string>> subscribersDelegateMap = 
            new Dictionary<AccelByteMessagingTopic, Action<string>>();

        private int totalSubscribers = 0;
        public int AllSubscribersCount => totalSubscribers;

        private IDebugger logger;

        public AccelByteMessagingSystem(IDebugger logger = null)
        {
            this.logger = logger;
            
            poller = new AccelByteHeartBeat(pollingIntervalMs, logger);
            poller.SetLogger(logger);
            poller.Start();

            poller.OnHeartbeatTrigger += PollMessages;
        }

        ~AccelByteMessagingSystem()
        {
            UnsubscribeAll();

            poller.OnHeartbeatTrigger -= PollMessages;
            poller = null;
        }

        public void SubscribeToTopic(AccelByteMessagingTopic topic, Action<string> callback)
        {
            if (topic == AccelByteMessagingTopic.None)
            {
                logger?.LogWarning("[MessagingSystem] Unable to subscribe, topic is empty.");
                return;
            }

            if (!subscribersDelegateMap.ContainsKey(topic))
            {
                subscribersDelegateMap.Add(topic, default);
            }

            subscribersDelegateMap[topic] += callback;
            totalSubscribers++;
        }

        public void UnsubscribeToTopic(AccelByteMessagingTopic topic, Action<string> callback)
        {
            if (topic == AccelByteMessagingTopic.None)
            {
                logger?.LogWarning("[MessagingSystem] Unable to unsubscribe, topic is empty.");
                return;
            }

            if (subscribersDelegateMap.ContainsKey(topic))
            {
                subscribersDelegateMap[topic] -= callback;
                totalSubscribers--;
            }
        }

        public void UnsubscribeAll()
        {
            subscribersDelegateMap.Clear();
            totalSubscribers = 0;
        }

        public void SendMessage(AccelByteMessagingTopic topic, bool isImmediate = true)
        {
            if (topic == AccelByteMessagingTopic.None)
            {
                return;
            }

            if (isImmediate)
            {
                if (subscribersDelegateMap.ContainsKey(topic))
                {
                    subscribersDelegateMap[topic]?.Invoke(string.Empty);
                }
                return;
            }

            MessagingSystemMessage message = new();
            message.Topic = topic;

            messages.Enqueue(message);
        }

        public void SendMessage<T>(AccelByteMessagingTopic topic, T payload, bool isImmediate = true)
        {
            if (topic == AccelByteMessagingTopic.None)
            {
                return;
            }

            string payloadString = typeof(T) == typeof(string) ? payload as string : payload.ToJsonString();

            if (isImmediate)
            {
                if (subscribersDelegateMap.ContainsKey(topic))
                {
                    subscribersDelegateMap[topic]?.Invoke(payloadString);
                }
                return;
            }

            MessagingSystemMessage message = new();
            message.Topic = topic;
            message.Payload = payloadString;

            messages.Enqueue(message);
        }

        private void PollMessages()
        {
            while (messages.Count > 0)
            {
                if (!messages.TryDequeue(out var message))
                {
                    continue;
                }

                if (subscribersDelegateMap.ContainsKey(message.Topic))
                {
                    subscribersDelegateMap[message.Topic]?.Invoke(message.Payload);
                }
            }
        }
    }
}