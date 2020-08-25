// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Models;
using AccelByte.Core;
using UnityEngine.Assertions;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utf8Json;
#if ENABLE_AGONES_PLUGIN
using Agones;
using Agones.Model;
#endif

namespace AccelByte.Server
{
    internal enum ServerType
    {
        NONE,
        LOCALSERVER,
        CLOUDSERVER
    }

    internal enum Provider
    {
       AGONES,
       AMPD,
       AWS,
       BAREMETAL,
       I3D,
       DEFAULT
    }

#if ENABLE_AGONES_PLUGIN
    internal class Agones
    {
        private AgonesSdk component = null;
        public void SetComponentSDK(AgonesSdk component_) { component = component_; }
        public AgonesSdk GetSDK() { return component; }

        private bool isReady = false;
        public void SetReady(bool ready) { isReady = ready; }
        public bool IsReady() { return isReady; }
        
        public readonly string MATCH_DETAILS_ANNOTATION = "match-details";
        // Agones.Ready() has 30 seconds limit 
        public readonly TimeSpan REGISTRATION_TIMEOUT = new TimeSpan(0,0,30);
        public readonly TimeSpan INITIAL_HEALTH_CHECK_TIMEOUT = new TimeSpan(0,0,30);
    }
#endif
    
    internal class DedicatedServerManagerApi
    {
        private readonly string baseUrl;
        private readonly IHttpWorker httpWorker;
        private readonly string namespace_;
        private string dsmServerUrl = "";
        private RegisterServerRequest serverSetup;
        private ServerType serverType = ServerType.NONE;
#if ENABLE_AGONES_PLUGIN
        private Agones agones = new Agones();
#endif

