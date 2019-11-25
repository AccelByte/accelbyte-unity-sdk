// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Runtime.Serialization;
using System.Threading;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class TelemetryTest
    {
        [UnityTest]
        public IEnumerator TestSendEvent_WithStringData_ReturnsError()
        {
            var userAccount = AccelBytePlugin.GetUser();
            Result emailLoginResult = null;

            Result<RegisterUserResponse> registerResult = null;
            var user = AccelBytePlugin.GetUser();
            user.Register(
                "testeraccelbyte@example.com",
                "Password123",
                "testeraccelbyte",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            userAccount.LoginWithUsername(
                "testeraccelbyte@example.com",
                "Password123",
                result => { emailLoginResult = result; });

            while (emailLoginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.That(!emailLoginResult.IsError);

            var telemetry = AccelBytePlugin.GetTelemetry();
            Result telemetryResult = null;

            telemetry.SendEvent(new TelemetryEventTag(), "string data", result => { telemetryResult = result; });

            while (telemetryResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }


            Result deleteResult = null;
            var helper = new TestHelper();
            helper.DeleteUser(userAccount, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result logoutResult = null;
            userAccount.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.That(telemetryResult.IsError);
            TestHelper.Assert.That(deleteResult.IsError, Is.False);
            TestHelper.Assert.That(logoutResult.IsError, Is.False);
        }

        [DataContract]
        public class ClassData
        {
            [DataMember] public int someInt;
            [DataMember] public float someFloat;
            [DataMember] public string someString;
        }

        [UnityTest]
        public IEnumerator TestSendEvent_WithClassData_ReturnsOK()
        {
            var userAccount = AccelBytePlugin.GetUser();
            Result emailLoginResult = null;

            Result<RegisterUserResponse> registerResult = null;
            var user = AccelBytePlugin.GetUser();
            user.Register(
                "testeraccelbyte@example.com",
                "Password123",
                "testeraccelbyte",
                "US",
                DateTime.Now.AddYears(-22),
                result => registerResult = result);

            while (registerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            userAccount.LoginWithUsername(
                "testeraccelbyte@example.com",
                "Password123",
                result => { emailLoginResult = result; });

            while (emailLoginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.That(!emailLoginResult.IsError);

            var telemetry = AccelBytePlugin.GetTelemetry();
            Result telemetryResult = null;

            telemetry.SendEvent(
                new TelemetryEventTag(),
                new ClassData {someInt = 7, someFloat = 27.0f, someString = "someString"},
                result => { telemetryResult = result; });

            while (telemetryResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }


            Result deleteResult = null;
            var helper = new TestHelper();
            helper.DeleteUser(userAccount, result => deleteResult = result);

            while (deleteResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result logoutResult = null;
            userAccount.Logout(r => logoutResult = r);

            while (logoutResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.That(telemetryResult.IsError, Is.False);
            TestHelper.Assert.That(deleteResult.IsError, Is.False);
            TestHelper.Assert.That(logoutResult.IsError, Is.False);
        }
    }
}