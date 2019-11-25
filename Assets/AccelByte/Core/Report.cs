// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Runtime.CompilerServices;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace AccelByte.Core
{
    public class Report
    {
        public static void GetFunctionLog(string className, [CallerMemberName] string functName = "")
        {
            Debug.Log("Current Function Called: \n" +
                "---\n" + 
                "Date : " + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\n" +
                "Class : " + className + "\n" +
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

            Debug.Log(tempLog);
        }

        public static void GetHttpResponse(UnityWebRequest unityWebRequest)
        {
            Debug.Log("HTTP Response: \n" +
                "---\n" +
                "HTTP/1.1 " + unityWebRequest.responseCode + "\n" +
                "Date : " + System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ") + "\n" +
                "Content-Length : " + unityWebRequest.downloadedBytes + "\n\n" +
                unityWebRequest.downloadHandler.text +
                "\n---\n");
        }

        public static void GetWebSocketRequest(string message)
        {
            Debug.Log("WebSocket Send Message: \n" +
                "---\n" +
                message +
                "\n---\n");
        }

        public static void GetWebSocketResponse(string message)
        {
            Debug.Log("WebSocket Receive Message: \n" +
                "---\n" +
                message +
                "\n---\n");
        }

        public static void GetWebSocketNotification(string message)
        {
            Debug.Log("WebSocket Receive Notification: \n" +
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