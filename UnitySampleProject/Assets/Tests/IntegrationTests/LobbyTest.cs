// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Linq;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using NUnit.Framework.Api;
using Steamworks;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using System.Threading;
using NUnit.Framework.Internal.Commands;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class LobbyTest
    {
        private TestHelper helper;
        private User[] users;
        private int setupCount;
        private AsyncTaskDispatcher taskDispatcher;
        private CoroutineRunner coroutineRunner;
        private AuthenticationApi authenticationApi;
        private UserApi userApi;
        private bool canConstruct;
        private bool canDestroy;

        [UnityTest, Order(0), Timeout(300000)]
        public IEnumerator TestSetup()
        {
            this.authenticationApi = new AuthenticationApi(AccelBytePlugin.Config.IamServerUrl);
            this.userApi = new UserApi(AccelBytePlugin.Config.IamServerUrl);
            this.taskDispatcher = new AsyncTaskDispatcher();
            this.coroutineRunner = new CoroutineRunner();
            this.helper = new TestHelper();

            this.users = new User[5];

            for (int i = 0; i < this.users.Length; i++)
            {
                Result<UserData> registerResult = null;

                this.users[i] = new User(
                    this.authenticationApi,
                    this.userApi,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.ClientId,
                    AccelBytePlugin.Config.ClientSecret,
                    AccelBytePlugin.Config.RedirectUri,
                    this.taskDispatcher,
                    this.coroutineRunner);

                this.users[i]
                    .Register(
                        string.Format("lobbyuser{0}+accelbyteunitysdk@example.com", i + 1),
                        "password",
                        "lobbyuser" + (i + 1),
                        result => registerResult = result);

                while (registerResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                TestHelper.LogResult(registerResult, "Setup: Registered lobbyuser" + (i + 1));
            }

            for (int i = 0; i < this.users.Length; i++)
            {
                Result loginResult = null;

                this.users[i]
                    .LoginWithUserName(
                        string.Format("lobbyuser{0}+accelbyteunitysdk@example.com", i + 1),
                        "password",
                        result => loginResult = result);

                while (loginResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                TestHelper.LogResult(loginResult, "Setup: Logged in " + this.users[i].DisplayName);
            }
        }

        [UnityTest, Order(1)]
        public IEnumerator SendPrivateChat_FromMultipleUsers_ChatReceived()
        {
            var lobbies = new Lobby[this.users.Length];

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = new Lobby(
                    AccelBytePlugin.Config.LobbyServerUrl,
                    this.users[i],
                    this.taskDispatcher,
                    this.coroutineRunner);

                lobbies[i].Connect();
            }

            int receivedChatCount = 0;

            lobbies[0].PersonalChatReceived += result =>
            {
                receivedChatCount++;
                Debug.Log(result.Value.payload);
            };

            for (int i = 0; i < lobbies.Length; i++)
            {
                var userId = this.users[0].UserId;
                var chatMessage = "Hello " + this.users[0].DisplayName + " from " + this.users[i].DisplayName;

                Result privateChatResult = null;
                lobbies[i].SendPersonalChat(userId, chatMessage, result => privateChatResult = result);

                yield return new WaitUntil(() => privateChatResult != null);

                Debug.Log(privateChatResult);
            }

            yield return new WaitUntil(() => receivedChatCount >= this.users.Length);

            Assert.IsTrue(true);

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }
        }

        [UnityTest, Order(1), Timeout(120000)]
        public IEnumerator ListOnlineFriends_MultipleUsersConnected_ReturnAllUsers()
        {
            var lobbies = new Lobby[this.users.Length];

            for (int i = 0; i < lobbies.Length; i++)
            {
                lobbies[i] = new Lobby(
                    AccelBytePlugin.Config.LobbyServerUrl,
                    this.users[i],
                    this.taskDispatcher,
                    this.coroutineRunner);

                lobbies[i].Connect();
            }

            Debug.Log("Online users:\n");

            foreach (var s in this.users)
            {
                Debug.Log(s.UserId);
            }

            Result userStatusResult;

            for (int i = 1; i < 4; i++)
            {
                Result requestFriendResult = null;
                lobbies[i].RequestFriend(this.users[0].UserId, result => requestFriendResult = result);

                while (requestFriendResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                Result acceptFriendResult = null;

                lobbies[0].AcceptFriend(this.users[i].UserId, result => acceptFriendResult = result);

                while (acceptFriendResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                userStatusResult = null;
                lobbies[i].SetUserStatus(UserStatus.Availabe, "random activity", result => userStatusResult = result);

                while (userStatusResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                TestHelper.LogResult(userStatusResult, "Set User Status Online");
            }

            Result<FriendsStatus> onlineFriendsResult = null;
            lobbies[0].ListFriendsStatus(result => onlineFriendsResult = result);

            while (onlineFriendsResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(onlineFriendsResult, "List Friends Presence");

            for (int i = 1; i < 4; i++)
            {
                userStatusResult = null;
                lobbies[i].SetUserStatus(UserStatus.Offline, "disappearing", result => userStatusResult = result);

                while (userStatusResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                TestHelper.LogResult(userStatusResult, "Set User Status Offline");

                Result unfriendResult = null;
                lobbies[i].Unfriend(this.users[0].UserId, result => unfriendResult = result);

                while (unfriendResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            Assert.False(onlineFriendsResult.IsError);
            Assert.That(onlineFriendsResult.Value.friendsId.Length, Is.GreaterThanOrEqualTo(3));
            Assert.IsTrue(onlineFriendsResult.Value.friendsId.Contains(this.users[1].UserId));
            Assert.IsTrue(onlineFriendsResult.Value.friendsId.Contains(this.users[2].UserId));
            Assert.IsTrue(onlineFriendsResult.Value.friendsId.Contains(this.users[3].UserId));
        }

        [UnityTest, Order(1)]
        public IEnumerator GetPartyInfo_NoParty_ReturnError()
        {
            var lobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby.Connect();

            //Ensure that user is not in party, doesn't care about its result.
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Result<PartyInfo> partyInfoResult = null;
            lobby.GetPartyInfo(result => partyInfoResult = result);

            while (partyInfoResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            lobby.Disconnect();

            Assert.That(partyInfoResult.IsError);
        }

        [UnityTest, Order(1)]
        public IEnumerator GetPartyInfo_PartyCreated_ReturnOk()
        {
            var lobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby.Connect();

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(createPartyResult, "Create Party");

            Result<PartyInfo> getPartyInfoResult = null;
            lobby.GetPartyInfo(result => getPartyInfoResult = result);

            while (getPartyInfoResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(getPartyInfoResult, "Get Party Info");
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            lobby.Disconnect();

            Assert.False(getPartyInfoResult.IsError);
            Assert.That(getPartyInfoResult.Value.partyID, Is.Not.Null.Or.Empty);
            Assert.That(getPartyInfoResult.Value.invitationToken, Is.Not.Null.Or.Empty);
            Assert.That(getPartyInfoResult.Value.members.Length, Is.GreaterThan(0));
            Assert.That(getPartyInfoResult.Value.members[0], Is.EqualTo(this.users[0].UserId));
        }

        [UnityTest, Order(1)]
        public IEnumerator CreateParty_PartyAlreadyCreated_ReturnError()
        {
            var lobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby.Connect();

            //Ensure there is no party before, doesn't care about its result
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(createPartyResult, "Create Party");

            Result<PartyInfo> createPartyResult2 = null;
            lobby.CreateParty(result => createPartyResult2 = result);

            while (createPartyResult2 == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(createPartyResult2, "Create Party Again");

            leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            lobby.Disconnect();

            Assert.False(createPartyResult.IsError);
            Assert.True(createPartyResult2.IsError);
        }

        [UnityTest, Order(1)]
        public IEnumerator InviteToParty_InvitationAccepted_CanChat()
        {
            var lobby1 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobby2 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby1.Connect();
            lobby2.Connect();

            Result<PartyInfo> createPartyResult = null;
            lobby1.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(createPartyResult, "Create Party");
            Result<PartyInvitation> invitedToPartyResult = null;
            lobby2.InvitedToParty += result => invitedToPartyResult = result;
            Result inviteResult = null;
            lobby1.InviteToParty(this.users[1].UserId, result => inviteResult = result);

            while (inviteResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(inviteResult, "Invite To Party");

            while (invitedToPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            Result<PartyInfo> joinPartyResult = null;

            lobby2.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);

            while (joinPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(joinPartyResult, "Join Party");

            Result<ChatMesssage> receivedPartyChatResult = null;
            lobby1.PartyChatReceived += result => receivedPartyChatResult = result;

            Result sendPartyChatResult = null;
            lobby2.SendPartyChat("This is a party chat", result => sendPartyChatResult = result);

            while (sendPartyChatResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(sendPartyChatResult, "Send Party Chat");

            while (receivedPartyChatResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(receivedPartyChatResult, "Received Party Chat");
            Result leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby2.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

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

        [UnityTest, Order(1), Timeout(120000)]
        public IEnumerator PartyMember_Kicked_CannotChat()
        {
            var lobby1 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobby2 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobby3 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[2],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby1.Connect();
            lobby2.Connect();
            lobby3.Connect();

            //1. lobby1 create party
            Result<PartyInfo> createPartyResult = null;
            lobby1.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(createPartyResult, "Create Party");

            //2. lobby2 join party
            Result<PartyInvitation> invitedToPartyResult = null;
            lobby2.InvitedToParty += result => invitedToPartyResult = result;
            Result inviteResult = null;
            lobby1.InviteToParty(this.users[1].UserId, result => inviteResult = result);

            while (inviteResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(inviteResult, "Invite To Party");

            while (invitedToPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            Result<PartyInfo> joinPartyResult = null;
            lobby2.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);

            while (joinPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(joinPartyResult, "Join Party");

            invitedToPartyResult = null;
            lobby3.InvitedToParty += result => invitedToPartyResult = result;
            inviteResult = null;
            lobby1.InviteToParty(this.users[2].UserId, result => inviteResult = result);

            while (inviteResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(inviteResult, "Invite To Party");

            while (invitedToPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            joinPartyResult = null;
            lobby3.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);

            while (joinPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(joinPartyResult, "Join Party");

            //3. Kick member

            Result kickResult = null;
            Result<KickNotification> kickedFromPartyResult = null;
            lobby3.KickedFromParty += result => kickedFromPartyResult = result;

            lobby1.KickPartyMember(this.users[2].UserId, result => kickResult = result);

            while (kickResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(kickResult, "Kick Member");

            while (kickedFromPartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(kickedFromPartyResult, "Kicked From Party");

            Result<PartyInfo> partyInfoResult = null;
            lobby2.GetPartyInfo(result => partyInfoResult = result);

            while (partyInfoResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //4. Leave party and disconnect
            Result leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby2.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby3.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            lobby1.Disconnect();
            lobby2.Disconnect();
            lobby3.Disconnect();

            Assert.False(kickResult.IsError);
            Assert.False(kickedFromPartyResult.IsError);
            Assert.That(joinPartyResult.Value.members.Length, Is.EqualTo(3));
            Assert.That(partyInfoResult.Value.members.Length, Is.EqualTo(2));
        }

        [UnityTest, Order(1), Timeout(120000), Ignore("Test takes too long.")]
        public IEnumerator LobbyConnected_ForMoreThan1Minutes_DoesntDisconnect()
        {
            var lobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby.Connect();

            for (int i = 0; i < 100; i += 5)
            {
                Thread.Sleep(5000);
                Debug.Log(string.Format("Wait for {0} seconds. Lobby.IsConnected={1}", i, lobby.IsConnected));

                yield return null;
            }

            Assert.That(lobby.IsConnected);

            lobby.Disconnect();
        }

        [UnityTest, Order(1)]
        public IEnumerator Notification_GetAsyncNotification()
        {
            Result sendNotificationResult = null;
            string notification = "this is a notification";
            this.helper.SendNotification(
                this.users[0].UserId,
                true,
                notification,
                result => { sendNotificationResult = result; });

            while (sendNotificationResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.LogResult(sendNotificationResult, "send notification");

            var lobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            lobby.Connect();

            Debug.Log("Connected to lobby");

            Result<Notification> getNotificationResult = null;
            lobby.OnNotification += result => getNotificationResult = result;

            Result pullResult = null;
            lobby.PullAsyncNotifications(result => pullResult = result);

            while (pullResult == null) yield return new WaitForSeconds(.2f);

            while (getNotificationResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.LogResult(getNotificationResult);

            lobby.Disconnect();

            yield return null;

            Assert.IsNotNull(sendNotificationResult);
            Assert.IsNotNull(getNotificationResult);
            Assert.IsFalse(sendNotificationResult.IsError);
            Assert.IsFalse(getNotificationResult.IsError);
            Assert.IsTrue(getNotificationResult.Value.payload == notification);
        }

        [UnityTest, Order(1)]
        public IEnumerator Notification_GetSyncNotification()
        {
            const int repetition = 2;

            bool[] getNotifSuccess = new bool[repetition];
            string[] payloads = new string[repetition];
            Result[] sendNotificationResults = new Result[repetition];

            var lobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
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
                    this.users[0].UserId,
                    false,
                    payloads[i],
                    result => { sendNotificationResults[i] = result; });

                while (sendNotificationResults[i] == null) yield return new WaitForSeconds(.2f);

                TestHelper.LogResult(sendNotificationResults[i]);

                while (incomingNotification == null) yield return new WaitForSeconds(.2f);

                if (incomingNotification.Value.payload == payloads[i])
                {
                    getNotifSuccess[i] = true;
                }

                incomingNotification = null;

                Assert.That(getNotifSuccess[i]);
            }

            lobby.Disconnect();
        }

        [UnityTest, Order(1)]
        public IEnumerator SetUserStatus_CheckedByAnotherUser()
        {
            UserStatus ExpectedUser1Status = UserStatus.Availabe;

            var lobby1 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            lobby1.Connect();

            Debug.Log("User1 Connected to lobby");

            var lobby2 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);
            lobby2.Connect();

            Debug.Log("User2 Connected to lobby");

            Result requestFriendResult = null;
            lobby1.RequestFriend(this.users[1].UserId, result => requestFriendResult = result);

            while (requestFriendResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].UserId, result => acceptFriendResult = result);

            while (acceptFriendResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Result setUser2StatusResult = null;
            lobby2.SetUserStatus(UserStatus.Availabe, "ready to play", result => { setUser2StatusResult = result; });

            while (setUser2StatusResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.LogResult(setUser2StatusResult);

            Result<FriendsStatusNotif> listenUser1StatusResult = null;
            lobby2.FriendsStatusChanged += result => { listenUser1StatusResult = result; };

            Result setUser1StatusResult = null;
            lobby1.SetUserStatus(
                ExpectedUser1Status,
                "expected activity",
                result => { setUser1StatusResult = result; });

            while (setUser1StatusResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.LogResult(setUser1StatusResult);

            while (listenUser1StatusResult == null) yield return new WaitForSeconds(.1f);

            while (listenUser1StatusResult.Value.availability != ((int) ExpectedUser1Status).ToString())
            {
                yield return new WaitForSeconds(.1f);
            }

            TestHelper.LogResult(listenUser1StatusResult);

            Result unfriendResult = null;
            lobby2.Unfriend(this.users[0].UserId, result => unfriendResult = result);

            while (unfriendResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            lobby1.Disconnect();
            lobby2.Disconnect();

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(listenUser1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(listenUser1StatusResult.IsError);
            Assert.AreEqual(((int) ExpectedUser1Status).ToString(), listenUser1StatusResult.Value.availability);
        }

        [UnityTest, Order(1)]
        public IEnumerator ChangeUserStatus_CheckedByAnotherUser()
        {
            UserStatus ExpectedUser1Status = UserStatus.Busy;

            var lobby1 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby1.Connect();

            Debug.Log("User1 Connected to lobby");

            var lobby2 = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);

            lobby2.Connect();

            Debug.Log("User2 Connected to lobby");

            Result requestFriendResult = null;
            lobby1.RequestFriend(this.users[1].UserId, result => requestFriendResult = result);

            while (requestFriendResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].UserId, result => acceptFriendResult = result);

            while (acceptFriendResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Result setUser2StatusResult = null;
            lobby2.SetUserStatus(
                UserStatus.Availabe,
                "ready to play again",
                result => { setUser2StatusResult = result; });

            while (setUser2StatusResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.LogResult(setUser2StatusResult);

            Result setUser1StatusResult = null;
            lobby1.SetUserStatus(
                UserStatus.Availabe,
                "ready to play too",
                result => { setUser1StatusResult = result; });

            while (setUser1StatusResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.LogResult(setUser1StatusResult);

            Result<FriendsStatusNotif> listenUser1StatusResult = null;
            lobby2.FriendsStatusChanged += result => { listenUser1StatusResult = result; };

            Result changeUser1StatusResult = null;
            lobby1.SetUserStatus(
                ExpectedUser1Status,
                "busy, can't play",
                result => { changeUser1StatusResult = result; });

            while (listenUser1StatusResult == null) yield return new WaitForSeconds(.1f);

            TestHelper.LogResult(listenUser1StatusResult);

            Result unfriendResult = null;
            lobby2.Unfriend(this.users[0].UserId, result => unfriendResult = result);

            while (unfriendResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            lobby1.Disconnect();
            lobby2.Disconnect();

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(listenUser1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(listenUser1StatusResult.IsError);
            Assert.AreEqual((int) ExpectedUser1Status, int.Parse(listenUser1StatusResult.Value.availability));
        }

        [UnityTest, Order(2), Timeout(40000)]
        public IEnumerator Friends_Request_Accept()
        {
            var lobbyA = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobbyB = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);
            string idA = this.users[0].UserId, idB = this.users[1].UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.NotFriend)));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendResult.IsError));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Outgoing)));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listOutgoingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB)));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Incoming)));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA)));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!acceptFriendRequestResult.IsError));

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            while (loadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListResult.IsError));
            TestHelper.Assert(() => Assert.That(loadFriendListResult.Value.friendsId.Contains(idB)));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            while (anotherLoadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!anotherLoadFriendListResult.IsError));
            TestHelper.Assert(() => Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA)));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!unfriendResult.IsError));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(40000)]
        public IEnumerator Friends_Notification_Request_Accept()
        {
            string serverUrl = AccelBytePlugin.Config.LobbyServerUrl.Replace("https", "wss");

            var lobbyA = new Lobby(serverUrl, this.users[0], this.taskDispatcher, this.coroutineRunner);
            var lobbyB = new Lobby(serverUrl, this.users[1], this.taskDispatcher, this.coroutineRunner);
            string idA = this.users[0].UserId, idB = this.users[1].UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<Friend> incomingNotificationFromAResult = null;
            lobbyB.OnIncomingFriendRequest += result => { incomingNotificationFromAResult = result; };

            Result<Friend> friendRequestAcceptedResult = null;
            lobbyA.FriendRequestAccepted += result => { friendRequestAcceptedResult = result; };

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendResult.IsError));

            while (incomingNotificationFromAResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!incomingNotificationFromAResult.IsError));
            TestHelper.Assert(() => Assert.That(incomingNotificationFromAResult.Value.friendId == idA));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!acceptFriendRequestResult.IsError));

            while (friendRequestAcceptedResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!friendRequestAcceptedResult.IsError));
            TestHelper.Assert(() => Assert.That(friendRequestAcceptedResult.Value.friendId == idB));

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            while (loadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListResult.IsError));
            TestHelper.Assert(() => Assert.That(loadFriendListResult.Value.friendsId.Contains(idB)));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            while (anotherLoadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!anotherLoadFriendListResult.IsError));
            TestHelper.Assert(() => Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA)));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!unfriendResult.IsError));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(40000)]
        public IEnumerator Friends_Request_Unfriend()
        {
            var lobbyA = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobbyB = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);
            string idA = this.users[0].UserId, idB = this.users[1].UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.NotFriend)));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendResult.IsError));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Outgoing)));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listOutgoingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB)));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Incoming)));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA)));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!acceptFriendRequestResult.IsError));

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            while (loadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListResult.IsError));
            TestHelper.Assert(() => Assert.That(loadFriendListResult.Value.friendsId.Contains(idB)));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            while (anotherLoadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!anotherLoadFriendListResult.IsError));
            TestHelper.Assert(() => Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA)));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!unfriendResult.IsError));

            Result<Friends> loadFriendListAfterUnfriend = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterUnfriend = result; });

            while (loadFriendListAfterUnfriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListAfterUnfriend.IsError));

            if (loadFriendListAfterUnfriend.Value.friendsId.Length != 0)
            {
                TestHelper.Assert(() => Assert.That(!loadFriendListAfterUnfriend.Value.friendsId.Contains(idB)));
            }

            Result<Friends> loadFriendListAfterGotUnfriend = null;
            lobbyB.LoadFriendsList(result => { loadFriendListAfterGotUnfriend = result; });

            while (loadFriendListAfterGotUnfriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListAfterGotUnfriend.IsError));

            if (loadFriendListAfterGotUnfriend.Value.friendsId.Length != 0)
            {
                TestHelper.Assert(() => Assert.That(!loadFriendListAfterGotUnfriend.Value.friendsId.Contains(idA)));
            }

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(40000)]
        public IEnumerator Friends_Request_Reject()
        {
            var lobbyA = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobbyB = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);
            string idA = this.users[0].UserId, idB = this.users[1].UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.NotFriend)));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendResult.IsError));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Outgoing)));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listOutgoingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB)));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Incoming)));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA)));

            Result rejectFriendRequestResult = null;
            lobbyB.RejectFriend(idA, result => { rejectFriendRequestResult = result; });

            while (rejectFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!rejectFriendRequestResult.IsError));

            Result<FriendshipStatus> getFriendshipStatusAfterRejecting = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRejecting = result; });

            while (getFriendshipStatusAfterRejecting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRejecting.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRejecting.Value.friendshipStatus == RelationshipStatusCode.NotFriend));

            Result<Friends> listIncomingFriendsAfterRejecting = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendsAfterRejecting = result; });

            while (listIncomingFriendsAfterRejecting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendsAfterRejecting.IsError));
            TestHelper.Assert(() => Assert.That(!listIncomingFriendsAfterRejecting.Value.friendsId.Contains(idA)));

            Result<FriendshipStatus> getFriendshipStatusAfterRejected = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRejected = result; });

            while (getFriendshipStatusAfterRejected == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRejected.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRejected.Value.friendshipStatus == RelationshipStatusCode.NotFriend));

            Result<Friends> listOutgoingFriendsAfterRejected = null;
            lobbyA.ListIncomingFriends(result => { listOutgoingFriendsAfterRejected = result; });

            while (listOutgoingFriendsAfterRejected == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listOutgoingFriendsAfterRejected.IsError));
            TestHelper.Assert(() => Assert.That(!listOutgoingFriendsAfterRejected.Value.friendsId.Contains(idB)));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(40000)]
        public IEnumerator Friends_Request_Cancel()
        {
            var lobbyA = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobbyB = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);
            string idA = this.users[0].UserId, idB = this.users[1].UserId;

            if (!lobbyA.IsConnected) lobbyA.Connect();

            if (!lobbyB.IsConnected) lobbyB.Connect();

            while (!lobbyA.IsConnected) yield return new WaitForSeconds(.2f);

            while (!lobbyB.IsConnected) yield return new WaitForSeconds(.2f);

            Result<FriendshipStatus> getFriendshipStatusBeforeRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusBeforeRequestFriend = result; });

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.NotFriend)));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendResult.IsError));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Outgoing)));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listOutgoingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB)));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Incoming)));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA)));

            Result cancelFriendRequestResult = null;
            lobbyA.CancelFriendRequest(idB, result => { cancelFriendRequestResult = result; });

            while (cancelFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!cancelFriendRequestResult.IsError));

            Result<Friends> listIncomingFriendRequestAfterCanceled = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestAfterCanceled = result; });

            while (listIncomingFriendRequestAfterCanceled == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestAfterCanceled.IsError));
            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestAfterCanceled.Value.friendsId.Contains(idA)));

            Result<Friends> loadFriendListAfterCanceled = null;
            lobbyB.LoadFriendsList(result => { loadFriendListAfterCanceled = result; });

            while (loadFriendListAfterCanceled == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListAfterCanceled.IsError));

            if (loadFriendListAfterCanceled.Value.friendsId.Length != 0)
            {
                TestHelper.Assert(() => Assert.That(!loadFriendListAfterCanceled.Value.friendsId.Contains(idA)));
            }

            Result<Friends> loadFriendListAfterCanceling = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterCanceling = result; });

            while (loadFriendListAfterCanceling == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListAfterCanceling.IsError));

            if (loadFriendListAfterCanceling.Value.friendsId.Length != 0)
            {
                TestHelper.Assert(() => Assert.That(!loadFriendListAfterCanceling.Value.friendsId.Contains(idB)));
            }

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(40000)]
        public IEnumerator Friends_Complete_Scenario()
        {
            var lobbyA = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            var lobbyB = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[1],
                this.taskDispatcher,
                this.coroutineRunner);
            string idA = this.users[0].UserId, idB = this.users[1].UserId;

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
                this.users[1].UserId,
                result => { getFriendshipStatusBeforeRequestFriend = result; });

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.NotFriend)));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(this.users[1].UserId, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendResult.IsError));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listOutgoingFriendRequestResult.IsError));
            TestHelper.Assert(
                () => Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(this.users[1].UserId)));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(
                this.users[1].UserId,
                result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestFriend.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Outgoing)));

            Result cancelFriendRequestResult = null;
            lobbyA.CancelFriendRequest(this.users[1].UserId, result => { cancelFriendRequestResult = result; });

            while (cancelFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!cancelFriendRequestResult.IsError));

            Result<Friends> listOutgoingFriendAfterCanceling = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendAfterCanceling = result; });

            while (listOutgoingFriendAfterCanceling == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listOutgoingFriendAfterCanceling.IsError));

            if (listOutgoingFriendAfterCanceling.Value.friendsId.Length != 0)
            {
                TestHelper.Assert(
                    () => Assert.That(
                        !listOutgoingFriendAfterCanceling.Value.friendsId.Contains(this.users[1].UserId)));
            }

            Result requestFriendAfterCanceling = null;
            lobbyA.RequestFriend(this.users[1].UserId, result => { requestFriendAfterCanceling = result; });

            while (requestFriendAfterCanceling == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendAfterCanceling.IsError));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestResult.IsError));
            TestHelper.Assert(() => Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA)));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Incoming)));

            Result rejectFriendRequestResult = null;
            lobbyB.RejectFriend(idA, result => { rejectFriendRequestResult = result; });

            while (rejectFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!rejectFriendRequestResult.IsError));

            Result requestFriendAfterRejected = null;
            lobbyA.RequestFriend(this.users[1].UserId, result => { requestFriendAfterRejected = result; });

            while (requestFriendAfterRejected == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!requestFriendAfterRejected.IsError));

            while (incomingNotificationFromAResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!incomingNotificationFromAResult.IsError));
            TestHelper.Assert(() => Assert.That(incomingNotificationFromAResult.Value.friendId == idA));

            Result<Friends> listIncomingFriendRequestAfterRejecting = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestAfterRejecting = result; });

            while (listIncomingFriendRequestAfterRejecting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!listIncomingFriendRequestAfterRejecting.IsError));
            TestHelper.Assert(() => Assert.That(listIncomingFriendRequestAfterRejecting.Value.friendsId.Contains(idA)));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!acceptFriendRequestResult.IsError));

            while (friendRequestAcceptedResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!friendRequestAcceptedResult.IsError));
            TestHelper.Assert(() => Assert.That(friendRequestAcceptedResult.Value.friendId == idB));

            Result<Friends> loadFriendListAfterAccepted = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterAccepted = result; });

            while (loadFriendListAfterAccepted == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListAfterAccepted.IsError));
            TestHelper.Assert(
                () => Assert.That(loadFriendListAfterAccepted.Value.friendsId.Contains(this.users[1].UserId)));

            Result<FriendshipStatus> getFriendshipStatusAfterAccepted = null;
            lobbyA.GetFriendshipStatus(this.users[1].UserId, result => { getFriendshipStatusAfterAccepted = result; });

            while (getFriendshipStatusAfterAccepted == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterAccepted.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterAccepted.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Friend)));

            Result<FriendshipStatus> getFriendshipStatusAfterAccepting = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterAccepting = result; });

            while (getFriendshipStatusAfterAccepting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterAccepting.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterAccepting.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.Friend)));

            Result unfriendResult = null;
            lobbyA.Unfriend(this.users[1].UserId, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!unfriendResult.IsError));

            Result<Friends> loadFriendListAfterUnfriend = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterUnfriend = result; });

            while (loadFriendListAfterUnfriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!loadFriendListAfterUnfriend.IsError));

            if (loadFriendListAfterUnfriend.Value.friendsId.Length != 0)
            {
                TestHelper.Assert(
                    () => Assert.That(!loadFriendListAfterUnfriend.Value.friendsId.Contains(this.users[1].UserId)));
            }

            Result<FriendshipStatus> getFriendshipStatusAfterUnfriended = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterUnfriended = result; });

            while (getFriendshipStatusAfterUnfriended == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert(() => Assert.That(!getFriendshipStatusAfterUnfriended.IsError));
            TestHelper.Assert(
                () => Assert.That(
                    getFriendshipStatusAfterUnfriended.Value.friendshipStatus,
                    Is.EqualTo(RelationshipStatusCode.NotFriend)));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(1)]
        public IEnumerator StartMatchmaking_ReturnOk()
        {
            const int NumUsers = 2;
            Lobby[] lobbies = new Lobby[NumUsers];
            Result<MatchmakingCode>[] startMatchmakingResponses = new Result<MatchmakingCode>[NumUsers];
            Result<MatchmakingNotif>[] matchMakingNotifs = new Result<MatchmakingNotif>[NumUsers];

            for (int i = 0; i < NumUsers; i++)
            {
                var lobby = lobbies[i] = new Lobby(
                    AccelBytePlugin.Config.LobbyServerUrl,
                    this.users[i],
                    this.taskDispatcher,
                    this.coroutineRunner);
                lobby.Connect();

                while (!lobby.IsConnected) yield return new WaitForSeconds(0.2f);

                Debug.Log(string.Format("User{0} Connected to lobby", i));

                Result<PartyInfo> createPartyResult = null;
                lobbies[i].CreateParty(result => createPartyResult = result);

                while (createPartyResult == null)
                {
                    yield return new WaitForSeconds(0.2f);
                }

                Debug.Log(string.Format("User{0} Party created", i));
            }

            int responseNum = 0;
            int matchMakingNotifNum = 0;

            for (int i = 0; i < NumUsers; i++)
            {
                int index = i;
                Debug.Log(string.Format("Start matchmaking {0}", index));

                lobbies[i].MatchmakingCompleted += delegate(Result<MatchmakingNotif> result)
                {
                    matchMakingNotifs[index] = result;
                    Debug.Log(string.Format("Notif matchmaking {0} response {1}", index, result.Value));
                    Interlocked.Increment(ref matchMakingNotifNum);
                };

                // Prerequisites: create matchmaking channel named "test"
                lobbies[i]
                    .StartMatchmaking(
                        "test",
                        result =>
                        {
                            Debug.Log(string.Format("Start matchmaking {0} response {1}", index, result.Value));
                            startMatchmakingResponses[index] = result;
                            Interlocked.Increment(ref responseNum);
                        });
            }

            while (responseNum < NumUsers || matchMakingNotifNum < NumUsers)
            {
                yield return new WaitForSeconds(0.2f);
            }

            foreach (var lobby in lobbies)
            {
                lobby.Disconnect();
            }

            foreach (var response in startMatchmakingResponses)
            {
                Assert.False(response.IsError);
                Assert.NotNull(response.Value);
            }

            foreach (var response in matchMakingNotifs)
            {
                Assert.False(response.IsError);
                Assert.NotNull(response.Value);
                Assert.NotNull(response.Value.partyMember);
                Assert.Greater(response.Value.partyMember.Length, 0);
                Assert.NotNull(response.Value.counterPartyMember);
                Assert.Greater(response.Value.counterPartyMember.Length, 0);
                Assert.NotNull(response.Value.matchId);
                Assert.NotNull(response.Value.status);
                Assert.AreEqual("done", response.Value.status);
            }
        }

        [UnityTest, Order(1)]
        public IEnumerator CancelMatchmaking_ReturnOk()
        {
            Lobby lobby = new Lobby(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0],
                this.taskDispatcher,
                this.coroutineRunner);
            Result<MatchmakingCode> startMatchmakingResponse = null;
            Result<MatchmakingNotif> matchMakingNotif = null;

            lobby.Connect();

            while (!lobby.IsConnected) yield return new WaitForSeconds(0.2f);

            Debug.Log("User Connected to lobby");

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            Debug.Log("Start matchmaking");

            // Prerequisites: create matchmaking channel named "test"
            lobby.StartMatchmaking(
                "test",
                result =>
                {
                    Debug.Log(string.Format("Start matchmaking response {0}", result.Value));
                    startMatchmakingResponse = result;
                });

            while (startMatchmakingResponse == null) yield return new WaitForSeconds(0.2f);

            lobby.MatchmakingCompleted += delegate(Result<MatchmakingNotif> result)
            {
                matchMakingNotif = result;
                Debug.Log(string.Format("Notif matchmaking response {0}", result.Value));
            };

            Result<MatchmakingCode> cancelMatchmakingResponse = null;
            lobby.CancelMatchmaking(
                "test",
                result =>
                {
                    Debug.Log(string.Format("Cancel matchmaking response {0}", 0));
                    cancelMatchmakingResponse = result;
                });

            while (matchMakingNotif == null || cancelMatchmakingResponse == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            lobby.Disconnect();

            Assert.False(startMatchmakingResponse.IsError);
            Assert.NotNull(startMatchmakingResponse.Value);

            Assert.False(cancelMatchmakingResponse.IsError);
            Assert.NotNull(cancelMatchmakingResponse.Value);

            Assert.False(matchMakingNotif.IsError);
            Assert.NotNull(matchMakingNotif.Value);
            Assert.AreEqual("cancel", matchMakingNotif.Value.status);
        }
    }
}