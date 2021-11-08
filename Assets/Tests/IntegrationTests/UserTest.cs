// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;
#if !DISABLESTEAMWORKS
using Steamworks;

#endif

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class UserTest
    {
        private readonly AccelByteHttpClient httpClient =  new AccelByteHttpClient();
        private readonly CoroutineRunner coroutineRunner = new CoroutineRunner();
        private readonly string steamTicket = TestHelper.GenerateSteamTicket();
        private readonly TestHelper helper = new TestHelper();

        User CreateUser()
        {
            this.httpClient.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            var loginSession = new LoginSession(
                AccelBytePlugin.Config.IamServerUrl,
                AccelBytePlugin.Config.Namespace,
                AccelBytePlugin.Config.RedirectUri,
                this.httpClient,
                coroutineRunner);
            
            return new User(
                loginSession,
                new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    this.httpClient),
                coroutineRunner);
        }

#if !DISABLESTEAMWORKS
        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithSteam_LoginTwice_SameUserId()
        {
            User user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var attempt = 0;
            var loginSteamResults = new Result[2];
            var steamUserIds = new string[2];
            Result deleteResult = null;

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Debug.Log("Login 1 With Steam Ticket: " + steamTicket);
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                steamTicket,
                result => { loginSteamResults[attempt] = result; });

            while (loginSteamResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginSteamResults[attempt], "Login 1 With Steam Ticket");
            steamUserIds[attempt] = user.Session.UserId;

            Result userLogoutResult = null;
            user.Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            attempt += 1;


            Debug.Log("Login 2 With Steam Ticket: " + steamTicket);
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                steamTicket.ToString(),
                result => { loginSteamResults[attempt] = result; });

            while (loginSteamResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginSteamResults[attempt], "Login 2 With Steam Ticket");
            steamUserIds[attempt] = user.Session.UserId;

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(deleteResult, "Logout");

            TestHelper.Assert.IsResultOk(loginSteamResults[0]);
            TestHelper.Assert.IsResultOk(loginSteamResults[1]);
            TestHelper.Assert.That(steamUserIds[0] == steamUserIds[1]);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithSteam_UniqueUserIdCreated_DifferentUserId()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            int attempt;
            Result[] loginSteamResults = new Result[2];
            Result[] deleteResults = new Result[2];
            string[] steamUserIds = new string[2];

            attempt = 0;

            Debug.Log("Login 1 With Steam Ticket");
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                steamTicket,
                result => { loginSteamResults[attempt] = result; });

            while (loginSteamResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginSteamResults[attempt], "Login 1 With Steam Ticket");
            steamUserIds[attempt] = user.Session.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 2");

            Result userLogoutResult = null;
            user.Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            attempt += 1;

            Debug.Log("Login 2 With Steam Ticket");
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                steamTicket,
                result => { loginSteamResults[attempt] = result; });

            while (loginSteamResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginSteamResults[attempt], "Login 2 With Steam Ticket");
            steamUserIds[attempt] = user.Session.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 2");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.IsResultOk(loginSteamResults[0]);
            TestHelper.Assert.IsResultOk(loginSteamResults[1]);
            TestHelper.Assert.That(steamUserIds[0] != steamUserIds[1]);
            TestHelper.Assert.IsResultOk(deleteResults[0]);
            TestHelper.Assert.IsResultOk(deleteResults[1]);
        }
