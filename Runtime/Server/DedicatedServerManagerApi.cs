﻿// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    internal enum ServerType
    {
        NONE,
        LOCALSERVER,
        CLOUDSERVER,
    }

    public struct ServerSetupData
    {
        public string region;
        public string provider;
        public string gameVersion;
    }

    public class DedicatedServerManagerApi : ServerApiBase
    {        
        private string region = "";
        internal RegisterServerRequest ServerSetup;
        internal ServerType ServerType = ServerType.NONE;

        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==DSMControllerServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal DedicatedServerManagerApi( IHttpClient httpClient
            , ServerConfig config
            , ISession session ) 
            : base( httpClient, config, config.DSMControllerServerUrl, session)
        {
            ServerSetup = new RegisterServerRequest() 
            {
                game_version = "",
                ip = "",
                pod_name = "",
                provider = "",
            };

            ParseArgsAndServerSetup();
        }

        public IEnumerator RegisterServer( RegisterServerRequest registerRequest
            , ResultCallback callback )
        {
            Assert.IsNotNull(registerRequest, "Register failed. registerserverRequest is null!");
            Assert.IsNotNull(AuthToken, "Can't update a slot! AuthToken parameter is null!");

            registerRequest.ip = ServerSetup.ip;
            registerRequest.provider = ServerSetup.provider;
            registerRequest.game_version = ServerSetup.game_version;
    
            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/namespaces/{namespace}/servers/register")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(registerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<ServerInfo>();
            if (!result.IsError)
            {
                if (result.Value != null)
                {
                    ServerSetup.pod_name = result.Value.pod_name;
                    ServerType = ServerType.CLOUDSERVER;
                    callback?.TryOk();
                }
                else
                {
                    callback?.TryError(new Error(ErrorCode.UnknownError, "Server response is null"));
                }
            }
            else
            {
                callback?.TryError(result.Error);
            }
        }

        public IEnumerator ShutdownServer( ShutdownServerRequest shutdownServerRequest
            , ResultCallback callback )
        {
            Assert.IsNotNull(shutdownServerRequest, "Register failed. shutdownServerNotif is null!");
            Assert.IsNotNull(AuthToken, "Can't update a slot! AuthToken parameter is null!");
            if (ServerType != ServerType.CLOUDSERVER)
            {
                callback?.TryError(ErrorCode.Conflict, "Server not registered as Cloud Server.");
                yield break;
            }

            shutdownServerRequest.pod_name = ServerSetup.pod_name;
            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/namespaces/{namespace}/servers/shutdown")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(shutdownServerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            ServerType = ServerType.NONE;
            callback?.Try(result);
        }

        public IEnumerator RegisterLocalServer( RegisterLocalServerRequest registerRequest
            , ResultCallback callback )
        {
            Assert.IsNotNull(registerRequest, "Register failed. registerRequest is null!");
            Assert.IsNotNull(AuthToken, "Can't update a slot! AuthToken parameter is null!");

            if (ServerType != ServerType.NONE)
            {
                callback?.TryError(ErrorCode.Conflict, "Server is already registered.");

                yield break;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/namespaces/{namespace}/servers/local/register")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(registerRequest.ToUtf8Json())
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            if(!result.IsError)
            {
                ServerType = ServerType.LOCALSERVER;
                ServerSetup.pod_name = registerRequest.name;
            }

            callback?.Try(result);
        }

        [Obsolete]
        public IEnumerator RegisterLocalServer( uint port
            , string inName
            , ResultCallback callback )
        {
            string ip = "";
            AccelByteNetUtilities.GetPublicIpWithIpify(HttpOperator, getPublicIpResult =>
            {
                ip = getPublicIpResult.Value.ip;
            });

            while(string.IsNullOrEmpty(ip))
            {
                yield return null;
            }

            RegisterLocalServerRequest request = new RegisterLocalServerRequest()
            {
                ip = ip,
                port = port,
                name = inName,
            };
            yield return RegisterLocalServer(request, callback);
        }
        
        public IEnumerator DeregisterLocalServer( string inName
            , ResultCallback callback )
        {
            Assert.IsNotNull(inName, "Deregister failed. name is null!");
            Assert.IsNotNull(AuthToken, "Can't update a slot! AuthToken parameter is null!");

            if (this.ServerType != ServerType.LOCALSERVER)
            {
                callback?.TryError(ErrorCode.Conflict, "Server not registered as Local Server.");

                yield break;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/namespaces/{namespace}/servers/local/deregister")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithBody(string.Format("{{\"name\": \"{0}\"}}", inName))
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParse();
            ServerType = ServerType.NONE;

            callback?.Try(result);
        }

        public IEnumerator GetSessionId( ResultCallback<ServerSessionResponse> callback )
        {
            Assert.IsNotNull(AuthToken, "Can't check session ID! AuthToken parameter is null!");

            if(ServerType == ServerType.NONE)
            {
                callback?.TryError(new Error(ErrorCode.NotFound, "Server not registered yet"));
                yield break;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/namespaces/{namespace}/servers/{server}/session")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("server", ServerSetup.pod_name)
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<ServerSessionResponse>();
            callback?.Try(result);
        }

        public IEnumerator GetSessionTimeout( ResultCallback<ServerSessionTimeoutResponse> callback )
        {
            if(AuthToken == null)
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "AuthToken parameter is null!"));
                yield break;
            }

            if (ServerType == ServerType.NONE)
            {
                callback?.TryError(new Error(ErrorCode.NotFound, "Server not registered yet"));
                yield break;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/namespaces/{namespace}/servers/{server}/config/sessiontimeout")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("server", ServerSetup.pod_name)
                .WithBearerAuth(AuthToken)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, rsp =>
            {
                response = rsp;
            });

            var result = response.TryParseJson<ServerSessionTimeoutResponse>();
            callback?.Try(result);
        }

        public void ServerHeartBeat(ResultCallback callback)
        {
            if (AuthToken == null)
            {
                callback?.TryError(new Error(ErrorCode.InvalidRequest, "AuthToken parameter is null!"));
                return;
            }

            if (ServerType == ServerType.NONE)
            {
                callback?.TryError(new Error(ErrorCode.NotFound, "Server not registered yet"));
                return;
            }

            var heartBeatRequest = new HeartBeatRequest
            {
                PodName = ServerSetup.pod_name
            };
            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/namespaces/{namespace}/servers/heartbeat")
                .WithPathParam("namespace", Namespace_)
                .WithBearerAuth(AuthToken)
                .WithContentType("application/json")
                .WithBody(heartBeatRequest.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                callback?.Try(result);
            });
        }

        private void ParseArgsAndServerSetup()
        {
            string[] args = System.Environment.GetCommandLineArgs();
            SharedMemory?.Logger?.Log($"parse server command arg: {args}");
            ServerSetupData serverSetupData = ParseCommandLine(args);
            
            region = serverSetupData.region;
            ServerSetup.provider = serverSetupData.provider;
            ServerSetup.game_version = serverSetupData.gameVersion;
        }

        private bool IsCurrentProvider( string provider )
        {
            return ServerSetup.provider != null && ServerSetup.provider.Equals(provider.ToLower());
        }

        public static ServerSetupData ParseCommandLine( string[] args )
        {
            bool isProviderFound = false;
            bool isGameVersionFound = false;
            bool isRegionFound = false;
            string region = "";
            string provider = "";
            string game_version = "";
            ServerSetupData serverSetupData = new ServerSetupData();
            foreach (string arg in args)
            {
                if (arg.Contains("provider"))
                {
                    string[] split = arg.Split('=');
                    provider = split[1];
                    isProviderFound = true;
                }

                if (arg.Contains("game_version"))
                {
                    string[] split = arg.Split('=');
                    game_version = split[1];
                    isGameVersionFound = true;
                }

                if(arg.Contains("region"))
                {
                    string[] split = arg.Split('=');
                    region = split[1];
                    isRegionFound = true;
                }

                if (isProviderFound && isGameVersionFound && isRegionFound)
                {
                    serverSetupData = new ServerSetupData()
                    {
                        region = region,
                        provider = provider,
                        gameVersion = game_version
                    };
                    break;
                }
            }
            return serverSetupData;
        }
    }
}
