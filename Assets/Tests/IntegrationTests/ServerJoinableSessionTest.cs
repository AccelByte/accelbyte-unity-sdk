using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using HybridWebSocket;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class ServerJoinableSessionTest
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

        const string joinableChannelName = "joinable";
        const string nonJoinableChannelName = "nonjoinable";
        const string podName = "sdktestds";

        private TestHelper helper;
        private User[] users;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private UserData[] usersData;
        private const int userCount = 2;
        private List<Lobby> activeLobbies = new List<Lobby>();
        private string clientAccessToken;

        private bool isHelperSetup = false;
        private bool isMMSetup = false;
        private bool isUserSetup = false;
        private bool isDSRegistered = false;
        private MatchmakingResult SessionData = null;
        private string SessionId = "";
        private bool isSessionQueued = false;

        private string joinableChannelNameGenerated;
        private string nonJoinableChannelNameGenerated;
        private string podNameGenerated;

        Lobby CreateLobby(ISession session)
        {
            Lobby lobby = LobbyTestUtil.CreateLobby(session, this.httpClient, coroutineRunner);
            lobby.Connect();
            activeLobbies.Add(lobby);
            return lobby;
        }

        [UnityTest, TestLog, Order(0)]
        public IEnumerator A_Setup()
        {
            SetupHelper();

            if(string.IsNullOrEmpty(clientAccessToken))
            {
                Result<TokenData> accessTokenResult = null;
                this.helper.GetAccessToken(result =>
                {
                    Debug.Log($"access token result null {accessTokenResult == null}, result null {result == null}");
                    accessTokenResult = result;
                });
                yield return TestHelper.WaitForValue(() => accessTokenResult, "Waiting access token");
                clientAccessToken = accessTokenResult.Value.access_token;
            }

            if(!isMMSetup)
            {
                // Create MM Channel joinable
                Result joinableCreated = null;
                joinableChannelNameGenerated = TestHelper.GenerateUnique(joinableChannelName);
                yield return helper.CreateMatchmakingChannelAsync(
                    clientAccessToken, 
                    joinableChannelNameGenerated, 
                    true,
                    new TestHelper.RuleSet
                    {
                        alliance = new TestHelper.AllianceRule
                        {
                            min_number = 1, max_number = 2, player_min_number = 1, player_max_number = 1
                        }
                    },
                    result => joinableCreated = result);
                // Creeate MM Channel NonJoinable
                Result nonJoinableCreated = null;
                nonJoinableChannelNameGenerated = TestHelper.GenerateUnique(nonJoinableChannelName);
                yield return helper.CreateMatchmakingChannelAsync(
                    clientAccessToken, 
                    nonJoinableChannelNameGenerated,
                    false,
                    new TestHelper.RuleSet
                    {
                        alliance =  new TestHelper.AllianceRule
                        {
                            min_number = 1, max_number = 2, player_min_number = 1, player_max_number = 1
                        }
                    }, 
                    r => nonJoinableCreated = r);

                yield return TestHelper.WaitForValue(() => joinableCreated, "Creating Joinable MM");
                yield return TestHelper.WaitForValue(() => nonJoinableCreated, "Creating non joinable MM");

                isMMSetup = true;
            }

            if(!isUserSetup)
            {
                var newUsers = new User[2];
                this.usersData = new UserData[2];
                var guid = Guid.NewGuid().ToString("N");

                for (int i = 0; i < newUsers.Length; i++)
                {
                    Result<RegisterUserResponse> registerResult = null;
                    var loginSession = new LoginSession(
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
            }
        }

        [UnityTest, TestLog, Order(999)]
        public IEnumerator Z_TearDown()
        {
            SetupHelper();

            if (string.IsNullOrEmpty(clientAccessToken))
            {
                Result<TokenData> accessTokenResult = null;
                this.helper.GetAccessToken(result =>
                {
                    Debug.Log($"access token result null {accessTokenResult == null}, result null {result == null}");
                    accessTokenResult = result;
                });
                yield return TestHelper.WaitForValue(() => accessTokenResult, "Waiting access token");
                clientAccessToken = accessTokenResult.Value.access_token;
            }

            // Delete MM Channel Joinable
            Result joinableDeleted = null;
            yield return helper.DeleteMatchmakingChannelAsync(clientAccessToken, joinableChannelNameGenerated, result => joinableDeleted = result);
            // Delete MM Channel NonJoinable
            Result nonJoinableDeleted = null;
            yield return helper.DeleteMatchmakingChannelAsync(clientAccessToken, nonJoinableChannelNameGenerated, result => nonJoinableDeleted = result);

            yield return TestHelper.WaitForValue(() => joinableDeleted, "Deleting Joinable MM");
            yield return TestHelper.WaitForValue(() => nonJoinableDeleted, "Deleting non joinable MM");

            isMMSetup = false;
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


            if(isSessionQueued && !String.IsNullOrEmpty(SessionId))
            {
                Debug.Log("Dequeueing Joinable Session");
                Result dequeueResult = null;
                AccelByteServerPlugin.GetMatchmaking().DequeueJoinableSession(SessionId, r => dequeueResult = r);
                yield return TestHelper.WaitForValue(() => dequeueResult, "Dequeueing Joinable Session");
                isSessionQueued = false;
            }

            if(isDSRegistered)
            {
                Debug.Log("Cleaning Registered Local DS");
                Result deregisterResult = null;
                AccelByteServerPlugin.GetDedicatedServerManager().DeregisterLocalServer(result => deregisterResult = result);
                yield return TestHelper.WaitForValue(() => deregisterResult, "Deregistering Local Server");
                isDSRegistered = false;
                SessionData = null;
            }
            yield break;
        }

        private void SetupHelper()
        {
            if(!isHelperSetup)
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

                isHelperSetup = true;
            }
        }

        private IEnumerator RegisterDS()
        {
            var ds = AccelByteServerPlugin.GetDedicatedServer();
            var dsm = AccelByteServerPlugin.GetDedicatedServerManager();
            var mm = AccelByteServerPlugin.GetMatchmaking();

            Result dsLoginResult = null;
            ds.LoginWithClientCredentials(result => dsLoginResult = result);
            yield return TestHelper.WaitForValue(() => dsLoginResult, "Logging in DS");
            TestHelper.Assert.IsResultOk(dsLoginResult, "Login DS Success");

            Result registerToDSMResult = null;

            Result deregisterResult = null;
            dsm.DeregisterLocalServer(result => deregisterResult = result);
            yield return TestHelper.WaitForValue(() => deregisterResult);
            TestHelper.LogResult(deregisterResult, "deregister DS before setup ");

            podNameGenerated = TestHelper.GenerateUnique(podName);
            dsm.RegisterLocalServer("127.0.0.1", 7777, podNameGenerated, result => registerToDSMResult = result);
            yield return TestHelper.WaitForValue(() => registerToDSMResult, "Registering to DSM");
            TestHelper.Assert.IsResultOk(registerToDSMResult, "Register to DSM Success");

            isDSRegistered = true;
        }

        private IEnumerator RegisterSessionAsync(string sessionId)
        {
            var mm = AccelByteServerPlugin.GetMatchmaking();

            Result<MatchmakingResult> sessionQuery = null;
            mm.QuerySessionStatus(sessionId, sesRes =>
            {
                sessionQuery = sesRes;
            });
            yield return TestHelper.WaitForValue(() => sessionQuery, "wait query session");
            TestHelper.Assert.IsResultOk(sessionQuery, "Query Joinable Session");

            Debug.Log("Session is " + (sessionQuery.Value.joinable?"JOINABLE":"NonJoinable"));

            if (sessionQuery.Value.joinable)
            {
                Result enqueueResult = null;
                mm.EnqueueJoinableSession(sessionQuery.Value, enqResult => 
                {
                    enqueueResult = enqResult;
                });

                yield return TestHelper.WaitForValue(() => enqueueResult, "wait enqueue joinable session");
                TestHelper.Assert.IsResultOk(enqueueResult, "Enqueue Joinable Session");
                isSessionQueued = true;
            }
        }

        private IEnumerator GetSessionIdAndQueueSession()
        {
            var mm = AccelByteServerPlugin.GetMatchmaking();
            var dsm = AccelByteServerPlugin.GetDedicatedServerManager();

            Result<ServerSessionResponse> sessionIdResult = null;
            dsm.GetSessionId(result => sessionIdResult = result);
            yield return TestHelper.WaitForValue(() => sessionIdResult, "Wait get session id");
            TestHelper.Assert.IsResultOk(sessionIdResult, "Result GetSessionId");
            SessionId = sessionIdResult.Value.session_id;

            Result<MatchmakingResult> sessionStatusResult = null;
            mm.QuerySessionStatus(SessionId, result => sessionStatusResult = result);
            yield return TestHelper.WaitForValue(() => sessionStatusResult, "Wait query session status");
            TestHelper.Assert.IsResultOk(sessionStatusResult, "Result QuerySessionStatus");
            SessionData = sessionStatusResult.Value;

            if (SessionData.joinable)
            {
                Result enqueueResult = null;
                mm.EnqueueJoinableSession(SessionData, result => enqueueResult = result);
                yield return TestHelper.WaitForValue(() => enqueueResult, "Wait queue session");
                TestHelper.Assert.IsResultOk(enqueueResult, "Result QueueSession");
            }

            yield break;
        }

        [UnityTest, TestLog, Order(1), Timeout(120000)]
        public IEnumerator TwoPartyMatchmake_JoinableSession_SameSession()
        {
            //Arrange
            Lobby lobbyA = CreateLobby(users[0].Session);
            Lobby lobbyB = CreateLobby(users[1].Session);
            
            string AMatchId = null;
            string BMatchId = null;

            lobbyA.MatchmakingCompleted += (result) =>
            {
                Debug.Log("A Received Matchmaking Completed, MatchID " + result.Value.matchId);
                lobbyA.ConfirmReadyForMatch(result.Value.matchId, confirmResult => AMatchId = result.Value.matchId);
            };

            lobbyB.DSUpdated += (result) =>
            {
                Debug.Log("B Received DS Update, MatchID " + result.Value.matchId);
                BMatchId = result.Value.matchId;
            };

            // register local DS & set heartbeat
            yield return coroutineRunner.Run(RegisterDS());

            // player A complete matchmaking flow with joinable gamemode channel
            Result<PartyInfo> ACreatePartyResult = null;
            lobbyA.CreateParty(result => ACreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => ACreatePartyResult, "Lobby A creating party");

            Result<MatchmakingCode> AMatchmakingResult = null;
            lobbyA.StartMatchmaking(joinableChannelNameGenerated, podNameGenerated, result => AMatchmakingResult = result);
            yield return TestHelper.WaitForValue(() => AMatchmakingResult);
            TestHelper.Assert.IsResultOk(AMatchmakingResult);

            yield return TestHelper.WaitWhile(() => string.IsNullOrEmpty(AMatchId));


            // DS set query session, enqueue joinable session
            yield return GetSessionIdAndQueueSession();

            yield return new WaitForSeconds(5f);

            // player B complete matchmaking with joinable gamemode channel
            Result<PartyInfo> BCreatePartyResult = null;
            lobbyB.CreateParty(result => BCreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => BCreatePartyResult, "Lobby B creating party");

            Result<MatchmakingCode> BMatchmakingResult = null;
            lobbyB.StartMatchmaking(joinableChannelNameGenerated, podNameGenerated, result => BMatchmakingResult = result);
            yield return TestHelper.WaitForValue(() => BMatchmakingResult, "wait B start matchmaking");
            TestHelper.Assert.IsResultOk(BMatchmakingResult, "B Start Matchmaking result");

            yield return TestHelper.WaitForValue(() => BMatchId);

            while(string.IsNullOrEmpty(BMatchId))
            {
                yield return new WaitForSeconds(0.2f);
            }

            for(int i = activeLobbies.Count - 1; i >= 0; i--)
            {
                activeLobbies[i].LeaveParty(r => { });
                activeLobbies[i].Disconnect();
                activeLobbies.RemoveAt(i);
            }


            Assert.IsFalse(string.IsNullOrEmpty(AMatchId));
            Assert.IsFalse(string.IsNullOrEmpty(BMatchId));
            Assert.AreEqual(AMatchId, BMatchId);
        }

        [UnityTest, TestLog, Order(1), Timeout(120000)]
        public IEnumerator AddRemovePlayerManual_JoinableSession_AllOk()
        {
            //Arrange
            Lobby lobbyA = CreateLobby(users[0].Session);

            string AMatchId = null;

            lobbyA.MatchmakingCompleted += (result) =>
            {
                Debug.Log("A Received Matchmaking Completed, MatchID " + result.Value.matchId);
                lobbyA.ConfirmReadyForMatch(result.Value.matchId, confirmResult => AMatchId = result.Value.matchId);
            };

            // register local DS & set heartbeat
            yield return coroutineRunner.Run(RegisterDS());

            // player A complete matchmaking flow with joinable gamemode channel
            Result<PartyInfo> ACreatePartyResult = null;
            lobbyA.CreateParty(result => ACreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => ACreatePartyResult, "Lobby A creating party");

            Result<MatchmakingCode> AMatchmakingResult = null;
            lobbyA.StartMatchmaking(joinableChannelNameGenerated, podNameGenerated, result => AMatchmakingResult = result);
            yield return TestHelper.WaitForValue(() => AMatchmakingResult);
            TestHelper.Assert.IsResultOk(AMatchmakingResult);

            yield return TestHelper.WaitWhile(() => string.IsNullOrEmpty(AMatchId));


            // DS set query session, enqueue joinable session
            yield return GetSessionIdAndQueueSession();

            yield return new WaitForSeconds(3f);

            Result AddUserResult = null;
            AccelByteServerPlugin.GetMatchmaking().AddUserToSession(SessionData.game_mode, SessionData.match_id, users[1].Session.UserId, result => AddUserResult = result);
            yield return TestHelper.WaitForValue(() => AddUserResult);

            Result<MatchmakingResult> mmResultAfterAddUser = null;
            AccelByteServerPlugin.GetMatchmaking().QuerySessionStatus(SessionData.match_id, result => mmResultAfterAddUser = result);
            yield return TestHelper.WaitForValue(() => mmResultAfterAddUser);

            Result removeUserResult = null;
            AccelByteServerPlugin.GetMatchmaking().RemoveUserFromSession(SessionData.game_mode, SessionData.match_id, users[1].Session.UserId, result => removeUserResult = result);
            yield return TestHelper.WaitForValue(() => removeUserResult);

            Result<MatchmakingResult> mmResultAfterRemoveUser = null;
            AccelByteServerPlugin.GetMatchmaking().QuerySessionStatus(SessionData.match_id, result => mmResultAfterRemoveUser = result);
            yield return TestHelper.WaitForValue(() => mmResultAfterRemoveUser);

            for (int i = activeLobbies.Count - 1; i >= 0; i--)
            {
                activeLobbies[i].LeaveParty(r => { });
                activeLobbies[i].Disconnect();
                activeLobbies.RemoveAt(i);
            }

            // Assertions
            Assert.IsFalse(string.IsNullOrEmpty(AMatchId));
            bool userIdExist = false;
            foreach(var ally in SessionData.matching_allies)
            {
                foreach(var party in ally.matching_parties)
                {
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsFalse(userIdExist, "First User id not exist in session");

            Assert.IsFalse(AddUserResult.IsError);
            Assert.IsFalse(mmResultAfterAddUser.IsError);
            userIdExist = false;
            foreach (var ally in mmResultAfterAddUser.Value.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsTrue(userIdExist, "Add User id exist in session");

            Assert.IsFalse(removeUserResult.IsError);
            Assert.IsFalse(mmResultAfterRemoveUser.IsError);
            userIdExist = false;
            foreach (var ally in mmResultAfterRemoveUser.Value.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsFalse(userIdExist, "Remover User id not exist in session");
        }


        [UnityTest, TestLog, Order(1), Timeout(120000)]
        public IEnumerator AddRemovePlayerManual_JoinableSession_WithOptionalPartyParam_AllOk()
        {
            //Arrange
            Lobby lobbyA = CreateLobby(users[0].Session);
            Lobby lobbyB = CreateLobby(users[1].Session);

            string AMatchId = null;

            lobbyA.MatchmakingCompleted += (result) =>
            {
                Debug.Log("A Received Matchmaking Completed, MatchID " + result.Value.matchId);
                lobbyA.ConfirmReadyForMatch(result.Value.matchId, confirmResult => AMatchId = result.Value.matchId);
            };

            // register local DS & set heartbeat
            yield return coroutineRunner.Run(RegisterDS());

            // player A complete matchmaking flow with joinable gamemode channel
            Result<PartyInfo> ACreatePartyResult = null;
            lobbyA.CreateParty(result => ACreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => ACreatePartyResult, "Lobby A creating party");

            Result<MatchmakingCode> AMatchmakingResult = null;
            lobbyA.StartMatchmaking(joinableChannelNameGenerated, podNameGenerated, result => AMatchmakingResult = result);
            yield return TestHelper.WaitForValue(() => AMatchmakingResult);
            TestHelper.Assert.IsResultOk(AMatchmakingResult);

            yield return TestHelper.WaitWhile(() => string.IsNullOrEmpty(AMatchId));

            // DS set query session, enqueue joinable session
            yield return GetSessionIdAndQueueSession();

            yield return new WaitForSeconds(3f);

            Result<PartyInfo> BCreatePartyResult = null;
            lobbyB.CreateParty(result => BCreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => BCreatePartyResult, "Lobby B creating party");

            Result AddUserResult = null;
            AccelByteServerPlugin.GetMatchmaking().AddUserToSession(SessionData.game_mode, SessionData.match_id, users[1].Session.UserId, result => AddUserResult = result, BCreatePartyResult.Value.partyID);
            yield return TestHelper.WaitForValue(() => AddUserResult);

            Result<MatchmakingResult> mmResultAfterAddUser = null;
            AccelByteServerPlugin.GetMatchmaking().QuerySessionStatus(SessionData.match_id, result => mmResultAfterAddUser = result);
            yield return TestHelper.WaitForValue(() => mmResultAfterAddUser);

            Result removeUserResult = null;
            AccelByteServerPlugin.GetMatchmaking().RemoveUserFromSession(SessionData.game_mode, SessionData.match_id, users[1].Session.UserId, result => removeUserResult = result);
            yield return TestHelper.WaitForValue(() => removeUserResult);

            Result<MatchmakingResult> mmResultAfterRemoveUser = null;
            AccelByteServerPlugin.GetMatchmaking().QuerySessionStatus(SessionData.match_id, result => mmResultAfterRemoveUser = result);
            yield return TestHelper.WaitForValue(() => mmResultAfterRemoveUser);

            for (int i = activeLobbies.Count - 1; i >= 0; i--)
            {
                activeLobbies[i].LeaveParty(r => { });
                activeLobbies[i].Disconnect();
                activeLobbies.RemoveAt(i);
            }

            // Assertions
            Assert.IsFalse(string.IsNullOrEmpty(AMatchId));
            bool userIdExist = false;
            bool partyIdExist = false;
            foreach (var ally in SessionData.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    if(party.party_id == BCreatePartyResult.Value.partyID)
                    {
                        partyIdExist = true;
                    }
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsFalse(partyIdExist, "Add user with party id exist in session");
            Assert.IsFalse(userIdExist, "First User id not exist in session");

            Assert.IsFalse(AddUserResult.IsError);
            Assert.IsFalse(mmResultAfterAddUser.IsError);
            userIdExist = false;
            partyIdExist = false;
            foreach (var ally in mmResultAfterAddUser.Value.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    if (party.party_id == BCreatePartyResult.Value.partyID)
                    {
                        partyIdExist = true;
                    }
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsTrue(partyIdExist, "Add user with party id exist in session");
            Assert.IsTrue(userIdExist, "Add User id exist in session");

            Assert.IsFalse(removeUserResult.IsError);
            Assert.IsFalse(mmResultAfterRemoveUser.IsError);
            userIdExist = false;
            partyIdExist = false;
            foreach (var ally in mmResultAfterRemoveUser.Value.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    if (party.party_id == BCreatePartyResult.Value.partyID)
                    {
                        partyIdExist = true;
                    }
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsFalse(partyIdExist, "Remove with party id exist in session");
            Assert.IsFalse(userIdExist, "Remove user id not exist in session");
        }

        [UnityTest, TestLog, Order(2), Timeout(120000)]
        public IEnumerator TwoPartyMatchmake_NonJoinableSession_DifferentSession()
        {
            //Arrange
            Lobby lobbyA = CreateLobby(users[0].Session);
            Lobby lobbyB = CreateLobby(users[1].Session);

            string AMatchId = null;
            string BMatchId = null;

            lobbyA.MatchmakingCompleted += (result) =>
            {
                Debug.Log("A Received Matchmaking Completed, MatchID " + result.Value.matchId);
                lobbyA.ConfirmReadyForMatch(result.Value.matchId, confirmResult => AMatchId = result.Value.matchId);
            };

            lobbyB.MatchmakingCompleted += (result) =>
            {
                Debug.Log("B Received Matchmaking Completed, MatchID " + result.Value.matchId);
                lobbyB.ConfirmReadyForMatch(result.Value.matchId, null);
                BMatchId = result.Value.matchId;
            };

            lobbyB.DSUpdated += (result) =>
            {
                Debug.Log("B Received DS Update, MatchID " + result.Value.matchId);
                BMatchId = result.Value.matchId;
            };

            // register local DS & set heartbeat
            yield return coroutineRunner.Run(RegisterDS());

            // player A complete matchmaking flow with joinable gamemode channel
            Result<PartyInfo> ACreatePartyResult = null;
            lobbyA.CreateParty(result => ACreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => ACreatePartyResult, "Lobby A creating party");

            Result<MatchmakingCode> AMatchmakingResult = null;
            lobbyA.StartMatchmaking(nonJoinableChannelNameGenerated, podNameGenerated, result => AMatchmakingResult = result);
            yield return TestHelper.WaitForValue(() => AMatchmakingResult);
            TestHelper.Assert.IsResultOk(AMatchmakingResult);

            yield return TestHelper.WaitWhile(() => string.IsNullOrEmpty(AMatchId));

            // DS set query session, enqueue joinable session
            yield return GetSessionIdAndQueueSession();

            // player B complete matchmaking with joinable gamemode channel
            Result<PartyInfo> BCreatePartyResult = null;
            lobbyB.CreateParty(result => BCreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => BCreatePartyResult, "Lobby B creating party");

            Result<MatchmakingCode> BMatchmakingResult = null;
            lobbyB.StartMatchmaking(nonJoinableChannelNameGenerated, podNameGenerated, result => BMatchmakingResult = result);
            yield return TestHelper.WaitForValue(() => BMatchmakingResult);
            TestHelper.Assert.IsResultOk(BMatchmakingResult);

            yield return TestHelper.WaitForValue(() => BMatchId);

            for (int i = activeLobbies.Count - 1; i >= 0; i--)
            {
                activeLobbies[i].LeaveParty(r => { });
                activeLobbies[i].Disconnect();
                activeLobbies.RemoveAt(i);
            }

            Assert.IsFalse(string.IsNullOrEmpty(AMatchId));
            Assert.IsFalse(string.IsNullOrEmpty(BMatchId));
            Assert.AreNotEqual(AMatchId, BMatchId);
        }

        [UnityTest, TestLog, Order(2), Timeout(120000)]
        public IEnumerator AddRemovePlayerManual_NonJoinableSession_AddFailed_RemoveOk()
        {
            //Arrange
            Lobby lobbyA = CreateLobby(users[0].Session);

            string AMatchId = null;

            lobbyA.MatchmakingCompleted += (result) =>
            {
                Debug.Log("A Received Matchmaking Completed, MatchID " + result.Value.matchId);
                lobbyA.ConfirmReadyForMatch(result.Value.matchId, confirmResult => AMatchId = result.Value.matchId);
            };

            // register local DS & set heartbeat
            yield return coroutineRunner.Run(RegisterDS());

            // player A complete matchmaking flow with joinable gamemode channel
            Result<PartyInfo> ACreatePartyResult = null;
            lobbyA.CreateParty(result => ACreatePartyResult = result);
            yield return TestHelper.WaitForValue(() => ACreatePartyResult, "Lobby A creating party");

            Result<MatchmakingCode> AMatchmakingResult = null;
            lobbyA.StartMatchmaking(nonJoinableChannelNameGenerated, podNameGenerated, result => AMatchmakingResult = result);
            yield return TestHelper.WaitForValue(() => AMatchmakingResult);
            TestHelper.Assert.IsResultOk(AMatchmakingResult);

            yield return TestHelper.WaitWhile(() => string.IsNullOrEmpty(AMatchId));


            // DS set query session, enqueue joinable session
            yield return GetSessionIdAndQueueSession();

            yield return new WaitForSeconds(3f);

            Result AddUserResult = null;
            AccelByteServerPlugin.GetMatchmaking().AddUserToSession(SessionData.game_mode, SessionData.match_id, users[1].Session.UserId, result => AddUserResult = result);
            yield return TestHelper.WaitForValue(() => AddUserResult);

            Result<MatchmakingResult> mmResultAfterAddUser = null;
            AccelByteServerPlugin.GetMatchmaking().QuerySessionStatus(SessionData.match_id, result => mmResultAfterAddUser = result);
            yield return TestHelper.WaitForValue(() => mmResultAfterAddUser);

            Result removeUserResult = null;
            AccelByteServerPlugin.GetMatchmaking().RemoveUserFromSession(SessionData.game_mode, SessionData.match_id, users[0].Session.UserId, result => removeUserResult = result);
            yield return TestHelper.WaitForValue(() => removeUserResult);

            Result<MatchmakingResult> mmResultAfterRemoveUser = null;
            AccelByteServerPlugin.GetMatchmaking().QuerySessionStatus(SessionData.match_id, result => mmResultAfterRemoveUser = result);
            yield return TestHelper.WaitForValue(() => mmResultAfterRemoveUser);

            for (int i = activeLobbies.Count - 1; i >= 0; i--)
            {
                activeLobbies[i].LeaveParty(r => { });
                activeLobbies[i].Disconnect();
                activeLobbies.RemoveAt(i);
            }

            // Assertions
            Assert.IsFalse(string.IsNullOrEmpty(AMatchId));
            bool userIdExist = false;
            foreach (var ally in SessionData.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsFalse(userIdExist, "First User id not exist in session");

            Assert.IsTrue(AddUserResult.IsError);
            Assert.IsFalse(mmResultAfterAddUser.IsError);
            userIdExist = false;
            foreach (var ally in mmResultAfterAddUser.Value.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[1].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsFalse(userIdExist, "Add User id exist in session");

            Assert.IsFalse(removeUserResult.IsError);
            Assert.IsFalse(mmResultAfterRemoveUser.IsError);
            userIdExist = false;
            foreach (var ally in mmResultAfterRemoveUser.Value.matching_allies)
            {
                foreach (var party in ally.matching_parties)
                {
                    foreach (var member in party.party_members)
                    {
                        if (member.user_id == users[0].Session.UserId)
                        {
                            userIdExist = true;
                        }
                    }
                }
            }
            Assert.IsFalse(userIdExist, "Remover User id not exist in session");
        }
    }
}