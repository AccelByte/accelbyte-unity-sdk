using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class ProfanityFilterTest
    {
        private TestHelper helper;
        private User[] users;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private UserData[] usersData;
        private List<Lobby> activeLobbies = new List<Lobby>();

        private string accessToken;

        private string[] filterListMandatory = { "mandatory" };
        private string[] filterListNonMandatory = { "optional" };

        private string listNameMandatory = "man";
        private string listNameNonMandatory = "non";

        private string testString = "test this is mandatory, this is optional";

        private string resultLevelNone = "test this is mandatory, this is optional";
        private string resultLevelMandatory = "test this is *********, this is optional";
        private string resultLevelAll = "test this is *********, this is ********";

        Lobby CreateLobby(ISession session)
        {
            Lobby lobby = LobbyTestUtil.CreateLobby(session, this.httpClient, this.coroutineRunner);
            this.activeLobbies.Add(lobby);
            return lobby;
        }

        [UnityTest, TestLog, Order(99999), Timeout(30000)]
        public IEnumerator ProfanityTearDown()
        {
            foreach (var lobby in this.activeLobbies)
            {
                if (lobby.IsConnected)
                {
                    Debug.LogWarning("[LOBBY] Dangling websocket connection, previous test are not closing WS connection, please disconnect it at the of of the tests, disconnecting...");
                    lobby.Disconnect();
                }
            }
            this.activeLobbies.Clear();

            if(this.helper == null)
            {
                this.helper = new TestHelper();
            }

            if(string.IsNullOrEmpty(this.accessToken))
            {
                Result<TokenData> getAccessToken = null;
                this.helper.GetAccessToken(r => getAccessToken = r);
                yield return TestHelper.WaitForValue(() => getAccessToken);
                this.accessToken = getAccessToken.Value.access_token;
            }

            Result deleteListMandatoryResult = null;
            this.helper.DeleteProfanityFilterList(this.accessToken, this.listNameMandatory, r => deleteListMandatoryResult = r);
            yield return TestHelper.WaitForValue(() => deleteListMandatoryResult);
            TestHelper.LogResult(deleteListMandatoryResult);

            Result deleteListNonMandatorResult = null;
            this.helper.DeleteProfanityFilterList(this.accessToken, this.listNameNonMandatory, r => deleteListNonMandatorResult = r);
            yield return TestHelper.WaitForValue(() => deleteListNonMandatorResult);
            TestHelper.LogResult(deleteListNonMandatorResult);

            for (int i = 0; i < this.usersData.Length; i++)
            {
                Result deleteUserResult = null;
                this.helper.DeleteUser(this.usersData[i].userId, result => { deleteUserResult = result; });
                yield return TestHelper.WaitForValue(() => deleteUserResult);
                TestHelper.Assert.IsResultOk(deleteUserResult);
            }
        }

        [UnityTest, TestLog, Order(0), Timeout(30000)]
        public IEnumerator ProfanityTestSetup()
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

            var newUsers = new User[2];
            this.usersData = new UserData[2];
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

            // Setup Filter
            Result<TokenData> getAccessToken = null;
            this.helper.GetAccessToken(r => getAccessToken = r);
            yield return TestHelper.WaitForValue(() => getAccessToken);
            this.accessToken = getAccessToken.Value.access_token;

            // check current profanity filter namespace rule
            Result<TestHelper.AdminSetProfanityRuleForNamespaceRequest> filterRule = null;
            this.helper.GetProfanityFilterRule(this.accessToken, r => filterRule = r);
            yield return TestHelper.WaitForValue(() => filterRule);
            TestHelper.LogResult(filterRule, "current filter rule");

            // Set profanity filter namespace rule to all
            Result setFilterRule = null;
            this.helper.SetProfanityFilterRule(this.accessToken, TestHelper.ProfanityNamespaceRule.all, r => setFilterRule = r);
            yield return TestHelper.WaitForValue(() => setFilterRule);
            TestHelper.Assert.IsResultOk(setFilterRule);
            TestHelper.LogResult(setFilterRule, "Set profanity filter rule");

            // Create filter mandatory
            Result createMandatoryList = null;
            this.helper.CreateProfanityFilterList(this.accessToken, true, true, this.listNameMandatory, r => createMandatoryList = r);
            yield return TestHelper.WaitForValue(() => createMandatoryList);
            TestHelper.Assert.IsResultOk(createMandatoryList);
            TestHelper.LogResult(createMandatoryList, "Create mandatory list");

            Result addToMandatoryList = null;
            this.helper.AddFilterIntoListBulk(this.accessToken, this.listNameMandatory, this.filterListMandatory, r => addToMandatoryList = r);
            yield return TestHelper.WaitForValue(() => addToMandatoryList);
            TestHelper.Assert.IsResultOk(addToMandatoryList);
            TestHelper.LogResult(addToMandatoryList, "Add to mandatory list");

            // Create filter non mandatory
            Result createNonMandatoryList = null;
            this.helper.CreateProfanityFilterList(this.accessToken, true, false, this.listNameNonMandatory, r => createNonMandatoryList = r);
            yield return TestHelper.WaitForValue(() => createNonMandatoryList);
            TestHelper.Assert.IsResultOk(createNonMandatoryList);
            TestHelper.LogResult(createNonMandatoryList, "Create non mandatory list");

            Result addToNonMandatoryList = null;
            this.helper.AddFilterIntoListBulk(this.accessToken, this.listNameNonMandatory, this.filterListNonMandatory, r => addToNonMandatoryList = r);
            yield return TestHelper.WaitForValue(() => addToNonMandatoryList);
            TestHelper.Assert.IsResultOk(addToNonMandatoryList);
            TestHelper.LogResult(addToNonMandatoryList, "Add to non mandatory list");
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator ProfanityFilter_Mandatory()
        {
            // Arrange
            var lobbyA = this.CreateLobby(this.users[0].Session);
            var lobbyB = this.CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId;
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Set profanity filter level for user B
            Result setProfanityFilterLevelA = null;
            lobbyA.SetProfanityFilterLevel(ProfanityFilterLevel.mandatory, r => setProfanityFilterLevelA = r);
            yield return TestHelper.WaitForValue(() => setProfanityFilterLevelA);
            TestHelper.LogResult(setProfanityFilterLevelA, "A Set Profanity Filter");

            // Set profanity filter level for user B
            Result setProfanityFilterLevel = null;
            lobbyB.SetProfanityFilterLevel(ProfanityFilterLevel.mandatory, r => setProfanityFilterLevel = r);
            yield return TestHelper.WaitForValue(() => setProfanityFilterLevel);
            TestHelper.LogResult(setProfanityFilterLevel, "B Set Profanity Filter");

            // User A join party with user B
            // Set listener B party invite and accept invite
            Result<PartyInfo> bJoinedParty = null;
            ResultCallback<PartyInvitation> bInvitedCallback = invite =>
            {
                lobbyB.JoinParty(invite.Value.partyID, invite.Value.invitationToken, r => bJoinedParty = r);
            };
            lobbyB.InvitedToParty += bInvitedCallback;

            // A crate party
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // A invite B to party
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            TestHelper.LogResult(invitePartyResult, "A send party invite");
            
            // Wait B join party
            yield return TestHelper.WaitForValue(() => bJoinedParty);
            TestHelper.LogResult(bJoinedParty, "B join party");

            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // A & B join chat channel
            // Result<ChatChannelSlug> aJoinChannel = null;
            // lobbyA.JoinDefaultChatChannel(r => aJoinChannel = r);
            // yield return TestHelper.WaitForValue(() => aJoinChannel);
            // Result<ChatChannelSlug> bJoinChannel = null;
            // lobbyB.JoinDefaultChatChannel(r => bJoinChannel = r);
            // yield return TestHelper.WaitForValue(() => bJoinChannel);

            // A Friended B
            // Set B listen to friend request & accept
            Result bAcceptFriend = null;
            ResultCallback<Friend> AcceptIncomingFriendRequest = request =>
            {
                lobbyB.AcceptFriend(request.Value.friendId, r => bAcceptFriend = r);
            };
            lobbyB.OnIncomingFriendRequest += AcceptIncomingFriendRequest;

            // A send friend request to B
            Result aRequestFriend = null;
            lobbyA.RequestFriend(idB, r => aRequestFriend = r);
            yield return TestHelper.WaitForValue(() => aRequestFriend);

            // Wait B accept friend request
            yield return TestHelper.WaitForValue(() => bAcceptFriend);

            string personalChat = null;
            ResultCallback<ChatMessage> personalChatCallback = msg =>
            {
                personalChat = msg.Value.payload;
            };
            lobbyB.PersonalChatReceived += personalChatCallback;

            string partyChat = null;
            ResultCallback<ChatMessage> partyChatCallback = msg =>
            {
                partyChat = msg.Value.payload;
            };
            lobbyB.PartyChatReceived += partyChatCallback;

            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // string channelChat = null;
            // ResultCallback<ChannelChatMessage> channelChatCallback = msg =>
            // {
            //     channelChat = msg.Value.payload;
            // };
            // lobbyB.ChannelChatReceived += channelChatCallback;

            string bGetActivityA = null;
            ResultCallback<FriendsStatusNotif> friendStatusChangeCallback = notif =>
            {
                bGetActivityA = notif.Value.activity;
            };
            lobbyB.FriendsStatusChanged += friendStatusChangeCallback;

            // Act
            Result sendPersonal = null;
            lobbyA.SendPersonalChat(idB, this.testString, r => sendPersonal = r);
            yield return TestHelper.WaitForValue(() => sendPersonal);

            Result sendPartyChat = null;
            lobbyA.SendPartyChat(this.testString, r => sendPartyChat = r);
            yield return TestHelper.WaitForValue(() => sendPartyChat);

            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // Result sendChannel = null;
            // lobbyA.SendChannelChat(testString, r => sendChannel = r);
            // yield return TestHelper.WaitForValue(() => sendChannel);

            Result setuserStatus = null;
            lobbyA.SetUserStatus(UserStatus.Availabe, this.testString, r => setuserStatus = r);
            yield return TestHelper.WaitForValue(() => setuserStatus);

            // Wait Act result
            yield return TestHelper.WaitForValue(() => personalChat);
            yield return TestHelper.WaitForValue(() => partyChat);
            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // yield return TestHelper.WaitForValue(() => channelChat);
            yield return TestHelper.WaitForValue(() => bGetActivityA);

            // Cleanup
            Result unfriedResult = null;
            lobbyA.Unfriend(idB, r => unfriedResult = r);
            yield return TestHelper.WaitForValue(() => unfriedResult);

            Result aLeaveResult = null;
            lobbyA.LeaveParty(r => aLeaveResult = r);
            yield return TestHelper.WaitForValue(() => aLeaveResult);

            Result bLeaveResult = null;
            lobbyB.LeaveParty(r => bLeaveResult = r);
            yield return TestHelper.WaitForValue(() => bLeaveResult);

            lobbyA.Disconnect();
            lobbyB.Disconnect();

            // Assert
            Assert.AreEqual(this.resultLevelMandatory, personalChat);
            Assert.AreEqual(this.resultLevelMandatory, partyChat);
            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // Assert.AreEqual(resultLevelMandatory, channelChat);
            // TODO, Assert check on user status notif
            //Assert.AreEqual(resultLevelMandatory, bGetActivityA);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator ProfanityFilter_All()
        {
            // Arrange
            var lobbyA = this.CreateLobby(this.users[0].Session);
            var lobbyB = this.CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId;
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Set profanity filter level for user B
            Result setProfanityFilterLevel = null;
            lobbyB.SetProfanityFilterLevel(ProfanityFilterLevel.all, r => setProfanityFilterLevel = r);
            yield return TestHelper.WaitForValue(() => setProfanityFilterLevel);
            TestHelper.LogResult(setProfanityFilterLevel, "B Set Profanity Filter");

            // User A join party with user B
            // Set listener B party invite and accept invite
            Result<PartyInfo> bJoinedParty = null;
            ResultCallback<PartyInvitation> bInvitedCallback = invite =>
            {
                lobbyB.JoinParty(invite.Value.partyID, invite.Value.invitationToken, r => bJoinedParty = r);
            };
            lobbyB.InvitedToParty += bInvitedCallback;

            // A crate party
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // A invite B to party
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            TestHelper.LogResult(invitePartyResult, "A send party invite");

            // Wait B join party
            yield return TestHelper.WaitForValue(() => bJoinedParty);
            TestHelper.LogResult(bJoinedParty, "B join party");

            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // A & B join chat channel
            // Result<ChatChannelSlug> aJoinChannel = null;
            // lobbyA.JoinDefaultChatChannel(r => aJoinChannel = r);
            // yield return TestHelper.WaitForValue(() => aJoinChannel);
            // Result<ChatChannelSlug> bJoinChannel = null;
            // lobbyB.JoinDefaultChatChannel(r => bJoinChannel = r);
            // yield return TestHelper.WaitForValue(() => bJoinChannel);

            // A Friended B
            // Set B listen to friend request & accept
            Result bAcceptFriend = null;
            ResultCallback<Friend> AcceptIncomingFriendRequest = request =>
            {
                lobbyB.AcceptFriend(request.Value.friendId, r => bAcceptFriend = r);
            };
            lobbyB.OnIncomingFriendRequest += AcceptIncomingFriendRequest;

            // A send friend request to B
            Result aRequestFriend = null;
            lobbyA.RequestFriend(idB, r => aRequestFriend = r);
            yield return TestHelper.WaitForValue(() => aRequestFriend);

            // Wait B accept friend request
            yield return TestHelper.WaitForValue(() => bAcceptFriend);

            string personalChat = null;
            ResultCallback<ChatMessage> personalChatCallback = msg =>
            {
                personalChat = msg.Value.payload;
            };
            lobbyB.PersonalChatReceived += personalChatCallback;

            string partyChat = null;
            ResultCallback<ChatMessage> partyChatCallback = msg =>
            {
                partyChat = msg.Value.payload;
            };
            lobbyB.PartyChatReceived += partyChatCallback;

            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // string channelChat = null;
            // ResultCallback<ChannelChatMessage> channelChatCallback = msg =>
            // {
            //     channelChat = msg.Value.payload;
            // };
            // lobbyB.ChannelChatReceived += channelChatCallback;

            string bGetActivityA = null;
            ResultCallback<FriendsStatusNotif> friendStatusChangeCallback = notif =>
            {
                bGetActivityA = notif.Value.activity;
            };
            lobbyB.FriendsStatusChanged += friendStatusChangeCallback;

            // Act
            Result sendPersonal = null;
            lobbyA.SendPersonalChat(idB, this.testString, r => sendPersonal = r);
            yield return TestHelper.WaitForValue(() => sendPersonal);
            TestHelper.Assert.IsResultOk(sendPersonal, "send personal chat");

            Result sendPartyChat = null;
            lobbyA.SendPartyChat(this.testString, r => sendPartyChat = r);
            yield return TestHelper.WaitForValue(() => sendPartyChat);
            TestHelper.Assert.IsResultOk(sendPartyChat, "send party chat");

            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // Result sendChannel = null;
            // lobbyA.SendChannelChat(testString, r => sendChannel = r);
            // yield return TestHelper.WaitForValue(() => sendChannel);
            // TestHelper.Assert.IsResultOk(sendChannel, "send channel chat");

            Result setuserStatus = null;
            lobbyA.SetUserStatus(UserStatus.Availabe, this.testString, r => setuserStatus = r);
            yield return TestHelper.WaitForValue(() => setuserStatus);
            TestHelper.Assert.IsResultOk(setuserStatus, "set user status");

            // Wait Act result
            yield return TestHelper.WaitForValue(() => personalChat);
            yield return TestHelper.WaitForValue(() => partyChat);
            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // yield return TestHelper.WaitForValue(() => channelChat);
            yield return TestHelper.WaitForValue(() => bGetActivityA);


            // Cleanup
            Result unfriedResult = null;
            lobbyA.Unfriend(idB, r => unfriedResult = r);
            yield return TestHelper.WaitForValue(() => unfriedResult);
            
            Result aLeaveResult = null;
            lobbyA.LeaveParty(r => aLeaveResult = r);
            yield return TestHelper.WaitForValue(() => aLeaveResult);

            Result bLeaveResult = null;
            lobbyB.LeaveParty(r => bLeaveResult = r);
            yield return TestHelper.WaitForValue(() => bLeaveResult);

            lobbyA.Disconnect();
            lobbyB.Disconnect();

            // Assert
            Assert.AreEqual(this.resultLevelAll, personalChat);
            Assert.AreEqual(this.resultLevelAll, partyChat);
            //NOTE: Channel chat profanity filter turned off from backend side due to performance problem
            // Assert.AreEqual(resultLevelAll, channelChat);
            // TODO, Assert check on user status notif
            //Assert.AreEqual(resultLevelAll, bGetActivityA);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator ProfanityFilter_None()
        {
            // Arrange
            var lobbyA = this.CreateLobby(this.users[0].Session);
            var lobbyB = this.CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId;
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // Set profanity filter level for user B
            Result setProfanityFilterLevel = null;
            lobbyB.SetProfanityFilterLevel(ProfanityFilterLevel.none, r => setProfanityFilterLevel = r);
            yield return TestHelper.WaitForValue(() => setProfanityFilterLevel);
            TestHelper.LogResult(setProfanityFilterLevel, "B Set Profanity Filter");

            // User A join party with user B
            // Set listener B party invite and accept invite
            Result<PartyInfo> bJoinedParty = null;
            ResultCallback<PartyInvitation> bInvitedCallback = invite =>
            {
                lobbyB.JoinParty(invite.Value.partyID, invite.Value.invitationToken, r => bJoinedParty = r);
            };
            lobbyB.InvitedToParty += bInvitedCallback;

            // A crate party
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // A invite B to party
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            TestHelper.LogResult(invitePartyResult, "A send party invite");

            // Wait B join party
            yield return TestHelper.WaitForValue(() => bJoinedParty);
            TestHelper.LogResult(bJoinedParty, "B join party");

            // A & B join chat channel
            Result<ChatChannelSlug> aJoinChannel = null;
            lobbyA.JoinDefaultChatChannel(r => aJoinChannel = r);
            yield return TestHelper.WaitForValue(() => aJoinChannel);
            Result<ChatChannelSlug> bJoinChannel = null;
            lobbyB.JoinDefaultChatChannel(r => bJoinChannel = r);
            yield return TestHelper.WaitForValue(() => bJoinChannel);

            // A Friended B
            // Set B listen to friend request & accept
            Result bAcceptFriend = null;
            ResultCallback<Friend> AcceptIncomingFriendRequest = request =>
            {
                lobbyB.AcceptFriend(request.Value.friendId, r => bAcceptFriend = r);
            };
            lobbyB.OnIncomingFriendRequest += AcceptIncomingFriendRequest;

            // A send friend request to B
            Result aRequestFriend = null;
            lobbyA.RequestFriend(idB, r => aRequestFriend = r);
            yield return TestHelper.WaitForValue(() => aRequestFriend);

            // Wait B accept friend request
            yield return TestHelper.WaitForValue(() => bAcceptFriend);

            string personalChat = null;
            ResultCallback<ChatMessage> personalChatCallback = msg =>
            {
                personalChat = msg.Value.payload;
            };
            lobbyB.PersonalChatReceived += personalChatCallback;

            string partyChat = null;
            ResultCallback<ChatMessage> partyChatCallback = msg =>
            {
                partyChat = msg.Value.payload;
            };
            lobbyB.PartyChatReceived += partyChatCallback;

            string channelChat = null;
            ResultCallback<ChannelChatMessage> channelChatCallback = msg =>
            {
                channelChat = msg.Value.payload;
            };
            lobbyB.ChannelChatReceived += channelChatCallback;

            string bGetActivityA = null;
            ResultCallback<FriendsStatusNotif> friendStatusChangeCallback = notif =>
            {
                bGetActivityA = notif.Value.activity;
            };
            lobbyB.FriendsStatusChanged += friendStatusChangeCallback;

            // Act
            Result sendPersonal = null;
            lobbyA.SendPersonalChat(idB, this.testString, r => sendPersonal = r);
            yield return TestHelper.WaitForValue(() => sendPersonal);

            Result sendPartyChat = null;
            lobbyA.SendPartyChat(this.testString, r => sendPartyChat = r);
            yield return TestHelper.WaitForValue(() => sendPartyChat);

            Result sendChannel = null;
            lobbyA.SendChannelChat(this.testString, r => sendChannel = r);
            yield return TestHelper.WaitForValue(() => sendChannel);

            Result setuserStatus = null;
            lobbyA.SetUserStatus(UserStatus.Availabe, this.testString, r => setuserStatus = r);
            yield return TestHelper.WaitForValue(() => setuserStatus);


            // Wait Act result
            yield return TestHelper.WaitForValue(() => personalChat);
            yield return TestHelper.WaitForValue(() => partyChat);
            yield return TestHelper.WaitForValue(() => channelChat);
            yield return TestHelper.WaitForValue(() => bGetActivityA);


            // Cleanup
            Result unfriedResult = null;
            lobbyA.Unfriend(idB, r => unfriedResult = r);
            yield return TestHelper.WaitForValue(() => unfriedResult);

            Result aLeaveResult = null;
            lobbyA.LeaveParty(r => aLeaveResult = r);
            yield return TestHelper.WaitForValue(() => aLeaveResult);

            Result bLeaveResult = null;
            lobbyB.LeaveParty(r => bLeaveResult = r);
            yield return TestHelper.WaitForValue(() => bLeaveResult);

            lobbyA.Disconnect();
            lobbyB.Disconnect();



            // Assert
            Assert.AreEqual(this.resultLevelNone, personalChat);
            Assert.AreEqual(this.resultLevelNone, partyChat);
            Assert.AreEqual(this.resultLevelNone, channelChat);
            // TODO, Assert check on user status notif
            //Assert.AreEqual(resultLevelNone, bGetActivityA);
        }

        [UnityTest, TestLog, Order(1), Timeout(30000)]
        public IEnumerator ProfanityFilter_Default()
        {
            // Arrange
            var lobbyA = this.CreateLobby(this.users[0].Session);
            var lobbyB = this.CreateLobby(this.users[1].Session);
            string idA = this.users[0].Session.UserId;
            string idB = this.users[1].Session.UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();
            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);
            if (!lobbyB.IsConnected) lobbyB.Connect();
            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            // User A join party with user B
            // Set listener B party invite and accept invite
            Result<PartyInfo> bJoinedParty = null;
            ResultCallback<PartyInvitation> bInvitedCallback = invite =>
            {
                lobbyB.JoinParty(invite.Value.partyID, invite.Value.invitationToken, r => bJoinedParty = r);
            };
            lobbyB.InvitedToParty += bInvitedCallback;

            // A crate party
            Result<PartyInfo> createPartyResult = null;
            lobbyA.CreateParty(r => createPartyResult = r);
            yield return TestHelper.WaitForValue(() => createPartyResult);

            // A invite B to party
            Result invitePartyResult = null;
            lobbyA.InviteToParty(idB, r => invitePartyResult = r);
            yield return TestHelper.WaitForValue(() => invitePartyResult);
            TestHelper.LogResult(invitePartyResult, "A send party invite");

            // Wait B join party
            yield return TestHelper.WaitForValue(() => bJoinedParty);
            TestHelper.LogResult(bJoinedParty, "B join party");

            // A & B join chat channel
            Result<ChatChannelSlug> aJoinChannel = null;
            lobbyA.JoinDefaultChatChannel(r => aJoinChannel = r);
            yield return TestHelper.WaitForValue(() => aJoinChannel);
            Result<ChatChannelSlug> bJoinChannel = null;
            lobbyB.JoinDefaultChatChannel(r => bJoinChannel = r);
            yield return TestHelper.WaitForValue(() => bJoinChannel);

            // A Friended B
            // Set B listen to friend request & accept
            Result bAcceptFriend = null;
            ResultCallback<Friend> AcceptIncomingFriendRequest = request =>
            {
                lobbyB.AcceptFriend(request.Value.friendId, r => bAcceptFriend = r);
            };
            lobbyB.OnIncomingFriendRequest += AcceptIncomingFriendRequest;

            // A send friend request to B
            Result aRequestFriend = null;
            lobbyA.RequestFriend(idB, r => aRequestFriend = r);
            yield return TestHelper.WaitForValue(() => aRequestFriend);

            // Wait B accept friend request
            yield return TestHelper.WaitForValue(() => bAcceptFriend);

            string personalChat = null;
            ResultCallback<ChatMessage> personalChatCallback = msg =>
            {
                personalChat = msg.Value.payload;
            };
            lobbyB.PersonalChatReceived += personalChatCallback;

            string partyChat = null;
            ResultCallback<ChatMessage> partyChatCallback = msg =>
            {
                partyChat = msg.Value.payload;
            };
            lobbyB.PartyChatReceived += partyChatCallback;

            string channelChat = null;
            ResultCallback<ChannelChatMessage> channelChatCallback = msg =>
            {
                channelChat = msg.Value.payload;
            };
            lobbyB.ChannelChatReceived += channelChatCallback;

            string bGetActivityA = null;
            ResultCallback<FriendsStatusNotif> friendStatusChangeCallback = notif =>
            {
                bGetActivityA = notif.Value.activity;
            };
            lobbyB.FriendsStatusChanged += friendStatusChangeCallback;

            // Act
            Result sendPersonal = null;
            lobbyA.SendPersonalChat(idB, this.testString, r => sendPersonal = r);
            yield return TestHelper.WaitForValue(() => sendPersonal);

            Result sendPartyChat = null;
            lobbyA.SendPartyChat(this.testString, r => sendPartyChat = r);
            yield return TestHelper.WaitForValue(() => sendPartyChat);

            Result sendChannel = null;
            lobbyA.SendChannelChat(this.testString, r => sendChannel = r);
            yield return TestHelper.WaitForValue(() => sendChannel);

            Result setuserStatus = null;
            lobbyA.SetUserStatus(UserStatus.Availabe, this.testString, r => setuserStatus = r);
            yield return TestHelper.WaitForValue(() => setuserStatus);


            // Wait Act result
            yield return TestHelper.WaitForValue(() => personalChat);
            yield return TestHelper.WaitForValue(() => partyChat);
            yield return TestHelper.WaitForValue(() => channelChat);
            yield return TestHelper.WaitForValue(() => bGetActivityA);


            // Cleanup
            Result unfriedResult = null;
            lobbyA.Unfriend(idB, r => unfriedResult = r);
            yield return TestHelper.WaitForValue(() => unfriedResult);

            Result aLeaveResult = null;
            lobbyA.LeaveParty(r => aLeaveResult = r);
            yield return TestHelper.WaitForValue(() => aLeaveResult);

            Result bLeaveResult = null;
            lobbyB.LeaveParty(r => bLeaveResult = r);
            yield return TestHelper.WaitForValue(() => bLeaveResult);

            lobbyA.Disconnect();
            lobbyB.Disconnect();



            // Assert
            Assert.AreEqual(this.resultLevelAll, personalChat);
            Assert.AreEqual(this.resultLevelAll, partyChat);
            // Assert.AreEqual(resultLevelAll, channelChat); // not filterd by default
            // TODO, Assert check on user status notif
            //Assert.AreEqual(resultLevelNone, bGetActivityA);
        }
    }
}