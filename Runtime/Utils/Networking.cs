// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
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
        public abstract class PingOptionalParameters : OptionalParametersBase
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
            UdpPingImp(url, (int) port, pingResult, optionalParameters.InTimeOutInMs, optionalParameters.MaxRetry, optionalParameters.Logger);
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
            HttpPingImp(url, pingResult, optionalParameters.InTimeOutInMs, optionalParameters.MaxRetry, optionalParameters.Logger);
            return pingResult;
        }

        private static async System.Threading.Tasks.Task HttpPingImp(string url, AccelByteResult<int, Error> pingResult, uint timeOutInMs, uint maxRetry, IDebugger debugger)
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
                using (UnityWebRequest webRequest = UnityWebRequest.Get(completeUrl))
                {
                    try
                    {
                        debugger?.LogVerbose($"Ping {tryCount} to {completeUrl}");
                        
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
                            debugger?.LogVerbose($"Ping {tryCount} Success, took {stopwatch.ElapsedMilliseconds} ms");
                        }
                        else
                        {
                            debugger?.LogVerbose($"Ping {tryCount} Failed. Reason: {asyncOp.webRequest.error}");
                            lastErrorMessage = asyncOp.webRequest.error;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        lastErrorMessage = ex.Message;
                        debugger?.LogVerbose($"Ping {tryCount} error: {lastErrorMessage}");
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
        
        private static async System.Threading.Tasks.Task UdpPingImp(string url, int targetPort, AccelByteResult<int, Error> pingResult, uint timeOutInMs, uint maxRetry, IDebugger debugger)
        {
            const int minPort = 0, maxPort = 10000;
            int? latencyResult = null;
            int tryCount = 0;
            var stopwatch = new Stopwatch();
            string lastErrorMessage = null;
            var randomizer = new System.Random();
            int selectedPort = targetPort;
            
            do
            {
                tryCount++;
                stopwatch.Reset();
                try
                {
                    using (var udpClient = new UdpClient(selectedPort))
                    {
                        try
                        {
                            debugger?.LogVerbose($"Ping {tryCount} from port {selectedPort} to {url}:{targetPort}");
                            
                            udpClient.Connect(url, targetPort);
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
                                    try
                                    {
                                        return udpClient.EndReceive(asyncResult, ref remoteIpEndPoint);
                                    }
                                    catch (System.Exception)
                                    {
                                        return null;
                                    }
                                };
                                var waitReceiveTask = System.Threading.Tasks.Task.Factory.FromAsync(receiveResult, receiveEndMethod);
                               
                                await waitReceiveTask;
    
                                byte[] receivedPacket = waitReceiveTask.Result;
                                if (receivedPacket != null)
                                {
                                    string receivedPackagePayloadString = System.Text.Encoding.ASCII.GetString(receivedPacket);
                                    latencyResult = stopwatch.Elapsed.Milliseconds;
                                    debugger?.LogVerbose($"Ping {tryCount} receiving response {receivedPackagePayloadString}");
                                }
                                else
                                {
                                    debugger?.LogVerbose($"Ping {tryCount} receiving no response");
                                    lastErrorMessage = "Receiving no response";
                                }
                            }
                        }
                        catch (System.Exception udpException)
                        {
                            lastErrorMessage = udpException.Message;
                        }
                        
                        udpClient.Close();
                    }
                }
                catch (System.Exception ex)
                {
                    lastErrorMessage = $"Unable to utilize port {selectedPort}\nError Message: {ex.Message}";
                    selectedPort = randomizer.Next(minPort, maxPort);
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
                    $"Encountered issue on calculating latency to \"{url}:{targetPort}\". {lastErrorMessage}";
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