        internal DedicatedServerManagerApi(string baseUrl, string namespace_, IHttpWorker httpWorker)
        {
            Debug.Log("ServerApi init serverapi start");
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsFalse(
                string.IsNullOrEmpty(namespace_),
                "Creating " + GetType().Name + " failed. Parameter namespace is null.");
            Assert.IsNotNull(httpWorker, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.namespace_ = namespace_;
            this.httpWorker = httpWorker;
            this.serverSetup = new RegisterServerRequest() {
                game_version = "",
                ip = "",
                pod_name = "",
                provider = ""
            };
            
            ParseArgsAndServerSetup();
        }

        public IEnumerator RegisterServer(RegisterServerRequest registerRequest, string accessToken,
            ResultCallback callback)
        {
            Assert.IsNotNull(registerRequest, "Register failed. registerserverRequest is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");
            if (this.serverType != ServerType.NONE)
            {
                callback.TryError(ErrorCode.Conflict, "Server is already registered.");

                yield break;
            }
            if (dsmServerUrl.Length == 0)
            {
                ServerQos qos = AccelByteServerPlugin.GetQos();
                Result<Dictionary<string, int>> latenciesResult = null;
                qos.GetServerLatencies(reqResult => latenciesResult = reqResult);
                yield return new WaitUntil(() => latenciesResult != null);

                foreach (KeyValuePair<string, int> latency in latenciesResult.Value.OrderBy(item => item.Value))
                {
                    var getUrlRequest = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/dsm?region=" + latency.Key)
                        .WithBearerAuth(accessToken)
                        .WithContentType(MediaType.ApplicationJson)
                        .Accepts(MediaType.ApplicationJson)
                        .GetResult();

                    IHttpResponse getUrlResponse = null;

                    yield return this.httpWorker.SendRequest(getUrlRequest, rsp => getUrlResponse = rsp);

                    var getUrlResult = getUrlResponse.TryParseJson<DSMClient>();
                    if (getUrlResult.Value.status == "HEALTHY")
                    {
                        dsmServerUrl = getUrlResult.Value.host_address;
                        break;
                    }
                }
            }
            if(serverSetup.ip.Length == 0)
            {
                var getPubIpRequest = HttpRequestBuilder.CreateGet("https://api.ipify.org?format=json")
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

                IHttpResponse getPubIpResponse = null;

                yield return this.httpWorker.SendRequest(getPubIpRequest, rsp => getPubIpResponse = rsp);

                var getPubIpResult = getPubIpResponse.TryParseJson<PubIp>();
                serverSetup.ip = getPubIpResult.Value.ip;
            }

            if (IsCurrentProvider(Provider.AGONES))
            {
#if ENABLE_AGONES_PLUGIN
                InitiateAgones(callback);
#else
                callback.TryError(ErrorCode.NotFound, 
                    "Can't request register server. Agones provider arguments is passed but Agones plugin is not found.");
#endif
            }
            else
            {
                registerRequest.ip = serverSetup.ip;
                registerRequest.provider = serverSetup.provider;
                registerRequest.game_version = serverSetup.game_version;
    
                var request = HttpRequestBuilder.CreatePost(this.dsmServerUrl + "/dsm/namespaces/{namespace}/servers/register")
                    .WithPathParam("namespace", this.namespace_)
                    .WithBearerAuth(accessToken)
                    .WithContentType(MediaType.ApplicationJson)
                    .Accepts(MediaType.ApplicationJson)
                    .WithBody(registerRequest.ToUtf8Json())
                    .GetResult();
    
                IHttpResponse response = null;
    
                yield return this.httpWorker.SendRequest(request, rsp => response = rsp);
    
                var result = response.TryParseJson<ServerInfo>();
                if (!result.IsError)
                {
                    serverSetup.pod_name = result.Value.pod_name;
                    serverType = ServerType.CLOUDSERVER;
                }   
                callback.Try(response.TryParse());
            }
        }

        public IEnumerator ShutdownServer(ShutdownServerRequest shutdownServerRequest, string accessToken,
            ResultCallback callback)
        {
            if (IsCurrentProvider(Provider.AGONES) && serverType != ServerType.LOCALSERVER)
            {
#if ENABLE_AGONES_PLUGIN
                yield return ShutdownAgones(callback);
#else
                callback.TryError(ErrorCode.NotFound, 
                    "Can't request shutdown server. Agones provider arguments is passed but Agones plugin is not found.");
#endif
                yield break;
            }
            Assert.IsNotNull(shutdownServerRequest, "Register failed. shutdownServerNotif is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");
            if (this.serverType != ServerType.CLOUDSERVER)
            {
                callback.TryError(ErrorCode.Conflict, "Server not registered as Cloud Server.");

                yield break;
            }

            shutdownServerRequest.pod_name = serverSetup.pod_name;
            var request = HttpRequestBuilder.CreatePost(this.dsmServerUrl + "/dsm/namespaces/{namespace}/servers/shutdown")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(shutdownServerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            serverType = ServerType.NONE;
            callback.Try(result);
        }

        public IEnumerator SendHeartBeat(string name, string accessToken, ResultCallback<MatchRequest> callback)
        {
            if (IsCurrentProvider(Provider.AGONES) && serverType == ServerType.CLOUDSERVER)
            {
#if ENABLE_AGONES_PLUGIN
                SendAgonesHeartBeat(callback);
#else
                callback.Try(Result<MatchRequest>.CreateError(ErrorCode.NotFound, 
                    "Can't send Agones health. Agones provider arguments is passed but Agones plugin is not found."));
#endif
            }
            else
            {
                yield return SendDefaultHeartBeat(name, accessToken, callback);
            }
        }

        private IEnumerator SendDefaultHeartBeat(string name, string accessToken, ResultCallback<MatchRequest> callback)
        {
            if (serverType == ServerType.CLOUDSERVER)
            {
                name = serverSetup.pod_name;
            }
            Assert.IsNotNull(accessToken, "Can't send heartbeat! accessToken parameter is null!");
            string reqUrl;
            switch (serverType)
            {
                case ServerType.CLOUDSERVER: reqUrl = dsmServerUrl + "/dsm"; break;
                case ServerType.LOCALSERVER:
                default: reqUrl = this.baseUrl; break; 
            }
            var request = HttpRequestBuilder.CreatePost(reqUrl + "/namespaces/{namespace}/servers/heartbeat")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"name\": \"{0}\"}}", name))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            if (response.BodyBytes == null || response.BodyBytes.Length == 0)
            {
                callback.Try(null);
            }
            else
            {
                var result = response.TryParseJson<MatchRequest>();
                callback.Try(result);
            }
        }
        
        public IEnumerator RegisterLocalServer(RegisterLocalServerRequest registerRequest, string accessToken,
            ResultCallback callback)
        {
            Assert.IsNotNull(registerRequest, "Register failed. registerRequest is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");

            if (this.serverType != ServerType.NONE)
            {
                callback.TryError(ErrorCode.Conflict, "Server is already registered.");

                yield break;
            }

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/servers/local/register")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(registerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            serverType = ServerType.LOCALSERVER;

            callback.Try(result);
        }
        
        public IEnumerator DeregisterLocalServer(string name, string accessToken, ResultCallback callback)
        {
            Assert.IsNotNull(name, "Deregister failed. name is null!");
            Assert.IsNotNull(accessToken, "Can't update a slot! accessToken parameter is null!");

            if (this.serverType != ServerType.LOCALSERVER)
            {
                callback.TryError(ErrorCode.Conflict, "Server not registered as Local Server.");

                yield break;
            }

            var request = HttpRequestBuilder.CreatePost(this.baseUrl + "/namespaces/{namespace}/servers/local/deregister")
                .WithPathParam("namespace", this.namespace_)
                .WithBearerAuth(accessToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"name\": \"{0}\"}}", name))
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpWorker.SendRequest(request, rsp => response = rsp);

