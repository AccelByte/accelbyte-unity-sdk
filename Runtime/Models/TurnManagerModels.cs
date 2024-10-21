// Copyright (c) 2022 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine.Scripting;

namespace AccelByte.Models
{
    [DataContract, Preserve]
    public enum P2PConnectionType
    {
        [EnumMember(Value = "none")] None,
        [EnumMember(Value = "host")] Host,
        [EnumMember(Value = "srflx")] Srflx,
        [EnumMember(Value = "prflx")] Prflx,
        [EnumMember(Value = "relay")] Relay
    }
    
    [DataContract, Preserve]
    public enum TurnServerStatus
    {
        [EnumMember(Value = "UNREACHABLE")] UNREACHABLE,
        [EnumMember(Value = "ACTIVE")] ACTIVE
    }

    [DataContract, Preserve]
    public class TurnServerList
    {
        [DataMember] public TurnServer[] servers;

        /// <summary>
        /// Get the closest TURN server
        /// </summary>
        /// <returns>Return the closest TURN server based on latencies.</returns>
        public AccelByteResult<TurnServer, Error> GetClosestTurnServer()
        {
            var retval = new AccelByteResult<TurnServer, Error>();
            if (servers == null || servers.Length == 0) 
            {
                retval.Reject(new Error(ErrorCode.InvalidRequest, message: "Server length is 0"));
                return retval;
            }
            
            System.Action<int, TurnServer[], int?, TurnServer, AccelByteResult<TurnServer, Error>> GetTurnServerTask = null;
            GetTurnServerTask = (index, turnServers, bestLatency, closestTurnServer, resultTask) =>
            {
                turnServers[index].GetLatency(useCache: true)
                    .OnSuccess(latency => 
                    {
                        if (bestLatency == null || bestLatency > latency)
                        {
                            closestTurnServer = turnServers[index];
                            bestLatency = latency;
                        }

                        int nextIndex = index + 1;
                        if (nextIndex < turnServers.Length)
                        {
                            GetTurnServerTask.Invoke(nextIndex, turnServers, bestLatency, closestTurnServer, resultTask);
                        }
                        else
                        {
                            if (closestTurnServer == null)
                            {
                                resultTask.Reject(new Error(ErrorCode.InternalServerError, message: "Failed to find closest turn server"));
                            }
                            else
                            {
                                resultTask.Resolve(closestTurnServer);
                            }
                        }
                    })
                    .OnFailed(error =>
                    {
                        int nextIndex = index + 1;
                        if (nextIndex < turnServers.Length)
                        {
                            GetTurnServerTask.Invoke(nextIndex, turnServers, bestLatency, closestTurnServer, resultTask);
                        }
                        else
                        {
                            if (closestTurnServer == null)
                            {
                                resultTask.Reject(new Error(ErrorCode.InternalServerError, message: "Failed to find closest turn server"));
                            }
                            else
                            {
                                resultTask.Resolve(closestTurnServer);
                            }
                        }
                    });
            };
                
            GetTurnServerTask.Invoke(0, servers, null, null, retval);

            return retval;
        }

        /// <summary>
        /// Build latencies map for matchmaking feature
        /// </summary>
        public AccelByteResult<Dictionary<string, int>, Error> GenerateLatenciesMap(bool useCache = true)
        {
            var retval = new AccelByteResult<Dictionary<string, int>, Error>();
            if (servers == null || servers.Length == 0) 
            {
                retval.Reject(new Error(ErrorCode.InvalidRequest, message: "Server length is 0"));
                return retval;
            }
            
            System.Action<int, TurnServer[], Dictionary<string, int>, AccelByteResult<Dictionary<string, int>, Error>> buildLatencyMap = null;
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
                
            buildLatencyMap.Invoke(0, servers, new Dictionary<string, int>(), retval);

            return retval;
        }
    }

    [DataContract, Preserve]
    public class TurnServer
    {
        [DataMember(Name = "alias")] public string Alias;
        [DataMember(Name = "cpu_usage")] public double CpuUsage;
        [DataMember(Name = "mem_usage")] public TurnServerMemoryUsage MemUsage;
        [DataMember(Name = "netUsage")] public TurnServerNetUsage NetUsage;
        [DataMember] public string ip;
        [DataMember] public int port;
        [DataMember] public int qos_port;
        [DataMember] public string region;
        [DataMember] public TurnServerStatus status;
        [DataMember] public string last_update;
        [DataMember] public long current_time;

        internal ILatencyCalculator LatencyCalculator;
        private int? cachedLatency;

        /// <summary>
        /// Get latency of turn server
        /// </summary>
        public AccelByteResult<int, Error> GetLatency(bool useCache = true)
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
            retval = calculator.CalculateLatency(ip, qos_port);
            retval.OnSuccess(newLatency =>
            {
                cachedLatency = newLatency;
            });
            return retval;
        }
        
        /// <summary>
        /// Build latencies map for matchmaking feature
        /// </summary>
        public AccelByteResult<Dictionary<string, int>, Error> GenerateLatencyMap(bool useCache = true)
        {
            var retval = new AccelByteResult<Dictionary<string, int>, Error>();
            GetLatency(useCache)
                .OnSuccess(latency =>
                {
                    Dictionary<string, int> latencyMap = new Dictionary<string, int>();
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

    [DataContract, Preserve]
    public class TurnServerMemoryUsage
    {
        [DataMember(Name = "total")] public ulong Total;
        [DataMember(Name = "used")] public ulong Used;
    }

    [DataContract, Preserve]
    public class TurnServerNetUsage
    {
        [DataMember(Name = "rx")] public ulong Rx;
        [DataMember(Name = "tx")] public ulong Tx;
    }

    [DataContract, Preserve]
    public class TurnServerCredential
    {
        [DataMember] public string ip;
        [DataMember] public int port;
        [DataMember] public string region;
        [DataMember] public string username;
        [DataMember] public string password;
    }

    [DataContract, Preserve]
    public class TurnServerMetricRequest
    {
        [DataMember(Name = "region")] public string Region;
        [DataMember(Name = "type")] public P2PConnectionType Type;
    }

    [DataContract, Preserve]
    public class DisconnectTurnServerRequest
    {
        [DataMember(Name = "user_id")] public string UserId;
    }
    
    [Preserve]
    public class GetTurnServerOptionalParameters
    {
        internal ILatencyCalculator LatencyCalculator;
    }
}
