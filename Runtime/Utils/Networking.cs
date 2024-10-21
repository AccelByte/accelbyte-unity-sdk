// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace AccelByte.Utils
{
    public static class Networking
    {
        private static CoreHeartBeat coreHeartBeat;
        private static uint timeOutInMs;
        
        /// <summary>
        /// Ping a specific url
        /// </summary>
        /// <param name="url">url to ping</param>
        /// <param name="inTimeOutInMs">timeout limit</param>
        public static AccelByteResult<int, Error> Ping(string url, uint inTimeOutInMs = 60 * 1000)
        {
            AccelByteResult<int, Error> pingResult = new AccelByteResult<int, Error>();
            timeOutInMs = inTimeOutInMs;
            if (coreHeartBeat == null)
            {
                coreHeartBeat = new CoreHeartBeat();
            }
#if UNITY_WEBGL
            HttpPing(url, pingResult);
#else
            HttpPing(url, pingResult);
#endif
            return pingResult;
        }

        internal static async System.Threading.Tasks.Task HttpPing(string url, AccelByteResult<int, Error> pingResult)
        {
            using (var cts = new System.Threading.CancellationTokenSource())
            { 
                coreHeartBeat.Wait(new WaitTimeCommand(timeOutInMs, cancellationToken: cts.Token, onDone: () => 
                { 
                    pingResult.Reject(new Error(ErrorCode.RequestTimeout));
                }));
                
                using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
                {
                    var stopWatch = new Stopwatch();
                    var asyncOp = webRequest.SendWebRequest();
                    stopWatch.Start();
                    while (!asyncOp.isDone)
                    {
                        await Task.Yield();
                    }

                    stopWatch.Stop();
                    pingResult.Resolve(stopWatch.Elapsed.Milliseconds);
                    cts.Cancel();
                }
            }
        }
    }
}