// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using HybridWebSocket;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

#if !DISABLESTEAMWORKS

#endif
namespace AccelByte.Models
{
    [DataContract]
    public class AllianceRule
    {
        [DataMember] public int max_number { get; set; }
        [DataMember] public int min_number { get; set; }
        [DataMember] public int player_max_number { get; set; }
        [DataMember] public int player_min_number { get; set; }
    }

    [DataContract]
    public class FlexingRule
    {
        [DataMember] public string attribute { get; set; }
        [DataMember] public string criteria { get; set; }
        [DataMember] public int duration { get; set; }
        [DataMember] public int reference { get; set; }
    }

    public enum MatchingRuleCriteria
    {
        none,
        distance
    }

    [DataContract]
    public class MatchingRule
    {
        [DataMember] public string attribute { get; set; }
        [DataMember] public string criteria { get; set; }
        [DataMember] public int reference { get; set; }
    }

    [DataContract]
    public class RuleSet
    {
        [DataMember] public AllianceRule alliance { get; set; }
        [DataMember] public FlexingRule[] flexing_rule { get; set; }
        [DataMember] public MatchingRule[] matching_rule { get; set; }
    }

    [DataContract]
    public class CreateChannelRequest
    {
        [DataMember] public string description { get; set; }
        [DataMember] public uint find_match_timeout_seconds { get; set; }
        [DataMember] public string game_mode { get; set; }
        [DataMember] public RuleSet rule_set { get; set; }
        [DataMember] public bool joinable { get; set; }
    }

    [DataContract]
    public class CreateChannelResponse
    {
        [DataMember] public string description { get; set; }
        [DataMember] public uint find_match_timeout_seconds { get; set; }
        [DataMember] public string game_mode { get; set; }
        [DataMember] public RuleSet rule_set { get; set; }
        [DataMember] public bool joinable { get; set; }
        [DataMember(Name = "namespace")] public string namespace_ { get; set; }
        [DataMember] public string deployment { get; set; }
        [DataMember] public string slug { get; set; }
        [DataMember] public bool social_matchmaking { get; set; }
        [DataMember] public uint session_queue_timeout_seconds { get; set; }
        [DataMember] public DateTime updated_at { get; set; }
    }
}

namespace Tests.IntegrationTests
{

    public class MatchServer
    {
        private readonly DedicatedServer server = AccelByteServerPlugin.GetDedicatedServer();
        private readonly DedicatedServerManager serverManager = AccelByteServerPlugin.GetDedicatedServerManager();
        private readonly ServerMatchmaking matchmaking = AccelByteServerPlugin.GetMatchmaking();
        private string name;

        public IEnumerator Login(ResultCallback callback)
        {
            Result result = null;
            this.server.LoginWithClientCredentials(r => result = r);
            yield return new WaitUntil(() => result != null);
            
            callback.Try(result);
        }
        
        public IEnumerator RegisterLocal(string serverName, string localIP, ResultCallback callback)
        {
            Result result = null;
            this.serverManager.RegisterLocalServer(localIP, 7777, serverName, r => result = r);
            yield return TestHelper.WaitUntil(() => result != null);

            this.name = result.IsError ? null : serverName;

            callback.Try(result);
        }

        public IEnumerator GetSessionStatus(ResultCallback<MatchmakingResult> callback)
        {
            Result<ServerSessionResponse> sessionIdResult = null;
            AccelByteServerPlugin.GetDedicatedServerManager().GetSessionId(r => sessionIdResult = r);
            yield return TestHelper.WaitUntil(() => sessionIdResult != null);

            Result<MatchmakingResult> sessionStatusResult = null;
            AccelByteServerPlugin.GetMatchmaking()
                .QuerySessionStatus(sessionIdResult.Value.session_id, r => sessionStatusResult = r);
            yield return TestHelper.WaitUntil(() => sessionStatusResult != null);

            callback.Try(sessionStatusResult);
        }
         
        public IEnumerator Deregister(ResultCallback callback)
        {
            if (string.IsNullOrEmpty(this.name))
            {
                callback.TryOk();
                yield break;
            }
            
            Result deregisterResult = null;
            this.serverManager.DeregisterLocalServer(r => deregisterResult = r);
            yield return TestHelper.WaitUntil(() => deregisterResult != null);
            
            callback.Try(deregisterResult);
        }
    }

    public class MatchmakingAdmin
    {
        private readonly string baseUrl;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly AccelByteHttpClient httpClient = new AccelByteHttpClient();
        private readonly TestHelper testHelper = new TestHelper();

