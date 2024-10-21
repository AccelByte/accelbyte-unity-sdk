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
        private Dictionary<AccelByteMessagingTopic, Action<AccelByteMessage>> subscribersDelegateMapV2 = 
            new Dictionary<AccelByteMessagingTopic, Action<AccelByteMessage>>();

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
        
        public void SubscribeToTopicV2(AccelByteMessagingTopic topic, Action<AccelByteMessage> callback)
        {
            if (topic == AccelByteMessagingTopic.None)
            {
                logger?.LogWarning("[MessagingSystem] Unable to subscribe, topic is empty.");
                return;
            }

            if (!subscribersDelegateMapV2.ContainsKey(topic))
            {
                subscribersDelegateMapV2.Add(topic, default);
            }

            subscribersDelegateMapV2[topic] += callback;
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
        
        public void UnsubscribeToTopicV2(AccelByteMessagingTopic topic, Action<AccelByteMessage> callback)
        {
            if (topic == AccelByteMessagingTopic.None)
            {
                logger?.LogWarning("[MessagingSystem] Unable to unsubscribe, topic is empty.");
                return;
            }

            if (subscribersDelegateMapV2.ContainsKey(topic))
            {
                subscribersDelegateMapV2[topic] -= callback;
                totalSubscribers--;
            }
        }

        public void UnsubscribeAll()
        {
            subscribersDelegateMap.Clear();
            subscribersDelegateMapV2.Clear();
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

                if (subscribersDelegateMapV2.ContainsKey(topic))
                {
                    AccelByteMessage newMessage = new AccelByteMessage() { Topic = topic, Message = string.Empty };
                    subscribersDelegateMapV2[topic]?.Invoke(newMessage);
                }
                return;
            }

            var message = new MessagingSystemMessage();
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
                if (subscribersDelegateMapV2.ContainsKey(topic))
                {
                    AccelByteMessage newMessage = new AccelByteMessage() { Topic = topic, Message = payloadString };
                    subscribersDelegateMapV2[topic]?.Invoke(newMessage);
                }
                return;
            }

            var message = new MessagingSystemMessage();
            message.Topic = topic;
            message.Payload = payloadString;

            messages.Enqueue(message);
        }

        private void PollMessages()
        {
            while (messages.Count > 0)
            {
                try
                {
                    var message = messages.Dequeue();

                    if (subscribersDelegateMap.ContainsKey(message.Topic))
                    {
                        subscribersDelegateMap[message.Topic]?.Invoke(message.Payload);
                    }

                    if (subscribersDelegateMapV2.ContainsKey(message.Topic))
                    {
                        AccelByteMessage newMessage =
                            new AccelByteMessage() { Topic = message.Topic, Message = message.Payload };
                        subscribersDelegateMapV2[message.Topic]?.Invoke(newMessage);
                    }
                }
                catch (System.Exception)
                {
                    
                }
            }
        }
    }
}