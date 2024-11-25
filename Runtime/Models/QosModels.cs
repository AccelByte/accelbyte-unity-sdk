// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using UnityEngine.Scripting;
using System.ComponentModel;

namespace AccelByte.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum QosStatus
    {
        [Description("UNREACHABLE"), EnumMember(Value = "UNREACHABLE")] Unreachable,
        [Description("INACTIVE"), EnumMember(Value = "ACTIVE")] Inactive,
        [Description("ACTIVE"), EnumMember(Value = "ACTIVE")] Active
    }

    [DataContract, Preserve]
    public class QosServerList
    {
        [DataMember] public QosServer[] servers;
        
        /// <summary>
        /// Build latencies map for matchmaking feature
        /// </summary>
        internal AccelByteResult<System.Collections.Generic.Dictionary<string, int>, Error> GenerateLatenciesMap(bool useCache = true)
        {
            var retval = new AccelByteResult<System.Collections.Generic.Dictionary<string, int>, Error>();
            if (servers == null || servers.Length == 0) 
            {
                retval.Reject(new Error(ErrorCode.InvalidRequest, message: "Server length is 0"));
                return retval;
            }
            
            System.Action<int, QosServer[], System.Collections.Generic.Dictionary<string, int>, AccelByteResult<System.Collections.Generic.Dictionary<string, int>, Error>> buildLatencyMap = null;
            buildLatencyMap = (index, turnServers, latenciesMap, resultTask) =>
            {
                turnServers[index].GetLatency(useCache)
                    .OnSuccess(latency => 
                    {
                        if (!latenciesMap.ContainsKey(turnServers[index].region))
                        {
                            latenciesMap.Add(turnServers[index].region, latency);   
                        }
                        
                        int nextIndex = index + 1;
                        if (nextIndex < turnServers.Length)
                        {
                            buildLatencyMap.Invoke(nextIndex, turnServers, latenciesMap, resultTask);
                        }
                        else
                        {
                            resultTask.Resolve(latenciesMap);
                        }
                    })
                    .OnFailed(error =>
                    {
                        int nextIndex = index + 1;
                        if (nextIndex < turnServers.Length)
                        {
                            buildLatencyMap.Invoke(nextIndex, turnServers, latenciesMap, resultTask);
                        }
                        else
                        {
                            resultTask.Resolve(latenciesMap);
                        }
                    });
            };
                
            buildLatencyMap.Invoke(0, servers, new System.Collections.Generic.Dictionary<string, int>(), retval);

            return retval;
        }
    }

    [DataContract, Preserve]
    public class QosServer
    {
        [DataMember] public string ip;
        [DataMember] public int port;
        [DataMember] public string region;
        [DataMember] public string status;
        [DataMember] public string last_update;
        
        internal ILatencyCalculator LatencyCalculator;
        private int? cachedLatency;

        /// <summary>
        /// Get latency of server
        /// </summary>
        internal AccelByteResult<int, Error> GetLatency(bool useCache = true)
        {
            AccelByteResult<int, Error> retval = new AccelByteResult<int, Error>();
            if (LatencyCalculator == null)
            {
                LatencyCalculator = LatencyCalculatorFactory.CreateDefaultCalculator();
            }
            UnityEngine.Assertions.Assert.IsNotNull(LatencyCalculator);
            if (useCache && cachedLatency != null)
            {
                retval.Resolve(cachedLatency.Value);
                return retval;
            }

            ILatencyCalculator calculator = LatencyCalculator;
            
            string url = ip;
#if UNITY_WEBGL && !UNITY_EDITOR
            url = AccelByte.Utils.Networking.GetTestServerUrlByRegion(region);
#endif
            retval = calculator.CalculateLatency(url, port);
            retval.OnSuccess(newLatency =>
            {
                cachedLatency = newLatency;
            });
            return retval;
        }
        
        /// <summary>
        /// Build latencies map for matchmaking feature
        /// </summary>
        internal AccelByteResult<System.Collections.Generic.Dictionary<string, int>, Error> GenerateLatencyMap(bool useCache = true)
        {
            var retval = new AccelByteResult<System.Collections.Generic.Dictionary<string, int>, Error>();
            GetLatency(useCache)
                .OnSuccess(latency =>
                {
                    var latencyMap = new System.Collections.Generic.Dictionary<string, int>();
                    latencyMap.Add(region, latency);
                    retval.Resolve(latencyMap);
                })
                .OnFailed(error =>
                {
                    retval.Reject(error);
                });
            
            return retval;
        }
    }

    [Preserve]
    public class GetQosServerOptionalParameters
    {
        internal QosStatus? Status;
        internal ILatencyCalculator LatencyCalculator;

        internal FetchQosServerOptionalParameters ConvertToFetchParameters()
        {
            FetchQosServerOptionalParameters retval = new FetchQosServerOptionalParameters();
            retval.Status = Status;
            retval.LatencyCalculator = LatencyCalculator;
            return retval;
        }
    }
    
    [Preserve]
    internal class GetAllServerOptionalParameters
    {
        internal ILatencyCalculator LatencyCalculator;
    }
    
    [Preserve]
    public class FetchQosServerOptionalParameters
    {
        public QosStatus? Status;
        internal ILatencyCalculator LatencyCalculator;
    }
}