// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using HybridWebSocket;
using NUnit.Framework;
using Steamworks;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

#if !DISABLESTEAMWORKS

#endif

namespace Tests.IntegrationTests
{
    static class LobbyTestUtil
    {
        public static Lobby CreateLobby(ISession session, AccelByteHttpClient httpClient, CoroutineRunner coroutineRunner)
        {
            var webSocket = new WebSocket();

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROXY_SERVER")))
            {
                webSocket.SetProxy("http://" + Environment.GetEnvironmentVariable("PROXY_SERVER"), "", "");
            }
            return new Lobby(AccelBytePlugin.Config.LobbyServerUrl, webSocket, new LobbyApi(AccelBytePlugin.Config.BaseUrl, httpClient), session, AccelBytePlugin.Config.Namespace, coroutineRunner);
        }
    }
    
    [TestFixture]
    public class LobbyTest
    {
        private int NumSetUp;
        private int NumTearDown;

        private TestHelper helper;
        private User[] users;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private UserData[] usersData;
        private const int userCount = 10;
        private string preferedDSRegion = Environment.GetEnvironmentVariable("PREFERED_DS_REGION");
        private Dictionary<string, int> preferedLatencies;
        private List<Lobby> activeLobbies = new List<Lobby>();

        Lobby CreateLobby(ISession session)
        {
            Lobby lobby = LobbyTestUtil.CreateLobby(session, this.httpClient, coroutineRunner);
            activeLobbies.Add(lobby);
            return lobby;
        }

        [UnityTearDown]
        public IEnumerator TearDown() 
        {
            foreach (var lobby in activeLobbies)
            {
                if (lobby.IsConnected)
                {
                    Debug.LogWarning("[LOBBY] Dangling websocket connection, previous test are not closing WS connection, please disconnect it at the of of the tests, disconnecting...");
                    lobby.Disconnect();
                }
            }
            activeLobbies.Clear();
            yield return null;
        }

        [UnitySetUp]
        private IEnumerator TestSetup()
        {
            if (this.httpClient == null)
            {
                this.httpClient = new AccelByteHttpClient();
                this.httpClient.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            }

            if (this.coroutineRunner == null)
            {
                this.coroutineRunner = new CoroutineRunner();
            }

            if (this.helper == null)
            {
                this.helper = new TestHelper();
            }

            if (this.users != null) yield break;

            var newUsers = new User[userCount];
            this.usersData = new UserData[userCount];
            var guid = Guid.NewGuid().ToString("N");

            for (int i = 0; i < newUsers.Length; i++)
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                        AccelBytePlugin.Config.IamServerUrl,
                        AccelBytePlugin.Config.Namespace,
                        AccelBytePlugin.Config.RedirectUri,
                        this.httpClient,
                        this.coroutineRunner);
            
                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    this.httpClient);

                newUsers[i] = new User(
                    loginSession,
                    userAccount,
                    this.coroutineRunner);

                newUsers[i]
                    .Register(
                        string.Format("lobbyuser+{0}{1}@example.com", i + 1, guid),
                        "Password123",
                        "lobbyuser" + (i + 1) + guid,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered lobbyuser" + (i + 1));
            }

            for (int i = 0; i < newUsers.Length; i++)
            {
                Result loginResult = null;

                newUsers[i]
                    .LoginWithUsername(
                        string.Format("lobbyuser+{0}{1}@example.com", i + 1, guid),
                        "Password123",
                        result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                Result<UserData> userResult = null;
                newUsers[i].GetData(r => userResult = r);
                yield return TestHelper.WaitForValue(() => userResult);

                this.usersData[i] = userResult.Value;
            
                TestHelper.LogResult(loginResult, "Setup: Logged in " + userResult.Value.displayName);
            }
            
            yield return new WaitForSeconds(0.1f);

            this.users = newUsers;

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
            helper.GetDsmConfig(AccelBytePlugin.Config.Namespace, result => getDSMConfigResult = result);
            yield return TestHelper.WaitForValue(() => getDSMConfigResult);

            if(getDSMConfigResult.IsError)
            {
                // if dsm config not set yet (probably new namespace) then create one.
                if(getDSMConfigResult.Error.Code == ErrorCode.DedicatedServerConfigNotFound)
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
                foreach(var port in customPorts)
                {
                    // if named port for testing not availble, then add it
                    if(!dsmConfig.ports.ContainsKey(port.Key))
                    {
                        dsmConfig.ports.Add(port.Key, port.Value);
                        isUpdateDsmConfig = true;
                    }
                }
            }

            // update the dsm config
            if(isUpdateDsmConfig && dsmConfig != null)
            {
                Result setDSMConfigResult = null;
                helper.SetDsmConfig(dsmConfig, result => setDSMConfigResult = result);
                yield return TestHelper.WaitForValue(() => setDSMConfigResult);
                TestHelper.Assert.IsResultOk(setDSMConfigResult, "Set DSM Config result");
            }
        }

        private TestHelper.DSMConfig GetNewDSMConfig()
        {
            Dictionary<string, string> versionMapping = new Dictionary<string, string>();
            versionMapping.Add("default", "no_image");

            return new TestHelper.DSMConfig()
            {
                namespace_ = AccelBytePlugin.Config.Namespace,
                providers = new string[] { "aws" },
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

        [UnityTest, TestLog, Order(99), Timeout(300000)]
        public IEnumerator TestTeardown()
        {
            for (int i = 0; i < this.users.Length; i++)
            {
                Result deleteResult = null;

                this.helper.DeleteUser(this.users[i], result => deleteResult = result);
                yield return TestHelper.WaitForValue(() => deleteResult);

                TestHelper.LogResult(deleteResult, "Setup: Deleted lobbyuser" + (i + 1));
                TestHelper.Assert.IsResultOk(deleteResult);
            }

            this.users = null;
        }
        
        [UnityTest, TestLog, Order(2), Timeout(100000), Ignore("Not working perfectly, need a proper way to test")]
        public IEnumerator Lobby_StopReconnectWhenNotReconnectable()
        {
            var lobby1A = CreateLobby(this.users[0].Session);
            var lobby1B = CreateLobby(this.users[0].Session);
            bool lobby1BDisconnected = false;
            
            lobby1A.Disconnected += (result) => Debug.Log("Lobby1A Disconnected. Code: " + result.ToString());
            lobby1B.Disconnected += (result) =>
            {
                lobby1BDisconnected = true;
                Debug.Log("Lobby1B Disconnected. Code: " + result.ToString());
            };
            
            lobby1A.Connect();
            yield return TestHelper.WaitUntil(() => lobby1A.IsConnected);
            lobby1B.Connect();
            yield return new WaitForSeconds(2);
            
            TestHelper.Assert.That(lobby1A.IsConnected);
            TestHelper.Assert.That(!lobby1B.IsConnected);
            TestHelper.Assert.That(lobby1BDisconnected);
            lobby1A.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(200000)]
        public IEnumerator SendPrivateChat_FromMultipleUsers_ChatReceived()
        {
            //Arrange
            var lobbies = new Lobby[this.users.Length];

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = CreateLobby(this.users[i].Session);

                lobbies[i].Connect();
            }

            int receivedChatCount = 0;

            lobbies[0].PersonalChatReceived += result =>
            {
                receivedChatCount++;
                Debug.Log(result.Value.payload);
            };

            //Act
            for (int i = 1; i < lobbies.Length; i++)
            {
                var userId = this.users[0].Session.UserId;
                var chatMessage = "Hello " + this.usersData[0].displayName + " from " + this.usersData[i].displayName;

                Result privateChatResult = null;
                lobbies[i].SendPersonalChat(userId, chatMessage, result => privateChatResult = result);

                yield return new WaitUntil(() => privateChatResult != null);

                Debug.Log(privateChatResult);
            }

            yield return new WaitUntil(() => receivedChatCount >= lobbies.Length - 1);

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            //Assert
            Assert.That(receivedChatCount, Is.GreaterThanOrEqualTo(lobbies.Length - 1));
        }

        [UnityTest, TestLog, Order(2), Timeout(200000)]
        public IEnumerator SendChannelChat_MultipleUsers_ChatReceived()
        {
            //Arrange
            var lobbies = new Lobby[this.users.Length];

            int receivedChatCount = 0;

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = CreateLobby(this.users[i].Session);

                lobbies[i].ChannelChatReceived += result =>
                {
                    Interlocked.Increment(ref receivedChatCount);
                    Debug.Log(result.Value.payload);
                };

                lobbies[i].Connect();

                if (i < lobbies.Length - 1)
                {
                    Result<ChatChannelSlug> joinResult = null;
                    lobbies[i].JoinDefaultChatChannel(result => joinResult = result);
                    yield return new WaitUntil(() => joinResult != null);
                }
            }

            //Act
            var userId = this.users[0].Session.UserId;
            var chatMessage = "Hello from " + this.usersData[0].displayName;

            Result channelChatResult = null;
            lobbies[0].SendChannelChat(chatMessage, result => channelChatResult = result);

            yield return new WaitUntil(() => channelChatResult != null);

            Debug.Log(channelChatResult);

            yield return new WaitUntil(() => receivedChatCount >= lobbies.Length - 1);

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            //Assert
            Assert.That(receivedChatCount, Is.GreaterThanOrEqualTo(lobbies.Length - 1));
        }

        const int chatMultiplier = 2;
        [UnityTest, TestLog, Order(2), Timeout(60000 + ( 10000 * chatMultiplier))]
        public IEnumerator SendChannelChat_BunchofPlayersSendChat()
        {
            //Arrange
            var lobbies = new Lobby[this.users.Length];

            int receivedChatCount = 0;

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = CreateLobby(this.users[i].Session);

                lobbies[i].ChannelChatReceived += result => Interlocked.Increment(ref receivedChatCount);

                lobbies[i].Connect();

                Result<ChatChannelSlug> joinResult = null;
                lobbies[i].JoinDefaultChatChannel(result => joinResult = result);
                yield return TestHelper.WaitForValue(() => joinResult);
            }

            //Act
            int chatSent = 0;
            for (int i = 0; i < chatMultiplier; i++)
            {
                for (int j = 0; j < lobbies.Length; j++)
                {
                    var userId = this.users[j].Session.UserId;
                    var chatMessage = "Hello from " + this.usersData[j].displayName;

                    lobbies[j].SendChannelChat(chatMessage, result => Interlocked.Increment(ref chatSent));
                }
            }

            yield return TestHelper.WaitUntil(() => chatSent == lobbies.Length * chatMultiplier, "chatSent");

            yield return TestHelper.WaitUntil(() => receivedChatCount == chatSent * lobbies.Length, "receivedChatCount");

            Debug.Log("Disconnecting chat lobbies");
            
            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            //Assert
            Assert.IsTrue(receivedChatCount == chatSent * lobbies.Length);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator SendChannelChat_SendUnescapedString()
        {
            //Arrange
            var lobbies = new Lobby[2];

            int receivedChatCount = 0;
            Result<ChannelChatMessage>[] chatReceived = new Result<ChannelChatMessage>[2] { null, null }; 

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = CreateLobby(this.users[i].Session);

                lobbies[i].Connect();

                Result<ChatChannelSlug> joinResult = null;
                lobbies[i].JoinDefaultChatChannel(result => joinResult = result);
                yield return new WaitUntil(() => joinResult != null);
            }

            lobbies[0].ChannelChatReceived += result =>
            {
                receivedChatCount++;
                chatReceived[0] = result;
                Debug.Log(result.Value.payload);
            };

            lobbies[1].ChannelChatReceived += result =>
            {
                receivedChatCount++;
                chatReceived[1] = result;
                Debug.Log(result.Value.payload);
            };

            //Act
            var userId = this.users[0].Session.UserId;
            var chatMessage = "{\",;:'\\|!@#$%^&*()_+-=`~?><.,";

            Result channelChatResult = null;
            lobbies[0].SendChannelChat(chatMessage, result => channelChatResult = result);

            yield return new WaitUntil(() => channelChatResult != null);

            Debug.Log(channelChatResult);

            yield return new WaitUntil(() => receivedChatCount >= lobbies.Length);

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            //Assert
            Assert.That(receivedChatCount, Is.GreaterThanOrEqualTo(lobbies.Length));
            Assert.That(chatReceived[0].Value.payload == chatMessage);
            Assert.That(chatReceived[1].Value.payload == chatMessage);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator SendChannelChat_Reconnecting_ReceiveNoMessage()
        {
            //Arrange
            var lobbies = new Lobby[2];

            int receivedChatCount = 0;
            Result<ChannelChatMessage>[] chatReceived = new Result<ChannelChatMessage>[2] { null, null };

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = CreateLobby(this.users[i].Session);

                lobbies[i].Connect();

                Result<ChatChannelSlug> joinResult = null;
                lobbies[i].JoinDefaultChatChannel(result => joinResult = result);
                yield return new WaitUntil(() => joinResult != null);
            }

            lobbies[0].ChannelChatReceived += result =>
            {
                receivedChatCount++;
                chatReceived[0] = result;
                Debug.Log(result.Value.payload);
            };

            lobbies[1].ChannelChatReceived += result =>
            {
                receivedChatCount++;
                chatReceived[1] = result;
                Debug.Log(result.Value.payload);
            };

            bool isConnected = false;
            bool isDisconnected = false;
            lobbies[1].Connected += () =>
            {
                isConnected = true;
                isDisconnected = false;
            };

            lobbies[1].Disconnected += (WsCloseCode closeCode) =>
            {
                isConnected = false;
                isDisconnected = true;
            };

            //Act
            var userId = this.users[0].Session.UserId;
            var chatMessage = "{\",;:'\\|!@#$%^&*()_+-=`~?><.,";

            lobbies[1].Disconnect();
            yield return new WaitUntil(() => isDisconnected == true);

            lobbies[1].Connect();
            yield return new WaitUntil(() => isConnected == true);

            Result channelChatResult = null;
            lobbies[0].SendChannelChat(chatMessage, result => channelChatResult = result);

            yield return new WaitUntil(() => channelChatResult != null);

            Debug.Log(channelChatResult);

            yield return new WaitUntil(() => receivedChatCount >= lobbies.Length - 1);

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            //Assert
            Assert.That(receivedChatCount, Is.EqualTo(1));
            Assert.That(chatReceived[0].Value.payload == chatMessage);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator ListOnlineFriends_MultipleUsersConnected_ReturnAllUsers()
        {
            var lobbies = new Lobby[this.users.Length];

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = CreateLobby(this.users[i].Session);

                lobbies[i].Connect();
            }

            Debug.Log("Online users:\n");

            foreach (var s in this.users)
            {
                Debug.Log(s.Session.UserId);
            }

            Result userStatusResult;

