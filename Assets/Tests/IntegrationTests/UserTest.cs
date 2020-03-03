// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Text;
using System.Threading;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using Steamworks;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class UserTest
    {
        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithSteam_LoginTwice_SameUserId()
        {
            TestHelper.LogStartTest();
            User user = AccelBytePlugin.GetUser();
            var stringBuilder = new StringBuilder();
            var helper = new TestHelper();
            var attempt = 0;
            var loginSteamResults = new Result[2];
            var steamUserIds = new string[2];
            Result deleteResult = null;

            if (SteamManager.Initialized)
            {
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int) actualTicketLength);

                foreach (byte b in ticket)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            }

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Debug.Log("Login 1 With Steam Ticket: " + stringBuilder);
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                stringBuilder.ToString(),
                result => { loginSteamResults[attempt] = result; });

            while (loginSteamResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginSteamResults[attempt], "Login 1 With Steam Ticket");
            steamUserIds[attempt] = user.Session.UserId;
            user.Logout(null);

            attempt += 1;

            if (SteamManager.Initialized)
            {
                stringBuilder = new StringBuilder();
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int) actualTicketLength);

                foreach (byte b in ticket)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            }

            Debug.Log("Login 2 With Steam Ticket: " + stringBuilder);
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                stringBuilder.ToString(),
                result => { loginSteamResults[attempt] = result; });

            while (loginSteamResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginSteamResults[attempt], "Login 2 With Steam Ticket");
            steamUserIds[attempt] = user.Session.UserId;

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Logout");

            TestHelper.Assert.That(!loginSteamResults[0].IsError);
            TestHelper.Assert.That(!loginSteamResults[1].IsError);
            TestHelper.Assert.That(steamUserIds[0] == steamUserIds[1]);
            TestHelper.Assert.That(!deleteResult.IsError);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithSteam_UniqueUserIdCreated_DifferentUserId()
        {
            TestHelper.LogStartTest();
            var user = AccelBytePlugin.GetUser();
            var stringBuilder = new StringBuilder();
            var helper = new TestHelper();
            int attempt;
            Result[] loginSteamResults = new Result[2];
            Result[] deleteResults = new Result[2];
            string[] steamUserIds = new string[2];

            attempt = 0;

            if (SteamManager.Initialized)
            {
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int) actualTicketLength);

                foreach (byte b in ticket)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            }

            Debug.Log("Login 1 With Steam Ticket");
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                stringBuilder.ToString(),
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
            user.Logout(null);

            attempt += 1;

            if (SteamManager.Initialized)
            {
                stringBuilder = new StringBuilder();
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int) actualTicketLength);

                foreach (byte b in ticket)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            }

            Debug.Log("Login 2 With Steam Ticket");
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                stringBuilder.ToString(),
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

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(!loginSteamResults[0].IsError);
            TestHelper.Assert.That(!loginSteamResults[1].IsError);
            TestHelper.Assert.That(steamUserIds[0] != steamUserIds[1]);
            TestHelper.Assert.That(!deleteResults[0].IsError);
            TestHelper.Assert.That(!deleteResults[1].IsError);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithDevice_LoginTwice_SameUserId()
        {
            TestHelper.LogStartTest();
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            int attempt = 0;
            Result[] loginDeviceResults = new Result[2];
            Result deleteResult = null;
            string[] deviceAccountIds = new string[2];
            Result logoutResult = null;

            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Debug.Log("Login 1 With Device Id");
            user.LoginWithDeviceId(result => { loginDeviceResults[attempt] = result; });

            while (loginDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResults[attempt], "Login 1 With Device Id");
            deviceAccountIds[attempt] = user.Session.UserId;

            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

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

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(!loginDeviceResults[0].IsError);
            TestHelper.Assert.That(!loginDeviceResults[1].IsError);
            TestHelper.Assert.That(deviceAccountIds[0] == deviceAccountIds[1]);
            TestHelper.Assert.That(!deleteResult.IsError);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithDevice_UniqueUserIdCreated_DifferentUserId()
        {
            TestHelper.LogStartTest();
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            int attempt = 0;
            Result[] loginWithDeviceResults = new Result[2];
            Result[] deleteResults = new Result[2];
            string[] deviceAccountUserIds = new string[2];

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
            Result logoutResult = null;

            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

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

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(!loginWithDeviceResults[0].IsError);
            TestHelper.Assert.That(!loginWithDeviceResults[1].IsError);
            TestHelper.Assert.That(deviceAccountUserIds[0] != deviceAccountUserIds[1]);
            TestHelper.Assert.That(!deleteResults[0].IsError);
            TestHelper.Assert.That(!deleteResults[1].IsError);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator UpgradeSteamAccount_ThenLoginWithEmail_Successful()
        {
            TestHelper.LogStartTest();
            var user = AccelBytePlugin.GetUser();
            var stringBuilder = new StringBuilder();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("sdkUpgrade+{0}@example.com", guid);
            const string password = "Password123";
            Result loginSteamResult = null;
            Result<UserData> upgradeResult = null;
            Result loginWithEmailResult = null;
            Result deleteResult = null;

            if (SteamManager.Initialized)
            {
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int) actualTicketLength);

                foreach (byte b in ticket)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            }

            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                stringBuilder.ToString(),
                result => { loginSteamResult = result; });

            while (loginSteamResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginSteamResult, "Login With Steam");
            string steamUserId = user.Session.UserId;
            string oldAccessToken = user.Session.AuthorizationToken;

            user.Upgrade(email, password, result => { upgradeResult = result; });


            while (upgradeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Count");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            user.LoginWithUsername(email, password, result => { loginWithEmailResult = result; });

            while (loginWithEmailResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.Session.UserId;
            string refreshedAccessToken = user.Session.AuthorizationToken;

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(steamUserId == upgradedUserId && steamUserId.Length > 0);
            TestHelper.Assert.That(
                refreshedAccessToken,
                Is.Not.EqualTo(oldAccessToken),
                "Access token isn't refreshed after username and password added to the user's account.");

            TestHelper.Assert.That(!deleteResult.IsError);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator UpgradeDeviceAccount_ThenLoginWithEmail_Successful()
        {
            TestHelper.LogStartTest();
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

            while (loginDeviceResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResult, "Login With Device");
            string deviceUserId = user.Session.UserId;
            string oldAccessToken = user.Session.AuthorizationToken;

            user.Upgrade(email, password, result => { upgradeResult = result; });

            while (upgradeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Account");

            user.Logout(null);

            user.LoginWithUsername(email, password, result => { loginWithEmailResult = result; });

            while (loginWithEmailResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.Session.UserId;
            string refreshedAccessToken = user.Session.AuthorizationToken;

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            user.Logout(null);

            user.LoginWithDeviceId(result => { secondLoginDeviceResult = result; });

            while (secondLoginDeviceResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(secondLoginDeviceResult, "Login With Device 2");
            string secondDeviceUserId = user.Session.UserId;

            helper.DeleteUser(user, result => { secondDeleteResult = result; });

            while (secondDeleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(secondDeleteResult, "Delete User 2");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(
                deviceUserId == upgradedUserId && deviceUserId.Length > 0 && upgradedUserId.Length > 0);

            TestHelper.Assert.That(
                deviceUserId != secondDeviceUserId && deviceUserId.Length > 0 && secondDeviceUserId.Length > 0);

            TestHelper.Assert.That(
                refreshedAccessToken,
                Is.Not.EqualTo(oldAccessToken),
                "Access token isn't refreshed after username and password added to the user's account.");

            TestHelper.Assert.That(!deleteResult.IsError);
            TestHelper.Assert.That(!secondDeleteResult.IsError);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator RegisterWithEmail_ThenLogin_Success()
        {
            TestHelper.LogStartTest();
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

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login With Email");

            Result<UserData> getDataResult = null;
            user.GetData(result => getDataResult = result);

            while (getDataResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(getDataResult, "Get User Data");
            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(getDataResult.Error, Is.Null);

            Result deleteResult = null;

            helper.DeleteUser(user, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete User");
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithEmail_ThenVerify_Success()
        {
            TestHelper.LogStartTest();
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

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });

            while (clientToken == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);

            while (userVerificationCode == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            var verificationCode = userVerificationCode.Value.accountRegistration;

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login With Email");

            Result verificationResult = null;
            user.Verify(verificationCode, result => verificationResult = result);

            while (verificationResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(verificationResult, "Verify");

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(verificationResult.Error, Is.Null);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete");
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator RegisterWithEmail_ThenResetPassword_Success()
        {
            TestHelper.LogStartTest();
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            Result deleteResult = null;
            Result<RegisterUserResponse> registerResult = null;
            Result loginResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";
            string steamAuthTicket;
            var stringBuilder = new StringBuilder();

            if (SteamManager.Initialized)
            {
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int) actualTicketLength);

                foreach (byte b in ticket)
                {
                    stringBuilder.AppendFormat("{0:x2}", b);
                }
            }

            steamAuthTicket = stringBuilder.ToString();
            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register With Email");

            user.LoginWithUsername(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result forgotPasswordResult = null;

            user.SendResetPasswordCode(email, result => forgotPasswordResult = result);

            while (forgotPasswordResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(forgotPasswordResult, "Forgot Password");

            if (forgotPasswordResult.IsError)
            {
                helper.DeleteUser(PlatformType.Steam, steamAuthTicket, result => deleteResult = result);

                while (deleteResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(deleteResult, "Delete User");

                TestHelper.Assert.Fail("Send Reset Password Code");

                yield break;
            }

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });

            while (clientToken == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                user.Session.UserId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);

            while (userVerificationCode == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            var verificationCode = userVerificationCode.Value.passwordReset;
            Debug.Log(
                string.Format("Reset Password, user:{0}, password:{1}, code:{2}", email, password, verificationCode));

            Result resetPasswordResult = null;
            password = "new" + password;

            user.ResetPassword(verificationCode, email, password, result => resetPasswordResult = result);

            while (resetPasswordResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(resetPasswordResult, "Reset Password");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout after reset password");

            loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            helper.DeleteUser(user, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.IsError, Is.False);
            TestHelper.Assert.That(resetPasswordResult.Error, Is.Null);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator RegisterWithEmail_ResendVerificationCode_VerifiedWithLastSentCode()
        {
            TestHelper.LogStartTest();
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

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login With Email");

            Result sendCodeResult = null;
            user.SendVerificationCode(result => sendCodeResult = result);

            while (sendCodeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<TokenData> clientToken = null;
            helper.GetAccessToken(result => { clientToken = result; });

            while (clientToken == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<TestHelper.UserVerificationCode> userVerificationCode = null;
            helper.GetUserVerificationCode(
                registerResult.Value.userId,
                clientToken.Value.access_token,
                result => userVerificationCode = result);

            while (userVerificationCode == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            var verificationCode = userVerificationCode.Value.accountRegistration;

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            Result verificationResult = null;
            user.Verify(verificationCode, result => verificationResult = result);

            while (verificationResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(verificationResult, "Verify");

            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(loginResult.Error, Is.Null);
            TestHelper.Assert.That(verificationResult.Error, Is.Null);

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.LogResult(deleteResult, "Delete");
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator UpdateUser_WithCountry_ReturnsTokenResponseWithCountry()
        {
            TestHelper.LogStartTest();
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();

            Result<RegisterUserResponse> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "AccelbytE123";
            ;

            Debug.Log(string.Format("Register by Publisher:{0}, {1}", email, password));
            user.Register(
                email,
                password,
                "testersdk",
                "ID",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register");

            Debug.Log("Login after register");
            Result loginResult = null;
            user.LoginWithUsername(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login");

            Result<UserData> updateResult = null;
            UpdateUserRequest updateRequest =
                new UpdateUserRequest {country = "US"};

            user.Update(updateRequest, result => updateResult = result);

            while (updateResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(updateResult, "Update User Account");
            Result deleteResult = null;

            helper.DeleteUser(user, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(!registerResult.IsError);
            TestHelper.Assert.That(!loginResult.IsError);
            TestHelper.Assert.That(!updateResult.IsError);
            TestHelper.Assert.That(registerResult.Value.country, !Is.EqualTo("US"));
            TestHelper.Assert.That(updateResult.Value.country, Is.EqualTo("US"));
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator CreateAndGetUserProfiles_WithDefaultFields_Success()
        {
            TestHelper.LogStartTest();
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var profileUpdate = new UpdateUserProfileRequest {firstName = "John", lastName = "Doe", language = "en"};
            Result loginResult = null;
            Result<UserProfile> createProfileResult = null;
            Result<UserProfile> getProfileResult = null;
            Result<UserProfile> updateResult = null;
            Result<UserProfile> getUpdatedProfileResult = null;
            Result deleteResult = null;
            Result deleteProfileResult = null;

            user.LoginWithDeviceId(result => { loginResult = result; });

            while (loginResult == null) yield return new WaitForSeconds(0.1f);

            Debug.Log("Access Token: " + user.Session.AuthorizationToken);
            var userProfiles = AccelBytePlugin.GetUserProfiles();

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

            while (createProfileResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(createProfileResult, "\r\nCreate User Profile");

            userProfiles.GetUserProfile(result => { getProfileResult = result; });

            while (getProfileResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(getProfileResult, "Get User Profile");

            userProfiles.UpdateUserProfile(profileUpdate, result => { updateResult = result; });

            while (updateResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(updateResult, "Update User Profile");

            userProfiles.GetUserProfile(result => { getUpdatedProfileResult = result; });

            while (getUpdatedProfileResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            helper.DeleteUserProfile(user, result => { deleteProfileResult = result; });

            while (deleteProfileResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteProfileResult, "Delete User Profile");

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            Result logoutResult = null;
            user.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(logoutResult, "Logout");

            TestHelper.Assert.That(!createProfileResult.IsError);
            TestHelper.Assert.That(!getProfileResult.IsError);
            TestHelper.Assert.That(!updateResult.IsError);
            TestHelper.Assert.That(!getUpdatedProfileResult.IsError);
            TestHelper.Assert.That(getProfileResult.Value.firstName != getUpdatedProfileResult.Value.firstName);
            TestHelper.Assert.That(!deleteResult.IsError);
            TestHelper.LogEndTest();
        }

        [UnityTest, Timeout(150000)]
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

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login");

            Result<UserData> userData = null;
            user.GetUserByUserId(user.Session.UserId, result => userData = result);

            while (userData == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(userData, "Search UserData with UserId");

            Result deleteResultUser = null;
            helper.DeleteUser(user, result => deleteResultUser = result);

            while (deleteResultUser == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResultUser, "Delete User With Device ID");

            user.Logout(null);


            TestHelper.Assert.That(registerResult.Error, Is.Null);
            TestHelper.Assert.That(userData.Error, Is.Null);
            TestHelper.Assert.That(deleteResultUser.Error, Is.Null);
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator RegisterWithEmail_GetUserDataWithEmailAddress_Success()
        {
            var httpWorker = new UnityHttpWorker();
            var coroutineRunner = new CoroutineRunner();
            var helper = new TestHelper();
            const string password = "AccelbytE123";
            ;

            var users = new User[2];
            var registerUserResponses = new RegisterUserResponse[2];

            for (int i = 0; i < users.Length; i++)
            {
                Result<RegisterUserResponse> registerResult = null;
                ILoginSession loginSession;

                if (AccelBytePlugin.Config.UseSessionManagement)
                {
                    loginSession = new ManagedLoginSession(
                        AccelBytePlugin.Config.LoginServerUrl,
                        AccelBytePlugin.Config.Namespace,
                        AccelBytePlugin.Config.ClientId,
                        AccelBytePlugin.Config.ClientSecret,
                        AccelBytePlugin.Config.RedirectUri,
                        httpWorker);
                }
                else
                {
                    loginSession = new OauthLoginSession(
                        AccelBytePlugin.Config.LoginServerUrl,
                        AccelBytePlugin.Config.Namespace,
                        AccelBytePlugin.Config.ClientId,
                        AccelBytePlugin.Config.ClientSecret,
                        AccelBytePlugin.Config.RedirectUri,
                        httpWorker,
                        coroutineRunner);
                }

                var userAccount = new UserAccount(
                    AccelBytePlugin.Config.IamServerUrl,
                    AccelBytePlugin.Config.Namespace,
                    loginSession,
                    httpWorker);

                users[i] = new User(
                    loginSession,
                    userAccount,
                    coroutineRunner,
                    AccelBytePlugin.Config.UseSessionManagement);

                var guid = Guid.NewGuid().ToString("N");
                users[i]
                    .Register(
                        string.Format("testeraccelbyte+sdk{0}@gmail.com", guid),
                        password,
                        "testuser" + (i + 1),
                        "US",
                        DateTime.Now.AddYears(-22),
                        result => registerResult = result);

                while (registerResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(registerResult, "Setup: Registered testuser" + (i + 1));
                TestHelper.Assert.That(!registerResult.IsError);
                registerUserResponses[i] = registerResult.Value;
            }

            Result loginResult = null;
            users[0].LoginWithUsername(registerUserResponses[0].emailAddress, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login With First Email");

            loginResult = null;
            users[1].LoginWithUsername(registerUserResponses[1].emailAddress, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login With Second Email");

            Result<PagedPublicUsersInfo> userData = null;
            users[0].SearchUsers(registerUserResponses[1].emailAddress, result => userData = result);

            while (userData == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(userData, "Search UserData of Second Email with email address");

            Result deleteResultUserA = null;
            helper.DeleteUser(users[0], result => deleteResultUserA = result);

            while (deleteResultUserA == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResultUserA, "Delete First Email");

            Result deleteResultUserB = null;
            helper.DeleteUser(users[1], result => deleteResultUserB = result);

            while (deleteResultUserB == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResultUserB, "Delete Second Email");

            users[0].Logout(null);

            TestHelper.Assert.That(userData.IsError, Is.False);
            TestHelper.Assert.That(userData.Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.That(!deleteResultUserA.IsError);
            TestHelper.Assert.That(!deleteResultUserB.IsError);
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator GetUserDataWithEmailAddress_NotRegistered_ReturnError()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);

            Result loginDeviceResult = null;
            user.LoginWithDeviceId(result => { loginDeviceResult = result; });

            while (loginDeviceResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResult, "Login With Device ID");

            Result<PagedPublicUsersInfo> usersInfo = null;
            user.SearchUsers(email, result => usersInfo = result);

            while (usersInfo == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(usersInfo, "Get User Data With Not Registered Email");

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            user.Logout(null);

            TestHelper.Assert.That(usersInfo.Value.data.Length, Is.EqualTo(0));
            TestHelper.Assert.That(deleteResult.IsError, Is.False);
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator GetUserDataWithEmailAddress_BadFormat_ReturnError()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("@tester@accelbyte+sdk{0}.com", guid);

            Result loginDeviceResult = null;
            user.LoginWithDeviceId(result => { loginDeviceResult = result; });

            while (loginDeviceResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResult, "Login With Device ID");


            Result<PagedPublicUsersInfo> userData = null;
            user.SearchUsers(email, result => userData = result);

            while (userData == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(userData, "Get User Data With Bad Format Email");

            Result deleteResult = null;
            helper.DeleteUser(user, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            user.Logout(null);

            TestHelper.Assert.That(userData.Value.data.Length, Is.EqualTo(0));
            TestHelper.Assert.That(!deleteResult.IsError);
            Debug.Log("============================================");
        }
    }
}