        private string clientAccessToken;
        private string channelName;

        public MatchmakingAdmin(string baseUrl, string clientId, string clientSecret)
        {
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(baseUrl));
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(clientId));
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(clientSecret));
            UnityEngine.Assertions.Assert.IsTrue(Uri.IsWellFormedUriString(baseUrl, UriKind.RelativeOrAbsolute));
            this.baseUrl = baseUrl;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
        }

        public IEnumerator CreateChannel(RuleSet ruleSet, bool joinable, ResultCallback<CreateChannelResponse> callback)
        {
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(this.clientAccessToken));
            this.channelName = TestHelper.GenerateUnique("unitySdkTestChannel");
            var requestBody = new CreateChannelRequest
            {
                rule_set = ruleSet,
                joinable = joinable,
                description = channelName,
                game_mode = channelName,
                find_match_timeout_seconds = 60
            };
            
            UnityWebRequest httpRequest = HttpRequestBuilder
                .CreatePost(this.baseUrl + "/matchmaking/namespaces/{namespace}/channels")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithBearerAuth(this.clientAccessToken)
                .WithContentType(MediaType.ApplicationJson)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult()
                .GetUnityWebRequest();

            yield return httpRequest.SendWebRequest();

            var result = httpRequest.GetHttpResponse().TryParseJson<CreateChannelResponse>();
            
            callback.Try(result);
        }

        public IEnumerator DeleteChannel(ResultCallback callback)
        {
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(this.clientAccessToken));

            if (string.IsNullOrEmpty(this.channelName))
            {
                callback.TryOk();
                yield break;
            }

            UnityWebRequest request = HttpRequestBuilder
                .CreateDelete(this.baseUrl + "/matchmaking/namespaces/{namespace}/channels/{channel}")
                .WithPathParam("namespace", AccelBytePlugin.Config.Namespace)
                .WithPathParam("channel", this.channelName)
                .WithBearerAuth(this.clientAccessToken)
                .GetResult()
                .GetUnityWebRequest();

            yield return request.SendWebRequest();

            Result result = request.GetHttpResponse().TryParse();
            this.channelName = null;
                
            callback.Try(result);
        }

        public IEnumerator Login(ResultCallback<TokenData> callback)
        {
            IHttpRequest request = HttpRequestBuilder.CreatePost(this.baseUrl + "/iam/v3/oauth/token")
                .WithBasicAuth(this.clientId, this.clientSecret)
                .WithContentType(MediaType.ApplicationForm)
                .Accepts(MediaType.ApplicationJson)
                .WithFormParam("grant_type", "client_credentials")
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<TokenData>();
            this.clientAccessToken = result.Value.access_token;
            callback.Try(result);
        }

        public IEnumerator DeleteClient(MatchmakingClient client, ResultCallback callback)
        {
            Result<UserData> userDataResult = null;
            
            yield return client.GetData(r => userDataResult = r);

            Result deleteResult = null;
            this.testHelper.DeleteUser(userDataResult.Value.userId, r => deleteResult = r);
            
            yield return new WaitUntil(() => deleteResult != null);
            
            callback.Try(deleteResult);
        }
    }
    
    
    public class MatchmakingRequest
    {
        public string ChannelName;
        public string ServerName;
        public string ClientVersion = "";
        public Dictionary<string, int> PreferredLatencies;
        public Dictionary<string, object> PartyAttributes;
        public string[] TempPartyMembers;
        public string[]  ExtraAttributes;
    }
    
    public enum MatchmakingStatus
    {
        NotInitialized,
        Initialized,
        Started,
        Rematchmaking,
        Done,
        ReceivedDsNotif,
        MatchmakingError,
        InParty
    }

    public class MatchmakingClient
    {
        public bool Connected => this.lobbyClient.IsConnected;
        public MatchmakingStatus Status { get; private set; } = MatchmakingStatus.NotInitialized;
        public int RematchmakingBanDuration { get; private set; }
        public DsNotif DsNotif { get; private set; }

        public string UserId => this.session.UserId;
        public string MatchId { get; private set; }

        private readonly LoginSession session;
        private readonly User user;
        private readonly Lobby lobbyClient;
        
        private string partyId;
        private bool matchmakingWithTempParty;

        public MatchmakingClient(Config config, AccelByteHttpClient httpClient, CoroutineRunner coroutineRunner)
        {
            UnityEngine.Assertions.Assert.IsNotNull(config);
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(config.IamServerUrl));
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(config.Namespace));
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(config.ClientId));
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(config.ClientSecret));
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(config.RedirectUri));
            UnityEngine.Assertions.Assert.IsNotNull(httpClient);
            UnityEngine.Assertions.Assert.IsNotNull(coroutineRunner);
            
            this.session = new LoginSession(
                config.IamServerUrl,
                config.Namespace,
                config.RedirectUri,
                httpClient,
                coroutineRunner);

            var userAccount = new UserAccount(
                config.IamServerUrl,
                config.Namespace,
                this.session,
                httpClient);

            this.user = new User(
                this.session,
                userAccount,
                coroutineRunner);
            
            var webSocket = new WebSocket();

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROXY_SERVER")))
            {
                webSocket.SetProxy("http://" + Environment.GetEnvironmentVariable("PROXY_SERVER"), "", "");
            }

            this.lobbyClient = new Lobby(
                config.LobbyServerUrl,
                webSocket,
                new LobbyApi(config.BaseUrl, httpClient),
                this.session,
                config.Namespace,
                coroutineRunner);
        }

        public IEnumerator Initialize()
        {
            if (this.session.IsValid())
            {
                yield break;
            }
            
            var guid = Guid.NewGuid().ToString("N");
            string username = $"lobbyuser+{guid}@example.com";
            string password = "Password123";
            string displayName = "lobbyuser" + guid;
            DateTime dateOfBirth = DateTime.Now.AddYears(-22);
            Result<RegisterUserResponse> registerResult = null;
            this.user.Register(username, password, displayName, "US", dateOfBirth, result => registerResult = result);
            yield return TestHelper.WaitUntil(() => registerResult != null);

            Result loginResult = null;
            this.user.LoginWithUsername(username, password, result => loginResult = result);
            yield return TestHelper.WaitUntil(() => loginResult != null);

            this.Status = MatchmakingStatus.Initialized;
        }

        public void Connect()
        {
            this.SubscribeEvents();
            this.lobbyClient.Connect();
        }

        public void Disconnect()
        {
            this.UnsubscribeEvents();            
            this.lobbyClient.Disconnect();
        }

        public IEnumerator GetData(ResultCallback<UserData> callback)
        {
            Result<UserData> result = null;
            this.user.GetData(r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }
        
        public IEnumerator SetSessionAttribute(string key, string value, ResultCallback callback)
        {
            Result result = null;
            this.lobbyClient.SetSessionAttribute(key, value, r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }
        
        public IEnumerator CreateParty(ResultCallback<PartyInfo> callback)
        {
            Result<PartyInfo> result = null;
            this.lobbyClient.CreateParty(r => result = r);

            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator InviteToParty(string userId, ResultCallback callback)
        {
            Result result = null;
            this.lobbyClient.InviteToParty(userId, r => result = r);

            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator JoinParty(PartyInfo partyInfo, ResultCallback<PartyInfo> callback)
        {
            Result<PartyInfo> result = null;
            this.lobbyClient.JoinParty(partyInfo.partyID, partyInfo.invitationToken, r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator LeaveParty(ResultCallback callback)
        {
            Result result = null;
            this.lobbyClient.LeaveParty(r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);

            this.Status = result.IsError ? MatchmakingStatus.MatchmakingError : MatchmakingStatus.Initialized;
            
            callback.Try(result);
        }
        
        public IEnumerator StartMatchmaking(string channelName, ResultCallback<MatchmakingCode> callback)
        {
            Result<MatchmakingCode> result = null;
            this.lobbyClient.StartMatchmaking(channelName, r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);

            this.Status = result.IsError ? MatchmakingStatus.MatchmakingError : MatchmakingStatus.Started;
            
            callback.Try(result);
        }

        public IEnumerator StartMatchmaking(string channelName, string serverName, ResultCallback<MatchmakingCode> callback)
        {
            Result<MatchmakingCode> result = null;
            this.lobbyClient.StartMatchmaking(channelName, serverName, r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);

            this.Status = result.IsError ? MatchmakingStatus.MatchmakingError : MatchmakingStatus.Started;
            
            callback.Try(result);
        }
        
        public IEnumerator StartMatchmaking(MatchmakingRequest request,
            ResultCallback<MatchmakingCode> callback)
        {
            Result<MatchmakingCode> result = null;
            this.lobbyClient.StartMatchmaking(
                request.ChannelName,
                request.ServerName,
                request.ClientVersion,
                request.PreferredLatencies,
                request.PartyAttributes,
                request.TempPartyMembers?.ToArray(),
                request.ExtraAttributes?.ToArray(),
                r => result = r);

            yield return TestHelper.WaitUntil(() => result != null);

            this.Status = result.IsError ? MatchmakingStatus.MatchmakingError : MatchmakingStatus.Started;
            this.matchmakingWithTempParty = request.TempPartyMembers != null && request.TempPartyMembers.Length > 0; 

            callback.Try(result);
        }

        public IEnumerator CancelMatchmaking(ResultCallback<MatchmakingCode> callback)
        {
            Result<MatchmakingCode> result = null;
            this.lobbyClient.CancelMatchmaking(this.MatchId, this.matchmakingWithTempParty, r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);
            
            callback.Try(result);
        }

        public IEnumerator ReadyConsent(ResultCallback callback)
        {
            Result result = null;
            this.lobbyClient.ConfirmReadyForMatch(this.MatchId, r => result = r);
            
            yield return TestHelper.WaitUntil(() => result != null);

            callback.Try(result);
        }

        private void SubscribeEvents()
        {
            this.lobbyClient.MatchmakingCompleted += this.OnMatchmakingCompleted;
            this.lobbyClient.RematchmakingNotif += this.OnRematchmakingNotif;
            this.lobbyClient.DSUpdated += this.OnDsUpdated;
        }

        private void UnsubscribeEvents()
        {
            this.lobbyClient.MatchmakingCompleted -= this.OnMatchmakingCompleted;
            this.lobbyClient.RematchmakingNotif -= this.OnRematchmakingNotif;
            this.lobbyClient.DSUpdated -= this.OnDsUpdated;
        }

        private void OnMatchmakingCompleted(Result<MatchmakingNotif> result)
        {
            if (result.IsError)
            {
                this.Status = MatchmakingStatus.MatchmakingError;
            }
            else
            {
                switch (result.Value.status)
                {
                    case "done":
                        this.MatchId = result.Value.matchId;
                        this.Status = MatchmakingStatus.Done;
                        break;
                    case "timeout":
                        this.Status = MatchmakingStatus.MatchmakingError;
                        break;
                }
            }
        }

        private void OnRematchmakingNotif(Result<RematchmakingNotification> result)
        {
            if (result.IsError)
            {
                this.Status = MatchmakingStatus.MatchmakingError;
            }
            else
            {
                this.RematchmakingBanDuration = result.Value.banDuration;
                this.Status = MatchmakingStatus.Rematchmaking;
            }
        }

        private void OnDsUpdated(Result<DsNotif> result)
        {
            if (result.IsError)
            {
                this.Status = MatchmakingStatus.MatchmakingError;
            }
            else
            {
                switch (result.Value.status)
                {
                    case "BUSY":
                    case "READY":
                        this.DsNotif = result.Value;
                        this.Status = MatchmakingStatus.ReceivedDsNotif;
                        break;
                }
            }
        }
    }

    [TestFixture]
    public class MatchmakingTest
    {
        private const int UserCount = 4;
        
        private readonly List<MatchmakingClient> clients = new List<MatchmakingClient>();
        private readonly MatchmakingAdmin admin;
        private readonly  MatchServer matchServer;
        
        private Dictionary<string, int> preferedLatencies;
        private bool shouldSetupFixture = true;
        private bool shouldTearDownFixture = false;

        public MatchmakingTest()
        {
            var coroutineRunner = new CoroutineRunner();
            var httpWorker = new AccelByteHttpClient();
            httpWorker.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);

            for (int i = 0; i < MatchmakingTest.UserCount; i++)
            {
                var client = new MatchmakingClient(AccelBytePlugin.Config, httpWorker, coroutineRunner);
                this.clients.Add(client);
            }

            string adminBaseUrl = Environment.GetEnvironmentVariable("ADMIN_BASE_URL");
            string adminClientId = Environment.GetEnvironmentVariable("ADMIN_CLIENT_ID");
            string adminClientSecret = Environment.GetEnvironmentVariable("ADMIN_CLIENT_SECRET");
            this.admin = new MatchmakingAdmin(adminBaseUrl, adminClientId, adminClientSecret);
            this.matchServer = new MatchServer();
        }

        [UnitySetUp]
        private IEnumerator Setup()
        {
            if (this.shouldSetupFixture)
            {
                foreach (var client in this.clients)
                {
                    yield return client.Initialize();
                }

                Result<TokenData> adminLoginResult = null;
                yield return this.admin.Login(r => adminLoginResult = r);

                Result matchServerLoginResult = null;
                yield return this.matchServer.Login(r => matchServerLoginResult = r);

                string preferedDSRegion = Environment.GetEnvironmentVariable("PREFERED_DS_REGION");
                // setup prefered region
                if (preferedDSRegion != null)
                {
                    Debug.Log("Setup prefered DS region: " + preferedDSRegion);
                    var qos = AccelBytePlugin.GetQos();
                    Result<Dictionary<string, int>> getLatenciesResult = null;
                    qos.GetServerLatencies(result => getLatenciesResult = result);
                    yield return TestHelper.WaitForValue(() => getLatenciesResult);

                    if (!getLatenciesResult.IsError)
                    {
                        preferedLatencies = getLatenciesResult.Value;
                        if (preferedLatencies.ContainsKey(preferedDSRegion))
                        {
                            preferedLatencies[preferedDSRegion] = 1;
                        }
                        else
                        {
                            Debug.Log("Prefered DS region is not available: " + preferedDSRegion);
                        }
                    }
                    else
                    {
                        Debug.Log("Unable to get qos latencies");
                    }
                }

                // setup dsm config
                Result<TestHelper.DSMConfig> getDSMConfigResult = null;
                TestHelper.DSMConfig dsmConfig = null;
                bool isUpdateDsmConfig = false;

                // get current DSM config for checking
                var helper = new TestHelper();
                helper.GetDsmConfig(AccelBytePlugin.Config.Namespace, result => getDSMConfigResult = result);
                yield return TestHelper.WaitForValue(() => getDSMConfigResult);

                if (getDSMConfigResult.IsError)
                {
                    // if dsm config not set yet (probably new namespace) then create one.
                    if (getDSMConfigResult.Error.Code == ErrorCode.DedicatedServerConfigNotFound)
                    {
                        dsmConfig = GetNewDSMConfig();
                        isUpdateDsmConfig = true;
                    }
                }
                else
                {
                    // if dsm config available, check if named ports used for tests are also available
                    dsmConfig = getDSMConfigResult.Value;
                    var customPorts = GetNewCustomPorts();
                    foreach (var port in customPorts)
                    {
                        // if named port for testing not availble, then add it
                        if (!dsmConfig.ports.ContainsKey(port.Key))
                        {
                            dsmConfig.ports.Add(port.Key, port.Value);
                            isUpdateDsmConfig = true;
                        }
                    }
                }

                // update the dsm config
                if (isUpdateDsmConfig && dsmConfig != null)
                {
                    Result setDSMConfigResult = null;
                    helper.SetDsmConfig(dsmConfig, result => setDSMConfigResult = result);
                    yield return TestHelper.WaitForValue(() => setDSMConfigResult);
                    TestHelper.Assert.IsResultOk(setDSMConfigResult, "Set DSM Config result");
                }

                this.shouldSetupFixture = false;
            }

            foreach (var client in this.clients.Where(client => !client.Connected))
            {
                client.Connect();
            }
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            foreach (var client in this.clients.Where(client => client.Connected))
            {
                yield return client.CancelMatchmaking(r => { });
                yield return client.LeaveParty(r => { });
                client.Disconnect();
            }

            yield return this.admin.DeleteChannel(r => { });
            yield return this.matchServer.Deregister(result => { });

            if (this.shouldTearDownFixture)
            {
                foreach (var client in this.clients)
                {
                    yield return this.admin.DeleteClient(client, null);
                }

                this.clients.Clear();
                this.shouldTearDownFixture = false;
            }
        }

        private TestHelper.DSMConfig GetNewDSMConfig()
        {
            Dictionary<string, string> versionMapping = new Dictionary<string, string>();
            versionMapping.Add("default", "no_image");

            return new TestHelper.DSMConfig()
            {
                namespace_ = AccelBytePlugin.Config.Namespace,
                providers = new string[] {"aws"},
                port = 1000,
                creation_timeout = 60,
                claim_timeout = 120,
                session_timeout = 1800,
                heartbeat_timeout = 30,
                unreachable_timeout = 3600,
                image_version_mapping = versionMapping,
                default_version = "default",
                cpu_limit = 200,
                mem_limit = 256,
                min_count = 0,
                max_count = 3,
                buffer_count = 0,
                allow_version_override = false,
                ports = GetNewCustomPorts(),
                protocol = "udp"
            };
        }

        private Dictionary<string, int> GetNewCustomPorts()
        {
            Dictionary<string, int> customPorts = new Dictionary<string, int>();
            customPorts.Add("custom", 1001);
            customPorts.Add("custom2", 1002);

            return customPorts;
        }

        [UnityTest, TestLog, Order(int.MaxValue), Timeout(300000)]
        public IEnumerator EndTests()
        {
            this.shouldTearDownFixture = true;
            yield break;
        }

        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator StartMatchmaking_ReturnOk()
        {
            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, r => createChannelResult = r);

            yield return this.clients[0].CreateParty(result => { });
            yield return this.clients[1].CreateParty(result => { });

            string channelName = createChannelResult.Value.game_mode;
            yield return this.clients[0].StartMatchmaking(channelName, result => { });
            yield return this.clients[1].StartMatchmaking(channelName, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[1].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
        }
        
        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator StartMatchmaking_CustomPortCheck_ReturnOk()
        {
            string[] customPortNames = {"custom", "custom2"};
        
            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, r => createChannelResult = r);

            yield return this.clients[0].CreateParty(result => { });
            yield return this.clients[1].CreateParty(result => { });

            string channelName = createChannelResult.Value.game_mode;
            yield return this.clients[0].StartMatchmaking(channelName, result => { });
            yield return this.clients[1].StartMatchmaking(channelName, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif);

            Assert.That(this.clients[0].DsNotif.ports.Keys, Is.EquivalentTo(customPortNames));
        }
        
        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator Rematchmaking_ReturnOk()
        {
            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, r => createChannelResult = r);

            yield return this.clients[0].CreateParty(result => { });
            yield return this.clients[1].CreateParty(result => { });

            string channelName = createChannelResult.Value.game_mode;
            yield return this.clients[0].StartMatchmaking(channelName, result => { });
            yield return this.clients[1].StartMatchmaking(channelName, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Rematchmaking &&
                    this.clients[1].Status == MatchmakingStatus.Rematchmaking);

            yield return this.clients[2].CreateParty(result => { });
            yield return this.clients[2].StartMatchmaking(channelName, r => { });
            
            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[2].Status == MatchmakingStatus.Done);
            
            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[2].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[2].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[2].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
        }
        
        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator CancelMatchmaking_ReturnOk()
        {
            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, result => createChannelResult = result);

            yield return this.clients[0].CreateParty(r => { });

            string channelName = createChannelResult.Value.game_mode;
            yield return this.clients[0].StartMatchmaking(channelName, r => { });

            Result<MatchmakingCode> cancelResult = null;
            yield return this.clients[0].CancelMatchmaking(result => cancelResult = result);

            Assert.That(cancelResult.IsError, Is.False);
       }
        
        private static string GetLocalIp()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }

        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator StartMatchmakingWithLocalDS_ReturnOk()
        {
            var localIP = MatchmakingTest.GetLocalIp();
            string serverName = "unitylocalds_manual_" + Guid.NewGuid();
            yield return this.matchServer.RegisterLocal(serverName, localIP, result => {});

            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };
            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, r => createChannelResult = r);

            yield return this.clients[0].CreateParty(result => { });
            yield return this.clients[1].CreateParty(result => { });

            string channelName = createChannelResult.Value.game_mode;
            yield return this.clients[0].StartMatchmaking(channelName, serverName, result => { });
            yield return this.clients[1].StartMatchmaking(channelName, serverName, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });
            
            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[1].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
        }

        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator StartMatchmaking_WithLatencies_MatchFoundWithSameIP()
        {
            var qos = AccelBytePlugin.GetQos();
            Dictionary<string, int> latencies = null;
            qos.GetServerLatencies(result => latencies = result.Value);
            yield return TestHelper.WaitForValue(() => latencies);
        
            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };
            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, r => createChannelResult = r);

            yield return this.clients[0].CreateParty(result => { });
            yield return this.clients[1].CreateParty(result => { });

            var matchmakingRequest = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                PreferredLatencies = latencies
            };
            
            yield return this.clients[0].StartMatchmaking(matchmakingRequest, result => { });
            yield return this.clients[1].StartMatchmaking(matchmakingRequest, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });
            
            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].DsNotif.ip, Is.EqualTo(this.clients[1].DsNotif.ip));
        }
        
        [UnityTest, TestLog, Order(1), Timeout(100000)]
        public IEnumerator StartMatchmaking_WithPartyAttributes_ReturnOk()
        {
            var localIP = MatchmakingTest.GetLocalIp();
            string serverName = "unitylocalds_manual_" + Guid.NewGuid();
            yield return this.matchServer.RegisterLocal(serverName, localIP, result => {});

            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };
            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, r => createChannelResult = r);

            yield return this.clients[0].CreateParty(result => { });
            yield return this.clients[1].CreateParty(result => { });

            Dictionary<string, object> partyAttributes = new Dictionary<string, object>();
            partyAttributes.Add("GameMap", "BasicTutorial");
            partyAttributes.Add("Timeout", "100");
            partyAttributes.Add("Label", "New Island");
        
            var matchmakingRequest = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                ServerName = serverName,
                PartyAttributes = partyAttributes
            };

            yield return this.clients[0].StartMatchmaking(matchmakingRequest, result => { });
            yield return this.clients[1].StartMatchmaking(matchmakingRequest, result => { });

            yield return new WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });
            
            yield return new WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Result<MatchmakingResult> sessionStatusResult = null;
            yield return this.matchServer.GetSessionStatus(result => sessionStatusResult = result);

            foreach (var ally in sessionStatusResult.Value.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    Assert.That(partyAttributes, Is.SubsetOf(party.party_attributes));
                }
            }

            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[1].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            
            yield return null;
        }
        
        [UnityTest, TestLog, Order(2), Timeout(180000)]
        public IEnumerator StartMatchmaking_WithTempParty_ReturnOk()
        {
            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, r => createChannelResult = r);

            var request0 = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                TempPartyMembers = new []{ this.clients[0].UserId }
            };
            
            yield return this.clients[0].StartMatchmaking(request0, result => { });
            
            var request1 = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                TempPartyMembers = new []{ this.clients[1].UserId }
            };
            
            yield return this.clients[1].StartMatchmaking(request1, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[1].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
        }
        
        [UnityTest, TestLog, Order(2), Timeout(180000)]
        public IEnumerator StartMatchmaking_WithTempParty_2PersonPerParty_ReturnOk()
        {
            var ruleSet2X2 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 2, player_max_number = 2
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet2X2, false, r => createChannelResult = r);

            var request0 = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                TempPartyMembers = new []{ this.clients[0].UserId, this.clients[2].UserId }
            };
            
            yield return this.clients[0].StartMatchmaking(request0, result => { });
            
            var request1 = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                TempPartyMembers = new []{ this.clients[1].UserId, this.clients[3].UserId }
            };
            
            yield return this.clients[1].StartMatchmaking(request1, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done &&
                    this.clients[2].Status == MatchmakingStatus.Done &&
                    this.clients[3].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });
            yield return this.clients[2].ReadyConsent(result => { });
            yield return this.clients[3].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[2].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[3].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[1].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[2].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[3].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
        }
        
        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator CancelMatchmaking_WithTempParty_ReturnOk()
        {
            var ruleSet1X1 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 1, player_max_number = 1
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet1X1, false, result => createChannelResult = result);

            var request = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                TempPartyMembers = new []{ this.clients[0].UserId },
            };
            
            yield return this.clients[0].StartMatchmaking(request, result => { });

            Result<MatchmakingCode> cancelResult = null;
            yield return this.clients[0].CancelMatchmaking(result => cancelResult = result);

            Assert.That(cancelResult.IsError, Is.False);
        }
        
        [UnityTest, TestLog, Order(2), Timeout(180000)]
        public IEnumerator StartMatchmaking_WithExtraAttributes_ReturnsOk()
        {
            var localIP = MatchmakingTest.GetLocalIp();
            string serverName = "unitylocalds_manual_" + Guid.NewGuid();
            yield return this.matchServer.RegisterLocal(serverName, localIP, result => {});

            var ruleSet2X2 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 2, player_max_number = 2
                },
                matching_rule = new []
                {
                    new MatchingRule
                    {
                        attribute = "mmr",
                        criteria = MatchingRuleCriteria.distance.ToString().ToLower(),
                        reference = 10,
                    }
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet2X2, false, result => createChannelResult = result);

            yield return this.clients[0].SetSessionAttribute("mmr", "120", result => {});
            yield return this.clients[1].SetSessionAttribute("mmr", "121", result => {});
            yield return this.clients[2].SetSessionAttribute("mmr", "122", result => {});
            yield return this.clients[3].SetSessionAttribute("mmr", "123", result => {});

            PartyInfo partyA = null;
            yield return this.clients[0].CreateParty(result => partyA = result.Value);
            yield return this.clients[0].InviteToParty(this.clients[2].UserId, result => {});

            PartyInfo partyB = null;
            yield return this.clients[1].CreateParty(result => partyB = result.Value);
            yield return this.clients[1].InviteToParty(this.clients[3].UserId, result => {});

            yield return this.clients[2].JoinParty(partyA, result => { });
            yield return this.clients[3].JoinParty(partyB, result => { });

            var partyAttributes = new Dictionary<string, object>
            {
                {"GameMap", "BasicTutorial"}, 
                {"Timeout", "100"}, 
                {"Label", "New Island"}
            };

            var request = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                ServerName = serverName,
                PartyAttributes = partyAttributes,
                ExtraAttributes = new []{ "mmr" }
            };
            
            yield return this.clients[0].StartMatchmaking(request, result => { });
            yield return this.clients[1].StartMatchmaking(request, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done &&
                    this.clients[2].Status == MatchmakingStatus.Done &&
                    this.clients[3].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });
            yield return this.clients[2].ReadyConsent(result => { });
            yield return this.clients[3].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[2].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[3].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[1].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[2].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[3].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));

            MatchmakingResult sessionStatus = null;
            yield return this.matchServer.GetSessionStatus(result => sessionStatus = result.Value);

            foreach (var ally in sessionStatus.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    Assert.That(partyAttributes, Is.SubsetOf(party.party_attributes));
                }
            }
        }
        
        [UnityTest, TestLog, Order(2), Timeout(180000)]
        public IEnumerator StartMatchmaking_WithExtraAttributes_NoPartySetAttribute_ReturnsOk()
        {
            var localIP = MatchmakingTest.GetLocalIp();
            string serverName = "unitylocalds_manual_" + Guid.NewGuid();
            yield return this.matchServer.RegisterLocal(serverName, localIP, result => {});

            var ruleSet2X2 = new RuleSet
            {
                alliance = new AllianceRule
                {
                    min_number = 2, max_number = 2, player_min_number = 2, player_max_number = 2
                },
                matching_rule = new []
                {
                    new MatchingRule
                    {
                        attribute = "mmr",
                        criteria = MatchingRuleCriteria.distance.ToString().ToLower(),
                        reference = 10,
                    }
                }
            };

            Result<CreateChannelResponse> createChannelResult = null;
            yield return this.admin.CreateChannel(ruleSet2X2, false, result => createChannelResult = result);

            PartyInfo partyA = null;
            yield return this.clients[0].CreateParty(result => partyA = result.Value);
            yield return this.clients[0].InviteToParty(this.clients[2].UserId, result => {});

            PartyInfo partyB = null;
            yield return this.clients[1].CreateParty(result => partyB = result.Value);
            yield return this.clients[1].InviteToParty(this.clients[3].UserId, result => {});

            yield return this.clients[2].JoinParty(partyA, result => { });
            yield return this.clients[3].JoinParty(partyB, result => { });

            var partyAttributes = new Dictionary<string, object>
            {
                {"GameMap", "BasicTutorial"}, 
                {"Timeout", "100"}, 
                {"Label", "New Island"}
            };

            var request = new MatchmakingRequest
            {
                ChannelName = createChannelResult.Value.game_mode,
                ServerName = serverName,
                PartyAttributes = partyAttributes,
                ExtraAttributes = new []{ "mmr" }
            };
            
            yield return this.clients[0].StartMatchmaking(request, result => { });
            yield return this.clients[1].StartMatchmaking(request, result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.Done &&
                    this.clients[1].Status == MatchmakingStatus.Done &&
                    this.clients[2].Status == MatchmakingStatus.Done &&
                    this.clients[3].Status == MatchmakingStatus.Done);

            yield return this.clients[0].ReadyConsent(result => { });
            yield return this.clients[1].ReadyConsent(result => { });
            yield return this.clients[2].ReadyConsent(result => { });
            yield return this.clients[3].ReadyConsent(result => { });

            yield return TestHelper.WaitUntil(
                () => 
                    this.clients[0].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[1].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[2].Status == MatchmakingStatus.ReceivedDsNotif &&
                    this.clients[3].Status == MatchmakingStatus.ReceivedDsNotif);
            
            Assert.That(this.clients[0].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[1].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[2].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));
            Assert.That(this.clients[3].Status, Is.EqualTo(MatchmakingStatus.ReceivedDsNotif));

            MatchmakingResult sessionStatus = null;
            yield return this.matchServer.GetSessionStatus(result => sessionStatus = result.Value);
            
            foreach (var ally in sessionStatus.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    Assert.That(partyAttributes, Is.SubsetOf(party.party_attributes));
                }
            }
        }
    }

}
