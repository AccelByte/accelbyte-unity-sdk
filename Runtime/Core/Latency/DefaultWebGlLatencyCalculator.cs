// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Core
{
    internal class DefaultWebGlLatencyCalculator : ILatencyCalculator
    {
        private int timeOutSeconds;
        private const string awsPingUrl = "https://{ip}:{port}";
        
        public DefaultWebGlLatencyCalculator(int timeOutSeconds = 60)
        {
            this.timeOutSeconds = timeOutSeconds;
        }
        
        private void StartPing(string url, IDebugger debugger, AccelByteResult<int, Error> resultCallback)
        {
            var pingResult = Utils.Networking.HttpPing(url, new Networking.HttpPingOptionalParameters()
            {
                InTimeOutInMs = (uint)timeOutSeconds * 1000
            });
            pingResult.OnSuccess(latencyResult => 
                { 
                    resultCallback.Resolve(latencyResult); 
                })
                .OnFailed(error => 
                { 
                    resultCallback.Reject(error); 
                });
        }
        
        public AccelByteResult<int, Error> CalculateLatency(string ip, int port, IDebugger debugger)
        {
            var result = new AccelByteResult<int, Error>();
            var url = awsPingUrl.Replace("{ip}", ip).Replace("{port}", port.ToString());
            
            StartPing(url, debugger, result);
            
            return result;
        }
    }
}