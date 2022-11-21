﻿// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

namespace AccelByte.Core
{
    public class Report
    {
        public static void GetFunctionLog(string className, [CallerMemberName] string functName = "")
        {
            string functionCaller = "";
#if DEBUG
            try
            {
                StackFrame callStack = new StackFrame(2, true);
                functionCaller = "File: " + callStack.GetFileName() + ", Line: " + callStack.GetFileLineNumber() + "\n";
            }
            catch (System.Exception){}
#endif

            AccelByteDebug.LogVerbose("Current Function Called: \n" +
                "---\n" + 
                "Date : " + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\n" +
                "Class : " + className + "\n" +
                functionCaller +
                "Function : " + functName +
                "\n---\n");
        }

        public static void GetHttpRequest(IHttpRequest request, UnityWebRequest unityWebRequest)
        {
            string requestBody = "";
            if (request.BodyBytes != null)
            {
                requestBody = System.Text.Encoding.UTF8.GetString(request.BodyBytes);
            }

            string tempLog = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> item in request.Headers)
            {
                tempLog += item.Key + " : " + item.Value + "\n";
            }

            tempLog = "HTTP Request: \n" +
                "---\n" +
                unityWebRequest.method + " " + unityWebRequest.url + "\n" +
                tempLog +
                "Content-Length : " + requestBody.Length + "\n\n" +
                requestBody +
                "\n---\n";

            AccelByteDebug.LogVerbose(tempLog);
        }

        public static void GetHttpResponse(UnityWebRequest unityWebRequest)
        {
            var responseLog = "HTTP Response: \n" +
                "---\n" +
                "HTTP/1.1 " + unityWebRequest.responseCode + "\n" +
                "Date : " + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\n" +
                "Content-Length : " + unityWebRequest.downloadedBytes + "\n\n" +
                unityWebRequest.downloadHandler.text +
                "\n---\n";
            if(unityWebRequest.responseCode >= 200 && unityWebRequest.responseCode < 300)
            {
                AccelByteDebug.LogVerbose(responseLog);
            }
            else
            {
                AccelByteDebug.LogWarning(responseLog);
            }
        }

        public static void GetWebSocketRequest(string message)
        {
            AccelByteDebug.LogVerbose("WebSocket Send Message: \n" +
                "---\n" +
                message +
                "\n---\n");
        }

        public static void GetWebSocketResponse(string message)
        {
            AccelByteDebug.LogVerbose("WebSocket Receive Message: \n" +
                "---\n" +
                message +
                "\n---\n");
        }

        public static void GetWebSocketNotification(string message)
        {
            AccelByteDebug.LogVerbose("WebSocket Receive Notification: \n" +
                "---\n" +
                message +
                "\n---\n");
        }
        
        public static void GetServerWebSocketResponse(string message)
        {
            AccelByteDebug.LogVerbose("[Server] WebSocket Receive Message: \n" +
                               "---\n" +
                               message +
                               "\n---\n");
        }
        
        public static void GetServerWebSocketNotification(string message)
        {
            AccelByteDebug.LogVerbose("[Server] WebSocket Receive Notification: \n" +
                               "---\n" +
                               message +
                               "\n---\n");
        }
        
        public static void GetProtobufNotification(string message)
        {
            AccelByteDebug.LogVerbose("WebSocket Receive Protobuf Notification: \n" +
                               "---\n" +
                               message +
                               "\n---\n");
        }
    }
}

#if NET_2_0
    namespace System.Runtime.CompilerServices
    {
        [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
        public class CallerMemberNameAttribute : Attribute
        {
        }
    }
#endif