            for (int i = 1; i < 4; i++)
            {
                Result requestFriendResult = null;
                lobbies[i].RequestFriend(this.users[0].Session.UserId, result => requestFriendResult = result);
                yield return TestHelper.WaitForValue(() => requestFriendResult);

                Result acceptFriendResult = null;

                lobbies[0].AcceptFriend(this.users[i].Session.UserId, result => acceptFriendResult = result);
                yield return TestHelper.WaitForValue(() => acceptFriendResult);

                userStatusResult = null;
                lobbies[i].SetUserStatus(UserStatus.Availabe, "random activity", result => userStatusResult = result);
                yield return TestHelper.WaitForValue(() => userStatusResult);

                TestHelper.LogResult(userStatusResult, "Set User Status Online");
            }

            Result<FriendsStatus> onlineFriendsResult = null;
            lobbies[0].ListFriendsStatus(result => onlineFriendsResult = result);
            yield return TestHelper.WaitForValue(() => onlineFriendsResult);

            TestHelper.LogResult(onlineFriendsResult, "List Friends Presence");

            for (int i = 1; i < 4; i++)
            {
                userStatusResult = null;
                lobbies[i].SetUserStatus(UserStatus.Offline, "disappearing", result => userStatusResult = result);
                yield return TestHelper.WaitForValue(() => userStatusResult);

                TestHelper.LogResult(userStatusResult, "Set User Status Offline");

                Result unfriendResult = null;
                lobbies[i].Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);
                yield return TestHelper.WaitForValue(() => unfriendResult);
            }

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            Assert.False(onlineFriendsResult.IsError);
            Assert.That(onlineFriendsResult.Value.friendsId.Length, Is.GreaterThanOrEqualTo(3));
            Assert.IsTrue(onlineFriendsResult.Value.friendsId.Contains(this.users[1].Session.UserId));
            Assert.IsTrue(onlineFriendsResult.Value.friendsId.Contains(this.users[2].Session.UserId));
            Assert.IsTrue(onlineFriendsResult.Value.friendsId.Contains(this.users[3].Session.UserId));
        }

#region Party Scenarios

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetPartyInfo_NoParty_ReturnError()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            //Ensure that user is not in party, doesn't care about its result.
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            Result<PartyInfo> partyInfoResult = null;
            lobby.GetPartyInfo(result => partyInfoResult = result);
            yield return TestHelper.WaitForValue(() => partyInfoResult);

            lobby.Disconnect();

            Assert.That(partyInfoResult.IsError);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator GetPartyInfo_PartyCreated_ReturnOk()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            TestHelper.LogResult(createPartyResult, "Create Party");

            Result<PartyInfo> getPartyInfoResult = null;
            lobby.GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);

            TestHelper.LogResult(getPartyInfoResult, "Get Party Info");
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            lobby.Disconnect();

            Assert.False(getPartyInfoResult.IsError);
            Assert.That(getPartyInfoResult.Value.partyID, Is.Not.Null.Or.Empty);
            Assert.That(getPartyInfoResult.Value.invitationToken, Is.Not.Null.Or.Empty);
            Assert.That(getPartyInfoResult.Value.members.Length, Is.GreaterThan(0));
            Assert.That(getPartyInfoResult.Value.members[0], Is.EqualTo(this.users[0].Session.UserId));
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator CreateParty_PartyAlreadyCreated_ReturnError()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            //Ensure there is no party before, doesn't care about its result
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            TestHelper.LogResult(createPartyResult, "Create Party");

            Result<PartyInfo> createPartyResult2 = null;
            lobby.CreateParty(result => createPartyResult2 = result);
            yield return TestHelper.WaitForValue(() => createPartyResult2);

            TestHelper.LogResult(createPartyResult2, "Create Party Again");

            leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            lobby.Disconnect();

            Assert.False(createPartyResult.IsError);
            Assert.True(createPartyResult2.IsError);
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator RejectPartyInvitation_Success()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user B listens incoming party invitation)
            PartyInvitation incomingInvitation = null;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitation = result.Value;
            };
            lobbyB.InvitedToParty += onInvitedToParty;
            
            // Arrange (user A listens rejected party invitation)
            PartyRejectNotif partyRejection = null;
            ResultCallback<PartyRejectNotif> onRejectPartyInvitation = result =>
            {
                if (result.IsError) return;
                partyRejection = result.Value;
            };
            lobbyA.RejectedPartyInvitation += onRejectPartyInvitation;
            
            // Arrange (user A cleans up & creates party)
            Result cleanupParty = null;
            lobbyA.LeaveParty(r => cleanupParty = r);
            yield return TestHelper.WaitForValue(() => cleanupParty);
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);
            
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            
            // Act (user B rejects)
            while (incomingInvitation == null) yield return new WaitForSeconds(.1f);
            Result<PartyRejectResponse> rejectPartyResult = null;
            lobbyB.RejectPartyInvitation(incomingInvitation.partyID, incomingInvitation.invitationToken, r => rejectPartyResult = r );
            yield return TestHelper.WaitForValue(() => rejectPartyResult);
            while (partyRejection == null) yield return new WaitForSeconds(.1f);
            
            // Assert (GET party info)
            Result<PartyInfo> getPartyInfoResult = null;
            lobbyA.GetPartyInfo(r => getPartyInfoResult = r);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsFalse(invitePartyResult.IsError);
            Assert.IsFalse(rejectPartyResult.IsError);
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsTrue(rejectPartyResult.Value.partyID == createPartyResult.Value.partyID);
            Assert.IsTrue(partyRejection.userID == idB);
            Assert.IsFalse(getPartyInfoResult.Value.invitees.Contains(idB));
            Assert.IsFalse(getPartyInfoResult.Value.members.Contains(idB));
        }
        
        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator RejectAndInviteParty_MultipleTimes_Success()
        {
            const int repetition = 5;
            
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);
            // Arrange (user A creates party)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // Arrange (user B listens incoming party invitation)
            int incomingInvitationCount = 0;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError && result.Value.partyID != createPartyResult.Value.partyID) return;
                Interlocked.Increment(ref incomingInvitationCount);
            };
            lobbyB.InvitedToParty += onInvitedToParty;

            for (int i = 0; i < repetition; i++)
            {
                // Act (user A invites user B)
                Result invitePartyResult = null;
                lobbyA.InviteToParty(idB, r => invitePartyResult = r);
                yield return TestHelper.WaitForValue(() => invitePartyResult);
                
                // Act (user B rejects)
                Result<PartyRejectResponse> rejectPartyResult = null;
                lobbyB.RejectPartyInvitation(createPartyResult.Value.partyID, createPartyResult.Value.invitationToken, r => rejectPartyResult = r );
                yield return TestHelper.WaitForValue(() => rejectPartyResult);
            }
            
            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsTrue(incomingInvitationCount == repetition);
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator RejectPartyInvitation_InviteAgain_AcceptParty_Success()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user B listens incoming party invitation)
            PartyInvitation incomingInvitation = null;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitation = result.Value;
            };
            lobbyB.InvitedToParty += onInvitedToParty;
            
            // Arrange (user A creates party)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);
            
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            
            // Act (user B rejects)
            while (incomingInvitation == null) yield return new WaitForSeconds(.1f);
            Result<PartyRejectResponse> rejectPartyResult = null;
            lobbyB.RejectPartyInvitation(incomingInvitation.partyID, incomingInvitation.invitationToken, r => rejectPartyResult = r );
            yield return TestHelper.WaitForValue(() => rejectPartyResult);
            
            // Act (user A tries to invite user B again & user B accepts it)
            Result invitePartyResult2 = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult2 = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult2);

            Result<PartyInfo> acceptPartyResult = null;
            lobbyB.JoinParty(incomingInvitation.partyID, incomingInvitation.invitationToken, r => acceptPartyResult = r);
            yield return TestHelper.WaitForValue(() => acceptPartyResult);
            
            // Assert (GET party info)
            Result<PartyInfo> getPartyInfoResult = null;
            lobbyA.GetPartyInfo(r => getPartyInfoResult = r);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            // Cleanup
            Result leavePartyResultA = null;
            Result leavePartyResultB = null;
            lobbyA.LeaveParty(r => leavePartyResultA = r);
            lobbyB.LeaveParty(r => leavePartyResultB = r);
            yield return TestHelper.WaitForValue(() => leavePartyResultA);
            yield return TestHelper.WaitForValue(() => leavePartyResultB);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResultA.IsError);
            Assert.IsFalse(leavePartyResultB.IsError);
            Assert.IsFalse(invitePartyResult.IsError);
            Assert.IsFalse(rejectPartyResult.IsError);
            Assert.IsFalse(invitePartyResult2.IsError);
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsTrue(rejectPartyResult.Value.partyID == createPartyResult.Value.partyID);
            Assert.IsFalse(getPartyInfoResult.Value.invitees.Contains(idB));
            Assert.IsTrue(getPartyInfoResult.Value.members.Contains(idB));
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator RejectLatestInvitation_OldInvitationsAreInvalid()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user B listens incoming party invitation)
            PartyInvitation incomingInvitation = null;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitation = result.Value;
            };
            lobbyB.InvitedToParty += onInvitedToParty;
            
            // Arrange (user A creates party)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);
            
            // Arrange (send multiple same invitation)
            for (int i = 0; i < 3; i++)
            {
                Result invitePartyResult = null;
                lobbyA.InviteToParty(idB, r => invitePartyResult = r);
                yield return TestHelper.WaitForValue(() => invitePartyResult);
            }
            
            // Act (user B rejects)
            while (incomingInvitation == null) yield return new WaitForSeconds(.1f);
            Result<PartyRejectResponse> rejectPartyResult = null;
            lobbyB.RejectPartyInvitation(incomingInvitation.partyID, incomingInvitation.invitationToken, r => rejectPartyResult = r );
            yield return TestHelper.WaitForValue(() => rejectPartyResult);

            // Act (user B accept one of the invitation)
            Result<PartyInfo> acceptPartyResult = null;
            lobbyB.JoinParty(incomingInvitation.partyID, incomingInvitation.invitationToken, r => acceptPartyResult = r);
            yield return TestHelper.WaitForValue(() => acceptPartyResult);
            
            // Assert (GET party info)
            Result<PartyInfo> getPartyInfoResult = null;
            lobbyA.GetPartyInfo(r => getPartyInfoResult = r);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            // Cleanup
            Result leavePartyResultA = null;
            Result leavePartyResultB = null;
            lobbyA.LeaveParty(r => leavePartyResultA = r);
            lobbyB.LeaveParty(r => leavePartyResultB = r);
            yield return TestHelper.WaitForValue(() => leavePartyResultA);
            yield return TestHelper.WaitForValue(() => leavePartyResultB);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResultA.IsError);
            Assert.IsFalse(rejectPartyResult.IsError);
            Assert.IsTrue(acceptPartyResult.IsError);
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsTrue(rejectPartyResult.Value.partyID == createPartyResult.Value.partyID);
            Assert.IsFalse(getPartyInfoResult.Value.invitees.Contains(idB));
            Assert.IsFalse(getPartyInfoResult.Value.members.Contains(idB));
        }
        
        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator RejectInvalidToken_AND_InvalidPartyID_Failed()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);
            
            // Arrange (user B listens incoming party invitation)
            PartyInvitation incomingInvitation = null;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitation = result.Value;
            };
            lobbyB.InvitedToParty += onInvitedToParty;
            
            // Arrange (user A creates party & invites user B)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);
            
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            while (incomingInvitation == null) yield return new WaitForSeconds(.2f);

            // Act (user B rejects party using invalid token)
            Result<PartyRejectResponse> rejectInvalidTokenResult = null;
            lobbyB.RejectPartyInvitation(incomingInvitation.partyID, "01234567890123456789", r => rejectInvalidTokenResult = r);
            yield return TestHelper.WaitForValue(() => rejectInvalidTokenResult);
            
            // Act (user B rejects party using invalid party ID)
            Result<PartyRejectResponse> rejectInvalidPartyIDResult = null;
            lobbyB.RejectPartyInvitation("Party ID invalid", incomingInvitation.invitationToken, r => rejectInvalidPartyIDResult = r);
            yield return TestHelper.WaitForValue(() => rejectInvalidPartyIDResult);
            
            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(invitePartyResult.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsTrue(rejectInvalidTokenResult.IsError);
            Assert.IsTrue(rejectInvalidPartyIDResult.IsError);
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator RejectInvitation_MultipleIncoming_Success()
        {
            // user A will accept invitation from the others
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            var lobbyC = CreateLobby(this.users[2].Session);
            var lobbyD = CreateLobby(this.users[3].Session);
            string idA = this.users[0].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            if (!lobbyC.IsConnected) lobbyC.Connect();
            if (!lobbyD.IsConnected) lobbyD.Connect();
            while (!lobbyA.IsConnected && !lobbyB.IsConnected && !lobbyC.IsConnected && !lobbyD.IsConnected) yield return new WaitForSeconds(.07f);
            
            // Arrange (user A listens incoming party invitations)
            ConcurrentQueue<PartyInvitation> incomingInvitations = new ConcurrentQueue<PartyInvitation>();
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitations.Enqueue(result.Value);
            };
            lobbyA.InvitedToParty += onInvitedToParty;
            
            // Arrange (user B, C, & D create party then invite user A)
            Result<PartyInfo> createPartyResult_B = null, createPartyResult_C = null, createPartyResult_D = null;
            lobbyB.CreateParty(r => createPartyResult_B = r);
            lobbyC.CreateParty(r => createPartyResult_C = r);
            lobbyD.CreateParty(r => createPartyResult_D = r);
            while (createPartyResult_B == null && createPartyResult_C == null && createPartyResult_D == null) yield return new WaitForSeconds(0.1f);
            lobbyB.InviteToParty(idA, r => { });
            lobbyC.InviteToParty(idA, r => { });
            lobbyD.InviteToParty(idA, r => { });
            
            while (incomingInvitations.Count < 3) yield return new WaitForSeconds(0.1f);
            
            // Act (user A reject all invitation)
            ConcurrentQueue<Result<PartyRejectResponse>> rejectPartyResult = new ConcurrentQueue<Result<PartyRejectResponse>>();
            foreach (var partyInvitation in incomingInvitations)
            {
                lobbyA.RejectPartyInvitation(partyInvitation.partyID, partyInvitation.invitationToken, result =>
                {
                    rejectPartyResult.Enqueue(result);
                });
            }
            
            while (rejectPartyResult.Count < 3) yield return new WaitForSeconds(0.1f);
            
            // Cleanup
            lobbyB.LeaveParty(r => { });
            lobbyC.LeaveParty(r => { });
            lobbyD.LeaveParty(r => { });
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            lobbyC.Disconnect();
            lobbyD.Disconnect();
            
            // Assert
            Assert.IsFalse(createPartyResult_B.IsError);
            Assert.IsFalse(createPartyResult_C.IsError);
            Assert.IsFalse(createPartyResult_D.IsError);
            foreach (var result in rejectPartyResult)
            {
                Assert.IsFalse(result.IsError);
            }
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator RejectInvitation_InviteeRemovedFromPartyStorage()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user B listens incoming party invitation)
            PartyInvitation incomingInvitation = null;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitation = result.Value;
            };
            lobbyB.InvitedToParty += onInvitedToParty;
            
            // Arrange (user A listens party notif)
            ConcurrentQueue<PartyDataUpdateNotif> partyInfos = new ConcurrentQueue<PartyDataUpdateNotif>();
            ResultCallback<PartyDataUpdateNotif> onPartyDataUpdate = result =>
            {
                if (result.IsError) return;
                partyInfos.Enqueue(result.Value);
            };
            lobbyA.PartyDataUpdateNotif += onPartyDataUpdate;
            
            // Arrange (user A creates party and invite)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);
            
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            
            // Act (user B rejects)
            while (incomingInvitation == null) yield return new WaitForSeconds(.1f);
            Result<PartyRejectResponse> rejectPartyResult = null;
            lobbyB.RejectPartyInvitation(incomingInvitation.partyID, incomingInvitation.invitationToken, r => rejectPartyResult = r );
            yield return TestHelper.WaitForValue(() => rejectPartyResult);
            
            // Assert
            yield return new WaitForSeconds(1.0f);
            bool inviteeIsEmpty = partyInfos.Last().invitees.Length == 0;
            
            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsFalse(invitePartyResult.IsError);
            Assert.IsFalse(rejectPartyResult.IsError);
            Assert.IsTrue(rejectPartyResult.Value.partyID == createPartyResult.Value.partyID);
            Assert.IsTrue(inviteeIsEmpty);
        }

        [UnityTest, TestLog, Order(2), TestLog, Timeout(40000)]
        public IEnumerator PromoteMemberAsPartyLeader_Success()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user B listens incoming party invitation)
            PartyInvitation incomingInvitation = null;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitation = result.Value;
            };
            lobbyB.InvitedToParty += onInvitedToParty;

            // Arrange (user A creates party and invite)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);

            // Act (user B accept the invitation)
            while (incomingInvitation == null) yield return new WaitForSeconds(.1f);
            Result<PartyInfo> joinPartyresult = null;
            lobbyB.JoinParty(incomingInvitation.partyID, incomingInvitation.invitationToken, r => joinPartyresult = r);
            yield return TestHelper.WaitForValue(() => joinPartyresult);

            // Act (user A promote user B to become party leader)
            Result<PartyPromoteLeaderResponse> promotePartyLeaderResponse = null;
            lobbyA.PromotePartyLeader(idB, r => promotePartyLeaderResponse = r);
            yield return TestHelper.WaitForValue(() => promotePartyLeaderResponse);

            // Assert
            bool user2BecomeLeader = promotePartyLeaderResponse.Value.leaderID == idB;

            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            Result leavePartyResult2 = null;
            lobbyB.LeaveParty(r => leavePartyResult2 = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult2);

            lobbyA.Disconnect();
            lobbyB.Disconnect();

            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsFalse(leavePartyResult2.IsError);
            Assert.IsFalse(invitePartyResult.IsError);
            Assert.IsFalse(promotePartyLeaderResponse.IsError);
            Assert.IsTrue(user2BecomeLeader);
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator PromoteMemberAsPartyLeader_ErrorUnableGetUser()
        {
            var lobbyA = CreateLobby(this.users[0].Session);

            if (!lobbyA.IsConnected) lobbyA.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user A creates party and invite)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // Act (user A promote user B to become party leader)
            Result<PartyPromoteLeaderResponse> promotePartyLeaderResponse = null;
            lobbyA.PromotePartyLeader("UnknownUserID", r => promotePartyLeaderResponse = r);
            yield return TestHelper.WaitForValue(() => promotePartyLeaderResponse);

            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);
            lobbyA.Disconnect();

            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsTrue(promotePartyLeaderResponse.IsError);
            Assert.IsTrue(promotePartyLeaderResponse.Error.Code == ErrorCode.PartyPromoteLeaderUnableToGetUserRegistry);
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator PromoteMemberAsPartyLeader_ErrorPartyNotFound()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            string idA = this.users[0].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            // Act (user A promote user B to become party leader)
            Result<PartyPromoteLeaderResponse> promotePartyLeaderResponse = null;
            lobbyA.PromotePartyLeader(idA, r => promotePartyLeaderResponse = r);
            yield return TestHelper.WaitForValue(() => promotePartyLeaderResponse);

            lobbyA.Disconnect();

            Assert.IsTrue(promotePartyLeaderResponse.IsError);
            Assert.IsTrue(promotePartyLeaderResponse.Error.Code == ErrorCode.PartyPromoteLeaderUserPartyNotFound);
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator PromoteMemberAsPartyLeader_ErrorUserNotLeader()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user B listens incoming party invitation)
            PartyInvitation incomingInvitation = null;
            ResultCallback<PartyInvitation> onInvitedToParty = result =>
            {
                if (result.IsError) return;
                incomingInvitation = result.Value;
            };
            lobbyB.InvitedToParty += onInvitedToParty;

            // Arrange (user A creates party and invite)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);

            // Act (user B accept the invitation)
            while (incomingInvitation == null) yield return new WaitForSeconds(.1f);
            Result<PartyInfo> joinPartyresult = null;
            lobbyB.JoinParty(incomingInvitation.partyID, incomingInvitation.invitationToken, r => joinPartyresult = r);
            yield return TestHelper.WaitForValue(() => joinPartyresult);

            // Act (user A promote user B to become party leader)
            Result<PartyPromoteLeaderResponse> promotePartyLeaderResponse = null;
            lobbyB.PromotePartyLeader(idB, r => promotePartyLeaderResponse = r);
            yield return TestHelper.WaitForValue(() => promotePartyLeaderResponse);

            Error err = promotePartyLeaderResponse.Error;

            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            Result leavePartyResult2 = null;
            lobbyB.LeaveParty(r => leavePartyResult2 = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult2);

            lobbyA.Disconnect();
            lobbyB.Disconnect();

            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsFalse(leavePartyResult2.IsError);
            Assert.IsFalse(invitePartyResult.IsError);
            Assert.IsTrue(promotePartyLeaderResponse.IsError);
            Assert.IsTrue(promotePartyLeaderResponse.Error.Code == ErrorCode.PartyPromoteLeaderUserNotLeader);
        }

        [UnityTest, TestLog, Order(2), Timeout(40000)]
        public IEnumerator PromoteMemberAsPartyLeader_ErrorUnablePromoteAnotherPartyLeader()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user A creates party and invite)
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // Arrange (user B creates party and invite)
            Result<PartyInfo> createPartyResult2 = null;
            lobbyB.CreateParty(r => createPartyResult2 = r);
            yield return TestHelper.WaitForValue(() => createPartyResult2);

            // Act (user A promote user B to become party leader)
            Result<PartyPromoteLeaderResponse> promotePartyLeaderResponse = null;
            lobbyA.PromotePartyLeader(idB, r => promotePartyLeaderResponse = r);
            yield return TestHelper.WaitForValue(() => promotePartyLeaderResponse);

            // Cleanup
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => leavePartyResult = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            Result leavePartyResult2 = null;
            lobbyB.LeaveParty(r => leavePartyResult2 = r);
            yield return TestHelper.WaitForValue(() => leavePartyResult2);

            lobbyA.Disconnect();
            lobbyB.Disconnect();

            Assert.IsFalse(createPartyResult.IsError);
            Assert.IsFalse(createPartyResult2.IsError);
            Assert.IsFalse(leavePartyResult.IsError);
            Assert.IsFalse(leavePartyResult2.IsError);
            Assert.IsTrue(promotePartyLeaderResponse.IsError);
            Assert.IsTrue(promotePartyLeaderResponse.Error.Code == ErrorCode.PartyPromoteLeaderUnableToPromoteLeader);
        }