            var result = response.TryParse();
            serverType = ServerType.NONE;

            callback.Try(result);
        }
        
        private void ParseArgsAndServerSetup()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            bool isProviderFound = false;
            bool isGameVersionFound = false;
            foreach (string arg in args)
            {
                Debug.Log("arg: " + arg);
                if (arg.Contains("provider"))
                {
                    string[] split = arg.Split('=');
                    string providerArgValue = split[1];
                    foreach (var _enum in Enum.GetNames(typeof(Provider)))
                    {
                        if (providerArgValue.Equals(_enum.ToLower()))
                        {
                            serverSetup.provider = providerArgValue.ToLower();
                            isProviderFound = true;
                        }
                    }
                }

                if (arg.Contains("game_version"))
                {
                    string[] split = arg.Split('=');
                    serverSetup.game_version = split[1];
                    isGameVersionFound = true;
                }

                if (isProviderFound && isGameVersionFound)
                {
                    break;
                }
            }
        }
        
        private bool IsCurrentProvider(Provider provider)
        {
            return serverSetup.provider.Equals(provider.ToString().ToLower());
        }

#if ENABLE_AGONES_PLUGIN
        private async void InitiateAgones(ResultCallback callback)
        {
            GameObject dummyGameObject = GameObject.Find("AccelByteDummyGameObject");
            if (dummyGameObject == null)
            {
                dummyGameObject = new GameObject("AccelByteDummyGameObject");
            }
            agones.SetComponentSDK(dummyGameObject.AddComponent<AgonesSdk>());
            UnityEngine.Object.DontDestroyOnLoad(dummyGameObject);

            bool isAgonesConnectionEstablished = await agones.GetSDK().Connect();
            if (isAgonesConnectionEstablished)
            {
                bool isAgonesGameServerIsReady = await agones.GetSDK().Ready();
                if (isAgonesGameServerIsReady)
                {
                    //Check Agones GameServer's health 
                    DateTime healthCheckStarted = DateTime.Now;
                    var gameServer = await agones.GetSDK().GameServer();
                    
                    if (gameServer == null && 
                        DateTime.Now.Subtract(healthCheckStarted) > agones.INITIAL_HEALTH_CHECK_TIMEOUT)
                    {
                        Debug.Log("[Agones] GameServer is not healthy. Shutting down.");
                        agones.GetSDK().Shutdown();
                    }
                    else
                    {
                        Debug.Log("[Agones] GameServer is healthy.");
                        agones.SetReady(true);
                    }
                }
                else
                {
                    Debug.Log("[Agones] GameServer is not ready.");
                }
            }
            else
            {
                Debug.Log("[Agones] Failed to establish a connection to GameServer.");
            }
            
            if (agones.IsReady())
            {
                callback.TryOk();
                serverType = ServerType.CLOUDSERVER;
            }
            else
            {
                DateTime startWaitingRegistrationTime = DateTime.Now;
                while (startWaitingRegistrationTime.Add(agones.REGISTRATION_TIMEOUT) > DateTime.Now)
                {
                    if (agones.IsReady())
                    {
                        break;
                    }

                    await Task.Delay(1000);
                }

                if (agones.IsReady())
                {
                    callback.TryOk();
                    serverType = ServerType.CLOUDSERVER;
                }
                else
                {
                    callback.TryError(ErrorCode.ServiceUnavailable, "Agones GameServer is not ready.");
                }
            }
        }

        private async Task ShutdownAgones(ResultCallback callback)
        {
            if (await agones.GetSDK().Shutdown())
            {
                Debug.Log("Successfully shutting down Agones GameServer.");
                agones.SetReady(false);
                callback.TryOk();
            }
            else
            {
                callback.TryError(ErrorCode.UnknownError, "Failed to shutdown Agones GameServer.");
            }
            serverType = ServerType.NONE;
        }
        
        private async void SendAgonesHeartBeat(ResultCallback<MatchRequest> callback)
        {
            if (!agones.IsReady())
            {
                return;
            }
            
            string matchDetails;
            GameServer gameServer = await agones.GetSDK().GameServer();

            if (!gameServer.ObjectMeta.Annotations.TryGetValue(agones.MATCH_DETAILS_ANNOTATION, out matchDetails) || gameServer == null)
            {
                return;
            }
            else 
            {
                var matchRequest = JsonSerializer.Deserialize<MatchRequest>(matchDetails);
                var result = Result<MatchRequest>.CreateOk(matchRequest);
                callback.Try(result);
            }
        }
#endif
    }
}