#endif

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithDevice_LoginTwice_SameUserId()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            int attempt = 0;
            Result[] loginDeviceResults = new Result[2];
            Result deleteResult = null;
            string[] deviceAccountIds = new string[2];
            Result logoutResult = null;

            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Debug.Log("Login 1 With Device Id");
            user.LoginWithDeviceId(result => { loginDeviceResults[attempt] = result; });

            while (loginDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResults[attempt], "Login 1 With Device Id");
            deviceAccountIds[attempt] = user.Session.UserId;

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            attempt += 1;
            Debug.Log("Login 2 With Device Id");
            user.LoginWithDeviceId(result => { loginDeviceResults[attempt] = result; });

            while (loginDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResults[attempt], "Login 2 With Device Id");
            deviceAccountIds[attempt] = user.Session.UserId;

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.IsResultOk(loginDeviceResults[0]);
            TestHelper.Assert.IsResultOk(loginDeviceResults[1]);
            TestHelper.Assert.That(deviceAccountIds[0] == deviceAccountIds[1]);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        const int tokenExpireTime = 60;

        [UnityTest, TestLog, Timeout((tokenExpireTime * 3000) + 1000000), Ignore("For Manual Test Only")]
        public IEnumerator LoginWithDevice_RefreshToken()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            float expire_in = tokenExpireTime;
            int refreshCount = 0;
            Dictionary<string, int> refreshedToken = new Dictionary<string, int>();
            Result loginDeviceResults = null;
            Result deleteResult = null;
            Result logoutResult = null;

            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Debug.Log("Login 1 With Device Id");
            user.LoginWithDeviceId(result => { loginDeviceResults = result; });

            while (loginDeviceResults == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResults, "Login With Device Id");
            refreshedToken[user.Session.AuthorizationToken] = refreshCount++;

            yield return new WaitForSeconds((float) ((expire_in - 1) * 0.8) + 2);

            refreshedToken[user.Session.AuthorizationToken] = refreshCount++;

            yield return new WaitForSeconds((float) ((expire_in - 1) * 0.8) + 2);

            refreshedToken[user.Session.AuthorizationToken] = refreshCount++;

            yield return new WaitForSeconds((float) ((expire_in - 1) * 0.8) + 2);

            refreshedToken[user.Session.AuthorizationToken] = refreshCount++;

            var entitlement = AccelBytePlugin.GetEntitlement();
            var lobby = AccelBytePlugin.GetLobby();

            Result<EntitlementPagingSlicedResult> entitlementResult = null;
            entitlement.QueryUserEntitlements("", "", 0, 0, result => { entitlementResult = result; });

            while (entitlementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            lobby.Connect();

            yield return new WaitForSeconds(10f);

            bool isLobbyConnected = lobby.IsConnected;

            lobby.Disconnect();

            string userId = user.Session.UserId;

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            helper.DeleteUser(userId, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            TestHelper.Assert.That(refreshCount > 0);
            TestHelper.Assert.That(refreshedToken.Count > 1);
            TestHelper.Assert.IsFalse(entitlementResult.IsError);
            TestHelper.Assert.IsTrue(isLobbyConnected);
            TestHelper.Assert.That(!deleteResult.IsError);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithDevice_UniqueUserIdCreated_DifferentUserId()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            int attempt = 0;
            Result[] loginWithDeviceResults = new Result[2];
            Result[] deleteResults = new Result[2];
            string[] deviceAccountUserIds = new string[2];
            Result logoutResult = null;

            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Debug.Log("Login 1 With Device Id");
            user.LoginWithDeviceId(result => { loginWithDeviceResults[attempt] = result; });

            while (loginWithDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginWithDeviceResults[attempt], "Login 1 With Device Id");
            deviceAccountUserIds[attempt] = user.Session.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 1");
            logoutResult = null;

            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            attempt += 1;
            Debug.Log("Login 2 With Device Id");
            user.LoginWithDeviceId(result => { loginWithDeviceResults[attempt] = result; });

            while (loginWithDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginWithDeviceResults[attempt], "Login 2 With Device Id");
            deviceAccountUserIds[attempt] = user.Session.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 2");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.IsResultOk(loginWithDeviceResults[0]);
            TestHelper.Assert.IsResultOk(loginWithDeviceResults[1]);
            TestHelper.Assert.That(deviceAccountUserIds[0] != deviceAccountUserIds[1]);
            TestHelper.Assert.IsResultOk(deleteResults[0]);
            TestHelper.Assert.IsResultOk(deleteResults[1]);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithDevice_RefreshSession_DifferentAuthorizationToken()
        {
            var user = AccelBytePlugin.GetUser();
            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);

            yield return TestHelper.WaitUntil(() => loginResult != null);

            string tokenBeforeRefresh = user.Session.AuthorizationToken;

            yield return new WaitForSeconds(2);

            Result refreshResult = null;
            user.RefreshSession(result => refreshResult = result);

            yield return TestHelper.WaitUntil(() => refreshResult != null);

            string tokenAfterRefresh = user.Session.AuthorizationToken;
            
            TestHelper helper = new TestHelper();
            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            yield return TestHelper.WaitUntil(() => deleteResult != null);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.Assert.That(tokenBeforeRefresh, Is.Not.EqualTo(tokenAfterRefresh));
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithDevice_IsComply_Success()
        {
            var user = AccelBytePlugin.GetUser();
            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);

            yield return TestHelper.WaitUntil(() => loginResult != null);

            bool isComply = user.Session.IsComply;

            TestHelper helper = new TestHelper();
            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            yield return TestHelper.WaitUntil(() => deleteResult != null);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.Assert.That(isComply);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithLauncher_Success()
        {
            var user = AccelBytePlugin.GetUser();

            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string username = (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48);
            const string password = "Password123";
            Result<RegisterUserResponse> registerResult = null;
            Result<string> loginWithUsernameResult = null;
            Result loginWithLauncherResult = null;
            Result deleteResult = null;
            Result<string> authCodeResult = null;

            user.Register(
                email,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            TestHelper.Assert.IsResultOk(registerResult);

            // simulate launcher
            helper.LoginInLauncher(email, password, result => loginWithUsernameResult = result);
            yield return TestHelper.WaitForValue(() => loginWithUsernameResult);
            TestHelper.Assert.IsResultOk(loginWithUsernameResult);

            helper.GetAuthCode(loginWithUsernameResult.Value, result => authCodeResult = result);
            yield return TestHelper.WaitForValue(() => authCodeResult);
            TestHelper.Assert.IsResultOk(authCodeResult);

            Environment.SetEnvironmentVariable("JUSTICE_AUTHORIZATION_CODE", authCodeResult.Value);

            user.LoginWithLauncher(r => loginWithLauncherResult = r);
            yield return TestHelper.WaitForValue(() => loginWithLauncherResult);

            Environment.SetEnvironmentVariable("JUSTICE_AUTHORIZATION_CODE", null);

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");
            TestHelper.Assert.IsResultOk(loginWithLauncherResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithLauncher_NoEnvVar_Failed()
        {
            var user = AccelBytePlugin.GetUser();

            Environment.SetEnvironmentVariable("JUSTICE_AUTHORIZATION_CODE", null);

            Result loginResult = null;
            user.LoginWithLauncher(r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.Assert.IsTrue(loginResult.IsError && loginResult.Error.Code == ErrorCode.InvalidArgument);
        }

#if !DISABLESTEAMWORKS
        [UnityTest, TestLog, Timeout(300000), Order(999)]
        public IEnumerator LoginWithSteam_GenerateNewTicketEveryLogin_Successful()
        {
            var user = AccelBytePlugin.GetUser();
            const int LoginCount = 37;
            int successfulLoginCount = 0;

            Result logoutResult = null;
            user.Logout(result => logoutResult = result);
            yield return TestHelper.WaitUntil(() => logoutResult != null);

            for (int i = 0; i < LoginCount; i++)
            {
                Result loginResult = null;
                string newSteamTicket = TestHelper.GenerateSteamTicket();
                user.LoginWithOtherPlatform(PlatformType.Steam, newSteamTicket, result => loginResult = result);
                yield return TestHelper.WaitUntil(() => loginResult != null);

                if (!loginResult.IsError)
                {
                    successfulLoginCount++;
                }

                TestHelper.Assert.IsResultOk(loginResult);

                Result deleteResult = null;
                this.helper.DeleteUser(user, result => deleteResult = result);
                yield return TestHelper.WaitUntil(() => deleteResult != null);
                
                logoutResult = null;
                user.Logout(result => logoutResult = result);
                yield return TestHelper.WaitUntil(() => logoutResult != null);
            }
            
            TestHelper.Assert.That(successfulLoginCount, Is.EqualTo(LoginCount));
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator UpgradeSteamAccount_ThenLoginWithEmail_Successful()
        {
            var user = AccelBytePlugin.GetUser();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("sdkUpgrade+{0}@example.com", guid);
            const string password = "Password123";
            Result loginSteamResult = null;
            Result<UserData> upgradeResult = null;
            Result loginWithEmailResult = null;
            Result deleteResult = null;


            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, result => { loginSteamResult = result; });
            yield return TestHelper.WaitForValue(() => loginSteamResult);

            TestHelper.LogResult(loginSteamResult, "Login With Steam");
            string steamUserId = user.Session.UserId;
            string oldAccessToken = user.Session.AuthorizationToken;

            user.Upgrade(email, password, result => { upgradeResult = result; });
            yield return TestHelper.WaitForValue(() => upgradeResult);

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Count");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            yield return new WaitForSeconds(2.0f);

            user.LoginWithUsername(email, password, result => { loginWithEmailResult = result; });
            yield return TestHelper.WaitForValue(() => loginWithEmailResult);

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.Session.UserId;
            string refreshedAccessToken = user.Session.AuthorizationToken;

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(steamUserId == upgradedUserId && steamUserId.Length > 0);
            TestHelper.Assert.That(
                refreshedAccessToken,
                Is.Not.EqualTo(oldAccessToken),
                "Access token isn't refreshed after username and password added to the user's account.");

            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginFromUpgradedSteamAccount_VerifyEmail_Verified()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("sdkUpgrade+{0}@example.com", guid);
            const string password = "Password123";
            Result loginSteamResult = null;
            Result<UserData> upgradeResult = null;
            Result loginWithEmailResult = null;
            Result deleteResult = null;


            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, result => { loginSteamResult = result; });
            yield return TestHelper.WaitForValue(() => loginSteamResult);

            TestHelper.LogResult(loginSteamResult, "Login With Steam");
            string steamUserId = user.Session.UserId;
            string oldAccessToken = user.Session.AuthorizationToken;

            user.Upgrade(email, password, result => { upgradeResult = result; });
            yield return TestHelper.WaitForValue(() => upgradeResult);

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Count");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            yield return new WaitForSeconds(2.0f);

            user.LoginWithUsername(email, password, result => { loginWithEmailResult = result; });
            yield return TestHelper.WaitForValue(() => loginWithEmailResult);

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.Session.UserId;
            string refreshedAccessToken = user.Session.AuthorizationToken;

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(steamUserId == upgradedUserId && steamUserId.Length > 0);
            TestHelper.Assert.That(
                refreshedAccessToken,
                Is.Not.EqualTo(oldAccessToken),
                "Access token isn't refreshed after username and password added to the user's account.");

            TestHelper.Assert.IsResultOk(deleteResult);
        }
#endif

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator Logout()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            Result loginDeviceResult = null;
            Result logoutResult = null;
            Result deleteResult = null;

            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Debug.Log("Login With Device Id");
            user.LoginWithDeviceId(result => { loginDeviceResult = result; });
            yield return TestHelper.WaitForValue(() => loginDeviceResult);

            TestHelper.LogResult(loginDeviceResult, "Login With Device Id");
            string userId = user.Session.UserId;

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout User");

            helper.DeleteUser(userId, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            TestHelper.Assert.IsResultOk(loginDeviceResult);
            TestHelper.Assert.IsResultOk(logoutResult);
            TestHelper.Assert.That(user.Session.UserId, Is.Null);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator UpgradeDeviceAccount_ThenLoginWithEmail_Successful()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            const string password = "Password123";
            Result loginDeviceResult = null;
            Result<UserData> upgradeResult = null;
            Result loginWithEmailResult = null;
            Result deleteResult = null;
            Result secondLoginDeviceResult = null;
            Result secondDeleteResult = null;

            user.LoginWithDeviceId(result => { loginDeviceResult = result; });
            yield return TestHelper.WaitForValue(() => loginDeviceResult);

            TestHelper.LogResult(loginDeviceResult, "Login With Device");
            string deviceUserId = user.Session.UserId;
            string oldAccessToken = user.Session.AuthorizationToken;

            user.Upgrade(email, password, result => { upgradeResult = result; });
            yield return TestHelper.WaitForValue(() => upgradeResult);

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Account");

            Result userLogoutResult1 = null;
            user.Logout(result => userLogoutResult1 = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult1);

            user.LoginWithUsername(email, password, result => { loginWithEmailResult = result; });
            yield return TestHelper.WaitForValue(() => loginWithEmailResult);

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.Session.UserId;
            string refreshedAccessToken = user.Session.AuthorizationToken;

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            Result userLogoutResult2 = null;
            user.Logout(result => userLogoutResult2 = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult2);

            user.LoginWithDeviceId(result => { secondLoginDeviceResult = result; });
            yield return TestHelper.WaitForValue(() => secondLoginDeviceResult);

            TestHelper.LogResult(secondLoginDeviceResult, "Login With Device 2");
            string secondDeviceUserId = user.Session.UserId;

            helper.DeleteUser(user, result => { secondDeleteResult = result; });
            yield return TestHelper.WaitForValue(() => secondDeleteResult);

            TestHelper.LogResult(secondDeleteResult, "Delete User 2");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(
                deviceUserId == upgradedUserId && deviceUserId.Length > 0 && upgradedUserId.Length > 0);

            TestHelper.Assert.That(
                deviceUserId != secondDeviceUserId && deviceUserId.Length > 0 && secondDeviceUserId.Length > 0);

            TestHelper.Assert.That(
                refreshedAccessToken,
                Is.Not.EqualTo(oldAccessToken),
                "Access token isn't refreshed after username and password added to the user's account.");

            TestHelper.Assert.IsResultOk(deleteResult);
            TestHelper.Assert.IsResultOk(secondDeleteResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator UpgradeDeviceAccount_ThenLoginWithUsername_Successful()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string username = (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48);
            const string password = "Password123";
            Result loginDeviceResult = null;
            Result<UserData> upgradeResult = null;
            Result loginWithEmailResult = null;
            Result deleteResult = null;
            Result secondLoginDeviceResult = null;
            Result secondDeleteResult = null;

            user.LoginWithDeviceId(result => { loginDeviceResult = result; });
            yield return TestHelper.WaitForValue(() => loginDeviceResult);

            TestHelper.LogResult(loginDeviceResult, "Login With Device");
            string deviceUserId = user.Session.UserId;
            string oldAccessToken = user.Session.AuthorizationToken;

            user.Upgradev2(email, username, password, result => { upgradeResult = result; });
            yield return TestHelper.WaitForValue(() => upgradeResult);

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Account");

            Result userLogoutResult1 = null;
            user.Logout(result => userLogoutResult1 = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult1);

            user.LoginWithUsername(username, password, result => { loginWithEmailResult = result; });
            yield return TestHelper.WaitForValue(() => loginWithEmailResult);

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.Session.UserId;
            string refreshedAccessToken = user.Session.AuthorizationToken;

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            Result userLogoutResult2 = null;
            user.Logout(result => userLogoutResult2 = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult2);

            user.LoginWithDeviceId(result => { secondLoginDeviceResult = result; });
            yield return TestHelper.WaitForValue(() => secondLoginDeviceResult);

            TestHelper.LogResult(secondLoginDeviceResult, "Login With Device 2");
            string secondDeviceUserId = user.Session.UserId;

            helper.DeleteUser(user, result => { secondDeleteResult = result; });
            yield return TestHelper.WaitForValue(() => secondDeleteResult);

            TestHelper.LogResult(secondDeleteResult, "Delete User 2");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(
                deviceUserId == upgradedUserId && deviceUserId.Length > 0 && upgradedUserId.Length > 0);

            TestHelper.Assert.That(
                deviceUserId != secondDeviceUserId && deviceUserId.Length > 0 && secondDeviceUserId.Length > 0);

            TestHelper.Assert.That(
                refreshedAccessToken,
                Is.Not.EqualTo(oldAccessToken),
                "Access token isn't refreshed after username and password added to the user's account.");

            TestHelper.Assert.IsResultOk(deleteResult);
            TestHelper.Assert.IsResultOk(secondDeleteResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator RegisterWithEmail_ThenLogin_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register With Email: {0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            Result<UserData> getDataResult = null;
            user.GetData(result => getDataResult = result);
            yield return TestHelper.WaitForValue(() => getDataResult);

            TestHelper.LogResult(getDataResult, "Get User Data");
            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(getDataResult.Error, Is.Null);

            Result deleteResult = null;

            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete User");
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator RegisterWithUsername_ThenLogin_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string username = (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48);
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register With Email: {0}, {1}", email, password));
            user.Registerv2(
                email,
                username,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithUsername(username, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            Result<UserData> getDataResult = null;
            user.GetData(result => getDataResult = result);
            yield return TestHelper.WaitForValue(() => getDataResult);

            TestHelper.LogResult(getDataResult, "Get User Data");
            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(getDataResult.Error, Is.Null);

            Result deleteResult = null;

            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete User");
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithEmail_ThenVerify_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });
            yield return TestHelper.WaitForValue(() => clientToken);

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);
            yield return TestHelper.WaitForValue(() => userVerificationCode);

            var verificationCode = userVerificationCode.Value.accountRegistration;

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            Result verificationResult = null;
            user.Verify(verificationCode, result => verificationResult = result);
            yield return TestHelper.WaitForValue(() => verificationResult);

            TestHelper.LogResult(verificationResult, "Verify");

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(verificationResult.Error, Is.Null);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete");
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithEmail_LoginWithIncorrectEmail_Failed()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });
            yield return TestHelper.WaitForValue(() => clientToken);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithUsername("a" + email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            TestHelper.Assert.That(loginResult.IsError);
        }

        const int normal_expired_time = 60;

        [UnityTest, TestLog, Timeout((normal_expired_time * 2000) + 10000), Ignore("For Manual Test Only")]
        public IEnumerator LoginWithEmail_RememberMe_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk" + guid,
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);

            yield return TestHelper.WaitForValue(() => registerResult);

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });

            yield return TestHelper.WaitForValue(() => clientToken);

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);

            yield return TestHelper.WaitForValue(() => userVerificationCode);

            var verificationCode = userVerificationCode.Value.accountRegistration;

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);

            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            Result verificationResult = null;
            user.Verify(verificationCode, result => verificationResult = result);

            yield return TestHelper.WaitForValue(() => verificationResult);

            TestHelper.LogResult(verificationResult, "Verify");

            yield return new WaitForSeconds(normal_expired_time + 5);

            user = null;
            user = CreateUser();

            Result refreshLoginResult = null;
            user.LoginWithLatestRefreshToken(result => refreshLoginResult = result);

            yield return TestHelper.WaitForValue(() => refreshLoginResult);

            Result loginResult2 = null;
            user.LoginWithUsername(email, password, result => loginResult2 = result, true);

            yield return TestHelper.WaitForValue(() => loginResult2);

            yield return new WaitForSeconds(normal_expired_time + 5);

            user = null;
            user = CreateUser();

            Result refreshLoginResult2 = null;
            user.LoginWithLatestRefreshToken(result => refreshLoginResult2 = result);

            yield return TestHelper.WaitForValue(() => refreshLoginResult2);

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(verificationResult.Error, Is.Null);
            TestHelper.Assert.That(refreshLoginResult.Error, Is.Not.Null);
            TestHelper.Assert.That(refreshLoginResult.Error.Code == ErrorCode.InvalidRequest);
            TestHelper.Assert.That(loginResult2.Error, Is.Null);
            TestHelper.Assert.That(refreshLoginResult2.Error, Is.Null);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            yield return TestHelper.WaitForValue(() => deleteResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete");
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithEmail_ThenLoginWithRefreshToken_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";
            string refreshToken = "";

            user.RefreshTokenCallback(
                result =>
                {
                    Debug.Log("Refresh token changed: " + result);
                    refreshToken = result;
                });

            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk" + guid,
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);

            yield return TestHelper.WaitForValue(() => registerResult);

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });

            yield return TestHelper.WaitForValue(() => clientToken);

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);

            yield return TestHelper.WaitForValue(() => userVerificationCode);

            var verificationCode = userVerificationCode.Value.accountRegistration;

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);

            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            user = null;
            user = CreateUser();

            Result refreshLoginResult = null;
            user.LoginWithLatestRefreshToken(result => refreshLoginResult = result);

            yield return TestHelper.WaitForValue(() => refreshLoginResult);

            if (refreshLoginResult.IsError && refreshLoginResult.Error.Code == ErrorCode.InvalidRequest)
            {
                refreshLoginResult = null;
                user.LoginWithLatestRefreshToken(refreshToken, result => refreshLoginResult = result);

                yield return TestHelper.WaitForValue(() => refreshLoginResult);
            }

            TestHelper.Assert.IsFalse(registerResult.IsError);
            TestHelper.Assert.IsFalse(loginResult.IsError);
            TestHelper.Assert.IsFalse(refreshLoginResult.IsError);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            yield return TestHelper.WaitForValue(() => deleteResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            yield return TestHelper.WaitForValue(() => logoutResult);

            user = null;
            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete");
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LoginWithUsername_ThenVerify_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string username = (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48);
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Registerv2(
                email,
                username,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });
            yield return TestHelper.WaitForValue(() => clientToken);

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);
            yield return TestHelper.WaitForValue(() => userVerificationCode);

            var verificationCode = userVerificationCode.Value.accountRegistration;

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result loginResult = null;
            user.LoginWithUsername(username, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            Result verificationResult = null;
            user.Verify(verificationCode, result => verificationResult = result);
            yield return TestHelper.WaitForValue(() => verificationResult);

            TestHelper.LogResult(verificationResult, "Verify");

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(verificationResult.Error, Is.Null);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete");
        }

