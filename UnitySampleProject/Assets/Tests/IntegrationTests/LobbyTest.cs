// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using System.Threading;
using HybridWebSocket;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class LobbyTest
    {
        private TestHelper helper;
        private User[] users;
        private int setupCount;
        private UnityHttpWorker httpWorker;
        private CoroutineRunner coroutineRunner;
        private bool canConstruct;
        private bool canDestroy;
        private UserData[] usersData;

        Lobby CreateLobby(ISession session)
        {
            var webSocket = new WebSocket();

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROXY_SERVER")))
            {
                webSocket.SetProxy("http://" + Environment.GetEnvironmentVariable("PROXY_SERVER"), "", "");
            }

            return new Lobby(AccelBytePlugin.Config.LobbyServerUrl, webSocket, session, this.coroutineRunner);
        }

        [UnityTest, Order(0), Timeout(300000)]
        public IEnumerator TestSetup()
        {
            this.httpWorker = new UnityHttpWorker();
            this.coroutineRunner = new CoroutineRunner();
            this.helper = new TestHelper();

            this.users = new User[5];
            this.usersData = new UserData[5];

            for (int i = 0; i < this.users.Length; i++)
            {
                Result<UserData> registerResult = null;
                ILoginSession loginSession;

                if (AccelBytePlugin.Config.UseSessionManagement)
                {
                    loginSession = new ManagedLoginSession(
                        AccelBytePlugin.Config.LoginServerUrl,
                        AccelBytePlugin.Config.Namespace,
                        AccelBytePlugin.Config.ClientId,
                        AccelBytePlugin.Config.ClientSecret,
                        AccelBytePlugin.Config.RedirectUri,
                        this.httpWorker);
                }
                else
                {
                    loginSession = new OauthLoginSession(
                        AccelBytePlugin.Config.LoginServerUrl,
                        AccelBytePlugin.Config.Namespace,
                        AccelBytePlugin.Config.ClientId,
                        AccelBytePlugin.Config.ClientSecret,
                        AccelBytePlugin.Config.RedirectUri,
                        this.httpWorker,
                        this.coroutineRunner);
                }

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    this.httpWorker);

                this.users[i] = new User(
                    loginSession,
                    userAccount,
                    this.coroutineRunner,
                    AccelBytePlugin.Config.UseSessionManagement);

                this.users[i]
                    .Register(
                        string.Format("lobbyuser{0}+accelbyteunitysdk@example.com", i + 1),
                        "password",
                        "lobbyuser" + (i + 1),
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);

                while (registerResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(registerResult, "Setup: Registered lobbyuser" + (i + 1));
            }

            for (int i = 0; i < this.users.Length; i++)
            {
                Result loginResult = null;

                this.users[i]
                    .LoginWithUsername(
                        string.Format("lobbyuser{0}+accelbyteunitysdk@example.com", i + 1),
                        "password",
                        result => loginResult = result);

                while (loginResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Result<UserData> userResult = null;
                this.users[i].GetData(r => userResult = r);

                while (userResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                this.usersData[i] = userResult.Value;

                TestHelper.LogResult(loginResult, "Setup: Logged in " + userResult.Value.DisplayName);
            }
        }

        [UnityTest, Order(99), Timeout(300000)]
        public IEnumerator TestTeardown()
        {
            for (int i = 0; i < this.users.Length; i++)
            {
                Result deleteResult = null;

                this.helper.DeleteUser(this.users[i], result => deleteResult = result);

                while (deleteResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(deleteResult, "Setup: Deleted lobbyuser" + (i + 1));
                Assert.True(!deleteResult.IsError);
            }
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator SendPrivateChat_FromMultipleUsers_ChatReceived()
        {
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

            for (int i = 0; i < lobbies.Length; i++)
            {
                var userId = this.users[0].Session.UserId;
                var chatMessage = "Hello " + this.usersData[0].DisplayName + " from " + this.usersData[i].DisplayName;

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

        [UnityTest, Order(2), Timeout(100000)]
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

                while (requestFriendResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Result acceptFriendResult = null;

                lobbies[0].AcceptFriend(this.users[i].Session.UserId, result => acceptFriendResult = result);

                while (acceptFriendResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                userStatusResult = null;
                lobbies[i].SetUserStatus(UserStatus.Availabe, "random activity", result => userStatusResult = result);

                while (userStatusResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(userStatusResult, "Set User Status Online");
            }

            Result<FriendsStatus> onlineFriendsResult = null;
            lobbies[0].ListFriendsStatus(result => onlineFriendsResult = result);

            while (onlineFriendsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(onlineFriendsResult, "List Friends Presence");

            for (int i = 1; i < 4; i++)
            {
                userStatusResult = null;
                lobbies[i].SetUserStatus(UserStatus.Offline, "disappearing", result => userStatusResult = result);

                while (userStatusResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(userStatusResult, "Set User Status Offline");

                Result unfriendResult = null;
                lobbies[i].Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);

                while (unfriendResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }
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

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator GetPartyInfo_NoParty_ReturnError()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            //Ensure that user is not in party, doesn't care about its result.
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<PartyInfo> partyInfoResult = null;
            lobby.GetPartyInfo(result => partyInfoResult = result);

            while (partyInfoResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            lobby.Disconnect();

            Assert.That(partyInfoResult.IsError);
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator GetPartyInfo_PartyCreated_ReturnOk()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(createPartyResult, "Create Party");

            Result<PartyInfo> getPartyInfoResult = null;
            lobby.GetPartyInfo(result => getPartyInfoResult = result);

            while (getPartyInfoResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(getPartyInfoResult, "Get Party Info");
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            lobby.Disconnect();

            Assert.False(getPartyInfoResult.IsError);
            Assert.That(getPartyInfoResult.Value.partyID, Is.Not.Null.Or.Empty);
            Assert.That(getPartyInfoResult.Value.invitationToken, Is.Not.Null.Or.Empty);
            Assert.That(getPartyInfoResult.Value.members.Length, Is.GreaterThan(0));
            Assert.That(getPartyInfoResult.Value.members[0], Is.EqualTo(this.users[0].Session.UserId));
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator CreateParty_PartyAlreadyCreated_ReturnError()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            //Ensure there is no party before, doesn't care about its result
            Result leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<PartyInfo> createPartyResult = null;
            lobby.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(createPartyResult, "Create Party");

            Result<PartyInfo> createPartyResult2 = null;
            lobby.CreateParty(result => createPartyResult2 = result);

            while (createPartyResult2 == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(createPartyResult2, "Create Party Again");

            leavePartyResult = null;
            lobby.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            lobby.Disconnect();

            Assert.False(createPartyResult.IsError);
            Assert.True(createPartyResult2.IsError);
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator InviteToParty_InvitationAccepted_CanChat()
        {
            var lobby1 = CreateLobby(this.users[0].Session);
            var lobby2 = CreateLobby(this.users[1].Session);

            lobby1.Connect();
            lobby2.Connect();

            Result leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            leavePartyResult = null;

            lobby2.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            Result<PartyInfo> createPartyResult = null;
            lobby1.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(createPartyResult, "Create Party");
            Result<PartyInvitation> invitedToPartyResult = null;
            lobby2.InvitedToParty += result => invitedToPartyResult = result;
            Result inviteResult = null;
            lobby1.InviteToParty(this.users[1].Session.UserId, result => inviteResult = result);

            while (inviteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(inviteResult, "Invite To Party");

            while (invitedToPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            Result<PartyInfo> joinPartyResult = null;

            lobby2.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);

            while (joinPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(joinPartyResult, "Join Party");

            Result<ChatMesssage> receivedPartyChatResult = null;
            lobby1.PartyChatReceived += result => receivedPartyChatResult = result;

            Result sendPartyChatResult = null;
            lobby2.SendPartyChat("This is a party chat", result => sendPartyChatResult = result);

            while (sendPartyChatResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(sendPartyChatResult, "Send Party Chat");

            while (receivedPartyChatResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(receivedPartyChatResult, "Received Party Chat");

            leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby2.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
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

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator PartyMember_Kicked_CannotChat()
        {
            var lobby1 = CreateLobby(this.users[0].Session);
            var lobby2 = CreateLobby(this.users[1].Session);
            var lobby3 = CreateLobby(this.users[2].Session);

            lobby1.Connect();
            lobby2.Connect();
            lobby3.Connect();

            Debug.Log("Connected to lobby");

            Result leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            leavePartyResult = null;

            lobby2.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");

            leavePartyResult = null;

            lobby3.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");


            //1. lobby1 create party
            Result<PartyInfo> createPartyResult = null;
            lobby1.CreateParty(result => createPartyResult = result);

            while (createPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(createPartyResult, "Create Party");

            //2. lobby2 join party
            Result<PartyInvitation> invitedToPartyResult = null;
            lobby2.InvitedToParty += result => invitedToPartyResult = result;
            Result inviteResult = null;
            lobby1.InviteToParty(this.users[1].Session.UserId, result => inviteResult = result);

            while (inviteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(inviteResult, "Invite To Party");

            while (invitedToPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            Result<PartyInfo> joinPartyResult = null;
            lobby2.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);

            while (joinPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(joinPartyResult, "Join Party");

            invitedToPartyResult = null;
            lobby3.InvitedToParty += result => invitedToPartyResult = result;
            inviteResult = null;
            lobby1.InviteToParty(this.users[2].Session.UserId, result => inviteResult = result);

            while (inviteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(inviteResult, "Invite To Party");

            while (invitedToPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(invitedToPartyResult, "Invited To Party");

            joinPartyResult = null;
            lobby3.JoinParty(
                invitedToPartyResult.Value.partyID,
                invitedToPartyResult.Value.invitationToken,
                result => joinPartyResult = result);

            while (joinPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(joinPartyResult, "Join Party");

            //3. Kick member

            Result kickResult = null;
            Result<KickNotification> kickedFromPartyResult = null;
            lobby3.KickedFromParty += result => kickedFromPartyResult = result;

            lobby1.KickPartyMember(this.users[2].Session.UserId, result => kickResult = result);

            while (kickResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(kickResult, "Kick Member");

            while (kickedFromPartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(kickedFromPartyResult, "Kicked From Party");

            Result<PartyInfo> partyInfoResult = null;
            lobby2.GetPartyInfo(result => partyInfoResult = result);

            while (partyInfoResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //4. Leave party and disconnect
            leavePartyResult = null;

            lobby1.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby2.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(leavePartyResult, "Leave Party");
            leavePartyResult = null;
            lobby3.LeaveParty(result => leavePartyResult = result);

            while (leavePartyResult == null)
            {
                Thread.Sleep(100);

                yield return null;
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

        [UnityTest, Order(1), Timeout(150000)]
        public IEnumerator LobbyConnected_ForMoreThan1Minutes_DoesntDisconnect()
        {
            var lobby = CreateLobby(this.users[0].Session);

            lobby.Connect();

            for (int i = 0; i < 125; i += 5)
            {
                Debug.Log(string.Format("Wait for {0} seconds. Lobby.IsConnected={1}", i, lobby.IsConnected));

                yield return new WaitForSeconds(5f);
            }

            Assert.That(lobby.IsConnected);

            lobby.Disconnect();
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator Lobby_Disconnect_CloseImmediately()
        {
            var lobby = CreateLobby(this.users[0].Session);

            Debug.Log(string.Format("Lobby.IsConnected={0}", lobby.IsConnected));

            lobby.Connect();

            Debug.Log(string.Format("Lobby.IsConnected={0}", lobby.IsConnected));

            lobby.Disconnect();

            Assert.That(!lobby.IsConnected);

            Debug.Log(string.Format("Lobby.IsConnected={0}", lobby.IsConnected));


            yield break;
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator WebsocketSharp_SendPingAsynchronously_CompletedSuccessfully()
        {
            var webSocket = new WebSocketSharp.WebSocket(
                AccelBytePlugin.Config.LobbyServerUrl,
                this.users[0].Session.AuthorizationToken);

            webSocket.Connect();


            bool successful = false;

            var stopwatch = Stopwatch.StartNew();
            webSocket.PingAsync(s => successful = s);
            var pingAsyncElapsed = stopwatch.Elapsed;
            stopwatch.Reset();
            stopwatch.Start();

            yield return new WaitUntil(() => successful || stopwatch.Elapsed > TimeSpan.FromSeconds(5));

            stopwatch.Reset();
            stopwatch.Start();
            var secondSuccessful = false;
            webSocket.PingAsync(s => secondSuccessful = s);

            yield return new WaitUntil(() => secondSuccessful || stopwatch.Elapsed > TimeSpan.FromSeconds(5));

            Assert.That(successful, "PingAsync should be completed with true value");
            Assert.That(secondSuccessful, "PingAsync should be completed with true value");
            Assert.That(pingAsyncElapsed, Is.LessThan(TimeSpan.FromMilliseconds(20)), "PingAsync should be 0 < 50ms");
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator Notification_GetAsyncNotification()
        {
            Result sendNotificationResult = null;
            string notification = "this is a notification";
            this.helper.SendNotification(
                this.users[0].Session.UserId,
                true,
                notification,
                result => { sendNotificationResult = result; });

            while (sendNotificationResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.LogResult(sendNotificationResult, "send notification");

            var lobby = CreateLobby(this.users[0].Session);
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

        [UnityTest, Order(2), Timeout(100000)]
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

        [UnityTest, Order(2), Timeout(100000)]
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

            while (requestFriendResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].Session.UserId, result => acceptFriendResult = result);

            while (acceptFriendResult == null)
            {
                Thread.Sleep(100);

                yield return null;
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
            lobby2.Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);

            while (unfriendResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            lobby1.Disconnect();
            lobby2.Disconnect();

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(listenUser1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(listenUser1StatusResult.IsError);
            Assert.AreEqual(((int) ExpectedUser1Status).ToString(), listenUser1StatusResult.Value.availability);
        }

        [UnityTest, Order(2), Timeout(100000)]
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

            while (requestFriendResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result acceptFriendResult = null;

            lobby2.AcceptFriend(this.users[0].Session.UserId, result => acceptFriendResult = result);

            while (acceptFriendResult == null)
            {
                Thread.Sleep(100);

                yield return null;
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

            while (changeUser1StatusResult == null) yield return new WaitForSeconds(.2f);

            while (listenUser1StatusResult == null) yield return new WaitForSeconds(.1f);

            TestHelper.LogResult(listenUser1StatusResult);

            Result unfriendResult = null;
            lobby2.Unfriend(this.users[0].Session.UserId, result => unfriendResult = result);

            while (unfriendResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            lobby1.Disconnect();
            lobby2.Disconnect();

            Assert.IsNotNull(setUser1StatusResult);
            Assert.IsNotNull(listenUser1StatusResult);
            Assert.IsFalse(setUser1StatusResult.IsError);
            Assert.IsFalse(listenUser1StatusResult.IsError);
            Assert.AreEqual((int) ExpectedUser1Status, int.Parse(listenUser1StatusResult.Value.availability));
        }

        [UnityTest, Order(2), Timeout(100000)]
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

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendResult.IsError);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listOutgoingFriendRequestResult.IsError);
            TestHelper.Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendRequestResult.IsError);
            TestHelper.Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!acceptFriendRequestResult.IsError);

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            while (loadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListResult.IsError);
            TestHelper.Assert.That(loadFriendListResult.Value.friendsId.Contains(idB));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            while (anotherLoadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!anotherLoadFriendListResult.IsError);
            TestHelper.Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!unfriendResult.IsError);

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(100000)]
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

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendResult.IsError);

            while (incomingNotificationFromAResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!incomingNotificationFromAResult.IsError);
            TestHelper.Assert.That(incomingNotificationFromAResult.Value.friendId == idA);

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!acceptFriendRequestResult.IsError);

            while (friendRequestAcceptedResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!friendRequestAcceptedResult.IsError);
            TestHelper.Assert.That(friendRequestAcceptedResult.Value.friendId == idB);

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            while (loadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListResult.IsError);
            TestHelper.Assert.That(loadFriendListResult.Value.friendsId.Contains(idB));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            while (anotherLoadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!anotherLoadFriendListResult.IsError);
            TestHelper.Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!unfriendResult.IsError);

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(100000)]
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

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendResult.IsError);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listOutgoingFriendRequestResult.IsError);
            TestHelper.Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendRequestResult.IsError);
            TestHelper.Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!acceptFriendRequestResult.IsError);

            Result<Friends> loadFriendListResult = null;
            lobbyA.LoadFriendsList(result => { loadFriendListResult = result; });

            while (loadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListResult.IsError);
            TestHelper.Assert.That(loadFriendListResult.Value.friendsId.Contains(idB));

            Result<Friends> anotherLoadFriendListResult = null;
            lobbyB.LoadFriendsList(result => { anotherLoadFriendListResult = result; });

            while (anotherLoadFriendListResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!anotherLoadFriendListResult.IsError);
            TestHelper.Assert.That(anotherLoadFriendListResult.Value.friendsId.Contains(idA));

            Result unfriendResult = null;
            lobbyA.Unfriend(idB, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!unfriendResult.IsError);

            Result<Friends> loadFriendListAfterUnfriend = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterUnfriend = result; });

            while (loadFriendListAfterUnfriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListAfterUnfriend.IsError);

            if (loadFriendListAfterUnfriend.Value.friendsId.Length != 0)
            {
                TestHelper.Assert.That(!loadFriendListAfterUnfriend.Value.friendsId.Contains(idB));
            }

            Result<Friends> loadFriendListAfterGotUnfriend = null;
            lobbyB.LoadFriendsList(result => { loadFriendListAfterGotUnfriend = result; });

            while (loadFriendListAfterGotUnfriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListAfterGotUnfriend.IsError);

            if (loadFriendListAfterGotUnfriend.Value.friendsId.Length != 0)
            {
                TestHelper.Assert.That(!loadFriendListAfterGotUnfriend.Value.friendsId.Contains(idA));
            }

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(100000)]
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

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendResult.IsError);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listOutgoingFriendRequestResult.IsError);
            TestHelper.Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendRequestResult.IsError);
            TestHelper.Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result rejectFriendRequestResult = null;
            lobbyB.RejectFriend(idA, result => { rejectFriendRequestResult = result; });

            while (rejectFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!rejectFriendRequestResult.IsError);

            Result<FriendshipStatus> getFriendshipStatusAfterRejecting = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRejecting = result; });

            while (getFriendshipStatusAfterRejecting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRejecting.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRejecting.Value.friendshipStatus == RelationshipStatusCode.NotFriend);

            Result<Friends> listIncomingFriendsAfterRejecting = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendsAfterRejecting = result; });

            while (listIncomingFriendsAfterRejecting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendsAfterRejecting.IsError);
            TestHelper.Assert.That(!listIncomingFriendsAfterRejecting.Value.friendsId.Contains(idA));

            Result<FriendshipStatus> getFriendshipStatusAfterRejected = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRejected = result; });

            while (getFriendshipStatusAfterRejected == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRejected.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRejected.Value.friendshipStatus == RelationshipStatusCode.NotFriend);

            Result<Friends> listOutgoingFriendsAfterRejected = null;
            lobbyA.ListIncomingFriends(result => { listOutgoingFriendsAfterRejected = result; });

            while (listOutgoingFriendsAfterRejected == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listOutgoingFriendsAfterRejected.IsError);
            TestHelper.Assert.That(!listOutgoingFriendsAfterRejected.Value.friendsId.Contains(idB));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(100000)]
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

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(idB, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendResult.IsError);

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(idB, result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listOutgoingFriendRequestResult.IsError);
            TestHelper.Assert.That(listOutgoingFriendRequestResult.Value.friendsId.Contains(idB));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendRequestResult.IsError);
            TestHelper.Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result cancelFriendRequestResult = null;
            lobbyA.CancelFriendRequest(idB, result => { cancelFriendRequestResult = result; });

            while (cancelFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!cancelFriendRequestResult.IsError);

            Result<Friends> listIncomingFriendRequestAfterCanceled = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestAfterCanceled = result; });

            while (listIncomingFriendRequestAfterCanceled == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendRequestAfterCanceled.IsError);
            TestHelper.Assert.That(!listIncomingFriendRequestAfterCanceled.Value.friendsId.Contains(idA));

            Result<Friends> loadFriendListAfterCanceled = null;
            lobbyB.LoadFriendsList(result => { loadFriendListAfterCanceled = result; });

            while (loadFriendListAfterCanceled == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListAfterCanceled.IsError);

            if (loadFriendListAfterCanceled.Value.friendsId.Length != 0)
            {
                TestHelper.Assert.That(!loadFriendListAfterCanceled.Value.friendsId.Contains(idA));
            }

            Result<Friends> loadFriendListAfterCanceling = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterCanceling = result; });

            while (loadFriendListAfterCanceling == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListAfterCanceling.IsError);

            if (loadFriendListAfterCanceling.Value.friendsId.Length != 0)
            {
                TestHelper.Assert.That(!loadFriendListAfterCanceling.Value.friendsId.Contains(idB));
            }

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(100000)]
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

            while (getFriendshipStatusBeforeRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusBeforeRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusBeforeRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            Result requestFriendResult = null;
            lobbyA.RequestFriend(this.users[1].Session.UserId, result => { requestFriendResult = result; });

            while (requestFriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendResult.IsError);

            Result<Friends> listOutgoingFriendRequestResult = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendRequestResult = result; });

            while (listOutgoingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listOutgoingFriendRequestResult.IsError);
            TestHelper.Assert.That(
                listOutgoingFriendRequestResult.Value.friendsId.Contains(this.users[1].Session.UserId));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestFriend = null;
            lobbyA.GetFriendshipStatus(
                this.users[1].Session.UserId,
                result => { getFriendshipStatusAfterRequestFriend = result; });

            while (getFriendshipStatusAfterRequestFriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestFriend.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestFriend.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Outgoing));

            Result cancelFriendRequestResult = null;
            lobbyA.CancelFriendRequest(this.users[1].Session.UserId, result => { cancelFriendRequestResult = result; });

            while (cancelFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!cancelFriendRequestResult.IsError);

            Result<Friends> listOutgoingFriendAfterCanceling = null;
            lobbyA.ListOutgoingFriends(result => { listOutgoingFriendAfterCanceling = result; });

            while (listOutgoingFriendAfterCanceling == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listOutgoingFriendAfterCanceling.IsError);

            if (listOutgoingFriendAfterCanceling.Value.friendsId.Length != 0)
            {
                TestHelper.Assert.That(
                    !listOutgoingFriendAfterCanceling.Value.friendsId.Contains(this.users[1].Session.UserId));
            }

            Result requestFriendAfterCanceling = null;
            lobbyA.RequestFriend(this.users[1].Session.UserId, result => { requestFriendAfterCanceling = result; });

            while (requestFriendAfterCanceling == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendAfterCanceling.IsError);

            Result<Friends> listIncomingFriendRequestResult = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestResult = result; });

            while (listIncomingFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendRequestResult.IsError);
            TestHelper.Assert.That(listIncomingFriendRequestResult.Value.friendsId.Contains(idA));

            Result<FriendshipStatus> getFriendshipStatusAfterRequestSentFromAnother = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterRequestSentFromAnother = result; });

            while (getFriendshipStatusAfterRequestSentFromAnother == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterRequestSentFromAnother.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterRequestSentFromAnother.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Incoming));

            Result rejectFriendRequestResult = null;
            lobbyB.RejectFriend(idA, result => { rejectFriendRequestResult = result; });

            while (rejectFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!rejectFriendRequestResult.IsError);

            Result requestFriendAfterRejected = null;
            lobbyA.RequestFriend(this.users[1].Session.UserId, result => { requestFriendAfterRejected = result; });

            while (requestFriendAfterRejected == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!requestFriendAfterRejected.IsError);

            while (incomingNotificationFromAResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!incomingNotificationFromAResult.IsError);
            TestHelper.Assert.That(incomingNotificationFromAResult.Value.friendId == idA);

            Result<Friends> listIncomingFriendRequestAfterRejecting = null;
            lobbyB.ListIncomingFriends(result => { listIncomingFriendRequestAfterRejecting = result; });

            while (listIncomingFriendRequestAfterRejecting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!listIncomingFriendRequestAfterRejecting.IsError);
            TestHelper.Assert.That(listIncomingFriendRequestAfterRejecting.Value.friendsId.Contains(idA));

            Result acceptFriendRequestResult = null;
            lobbyB.AcceptFriend(idA, result => { acceptFriendRequestResult = result; });

            while (acceptFriendRequestResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!acceptFriendRequestResult.IsError);

            while (friendRequestAcceptedResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!friendRequestAcceptedResult.IsError);
            TestHelper.Assert.That(friendRequestAcceptedResult.Value.friendId == idB);

            Result<Friends> loadFriendListAfterAccepted = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterAccepted = result; });

            while (loadFriendListAfterAccepted == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListAfterAccepted.IsError);
            TestHelper.Assert.That(loadFriendListAfterAccepted.Value.friendsId.Contains(this.users[1].Session.UserId));

            Result<FriendshipStatus> getFriendshipStatusAfterAccepted = null;
            lobbyA.GetFriendshipStatus(
                this.users[1].Session.UserId,
                result => { getFriendshipStatusAfterAccepted = result; });

            while (getFriendshipStatusAfterAccepted == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterAccepted.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterAccepted.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Friend));

            Result<FriendshipStatus> getFriendshipStatusAfterAccepting = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterAccepting = result; });

            while (getFriendshipStatusAfterAccepting == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterAccepting.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterAccepting.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.Friend));

            Result unfriendResult = null;
            lobbyA.Unfriend(this.users[1].Session.UserId, result => { unfriendResult = result; });

            while (unfriendResult == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!unfriendResult.IsError);

            Result<Friends> loadFriendListAfterUnfriend = null;
            lobbyA.LoadFriendsList(result => { loadFriendListAfterUnfriend = result; });

            while (loadFriendListAfterUnfriend == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!loadFriendListAfterUnfriend.IsError);

            if (loadFriendListAfterUnfriend.Value.friendsId.Length != 0)
            {
                TestHelper.Assert.That(
                    !loadFriendListAfterUnfriend.Value.friendsId.Contains(this.users[1].Session.UserId));
            }

            Result<FriendshipStatus> getFriendshipStatusAfterUnfriended = null;
            lobbyB.GetFriendshipStatus(idA, result => { getFriendshipStatusAfterUnfriended = result; });

            while (getFriendshipStatusAfterUnfriended == null) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.That(!getFriendshipStatusAfterUnfriended.IsError);
            TestHelper.Assert.That(
                getFriendshipStatusAfterUnfriended.Value.friendshipStatus,
                Is.EqualTo(RelationshipStatusCode.NotFriend));

            lobbyA.Disconnect();
            lobbyB.Disconnect();
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator StartMatchmaking_ReturnOk()
        {
            const int NumUsers = 2;
            Lobby[] lobbies = new Lobby[NumUsers];
            Result<MatchmakingCode>[] startMatchmakingResponses = new Result<MatchmakingCode>[NumUsers];
            Result<MatchmakingNotif>[] matchMakingNotifs = new Result<MatchmakingNotif>[NumUsers];

            Result<TokenData> accessTokenResult = null;

            this.helper.GetAccessToken(r => accessTokenResult = r);

            while (accessTokenResult == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            string clientAccessToken = accessTokenResult.Value.access_token;
            string channelName = "unitysdktest" + Guid.NewGuid();

            Result createChannelResult = null;

            this.helper.CreateMatchmakingChannel(clientAccessToken, channelName, r => createChannelResult = r);

            while (createChannelResult == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            TestHelper.LogResult(createChannelResult, "Create Matchmaking Channel");

            for (int i = 0; i < NumUsers; i++)
            {
                var lobby = lobbies[i] = CreateLobby(this.users[i].Session);

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
            int readyConsentNotifNum = 0;
            //int dsNotifNum = 0;

            for (int i = 0; i < NumUsers; i++)
            {
                int index = i;
                Debug.Log(string.Format("Start matchmaking {0}", index));

                lobbies[index].MatchmakingCompleted += delegate(Result<MatchmakingNotif> result)
                {
                    matchMakingNotifs[index] = result;
                    Debug.Log(string.Format("Notif matchmaking {0} response {1}", index, result.Value.status));
                    Interlocked.Increment(ref matchMakingNotifNum);
                };

                lobbies[index].ReadyForMatchConfirmed += result =>
                {
                    Debug.Log(
                        string.Format(
                            "User {0} received: User {1} confirmed ready for match.",
                            this.users[index].Session.UserId,
                            result.Value.userId));

                    readyConsentNotifNum++;
                };

                //lobbies[index].DSUpdated += result =>
                //{
                //    Debug.Log(
                //        string.Format(
                //            "DS Notif {0} for user {1}: {2}",
                //            index,
                //            this.users[index],
                //            result.Value.matchId));

                //    dsNotifNum++;
                //};

                lobbies[index]
                    .StartMatchmaking(
                        channelName,
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

            responseNum = 0;

            for (int i = 0; i < NumUsers; i++)
            {
                int index = i;

                string matchId = matchMakingNotifs[index].Value.matchId;
                lobbies[i]
                    .ConfirmReadyForMatch(
                        matchId,
                        result =>
                        {
                            Debug.Log(string.Format("Ready Consent {0}", index));
                            Interlocked.Increment(ref responseNum);
                        });
            }

            while (responseNum < NumUsers)
            {
                yield return new WaitForSeconds(0.2f);
            }

            while (readyConsentNotifNum < NumUsers * NumUsers)
            {
                yield return new WaitForSeconds(0.2f);
            }

            //while (dsNotifNum < NumUsers)
            //{
            //    yield return new WaitForSeconds(0.2f);
            //}

            foreach (var lobby in lobbies)
            {
                Result<MatchmakingCode> result = null;
                lobby.CancelMatchmaking(channelName, r => result = r);

                while (result == null)
                {
                    yield return new WaitForSeconds(0.2f);
                }

                lobby.Disconnect();
            }

            Result deleteChannelResult = null;

            this.helper.DeleteMatchmakingChannel(clientAccessToken, channelName, r => deleteChannelResult = r);

            while (deleteChannelResult == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            TestHelper.LogResult(deleteChannelResult, "Delete matchmaking channel");

            foreach (var response in startMatchmakingResponses)
            {
                TestHelper.Assert.That(response.IsError, Is.False);
                TestHelper.Assert.That(response.Value, Is.Not.Null);
            }

            foreach (var response in matchMakingNotifs)
            {
                TestHelper.Assert.That(response.IsError, Is.False);
                TestHelper.Assert.That(response.Value, Is.Not.Null);
                TestHelper.Assert.That(response.Value.matchId, Is.Not.Null);
                TestHelper.Assert.That(response.Value.status, Is.Not.Null);
                TestHelper.Assert.That("done", Is.EqualTo(response.Value.status));
            }
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator Rematchmaking_ReturnOk()
        {
            const int NumUsers = 3;
            Lobby[] lobbies = new Lobby[NumUsers];
            Result<MatchmakingCode>[] startMatchmakingResponses = new Result<MatchmakingCode>[NumUsers];
            Result<MatchmakingNotif>[] matchMakingNotifs = new Result<MatchmakingNotif>[NumUsers];
            int responseNum = 0;
            int matchMakingNotifNum = 0;
            int readyConsentNotifNum = 0;
            int rematchmakingNotifNum = 0;
            //int dsNotifNum = 0;

            for (int i = 0; i < NumUsers; i++)
            {
                var lobby = lobbies[i] = CreateLobby(this.users[i].Session);
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

                int index = i;
                lobbies[i].MatchmakingCompleted += delegate(Result<MatchmakingNotif> result)
                {
                    matchMakingNotifs[index] = result;
                    Debug.Log(
                        string.Format(
                            "User {0}: Matchmaking completed matchId {1}, status {2}",
                            index,
                            result.Value.matchId,
                            result.Value.status));

                    Interlocked.Increment(ref matchMakingNotifNum);
                };

                lobbies[i].ReadyForMatchConfirmed += result =>
                {
                    Debug.Log(
                        string.Format(
                            "User {0} received: User {1} confirmed ready for match id {2}.",
                            this.users[index].Session.UserId,
                            result.Value.userId,
                            result.Value.matchId));

                    readyConsentNotifNum++;
                };

                lobbies[i].RematchmakingNotif += delegate(Result<RematchmakingNotification> result)
                {
                    Debug.Log(string.Format("User {0} received: banned for {1} secs", index, result.Value.banDuration));

                    Interlocked.Increment(ref rematchmakingNotifNum);
                };

                //lobbies[index].DSUpdated += result =>
                //{
                //    Debug.Log(
                //        string.Format(
                //            "DS Notif {0} for user {1}: {2}",
                //            index,
                //            this.users[index],
                //            result.Value.matchId));

                //    dsNotifNum++;
                //};
            }

            Result<TokenData> accessTokenResult = null;

            this.helper.GetAccessToken(r => accessTokenResult = r);

            while (accessTokenResult == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            string clientAccessToken = accessTokenResult.Value.access_token;
            string channelName = "unitysdktest" + Guid.NewGuid();

            Result createChannelResult = null;

            this.helper.CreateMatchmakingChannel(clientAccessToken, channelName, r => createChannelResult = r);

            while (createChannelResult == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            TestHelper.LogResult(createChannelResult, "Create Matchmaking Channel");

            for (int i = 0; i < (NumUsers - 1); i++)
            {
                int index = i;
                Debug.Log(string.Format("Start matchmaking {0}", index));

                lobbies[index]
                    .StartMatchmaking(
                        channelName,
                        result =>
                        {
                            Debug.Log(string.Format("Start matchmaking {0} response {1}", index, result.Value.code));
                            startMatchmakingResponses[index] = result;
                            Interlocked.Increment(ref responseNum);
                        });
            }

            while (matchMakingNotifNum < NumUsers - 1)
            {
                yield return new WaitForSeconds(0.2f);
            }

            responseNum = 0;

            string matchId = matchMakingNotifs[0].Value.matchId;
            lobbies[0]
                .ConfirmReadyForMatch(
                    matchId,
                    result =>
                    {
                        Debug.Log(string.Format("Ready Consent {0}", 0));
                        Interlocked.Increment(ref responseNum);
                    });

            var stopwatch = Stopwatch.StartNew();

            while (responseNum == 0 && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                yield return new WaitForSeconds(0.2f);
            }

            Assert.That(readyConsentNotifNum, Is.EqualTo(2));
            stopwatch.Reset();
            stopwatch.Start();

            while (rematchmakingNotifNum == 0 && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
            {
                yield return new WaitForSeconds(0.2f);
            }

            for (int i = 0; i < NumUsers; i++)
            {
                matchMakingNotifs[i] = null;
            }

            matchMakingNotifNum = 0;

            const int lastUser = NumUsers - 1;
            Debug.Log(string.Format("Start matchmaking {0}", lastUser));

            lobbies[lastUser]
                .StartMatchmaking(
                    channelName,
                    result =>
                    {
                        Debug.Log(string.Format("Start matchmaking {0} response {1}", lastUser, result.Value.code));
                        startMatchmakingResponses[lastUser] = result;
                        Interlocked.Increment(ref responseNum);
                    });

            while (matchMakingNotifNum < 2)
            {
                yield return new WaitForSeconds(0.2f);
            }

            Assert.IsTrue(matchMakingNotifs[0] != null);
            Assert.IsTrue(matchMakingNotifs[lastUser] != null);
            Assert.False(matchMakingNotifs[1] != null);

            readyConsentNotifNum = 0;
            responseNum = 0;

            for (int i = 0; i < NumUsers; i++)
            {
                if (matchMakingNotifs[i] != null)
                {
                    int index = i;

                    lobbies[i]
                        .ConfirmReadyForMatch(
                            matchMakingNotifs[i].Value.matchId,
                            result =>
                            {
                                Debug.Log(string.Format("Ready Consent {0}", index));
                                Interlocked.Increment(ref responseNum);
                            });
                }
            }

            while (responseNum < NumUsers - 1)
            {
                yield return new WaitForSeconds(0.2f);
            }

            Assert.IsTrue(readyConsentNotifNum == (NumUsers - 1) * (NumUsers - 1));

            //while (dsNotifNum < NumUsers - 1)
            //{
            //    yield return new WaitForSeconds(0.2f);
            //}

            //Assert.IsTrue(dsNotifNum == 2);

            foreach (var lobby in lobbies)
            {
                Result<MatchmakingCode> result = null;
                lobby.CancelMatchmaking(channelName, r => result = r);

                while (result == null)
                {
                    yield return new WaitForSeconds(0.2f);
                }

                lobby.Disconnect();
            }

            Result deleteChannelResult = null;

            this.helper.DeleteMatchmakingChannel(clientAccessToken, channelName, r => deleteChannelResult = r);

            while (deleteChannelResult == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            TestHelper.LogResult(deleteChannelResult, "Delete matchmaking channel");

            foreach (var response in startMatchmakingResponses)
            {
                Assert.False(response.IsError);
                Assert.NotNull(response.Value);
            }

            Assert.False(matchMakingNotifs[0].IsError);
            Assert.NotNull(matchMakingNotifs[0].Value);
            Assert.NotNull(matchMakingNotifs[0].Value.matchId);
            Assert.NotNull(matchMakingNotifs[0].Value.status);
            Assert.AreEqual("done", matchMakingNotifs[0].Value.status);
            Assert.False(matchMakingNotifs[lastUser].IsError);
            Assert.NotNull(matchMakingNotifs[lastUser].Value);
            Assert.NotNull(matchMakingNotifs[lastUser].Value.matchId);
            Assert.NotNull(matchMakingNotifs[lastUser].Value.status);
            Assert.AreEqual("done", matchMakingNotifs[lastUser].Value.status);
        }

        [UnityTest, Order(2), Timeout(100000)]
        public IEnumerator CancelMatchmaking_ReturnOk()
        {
            Lobby lobby = CreateLobby(this.users[0].Session);
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

            // Prerequisites: create matchmaking channel named "unitysdktest"
            lobby.StartMatchmaking(
                "unitysdktest",
                result =>
                {
                    Debug.Log(string.Format("Start matchmaking response code {0}", result.Value.code));
                    startMatchmakingResponse = result;
                });

            while (startMatchmakingResponse == null) yield return new WaitForSeconds(0.2f);

            lobby.MatchmakingCompleted += delegate(Result<MatchmakingNotif> result)
            {
                matchMakingNotif = result;
                Debug.Log(
                    string.Format(
                        "Notif matchmaking response status {0}, matchId {1}",
                        result.Value.status,
                        result.Value.matchId));
            };

            Result<MatchmakingCode> cancelMatchmakingResponse = null;
            lobby.CancelMatchmaking(
                "unitysdktest",
                result =>
                {
                    Debug.Log(string.Format("Cancel matchmaking response {0}", 0));
                    cancelMatchmakingResponse = result;
                });

            while (matchMakingNotif == null && cancelMatchmakingResponse == null)
            {
                yield return new WaitForSeconds(0.2f);
            }

            lobby.Disconnect();

            TestHelper.Assert.IsFalse(startMatchmakingResponse.IsError);
            TestHelper.Assert.That(startMatchmakingResponse.Value, Is.Not.Null);

            TestHelper.Assert.IsFalse(cancelMatchmakingResponse.IsError);
            TestHelper.Assert.That(cancelMatchmakingResponse.Value, Is.Not.Null);

            //TestHelper.Assert.IsFalse(matchMakingNotif.IsError);
            //TestHelper.Assert.That(matchMakingNotif.Value, Is.Not.Null);
            //TestHelper.Assert.That(matchMakingNotif.Value.status, Is.EqualTo("cancel"));
        }
    }
}