#endregion

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator InviteToParty_InvitationAccepted_CanChat()
        {
            var lobby1 = CreateLobby(this.users[0].Session);
            var lobby2 = CreateLobby(this.users[1].Session);

            lobby1.Connect();
            lobby2.Connect();

            Result leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            leavePartyResult = null;

            lobby2.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            Result<PartyInfo> createPartyResult = null;
            lobby1.CreateParty(result => createPartyResult = result);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            TestHelper.LogResult(createPartyResult, "Create Party");
            Result<PartyInvitation> invitedToPartyResult = null;
            lobby2.InvitedToParty += result => invitedToPartyResult = result;
            Result inviteResult = null;
            lobby1.InviteToParty(this.users[1].Session.UserId, result => inviteResult = result);
            yield return TestHelper.WaitForValue(() => inviteResult);

            TestHelper.LogResult(inviteResult, "Invite To Party");
            yield return TestHelper.WaitForValue(() => invitedToPartyResult);

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            Result<PartyInfo> joinPartyResult = null;

            lobby2.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);
            yield return TestHelper.WaitForValue(() => joinPartyResult);

            TestHelper.LogResult(joinPartyResult, "Join Party");

            Result<ChatMessage> receivedPartyChatResult = null;
            lobby1.PartyChatReceived += result => receivedPartyChatResult = result;

            Result sendPartyChatResult = null;
            lobby2.SendPartyChat("This is a party chat", result => sendPartyChatResult = result);
            yield return TestHelper.WaitForValue(() => sendPartyChatResult);

            TestHelper.LogResult(sendPartyChatResult, "Send Party Chat");
            yield return TestHelper.WaitForValue(() => receivedPartyChatResult);

            TestHelper.LogResult(receivedPartyChatResult, "Received Party Chat");

            leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby2.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            lobby1.Disconnect();
            lobby2.Disconnect();

            Assert.False(createPartyResult.IsError);
            Assert.False(inviteResult.IsError);
            Assert.False(invitedToPartyResult.IsError);
            Assert.False(joinPartyResult.IsError);
            Assert.False(receivedPartyChatResult.IsError);
            Assert.False(leavePartyResult.IsError);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator PartyMember_Kicked_CannotChat()
        {
            var lobby1 = CreateLobby(this.users[0].Session);
            var lobby2 = CreateLobby(this.users[1].Session);
            var lobby3 = CreateLobby(this.users[2].Session);

            lobby1.Disconnected += (result) => Debug.Log("Lobby1 Disconnected. Code: " + result.ToString());
            lobby2.Disconnected += (result) => Debug.Log("Lobby2 Disconnected. Code: " + result.ToString());
            lobby3.Disconnected += (result) => Debug.Log("Lobby3 Disconnected. Code: " + result.ToString());

            lobby1.Connect();
            lobby2.Connect();
            lobby3.Connect();

            Debug.Log("Connected to lobby");

            Result leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            leavePartyResult = null;

            lobby2.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            leavePartyResult = null;

            lobby3.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            //1. lobby1 create party
            Result<PartyInfo> createPartyResult = null;
            lobby1.CreateParty(result => createPartyResult = result);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            TestHelper.LogResult(createPartyResult, "Create Party");

            //2. lobby2 join party
            Result<PartyInvitation> invitedToPartyResult = null;
            lobby2.InvitedToParty += result => invitedToPartyResult = result;
            Result inviteResult = null;
            lobby1.InviteToParty(this.users[1].Session.UserId, result => inviteResult = result);
            yield return TestHelper.WaitForValue(() => inviteResult);

            TestHelper.LogResult(inviteResult, "Invite To Party");
            yield return TestHelper.WaitForValue(() => invitedToPartyResult);

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            Result<PartyInfo> joinPartyResult = null;
            lobby2.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);
            yield return TestHelper.WaitForValue(() => joinPartyResult);

            TestHelper.LogResult(joinPartyResult, "Join Party");

            invitedToPartyResult = null;
            lobby3.InvitedToParty += result => invitedToPartyResult = result;
            inviteResult = null;
            lobby1.InviteToParty(this.users[2].Session.UserId, result => inviteResult = result);
            yield return TestHelper.WaitForValue(() => inviteResult);

            TestHelper.LogResult(inviteResult, "Invite To Party");
            yield return TestHelper.WaitForValue(() => invitedToPartyResult);

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            joinPartyResult = null;
            lobby3.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);
            yield return TestHelper.WaitForValue(() => joinPartyResult);

            TestHelper.LogResult(joinPartyResult, "Join Party");

            //3. Kick member

            Result kickResult = null;
            Result<KickNotification> kickedFromPartyResult = null;
            lobby3.KickedFromParty += result => kickedFromPartyResult = result;

            lobby1.KickPartyMember(this.users[2].Session.UserId, result => kickResult = result);
            yield return TestHelper.WaitForValue(() => kickResult);

            TestHelper.LogResult(kickResult, "Kick Member");
            yield return TestHelper.WaitForValue(() => kickedFromPartyResult);

            TestHelper.LogResult(kickedFromPartyResult, "Kicked From Party");

            Result<PartyInfo> partyInfoResult = null;
            lobby2.GetPartyInfo(result => partyInfoResult = result);
            yield return TestHelper.WaitForValue(() => partyInfoResult);

            //4. Leave party and disconnect
            leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby2.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby3.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            lobby1.Disconnect();
            lobby2.Disconnect();
            lobby3.Disconnect();

            Assert.False(kickResult.IsError);
            Assert.False(kickedFromPartyResult.IsError);
            Assert.That(joinPartyResult.Value.members.Length, Is.EqualTo(3));
            Assert.That(partyInfoResult.Value.members.Length, Is.EqualTo(2));
        }

        [UnityTest, TestLog, Order(1), Timeout(200000)]
        public IEnumerator LobbyConnected_ForMoreThan1Minutes_DoesntDisconnect()
        {
            var lobby = CreateLobby(this.users[0].Session);
            lobby.Disconnected += (WsCloseCode closeCode) => Debug.Log("Disconnected");

            lobby.Connect();

            for (int i = 0; i < 125; i += 5)
            {
                Debug.Log(string.Format("Wait for {0} seconds. Lobby.IsConnected={1}", i, lobby.IsConnected));

                yield return new WaitForSeconds(5f);
            }

            Assert.That(lobby.IsConnected);

            lobby.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Lobby_Disconnect_CloseImmediately()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Disconnected += (WsCloseCode code) =>
            {
                Debug.Log("Disconnected Code: " + code);
            };

            Debug.Log(string.Format("Lobby.IsConnected={0}", lobby.IsConnected));

            lobby.Connect();

            Debug.Log(string.Format("Lobby.IsConnected={0}", lobby.IsConnected));

            lobby.Disconnect();

            Assert.That(!lobby.IsConnected);

            Debug.Log(string.Format("Lobby.IsConnected={0}", lobby.IsConnected));


            yield break;
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Notification_GetAsyncNotification()
        {
            Result sendNotificationResult = null;
            string notification = "this is a notification";
            this.helper.SendNotification(
                this.users[0].Session.UserId,
                true,
                notification,
                result => { sendNotificationResult = result; });

            yield return TestHelper.WaitForValue(() =>sendNotificationResult);

            TestHelper.LogResult(sendNotificationResult, "send notification");

            var lobby = CreateLobby(this.users[0].Session);
            lobby.Connect();

            Debug.Log("Connected to lobby");

            Result<Notification> getNotificationResult = null;
            lobby.OnNotification += result => getNotificationResult = result;

            Result pullResult = null;
            lobby.PullAsyncNotifications(result => pullResult = result);

            yield return TestHelper.WaitForValue(() => pullResult);

            yield return TestHelper.WaitForValue(() =>getNotificationResult);

            TestHelper.LogResult(getNotificationResult);

            lobby.Disconnect();

            yield return null;

            Assert.IsNotNull(sendNotificationResult);
            Assert.IsNotNull(getNotificationResult);
            Assert.IsFalse(sendNotificationResult.IsError);
            Assert.IsFalse(getNotificationResult.IsError);
            Assert.IsTrue(getNotificationResult.Value.payload == notification);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Notification_GetSyncNotification()
        {
            const int repetition = 2;

            bool[] getNotifSuccess = new bool[repetition];
            string[] payloads = new string[repetition];
            Result[] sendNotificationResults = new Result[repetition];

            var lobby = CreateLobby(this.users[0].Session);
            lobby.Connect();

            while (!lobby.IsConnected) yield return new WaitForSeconds(.2f);

            Debug.Log("Connected to lobby");

            Result<Notification> incomingNotification = null;
            lobby.OnNotification += result => { incomingNotification = result; };

            for (int i = 0; i < repetition; i++)
            {
                payloads[i] = "Notification number: " + i;
                sendNotificationResults[i] = null;

                this.helper.SendNotification(
                    this.users[0].Session.UserId,
                    false,
                    payloads[i],
                    result => { sendNotificationResults[i] = result; });

                yield return TestHelper.WaitForValue(() =>sendNotificationResults[i]);

                TestHelper.LogResult(sendNotificationResults[i]);

                yield return TestHelper.WaitForValue(() =>incomingNotification);

                if (incomingNotification.Value.payload == payloads[i])
                {
                    getNotifSuccess[i] = true;
                }

                incomingNotification = null;

            }

            lobby.Disconnect();

            Assert.IsTrue(getNotifSuccess.All(notif => notif));
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator SetUserStatus_CheckedByAnotherUser()
        {
            UserStatus ExpectedUser1Status = UserStatus.Availabe;

            var lobby1 = CreateLobby(this.users[0].Session);
            lobby1.Connect();

            Debug.Log("User1 Connected to lobby");

            var lobby2 = CreateLobby(this.users[1].Session);
            lobby2.Connect();

            Debug.Log("User2 Connected to lobby");

            Result requestFriendResult = null;
            lobby1.RequestFriend(this.users[1].Session.UserId, result => requestFriendResult = result);

            yield return TestHelper.WaitForValue(() =>requestFriendResult);

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].Session.UserId, result => acceptFriendResult = result);

            yield return TestHelper.WaitForValue(() =>acceptFriendResult);

            Result setUser2StatusResult = null;
            lobby2.SetUserStatus(UserStatus.Availabe, "ready to play", result => { setUser2StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>setUser2StatusResult);

            TestHelper.LogResult(setUser2StatusResult);

            Result<FriendsStatusNotif> listenUser1StatusResult = null;
            lobby2.FriendsStatusChanged += result => { listenUser1StatusResult = result; };

            Result setUser1StatusResult = null;
            lobby1.SetUserStatus(
                ExpectedUser1Status,
                "expected activity",
                result => { setUser1StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>setUser1StatusResult);

            TestHelper.LogResult(setUser1StatusResult);

            yield return TestHelper.WaitForValue(() =>listenUser1StatusResult);

            while (listenUser1StatusResult.Value.availability != ((int)ExpectedUser1Status).ToString())
            {
                yield return new WaitForSeconds(.1f);
            }

            TestHelper.LogResult(listenUser1StatusResult);

            Result unfriendResult = null;
            lobby2.Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);

            yield return TestHelper.WaitForValue(() =>unfriendResult);

            lobby1.Disconnect();
            lobby2.Disconnect();

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(listenUser1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(listenUser1StatusResult.IsError);
            Assert.AreEqual(((int)ExpectedUser1Status).ToString(), listenUser1StatusResult.Value.availability);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator ChangeUserStatus_CheckedByAnotherUser()
        {
            UserStatus ExpectedUser1Status = UserStatus.Busy;

            var lobby1 = CreateLobby(this.users[0].Session);

            lobby1.Connect();

            Debug.Log("User1 Connected to lobby");

            var lobby2 = CreateLobby(this.users[1].Session);

            lobby2.Connect();

            Debug.Log("User2 Connected to lobby");

            Result requestFriendResult = null;
            lobby1.RequestFriend(this.users[1].Session.UserId, result => requestFriendResult = result);
            yield return TestHelper.WaitForValue(() => requestFriendResult);

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].Session.UserId, result => acceptFriendResult = result);
            yield return TestHelper.WaitForValue(() => acceptFriendResult);

            Result setUser2StatusResult = null;
            lobby2.SetUserStatus(
                UserStatus.Availabe,
                "ready to play again",
                result => { setUser2StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>setUser2StatusResult);

            TestHelper.LogResult(setUser2StatusResult);

            Result setUser1StatusResult = null;
            lobby1.SetUserStatus(
                UserStatus.Availabe,
                "ready to play too",
                result => { setUser1StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>setUser1StatusResult);

            TestHelper.LogResult(setUser1StatusResult);

            Result<FriendsStatusNotif> listenUser1StatusResult = null;
            lobby2.FriendsStatusChanged += result => { listenUser1StatusResult = result; };

            Result changeUser1StatusResult = null;
            lobby1.SetUserStatus(
                ExpectedUser1Status,
                "busy, can't play",
                result => { changeUser1StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>changeUser1StatusResult);

            yield return TestHelper.WaitForValue(() =>listenUser1StatusResult);

            TestHelper.LogResult(listenUser1StatusResult);

            Result unfriendResult = null;
            lobby2.Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);
            yield return TestHelper.WaitForValue(() => unfriendResult);

            lobby1.Disconnect();
            lobby2.Disconnect();

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(listenUser1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(listenUser1StatusResult.IsError);
            Assert.AreEqual((int) ExpectedUser1Status, int.Parse(listenUser1StatusResult.Value.availability));
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator ChangeLongUserStatus_CheckedByAnotherUser()
        {
            /*******************************************************
             * 
             * as of 15 July 2021, intended max ws message is 1KB
             * 
             * ****************************************************/

            var lobby1 = CreateLobby(this.users[0].Session);

            lobby1.Connect();

            Debug.Log("User1 Connected to lobby");

            var lobby2 = CreateLobby(this.users[1].Session);

            lobby2.Connect();

            Debug.Log("User2 Connected to lobby");

            var lobby3 = CreateLobby(this.users[2].Session);

            lobby3.Connect();

            Debug.Log("User3 Connected to lobby");

            Result requestFriendResult = null;
            lobby1.RequestFriend(this.users[1].Session.UserId, result => requestFriendResult = result);

            yield return TestHelper.WaitForValue(() =>requestFriendResult);

            requestFriendResult = null;
            lobby3.RequestFriend(this.users[1].Session.UserId, result => requestFriendResult = result);

            yield return TestHelper.WaitForValue(() =>requestFriendResult);
            

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].Session.UserId, result => acceptFriendResult = result);

            yield return TestHelper.WaitForValue(() =>acceptFriendResult);
            

            acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[2].Session.UserId, result => acceptFriendResult = result);

            yield return TestHelper.WaitForValue(() =>acceptFriendResult);

            Result<FriendsStatusNotif> listenUserStatusResult = null;
            lobby2.FriendsStatusChanged += result => { listenUserStatusResult = result; };

            Result setUser1StatusResult = null;
            lobby1.SetUserStatus(
                UserStatus.Availabe,
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras lacinia a leo sit amet ullamcorper. Sed et orci tellus. Nam efficitur quis neque at ullamcorper. Nullam et ipsum metus. Nam ante sapien, pretium vel odio ut, ultricies venenatis urna. Nullam et ipsum metus. Nam ante sapien, pretium vel odio ut, ultricies venenatis urna Nullam et ipsum metus. Nam ante sapien, pretium vel odio ut, ultricies venenatis urna Nullam et ipsum metus. Nam ante sapien, pretium vel odio ut, ultricies venenatis urna Nullam et ipsum metus. Nam ante sapien, pretium vel odio ut, ultricies venenatis urna Nullam et ipsum metus. Nam ante sapien, pretium vel odio ut, 0",
                result => { setUser1StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>setUser1StatusResult);

            TestHelper.LogResult(setUser1StatusResult);

            yield return TestHelper.WaitForValue(() =>listenUserStatusResult);

            TestHelper.LogResult(listenUserStatusResult);

            Result<FriendsStatusNotif> user1StatusResult = listenUserStatusResult;
            listenUserStatusResult = null;

            Result setUser3StatusResult = null;
            lobby3.SetUserStatus(
                UserStatus.Availabe,
                "test",
                result => { setUser3StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>setUser3StatusResult);

            TestHelper.LogResult(setUser3StatusResult);

            yield return TestHelper.WaitForValue(() =>listenUserStatusResult);

            TestHelper.LogResult(listenUserStatusResult);

            Result<FriendsStatusNotif> user3StatusResult = listenUserStatusResult;

            Result<FriendsStatus> friendsStatus = null;
            lobby2.ListFriendsStatus(result =>
            {
                friendsStatus = result;
            });

            yield return TestHelper.WaitForValue(() =>friendsStatus);

            TestHelper.LogResult(friendsStatus);

            Result unfriendResult = null;
            lobby2.Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);

            yield return TestHelper.WaitForValue(() =>unfriendResult);

            lobby1.Disconnect();
            lobby2.Disconnect();
            lobby3.Disconnect();

            int i = 0;
            for (; i < friendsStatus.Value.friendsId.Length; i++)
            {
                if (friendsStatus.Value.friendsId[i] == user1StatusResult.Value.userID)
                {
                    break;
                }
            }

            int j = 0;
            for (; j < friendsStatus.Value.friendsId.Length; j++)
            {
                if (friendsStatus.Value.friendsId[j] == user3StatusResult.Value.userID)
                {
                    break;
                }
            }

            foreach (var activity in friendsStatus.Value.activity)
            {
                Debug.Log("Activity: " + activity);
            }

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(user1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(user1StatusResult.IsError);
            Assert.IsNotNull(user3StatusResult);
            Assert.IsFalse(setUser3StatusResult.IsError);
            Assert.IsFalse(user3StatusResult.IsError);
            Assert.AreEqual(user1StatusResult.Value.activity, friendsStatus.Value.activity[i]);
            Assert.AreEqual(user1StatusResult.Value.activity[user1StatusResult.Value.activity.Length - 1], '0');
            Assert.AreEqual(friendsStatus.Value.activity[i][friendsStatus.Value.activity[i].Length - 1], '0');
            Assert.AreEqual(user3StatusResult.Value.activity, friendsStatus.Value.activity[j]);
            Assert.AreEqual(user3StatusResult.Value.activity[user3StatusResult.Value.activity.Length - 1], 't');
            Assert.AreEqual(friendsStatus.Value.activity[j][friendsStatus.Value.activity[j].Length - 1], 't');
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator ChangeCustomUserStatus_CheckedByAnotherUser()
        {

            var lobby1 = CreateLobby(this.users[0].Session);

            lobby1.Connect();

            Debug.Log("User1 Connected to lobby");

            var lobby2 = CreateLobby(this.users[1].Session);

            lobby2.Connect();

            Debug.Log("User2 Connected to lobby");

            var lobby3 = CreateLobby(this.users[2].Session);

            lobby3.Connect();

            Debug.Log("User3 Connected to lobby");

            Result requestFriendResult = null;
            lobby1.RequestFriend(this.users[1].Session.UserId, result => requestFriendResult = result);

            yield return TestHelper.WaitForValue(() =>requestFriendResult);

            requestFriendResult = null;
            lobby3.RequestFriend(this.users[1].Session.UserId, result => requestFriendResult = result);

            yield return TestHelper.WaitForValue(() =>requestFriendResult);

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].Session.UserId, result => acceptFriendResult = result);

            yield return TestHelper.WaitForValue(() =>acceptFriendResult);

            acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[2].Session.UserId, result => acceptFriendResult = result);

            yield return TestHelper.WaitForValue(() =>acceptFriendResult);

            Result<FriendsStatusNotif> listenUserStatusResult = null;
            lobby2.FriendsStatusChanged += result => { listenUserStatusResult = result; };

            string user1activity = "{\"RoomState\":0,\"HomeIslandState\":1,\"HomeOwnerName\":\"\",\"PhotonRoomName\":\"HomeIsland_afe75cd89eb74db88f95399b5d6f381e_eu\",\"SceneID\":3491,\"Time\":18390.612,\"CurrentPlayerCount\":1,\"MaxPlayers\":8}";
            Result setUser1StatusResult = null;
            lobby1.SetUserStatus(
                UserStatus.Availabe,
                user1activity,
                result => { setUser1StatusResult = result; });

            yield return TestHelper.WaitForValue(() =>setUser1StatusResult);

            TestHelper.LogResult(setUser1StatusResult);

            yield return TestHelper.WaitForValue(() =>listenUserStatusResult);

            TestHelper.LogResult(listenUserStatusResult);

            Result<FriendsStatusNotif> user1StatusResult = listenUserStatusResult;
            listenUserStatusResult = null;

            lobby3.Disconnect();

            yield return TestHelper.WaitForValue(() =>listenUserStatusResult);

            TestHelper.LogResult(listenUserStatusResult);

            Result<FriendsStatusNotif> user3StatusResult = listenUserStatusResult;

            Result<FriendsStatus> friendsStatus = null;
            lobby2.ListFriendsStatus(result =>
            {
                friendsStatus = result;
            });

            yield return TestHelper.WaitForValue(() =>friendsStatus);

            TestHelper.LogResult(friendsStatus);

            Result unfriendResult = null;
            lobby2.Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);
            yield return TestHelper.WaitForValue(() => unfriendResult);

            lobby1.Disconnect();
            lobby2.Disconnect();

            int i = 0;
            for (; i < friendsStatus.Value.friendsId.Length; i++)
            {
                if (friendsStatus.Value.friendsId[i] == user1StatusResult.Value.userID)
                {
                    break;
                }
            }

            int j = 0;
            for (; j < friendsStatus.Value.friendsId.Length; j++)
            {
                if (friendsStatus.Value.friendsId[j] == user3StatusResult.Value.userID)
                {
                    break;
                }
            }

            foreach (var activity in friendsStatus.Value.activity)
            {
                Debug.Log("Activity: " + activity);
            }

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(user1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(user1StatusResult.IsError);
            Assert.IsNotNull(user3StatusResult);
            Assert.IsFalse(user3StatusResult.IsError);
            Assert.AreEqual(user1StatusResult.Value.activity, friendsStatus.Value.activity[i]);
            Assert.AreEqual(user1StatusResult.Value.activity, user1activity);
            Assert.AreEqual(user3StatusResult.Value.activity, friendsStatus.Value.activity[j]);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Friends_Request_Accept()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            yield return TestHelper.WaitForValue(() =>getFriendshipStatusBeforeRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusBeforeRequestFriend);
            Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            yield return TestHelper.WaitForValue(() =>requestFriendResult);

            TestHelper.Assert.IsResultOk(requestFriendResult);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            yield return TestHelper.WaitForValue(() =>getFriendshipStatusAfterRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestFriend);
            Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() =>listOutgoingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listOutgoingFriendRequestResult);
            Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            yield return TestHelper.WaitForValue(() =>getFriendshipStatusAfterRequestSentFromAnother);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestSentFromAnother);
            Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() =>listIncomingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listIncomingFriendRequestResult);
            Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() =>acceptFriendRequestResult);

            TestHelper.Assert.IsResultOk(acceptFriendRequestResult);

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() =>loadFriendListResult);

            TestHelper.Assert.IsResultOk(loadFriendListResult);
            Assert.That(loadFriendListResult.Value.friendsId.Contains(idB));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() =>anotherLoadFriendListResult);

            TestHelper.Assert.IsResultOk(anotherLoadFriendListResult);
            Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            yield return TestHelper.WaitForValue(() =>unfriendResult);

            TestHelper.Assert.IsResultOk(unfriendResult);

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Friends_Notification_Request_Accept()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);

            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<Friend> incomingNotificationFromAResult = null;
            lobbyB.OnIncomingFriendRequest += result => { incomingNotificationFromAResult = result; };

            Result<Friend> friendRequestAcceptedResult = null;
            lobbyA.FriendRequestAccepted += result => { friendRequestAcceptedResult = result; };

            // TODO: fix this (unfriend from the test that request friend)
            // unfriend if already friend, so the friend request success
            Result requestUnfriendResult = null;
            lobbyA.Unfriend(idB, result => { requestUnfriendResult = result; });
            yield return TestHelper.WaitForValue(() =>requestUnfriendResult);
            
            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            yield return TestHelper.WaitForValue(() =>requestFriendResult);

            TestHelper.Assert.IsResultOk(requestFriendResult);

            yield return TestHelper.WaitForValue(() =>incomingNotificationFromAResult);

            TestHelper.Assert.IsResultOk(incomingNotificationFromAResult);
            Assert.That(incomingNotificationFromAResult.Value.friendId == idA);

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() =>acceptFriendRequestResult);

            TestHelper.Assert.IsResultOk(acceptFriendRequestResult);

            yield return TestHelper.WaitForValue(() =>friendRequestAcceptedResult);

            TestHelper.Assert.IsResultOk(friendRequestAcceptedResult);
            Assert.That(friendRequestAcceptedResult.Value.friendId == idB);

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() =>loadFriendListResult);

            TestHelper.Assert.IsResultOk(loadFriendListResult);
            Assert.That(loadFriendListResult.Value.friendsId.Contains(idB));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() =>anotherLoadFriendListResult);

            TestHelper.Assert.IsResultOk(anotherLoadFriendListResult);
            Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            yield return TestHelper.WaitForValue(() =>unfriendResult);

            TestHelper.Assert.IsResultOk(unfriendResult);

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Friends_Request_Unfriend()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            Result<Friend> unfriendNotification = null;
            lobbyB.OnUnfriend += result => { unfriendNotification = result; };

            yield return TestHelper.WaitForValue(() => getFriendshipStatusBeforeRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusBeforeRequestFriend);
            Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            yield return TestHelper.WaitForValue(() => requestFriendResult);

            TestHelper.Assert.IsResultOk(requestFriendResult);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestFriend);
            Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listOutgoingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listOutgoingFriendRequestResult);
            Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestSentFromAnother);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestSentFromAnother);
            Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listIncomingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listIncomingFriendRequestResult);
            Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => acceptFriendRequestResult);

            TestHelper.Assert.IsResultOk(acceptFriendRequestResult);

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListResult);

            TestHelper.Assert.IsResultOk(loadFriendListResult);
            Assert.That(loadFriendListResult.Value.friendsId.Contains(idB));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() => anotherLoadFriendListResult);

            TestHelper.Assert.IsResultOk(anotherLoadFriendListResult);
            Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            yield return TestHelper.WaitForValue(() => unfriendResult);

            TestHelper.Assert.IsResultOk(unfriendResult);

            yield return TestHelper.WaitForValue(() => unfriendNotification);

            TestHelper.Assert.IsResultOk(unfriendNotification);
            Assert.That(unfriendNotification.Value.friendId == idA);

            Result<Friends> loadFriendListAfterUnfriend = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterUnfriend = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListAfterUnfriend);

            TestHelper.Assert.IsResultOk(loadFriendListAfterUnfriend);

            if (loadFriendListAfterUnfriend.Value.friendsId.Length != 0)
            {
                Assert.That(!loadFriendListAfterUnfriend.Value.friendsId.Contains(idB));
            }

            Result<Friends> loadFriendListAfterGotUnfriend = null;
            lobbyB.LoadFriendsList(result => { loadFriendListAfterGotUnfriend = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListAfterGotUnfriend);

            TestHelper.Assert.IsResultOk(loadFriendListAfterGotUnfriend);

            if (loadFriendListAfterGotUnfriend.Value.friendsId.Length != 0)
            {
                Assert.That(!loadFriendListAfterGotUnfriend.Value.friendsId.Contains(idA));
            }

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Friends_Request_Reject()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            Result<Acquaintance> friendRequestRejectedNotif = null;
            lobbyA.FriendRequestRejected += result => { friendRequestRejectedNotif = result; };

            yield return TestHelper.WaitForValue(() => getFriendshipStatusBeforeRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusBeforeRequestFriend);
            Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            yield return TestHelper.WaitForValue(() => requestFriendResult);

            TestHelper.Assert.IsResultOk(requestFriendResult);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestFriend);
            Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listOutgoingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listOutgoingFriendRequestResult);
            Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestSentFromAnother);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestSentFromAnother);
            Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listIncomingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listIncomingFriendRequestResult);
            Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result rejectFriendRequestResult = null;
            lobbyB.RejectFriend(idA, result => { rejectFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => rejectFriendRequestResult);

            TestHelper.Assert.IsResultOk(rejectFriendRequestResult);

            yield return TestHelper.WaitForValue(() => friendRequestRejectedNotif);

            TestHelper.Assert.IsResultOk(friendRequestRejectedNotif);
            Assert.That(friendRequestRejectedNotif.Value.userId == idB);

            Result<FriendshipStatus> getFriendshipStatusAfterRejecting = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRejecting = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRejecting);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRejecting);
            Assert.That(
                getFriendshipStatusAfterRejecting.Value.friendshipStatus == RelationshipStatusCode.NotFriend);

            Result<Friends> listIncomingFriendsAfterRejecting = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendsAfterRejecting = result; });

            yield return TestHelper.WaitForValue(() => listIncomingFriendsAfterRejecting);

            TestHelper.Assert.IsResultOk(listIncomingFriendsAfterRejecting);
            Assert.That(!listIncomingFriendsAfterRejecting.Value.friendsId.Contains(idA));

            Result<FriendshipStatus> getFriendshipStatusAfterRejected = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRejected = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRejected);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRejected);
            Assert.That(
                getFriendshipStatusAfterRejected.Value.friendshipStatus == RelationshipStatusCode.NotFriend);

            Result<Friends> listOutgoingFriendsAfterRejected = null;
            lobbyA.ListIncomingFriends(result => { listOutgoingFriendsAfterRejected = result; });

            yield return TestHelper.WaitForValue(() => listOutgoingFriendsAfterRejected);

            TestHelper.Assert.IsResultOk(listOutgoingFriendsAfterRejected);
            Assert.That(!listOutgoingFriendsAfterRejected.Value.friendsId.Contains(idB));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Friends_Request_Cancel()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            Result<Acquaintance> friendRequestCanceledNotif = null;
            lobbyB.FriendRequestCanceled += result => { friendRequestCanceledNotif = result; };

            yield return TestHelper.WaitForValue(() => getFriendshipStatusBeforeRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusBeforeRequestFriend);
            Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            yield return TestHelper.WaitForValue(() => requestFriendResult);

            TestHelper.Assert.IsResultOk(requestFriendResult);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestFriend);
            Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listOutgoingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listOutgoingFriendRequestResult);
            Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestSentFromAnother);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestSentFromAnother);
            Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listIncomingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listIncomingFriendRequestResult);
            Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result cancelFriendRequestResult = null;
            lobbyA.CancelFriendRequest(idB, result => { cancelFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => cancelFriendRequestResult);

            TestHelper.Assert.IsResultOk(cancelFriendRequestResult);

            yield return TestHelper.WaitForValue(() => friendRequestCanceledNotif);

            TestHelper.Assert.IsResultOk(friendRequestCanceledNotif);
            Assert.That(friendRequestCanceledNotif.Value.userId == idA);

            Result<Friends> listIncomingFriendRequestAfterCanceled = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestAfterCanceled = result; });

            yield return TestHelper.WaitForValue(() => listIncomingFriendRequestAfterCanceled);

            TestHelper.Assert.IsResultOk(listIncomingFriendRequestAfterCanceled);
            Assert.That(!listIncomingFriendRequestAfterCanceled.Value.friendsId.Contains(idA));

            Result<Friends> loadFriendListAfterCanceled = null;
            lobbyB.LoadFriendsList(result => { loadFriendListAfterCanceled = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListAfterCanceled);

            TestHelper.Assert.IsResultOk(loadFriendListAfterCanceled);

            if (loadFriendListAfterCanceled.Value.friendsId.Length != 0)
            {
                Assert.That(!loadFriendListAfterCanceled.Value.friendsId.Contains(idA));
            }

            Result<Friends> loadFriendListAfterCanceling = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterCanceling = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListAfterCanceling);

            TestHelper.Assert.IsResultOk(loadFriendListAfterCanceling);

            if (loadFriendListAfterCanceling.Value.friendsId.Length != 0)
            {
                Assert.That(!loadFriendListAfterCanceling.Value.friendsId.Contains(idB));
            }

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Friends_Complete_Scenario()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<Friend> incomingNotificationFromAResult = null;
            lobbyB.OnIncomingFriendRequest += result => { incomingNotificationFromAResult = result; };

            Result<Friend> friendRequestAcceptedResult = null;
            lobbyA.FriendRequestAccepted += result => { friendRequestAcceptedResult = result; };

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(
                this.users[1].Session.UserId,
                result => { getFriendshipStatusBeforeRequestFriend = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusBeforeRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusBeforeRequestFriend);
            Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(this.users[1].Session.UserId, result => { requestFriendResult = result; });

            yield return TestHelper.WaitForValue(() => requestFriendResult);

            TestHelper.Assert.IsResultOk(requestFriendResult);

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listOutgoingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listOutgoingFriendRequestResult);
            Assert.That(
                listOutgoingFriendRequestResult.Value.friendsId.Contains(this.users[1].Session.UserId));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(
                this.users[1].Session.UserId,
                result => { getFriendshipStatusAfterRequestFriend = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestFriend);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestFriend);
            Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result cancelFriendRequestResult = null;
            lobbyA.CancelFriendRequest(this.users[1].Session.UserId, result => { cancelFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => cancelFriendRequestResult);

            TestHelper.Assert.IsResultOk(cancelFriendRequestResult);

            Result<Friends> listOutgoingFriendAfterCanceling = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendAfterCanceling = result; });

            yield return TestHelper.WaitForValue(() => listOutgoingFriendAfterCanceling);

            TestHelper.Assert.IsResultOk(listOutgoingFriendAfterCanceling);

            if (listOutgoingFriendAfterCanceling.Value.friendsId.Length != 0)
            {
                Assert.That(
                    !listOutgoingFriendAfterCanceling.Value.friendsId.Contains(this.users[1].Session.UserId));
            }

            Result requestFriendAfterCanceling = null;
            lobbyA.RequestFriend(this.users[1].Session.UserId, result => { requestFriendAfterCanceling = result; });

            yield return TestHelper.WaitForValue(() => requestFriendAfterCanceling);

            TestHelper.Assert.IsResultOk(requestFriendAfterCanceling);

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => listIncomingFriendRequestResult);

            TestHelper.Assert.IsResultOk(listIncomingFriendRequestResult);
            Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterRequestSentFromAnother);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterRequestSentFromAnother);
            Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result rejectFriendRequestResult = null;
            lobbyB.RejectFriend(idA, result => { rejectFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => rejectFriendRequestResult);

            TestHelper.Assert.IsResultOk(rejectFriendRequestResult);

            Result requestFriendAfterRejected = null;
            lobbyA.RequestFriend(this.users[1].Session.UserId, result => { requestFriendAfterRejected = result; });

            yield return TestHelper.WaitForValue(() => requestFriendAfterRejected);

            TestHelper.Assert.IsResultOk(requestFriendAfterRejected);

            yield return TestHelper.WaitForValue(() => incomingNotificationFromAResult);

            TestHelper.Assert.IsResultOk(incomingNotificationFromAResult);
            Assert.That(incomingNotificationFromAResult.Value.friendId == idA);

            Result<Friends> listIncomingFriendRequestAfterRejecting = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestAfterRejecting = result; });

            yield return TestHelper.WaitForValue(() => listIncomingFriendRequestAfterRejecting);

            TestHelper.Assert.IsResultOk(listIncomingFriendRequestAfterRejecting);
            Assert.That(listIncomingFriendRequestAfterRejecting.Value.friendsId.Contains(idA));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            yield return TestHelper.WaitForValue(() => acceptFriendRequestResult);

            TestHelper.Assert.IsResultOk(acceptFriendRequestResult);

            yield return TestHelper.WaitForValue(() => friendRequestAcceptedResult);

            TestHelper.Assert.IsResultOk(friendRequestAcceptedResult);
            Assert.That(friendRequestAcceptedResult.Value.friendId == idB);

            Result<Friends> loadFriendListAfterAccepted = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterAccepted = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListAfterAccepted);

            TestHelper.Assert.IsResultOk(loadFriendListAfterAccepted);
            Assert.That(loadFriendListAfterAccepted.Value.friendsId.Contains(this.users[1].Session.UserId));

            Result<FriendshipStatus> getFriendshipStatusAfterAccepted = null;
            lobbyA.GetFriendshipStatus(
                this.users[1].Session.UserId,
                result => { getFriendshipStatusAfterAccepted = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterAccepted);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterAccepted);
            Assert.That(
                getFriendshipStatusAfterAccepted.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Friend));

            Result<FriendshipStatus> getFriendshipStatusAfterAccepting = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterAccepting = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterAccepting);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterAccepting);
            Assert.That(
                getFriendshipStatusAfterAccepting.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Friend));

            Result unfriendResult = null;
            lobbyA.Unfriend(this.users[1].Session.UserId, result => { unfriendResult = result; });

            yield return TestHelper.WaitForValue(() => unfriendResult);

            TestHelper.Assert.IsResultOk(unfriendResult);

            Result<Friends> loadFriendListAfterUnfriend = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterUnfriend = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListAfterUnfriend);

            TestHelper.Assert.IsResultOk(loadFriendListAfterUnfriend);

            if (loadFriendListAfterUnfriend.Value.friendsId.Length != 0)
            {
                Assert.That(
                    !loadFriendListAfterUnfriend.Value.friendsId.Contains(this.users[1].Session.UserId));
            }

            Result<FriendshipStatus> getFriendshipStatusAfterUnfriended = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterUnfriended = result; });

            yield return TestHelper.WaitForValue(() => getFriendshipStatusAfterUnfriended);

            TestHelper.Assert.IsResultOk(getFriendshipStatusAfterUnfriended);
            Assert.That(
                getFriendshipStatusAfterUnfriended.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Bulk_Friends_Request_Success()
        {
            var lobbyA = CreateLobby(this.users[0].Session);

            List<string> userIds = new List<string>();
            for (int i = 1; i < users.Length; i++)
            {
                userIds.Add(users[i].Session.UserId);
            }

            if (!lobbyA.IsConnected) lobbyA.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            Result friendReqResult = null;
            lobbyA.BulkRequestFriend(userIds.ToArray(), result =>
            {
                friendReqResult = result;
            });

            yield return TestHelper.WaitForValue(() => friendReqResult);

            TestHelper.Assert.That(!friendReqResult.IsError || friendReqResult.Error.Code != ErrorCode.FriendListIsEmpty, "Bulk Friend Request Error!");

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListResult);

            TestHelper.Assert.IsResultOk(loadFriendListResult);
            foreach (var userId in userIds)
            {
                Assert.That(loadFriendListResult.Value.friendsId.Contains(userId));
            }

            foreach (var userId in userIds)
            {
                Result unfriendResult = null;
                lobbyA.Unfriend(userId, result => { unfriendResult = result; });

                yield return TestHelper.WaitForValue(() => unfriendResult);

                TestHelper.Assert.IsResultOk(unfriendResult);
            }

            lobbyA.Disconnect();
        }

