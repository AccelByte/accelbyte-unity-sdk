// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    /// <summary>
    /// Endpoint involved:
    /// * Lobby user ban notification test
    /// * User ban test
    /// </summary>
    [TestFixture]
    public class LiveModerationTest
    {
        private TestHelper helper;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private User userAdmin;
        private User userBanned;
        private List<Dictionary<string, string>> usersLoginData;

        const int NUMBEROFUSER = 2;
        private string adminRoleId = "";

        Result<UserBannedNotification> banNotif = null;
        Result<UserBannedNotification> unbanNotif = null;
        Result<DisconnectNotif> disconnectNotif = null;
        void BanNotifHandler(Result<UserBannedNotification> result)
        {
            banNotif = result;
        }
        void UnbanNotifHandler(Result<UserBannedNotification> result)
        {
            unbanNotif = result;
        }
        void DisconnectNotifHandler(Result<DisconnectNotif> result)
        {
            disconnectNotif = result;
        }

        private Lobby CreateLobby(User user)
        {
            return LobbyTestUtil.CreateLobby(user.Session, this.httpClient, coroutineRunner);
        }

        private User CreateUser()
        {
            LoginSession loginSession = new LoginSession(
                           AccelBytePlugin.Config.IamServerUrl,
                           AccelBytePlugin.Config.Namespace,
                           AccelBytePlugin.Config.RedirectUri,
                           this.httpClient,
                           this.coroutineRunner);

            UserAccount userAccount = new UserAccount(
                AccelBytePlugin.Config.IamServerUrl,
                AccelBytePlugin.Config.Namespace,
                loginSession,
                this.httpClient);

            User newUser = new User(
                    loginSession,
                    userAccount,
                    this.coroutineRunner);

            return newUser;
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

            if (this.userAdmin != null) yield break;
            if (this.userBanned != null) yield break;

            Result<TestHelper.ListRoleV4Response> getAdminRole = null;
            helper.GetRoles(false, true, 10, "", "", result => getAdminRole = result);
            yield return TestHelper.WaitForValue(() => getAdminRole);
            TestHelper.LogResult(getAdminRole, "Get Admin Role");

            // find admin role that have ban permission
            for (int i = 0; i < getAdminRole.Value.data.Length; i++)
            {
                var data = getAdminRole.Value.data[i];
                for (int j = 0; j < data.permissions.Length; j++)
                {
                    if (data.permissions[j].resource.Contains("BAN") && data.permissions[j].action == 15)
                    {
                        adminRoleId = data.roleId;
                        break;
                    }
                }
                if (adminRoleId != "") break;
            }
            TestHelper.Assert.IsTrue(adminRoleId != "", "Can't get Admin Role with ban Permission");

            var newUsers = new User[NUMBEROFUSER];
            var guid = Guid.NewGuid().ToString("N");
            usersLoginData = new List<Dictionary<string, string>>();

            for (int i = 0; i < newUsers.Length; i++)
            {
                Result<RegisterUserResponse> registerResult = null;

                newUsers[i] = CreateUser();

                Dictionary<string, string> userLoginData = new Dictionary<string, string>();
                userLoginData.Add("email", string.Format("bantestunity+{0}{1}@example.com", i + 1, guid));
                userLoginData.Add("password", "Password123");
                userLoginData.Add("username", "userban" + (i + 1) + guid);
                usersLoginData.Add(userLoginData);

                newUsers[i]
                    .Register(
                        userLoginData["email"],
                        userLoginData["password"],
                        userLoginData["username"],
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered user" + (i + 1));

                // add the game admin role to user 0
                if (i == 0)
                {
                    List<string> assignedNamespace = new List<string>();
                    assignedNamespace.Add(AccelBytePlugin.Config.Namespace);

                    Result<TestHelper.AssignUserV4Response> addGameAdminRole = null;
                    helper.AddUserRole(registerResult.Value.userId, adminRoleId, assignedNamespace.ToArray(), result => addGameAdminRole = result);
                    yield return TestHelper.WaitForValue(() => addGameAdminRole);
                    TestHelper.LogResult(addGameAdminRole, "Add Game Admin Role to user");
                }
            }

            for (int i = 0; i < newUsers.Length; i++)
            {
                Result loginResult = null;
                newUsers[i]
                    .LoginWithUsername(
                        usersLoginData[i]["email"],
                        usersLoginData[i]["password"],
                        result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                Result<UserData> userResult = null;
                newUsers[i].GetData(r => userResult = r);
                yield return TestHelper.WaitForValue(() => userResult);

                TestHelper.LogResult(loginResult, "Setup: Logged in " + userResult.Value.displayName);

                usersLoginData[i].Add("userId", userResult.Value.userId);
            }

            yield return new WaitForSeconds(0.1f);

            this.userAdmin = newUsers[0];
            this.userBanned = newUsers[1];
        }

        [UnityTearDown]
        public IEnumerator TestTeardown()
        {
            for (int i = 0; i < NUMBEROFUSER; i++)
            {
                Result deleteResult = null;

                this.helper.DeleteUser(usersLoginData[i]["userId"], result => deleteResult = result);
                yield return TestHelper.WaitForValue(() => deleteResult);

                TestHelper.LogResult(deleteResult, "Setup: Deleted user : " + usersLoginData[i]["username"]);
                TestHelper.Assert.IsResultOk(deleteResult);

                usersLoginData[i].Clear();
            }

            this.usersLoginData.Clear();
            this.userAdmin = null;
            this.userBanned = null;
        }

        [UnityTest, TestLog, Timeout(30000), Order(1)]
        public IEnumerator BanUser_Success()
        {

            DateTime endDate = DateTime.UtcNow;
            endDate = endDate.AddDays(1);

            Result<UserBanResponseV3> banResponse = null;
            userAdmin.BanUser(userBanned.Session.UserId, BanType.CHAT_ALL, BanReason.MALICIOUS_CONTENT, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse);

            TestHelper.Assert.IsFalse(banResponse.IsError);
            TestHelper.Assert.IsTrue(banResponse.Value.reason == BanReason.MALICIOUS_CONTENT);
        }

        [UnityTest, TestLog, Timeout(30000), Order(2)]
        public IEnumerator BanUser_GetBanListSuccess()
        {
            //Arrange
            DateTime endDate = DateTime.UtcNow.AddDays(1);

            Result<UserBanResponseV3> banResponse = null;
            userAdmin.BanUser(userBanned.Session.UserId, BanType.CHAT_ALL, BanReason.MALICIOUS_CONTENT, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse);

            //Act
            Result<UserBanPagedList> banListResult = null;
            userAdmin.GetUserBannedList(BanType.CHAT_ALL, 0, 20, result => banListResult = result);
            yield return TestHelper.WaitForValue(() => banListResult);

            bool isBanned = false;
            foreach(var data in banListResult.Value.data)
            {
                if (data.userId == userBanned.Session.UserId)
                {
                    isBanned = true;
                }
            }

            //Assert
            TestHelper.Assert.IsFalse(banResponse.IsError);
            TestHelper.Assert.IsFalse(banListResult.IsError);
            TestHelper.Assert.IsTrue(isBanned);
        }

        [UnityTest, TestLog, Timeout(30000), Order(3)]
        public IEnumerator UnbanUser_Success()
        {
            //Arrange
            DateTime endDate = DateTime.UtcNow.AddDays(1);

            Result<UserBanResponseV3> banResponse = null;
            userAdmin.BanUser(userBanned.Session.UserId, BanType.CHAT_ALL, BanReason.MALICIOUS_CONTENT, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse);

            string banId = banResponse.Value.banId;

            //Act
            Result<UserBanResponseV3> unbanResponse = null;
            userAdmin.ChangeUserBanStatus(userBanned.Session.UserId, banId, false, result => unbanResponse = result);
            yield return TestHelper.WaitForValue(() => unbanResponse);

            //Assert
            TestHelper.Assert.IsFalse(banResponse.IsError);
            TestHelper.Assert.IsTrue(banResponse.Value.enabled);
            TestHelper.Assert.IsFalse(unbanResponse.IsError);
            TestHelper.Assert.IsFalse(unbanResponse.Value.enabled);
        }

        [UnityTest, TestLog, Timeout(30000), Order(4)]
        public IEnumerator BanUser_GetNotifSuccess()
        {
            //Arrange
            var user = userAdmin;
            var lobby = CreateLobby(userBanned);
            var userId = userBanned.Session.UserId;

            if (!lobby.IsConnected) lobby.Connect();
            while (!lobby.IsConnected) yield return new WaitForSeconds(.2f);

            banNotif = null;
            lobby.UserBannedNotification += BanNotifHandler;

            DateTime endDate = DateTime.UtcNow;
            endDate = endDate.AddDays(1);

            Result<UserBanResponseV3> banResponse = null;
            user.BanUser(userBanned.Session.UserId, BanType.CHAT_ALL, BanReason.MALICIOUS_CONTENT, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse);

            yield return TestHelper.WaitForValue(() => banNotif);

            lobby.Disconnect();

            Assert.IsFalse(banResponse.IsError);
            TestHelper.Assert.IsResultOk(banNotif);
            Assert.That(banNotif.Value.userId == userId);
            TestHelper.Assert.IsTrue(banNotif.Value.reason == BanReason.MALICIOUS_CONTENT);
        }

        [UnityTest, TestLog, Timeout(400000), Order(5)]
        public IEnumerator BanUser_TokenRefreshed()
        {
            User userBannedRefresh = userBanned;
            Dictionary<string, string> userBannedLoginData = usersLoginData[1];

            DateTime endDate = DateTime.UtcNow;
            endDate = endDate.AddDays(1);

            string oldAccessToken = userBannedRefresh.Session.AuthorizationToken;
            Result<UserBanResponseV3> banResponse = null;
            userAdmin.BanUser(userBannedLoginData["userId"], BanType.MATCHMAKING, BanReason.IMPERSONATION, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse, "wait for ban response", 120000);

            TestHelper.Assert.IsFalse(banResponse.IsError);
            TestHelper.Assert.IsTrue(banResponse.Value.reason == BanReason.IMPERSONATION);
            yield return new WaitForSeconds(2f);

            int RandomRequestsCount = 5;
            int SearchUserSuccessfulCount = 0;
            for (int i = 0; i < RandomRequestsCount; i++)
            {
                userBannedRefresh.SearchUsers("randomname", result => { if (!result.IsError) SearchUserSuccessfulCount++; });
            }
            yield return TestHelper.WaitForValue(() => oldAccessToken != userBannedRefresh.Session.AuthorizationToken);
            while (SearchUserSuccessfulCount != RandomRequestsCount) yield return new WaitForSeconds(.2f);

            string oldAccessTokenUnban = userBannedRefresh.Session.AuthorizationToken;
            Result<UserBanResponseV3> unbanResponse = null;
            userAdmin.ChangeUserBanStatus(userBannedLoginData["userId"], banResponse.Value.banId, false, result => { unbanResponse = result; });
            yield return TestHelper.WaitForValue(() => unbanResponse, "wait for unban response", 120000);
            TestHelper.Assert.IsFalse(unbanResponse.IsError, "unban user ");

            yield return new WaitForSeconds(2f);
            int SearchUserSuccessfulUnbanCount = 0;
            for (int i = 0; i < RandomRequestsCount; i++)
            {
                userBannedRefresh.SearchUsers("randomname", result => { if (!result.IsError) SearchUserSuccessfulUnbanCount++; });
            }
            yield return TestHelper.WaitForValue(() => oldAccessTokenUnban != userBannedRefresh.Session.AuthorizationToken);
            while (SearchUserSuccessfulUnbanCount != RandomRequestsCount) yield return new WaitForSeconds(.2f);

            string oldAccessTokenEnableBan = userBannedRefresh.Session.AuthorizationToken;
            Result<UserBanResponseV3> enableBanResponse = null;
            userAdmin.ChangeUserBanStatus(userBannedLoginData["userId"], banResponse.Value.banId, true, result => { enableBanResponse = result; });
            yield return TestHelper.WaitForValue(() => enableBanResponse, "wait for enable ban response", 120000);
            TestHelper.Assert.IsFalse(enableBanResponse.IsError, "enable ban user ");

            yield return new WaitForSeconds(2f);
            int SearchUserSuccessfulEnableBanCount = 0;
            for (int i = 0; i < RandomRequestsCount; i++)
            {
                userBannedRefresh.SearchUsers("randomname", result => { if (!result.IsError) SearchUserSuccessfulEnableBanCount++; });
            }
            yield return TestHelper.WaitForValue(() => oldAccessTokenEnableBan != userBannedRefresh.Session.AuthorizationToken);
            while (SearchUserSuccessfulEnableBanCount != RandomRequestsCount) yield return new WaitForSeconds(.2f);

            TestHelper.Assert.IsTrue(SearchUserSuccessfulCount == RandomRequestsCount, "SearchUserSuccessfulCount :" + SearchUserSuccessfulCount);
            TestHelper.Assert.IsTrue(SearchUserSuccessfulUnbanCount == RandomRequestsCount, "SearchUserSuccessfulUnbanCount :" + SearchUserSuccessfulUnbanCount);
            TestHelper.Assert.IsTrue(SearchUserSuccessfulEnableBanCount == RandomRequestsCount, "SearchUserSuccessfulEnableBanCount :" + SearchUserSuccessfulEnableBanCount);
        }

        [UnityTest, TestLog, Timeout(150000), Order(6)]
        public IEnumerator BanUser_AccountBan()
        {
            User userBannedRefresh = userBanned;
            Dictionary<string, string> userBannedLoginData = usersLoginData[1];

            Result loginResult = null;

            DateTime endDate = DateTime.UtcNow;
            endDate = endDate.AddDays(1);

            string oldAccessToken = userBannedRefresh.Session.AuthorizationToken;
            Result<UserBanResponseV3> banResponse = null;
            userAdmin.BanUser(userBannedLoginData["userId"], BanType.LOGIN, BanReason.IMPERSONATION, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse, "wait for ban response", 120000);

            TestHelper.Assert.IsFalse(banResponse.IsError);
            TestHelper.Assert.IsTrue(banResponse.Value.reason == BanReason.IMPERSONATION);

            Result<PagedPublicUsersInfo> banSearchResponse = null;
            userBannedRefresh.SearchUsers("", result => { banSearchResponse = result; });
            yield return TestHelper.WaitForValue(() => banSearchResponse);

            loginResult = null;
            userBannedRefresh.LoginWithUsername(userBannedLoginData["email"], userBannedLoginData["password"], result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");
            TestHelper.Assert.IsTrue(loginResult.IsError, "login user ");

            string oldAccessTokenUnban = userBannedRefresh.Session.AuthorizationToken;
            Result<UserBanResponseV3> unbanResponse = null;
            userAdmin.ChangeUserBanStatus(userBannedLoginData["userId"], banResponse.Value.banId, false, result => { unbanResponse = result; });
            yield return TestHelper.WaitForValue(() => unbanResponse, "wait for unban response", 120000);
            TestHelper.Assert.IsFalse(unbanResponse.IsError, "unban user ");

            loginResult = null;
            userBannedRefresh.LoginWithUsername(userBannedLoginData["email"], userBannedLoginData["password"], result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");
            TestHelper.Assert.IsResultOk(loginResult, "login user ");

            Result<PagedPublicUsersInfo> unbanSearchResponse = null;
            userBannedRefresh.SearchUsers("", result => { unbanSearchResponse = result; });
            yield return TestHelper.WaitForValue(() => unbanSearchResponse);
            TestHelper.Assert.IsFalse(unbanSearchResponse.IsError);

            string oldAccessTokenEnableBan = userBannedRefresh.Session.AuthorizationToken;
            Result<UserBanResponseV3> enableBanResponse = null;
            userAdmin.ChangeUserBanStatus(userBannedLoginData["userId"], banResponse.Value.banId, true, result => { enableBanResponse = result; });
            yield return TestHelper.WaitForValue(() => enableBanResponse, "wait for enable ban response", 120000);
            TestHelper.Assert.IsResultOk(enableBanResponse, "enable ban user ");

            Result<PagedPublicUsersInfo> enableBanSearchResponse = null;
            userBannedRefresh.SearchUsers("", result => { enableBanSearchResponse = result; });
            yield return TestHelper.WaitForValue(() => enableBanSearchResponse);
            TestHelper.Assert.IsTrue(enableBanSearchResponse.IsError);

        }

        [UnityTest, TestLog, Timeout(600000), Order(7)]
        public IEnumerator BanUser_Lobby_FeatureBan()
        {
            User userBannedLobby = userBanned;
            Dictionary<string, string> userBannedLoginData = usersLoginData[1];

            var lobby = CreateLobby(userBannedLobby);

            banNotif = null;
            unbanNotif = null;
            disconnectNotif = null;
            lobby.UserBannedNotification += BanNotifHandler;
            lobby.UserUnbannedNotification += UnbanNotifHandler;
            lobby.Disconnecting += DisconnectNotifHandler;

            if (!lobby.IsConnected) lobby.Connect();
            while (!lobby.IsConnected) yield return new WaitForSeconds(.2f);

            DateTime endDate = DateTime.UtcNow;
            endDate = endDate.AddDays(1);

            banNotif = null;
            unbanNotif = null;
            disconnectNotif = null;
            Result<UserBanResponseV3> banResponse = null;
            userAdmin.BanUser(userBannedLoginData["userId"], BanType.MATCHMAKING, BanReason.IMPERSONATION, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse, "wait for ban response", 120000);
            Assert.IsFalse(banResponse.IsError);

            Debug.Log($"Waiting: banNotif");
            yield return new WaitUntil(() => { return banNotif != null; });
            Debug.Log($"Waiting: disconnectNotif");
            yield return new WaitUntil(() => { return disconnectNotif != null; });
            Debug.Log($"Waiting: connection closed");
            yield return new WaitUntil(() => { return !lobby.IsConnected; });
            bool LobbyDisconnectedBan = !lobby.IsConnected;

            yield return TestHelper.WaitUntil(() => lobby.IsConnected, "wait connection reconnected");
            bool LobbyReconnectedBan = lobby.IsConnected;

            banNotif = null;
            unbanNotif = null;
            disconnectNotif = null;
            Result<UserBanResponseV3> unbanResponse = null;
            userAdmin.ChangeUserBanStatus(userBannedLoginData["userId"], banResponse.Value.banId, false, result => { unbanResponse = result; });
            yield return TestHelper.WaitForValue(() => unbanResponse, "wait for unban response", 120000);
            TestHelper.Assert.IsFalse(unbanResponse.IsError, "unban user ");

            Debug.Log($"Waiting: unbanNotif");
            yield return new WaitUntil(() => { return unbanNotif != null; });
            Debug.Log($"Waiting: disconnectNotif");
            yield return new WaitUntil(() => { return disconnectNotif != null; });
            Debug.Log($"Waiting: connection closed");
            yield return new WaitUntil(() => { return !lobby.IsConnected; });
            bool LobbyDisconnectedUnban = !lobby.IsConnected;

            yield return TestHelper.WaitUntil(() => lobby.IsConnected, "wait connection reconnected");
            bool LobbyReconnectedUnban = lobby.IsConnected;

            banNotif = null;
            unbanNotif = null;
            disconnectNotif = null;
            Result<UserBanResponseV3> enableBanResponse = null;
            userAdmin.ChangeUserBanStatus(userBannedLoginData["userId"], banResponse.Value.banId, true, result => { enableBanResponse = result; });
            yield return TestHelper.WaitForValue(() => enableBanResponse, "wait for enable ban response", 120000);
            TestHelper.Assert.IsFalse(enableBanResponse.IsError, "enable ban user ");

            Debug.Log($"Waiting: banNotif");
            yield return new WaitUntil(() => { return banNotif != null; });
            Debug.Log($"Waiting: disconnectNotif");
            yield return new WaitUntil(() => { return disconnectNotif != null; });
            Debug.Log($"Waiting: connection closed");
            yield return new WaitUntil(() => { return !lobby.IsConnected; });
            bool LobbyDisconnectedEnableBan = !lobby.IsConnected;

            yield return TestHelper.WaitUntil(() => lobby.IsConnected, "wait connection reconnected");
            bool LobbyReconnectedEnableBan = lobby.IsConnected;

            TestHelper.Assert.IsTrue(LobbyDisconnectedBan, "lobby disconnected on Ban");
            TestHelper.Assert.IsTrue(LobbyReconnectedBan, "lobby reconnected on Ban");
            TestHelper.Assert.IsTrue(LobbyDisconnectedUnban, "lobby disconnected on Unban");
            TestHelper.Assert.IsTrue(LobbyReconnectedUnban, "lobby reconnected on Unban");
            TestHelper.Assert.IsTrue(LobbyDisconnectedEnableBan, "lobby disconnected on Enable Ban");
            TestHelper.Assert.IsTrue(LobbyReconnectedEnableBan, "lobby reconnected on Enable Ban");

            lobby.Disconnect();
            yield return TestHelper.WaitUntil(() => !lobby.IsConnected, "wait connection closed", 120000);
        }

        [UnityTest, TestLog, Timeout(200000), Order(8)]
        public IEnumerator BanUser_Lobby_AccountBan()
        {
            User userBannedLobby = userBanned;
            Dictionary<string, string> userBannedLoginData = usersLoginData[1];

            var lobby = CreateLobby(userBannedLobby);

            banNotif = null;
            unbanNotif = null;
            disconnectNotif = null;
            lobby.UserBannedNotification += BanNotifHandler;
            lobby.UserUnbannedNotification += UnbanNotifHandler;

            Result loginResult = null;

            if (!lobby.IsConnected) lobby.Connect();
            while (!lobby.IsConnected) yield return new WaitForSeconds(.2f);

            DateTime endDate = DateTime.UtcNow;
            endDate = endDate.AddDays(1);

            banNotif = null;
            unbanNotif = null;
            disconnectNotif = null;
            Result<UserBanResponseV3> banResponse = null;
            userAdmin.BanUser(userBannedLoginData["userId"], BanType.LOGIN, BanReason.IMPERSONATION, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse, "wait for ban response", 120000);
            TestHelper.Assert.IsFalse(banResponse.IsError);
            
            yield return TestHelper.WaitUntil(() => !lobby.IsConnected, "wait connection closed", 120000);
            bool LobbyDisconnectedBan = !lobby.IsConnected;

            if (!lobby.IsConnected) lobby.Connect();
            yield return new WaitForSeconds(3f);
            bool LobbyReconnectedBanFailed = !lobby.IsConnected;

            Result<UserData> userDataResult = null;
            userBannedLobby.RefreshData(result => userDataResult = result);
            yield return TestHelper.WaitForValue(() => userDataResult);
            TestHelper.Assert.IsTrue(userDataResult.IsError, "get user data ");

            banNotif = null;
            unbanNotif = null;
            disconnectNotif = null;
            Result<UserBanResponseV3> unbanResponse = null;
            userAdmin.ChangeUserBanStatus(userBannedLoginData["userId"], banResponse.Value.banId, false, result => { unbanResponse = result; });
            yield return TestHelper.WaitForValue(() => unbanResponse, "wait for enable unban response", 120000);
            TestHelper.Assert.IsFalse(unbanResponse.IsError, "unban user ");

            loginResult = null;
            userBannedLobby.LoginWithUsername(userBannedLoginData["email"], userBannedLoginData["password"], result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");
            TestHelper.Assert.IsFalse(loginResult.IsError, "login user ");

            lobby = CreateLobby(userBannedLobby);

            if (!lobby.IsConnected) lobby.Connect();
            while (!lobby.IsConnected) yield return new WaitForSeconds(.2f);

            bool LobbyReconnectedUnbanSuccess = lobby.IsConnected;

            TestHelper.Assert.IsTrue(LobbyDisconnectedBan, "lobby disconnected on Ban"); 
            TestHelper.Assert.IsTrue(LobbyReconnectedUnbanSuccess, "lobby reconnected on Unban");

            lobby.Disconnect();
            yield return TestHelper.WaitUntil(() => !lobby.IsConnected, "wait connection closed", 120000);
        }
    }
}