#if !DISABLESTEAMWORKS
        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator RegisterWithEmail_ThenResetPassword_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            Result deleteResult = null;
            Result<RegisterUserResponse> registerResult = null;
            Result loginResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Email");

            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result forgotPasswordResult = null;

            user.SendResetPasswordCode(email, result => forgotPasswordResult = result);
            yield return TestHelper.WaitForValue(() => forgotPasswordResult);

            TestHelper.LogResult(forgotPasswordResult, "Forgot Password");

            if (forgotPasswordResult.IsError)
            {
                helper.DeleteUser(PlatformType.Steam, steamTicket, result => deleteResult = result);
                yield return TestHelper.WaitForValue(() => deleteResult);

                TestHelper.LogResult(deleteResult, "Delete User");

                TestHelper.Assert.Fail("Send Reset Password Code");

                yield break;
            }

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });
            yield return TestHelper.WaitForValue(() => clientToken);

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                user.Session.UserId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);
            yield return TestHelper.WaitForValue(() => userVerificationCode);

            var verificationCode = userVerificationCode.Value.passwordReset;
            Debug.Log(
                string.Format("Reset Password, user:{0}, password:{1}, code:{2}", email, password, verificationCode));

            Result resetPasswordResult = null;
            password = "new" + password;

            user.ResetPassword(verificationCode, email, password, result => resetPasswordResult = result);
            yield return TestHelper.WaitForValue(() => resetPasswordResult);

            TestHelper.LogResult(resetPasswordResult, "Reset Password");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout after reset password");

            loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.IsResultOk(registerResult);
            TestHelper.Assert.IsResultOk(loginResult);
            TestHelper.Assert.IsResultOk(resetPasswordResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator RegisterWithEmail_ResendVerificationCode_VerifiedWithLastSentCode()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Email");

            Result sendCodeResult = null;
            user.SendVerificationCode(result => sendCodeResult = result);
            yield return TestHelper.WaitForValue(() => sendCodeResult);

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });
            yield return TestHelper.WaitForValue(() => clientToken);

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);
            yield return TestHelper.WaitForValue(() => userVerificationCode);

            var verificationCode = userVerificationCode.Value.accountRegistration;

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            Result verificationResult = null;
            user.Verify(verificationCode, result => verificationResult = result);
            yield return TestHelper.WaitForValue(() => verificationResult);

            TestHelper.LogResult(verificationResult, "Verify");

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(verificationResult.Error, Is.Null);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete");
        }
