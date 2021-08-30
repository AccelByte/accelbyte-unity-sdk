using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using Tests.UnitTests;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Endpoint involved:
    /// * Lobby party member notification test
    /// * GameServer PUT/GET party storage
    /// * GameClient GET party storage
    /// </summary>
    [TestFixture]
    //[Ignore("Temporary Igonre")]
    public class PartyStorageTest
    {
        private TestHelper helper;
        private User[] users;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private UserData[] usersData;
        private Lobby[] userLobbies;

        private const int PARTY_SIZE = 4;
        private const string EMAIL_PREFIX = "party_storage_unity+";
        private const string EMAIL_SUFFIX = "@accelbyte.net";
        private const string DISPLAYNAME_PREFIX = "PartyDataTester";
        private const string TEST_PASSWORD = "par+yST0R4G3";

        [Serializable]
        private struct DropItem
        {
            [DataMember] public string name { get; set; }
            [DataMember] public int quantity { get; set; }
            [DataMember] public float rarity { get; set; }

            public bool Compare(DropItem other)
            {
                return (this.name == other.name && this.quantity == other.quantity && this.rarity == other.rarity);
            }
        }

        private readonly Dictionary<string, object> PARTY_DATA = AwesomeFormatTest.PARTY_DATA;
        
        private Lobby CreateLobby(User user)
        {
            return LobbyTestUtil.CreateLobby(user.Session, this.httpClient, this.coroutineRunner);
        }
        
        private IEnumerator CleanupBeforeTestCase(Action<bool> actionOnDone)
        {
            int leavingPartyCount = 0;
            for (int i = 0; i < this.userLobbies.Length; i++)
            {
                if (this.users[i] != null && this.users[i].Session.IsValid())
                {
                    this.userLobbies[i].LeaveParty(result => { Interlocked.Increment(ref leavingPartyCount);});
                }
            }
            while (leavingPartyCount < this.userLobbies.Length) yield return new WaitForSeconds(0.04f);
            actionOnDone.Invoke(true);
        }

        private IEnumerator FormParty(int leaderIndex, Action onPartyFormed)
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.1f);
   
            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            this.userLobbies[leaderIndex].CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // Invite to party
            int invitationSentCount = 0;
            for (int i = 0; i < PartyStorageTest.PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    this.userLobbies[leaderIndex].InviteToParty(this.users[i].Session.UserId, result => { Interlocked.Increment(ref invitationSentCount); });
                }
            }
            while (invitationSentCount < PartyStorageTest.PARTY_SIZE - 1) yield return new WaitForSeconds(0.2f);
            
            // Join party
            int joinPartyCount = 0;
            for (int i = 0; i < PartyStorageTest.PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    this.userLobbies[i].JoinParty(createPartyResult.Value.partyID, createPartyResult.Value.invitationToken, result => { Interlocked.Increment(ref joinPartyCount); });
                }
            }
            while (joinPartyCount < PartyStorageTest.PARTY_SIZE - 1) yield return new WaitForSeconds(0.2f);

            Result<PartyInfo> getPartyResult = null;
            this.userLobbies[leaderIndex].GetPartyInfo(result => getPartyResult = result);
            yield return TestHelper.WaitForValue(() => getPartyResult);

            // ASSERT
            Assert.That(getPartyResult.IsError, Is.False);
            Assert.That(getPartyResult.Value, Is.Not.Null);
            Assert.That(getPartyResult.Value.members.Length, Is.EqualTo(PartyStorageTest.PARTY_SIZE));
            
            onPartyFormed.Invoke();
        }

        private void OverwritePartyStorage(Dictionary<string, object> partyData, string valuePartyId, ResultCallback<PartyDataUpdateNotif> result)
        {
            AccelByteServerPlugin.GetLobby().WritePartyStorage(valuePartyId, result, x => partyData, 7);
        }

        private void GameClientOverwritePartyStorage(Lobby lobby, Dictionary<string, object> partyData, string valuePartyId, ResultCallback<PartyDataUpdateNotif> result)
        {
            lobby.WritePartyStorage(valuePartyId, result, x => partyData, 7);
        }

        [UnityTest, TestLog, Order(0), Timeout(40000)]
        public IEnumerator PartyStorageSetup()
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

            this.users = new User[PartyStorageTest.PARTY_SIZE];
            this.usersData = new UserData[PartyStorageTest.PARTY_SIZE];
            this.userLobbies = new Lobby[PartyStorageTest.PARTY_SIZE];

            // Users creation
            for (int i = 0; i < PartyStorageTest.PARTY_SIZE; i++)
            {
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

                this.users[i] = new User(
                    loginSession,
                    userAccount,
                    this.coroutineRunner);
                
                // Create the user using email & password
                string email = PartyStorageTest.EMAIL_PREFIX + $"{i}" + PartyStorageTest.EMAIL_SUFFIX;
                string displayName = PartyStorageTest.DISPLAYNAME_PREFIX + $"{i}";
                Result<RegisterUserResponse> registerResult = null;
                this.users[i].Register(email, PartyStorageTest.TEST_PASSWORD, displayName, "US", DateTime.Now.AddYears(-22), result =>
                {
                    registerResult = result;
                });
                yield return TestHelper.WaitForValue(() => registerResult);
                TestHelper.LogResult(registerResult, "PartyStorage Setup: register user " + (i + 1));
      
                // Login the user to get the UserData
                Result loginResult = null;
                this.users[i].LoginWithUsername(email, PartyStorageTest.TEST_PASSWORD, result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                Result<UserData> userResult = null;
                this.users[i].GetData(result => userResult = result);
                yield return TestHelper.WaitForValue(() => userResult);

                this.usersData[i] = userResult.Value;
                TestHelper.LogResult(loginResult, "PartyStorage Setup: Logged in " + userResult.Value.displayName);

                this.userLobbies[i] = this.CreateLobby(this.users[i]);
                this.userLobbies[i].Connect();
            }

            // Login user to be used by Game Client
            Result logoutResult = null;
            AccelBytePlugin.GetUser().Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginDeviceIdResult = null;
            AccelBytePlugin.GetUser().LoginWithDeviceId(result => { loginDeviceIdResult = result;});
            yield return TestHelper.WaitForValue(() => loginDeviceIdResult);
            
            // Login the GameServer
            Result gameServerLoginResult = null;
            AccelByteServerPlugin.GetDedicatedServer().LoginWithClientCredentials(result => gameServerLoginResult = result);
            yield return TestHelper.WaitForValue(() => gameServerLoginResult);
            
            // Assertion
            for (int i = 0; i < PartyStorageTest.PARTY_SIZE; i++)
            {
                Assert.IsTrue(this.users[i].Session.IsValid());
            }
            Assert.IsFalse(loginDeviceIdResult.IsError);
            Assert.IsFalse(gameServerLoginResult.IsError);
        }
        
        [UnityTest, TestLog, Order(99999), Timeout(30000)]
        public IEnumerator PartyStorageTeardown()
        {
            for (int i = 0; i < PartyStorageTest.PARTY_SIZE; i++)
            {
                this.userLobbies[i].LeaveParty(result => { });
                this.userLobbies[i].Disconnect();
                Result deleteUserResult = null;
                this.helper.DeleteUser(this.usersData[i].userId, result => { deleteUserResult = result; });
                yield return TestHelper.WaitForValue(() => deleteUserResult);
                TestHelper.Assert.IsResultOk(deleteUserResult);
            }

            Result logoutResult = null;
            AccelBytePlugin.GetUser().Logout(result => logoutResult = result);
            yield return TestHelper.WaitForValue(() => logoutResult);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator FormAParty()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);
        }
        
        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_OverwritePartyStorage_ValidPartyId_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);
            
            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(20000)]
        public IEnumerator GameClient_OverwritePartyStorage_ValidPartyId_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);

            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.GameClientOverwritePartyStorage(this.userLobbies[0], this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_WriteEmptyPartyStorage_ValidPartyId_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            var emptyPartyData = new Dictionary<string, object>();
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(emptyPartyData, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(20000)]
        public IEnumerator GameClient_WriteEmptyPartyStorage_ValidPartyId_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);

            var emptyPartyData = new Dictionary<string, object>();
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.GameClientOverwritePartyStorage(this.userLobbies[0], emptyPartyData, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_OverwritePartyStorage_InvalidPartyId_Failed()
        {
            string invalidPartyId = "loremIpsumParty12345!!!";
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, invalidPartyId, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            // ASSERT
            Assert.IsTrue(OverwritePartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(20000)]
        public IEnumerator GameClient_OverwritePartyStorage_InvalidPartyId_Failed()
        {
            string invalidPartyId = "loremIpsumParty12345!!!";
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.GameClientOverwritePartyStorage(this.userLobbies[0], this.PARTY_DATA, invalidPartyId, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            // ASSERT
            Assert.IsTrue(OverwritePartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_GetPartyStorage_ValidPartyStorage_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            Result<PartyDataUpdateNotif> getPartyStorageResult = null;
            AccelByteServerPlugin.GetLobby().GetPartyStorage(getPartyInfoResult.Value.partyID, result => getPartyStorageResult = result );
            yield return TestHelper.WaitForValue(() => getPartyStorageResult);

            // ASSERT
            Assert.IsFalse(getPartyStorageResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
            bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, getPartyStorageResult.Value.custom_attribute);
            Assert.IsTrue(contentIdentical);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_GetPartyStorage_ValidPartyButEmptyStorage_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);

            // cleanup
            Result<PartyDataUpdateNotif> writePartyResult = null;
            this.OverwritePartyStorage(new Dictionary<string, object>(), getPartyInfoResult.Value.partyID, result => writePartyResult = result );
            yield return TestHelper.WaitForValue(() => writePartyResult);
            
            // act
            Result<PartyDataUpdateNotif> getPartyStorageResult = null;
            AccelByteServerPlugin.GetLobby().GetPartyStorage(getPartyInfoResult.Value.partyID, result => getPartyStorageResult = result );
            yield return TestHelper.WaitForValue(() => getPartyStorageResult);

            // ASSERT
            Assert.IsFalse(getPartyStorageResult.IsError);
            bool isAttributeNull = getPartyStorageResult.Value.custom_attribute == null;
            if (isAttributeNull)
            {
                Assert.IsTrue(getPartyStorageResult.Value.custom_attribute.Count == 0);
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_GetPartyStorage_NonexistentPartyId_Failed()
        {
            string nonExistentPartyId = "loremipsum12345";
            Result<PartyDataUpdateNotif> getPartyStorageResult = null;
            AccelByteServerPlugin.GetLobby().GetPartyStorage(nonExistentPartyId, result => getPartyStorageResult = result );
            yield return TestHelper.WaitForValue(() => getPartyStorageResult);
            
            Assert.IsTrue(getPartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(20000)]
        public IEnumerator GameClient_GetPartyStorage_ValidPartyStorage_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            Result<PartyDataUpdateNotif> getPartyStorageResult = null;
            AccelBytePlugin.GetLobby().GetPartyStorage(getPartyInfoResult.Value.partyID, result => getPartyStorageResult = result );
            yield return TestHelper.WaitForValue(() => getPartyStorageResult);

            // ASSERT
            Assert.IsFalse(getPartyStorageResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
            bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, getPartyStorageResult.Value.custom_attribute);
            Assert.IsTrue(contentIdentical);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameClient_GetPartyStorage_ValidPartyButEmptyStorage_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);

            Result<PartyDataUpdateNotif> getPartyStorageResult = null;
            AccelBytePlugin.GetLobby().GetPartyStorage(getPartyInfoResult.Value.partyID, result => getPartyStorageResult = result );
            yield return TestHelper.WaitForValue(() => getPartyStorageResult);

            // ASSERT
            Assert.IsFalse(getPartyStorageResult.IsError);
            Assert.IsTrue(getPartyStorageResult.Value.custom_attribute.Count == 0);
        }
        
        [UnityTest, TestLog, Order(1), Timeout(20000)]
        public IEnumerator GameClient_GetPartyStorage_NonexistentPartyId_Failed()
        {
            string nonExistentPartyId = "loremipsum12345";
            Result<PartyDataUpdateNotif> getPartyStorageResult = null;
            AccelBytePlugin.GetLobby().GetPartyStorage(nonExistentPartyId, result => getPartyStorageResult = result );
            yield return TestHelper.WaitForValue(() => getPartyStorageResult);
            
            Assert.IsTrue(getPartyStorageResult.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_OverwritePartyStorage_PartyFormed_SuccessGotNotification()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            var partyDataNotifResults = new ConcurrentQueue<Result<PartyDataUpdateNotif>>(); 
            for (var i = 0; i < this.userLobbies.Length; i++)
            {
                this.userLobbies[i].PartyDataUpdateNotif += result =>
                { 
                    partyDataNotifResults.Enqueue(result);
                };
            }

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            // Game Server Write PARTY_DATA
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            while (partyDataNotifResults.Count < this.userLobbies.Length) yield return new WaitForSeconds(0.2f);
            
            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
            foreach (var updateNotif in partyDataNotifResults)
            {
                Assert.IsFalse(updateNotif.IsError);
                bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, updateNotif.Value?.custom_attribute);
                Assert.IsTrue(contentIdentical);
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(40000)]
        public IEnumerator GameClient_OverwritePartyStorage_PartyFormed_SuccessGotNotification()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            var partyDataNotifResults = new ConcurrentQueue<Result<PartyDataUpdateNotif>>();
            for (var i = 0; i < this.userLobbies.Length; i++)
            {
                this.userLobbies[i].PartyDataUpdateNotif += result =>
                {
                    partyDataNotifResults.Enqueue(result);
                };
            }

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);

            // Game Server Write PARTY_DATA
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.GameClientOverwritePartyStorage(this.userLobbies[0], this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            while (partyDataNotifResults.Count < this.userLobbies.Length) yield return new WaitForSeconds(0.2f);

            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
            foreach (var updateNotif in partyDataNotifResults)
            {
                Assert.IsFalse(updateNotif.IsError);
                bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, updateNotif.Value?.custom_attribute);
                Assert.IsTrue(contentIdentical);
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(40000)]
        public IEnumerator GameServer_OverwritePartyStorage_PartyFormed_SuccessGotNotification_THROTTLE()
        {
            var lobby = this.userLobbies[0];
            
            // cleanup
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            // notif listen racing update party data
            var partyDataNotifs = new ConcurrentQueue<Result<PartyDataUpdateNotif>>();
            lobby.PartyDataUpdateNotif += result => partyDataNotifs.Enqueue(result);

            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);
            Stopwatch stopwatch = Stopwatch.StartNew();

            yield return new WaitUntil(() => partyDataNotifs.Count > 0 || stopwatch.Elapsed.Seconds > 5);

            partyDataNotifs.TryDequeue(out var notifAfterCreateParty);
            
            // spam update party data
            int spamUpdateCount = 10;
            int serverUpdateSuccessCount = 0;
            int serverUpdateDone = 0;
            for (int i = 0; i < spamUpdateCount; i++)
            {
                this.OverwritePartyStorage(new Dictionary<string, object>(){{"key", i}}, createPartyResult.Value.partyID, result =>
                {
                    Interlocked.Increment(ref serverUpdateDone);
                    if (!result.IsError)
                    {
                        Interlocked.Increment(ref serverUpdateSuccessCount);
                    }
                });
                yield return new WaitForSeconds(1.0f);
            }
            
            while (serverUpdateDone < spamUpdateCount) yield return new WaitForSeconds(0.2f);
            while (partyDataNotifs.Count < serverUpdateSuccessCount) yield return new WaitForSeconds(0.05f);
            lobby.LeaveParty(result => { });

            bool assertNotifCount = partyDataNotifs.Count == serverUpdateSuccessCount;
            Assert.IsTrue(assertNotifCount);
            foreach (var notif in partyDataNotifs)
            {
                Assert.IsFalse(notif.IsError);
                Assert.IsTrue(notif.Value?.custom_attribute.ContainsKey("key"));
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameClient_OverwritePartyStorage_PartyFormed_SuccessGotNotification_THROTTLE()
        {
            var lobby = this.userLobbies[0];

            // cleanup
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            // notif listen racing update party data
            var partyDataNotifs = new ConcurrentQueue<Result<PartyDataUpdateNotif>>();
            lobby.PartyDataUpdateNotif += result => partyDataNotifs.Enqueue(result);

            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);
            Stopwatch stopwatch = Stopwatch.StartNew();

            yield return new WaitUntil(() => partyDataNotifs.Count > 0 || stopwatch.Elapsed.Seconds > 5);

            partyDataNotifs.TryDequeue(out var notifAfterCreateParty);

            // spam update party data
            int spamUpdateCount = 10;
            int serverUpdateSuccessCount = 0;
            int serverUpdateDone = 0;
            for (int i = 0; i < spamUpdateCount; i++)
            {
                this.GameClientOverwritePartyStorage(lobby, new Dictionary<string, object>() { { "key", i } }, createPartyResult.Value.partyID, result =>
                {
                    Interlocked.Increment(ref serverUpdateDone);
                    if (!result.IsError)
                    {
                        Interlocked.Increment(ref serverUpdateSuccessCount);
                    }
                });
                yield return new WaitForSeconds(1.0f);
            }

            while (serverUpdateDone < spamUpdateCount) yield return new WaitForSeconds(0.1f);
            while (partyDataNotifs.Count < serverUpdateSuccessCount) yield return new WaitForSeconds(0.1f);
            lobby.LeaveParty(result => { });

            Assert.That(partyDataNotifs.Count, Is.EqualTo(serverUpdateSuccessCount));
            Assert.True(partyDataNotifs.All(notif => !notif.IsError && notif.Value.custom_attribute.ContainsKey("key")));
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator AllSequenceOfPartyState_ShouldGetAllPartyStorageNotification()
        {
            // Sequence of party create, someone join, someone leave
            // Check the content of party storage & member info
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int leaderIndex = 0;
            int CURRENT_PARTY_SIZE = 2;
            
            // Notification listener (for the leader)
            var partyDataNotifResults = new ConcurrentQueue<PartyDataUpdateNotif>();
            this.userLobbies[leaderIndex].PartyDataUpdateNotif += result =>
            { 
                partyDataNotifResults.Enqueue(result.Value);
            };
            
            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            this.userLobbies[leaderIndex].CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);

            while (partyDataNotifResults.Count == 0 ) yield return new WaitForSeconds(0.2f);
            partyDataNotifResults.TryDequeue(out var partyDataNotifDequeue);
            Assert.IsTrue(partyDataNotifDequeue.members.Length == 1);
            while (!partyDataNotifResults.IsEmpty) { partyDataNotifResults.TryDequeue(out var result); }

            // Invite to party
            int inviteCount = 0;
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    this.userLobbies[leaderIndex].InviteToParty(this.users[i].Session.UserId,
                        result => { Interlocked.Increment(ref inviteCount); });
                }
            }
            while (inviteCount < CURRENT_PARTY_SIZE - 1 ) yield return new WaitForSeconds(0.2f);

            // Join party
            int joinCount = 0;
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    this.userLobbies[i].JoinParty(createPartyResult.Value.partyID, createPartyResult.Value.invitationToken,
                        result => { Interlocked.Increment(ref joinCount);});
                }
            }
            while (joinCount < CURRENT_PARTY_SIZE - 1) yield return new WaitForSeconds(0.2f);

            // Check the latest notification, the new member should be there
            while (partyDataNotifResults.Count != CURRENT_PARTY_SIZE) yield return new WaitForSeconds(0.2f);
            while (!partyDataNotifResults.IsEmpty)
            {
                partyDataNotifResults.TryDequeue(out var result);
                if (partyDataNotifResults.Count == 0) // Last index
                {
                    Assert.IsTrue(result.members.Length > 1);
                }
            }

            // Leave Party
            int leaveMemberIndex = -1;
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex)
                {
                    leaveMemberIndex = i;
                    Result leaveParty = null;
                    this.userLobbies[i].LeaveParty(result => { leaveParty = result; });
                    yield return TestHelper.WaitForValue(() => leaveParty);
                    break;
                }
            }

            while (partyDataNotifResults.Count == 0) yield return new WaitForSeconds(0.2f);
            
            partyDataNotifResults.TryDequeue(out var partyDataNotifDequeue3);
            Assert.IsTrue(partyDataNotifDequeue3.members.Length == CURRENT_PARTY_SIZE - 1);
        }

        [UnityTest, TestLog, Order(1), Timeout(35000)]
        public IEnumerator SomeoneCreateParty_GotNotification_ClientServerAssertToo_PartyStorageNotification_Success()
        {
            var lobby = this.userLobbies[0];
            
            // cleanup
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            // notif listen
            Result<PartyDataUpdateNotif> partyDataNotif = null;
            lobby.PartyDataUpdateNotif += result => partyDataNotif = result;
            
            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);
            yield return TestHelper.WaitForValue(() => partyDataNotif);
            
            // GameClient assertion
            Result<PartyDataUpdateNotif> gameClientAssertion = null;
            AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion = result);
            yield return TestHelper.WaitForValue(() => gameClientAssertion);
            Assert.IsTrue(gameClientAssertion.Value.members.Contains(this.users[0].Session.UserId));
            
            // GameServer assertion
            Result<PartyDataUpdateNotif> gameServerAssertion = null;
            AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion = result);
            yield return TestHelper.WaitForValue(() => gameServerAssertion);
            Assert.IsTrue(gameServerAssertion.Value.members.Contains(this.users[0].Session.UserId));
            
            lobby.LeaveParty(result => { });

            Assert.IsFalse(partyDataNotif.IsError);
            Assert.IsTrue(partyDataNotif.Value.members.Contains(this.users[0].Session.UserId));
            Assert.IsTrue(partyDataNotif.Value.invitees.Length == 0);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator SomeoneJoinParty_GotPartyStorageNotification_ClientServerAssertToo_Success()
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int leaderIndex = 0;
            int CURRENT_PARTY_SIZE = 2;
            var lobbies = new Lobby[CURRENT_PARTY_SIZE];
         
            // Connect to lobby
            for (int i = 0 ; i < CURRENT_PARTY_SIZE ; i++)
            {
                lobbies[i] = this.userLobbies[i];
            }

            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobbies[leaderIndex].CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // Write party data
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, createPartyResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);
            
            // Invite to party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result inviteResult = null;
                    lobbies[leaderIndex].InviteToParty(this.users[i].Session.UserId, result => { inviteResult = result; });
                    yield return TestHelper.WaitForValue(() => inviteResult);
                }
            }

            // Notification listener
            var partyDataNotifResults = new Result<PartyDataUpdateNotif>[CURRENT_PARTY_SIZE];
            var partyDataNotifDoneForEach = new bool[CURRENT_PARTY_SIZE];
            for (var i = 0; i < lobbies.Length; i++)
            {
                partyDataNotifDoneForEach[i] = false;
                int index = i;
                lobbies[i].PartyDataUpdateNotif += result =>
                { 
                    partyDataNotifResults[index] = result;
                    partyDataNotifDoneForEach[index] = true;
                };
            }

            {
                // GameClient assertion
                Result<PartyDataUpdateNotif> gameClientAssertion1 = null;
                AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameClientAssertion1);
                
                // GameServer assertion
                Result<PartyDataUpdateNotif> gameServerAssertion1 = null;
                AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameServerAssertion1);
                
                Assert.IsTrue(gameClientAssertion1.Value.invitees.Length == CURRENT_PARTY_SIZE - 1);
                Assert.IsTrue(gameServerAssertion1.Value.invitees.Length == CURRENT_PARTY_SIZE - 1);
                Assert.IsTrue(gameClientAssertion1.Value.members.Length == 1);
                Assert.IsTrue(gameServerAssertion1.Value.members.Length == 1);
            }

            // Join party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result<PartyInfo> joinPartyResult = null;
                    lobbies[i].JoinParty(createPartyResult.Value.partyID, createPartyResult.Value.invitationToken,
                        result => { joinPartyResult = result; });
                    yield return TestHelper.WaitForValue(() => joinPartyResult);
                }
            }
            
            {
                // GameClient assertion
                Result<PartyDataUpdateNotif> gameClientAssertion1 = null;
                AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameClientAssertion1);
    
                // GameServer assertion
                Result<PartyDataUpdateNotif> gameServerAssertion1 = null;
                AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameServerAssertion1);
                 
                Assert.IsTrue(gameClientAssertion1.Value.invitees.Length == 0);
                Assert.IsTrue(gameServerAssertion1.Value.invitees.Length == 0);
                Assert.IsTrue(gameServerAssertion1.Value.members.Length == CURRENT_PARTY_SIZE);
                Assert.IsTrue(gameServerAssertion1.Value.members.Length == CURRENT_PARTY_SIZE);
            }

            // Wait for notification
            while (partyDataNotifDoneForEach.Contains(false)) yield return new WaitForSeconds(0.2f);
            foreach (var updateNotif in partyDataNotifResults)
            {
                Assert.IsFalse(updateNotif.IsError);
                bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, updateNotif.Value?.custom_attribute);
                Assert.IsTrue(contentIdentical);
                Assert.IsTrue(updateNotif.Value?.members.Length == CURRENT_PARTY_SIZE);
            }

            foreach (var lobby in lobbies)
            {
                lobby.LeaveParty(result => { });
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator SomeoneLeave_GotPartyStorageNotification_ClientServerAssertToo_Success()
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int leaderIndex = 0;
            int CURRENT_PARTY_SIZE = 3;
            var lobbies = new Lobby[CURRENT_PARTY_SIZE];
         
            // Connect to lobby
            for (int i = 0 ; i < CURRENT_PARTY_SIZE ; i++)
            {
                lobbies[i] = this.userLobbies[i];
            }

            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobbies[leaderIndex].CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // Write party data
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, createPartyResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            // Invite to party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result inviteResult = null;
                    lobbies[leaderIndex].InviteToParty(this.users[i].Session.UserId, result => { inviteResult = result; });
                    yield return TestHelper.WaitForValue(() => inviteResult);
                }
            }

            // Join party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result<PartyInfo> joinPartyResult = null;
                    lobbies[i].JoinParty(createPartyResult.Value.partyID, createPartyResult.Value.invitationToken,
                        result => { joinPartyResult = result; });
                    yield return TestHelper.WaitForValue(() => joinPartyResult);
                }
            }

            // Notification listener
            var partyDataNotifResults = new Result<PartyDataUpdateNotif>[CURRENT_PARTY_SIZE];
            var partyDataNotifDoneForEach = new bool[CURRENT_PARTY_SIZE];
            for (var i = 0; i < lobbies.Length; i++)
            {
                partyDataNotifDoneForEach[i] = false;
                int index = i;
                lobbies[i].PartyDataUpdateNotif += result =>
                { 
                    partyDataNotifResults[index] = result;
                    partyDataNotifDoneForEach[index] = true;
                };
            }
            
            // Someone, will leave party
            int leavePartyMemberIndex = -1;
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex)
                {
                    leavePartyMemberIndex = i;
                    Result leavePartyResult = null;
                    lobbies[i].LeaveParty(result => leavePartyResult = result);
                    yield return TestHelper.WaitForValue(() => leavePartyResult);
                    break;
                }
            }
            
            {
                // GameClient assertion
                Result<PartyDataUpdateNotif> gameClientAssertion1 = null;
                AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameClientAssertion1);
    
                // GameServer assertion
                Result<PartyDataUpdateNotif> gameServerAssertion1 = null;
                AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameServerAssertion1);
                
                Assert.IsTrue(gameClientAssertion1.Value.members.Length == CURRENT_PARTY_SIZE - 1);
                Assert.IsTrue(gameServerAssertion1.Value.members.Length == CURRENT_PARTY_SIZE - 1);
                Assert.IsFalse(gameClientAssertion1.Value.members.Contains(this.users[leavePartyMemberIndex].Session.UserId));
                Assert.IsFalse(gameServerAssertion1.Value.members.Contains(this.users[leavePartyMemberIndex].Session.UserId));
            }
            
            // Wait for notification
            while (partyDataNotifDoneForEach.Count(x => x == false) > 1) yield return new WaitForSeconds(0.2f);
            for (var i = 0; i < partyDataNotifResults.Length; i++)
            {
                if (i == leavePartyMemberIndex) continue;
                var updateNotif = partyDataNotifResults[i];
                Assert.IsFalse(updateNotif.IsError);
                bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, updateNotif.Value?.custom_attribute);
                Assert.IsTrue(contentIdentical);
                Assert.IsTrue(updateNotif.Value.members.Length == CURRENT_PARTY_SIZE - 1);
            }

            foreach (var lobby in lobbies)
            {
                lobby.LeaveParty(result => { });
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(36000)]
        public IEnumerator LeaderKickMember_GotPartyStorageNotification_ClientServerAssertToo_Success()
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int leaderIndex = 0;
            int CURRENT_PARTY_SIZE = 3;
            var lobbies = new Lobby[CURRENT_PARTY_SIZE];
         
            // Connect to lobby
            for (int i = 0 ; i < CURRENT_PARTY_SIZE ; i++)
            {
                lobbies[i] = this.userLobbies[i];
            }

            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobbies[leaderIndex].CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // Write party data
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, createPartyResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);
            
            // Invite to party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result inviteResult = null;
                    lobbies[leaderIndex].InviteToParty(this.users[i].Session.UserId, result => { inviteResult = result; });
                    yield return TestHelper.WaitForValue(() => inviteResult);
                }
            }

            // Join party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result<PartyInfo> joinPartyResult = null;
                    lobbies[i].JoinParty(createPartyResult.Value.partyID, createPartyResult.Value.invitationToken,
                        result => { joinPartyResult = result; });
                    yield return TestHelper.WaitForValue(() => joinPartyResult);
                }
            }

            // Notification listener
            var partyDataNotifResults = new Result<PartyDataUpdateNotif>[CURRENT_PARTY_SIZE];
            var partyDataNotifDoneForEach = new bool[CURRENT_PARTY_SIZE];
            for (var i = 0; i < lobbies.Length; i++)
            {
                int index = i;
                partyDataNotifDoneForEach[i] = false;
                lobbies[i].PartyDataUpdateNotif += result =>
                { 
                    partyDataNotifResults[index] = result;
                    partyDataNotifDoneForEach[index] = true;
                };
            }
            
            // Someone, will be kicked party
            int kickedMemberIndex = -1;
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex)
                {
                    kickedMemberIndex = i;
                    Result kickResult = null;
                    lobbies[leaderIndex].KickPartyMember(this.users[i].Session.UserId, result => kickResult = result);
                    yield return TestHelper.WaitForValue(() => kickResult);
                    break;
                }
            }
                        
            {
                // GameClient assertion
                Result<PartyDataUpdateNotif> gameClientAssertion1 = null;
                AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameClientAssertion1);
    
                // GameServer assertion
                Result<PartyDataUpdateNotif> gameServerAssertion1 = null;
                AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion1 = result);
                yield return TestHelper.WaitForValue(() => gameServerAssertion1);
                
                Assert.IsTrue(gameClientAssertion1.Value.members.Length == CURRENT_PARTY_SIZE - 1);
                Assert.IsTrue(gameServerAssertion1.Value.members.Length == CURRENT_PARTY_SIZE - 1);
                Assert.IsFalse(gameClientAssertion1.Value.members.Contains(this.users[kickedMemberIndex].Session.UserId));
                Assert.IsFalse(gameServerAssertion1.Value.members.Contains(this.users[kickedMemberIndex].Session.UserId));
            }
            
            // Wait for notification
            // The kicked player won't get notification. So the NotifDoneForEach will be expected to false
            while (partyDataNotifDoneForEach.Count(x => x == false) > 1) yield return new WaitForSeconds(0.05f);
            for (var i = 0; i < partyDataNotifResults.Length; i++)
            {
                if (i == kickedMemberIndex) continue;
                var updateNotif = partyDataNotifResults[i];
                Assert.IsFalse(updateNotif.IsError);
                bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, updateNotif.Value?.custom_attribute);
                Assert.IsTrue(contentIdentical);
                Assert.IsTrue(updateNotif.Value.members.Length == CURRENT_PARTY_SIZE - 1);
            }

            foreach (var lobby in lobbies) { lobby.LeaveParty(result => { }); }
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_QueryPartyByUserId_allOk()
        {
            bool isCleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => isCleanupDone = result));
            Debug.Log("Wait for cleanup");
            while (!isCleanupDone)
                yield return new WaitForSeconds(0.2f);
            Debug.Log("Cleanup finsih");

            // Arrange
            int userCount = 2;

            var serverLobby = AccelByteServerPlugin.GetLobby();
            List<Lobby> activeLobbies = new List<Lobby>();
            List<string> activeUsersId = new List<string>();
            
            for(int i = 0; i < userCount; i++)
            {
                int userNum = i;
                activeLobbies.Add(this.userLobbies[userNum]);
                activeUsersId.Add(this.users[userNum].Session.UserId);
            }

            // User 0 create party
            Result<PartyInfo> createPartyResult = null;
            activeLobbies[0].CreateParty(result => createPartyResult = result);
            yield return TestHelper.WaitForValue(() => createPartyResult, "Waiting create party");

            Result<PartyDataUpdateNotif> queryOnPartyCreated = null;
            serverLobby.GetPartyDataByUserId(activeUsersId[0], result => queryOnPartyCreated = result);
            yield return TestHelper.WaitForValue(() => queryOnPartyCreated);


            // User 1 join party
            Result<PartyInfo> joinPartyResult = null;
            ResultCallback<PartyInvitation> OnPartyInvitedJoinParty = inviteResult =>
            {
                activeLobbies[1].JoinParty(inviteResult.Value.partyID, inviteResult.Value.invitationToken, result => joinPartyResult = result);
            };
            activeLobbies[1].InvitedToParty += OnPartyInvitedJoinParty;

            activeLobbies[0].InviteToParty(activeUsersId[1], result =>
            {
                Debug.Log($"user {activeUsersId[0]} invite {activeUsersId[1]} to party {(result.IsError ? "failed" : "success")}");
            });
            
            yield return TestHelper.WaitForValue(() => joinPartyResult, "Wait join party");

            activeLobbies[1].InvitedToParty -= OnPartyInvitedJoinParty;

            Result<PartyDataUpdateNotif> queryOnJoinPartyUser0 = null;
            Result<PartyDataUpdateNotif> queryOnJoinPartyUser1 = null;
            serverLobby.GetPartyDataByUserId(activeUsersId[0], result => queryOnJoinPartyUser0 = result);
            serverLobby.GetPartyDataByUserId(activeUsersId[1], result => queryOnJoinPartyUser1 = result);

            yield return TestHelper.WaitForValue(() => queryOnJoinPartyUser0);
            yield return TestHelper.WaitForValue(() => queryOnJoinPartyUser1);

            // User 1 leave party
            Result leavePartyResult = null;
            activeLobbies[1].LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            Result<PartyDataUpdateNotif> queryOnLeavePartyUser0 = null;
            Result<PartyDataUpdateNotif> queryOnLeavePartyUser1 = null;
            serverLobby.GetPartyDataByUserId(activeUsersId[0], result => queryOnLeavePartyUser0 = result);
            serverLobby.GetPartyDataByUserId(activeUsersId[1], result => queryOnLeavePartyUser1 = result);

            yield return TestHelper.WaitForValue(() => queryOnLeavePartyUser0);
            yield return TestHelper.WaitForValue(() => queryOnLeavePartyUser1);

            // User 0 disband party
            Result disbandPartyResult = null;
            activeLobbies[0].LeaveParty(result => disbandPartyResult = result);
            yield return TestHelper.WaitForValue(() => disbandPartyResult);

            Result<PartyDataUpdateNotif> queryOnPartyDisband = null;
            serverLobby.GetPartyDataByUserId(activeUsersId[0], result => queryOnPartyDisband = result);

            yield return TestHelper.WaitForValue(() => queryOnPartyDisband);

            // Asserts in party created.
            Assert.IsFalse(queryOnPartyCreated.IsError);
            TestHelper.LogResult(queryOnPartyCreated);
            Assert.AreEqual(createPartyResult.Value.partyID, queryOnPartyCreated.Value.partyId);

            // Asserts in a user join party.
            Assert.IsFalse(queryOnJoinPartyUser0.IsError);
            Assert.IsFalse(queryOnJoinPartyUser1.IsError);
            Assert.AreEqual(joinPartyResult.Value.partyID, queryOnJoinPartyUser0.Value.partyId);
            Assert.AreEqual(joinPartyResult.Value.partyID, queryOnJoinPartyUser1.Value.partyId);
            Assert.IsTrue(queryOnJoinPartyUser0.Value.members
                .Where(m => m.Equals(activeUsersId[1]))
                .Any(), "new member party ID not found after member join party.");


            // Asserts in a user leave party.
            Assert.IsFalse(queryOnLeavePartyUser0.IsError);
            Assert.IsTrue(queryOnLeavePartyUser1.IsError);
            Assert.AreEqual(createPartyResult.Value.partyID, queryOnLeavePartyUser0.Value.partyId);
            Assert.IsFalse(queryOnLeavePartyUser0.Value.members
                .Where(m => m.Equals(activeUsersId[1]))
                .Any(), "Left Member still found in party query even after leaving.");

            // Asserts when a party is disbanded (all members left)
            Assert.IsTrue(queryOnPartyDisband.IsError);
        }

        [UnityTest, TestLog, Order(1), Timeout(35000)]
        public IEnumerator PartyStorageNotification_CheckInvitees()
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int leaderIndex = 0;
            int CURRENT_PARTY_SIZE = 3;
            var lobbies = new Lobby[CURRENT_PARTY_SIZE];
         
            // Connect to lobby
            for (int i = 0 ; i < CURRENT_PARTY_SIZE ; i++)
            {
                lobbies[i] = this.userLobbies[i];
            }

            // Notification listener
            Result<PartyDataUpdateNotif> partyDataNotifResult = null; 
            var partyDataNotifDoneForEach = new bool[CURRENT_PARTY_SIZE];
            lobbies[leaderIndex].PartyDataUpdateNotif += result => partyDataNotifResult = result;

            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobbies[leaderIndex].CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);
            
            // ASSERT on party created
            while (partyDataNotifResult == null) yield return new WaitForSeconds(0.2f);
            Assert.IsTrue(partyDataNotifResult.Value?.invitees.Length == 0);
            partyDataNotifResult = null;
            {
                // GameClient assertion
                Result<PartyDataUpdateNotif> gameClientAssertion = null;
                AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion = result);
                // GameServer assertion
                Result<PartyDataUpdateNotif> gameServerAssertion = null;
                AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion = result);
    
                yield return TestHelper.WaitForValue(() => gameClientAssertion);
                yield return TestHelper.WaitForValue(() => gameServerAssertion);

                Assert.IsTrue(gameClientAssertion.Value.invitees.Length == 0);
                Assert.IsTrue(gameServerAssertion.Value.invitees.Length == 0);
            }

            // Invite to party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result inviteResult = null;
                    lobbies[leaderIndex].InviteToParty(this.users[i].Session.UserId, result => { inviteResult = result; });
                    yield return TestHelper.WaitForValue(() => inviteResult);
                }
            }
            
            // ASSERT on Invitation Sent
            while (partyDataNotifResult == null) yield return new WaitForSeconds(0.2f);
            while (partyDataNotifResult.Value.invitees.Length != CURRENT_PARTY_SIZE - 1) yield return new WaitForSeconds(0.05f);
            Assert.IsTrue(partyDataNotifResult.Value?.invitees.Length == CURRENT_PARTY_SIZE - 1);
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex)
                {
                    Assert.IsTrue(partyDataNotifResult.Value.invitees.Contains(this.users[i].Session.UserId));
                }
            }
            partyDataNotifResult = null;
            {
                // GameClient assertion
                Result<PartyDataUpdateNotif> gameClientAssertion = null;
                AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion = result);
                // GameServer assertion
                Result<PartyDataUpdateNotif> gameServerAssertion = null;
                AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion = result);
    
                yield return TestHelper.WaitForValue(() => gameClientAssertion);
                yield return TestHelper.WaitForValue(() => gameServerAssertion);
    
                Assert.IsTrue(gameClientAssertion.Value.invitees.Length == CURRENT_PARTY_SIZE - 1);
                Assert.IsTrue(gameServerAssertion.Value.invitees.Length == CURRENT_PARTY_SIZE - 1);
            }

            // Join party
            for (int i = 0; i < CURRENT_PARTY_SIZE; i++)
            {
                if (i != leaderIndex && createPartyResult.Value != null)
                {
                    Result<PartyInfo> joinPartyResult = null;
                    lobbies[i].JoinParty(createPartyResult.Value.partyID, createPartyResult.Value.invitationToken,
                        result => { joinPartyResult = result; });
                    yield return TestHelper.WaitForValue(() => joinPartyResult);
                }
            }
            while (partyDataNotifResult == null) yield return new WaitForSeconds(0.2f);
            while (partyDataNotifResult.Value.members.Length != CURRENT_PARTY_SIZE) yield return new WaitForSeconds(0.2f);
            
            Assert.IsTrue(partyDataNotifResult.Value?.invitees.Length == 0);
            Assert.IsTrue(partyDataNotifResult.Value?.members.Length == CURRENT_PARTY_SIZE);
            
            partyDataNotifResult = null;
            {
                // GameClient assertion
                Result<PartyDataUpdateNotif> gameClientAssertion = null;
                AccelBytePlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameClientAssertion = result);
                // GameServer assertion
                Result<PartyDataUpdateNotif> gameServerAssertion = null;
                AccelByteServerPlugin.GetLobby().GetPartyStorage(createPartyResult.Value.partyID, result => gameServerAssertion = result);
    
                yield return TestHelper.WaitForValue(() => gameClientAssertion);
                yield return TestHelper.WaitForValue(() => gameServerAssertion);
    
                Assert.IsTrue(gameClientAssertion.Value.invitees.Length == 0);
                Assert.IsTrue(gameServerAssertion.Value.invitees.Length == 0);
                Assert.IsTrue(gameClientAssertion.Value.members.Length == CURRENT_PARTY_SIZE);
                Assert.IsTrue(gameServerAssertion.Value.members.Length == CURRENT_PARTY_SIZE);
            }
            
            foreach (var lobby in lobbies) { lobby.LeaveParty(result => { }); }
        }
        
        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GameServer_OverwritePartyStorage_AssertTheResponse_Same()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            foreach (var lobby in this.userLobbies){ lobby.LeaveParty(result => { });}
            
            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
            bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, OverwritePartyStorageResult.Value?.custom_attribute);
            Assert.IsTrue(contentIdentical);
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator Client_WritePartyStorage_OK()
        {
            bool cleanupDone = false;

            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);
            var lobby = this.userLobbies[0];

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            Result<PartyDataUpdateNotif> updateNotif = null;
            lobby.PartyDataUpdateNotif += result =>
            {
                updateNotif = result;
            };

            Result<PartyDataUpdateNotif> writePartyStorageResult = null;
            lobby.WritePartyStorage(createPartyResult.Value.partyID, result => writePartyStorageResult = result, x => this.PARTY_DATA);
            yield return TestHelper.WaitForValue(() => writePartyStorageResult);

            yield return TestHelper.WaitForValue(() => updateNotif);

            Assert.IsFalse(writePartyStorageResult.IsError);
            bool isEqual = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, writePartyStorageResult.Value.custom_attribute);
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator Client_WritePartyStorage_2Concurent_1Success_1Fail()
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int numUser = 2;
            Lobby[] lobbies = new Lobby[numUser]; 

            for(int i = 0; i < numUser; i++)
            {
                lobbies[i] = this.userLobbies[i];
            }

            Result<PartyInfo> createPartyResult = null;
            lobbies[0].CreateParty(result => createPartyResult = result);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            Result<PartyDataUpdateNotif> updateNotif = null;
            lobbies[0].PartyDataUpdateNotif += result =>
            {
                updateNotif = result;
            };

            Result<PartyInfo> joinPartyResult = null;
            ResultCallback<PartyInvitation> onPartyInvited = null;

            onPartyInvited = (invitation) =>
            {
                lobbies[1].InvitedToParty -= onPartyInvited;
                lobbies[1].JoinParty(invitation.Value.partyID, invitation.Value.invitationToken, result => joinPartyResult = result);
            };
            lobbies[1].InvitedToParty += onPartyInvited;

            Result inviteResult = null;
            lobbies[0].InviteToParty(this.users[1].Session.UserId, result => inviteResult = result);
            yield return TestHelper.WaitForValue(() => inviteResult);

            yield return TestHelper.WaitForValue(() => joinPartyResult);

            int successCount = 0;
            int failCount = 0;
            Result<PartyDataUpdateNotif>[] writeResults = new Result<PartyDataUpdateNotif>[numUser];
            for(int i = 0; i < numUser; i++)
            {
                int localIndex = i;
                lobbies[i].WritePartyStorage(createPartyResult.Value.partyID, result => writeResults[localIndex] = result, x => this.PARTY_DATA);
            }
            
            for(int i = 0; i < numUser; i++)
            {
                yield return TestHelper.WaitForValue(() => writeResults[i]);
                if (writeResults[i].IsError)
                    failCount++;
                else
                    successCount++;
            }

            Assert.AreEqual(1, failCount);
            Assert.AreEqual(1, successCount);
        }

        [UnityTest, TestLog, Order(1), Timeout(20000)]
        public IEnumerator GameClient_OverwritePartyStorage_AssertTheResponse_Same()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);

            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.GameClientOverwritePartyStorage(this.userLobbies[0], this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            foreach (var lobby in this.userLobbies) { lobby.LeaveParty(result => { }); }

            // ASSERT
            Assert.IsFalse(getPartyInfoResult.IsError);
            Assert.IsFalse(OverwritePartyStorageResult.IsError);
            bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, OverwritePartyStorageResult.Value?.custom_attribute);
            Assert.IsTrue(contentIdentical);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000), Ignore("hanging parties keep existing in backend, not isolated to only this testcase")]
        public IEnumerator GetActiveParties_NoPartyActive_SuccessZero()
        {
            Result<ActivePartiesData> getActivePartiesResult = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(999, 0, result => getActivePartiesResult = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult);

            Assert.IsFalse(getActivePartiesResult.IsError);
            Assert.IsTrue(getActivePartiesResult.Value.data.Length == 0);
        }

        [UnityTest, TestLog, Order(1), Timeout(20000)]
        public IEnumerator GetActiveParties_ActivePartiesCreated_Success()
        {
            bool isDone = false;
            this.coroutineRunner.Run(this.FormParty(0, () => isDone = true));
            while (isDone == false) yield return new WaitForSeconds(0.1f);

            Result<PartyInfo> getPartyInfoResult = null;
            this.userLobbies[0].GetPartyInfo(result => getPartyInfoResult = result);
            yield return TestHelper.WaitForValue(() => getPartyInfoResult);
            
            Result<PartyDataUpdateNotif> OverwritePartyStorageResult = null;
            this.OverwritePartyStorage(this.PARTY_DATA, getPartyInfoResult.Value.partyID, result => OverwritePartyStorageResult = result);
            yield return TestHelper.WaitForValue(() => OverwritePartyStorageResult);

            Result<ActivePartiesData> getActivePartiesResultPostTest = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(99999, 0, result => getActivePartiesResultPostTest = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResultPostTest);
            
            foreach (var lobby in this.userLobbies) { lobby.LeaveParty(result => { }); }

            Assert.IsFalse(getActivePartiesResultPostTest.IsError);
            var partyInActiveParties = getActivePartiesResultPostTest.Value.data.Where(p => p.partyId == getPartyInfoResult.Value.partyID);
            Assert.IsTrue(partyInActiveParties.Any(), "Check party exist in active parties"); // if current party exist in Active parties
            bool contentIdentical = AwesomeFormatTest.ComparePartyData(this.PARTY_DATA, partyInActiveParties.First().custom_attribute);
            Assert.IsTrue(contentIdentical, "check party data same as set");
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GetActiveParties_CheckLimitAndOffsetParam_Success()
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int CURRENT_PARTY_SIZE = 4;
            var lobbies = new Lobby[CURRENT_PARTY_SIZE];

            // All connect to lobby and create party
            for (int i = 0 ; i < CURRENT_PARTY_SIZE ; i++)
            {
                lobbies[i] = this.userLobbies[i];
                Result leavePartyResult = null;
                lobbies[i].LeaveParty(result => { leavePartyResult = result; });
                yield return TestHelper.WaitForValue(() => leavePartyResult);
                Result<PartyInfo> createPartyResult = null;
                lobbies[i].CreateParty(result => { createPartyResult = result; });
                yield return TestHelper.WaitForValue(() => createPartyResult);
            }

            // page 1
            Result<ActivePartiesData> getActivePartiesResult1 = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(2, 0, result => getActivePartiesResult1 = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult1);
            // page 2
            Result<ActivePartiesData> getActivePartiesResult2 = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(2, 2, result => getActivePartiesResult2 = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult2);
            
            foreach (var lobby in lobbies) { lobby.LeaveParty(result => { }); }
            
            Assert.IsFalse(getActivePartiesResult1.IsError);
            Assert.IsTrue(getActivePartiesResult1.Value.data.Length == 2);
            Assert.IsTrue(getActivePartiesResult2.Value.data.Length == 2);
            var partyIDs = new HashSet<string>();
            foreach (var entry in getActivePartiesResult1.Value.data)
            {
                // if the Add() function return false, then there's a repetitive party ID. Query is not okay
                Assert.That(partyIDs.Add(entry.partyId));
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator GetActiveParties_UsingPagination_Success()
        {
            bool cleanupDone = false;
            this.coroutineRunner.Run(this.CleanupBeforeTestCase(result => { cleanupDone = result; }));
            while (!cleanupDone) yield return new WaitForSeconds(0.2f);

            int CURRENT_PARTY_SIZE = 4;
            var lobbies = new Lobby[CURRENT_PARTY_SIZE];

            // All connect to lobby and create party
            for (int i = 0 ; i < CURRENT_PARTY_SIZE ; i++)
            {
                lobbies[i] = this.userLobbies[i];
                Result leavePartyResult = null;
                lobbies[i].LeaveParty(result => { leavePartyResult = result; });
                yield return TestHelper.WaitForValue(() => leavePartyResult);
                Result<PartyInfo> createPartyResult = null;
                lobbies[i].CreateParty(result => { createPartyResult = result; });
                yield return TestHelper.WaitForValue(() => createPartyResult);
            }

            // page 1
            Result<ActivePartiesData> getActivePartiesResult1 = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(1, 0, result => getActivePartiesResult1 = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult1);
            // next page (2)
            Result<ActivePartiesData> getActivePartiesResult2 = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(getActivePartiesResult1.Value.paging, PaginationType.NEXT, result => getActivePartiesResult2 = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult2);
            // first page (1)
            Result<ActivePartiesData> getActivePartiesResult3 = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(getActivePartiesResult1.Value.paging, PaginationType.FIRST, result => getActivePartiesResult3 = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult3);
            // previous page from page 2 (1)
            Result<ActivePartiesData> getActivePartiesResult4 = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(getActivePartiesResult2.Value.paging, PaginationType.PREVIOUS, result => getActivePartiesResult4 = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult4);
            // next page from page 2 (3)
            Result<ActivePartiesData> getActivePartiesResult5 = null;
            AccelByteServerPlugin.GetLobby().GetActiveParties(getActivePartiesResult2.Value.paging, PaginationType.NEXT, result => getActivePartiesResult5 = result);
            yield return TestHelper.WaitForValue(() => getActivePartiesResult5);
            
            foreach (var lobby in lobbies) { lobby.LeaveParty(result => { }); }

            Assert.IsFalse(getActivePartiesResult1.IsError);
            Assert.IsTrue(getActivePartiesResult1.Value.data.Length == 1);
            Assert.IsTrue(getActivePartiesResult2.Value.data.Length == 1);
            Assert.IsTrue(getActivePartiesResult3.Value.data.Length == 1);
            Assert.IsTrue(getActivePartiesResult4.Value.data.Length == 1);
            Assert.IsTrue(getActivePartiesResult5.Value.data.Length == 1);
            string firstPagePartyID = getActivePartiesResult1.Value.data[0].partyId;
            Assert.IsTrue(getActivePartiesResult2.Value.data[0].partyId != firstPagePartyID);
            Assert.IsTrue(getActivePartiesResult3.Value.data[0].partyId == firstPagePartyID);
            Assert.IsTrue(getActivePartiesResult4.Value.data[0].partyId == firstPagePartyID);
            
            var partyIDs = new HashSet<string>();
            foreach (var entry in getActivePartiesResult1.Value.data)
            {
                // if the Add() function return false, then there's a repetitive party ID. Query is not okay
                Assert.That(partyIDs.Add(entry.partyId));
            }
        }

        [UnityTest, TestLog, Order(1), Timeout(60000)]
        public IEnumerator WritePartyStorage_RacingCondition_6ServersAtOnce_Success()
        {
            var lobby = this.userLobbies[0];
            
            // cleanup
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);
            yield return TestHelper.WaitForValue(() => leavePartyResult);

            // notif listen racing update party data
            var partyDataNotifs = new ConcurrentQueue<Result<PartyDataUpdateNotif>>();
            lobby.PartyDataUpdateNotif += result => partyDataNotifs.Enqueue(result);

            // Leader create party
            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => { createPartyResult = result; });
            yield return TestHelper.WaitForValue(() => createPartyResult);
            Stopwatch stopwatch = Stopwatch.StartNew();

            yield return new WaitUntil(() => partyDataNotifs.Count > 0 || stopwatch.Elapsed.Seconds > 5);

            partyDataNotifs.TryDequeue(out var notifAfterCreateParty);
            
            // update party data
            int concurrentServerWriteCount = 6;
            int serverUpdateSuccessCount = 0;
            int serverUpdateDone = 0;
            for (int i = 0; i < concurrentServerWriteCount; i++)
            {
                int dictIndex = i;
                AccelByteServerPlugin.GetLobby().WritePartyStorage(createPartyResult.Value.partyID, result =>
                    {
                        if (!result.IsError)
                        {
                            Interlocked.Increment(ref serverUpdateDone);
                            Interlocked.Increment(ref serverUpdateSuccessCount);
                        }
                    }, 
                    oldData =>
                    {
                        oldData.Add($"key_{dictIndex}", dictIndex);
                        return oldData;
                    }, 7);
                yield return new WaitForSeconds(0.1f);
            }
            
            while (serverUpdateDone < concurrentServerWriteCount) yield return new WaitForSeconds(0.2f);
            while (partyDataNotifs.Count < serverUpdateSuccessCount) yield return new WaitForSeconds(0.05f);
            lobby.LeaveParty(result => { });

            bool latestNotifFound = false;
            bool latestNotifContentIsUnique = true;
            foreach (var notif in partyDataNotifs)
            {
                Assert.IsFalse(notif.IsError);
                if (notif.Value?.custom_attribute.Count == concurrentServerWriteCount)
                {
                    latestNotifFound = true;
                    
                    var keyCollection = new HashSet<string>();
                    var valueCollection = new HashSet<int>();
                    foreach (var dict in notif.Value.custom_attribute)
                    {
                        if (!valueCollection.Add(Convert.ToInt32(dict.Value)) || !keyCollection.Add(dict.Key))
                        {
                            latestNotifContentIsUnique = false;
                        }
                    }
                }
            }
            Assert.AreEqual(partyDataNotifs.Count, serverUpdateSuccessCount);
            Assert.IsTrue(latestNotifFound);
            Assert.IsTrue(latestNotifContentIsUnique);
        }
    }
}