// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;

namespace AccelByte.Utils
{
    internal class AccelByteServiceTracker
    {
        internal System.Action<ServiceLog, IDebugger> OnNewRequestSentEvent, OnNewResponseReceivedEvent, OnSendingWebsocketRequestEvent, OnReceivingWebsocketNotificationEvent;

        public void OnNewWebRequestSchedulerCreated(WebRequestScheduler newScheduler)
        {
            newScheduler.PreHttpRequest += (webRequest, headers, payload, logger) =>
            {
                bool enhancedLoggingEnabled = logger != null ? logger.IsEnhancedLoggingEnabled() : false;
                if (enhancedLoggingEnabled && !string.IsNullOrEmpty(webRequest.RequestId))
                {
                    var requestData = new ServiceRequestData()
                    {
                        Verb = webRequest.method.ToUpper(),
                        Url = webRequest.url,
                        Header = headers
                    };
                    if (payload != null)
                    {
                        requestData.Payload = System.Text.Encoding.UTF8.GetString(payload);
                    }
                    
                    var requestLog = new ServiceLog()
                    {
                        MessageId = $"{webRequest.RequestId}",
                        Timestamp = webRequest.SentTimestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture),
                        Type = (int) DataType.Request,
                        Direction = (int) DataDirection.Sending,
                        Data = requestData
                    };
                    OnNewRequestSentEvent?.Invoke(requestLog, logger);
                }
            };
            
            newScheduler.PostHttpRequest += (webRequest, logger) =>
            {
                bool enhancedLoggingEnabled = logger != null ? logger.IsEnhancedLoggingEnabled() : false;
                if (enhancedLoggingEnabled && !string.IsNullOrEmpty(webRequest.RequestId))
                {
                    var requestData = new ServiceResponseData()
                    {
                        Verb = webRequest.method.ToUpper(),
                        Url = webRequest.url,
                        Header = webRequest.GetResponseHeaders(),
                        Status = webRequest.responseCode,
                    };
                    if (webRequest.downloadHandler.data != null)
                    {
                        requestData.Payload = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                    }

                    string responseContentType = webRequest.GetResponseHeader(AccelByteWebRequest.ResponseContentTypeHeader);
                    if (!string.IsNullOrEmpty(responseContentType))
                    {
                        requestData.ContentType = responseContentType;
                    }
                    
                    var requestLog = new ServiceLog()
                    {
                        MessageId = $"{webRequest.RequestId}",
                        Timestamp = webRequest.ResponseTimestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture),
                        Type = (int) DataType.Request,
                        Direction = (int) DataDirection.Receiving,
                        Data = requestData
                    };
                    OnNewResponseReceivedEvent?.Invoke(requestLog, logger);
                }
            };
        }

        public void OnSendingWebsocketRequest(string message, IDebugger logger)
        {
            bool enhancedLoggingEnabled = logger != null ? logger.IsEnhancedLoggingEnabled() : false;
            if (enhancedLoggingEnabled)
            {
                var requestData = new WebsocketData()
                {
                    Payload = message
                };
                    
                var requestLog = new ServiceLog()
                {
                    MessageId = $"{Guid.NewGuid().ToString()}",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture),
                    Type = (int) DataType.WebsocketNotification,
                    Direction = (int) DataDirection.Sending,
                    Data = requestData
                };
                OnSendingWebsocketRequestEvent?.Invoke(requestLog, logger);
            }
        }
        
        public void OnReceivingWebsocketNotification(string message, IDebugger logger)
        {
            bool enhancedLoggingEnabled = logger != null ? logger.IsEnhancedLoggingEnabled() : false;
            if (enhancedLoggingEnabled)
            {
                var requestData = new WebsocketData()
                {
                    Payload = message
                };
                    
                var requestLog = new ServiceLog()
                {
                    MessageId = $"{Guid.NewGuid().ToString()}",
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", System.Globalization.CultureInfo.InvariantCulture),
                    Type = (int) DataType.WebsocketNotification,
                    Direction = (int) DataDirection.Receiving,
                    Data = requestData
                };
                OnReceivingWebsocketNotificationEvent?.Invoke(requestLog, logger);
            }
        }
    }
}