#endif

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator UpdateUser_WithCountry_ReturnsTokenResponseWithCountry()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();

            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register by Publisher:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "ID",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register");

            Debug.Log("Login after register");
            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");

            Result<UserData> updateResult = null;
            UpdateUserRequest updateRequest = new UpdateUserRequest {country = "US"};

            user.Update(updateRequest, result => updateResult = result);
            yield return TestHelper.WaitForValue(() => updateResult);

            TestHelper.LogResult(updateResult, "Update User Account");
            Result deleteResult = null;

            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.IsResultOk(registerResult, "register user ");
            TestHelper.Assert.IsResultOk(loginResult, "login user ");
            TestHelper.Assert.IsResultOk(updateResult, "update user ");
            TestHelper.Assert.That(registerResult.Value.country, !Is.EqualTo("US"));
            TestHelper.Assert.That(updateResult.Value.country, Is.EqualTo("US"));
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator UpdateUser_WithUsername_ThenLogin()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();

            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string username = (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48);
            string password = "AccelbytE123";

            Debug.Log(string.Format("Register by Publisher:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "ID",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register");

            Debug.Log("Login after register");
            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");

            Result<UserData> updateResult = null;
            UpdateUserRequest updateRequest = new UpdateUserRequest {username = username};

            user.Update(updateRequest, result => updateResult = result);
            yield return TestHelper.WaitForValue(() => updateResult);

            TestHelper.LogResult(updateResult, "Update User Account");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            Result usernameLoginResult = null;
            user.LoginWithUsername(username, password, r => usernameLoginResult = r);
            yield return TestHelper.WaitForValue(() => usernameLoginResult);

            TestHelper.LogResult(usernameLoginResult, "Username Login");

            Result deleteResult = null;

            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.IsResultOk(registerResult, "register user ");
            TestHelper.Assert.IsResultOk(loginResult, "login user ");
            TestHelper.Assert.IsResultOk(updateResult, "update user ");
            TestHelper.Assert.That(registerResult.Value.username, !Is.EqualTo(username));
            TestHelper.Assert.That(updateResult.Value.userName, Is.EqualTo(username));
            TestHelper.Assert.IsResultOk(usernameLoginResult, "username login ");
        }
        
        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator GetPublicUserProfile()
        {
            var user = AccelBytePlugin.GetUser();
            Result loginResult = null;
            var userProfiles = AccelBytePlugin.GetUserProfiles();
            
            Result<UserProfile> createProfileResult = null;
            Result deleteResult = null;
            Result deleteProfileResult = null;
            
            user.LoginWithDeviceId(result => { loginResult = result; });
            yield return TestHelper.WaitForValue(() => loginResult);
            
            Debug.Log("Access Token: " + user.Session.AuthorizationToken);
            
            userProfiles.CreateUserProfile(
                new CreateUserProfileRequest
                {
                    language = "en",
                    timeZone = "Asia/Jakarta",
                    firstName = "first",
                    lastName = "last",
                    dateOfBirth = "2000-01-01"
                },
                result => createProfileResult = result);
            yield return TestHelper.WaitForValue(() => createProfileResult);

            TestHelper.LogResult(createProfileResult, "\r\nCreate User Profile");
            
            Debug.Log("userID: " + user.Session.UserId);

            Result<PublicUserProfile> getPublicProfileResult = null;
            userProfiles.GetPublicUserProfile(user.Session.UserId,result => { getPublicProfileResult = result; });
            yield return TestHelper.WaitForValue(() => getPublicProfileResult);

            TestHelper.LogResult(getPublicProfileResult, "Get Public User Profile");
            
            helper.DeleteUserProfile(user, result => { deleteProfileResult = result; });
            yield return TestHelper.WaitForValue(() => deleteProfileResult);

            TestHelper.LogResult(deleteProfileResult, "Delete User Profile");

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");
            
        }
        
        
        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator CreateAndGetUserProfiles_WithDefaultFields_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            Result loginResult = null;
            Result<UserProfile> createProfileResult = null;
            Result<UserProfile> getProfileResult = null;
            Result<UserProfile> updateResult = null;
            Result<UserProfile> getUpdatedProfileResult = null;
            Result deleteResult = null;
            Result deleteProfileResult = null;

            user.LoginWithDeviceId(result => { loginResult = result; });

            yield return TestHelper.WaitForValue(() => loginResult);

            Debug.Log("Access Token: " + user.Session.AuthorizationToken);
            var userProfiles = AccelBytePlugin.GetUserProfiles();
            var customAttributes = new Dictionary<string, object>
            {
                {"number_int", 123},
                {"number_float", 123.123},
                {"string", "jahe santan gula aren"},
                {"bool", true},
                {"string_date", DateTime.UtcNow.ToString("O")}
            };

            userProfiles.CreateUserProfile(
                new CreateUserProfileRequest
                {
                    language = "en",
                    timeZone = "Asia/Jakarta",
                    firstName = "first",
                    lastName = "last",
                    dateOfBirth = "2000-01-01",
                    customAttributes = customAttributes
                },
                result => createProfileResult = result);
            yield return TestHelper.WaitForValue(() => createProfileResult);

            TestHelper.LogResult(createProfileResult, "\r\nCreate User Profile");

            userProfiles.GetUserProfile(result => { getProfileResult = result; });
            yield return TestHelper.WaitForValue(() => getProfileResult);

            TestHelper.LogResult(getProfileResult, "Get User Profile");

            customAttributes.Add("another_string", "Ovis aries");

            var profileUpdate = new UpdateUserProfileRequest
            {
                firstName = "John", lastName = "Doe", language = "en", customAttributes = customAttributes
            };

            userProfiles.UpdateUserProfile(profileUpdate, result => { updateResult = result; });
            yield return TestHelper.WaitForValue(() => updateResult);

            TestHelper.LogResult(updateResult, "Update User Profile");

            userProfiles.GetUserProfile(result => { getUpdatedProfileResult = result; });
            yield return TestHelper.WaitForValue(() => getUpdatedProfileResult);

            helper.DeleteUserProfile(user, result => { deleteProfileResult = result; });
            yield return TestHelper.WaitForValue(() => deleteProfileResult);

            TestHelper.LogResult(deleteProfileResult, "Delete User Profile");

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.IsResultOk(createProfileResult);
            TestHelper.Assert.IsResultOk(getProfileResult);
            TestHelper.Assert.That(getProfileResult.Value.customAttributes.Count == 5);
            TestHelper.Assert.IsResultOk(updateResult);
            TestHelper.Assert.IsResultOk(getUpdatedProfileResult);
            TestHelper.Assert.That(getUpdatedProfileResult.Value.firstName.Length > 0);
            TestHelper.Assert.That(getUpdatedProfileResult.Value.customAttributes.ContainsKey("another_string"));
            TestHelper.Assert.That(getProfileResult.Value.firstName != getUpdatedProfileResult.Value.firstName);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator UpdateUserEmail_ThenLoginWithNewEmail_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            yield return null;

            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";
            var newMailGuid = Guid.NewGuid().ToString("N");
            string newEmail = string.Format("testeraccelbyte+sdk{0}@gmail.com", newMailGuid);

            user.Register(
                email,
                password,
                "testersdk",
                "ID",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            TestHelper.LogResult(registerResult, "Register");
            TestHelper.Assert.IsResultOk(registerResult, "register user ");

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");
            TestHelper.Assert.IsResultOk(loginResult, "login user ");

            Result sendCodeResult = null;
            user.SendVerificationCode(VerificationContext.UpdateEmailAddress, result => sendCodeResult = result);
            yield return TestHelper.WaitForValue(() => sendCodeResult);

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });
            yield return TestHelper.WaitForValue(() => clientToken);

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);
            yield return TestHelper.WaitForValue(() => userVerificationCode);
            TestHelper.LogResult(userVerificationCode, "Get Verification Code");

            string updateMailVerifCode = userVerificationCode.Value.updateEmail;

            Result updateEmailResult = null;
            UpdateEmailRequest updateEmailRequest = new UpdateEmailRequest
            {
                code = updateMailVerifCode, emailAddress = newEmail
            };
            user.UpdateEmail(updateEmailRequest, result => updateEmailResult = result);
            yield return TestHelper.WaitForValue(() => updateEmailResult);
            TestHelper.Assert.IsResultOk(updateEmailResult, "update email ");
            TestHelper.LogResult(updateEmailResult, "Update User Email");

            // get verify code that sent to new email
            userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);
            yield return TestHelper.WaitForValue(() => userVerificationCode);
            TestHelper.LogResult(userVerificationCode, "Get Verification Code");

            string newEmailVerifCode = userVerificationCode.Value.updateEmail;

            Result verificationResult = null;
            user.Verify(newEmailVerifCode, result => verificationResult = result);
            yield return TestHelper.WaitForValue(() => verificationResult);
            TestHelper.LogResult(verificationResult, "Verify");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.LogResult(logoutResult, "Logout User");

            Result loginResult2 = null;
            user.LoginWithUsername(newEmail, password, result => loginResult2 = result);
            yield return TestHelper.WaitForValue(() => loginResult2);
            TestHelper.LogResult(loginResult2, "Login With Updated Email");
            TestHelper.Assert.IsResultOk(loginResult2, "login with new email ");

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);
            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);
            TestHelper.LogResult(logoutResult, "Logout User");
        }

        public class NestedClass
        {
            [DataMember] public bool SomeBool;
            [DataMember] public DateTime SomeDate;
        }

        [DataContract]
        class AClass
        {
            [DataMember] public int SomeInt;

            [DataMember] public string SomeString;

            [DataMember] public NestedClass SomeNestedClass;
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator UpdateCustomAttributes_WithNestedFields_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            Result loginResult = null;
            Result<UserProfile> createProfileResult = null;
            Result deleteResult = null;
            Result deleteProfileResult = null;

            user.LoginWithDeviceId(result => { loginResult = result; });

            yield return TestHelper.WaitForValue(() => loginResult);

            Debug.Log("Access Token: " + user.Session.AuthorizationToken);
            var userProfiles = AccelBytePlugin.GetUserProfiles();
            var customAttributes = new Dictionary<string, object>
            {
                {"number_int", 123}, {"number_float", 125.125}, {"string", "jahe santan gula aren"},
            };

            userProfiles.CreateUserProfile(
                new CreateUserProfileRequest
                {
                    firstName = "John",
                    lastName = "Doe",
                    language = "en",
                    timeZone = "Asia/Jakarta",
                    dateOfBirth = "1998-01-01",
                },
                result => createProfileResult = result);
            yield return TestHelper.WaitForValue(() => createProfileResult);

            Result<Dictionary<string, object>> updatedAttributes = null;

            userProfiles.UpdateCustomAttributes(customAttributes, result => updatedAttributes = result);
            yield return TestHelper.WaitForValue(() => updatedAttributes);

            customAttributes.Clear();
            customAttributes.Add(
                "nested",
                new AClass
                {
                    SomeNestedClass = new NestedClass {SomeBool = true, SomeDate = DateTime.Today},
                    SomeInt = 7070,
                    SomeString = "ur nee"
                });

            Result<Dictionary<string, object>> updatedAttributes2 = null;

            userProfiles.UpdateCustomAttributes(customAttributes, result => updatedAttributes2 = result);
            yield return TestHelper.WaitForValue(() => updatedAttributes2);

            Result<UserProfile> getProfileResult = null;
            userProfiles.GetUserProfile(result => { getProfileResult = result; });
            yield return TestHelper.WaitForValue(() => getProfileResult);

            TestHelper.LogResult(getProfileResult, "Get User Profile");

            helper.DeleteUserProfile(user, result => { deleteProfileResult = result; });
            yield return TestHelper.WaitForValue(() => deleteProfileResult);

            TestHelper.LogResult(deleteProfileResult, "Delete User Profile");

            helper.DeleteUser(user, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Delete User");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.LogResult(logoutResult, "Logout");
            TestHelper.Assert.That(updatedAttributes.Value["number_int"], Is.EqualTo(123));
            TestHelper.Assert.That(updatedAttributes.Value["number_float"], Is.EqualTo(125.125));
            TestHelper.Assert.That(updatedAttributes.Value["string"], Is.EqualTo("jahe santan gula aren"));
            TestHelper.Assert.That(updatedAttributes2.Value.Count, Is.EqualTo(4));
            TestHelper.Assert.That(getProfileResult.Value.customAttributes.Count, Is.EqualTo(4));
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator RegisterWithEmail_GetUserDataWithUserId_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var stringBuilder = new StringBuilder();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Result<RegisterUserResponse> registerResult = null;
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");

            Result<PublicUserData> userData = null;
            user.GetUserByUserId(user.Session.UserId, result => userData = result);
            yield return TestHelper.WaitForValue(() => userData);

            TestHelper.LogResult(userData, "Search UserData with UserId");

            Result deleteResultUser = null;
            helper.DeleteUser(user, result => deleteResultUser = result);
            yield return TestHelper.WaitForValue(() => deleteResultUser);

            TestHelper.LogResult(deleteResultUser, "Delete User With Device ID");

            Result userLogoutResult = null;
            user.Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(userData.Error, Is.Null);
            TestHelper.Assert.That(deleteResultUser.Error, Is.Null);
            Debug.Log("============================================");
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator RegisterWithUsername_GetUserDataWithUserId_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var stringBuilder = new StringBuilder();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string username = (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48);
            string password = "AccelbytE123";

            Result<RegisterUserResponse> registerResult = null;
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Registerv2(
                email,
                username,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Username");

            string userId = registerResult.Value.userId;

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");

            Result<PublicUserData> userData = null;
            user.GetUserByUserId(userId, result => userData = result);
            yield return TestHelper.WaitForValue(() => userData);

            TestHelper.LogResult(userData, "Search UserData with UserId");

            Result deleteResultUser = null;
            helper.DeleteUser(user, result => deleteResultUser = result);
            yield return TestHelper.WaitForValue(() => deleteResultUser);

            TestHelper.LogResult(deleteResultUser, "Delete User With Device ID");

            Result userLogoutResult = null;
            user.Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(userData.Error, Is.Null);
            TestHelper.Assert.That(deleteResultUser.Error, Is.Null);
            Debug.Log("============================================");
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator SearchUserWithAllSearchType_UserExists_UserFound()
        {
            //Arrange
            var httpWorker = new AccelByteHttpClient();
            httpWorker.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            var coroutineRunner = new CoroutineRunner();
            var helper = new TestHelper();
            const string password = "AccelbytE123";
            var users = new User[2];
            var registerUserResponses = new RegisterUserResponse[2];

            for (int i = 0; i < users.Length; i++)
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.RedirectUri,
                    httpWorker,
                    coroutineRunner);

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    httpWorker);

                users[i] = new User(loginSession, userAccount, coroutineRunner);

                var guid = Guid.NewGuid().ToString("N");
                users[i]
                    .Register(
                        string.Format("testeraccelbyte+sdk{0}@gmail.com", guid),
                        password,
                        "testuser" + (i + 1) + guid,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered testuser" + (i + 1));
                TestHelper.Assert.IsResultOk(registerResult);
                registerUserResponses[i] = registerResult.Value;
            }

            Result loginResult = null;
            users[0].LoginWithUsername(registerUserResponses[0].emailAddress, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With First Email");

            loginResult = null;
            users[1].LoginWithUsername(registerUserResponses[1].emailAddress, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Second Email");

            //Act
            Result<PagedPublicUsersInfo> userData = null;
            users[0].SearchUsers(registerUserResponses[1].displayName, result => userData = result);
            yield return TestHelper.WaitForValue(() => userData);

            TestHelper.LogResult(userData, "Search UserData of Second Email with email address");

            //Assert
            Result deleteResultUserA = null;
            helper.DeleteUser(users[0], result => deleteResultUserA = result);
            yield return TestHelper.WaitForValue(() => deleteResultUserA);

            TestHelper.LogResult(deleteResultUserA, "Delete First Email");

            Result deleteResultUserB = null;
            helper.DeleteUser(users[1], result => deleteResultUserB = result);
            yield return TestHelper.WaitForValue(() => deleteResultUserB);

            TestHelper.LogResult(deleteResultUserB, "Delete Second Email");

            Result userLogoutResult = null;
            users[0].Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            TestHelper.Assert.IsResultOk(userData);
            TestHelper.Assert.That(userData.Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.IsResultOk(deleteResultUserA);
            TestHelper.Assert.IsResultOk(deleteResultUserB);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator SearchUserWithUsername_UserExists_UserFound()
        {
            //Arrange
            var httpWorker = new AccelByteHttpClient();
            httpWorker.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            var coroutineRunner = new CoroutineRunner();
            var helper = new TestHelper();
            const string password = "AccelbytE123";
            var users = new User[2];
            var registerUserResponses = new RegisterUserResponse[2];

            for (int i = 0; i < users.Length; i++)
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.RedirectUri,
                    httpWorker,
                    coroutineRunner);

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    httpWorker);

                users[i] = new User(loginSession, userAccount, coroutineRunner);

                var guid = Guid.NewGuid().ToString("N");
                users[i]
                    .Registerv2(
                        string.Format("testeraccelbyte+sdk{0}@gmail.com", guid),
                        (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48),
                        password,
                        "testuser" + (i + 1) + guid,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered testuser" + (i + 1));
                TestHelper.Assert.IsResultOk(registerResult);
                registerUserResponses[i] = registerResult.Value;
            }

            Result loginResult = null;
            users[0].LoginWithUsername(registerUserResponses[0].emailAddress, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With First Email");

            loginResult = null;
            users[1].LoginWithUsername(registerUserResponses[1].username, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Second Email");

            //Act
            Result<PagedPublicUsersInfo> userData = null;
            users[0].SearchUsers(registerUserResponses[1].username, SearchType.USERNAME, result => userData = result);
            yield return TestHelper.WaitForValue(() => userData);

            TestHelper.LogResult(userData, "Search UserData of Second Email with username");

            //Assert
            Result deleteResultUserA = null;
            helper.DeleteUser(users[0], result => deleteResultUserA = result);
            yield return TestHelper.WaitForValue(() => deleteResultUserA);

            TestHelper.LogResult(deleteResultUserA, "Delete First Email");

            Result deleteResultUserB = null;
            helper.DeleteUser(users[1], result => deleteResultUserB = result);
            yield return TestHelper.WaitForValue(() => deleteResultUserB);

            TestHelper.LogResult(deleteResultUserB, "Delete Second Email");

            Result userLogoutResult = null;
            users[0].Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            TestHelper.Assert.IsResultOk(userData);
            TestHelper.Assert.That(userData.Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.That(userData.Value.data[0].userName, Is.EqualTo(registerUserResponses[1].username));
            TestHelper.Assert.IsResultOk(deleteResultUserA);
            TestHelper.Assert.IsResultOk(deleteResultUserB);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator SearchUserWithDisplayname_UserExists_UserFound()
        {
            //Arrange
            var httpWorker = new AccelByteHttpClient();
            httpWorker.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            var coroutineRunner = new CoroutineRunner();
            var helper = new TestHelper();
            const string password = "AccelbytE123";
            var users = new User[2];
            var registerUserResponses = new RegisterUserResponse[2];

            for (int i = 0; i < users.Length; i++)
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.RedirectUri,
                    httpWorker,
                    coroutineRunner);

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    httpWorker);

                users[i] = new User(loginSession, userAccount, coroutineRunner);

                var guid = Guid.NewGuid().ToString("N");
                users[i]
                    .Registerv2(
                        string.Format("testeraccelbyte+sdk{0}@gmail.com", guid),
                        (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48),
                        password,
                        "testuser" + (i + 1) + guid,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered testuser" + (i + 1));
                TestHelper.Assert.IsResultOk(registerResult);
                registerUserResponses[i] = registerResult.Value;
            }

            Result loginResult = null;
            users[0].LoginWithUsername(registerUserResponses[0].emailAddress, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With First Email");

            loginResult = null;
            users[1].LoginWithUsername(registerUserResponses[1].username, password, result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login With Second Email");

            //Act
            Result<PagedPublicUsersInfo> userData = null;
            users[0].SearchUsers(registerUserResponses[1].displayName, SearchType.DISPLAYNAME, result => userData = result);
            yield return TestHelper.WaitForValue(() => userData);

            TestHelper.LogResult(userData, "Search UserData of Second Email with display name");

            //Assert
            Result deleteResultUserA = null;
            helper.DeleteUser(users[0], result => deleteResultUserA = result);
            yield return TestHelper.WaitForValue(() => deleteResultUserA);

            TestHelper.LogResult(deleteResultUserA, "Delete First Email");

            Result deleteResultUserB = null;
            helper.DeleteUser(users[1], result => deleteResultUserB = result);
            yield return TestHelper.WaitForValue(() => deleteResultUserB);

            TestHelper.LogResult(deleteResultUserB, "Delete Second Email");

            Result userLogoutResult = null;
            users[0].Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            TestHelper.Assert.IsResultOk(userData);
            TestHelper.Assert.That(userData.Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.That(userData.Value.data[0].displayName, Is.EqualTo(registerUserResponses[1].displayName));
            TestHelper.Assert.IsResultOk(deleteResultUserA);
            TestHelper.Assert.IsResultOk(deleteResultUserB);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator SearchUserWithSearchType_UserExists_UserFound()
        {
            //Arrange
            var httpWorker = new AccelByteHttpClient();
            httpWorker.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            var coroutineRunner = new CoroutineRunner();
            var helper = new TestHelper();
            const string password = "AccelbytE123";
            var users = new User[3];
            var registerUserResponses = new RegisterUserResponse[3];

            string SearchQuery = Guid.NewGuid().ToString("N");

            // User 0.
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.RedirectUri,
                    httpWorker,
                    coroutineRunner);

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    httpWorker);

                users[0] = new User(loginSession, userAccount, coroutineRunner);

                var guid = Guid.NewGuid().ToString("N");
                users[0]
                    .Registerv2(
                        string.Format("testeraccelbyte+sdk{0}@gmail.com", guid),
                        (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48),
                        password,
                        "testuser1" + guid,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered testuser1");
                registerUserResponses[0] = registerResult.Value;
            }

            // User 1.
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.RedirectUri,
                    httpWorker,
                    coroutineRunner);

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    httpWorker);

                users[1] = new User(loginSession, userAccount, coroutineRunner);

                var guid = Guid.NewGuid().ToString("N");
                users[1]
                    .Registerv2(
                        string.Format("testeraccelbyte+sdk{0}@gmail.com", guid),
                        SearchQuery,
                        password,
                        "testuser2" + guid,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered testuser2");
                registerUserResponses[1] = registerResult.Value;
            }

            // User 2.
            {
                Result<RegisterUserResponse> registerResult = null;
                LoginSession loginSession = new LoginSession(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    AccelBytePlugin.Config.RedirectUri,
                    httpWorker,
                    coroutineRunner);

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    httpWorker);

                users[2] = new User(loginSession, userAccount, coroutineRunner);

                var guid = Guid.NewGuid().ToString("N");
                users[2]
                    .Registerv2(
                        string.Format("testeraccelbyte+sdk{0}@gmail.com", guid),
                        (string.Format("testeraccelbytesdk{0}", guid)).Substring(0, 48),
                        password,
                        SearchQuery,
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);
                yield return TestHelper.WaitForValue(() => registerResult);

                TestHelper.LogResult(registerResult, "Setup: Registered testuser3");
                registerUserResponses[2] = registerResult.Value;
            }

            string[] emailOrder = {"First", "Second", "Third"};
            for (int i = 0; i < users.Length; i++)
            {
                Result loginResult = null;
                users[i]
                    .LoginWithUsername(registerUserResponses[i].emailAddress, password, result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                TestHelper.LogResult(loginResult, string.Format("Login With {0} Email", emailOrder[i]));
            }

            //Act
            Result<PagedPublicUsersInfo>[] userData = new Result<PagedPublicUsersInfo>[3];
            users[0]
                .SearchUsers(SearchQuery, SearchType.USERNAME, result => userData[1] = result);
            yield return TestHelper.WaitForValue(() => userData[1]);

            TestHelper.LogResult(userData[1], "Search UserData of Second Email with Username");

            users[0]
                .SearchUsers(
                    SearchQuery,
                    SearchType.DISPLAYNAME,
                    result => userData[2] = result);
            yield return TestHelper.WaitForValue(() => userData[2]);

            TestHelper.LogResult(userData[2], "Search UserData of Third Email with Display Name");


            //Assert
            Result[] deleteResultUser = new Result[3];
            for (int i = 0; i < deleteResultUser.Length; i++)
            {
                deleteResultUser[i] = null;
                helper.DeleteUser(users[i], result => deleteResultUser[i] = result);
                yield return TestHelper.WaitForValue(() => deleteResultUser[i]);

                TestHelper.LogResult(deleteResultUser[i], string.Format("Delete {0} Email", emailOrder[i]));
            }

            Result userLogoutResult = null;
            users[0].Logout(result => userLogoutResult = result);
            yield return TestHelper.WaitForValue(() => userLogoutResult);

            TestHelper.Assert.IsResultOk(userData[1]);
            TestHelper.Assert.That(userData[1].Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.That(userData[1].Value.data[0].userName, Is.EqualTo(SearchQuery));
            TestHelper.Assert.IsResultOk(userData[2]);
            TestHelper.Assert.That(userData[2].Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.That(userData[2].Value.data[0].displayName, Is.EqualTo(SearchQuery));
            TestHelper.Assert.IsResultOk(deleteResultUser[0]);
            TestHelper.Assert.IsResultOk(deleteResultUser[1]);
            TestHelper.Assert.IsResultOk(deleteResultUser[2]);
        }

#if !DISABLESTEAMWORKS
        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator BulkGetUserBySteamIds_SteamAccount_Success()
        {
            //Arrange
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();

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
            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            Result deviceLoginResult = null;
            Debug.Log("Login With DeviceID");
            user.LoginWithDeviceId(result => deviceLoginResult = result);
            yield return TestHelper.WaitForValue(() => deviceLoginResult);

            List<string> steamIds = new List<string>();
            steamIds.Add(SteamUser.GetSteamID().ToString());
            for (int i = 0; i < SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate); i++)
            {
                steamIds.Add(SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate).ToString());
            }

            //Act
            Result<BulkPlatformUserIdResponse> getOtherUserDataResult = null;
            user.BulkGetUserByOtherPlatformUserIds(
                PlatformType.Steam,
                steamIds.ToArray(),
                result => getOtherUserDataResult = result);
            yield return TestHelper.WaitForValue(() => getOtherUserDataResult);

            foreach (var platformUser in getOtherUserDataResult.Value.userIdPlatforms)
            {
                Result<PublicUserData> dataResult = null;
                user.GetUserByUserId(
                    platformUser.userId,
                    result =>
                    {
                        TestHelper.Assert.IsResultOk(result);
                        dataResult = result;
                    });

                yield return TestHelper.WaitForValue(() => dataResult);
            }

            //Cleanup
            Result deleteResult = null;
            helper.DeleteUser(steamUserId, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            //Assert
            TestHelper.Assert.IsResultOk(getOtherUserDataResult);
            TestHelper.Assert.That(getOtherUserDataResult.Value.userIdPlatforms.Length > 0);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator GetCountryFromIP_Success()
        {
            Result<CountryInfo> countryResult = null;
            var user = AccelBytePlugin.GetUser();

            user.GetCountryFromIP(result => countryResult = result);
            yield return TestHelper.WaitForValue(() => countryResult);

            TestHelper.LogResult(countryResult, "Get country from IP");

            TestHelper.Assert.IsResultOk(countryResult, "Get country from IP failed.");
            TestHelper.Assert.That(countryResult.Value.countryCode, Is.Not.Null);
            TestHelper.Assert.That(countryResult.Value.countryName, Is.Not.Null);
            TestHelper.Assert.That(countryResult.Value.state, Is.Not.Null);
            TestHelper.Assert.That(countryResult.Value.city, Is.Not.Null);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LinkUnlinkSteamAccount()
        {
            //Arrange
            var helper = new TestHelper();

            var user = AccelBytePlugin.GetUser();
            Result steamLoginResult = null;
            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, result => steamLoginResult = result);
            yield return TestHelper.WaitForValue(() => steamLoginResult);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            Result steamLogoutResult = null;
            user.Logout(result => steamLogoutResult = result);
            yield return TestHelper.WaitForValue(() => steamLogoutResult);

            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Result<RegisterUserResponse> registerResult = null;
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk" + guid,
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithUsername(email, password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");

            //Act

            Result linkResult = null;
            user.LinkOtherPlatform(PlatformType.Steam, steamTicket, result => linkResult = result);
            yield return TestHelper.WaitForValue(() => linkResult);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            loginResult = null;
            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result<UserData> getDataResult = null;
            user.GetData(r => getDataResult = r);
            yield return TestHelper.WaitForValue(() => getDataResult);

            Result unlinkResult = null;
            user.UnlinkOtherPlatform(PlatformType.Steam, r => unlinkResult = r);
            yield return TestHelper.WaitForValue(() => unlinkResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            loginResult = null;
            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result<UserData> getDataUnlinkResult = null;
            user.GetData(r => getDataUnlinkResult = r);
            yield return TestHelper.WaitForValue(() => getDataUnlinkResult);

            deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            //Assert
            TestHelper.Assert.IsResultOk(linkResult);
            TestHelper.Assert.IsResultOk(getDataResult);
            TestHelper.Assert.IsResultOk(unlinkResult);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator LinkSteamAccountConflict()
        {
            //Arrange
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();

            Result loginResult = null;
            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";

            Result<RegisterUserResponse> registerResult = null;
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk" + guid,
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Register With Email");

            loginResult = null;
            user.LoginWithUsername(email, password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);

            TestHelper.LogResult(loginResult, "Login");

            //Act
            Result linkResult = null;
            user.LinkOtherPlatform(PlatformType.Steam, steamTicket, result => linkResult = result);
            yield return TestHelper.WaitForValue(() => linkResult);

            AccountLinkConfictMessageVariables messageVariables = null;
            if (linkResult.IsError && linkResult.Error.Code == ErrorCode.PlatformAlreadyLinked)
            {
                messageVariables = linkResult.Error.messageVariables.ToJsonString()
                    .ToObject<AccountLinkConfictMessageVariables>();
            }

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            //Assert
            TestHelper.Assert.IsTrue(linkResult.IsError);
            Debug.Log($"link steam conflict error code {linkResult.Error.Code}");
            TestHelper.Assert.IsTrue(linkResult.Error.Code == ErrorCode.PlatformAlreadyLinked);
            TestHelper.Assert.IsFalse(messageVariables == null);
        }

        [UnityTest, TestLog, Timeout(150000), Order(0)]
        public IEnumerator ForcedLinkSteamAccount()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();

            
            //Arrange
            Result loginResult = null;
            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            Result logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";
            Result<RegisterUserResponse> registerResult = null;
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk" + guid,
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            TestHelper.LogResult(registerResult, "Register With Email");

            loginResult = null;
            user.LoginWithUsername(email, password, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);
            TestHelper.LogResult(loginResult, "Login");

            string steamUserId = "";
            if (SteamManager.Initialized)
            {
                steamUserId = SteamUser.GetSteamID().m_SteamID.ToString();
            }


            //Act: Force link steam account and email account
            Result linkResult = null;
            user.ForcedLinkOtherPlatform(PlatformType.Steam, steamUserId, result => linkResult = result);
            yield return TestHelper.WaitForValue(() => linkResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            loginResult = null;

            user.LoginWithOtherPlatform(PlatformType.Steam, steamTicket, r => loginResult = r);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result<UserData> getDataResult = null;
            user.GetData(r => getDataResult = r);
            yield return TestHelper.WaitForValue(() => getDataResult);


            //Cleanup: Delete user account
            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            logoutResult = null;
            user.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);


            //Assert
            TestHelper.Assert.IsResultOk(linkResult);
            TestHelper.Assert.IsResultOk(getDataResult);
            TestHelper.Assert.That(getDataResult.Value.emailAddress == email);
        }
#endif
    }
}