#if !DISABLESTEAMWORKS
        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Bulk_Friends_Request_SteamFriend_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var steamTicket = TestHelper.GenerateSteamTicket();

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result steamLoginResult = null;
            Debug.Log("Login With Steam Ticket: " + steamTicket);
            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, result => steamLoginResult = result);
            yield return TestHelper.WaitForValue(() => steamLoginResult);

            Result<UserData> getDataResult = null;
            user.GetData(result => getDataResult = result);
            yield return TestHelper.WaitForValue(() => getDataResult);

            var steamUserId = getDataResult.Value.userId;

            List<string> steamIds = new List<string>();
            for (int i = 0; i < SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate); i++)
            {
                steamIds.Add(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate).ToString());
            }

            //Act
            Result<BulkPlatformUserIdResponse> getOtherUserDataResult = null;
            user.BulkGetUserByOtherPlatformUserIds(PlatformType.Steam, steamIds.ToArray(), result => getOtherUserDataResult = result);
            yield return TestHelper.WaitForValue(() => getOtherUserDataResult);

            List<string> userIds = new List<string>();
            foreach (var userData in getOtherUserDataResult.Value.userIdPlatforms)
            {
                userIds.Add(userData.userId);
            }

            var lobbyA = CreateLobby(user.Session);

            if (!lobbyA.IsConnected) lobbyA.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            Result friendReqResult = null;
            lobbyA.BulkRequestFriend(userIds.ToArray(), result =>
            {
                friendReqResult = result;
            });

            yield return TestHelper.WaitForValue(() => friendReqResult);

            TestHelper.Assert.That(!friendReqResult.IsError || friendReqResult.Error.Code != ErrorCode.FriendListIsEmpty, "Bulk Friend Request Error!");

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            yield return TestHelper.WaitForValue(() => loadFriendListResult);

            TestHelper.Assert.IsResultOk(loadFriendListResult);
            foreach (var userId in userIds)
            {
                Assert.That(loadFriendListResult.Value.friendsId.Contains(userId));
            }

            foreach (var userId in userIds)
            {
                Result unfriendResult = null;
                lobbyA.Unfriend(userId, result => { unfriendResult = result; });

                yield return TestHelper.WaitForValue(() => unfriendResult);

                TestHelper.Assert.IsResultOk(unfriendResult);
            }

            lobbyA.Disconnect();
        }
