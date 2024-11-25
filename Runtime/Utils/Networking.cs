// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace AccelByte.Utils
{
    public static class Networking
    {
        public abstract class PingOptionalParameters
        {
            public uint InTimeOutInMs = 10 * 1000;
            public uint MaxRetry = 6;
        }

        public class UdpPingOptionalParameters : PingOptionalParameters
        {
            
        }

        public class HttpPingOptionalParameters : PingOptionalParameters
        {
            
        }
        
        /// <summary>
        /// Generate test server address by region name
        /// </summary>
        /// <param name="region"></param>
        public static string GetTestServerUrlByRegion(string region)
        {
            string retval = $"https://ec2.{region}.amazonaws.com/ping";
            return retval;
        }
        
        /// <summary>
        /// Ping a specific url
        /// </summary>
        /// <param name="url">url to ping</param>
        /// <param name="inTimeOutInMs">timeout limit</param>
        [System.Obsolete("This ping method is deprecated. Please use UdpPing or HttpPing method. This method will be removed on AGS 3.82.")]
        public static AccelByteResult<int, Error> Ping(string url, uint inTimeOutInMs = 60 * 1000)
        {
            AccelByteResult<int, Error> pingResult = new AccelByteResult<int, Error>();
            HttpPingImp(url, pingResult, inTimeOutInMs, maxRetry: 1);
            return pingResult;
        }
        
        /// <summary>
        /// Ping a specific url with UDP client. This feature isn't supported on Web Platform.
        /// </summary>
        /// <param name="url">Url to ping</param>
        /// <param name="port">Assign the port number to ping</param>
        /// <param name="optionalParameters">Ping optional parameters</param>
        public static AccelByteResult<int, Error> UdpPing(string url, uint port, UdpPingOptionalParameters optionalParameters = null)
        {
            AccelByteResult<int, Error> pingResult = new AccelByteResult<int, Error>();
#if !UNITY_WEBGL || UNITY_EDITOR
            if (optionalParameters == null)
            {
                optionalParameters = new UdpPingOptionalParameters();
            }
            UdpPingImp(url, (int) port, pingResult, optionalParameters.InTimeOutInMs, optionalParameters.MaxRetry);
#else
            pingResult.Reject(new Error(ErrorCode.ErrorFromException, message: "UDP ping isn't supported on Web Platform"));
#endif
            return pingResult;
        }
        
        /// <summary>
        /// Ping a specific url with Http Webrequest.
        /// </summary>
        /// <param name="url">Url to ping</param>
        /// <param name="optionalParameters">Ping optional parameters</param>
        public static AccelByteResult<int, Error> HttpPing(string url, HttpPingOptionalParameters optionalParameters = null)
        {
            AccelByteResult<int, Error> pingResult = new AccelByteResult<int, Error>();
            if (optionalParameters == null)
            {
                optionalParameters = new HttpPingOptionalParameters();
            }
            HttpPingImp(url, pingResult, optionalParameters.InTimeOutInMs, optionalParameters.MaxRetry);
            return pingResult;
        }

        private static async System.Threading.Tasks.Task HttpPingImp(string url, AccelByteResult<int, Error> pingResult, uint timeOutInMs, uint maxRetry)
        {
            string completeUrl = url;
            
            double? latencyResult = null;
            int tryCount = 0;
            var stopwatch = new Stopwatch();
            string lastErrorMessage = null;

            do
            {
                tryCount++;
                stopwatch.Reset();
                using (UnityWebRequest webRequest = UnityWebRequest.Head(completeUrl))
                {
                    try
                    {
                        webRequest.timeout = (int)(timeOutInMs / 1000);
                        var asyncOp = webRequest.SendWebRequest();

                        stopwatch.Start();
                        while (!asyncOp.isDone)
                        {
                            await Task.Yield();
                        }

                        stopwatch.Stop();

                        if (asyncOp.webRequest.result != UnityWebRequest.Result.ConnectionError)
                        {
                            latencyResult = stopwatch.Elapsed.TotalMilliseconds;
                        }
                        else
                        {
                            lastErrorMessage = asyncOp.webRequest.error;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        lastErrorMessage = ex.Message;
                    }
                }
            } while (latencyResult == null && tryCount < maxRetry);

            if (latencyResult != null)
            {
                pingResult.Resolve((int) System.Math.Round(latencyResult.Value));
            }
            else
            {
                string errorMessage =
                    $"Encountered issue on calculating latency to \"{url}\". {lastErrorMessage}";
                pingResult.Reject(new Error(ErrorCode.InternalServerError, errorMessage));
            }
        }
        
        private static async System.Threading.Tasks.Task UdpPingImp(string url, int port, AccelByteResult<int, Error> pingResult, uint timeOutInMs, uint maxRetry)
        {
            int? latencyResult = null;
            int tryCount = 0;
            var stopwatch = new Stopwatch();
            string lastErrorMessage = null;

            do
            {
                tryCount++;
                stopwatch.Reset();
                
                try
                {
                    using (var udpClient = new UdpClient(port))
                    {
                        udpClient.Connect(url, port);
                        byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes("PING");
                        stopwatch.Start();

                        using (var cts = new System.Threading.CancellationTokenSource())
                        {
                            System.Threading.Tasks.Task<int> sendPingTask = null;
                            UdpPingTimer((int) timeOutInMs, udpClient, cts.Token);

                            System.IAsyncResult sendResult = udpClient.BeginSend(
                                sendBytes,
                                sendBytes.Length,
                                null,
                                null);

                            sendPingTask = System.Threading.Tasks.Task.Factory.FromAsync(sendResult, udpClient.EndSend);
                            await sendPingTask;

                            var remoteIpEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);

                            System.IAsyncResult receiveResult = udpClient.BeginReceive(null, null);

                            System.Func<System.IAsyncResult, byte[]> receiveEndMethod = (asyncResult) =>
                            {
                                return udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
                            };
                            await System.Threading.Tasks.Task.Factory.FromAsync(receiveResult, receiveEndMethod);
                        }

                        udpClient.Close();
                        latencyResult = stopwatch.Elapsed.Milliseconds;
                    }
                }
                catch (System.Exception ex)
                {
                    lastErrorMessage = ex.Message;
                }

                stopwatch.Stop();
            } while (latencyResult == null && tryCount < maxRetry);

            if (latencyResult != null)
            {
                pingResult.Resolve(latencyResult.Value);
            }
            else
            {
                string errorMessage =
                    $"Encountered issue on calculating latency to \"{url}:{port}\". {lastErrorMessage}";
                pingResult.Reject(new Error(ErrorCode.InternalServerError, errorMessage));
            }
        }

        private static async void UdpPingTimer(int timeoutMiliseconds, UdpClient udpClient, System.Threading.CancellationToken ct)
        {
            await System.Threading.Tasks.Task.Delay(timeoutMiliseconds);
            if (!ct.IsCancellationRequested && udpClient != null && udpClient.Client != null)
            {
                udpClient.Close();
            }
        }
    }
}