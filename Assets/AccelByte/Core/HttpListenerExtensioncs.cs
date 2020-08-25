// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;

namespace AccelByte.Core
{
    class HttpListenerExtension
    {
        public static string availableLocalUrl;
        private static int availablePort;
        private static string localUrl = "http://localhost";

        public static Result listenerResult;
        private static Thread listenerThread;
        private static HttpListener httpListener;

        public static string GetAvailableLocalUrl()
        {
            availablePort = GetAvailablePort();
            availableLocalUrl = string.Format(("{0}:{1}/"),
                localUrl,
                availablePort);

            return availableLocalUrl;
        }

        private static int GetAvailablePort()
        {
            availablePort = (new Random()).Next(1024, 49151);

            IPEndPoint[] endPoints;
            List<int> portArray = new List<int>();

            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();

            //Get Active Connection
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            portArray.AddRange(from n in connections
                               where n.LocalEndPoint.Port >= availablePort
                               select n.LocalEndPoint.Port);

            //Get Active TCP Listener
            endPoints = properties.GetActiveTcpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= availablePort
                               select n.Port);

            //Get Active UDP Listener
            endPoints = properties.GetActiveUdpListeners();
            portArray.AddRange(from n in endPoints
                               where n.Port >= availablePort
                               select n.Port);

            portArray.Sort();

            for (int i = availablePort; i < UInt16.MaxValue; i++)
            {
                if (!portArray.Contains(i))
                {
                    availablePort = i;
                    return i;
                }
            }

            return 0;
        }

        public static void StartHttpListener(string sessionId)
        {
            Process.Start(string.Format("https://{0}/upgrade-account-from-sdk?temporary_session_id={1}",
                AccelBytePlugin.Config.NonApiBaseUrl,
                sessionId));

            if (httpListener != null)
            {
                if (httpListener.IsListening)
                {
                    httpListener.Stop();
                    httpListener.Prefixes.Clear();
                }
            }

            listenerResult = null;

            httpListener = new HttpListener();
            httpListener.Prefixes.Add(availableLocalUrl);
            httpListener.Start();

            listenerThread = new Thread(StartListener);
            listenerThread.Start();
        }

        private static void StartListener()
        {
            while (true)
            {
                var result = httpListener.BeginGetContext(ListenerCallback, httpListener);
                result.AsyncWaitHandle.WaitOne();

                if (!httpListener.IsListening)
                    break;
            }
        }

        private static void ListenerCallback(IAsyncResult result)
        {
            var context = httpListener.EndGetContext(result);

            if (context.Request.HttpMethod == "GET" || context.Request.HttpMethod == "OPTIONS")
            {
                context.Response.StatusCode = (int)HttpStatusCode.OK;

                listenerResult = Result.CreateOk();

                httpListener.Stop();
            }

            context.Response.Close();
        }

    }
}