#endif

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator Bulk_Friends_Request_OneFLawUserId_Failed()
        {
            var lobbyA = CreateLobby(this.users[0].Session);

            List<string> userIds = new List<string>();
            for (int i = 0; i < users.Length; i++)
            {
                userIds.Add(users[i].Session.UserId);
            }

            if (!lobbyA.IsConnected) lobbyA.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            Result friendReqResult = null;
            lobbyA.BulkRequestFriend(userIds.ToArray(), result =>
            {
                friendReqResult = result;
            });

            yield return TestHelper.WaitForValue(() => friendReqResult);

            TestHelper.Assert.That(friendReqResult.IsError && friendReqResult.Error.Code != ErrorCode.FriendListIsEmpty, "Bulk Friend Request Unexpected Success!");

            lobbyA.Disconnect();
        }

        [UnityTest, TestLog, Order(2), Timeout(150000)]
        public IEnumerator Bulk_User_Get_Presence_Success()
        {
            const int expectedOffline = 2;
            Lobby[] lobbies = new Lobby[userCount - expectedOffline];
            Lobby[] offlineLobbies = new Lobby[expectedOffline];
            for (int i= 0; i < users.Length; i++)
            {
                Result logoutResult = null;
                users[i].Logout(r => logoutResult = r);

                yield return TestHelper.WaitForValue(() => logoutResult);
                yield return new WaitForSeconds(5f);

                Result loginResult = null;

                users[i]
                    .LoginWithUsername(
                        usersData[i].emailAddress,
                        "Password123",
                        result => loginResult = result);

                yield return TestHelper.WaitForValue(() => loginResult);

                Lobby lobby;
                if (i < lobbies.Length)
                {
                    lobby = lobbies[i] = CreateLobby(this.users[i].Session);
                }
                else
                {
                    lobby = lobbies[i - lobbies.Length] = CreateLobby(this.users[i].Session);
                }

                lobby.Connect();

                while (!lobby.IsConnected) yield return new WaitForSeconds(0.2f);

                Debug.Log(string.Format("User{0} Connected to lobby", i));

                if(i >= lobbies.Length)
                {
                    lobby.Disconnect();
                }
            }

            var userIds = new List<string>(); 
            for(int i = 1; i < users.Length; i++)
            {
                userIds.Add(users[i].Session.UserId);
            }

            Result<BulkUserStatusNotif> getPresenceResult = null;
            lobbies[0].BulkGetUserPresence(userIds.ToArray(), result =>
            {
                getPresenceResult = result;
            });

            yield return TestHelper.WaitForValue(() => getPresenceResult);

            bool expectedOnline = true;
            int onlineCount = 0;
            int offlineCount = 0;
            foreach(var online in getPresenceResult.Value.data)
            {
                bool isOnline = false;
                bool isExpectedOnline = false;
                for (int i = 1; i < lobbies.Length; i++) {
                    if (online.userID == users[i].Session.UserId)
                    {
                        isExpectedOnline = true;
                        if (online.availability == GeneralUserStatus.online)
                        {
                            isOnline = true;
                            onlineCount++;
                        }
                        break;
                    }
                }
                if (!isOnline && isExpectedOnline)
                {
                    expectedOnline = false;
                    break;
                }
                else if(!isExpectedOnline)
                {
                    offlineCount++;
                }
            }

            foreach(var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            Assert.True(!getPresenceResult.IsError);
            Assert.True(onlineCount == userCount - expectedOffline - 1);
            Assert.True(onlineCount == getPresenceResult.Value.online);
            Assert.True(offlineCount == expectedOffline);
            Assert.True(offlineCount == getPresenceResult.Value.offline);
            Assert.True(expectedOnline);
        }

        [UnityTest, TestLog, Order(2), Timeout(150000)]
        public IEnumerator Bulk_User_Get_Presence_CountOnly()
        {
            const int expectedOffline = 2;
            Lobby[] lobbies = new Lobby[userCount - expectedOffline];
            Lobby[] offlineLobbies = new Lobby[expectedOffline];
            for (int i = 0; i < users.Length; i++)
            {
                Result logoutResult = null;
                users[i].Logout(r => logoutResult = r);

                yield return TestHelper.WaitForValue(() => logoutResult);
                yield return new WaitForSeconds(5f);

                Result loginResult = null;

                users[i]
                    .LoginWithUsername(
                        usersData[i].emailAddress,
                        "Password123",
                        result => loginResult = result);

                yield return TestHelper.WaitForValue(() => loginResult);

                Lobby lobby;
                if (i < lobbies.Length)
                {
                    lobby = lobbies[i] = CreateLobby(this.users[i].Session);
                }
                else
                {
                    lobby = lobbies[i - lobbies.Length] = CreateLobby(this.users[i].Session);
                }

                lobby.Connect();

                while (!lobby.IsConnected) yield return new WaitForSeconds(0.2f);

                Debug.Log(string.Format("User{0} Connected to lobby", i));

                if (i >= lobbies.Length)
                {
                    lobby.Disconnect();
                }
            }

            var userIds = new List<string>();
            for (int i = 1; i < users.Length; i++)
            {
                userIds.Add(users[i].Session.UserId);
            }

            Result<BulkUserStatusNotif> getPresenceResult = null;
            lobbies[0].BulkGetUserPresence(userIds.ToArray(), result =>
            {
                getPresenceResult = result;
            }, true);

            yield return TestHelper.WaitForValue(() => getPresenceResult);

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            Assert.True(!getPresenceResult.IsError);
            Assert.True(getPresenceResult.Value.data.Length == 0);
            Assert.True(getPresenceResult.Value.online == userCount - expectedOffline - 1);
            Assert.True(getPresenceResult.Value.offline == expectedOffline);
        }

        [UnityTest, TestLog, Order(2), Timeout(50000)]
        public IEnumerator Bulk_User_Get_Presence_EmptyUserId()
        {
            Result logoutResult = null;
            users[0].Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);
            yield return new WaitForSeconds(5f);

            Result loginResult = null;

            users[0]
                .LoginWithUsername(
                    usersData[0].emailAddress,
                    "Password123",
                    result => loginResult = result);

            yield return TestHelper.WaitForValue(() => loginResult);

            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            while (!lobby.IsConnected) yield return new WaitForSeconds(0.2f);

            Debug.Log(string.Format("User Connected to lobby"));

            var userIds = new List<string>();

            Result<BulkUserStatusNotif> getPresenceResult = null;
            lobby.BulkGetUserPresence(userIds.ToArray(), result =>
            {
                getPresenceResult = result;
            });

            yield return TestHelper.WaitForValue(() => getPresenceResult);

            lobby.Disconnect();

            Assert.True(getPresenceResult.IsError);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000), Ignore("revoke flow backend changes. doesn't disconnect lobby fo now. (14 July 2021)")]
        public IEnumerator LobbyConnected_AuthTokenRevoked_Disconnected()
        {
            //Arrange
            Result<TokenData> accessTokenResult = null;
            this.helper.GetAccessToken(r => accessTokenResult = r); 
            yield return TestHelper.WaitForValue(() => accessTokenResult);

            var user = AccelBytePlugin.GetUser();
            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result); 
            yield return TestHelper.WaitForValue(() => loginResult);
            
            string userId = user.Session.UserId;
            var lobby = AccelBytePlugin.GetLobby();
            int numLobbyConnect = 0;
            int numLobbyDisconnect = 0;
            int numDisconnectNotif = 0;
            void OnConnected() => numLobbyConnect++;
            void OnDisconnected(WsCloseCode code) => numLobbyDisconnect++;
            void OnDisconnecting(Result<DisconnectNotif> result) { Debug.Log("Disconnect Message: " + result.Value.message); numDisconnectNotif++; };
            lobby.Connected += OnConnected;
            lobby.Disconnected += OnDisconnected;
            lobby.Disconnecting += OnDisconnecting;

            lobby.Connect();

            while (!lobby.IsConnected)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            user.Logout(result => logoutResult = result); 
            yield return TestHelper.WaitForValue(() => logoutResult);
            logoutResult = null;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.Elapsed < TimeSpan.FromSeconds(10))
            {
                yield return new WaitForSeconds(0.1f);
            }

            bool isLobbyConnected = lobby.IsConnected;

            lobby.Connected -= OnConnected;
            lobby.Disconnected -= OnDisconnected;
            lobby.Disconnecting -= OnDisconnecting;
            lobby.Disconnect();

            Result deleteResult = null;
            (new TestHelper()).DeleteUser(userId, result => deleteResult = result); 
            yield return TestHelper.WaitForValue(() => deleteResult);

            //Assert
            Debug.Log("lobby.IsConnected=" + isLobbyConnected);
            Debug.Log("numLobbyConnect=" + numLobbyConnect);
            Debug.Log("numLobbyDisconnect=" + numLobbyDisconnect);
            Debug.Log("numDisconnectNotif=" + numDisconnectNotif);
            Assert.That(isLobbyConnected, Is.False);
            Assert.That(logoutResult.IsError, Is.False);
            Assert.That(numLobbyConnect, Is.GreaterThan(0));
            Assert.That(numLobbyDisconnect, Is.GreaterThan(0));
            Assert.That(numDisconnectNotif, Is.GreaterThan(0));
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator LobbyConnected_SameUserConnectWithDifferentToken_CurrentLobbyDisconnected()
        {
            //Arrange
            var user = AccelBytePlugin.GetUser();

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result); 
            yield return TestHelper.WaitForValue(() => loginResult);

            var lobby = CreateLobby(user.Session);
            int numLobbyConnect = 0;
            int numLobbyDisconnect = 0;
            int numDisconnectNotif = 0;
            void OnConnected() => numLobbyConnect++;
            void OnDisconnected(WsCloseCode code) { Debug.Log("Disconnect Code: " + code); numLobbyDisconnect++; }
            void OnDisconnecting(Result<DisconnectNotif> _) => numDisconnectNotif++;
            lobby.Connected += OnConnected;
            lobby.Disconnected += OnDisconnected;
            lobby.Disconnecting += OnDisconnecting;
            lobby.Connect();

            while (!lobby.IsConnected)
            {
                yield return new WaitForSeconds(0.1f);
            }

            LoginSession loginSession = new LoginSession(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.RedirectUri,
                    this.httpClient,
                    this.coroutineRunner);

            var userAccount = new UserAccount(
                AccelBytePlugin.Config.IamServerUrl,
                AccelBytePlugin.Config.Namespace,
                loginSession,
                this.httpClient);

            yield return new WaitForSeconds(1); // wait for token generation to generate different token

            User otherUser = new User(
                loginSession,
                userAccount,
                this.coroutineRunner);

            Result otherUserLogoutResult = null;
            otherUser.Logout(r => otherUserLogoutResult = r);
            yield return TestHelper.WaitForValue(() => otherUserLogoutResult);
            Result otherUserLoginResult = null;
            otherUser.LoginWithDeviceId(result => otherUserLoginResult = result); 
            yield return TestHelper.WaitForValue(() => otherUserLoginResult);

            var otherUserLobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                new WebSocket(),
                new LobbyApi(AccelBytePlugin.Config.BaseUrl, this.httpClient),
                otherUser.Session,
                AccelBytePlugin.Config.Namespace,
                this.coroutineRunner);

            //Act
            otherUserLobby.Connect();

            while (!otherUserLobby.IsConnected)
            {
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(5f);

            bool isLobbyConnected = lobby.IsConnected;

            otherUserLobby.Disconnect();
            lobby.Disconnect();

            Result deleteResult = null;
            (new TestHelper()).DeleteUser(user, result => deleteResult = result); 
            yield return TestHelper.WaitForValue(() => deleteResult);

            //Assert
            Debug.Log("lobby.IsConnected=" + isLobbyConnected);
            Debug.Log("numLobbyConnect=" + numLobbyConnect);
            Debug.Log("numLobbyDisconnect=" + numLobbyDisconnect);
            Debug.Log("numDisconnectNotif=" + numDisconnectNotif);
            Assert.That(isLobbyConnected, Is.False);
            Assert.That(numLobbyConnect, Is.EqualTo(1));
            Assert.That(numLobbyDisconnect, Is.GreaterThan(0));
            Assert.That(numDisconnectNotif, Is.GreaterThan(0));
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator LobbyConnected_SameUserConnectWithSameToken_OtherLobbyRejected()
        {
            //Arrange
            var user = AccelBytePlugin.GetUser();

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithDeviceId( result => loginResult = result); 
            yield return TestHelper.WaitForValue(() => loginResult);

            var lobby = AccelBytePlugin.GetLobby();
            int numLobbyConnect = 0;
            int numLobbyDisconnect = 0;
            int numDisconnectNotif = 0;
            void OnConnected() => numLobbyConnect++;
            void OnDisconnected(WsCloseCode code) { Debug.Log("Disconnect Code: " + code); numLobbyDisconnect++; }
            void OnDisconnecting(Result<DisconnectNotif> _) => numDisconnectNotif++;
            lobby.Connected += OnConnected;
            lobby.Disconnected += OnDisconnected;
            lobby.Disconnecting += OnDisconnecting;
            lobby.Connect();

            yield return new WaitForSeconds(5.0f);

            var otherLobby = CreateLobby(user.Session);

            //Act
            otherLobby.Connect();

            yield return new WaitForSeconds(2f);

            bool isLobbyConnected = lobby.IsConnected;
            lobby.Connected -= OnConnected;
            lobby.Disconnected -= OnDisconnected;
            lobby.Disconnecting -= OnDisconnecting;

            otherLobby.Disconnect();
            lobby.Disconnect();

            Result deleteResult = null;
            (new TestHelper()).DeleteUser(user, result => deleteResult = result); 
            yield return TestHelper.WaitForValue(() => deleteResult);

            //Assert
            Debug.Log("lobby.IsConnected=" + isLobbyConnected);
            Debug.Log("numLobbyConnect=" + numLobbyConnect);
            Debug.Log("numLobbyDisconnect=" + numLobbyDisconnect);
            Debug.Log("numDisconnectNotif=" + numDisconnectNotif);

            Assert.That(isLobbyConnected);
            Assert.That(otherLobby.IsConnected, Is.False);
            Assert.That(numLobbyConnect, Is.EqualTo(1));
            Assert.That(numLobbyDisconnect, Is.Zero);
            Assert.That(numDisconnectNotif, Is.Zero);
        }

        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator LobbyReconnect_SameToken_withSessionIdHeader()
        {
            //Arrange
            var user = AccelBytePlugin.GetUser();

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);

            yield return TestHelper.WaitForValue(() => loginResult);

            var lobby = CreateLobby(user.Session);
            int numLobbyConnect = 0;
            int numLobbyDisconnect = 0;
            int numDisconnectNotif = 0;
            void OnConnected() => numLobbyConnect++;
            void OnDisconnected(WsCloseCode code) => numLobbyDisconnect++;
            void OnDisconnecting(Result<DisconnectNotif> _) => numDisconnectNotif++;
            lobby.Connected += OnConnected;
            lobby.Disconnected += OnDisconnected;
            lobby.Disconnecting += OnDisconnecting;
            lobby.Connect();

            yield return new WaitForSeconds(5.0f);

            lobby.Disconnect();

            yield return new WaitForSeconds(5.0f);

            //Act
            lobby.Connect();

            yield return new WaitForSeconds(5.0f);

            bool isLobbyConnected = lobby.IsConnected;
            lobby.Connected -= OnConnected;
            lobby.Disconnected -= OnDisconnected;
            lobby.Disconnecting -= OnDisconnecting;

            lobby.Disconnect();

            Result deleteResult = null;
            (new TestHelper()).DeleteUser(user, result => deleteResult = result);

            yield return TestHelper.WaitForValue(() => deleteResult);

            //Assert
            Debug.Log("lobby.IsConnected=" + isLobbyConnected);
            Debug.Log("numLobbyConnect=" + numLobbyConnect);
            Debug.Log("numLobbyDisconnect=" + numLobbyDisconnect);
            Debug.Log("numDisconnectNotif=" + numDisconnectNotif);

            Assert.That(isLobbyConnected);
            Assert.That(numLobbyConnect, Is.GreaterThan(1));
            Assert.That(numLobbyDisconnect, Is.GreaterThan(0));
        }

        #region Block/Unblock Player

        [UnityTest, TestLog, Order(3), Timeout(35000)]
        public IEnumerator BlockFriend_NotFriendAnymore()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (both must be friend first)
            Result sentFriendRequestResult = null;
            lobbyA.RequestFriend(idB, r => sentFriendRequestResult = r);
            yield return TestHelper.WaitForValue(() => sentFriendRequestResult);

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, r => acceptFriendRequestResult = r);
            yield return TestHelper.WaitForValue(() => acceptFriendRequestResult);

            Result<Friends> loadFriendResultAfterBefriend = null;
            lobbyA.LoadFriendsList(r => loadFriendResultAfterBefriend = r );
            yield return TestHelper.WaitForValue(() => loadFriendResultAfterBefriend);
            bool bothWereFriend = loadFriendResultAfterBefriend.Value.friendsId.Contains(idB);
            
            // Act (block friend)
            Result<BlockPlayerResponse> blockPlayerResult = null;
            lobbyA.BlockPlayer(idB, r => blockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            
            // Assert
            Result<Friends> loadFriendResultAfterBlocking = null;
            lobbyA.LoadFriendsList(r => loadFriendResultAfterBlocking = r );
            yield return TestHelper.WaitForValue(() => loadFriendResultAfterBlocking);
            bool bothAreNotFriend = loadFriendResultAfterBlocking.Value.friendsId.Contains(idB) == false;
            
            // Cleanup
            Result<UnblockPlayerResponse> unblockPlayerResult = null;
            lobbyA.UnblockPlayer(idB, r => unblockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => unblockPlayerResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            Assert.IsTrue(bothWereFriend);
            Assert.IsFalse(blockPlayerResult.IsError);
            Assert.IsTrue(bothAreNotFriend);
        }

        [UnityTest, TestLog, Order(3), Timeout(35000)]
        public IEnumerator BlockPlayerSuccess_GotNotif()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (notif listener for the targeted player)
            bool blockNotifAccepted_B = false;
            PlayerBlockedNotif blockNotifResult_B = null;
            ResultCallback<PlayerBlockedNotif> targetedPlayerNotifListener = result =>
            {
                blockNotifAccepted_B = true;
                if (result.IsError) return;
                blockNotifResult_B = result.Value;
            };
            lobbyB.PlayerBlockedNotif += targetedPlayerNotifListener;
            
            // Arrange (notif listener for the block issuer)
            bool blockNotifAccepted_A = false;
            PlayerBlockedNotif blockNotifResult_A = null;
            ResultCallback<PlayerBlockedNotif> issuerNotifListener = result =>
            {
                blockNotifAccepted_A = true;
                if (result.IsError) return;
                blockNotifResult_A = result.Value;
            };
            lobbyA.PlayerBlockedNotif += issuerNotifListener;
            
            // Act (block player)
            Result<BlockPlayerResponse> blockPlayerResult = null;
            lobbyA.BlockPlayer(idB, r => blockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            while (!blockNotifAccepted_B) yield return new WaitForSeconds(.2f);
            while (!blockNotifAccepted_A) yield return new WaitForSeconds(.2f);
            
            // Cleanup
            Result<UnblockPlayerResponse> unblockPlayerResult = null;
            lobbyA.UnblockPlayer(idB, r => unblockPlayerResult = r);
            lobbyB.PlayerBlockedNotif -= targetedPlayerNotifListener;
            lobbyA.PlayerBlockedNotif -= issuerNotifListener;
            yield return TestHelper.WaitForValue(() => unblockPlayerResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            // Assert
            Assert.IsTrue(blockNotifResult_B.blockedUserId == idB);
            Assert.IsTrue(blockNotifResult_B.userId == idA);
            Assert.IsTrue(blockNotifResult_A.blockedUserId == idB);
            Assert.IsTrue(blockNotifResult_A.userId == idA);
            Assert.IsFalse(blockPlayerResult.IsError);
        }

        [UnityTest, TestLog, Order(3), Timeout(35000)]
        public IEnumerator UnblockPlayerSuccess_GotUnblockNotif()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (notif listener for the targeted player)
            bool unblockNotifAccepted_B = false;
            PlayerUnblockedNotif unblockNotifResult_B = null;
            ResultCallback<PlayerUnblockedNotif> targetedPlayerNotifListener = result =>
            {
                unblockNotifAccepted_B = true;
                if (result.IsError) return;
                unblockNotifResult_B = result.Value;
            };
            lobbyB.PlayerUnblockedNotif += targetedPlayerNotifListener;
            
            // Arrange (notif listener for the unblock issuer)
            bool unblockNotifAccepted_A = false;
            PlayerUnblockedNotif unblockNotifResult_A = null;
            ResultCallback<PlayerUnblockedNotif> issuerNotifListener = result =>
            {
                unblockNotifAccepted_A = true;
                if (result.IsError) return;
                unblockNotifResult_A = result.Value;
            };
            lobbyA.PlayerUnblockedNotif += issuerNotifListener;
            
            // Act (block then unblock player)
            Result<BlockPlayerResponse> blockPlayerResult = null;
            lobbyA.BlockPlayer(idB, r => blockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            Result<UnblockPlayerResponse> unblockPlayerResult = null;
            lobbyA.UnblockPlayer(idB, r => unblockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            while (!unblockNotifAccepted_B) yield return new WaitForSeconds(.2f);
            while (!unblockNotifAccepted_A) yield return new WaitForSeconds(.2f);
            
            // Cleanup
            lobbyB.PlayerUnblockedNotif -= targetedPlayerNotifListener;
            lobbyA.PlayerUnblockedNotif -= issuerNotifListener;
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            // Assert
            Assert.IsTrue(unblockNotifResult_B.unblockedUserId == idB);
            Assert.IsTrue(unblockNotifResult_B.userId == idA);
            Assert.IsTrue(unblockNotifResult_A.unblockedUserId == idB);
            Assert.IsTrue(unblockNotifResult_A.userId == idA);
            Assert.IsFalse(blockPlayerResult.IsError);
            Assert.IsFalse(unblockPlayerResult.IsError);
        }

        [UnityTest, TestLog, Order(3), Timeout(35000)]
        public IEnumerator BlockPlayer_GetListOfBlocked()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (block player)
            Result<BlockPlayerResponse> blockPlayerResult = null;
            lobbyA.BlockPlayer(idB, r => blockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            
            // Act (Issuer GET list of blocked user)
            Result<BlockedList> getListResult = null;
            lobbyA.GetListOfBlockedUser(r => getListResult = r);
            yield return TestHelper.WaitForValue(() => getListResult);

            bool isExpectedIdExist = false;
            foreach(var blockedUser in getListResult.Value.data)
            {
                if(blockedUser.blockedUserId == idB)
                {
                    isExpectedIdExist = true;
                    break;
                }
            }

            // Cleanup
            Result<UnblockPlayerResponse> unblockPlayerResult = null;
            lobbyA.UnblockPlayer(idB, r => unblockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => unblockPlayerResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            // Assert
            Assert.IsFalse(getListResult.IsError);
            Assert.IsTrue(isExpectedIdExist);
            Assert.IsFalse(blockPlayerResult.IsError);
        }

        [UnityTest, TestLog, Order(3), Timeout(30000)]
        public IEnumerator BlockedByPlayer_GetListOfBlocker()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (block player)
            Result<BlockPlayerResponse> blockPlayerResult = null;
            lobbyA.BlockPlayer(idB, r => blockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            
            // Act (Blocked player GET list of blocker)
            Result<BlockerList> getListResult = null;
            lobbyB.GetListOfBlocker(r => getListResult = r);
            yield return TestHelper.WaitForValue(() => getListResult);

            bool isExpectedIdExist = false;
            foreach (var blockedUser in getListResult.Value.data)
            {
                if (blockedUser.userId == idA)
                {
                    isExpectedIdExist = true;
                    break;
                }
            }

            // Cleanup
            Result<UnblockPlayerResponse> unblockPlayerResult = null;
            lobbyA.UnblockPlayer(idB, r => unblockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => unblockPlayerResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            // Assert
            Assert.IsFalse(getListResult.IsError);
            Assert.IsTrue(isExpectedIdExist);
            Assert.IsFalse(blockPlayerResult.IsError);
        }

        [UnityTest, TestLog, Order(3), Timeout(30000)]
        public IEnumerator BlockPlayer_CanNotDoActionsToBlocker()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (block player)
            Result<BlockPlayerResponse> blockPlayerResult = null;
            lobbyA.BlockPlayer(idB, r => blockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            
            // Act1. Add friend is not allowed
            Result addFriendResult = null;
            lobbyA.RequestFriend(idB, r => addFriendResult = r);

            bool thereIsIncomingFriendRequest = false;
            ResultCallback<Friend> onIncomingFriendRequest = r =>
            {
                if (!r.IsError && r.Value.friendId == idA)
                {
                    thereIsIncomingFriendRequest = true;
                }
            };
            lobbyB.OnIncomingFriendRequest += onIncomingFriendRequest; 
            
            // Act2. Direct chat is not allowed
            Result directChatResult = null;
            lobbyA.SendPersonalChat(idB, "TEST", r => directChatResult = r);

            bool thereIsIncomingDirectChat = false;
            ResultCallback<ChatMessage> onIncomingDirectChat = r =>
            {
                if (!r.IsError && r.Value.@from == idA)
                {
                    thereIsIncomingDirectChat = true;
                }
            };
            lobbyB.PersonalChatReceived += onIncomingDirectChat; 
            
            // Act3. Invite party is not allowed
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);

            bool thereIsIncomingPartyInvitation = false;
            ResultCallback<PartyInvitation> onInvitedToParty = r =>
            {
                if (!r.IsError && r.Value.@from == idA)
                {
                    thereIsIncomingPartyInvitation = true;
                }
            };
            lobbyB.InvitedToParty += onInvitedToParty; 
            
            // TODO
            // Act4. Invite group is not allowed
            // Act5. Global Chat filter test 
            
            // ActFinal. Waiter
            yield return TestHelper.WaitForValue(() => addFriendResult);
            yield return TestHelper.WaitForValue(() => directChatResult);
            yield return TestHelper.WaitForValue(() => invitePartyResult);

            // Cleanup
            lobbyB.OnIncomingFriendRequest -= onIncomingFriendRequest; 
            lobbyB.PersonalChatReceived -= onIncomingDirectChat;
            lobbyB.InvitedToParty -= onInvitedToParty;
            Result<UnblockPlayerResponse> unblockPlayerResult = null;
            lobbyA.UnblockPlayer(idB, r => unblockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => unblockPlayerResult);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            // Assert
            Assert.IsFalse(blockPlayerResult.IsError);
            Assert.IsTrue(addFriendResult.IsError);
            Assert.IsTrue(directChatResult.IsError);
            Assert.IsTrue(invitePartyResult.IsError);
            Assert.IsFalse(thereIsIncomingFriendRequest);
            Assert.IsFalse(thereIsIncomingDirectChat);
            Assert.IsFalse(thereIsIncomingPartyInvitation);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator UnblockPlayer_CanDoActionsToBlocker()
        {
            var lobbyA = CreateLobby(this.users[0].Session);
            var lobbyB = CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId, idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Arrange (user A create party)
            Result leavePartyResult = null;
            lobbyA.LeaveParty(r => { leavePartyResult = r; });
            yield return TestHelper.WaitForValue(() => leavePartyResult);
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);
            
            // Arrange (block player then unblock player)
            Result<BlockPlayerResponse> blockPlayerResult = null;
            lobbyA.BlockPlayer(idB, r => blockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => blockPlayerResult);
            
            Result<UnblockPlayerResponse> unblockPlayerResult = null;
            lobbyA.UnblockPlayer(idB, r => unblockPlayerResult = r);
            yield return TestHelper.WaitForValue(() => unblockPlayerResult);
            
            // Act1. Add friend is ALLOWED
            Result addFriendResult = null;
            lobbyA.RequestFriend(idB, r => addFriendResult = r);

            bool thereIsIncomingFriendRequest = false;
            ResultCallback<Friend> onIncomingFriendRequest = r =>
            {
                if (!r.IsError && r.Value.friendId == idA)
                {
                    thereIsIncomingFriendRequest = true;
                }
            };
            lobbyB.OnIncomingFriendRequest += onIncomingFriendRequest; 
            
            // Act2. Direct chat is ALLOWED
            Result directChatResult = null;
            lobbyA.SendPersonalChat(idB, "TEST", r => directChatResult = r);

            bool thereIsIncomingDirectChat = false;
            ResultCallback<ChatMessage> onIncomingDirectChat = r =>
            {
                if (!r.IsError && r.Value.@from == idA)
                {
                    thereIsIncomingDirectChat = true;
                }
            };
            lobbyB.PersonalChatReceived += onIncomingDirectChat; 
            
            // Act3. Invite party is ALLOWED
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);

            bool thereIsIncomingPartyInvitation = false;
            ResultCallback<PartyInvitation> onInvitedToParty = r =>
            {
                if (!r.IsError && r.Value.@from == idA)
                {
                    thereIsIncomingPartyInvitation = true;
                }
            };
            lobbyB.InvitedToParty += onInvitedToParty; 
            
            // TODO
            // Act4. Invite group is ALLOWED
            // Act5. Global Chat filter test 
            
            // ActFinal. Waiter
            yield return TestHelper.WaitForValue(() => addFriendResult);
            yield return TestHelper.WaitForValue(() => directChatResult);
            yield return TestHelper.WaitForValue(() => invitePartyResult);

            // Cleanup
            lobbyB.OnIncomingFriendRequest -= onIncomingFriendRequest; 
            lobbyB.PersonalChatReceived -= onIncomingDirectChat;
            lobbyB.InvitedToParty -= onInvitedToParty;
            
            Result leavePartyResultB = null, leavePartyResultA = null;
            lobbyB.LeaveParty(r => leavePartyResultB = r);
            lobbyA.LeaveParty(r => leavePartyResultA = r);
            yield return TestHelper.WaitForValue(() => leavePartyResultB);
            yield return TestHelper.WaitForValue(() => leavePartyResultA);
            lobbyA.Disconnect();
            lobbyB.Disconnect();
            
            // Assert
            Assert.IsFalse(blockPlayerResult.IsError);
            Assert.IsFalse(unblockPlayerResult.IsError);
            Assert.IsFalse(addFriendResult.IsError);
            Assert.IsFalse(directChatResult.IsError);
            Assert.IsFalse(invitePartyResult.IsError);
            Assert.IsTrue(thereIsIncomingFriendRequest);
            Assert.IsTrue(thereIsIncomingDirectChat);
            Assert.IsTrue(thereIsIncomingPartyInvitation);
        }
        

        #endregion

        #region Entitlement Check Connection
        /***********************************************************************************
         * Setup needed to run test
         * 
         * 1. create an item that have appId in publisher namespace
         * 2. enable the entitlement check in lobby config
         * 3. add item id for entitlement check in lobby config
         * 
         * Don't forget to disable entitlement check in lobby config to test another lobby tests. 
         ***********************************************************************************/

        [UnityTest, TestLog, Order(1), Timeout(1000000), Ignore("Don't run in demo env.")]
        public IEnumerator ConnectLobbyWithEntitlementCheck_NotOwnedApp_ConnectionFailed()
        {
            // Arrange
            string[] skus = new string[] { "sdktestSkuApp001" };

            var user = AccelBytePlugin.GetUser();

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            var lobby = CreateLobby(user.Session);
            int numLobbyConnect = 0;
            int numLobbyDisconnect = 0;
            void OnConnected() => numLobbyConnect++;
            void OnDisconnected(WsCloseCode code) => numLobbyDisconnect++;
            lobby.Connected += OnConnected;
            lobby.Disconnected += OnDisconnected;

            // Act
            lobby.SetConnectionTokenGenerator(new EntitlementTokenGenerator(null, null, skus));
            lobby.Connect();
            yield return TestHelper.WaitUntil(() => numLobbyConnect != 0 || numLobbyDisconnect != 0);

            bool isLobbyConnected = lobby.IsConnected;

            lobby.Disconnect();

            lobby.Connected -= OnConnected;
            lobby.Disconnected -= OnDisconnected;

            Result deleteResult = null;
            (new TestHelper()).DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            // Assert
            Debug.Log("numLobbyConnect=" + numLobbyConnect);
            Debug.Log("numLobbyDisconnect=" + numLobbyDisconnect);

            Assert.IsFalse(isLobbyConnected);
            Assert.That(numLobbyConnect, Is.EqualTo(0));
            Assert.That(numLobbyDisconnect, Is.EqualTo(1));
        }

        [UnityTest, TestLog, Order(2), Timeout(1000000), Ignore("Don't run in demo env.")]
        public IEnumerator ConnectLobbyWithEntitlementCheck_OwnedApp_ConnectionSuccessful()
        {
            // Arrange
            string accessToken;
            string publishedStoreId;
            string itemId;
            string publisherUserId;
            string publisherNamespace = AccelBytePlugin.Config.PublisherNamespace;
            string sku = "sdktestSkuApp001";
            string[] skus = new string[] { sku };

            // Arrange (Get Access Token)
            Result<TokenData> accessTokenResult = null;
            this.helper.GetAccessToken(r => accessTokenResult = r);
            yield return TestHelper.WaitForValue(() => accessTokenResult);

            accessToken = accessTokenResult.Value.access_token;
            TestHelper.Assert.IsResultOk(accessTokenResult);

            // Arrange (Get Publisher Store Id)
            Result<TestHelper.StoreInfoModel> getPublishedStore = null;
            this.helper.GetPublishedStore(publisherNamespace, accessToken, res => getPublishedStore = res);
            yield return TestHelper.WaitForValue(() => getPublishedStore);

            publishedStoreId = getPublishedStore.Value.storeId;
            TestHelper.Assert.IsResultOk(getPublishedStore);

            // Arrange (Get Item Id from SKU)
            Result<TestHelper.FullItemInfo> getItemResult = null;
            this.helper.GetItemBySKU(publisherNamespace, accessToken, publishedStoreId, sku, true, result => getItemResult = result);
            yield return TestHelper.WaitForValue(() => getItemResult);

            itemId = getItemResult.Value.itemId;
            TestHelper.Assert.IsResultOk(getItemResult);

            // Arrange (Setup User)
            var user = AccelBytePlugin.GetUser();

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            // Arrange (Get Publisher User Id)
            Result<TestHelper.UserMapResponse> userMapResult = null;
            this.helper.GetUserMap(user.Session.UserId, res => userMapResult = res);
            yield return TestHelper.WaitForValue(() => userMapResult);

            publisherUserId = userMapResult.Value.UserId;
            TestHelper.Assert.IsResultOk(userMapResult);

            // Arrange (Grant App to User)
            TestHelper.FulfillmentModel fulfillmentItemRequest = new TestHelper.FulfillmentModel
            {
                itemId = itemId,
                quantity = 1,
            };

            Result fulfillItemResult = null;
            this.helper.FulfillItem(publisherNamespace, accessToken, publisherUserId, fulfillmentItemRequest, result => fulfillItemResult = result);
            yield return TestHelper.WaitForValue(() => fulfillItemResult);

            TestHelper.Assert.IsResultOk(fulfillItemResult);

            // Arrange (Setup Lobby)
            var lobby = CreateLobby(user.Session);
            int numLobbyConnect = 0;
            int numLobbyDisconnect = 0;
            void OnConnected() => numLobbyConnect++;
            void OnDisconnected(WsCloseCode code) => numLobbyDisconnect++;
            lobby.Connected += OnConnected;
            lobby.Disconnected += OnDisconnected;

            // Act
            lobby.SetConnectionTokenGenerator(new EntitlementTokenGenerator(null, null, skus));
            lobby.Connect();
            yield return TestHelper.WaitUntil(() => numLobbyConnect != 0 || numLobbyDisconnect != 0);

            bool isLobbyConnected = lobby.IsConnected;

            lobby.Disconnect();

            lobby.Connected -= OnConnected;
            lobby.Disconnected -= OnDisconnected;

            Result deleteResult = null;
            (new TestHelper()).DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            // Assert
            Debug.Log("numLobbyConnect=" + numLobbyConnect);
            Debug.Log("numLobbyDisconnect=" + numLobbyDisconnect);

            Assert.That(isLobbyConnected);
            Assert.That(numLobbyConnect, Is.EqualTo(1));
            Assert.That(numLobbyDisconnect, Is.EqualTo(0));
        }
        #endregion

        #region Session Attributes
        [UnityTest, TestLog, Order(2), Timeout(100000)]
        public IEnumerator SessionAttribute_Set_Get()
        {
            //Arrange
            Dictionary<string, string> attributes = new Dictionary<string, string>()
            {
                {"mmr", "10" },
                {"sr", "20" }
            };

            var user = AccelBytePlugin.GetUser();

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            var lobby = CreateLobby(user.Session);
            lobby.Connect();

            yield return new WaitForSeconds(5.0f);

            int setAttributeSuccessCount = 0;
            bool isSetAttributeError = false;
            ResultCallback setAttributeCallback = (result) =>
            {
                if (result.IsError)
                {
                    isSetAttributeError = true;
                }
                else
                {
                    setAttributeSuccessCount++;
                }
            };

            //Act
            foreach(var attribute in attributes)
            {
                lobby.SetSessionAttribute(attribute.Key, attribute.Value, setAttributeCallback);
            }

            yield return TestHelper.WaitUntil(() => { return isSetAttributeError || (setAttributeSuccessCount == attributes.Count()); }, "Wait user set session attribute", 10000);

            Result<GetSessionAttributeResponse> getResult = null;
            lobby.GetSessionAttribute(attributes.Keys.ToArray()[0], (result) => getResult = result);

            yield return TestHelper.WaitForValue(() => getResult, "Waiting user get session attribute", 5000);

            Result<GetSessionAttributeAllResponse> getAllResult = null;
            lobby.GetSessionAttributeAll((result) => getAllResult = result);

            yield return TestHelper.WaitForValue(() => getAllResult, "Waiting user get all session attribute", 10000);

            lobby.Disconnect();

            Result deleteResult = null;
            (new TestHelper()).DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            //Assert
            Assert.IsFalse(isSetAttributeError);

            Assert.IsFalse(getResult.IsError);
            Assert.AreEqual(attributes[attributes.Keys.ToArray()[0]], getResult.Value.value);

            Assert.IsFalse(getAllResult.IsError);
            bool isNotMatch = attributes.Count() != getAllResult.Value.attributes.Count();
            foreach(var attribute in attributes)
            {
                if(!getAllResult.Value.attributes.ContainsKey(attribute.Key))
                {
                    isNotMatch = true;
                    break;
                }

                if(getAllResult.Value.attributes[attribute.Key] != attribute.Value)
                {
                    isNotMatch = true;
                    break;
                }
            }
            Assert.IsFalse(isNotMatch);
        }
        #endregion
    }
}
