using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Tests.IntegrationTests
{
    [TestFixture]
    class ServerLobbyTest
    {
        private TestHelper helper;
        private User[] users;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private UserData[] usersData;
        private List<Lobby> userLobbies = new List<Lobby>();
        private string clientAccessToken;

        private const int userCount = 1;

        private Lobby CreateLobby(User user)
        {
            var lobby = LobbyTestUtil.CreateLobby(user.Session, this.httpClient, this.coroutineRunner);
            userLobbies.Add(lobby);
            return lobby;
        }

        private IEnumerator CleanupBeforeTestCase(Action<bool> actionOnDone)
        {
            int leavingPartyCount = 0;
            for (int i = 0; i < this.userLobbies.Count; i++)
            {
                if (this.users[i] != null && this.users[i].Session.IsValid())
                {
                    this.userLobbies[i].LeaveParty(result => { Interlocked.Increment(ref leavingPartyCount); });
                }
            }
            while (leavingPartyCount < this.userLobbies.Count) yield return new WaitForSeconds(0.04f);
            actionOnDone.Invoke(true);
        }

        [UnitySetUp]
        public IEnumerator A_Setup()
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

                Result loginDS = null;
                AccelByteServerPlugin.GetDedicatedServer().LoginWithClientCredentials(result => loginDS = result);
                yield return TestHelper.WaitForValue(() => loginDS, "Logging in DS");
            }

            if (this.users == null || this.users.Length == 0)
            {
                var newUsers = new User[userCount];
                this.usersData = new UserData[userCount];
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
                            string.Format("serverlobbyuser+{0}{1}@example.com", i + 1, guid),
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
                            string.Format("serverlobbyuser+{0}{1}@example.com", i + 1, guid),
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

        [UnityTearDown]
        public IEnumerator CleanupLobbyConnections()
        {
            foreach (var lobby in userLobbies)
            {
                if (lobby.IsConnected)
                {
                    Debug.LogWarning("[LOBBY] Dangling websocket connection, previous test are not closing WS connection, please disconnect it at the of of the tests, disconnecting...");
                    lobby.Disconnect();
                }
            }
            userLobbies.Clear();
            yield return null;
        }

        [UnityTest, TestLog, Order(2), Timeout(60000)]
        public IEnumerator Z_Teardown()
        {
            for (int i = 0; i < userCount; i++)
            {
                Result deleteUserResult = null;
                this.helper.DeleteUser(this.usersData[i].userId, result => { deleteUserResult = result; });
                yield return TestHelper.WaitForValue(() => deleteUserResult);
                TestHelper.Assert.IsResultOk(deleteUserResult);
            }

            Result logoutResult = null;
            AccelBytePlugin.GetUser().Logout(result => logoutResult = result);
            yield return TestHelper.WaitForValue(() => logoutResult);
        }

        [UnityTest, TestLog, Order(1), Timeout(60000)]
        public IEnumerator Server_SessionAttributGetSet_AllOk()
        {
            ServerLobby serverLobby = AccelByteServerPlugin.GetLobby();
            // Setup attributes, user & lobby connection
            Dictionary<string, string> attributes = new Dictionary<string, string>()
            {
                { "mmr", "20" },
                { "sr", "10" }
            };
            var attributesKey = attributes.Keys.ToArray();
            User user = this.users[0];

            Lobby lobby = CreateLobby(user);

            lobby.Connect();

            yield return TestHelper.WaitUntil(() => { return lobby.IsConnected; }, "waiting lobby connect");

            // ACT, client set attribute, server set attribute, then server get & getAll attribute
            Result clientSetAttributeResult = null;
            lobby.SetSessionAttribute(attributesKey[0], attributes[attributesKey[0]], (result) => clientSetAttributeResult = result);
            yield return TestHelper.WaitForValue(() => clientSetAttributeResult, "Waiting client set attribute", 60000);

            Result serverSetAttributeResult = null;
            serverLobby.SetSessionAttribute(user.Session.UserId, attributesKey[1], attributes[attributesKey[1]], (result) => serverSetAttributeResult = result);
            yield return TestHelper.WaitForValue(() => serverSetAttributeResult, "Waiting server set attribute", 60000);

            Result<ServerGetSessionAttributeResponse> serverGetResult = null;
            serverLobby.GetSessionAttribute(user.Session.UserId, attributesKey[0], (result) => serverGetResult = result);
            yield return TestHelper.WaitForValue(() => serverGetResult, "Waiting server get attribute", 60000);

            Result<GetSessionAttributeAllResponse> serverGetAllResult = null;
            serverLobby.GetSessionAttributeAll(user.Session.UserId, (result) => serverGetAllResult = result);
            yield return TestHelper.WaitForValue(() => serverGetAllResult, "Waiting server get all attribute", 60000);

            // Asserts
            Assert.IsFalse(clientSetAttributeResult.IsError);
            Assert.IsFalse(serverSetAttributeResult.IsError);
            Assert.IsFalse(serverGetResult.IsError);
            Assert.AreEqual(attributesKey[0], serverGetResult.Value.key);
            Assert.AreEqual(attributes[attributesKey[0]], serverGetResult.Value.value);
            Assert.IsFalse(serverGetAllResult.IsError);
            bool contentInconsistent = serverGetAllResult.Value.attributes.Count != attributes.Count;
            if(!contentInconsistent)
            {
                foreach(var attribute in attributes)
                {
                    if(!serverGetAllResult.Value.attributes.ContainsKey(attribute.Key))
                    {
                        contentInconsistent = true;
                        Debug.Log($"get all attribute inconsistent, key {attribute.Key} not found");
                        break;
                    }
                
                    if(serverGetAllResult.Value.attributes[attribute.Key] != attribute.Value)
                    {
                        contentInconsistent = true;
                        Debug.Log($"get all attribute inconsistent, key {attribute.Key} has value {serverGetAllResult.Value.attributes[attribute.Key]}, expected {attribute.Value}");
                        break;
                    }
                }
            }
            else
            {
                Debug.Log($"get all attribute inconsistent, count is different expected {attributes.Count} actual is {serverGetAllResult.Value.attributes.Count}");
            }
            Assert.IsFalse(contentInconsistent);
        }
    }
}
