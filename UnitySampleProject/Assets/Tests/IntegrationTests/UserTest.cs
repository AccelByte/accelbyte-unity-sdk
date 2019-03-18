// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenPop.Mime;
using OpenPop.Pop3;
using Steamworks;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class UserTest
    {
        private static readonly string PublisherNamespace = Environment.GetEnvironmentVariable("PUBLISHER_NAMESPACE");

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true; // force the validation of any certificate
        }

        private string GetEmailVerificationCode(string email)
        {
            Message message = null;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (Pop3Client client = new Pop3Client())
            {
                while (message == null && stopwatch.ElapsedMilliseconds < 100000)
                {
                    Debug.Log("Sleep for 0.5s, wait for new incoming email.");
                    Thread.Sleep(500);

                    client.Connect(
                        "pop.gmail.com",
                        995,
                        true,
                        60000,
                        60000,
                        UserTest.ValidateServerCertificate);
                    client.Authenticate("testeraccelbyte@gmail.com", "RoNINkETYpHOaDRo");

                    int messageCount = client.GetMessageCount();

                    for (int i = 1; i <= messageCount; i++)
                    {
                        var header = client.GetMessageHeaders(i);

                        if (header.To[0].MailAddress.Address == email)
                        {
                            Debug.Log("Get Email Message With Subject: " + header.Subject);
                            message = client.GetMessage(i);
                        }
                    }

                    client.Disconnect();
                }
            }

            if (message == null)
            {
                return null;
            }

            var msgBuilder = new StringBuilder();

            foreach (var messagePart in message.FindAllTextVersions())
            {
                msgBuilder.Append(messagePart.GetBodyAsText());
            }

            Regex regex = new Regex("[^\\w\\d](\\d{6})[^\\w\\d]");
            var verificationCode = regex.Match(msgBuilder.ToString()).Groups[1].Value;

            return verificationCode;
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithSteam_LoginTwice_SameUserId()
        {
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
            steamUserIds[attempt] = user.UserId;
            user.Logout();

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
            steamUserIds[attempt] = user.UserId;

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");
            user.Logout();

            TestHelper.Assert(() => Assert.That(!loginSteamResults[0].IsError));
            TestHelper.Assert(() => Assert.That(!loginSteamResults[1].IsError));
            TestHelper.Assert(() => Assert.That(steamUserIds[0] == steamUserIds[1]));
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithSteam_UniqueUserIdCreated_DifferentUserId()
        {
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
            steamUserIds[attempt] = user.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 2");
            user.Logout();

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
            steamUserIds[attempt] = user.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 2");
            user.Logout();

            TestHelper.Assert(() => Assert.That(!loginSteamResults[0].IsError));
            TestHelper.Assert(() => Assert.That(!loginSteamResults[1].IsError));
            TestHelper.Assert(() => Assert.That(steamUserIds[0] != steamUserIds[1]));
            TestHelper.Assert(() => Assert.That(!deleteResults[0].IsError));
            TestHelper.Assert(() => Assert.That(!deleteResults[1].IsError));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithDevice_LoginTwice_SameUserId()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            int attempt = 0;
            Result[] loginDeviceResults = new Result[2];
            Result deleteResult = null;
            string[] deviceAccountIds = new string[2];

            user.Logout();
            Debug.Log("Login 1 With Device Id");
            user.LoginWithDeviceId(result => { loginDeviceResults[attempt] = result; });

            while (loginDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResults[attempt], "Login 1 With Device Id");
            deviceAccountIds[attempt] = user.UserId;

            user.Logout();

            attempt += 1;
            Debug.Log("Login 2 With Device Id");
            user.LoginWithDeviceId(result => { loginDeviceResults[attempt] = result; });

            while (loginDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginDeviceResults[attempt], "Login 2 With Device Id");
            deviceAccountIds[attempt] = user.UserId;

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");
            user.Logout();

            TestHelper.Assert(() => Assert.That(!loginDeviceResults[0].IsError));
            TestHelper.Assert(() => Assert.That(!loginDeviceResults[1].IsError));
            TestHelper.Assert(() => Assert.That(deviceAccountIds[0] == deviceAccountIds[1]));
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithDevice_UniqueUserIdCreated_DifferentUserId()
        {
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
            deviceAccountUserIds[attempt] = user.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 1");
            user.Logout();

            attempt += 1;
            Debug.Log("Login 2 With Device Id");
            user.LoginWithDeviceId(result => { loginWithDeviceResults[attempt] = result; });

            while (loginWithDeviceResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginWithDeviceResults[attempt], "Login 2 With Device Id");
            deviceAccountUserIds[attempt] = user.UserId;

            helper.DeleteUser(user, result => { deleteResults[attempt] = result; });

            while (deleteResults[attempt] == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResults[attempt], "Delete User 2");
            user.Logout();

            TestHelper.Assert(() => Assert.That(!loginWithDeviceResults[0].IsError));
            TestHelper.Assert(() => Assert.That(!loginWithDeviceResults[1].IsError));
            TestHelper.Assert(() => Assert.That(deviceAccountUserIds[0] != deviceAccountUserIds[1]));
            TestHelper.Assert(() => Assert.That(!deleteResults[0].IsError));
            TestHelper.Assert(() => Assert.That(!deleteResults[1].IsError));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator UpgradeSteamAccount_ThenLoginWithEmail_Successful()
        {
            var user = AccelBytePlugin.GetUser();
            var stringBuilder = new StringBuilder();
            var helper = new TestHelper();
            const string Email = "sdkUpgrade@example.com";
            const string Password = "pass";
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
            string steamUserId = user.UserId;
            string oldAccessToken = user.AccessToken;

            user.Upgrade(Email, Password, result => { upgradeResult = result; });

            while (upgradeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Count");

            user.Logout();

            user.LoginWithUserName(Email, Password, result => { loginWithEmailResult = result; });

            while (loginWithEmailResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.UserId;
            string refreshedAccessToken = user.AccessToken;

            helper.DeleteUser(AccelBytePlugin.Config.Namespace, Email, Password, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            user.Logout();

            TestHelper.Assert(() => Assert.That(steamUserId == upgradedUserId && steamUserId.Length > 0));
            TestHelper.Assert(
                () => Assert.AreNotEqual(
                    refreshedAccessToken,
                    oldAccessToken,
                    "Access token isn't refreshed after username and password added to the user's account."));
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator UpgradeSteamAccountWithVerificationCode_ThenLoginWithEmail_Successful()
        {
            var user = AccelBytePlugin.GetUser();
            var stringBuilder = new StringBuilder();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            const string Password = "pass";
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

            user.Logout();
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
            string steamUserId = user.UserId;
            string oldAccessToken = user.AccessToken;

            Result sendCodeResult = null;

            user.SendUpgradeVerificationCode(email, result => sendCodeResult = result);

            while (sendCodeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(sendCodeResult, "Send verification code to email");

            Debug.Log("Getting email messages with POP client");
            var verificationCode = GetEmailVerificationCode(email);

            user.UpgradeAndVerify(email, Password, verificationCode, result => { upgradeResult = result; });

            while (upgradeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Count");

            user.Logout();

            user.LoginWithUserName(email, Password, result => { loginWithEmailResult = result; });

            while (loginWithEmailResult == null) { yield return new WaitForSeconds(0.1f); }

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.UserId;
            string refreshedAccessToken = user.AccessToken;

            helper.DeleteUser(UserTest.PublisherNamespace, email, Password, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            user.Logout();

            TestHelper.Assert(() => Assert.That(steamUserId == upgradedUserId && steamUserId.Length > 0));
            TestHelper.Assert(
                () => Assert.AreNotEqual(
                    refreshedAccessToken,
                    oldAccessToken,
                    "Access token isn't refreshed after username and password added to the user's account."));
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator UpgradeDeviceAccount_ThenLoginWithEmail_Successful()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            const string Password = "pass";
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
            string deviceUserId = user.UserId;
            string oldAccessToken = user.AccessToken;

            user.Upgrade(email, Password, result => { upgradeResult = result; });

            while (upgradeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(upgradeResult, "Upgrade Headless Account");

            user.Logout();

            user.LoginWithUserName(email, Password, result => { loginWithEmailResult = result; });

            while (loginWithEmailResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginWithEmailResult, "Login With Email");
            string upgradedUserId = user.UserId;
            string refreshedAccessToken = user.AccessToken;

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            user.Logout();

            user.LoginWithDeviceId(result => { secondLoginDeviceResult = result; });

            while (secondLoginDeviceResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(secondLoginDeviceResult, "Login With Device 2");
            string secondDeviceUserId = user.UserId;

            helper.DeleteUser(user, result => { secondDeleteResult = result; });

            while (secondDeleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(secondDeleteResult, "Delete User 2");

            user.Logout();

            TestHelper.Assert(
                () => Assert.That(
                    deviceUserId == upgradedUserId && deviceUserId.Length > 0 && upgradedUserId.Length > 0));
            TestHelper.Assert(
                () => Assert.That(
                    deviceUserId != secondDeviceUserId && deviceUserId.Length > 0 && secondDeviceUserId.Length > 0));
            TestHelper.Assert(
                () => Assert.AreNotEqual(
                    refreshedAccessToken,
                    oldAccessToken,
                    "Access token isn't refreshed after username and password added to the user's account."));
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
            TestHelper.Assert(() => Assert.That(!secondDeleteResult.IsError));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator RegisterWithEmail_ThenLogin_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<UserData> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "accelbyte";

            Debug.Log(string.Format("Register With Email: {0}, {1}", email, password));
            user.Register(email, password, "testeraccelbyte+sdk" + guid, result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register With Email");

            Result loginResult = null;
            user.LoginWithUserName(email, password, result => loginResult = result);

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
            TestHelper.Assert(() => Assert.That(registerResult.Error, Is.Null));
            TestHelper.Assert(() => Assert.That(loginResult.Error, Is.Null));
            TestHelper.Assert(() => Assert.That(getDataResult.Error, Is.Null));

            Result deleteResult = null;

            helper.DeleteUser(AccelBytePlugin.Config.Namespace, email, password, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            user.Logout();

            TestHelper.LogResult(deleteResult, "Delete User");
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator LoginWithEmail_ThenVerify_Success()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();
            Result<UserData> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "accelbyte";

            Debug.Log(string.Format("Register With Email:{0}, {1}", email, password));
            user.Register(email, password, "testeraccelbyte+sdk" + guid, result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register With Email");
            Result loginResult = null;

            Debug.Log(string.Format("Login With Email:{0}, {1}", email, password));
            user.LoginWithUserName(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login With Email");

            Result sendCodeResult = null;
            Debug.Log("Send Verification Code to Email");
            user.SendVerificationCode(result => sendCodeResult = result);

            while (sendCodeResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Send Verification Code to Email");

            Debug.Log("Getting email messages with POP client");

            var verificationCode = GetEmailVerificationCode(email);

            Debug.Log(
                string.Format(
                    "Verify Email: {0}, password: {1}, verificationCode: {2}",
                    email,
                    password,
                    verificationCode));

            loginResult = null;
            user.LoginWithUserName(email, password, result => loginResult = result);

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

            TestHelper.Assert(() => Assert.That(registerResult.Error, Is.Null));
            TestHelper.Assert(() => Assert.That(loginResult.Error, Is.Null));
            TestHelper.Assert(() => Assert.That(verificationResult.Error, Is.Null));

            Result deleteResult = null;
            helper.DeleteUser(AccelBytePlugin.Config.Namespace, email, password, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            user.Logout();

            TestHelper.LogResult(deleteResult, "Delete");
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator RegisterWithEmail_ThenResetPassword_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            Result deleteResult = null;
            Result<UserData> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "accelbyte";
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
            user.Register(email, password, "testeraccelbyte+sdk" + guid, result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register With Email");

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
                helper.DeleteUser(
                    AccelBytePlugin.Config.Namespace,
                    PlatformType.Steam,
                    steamAuthTicket,
                    result => deleteResult = result);

                while (deleteResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(deleteResult, "Delete User");

                TestHelper.Assert(() => Assert.Fail("Send Reset Password Code"));

                yield break;
            }

            var verificationCode = GetEmailVerificationCode(email);
            Debug.Log(
                string.Format(
                    "Reset Email Password: {0}, password: {1}, reset code: {2}",
                    email,
                    password,
                    verificationCode));
            Result resetPasswordResult = null;

            user.ResetPassword(verificationCode, email, "new " + password, result => resetPasswordResult = result);

            while (resetPasswordResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            user.Logout();

            TestHelper.LogResult(resetPasswordResult, "Reset Password");

            helper.DeleteUser(
                AccelBytePlugin.Config.Namespace,
                email,
                "new " + password,
                result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User");

            user.Logout();

            TestHelper.Assert(() => Assert.That(registerResult.Error, Is.Null));
            TestHelper.Assert(() => Assert.That(resetPasswordResult.Error, Is.Null));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator UpdateUser_WithCountry_ReturnsTokenResponseWithCountry()
        {
            var helper = new TestHelper();
            var user = AccelBytePlugin.GetUser();

            Result<UserData> registerResult = null;
            var guid = Guid.NewGuid().ToString("N");
            string email = string.Format("testeraccelbyte+sdk{0}@gmail.com", guid);
            string password = "accelbyte";

            Debug.Log(string.Format("Register by Publisher:{0}, {1}", email, password));
            user.Register(email, password, "testeraccelbyte+sdk" + guid, result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(registerResult, "Register");

            Debug.Log("Login after register");
            Result loginResult = null;
            user.LoginWithUserName(email, password, result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(loginResult, "Login");

            Result<UserData> updateResult = null;
            UpdateUserRequest updateRequest = new UpdateUserRequest {Country = "US"};

            user.Update(updateRequest, result => updateResult = result);

            while (updateResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(updateResult, "Update User Account");
            Result deleteResult = null;

            helper.DeleteUser(AccelBytePlugin.Config.Namespace, email, password, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete");

            user.Logout();

            TestHelper.Assert(() => Assert.That(!registerResult.IsError));
            TestHelper.Assert(() => Assert.That(!loginResult.IsError));
            TestHelper.Assert(() => Assert.That(!updateResult.IsError));
            TestHelper.Assert(() => Assert.That(string.IsNullOrEmpty(registerResult.Value.Country)));
            TestHelper.Assert(() => Assert.That(updateResult.Value.Country, Is.EqualTo("US")));
            Debug.Log("============================================");
        }

        [UnityTest, Timeout(150000)]
        public IEnumerator CreateAndGetUserProfiles_WithDefaultFields_Success()
        {
            var user = AccelBytePlugin.GetUser();
            var helper = new TestHelper();
            var profileUpdate = new UpdateUserProfileRequest {firstName = "John", lastName = "Doe", language = "en"};
            Result loginResult = null;
            Result<UserProfile> createProfileResult = null;
            Result<UserProfile> getProfileResult = null;
            Result<UserProfile> updateResult = null;
            Result<UserProfile> getUpdatedProfileResult = null;
            Result deleteResult = null;

            user.LoginWithDeviceId(result => { loginResult = result; });

            while (loginResult == null) { yield return new WaitForSeconds(0.1f); }

            Debug.Log("Access Token: " + user.AccessToken);
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

            helper.DeleteUser(user, result => { deleteResult = result; });

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(deleteResult, "Delete User Profile");

            user.Logout();

            TestHelper.Assert(() => Assert.That(!createProfileResult.IsError));
            TestHelper.Assert(() => Assert.That(!getProfileResult.IsError));
            TestHelper.Assert(() => Assert.That(!updateResult.IsError));
            TestHelper.Assert(() => Assert.That(!getUpdatedProfileResult.IsError));
            TestHelper.Assert(
                () => Assert.That(getProfileResult.Value.firstName != getUpdatedProfileResult.Value.firstName));
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
            Debug.Log("============================================");
        }
    }
}