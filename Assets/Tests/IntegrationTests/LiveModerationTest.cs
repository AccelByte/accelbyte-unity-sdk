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

        const int NUMBEROFUSER = 2;
        private string adminRoleId = "";

        private Lobby CreateLobby(User user)
        {
            return LobbyTestUtil.CreateLobby(user.Session, this.httpClient, coroutineRunner);
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
                        string.Format("bantestunity+{0}{1}@example.com", i + 1, guid),
                        "Password123",
                        "userban" + (i + 1) + guid,
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
                        string.Format("bantestunity+{0}{1}@example.com", i + 1, guid),
                        "Password123",
                        result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                Result<UserData> userResult = null;
                newUsers[i].GetData(r => userResult = r);
                yield return TestHelper.WaitForValue(() => userResult);

                TestHelper.LogResult(loginResult, "Setup: Logged in " + userResult.Value.displayName);
            }

            yield return new WaitForSeconds(0.1f);

            this.userAdmin = newUsers[0];
            this.userBanned = newUsers[1];
        }

        [UnityTearDown]
        public IEnumerator TestTeardown()
        {
            var user = new User[NUMBEROFUSER];
            user[0] = this.userAdmin;
            user[1] = this.userBanned;
            for (int i = 0; i < NUMBEROFUSER; i++)
            {
                Result deleteResult = null;

                this.helper.DeleteUser(user[i], result => deleteResult = result);
                yield return TestHelper.WaitForValue(() => deleteResult);

                TestHelper.LogResult(deleteResult, "Setup: Deleted user" + (i + 1));
                TestHelper.Assert.IsResultOk(deleteResult);
            }

            this.userAdmin = null;
            this.userBanned = null;
        }

        [UnityTest, TestLog, Timeout(30000)]
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

        [UnityTest, TestLog, Timeout(30000)]
        public IEnumerator BanUser_GetNotifSuccess()
        {
            //Arrange
            var user = userAdmin;
            var lobby = CreateLobby(userBanned);

            if (!lobby.IsConnected) lobby.Connect();
            while (!lobby.IsConnected) yield return new WaitForSeconds(.2f);

            Result<UserBannedNotification> banNotif = null;
            lobby.UserBannedNotification += result => { banNotif = result; };

            DateTime endDate = DateTime.UtcNow;
            endDate = endDate.AddDays(1);

            Result<UserBanResponseV3> banResponse = null;
            user.BanUser(userBanned.Session.UserId, BanType.CHAT_ALL, BanReason.MALICIOUS_CONTENT, endDate, "test", false, result => { banResponse = result; });
            yield return TestHelper.WaitForValue(() => banResponse);

            yield return TestHelper.WaitForValue(() => banNotif);

            lobby.Disconnect();

            Assert.IsFalse(banResponse.IsError);
            TestHelper.Assert.IsResultOk(banNotif);
            Assert.That(banNotif.Value.userId == userBanned.Session.UserId);
            TestHelper.Assert.IsTrue(banNotif.Value.reason == BanReason.MALICIOUS_CONTENT);
        }
    }
}