// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
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

            Result<UserData> registerResult = null;
            var user = AccelBytePlugin.GetUser();
            user.Register("johndoe@example.com", "password", "johndoe_example_com",
                result => registerResult = result);

            while (registerResult == null)
            {
                yield return null;
            }

            userAccount.LoginWithUserName("johndoe@example.com", "password",
                result => { emailLoginResult = result; });

            while (emailLoginResult == null)
            {
                yield return null;
            }
            
            TestHelper.Assert(() => Assert.That(!emailLoginResult.IsError));

            var telemetry = AccelBytePlugin.GetTelemetry();
            Result telemetryResult = null;
            
            telemetry.SendEvent(new TelemetryEventTag(), "stringdata", result => { telemetryResult = result; });

            while (telemetryResult == null)
            {
                yield return null;
            }

            TestHelper.Assert(() => Assert.That(telemetryResult.IsError));        

            Result deleteResult = null;
            var helper = new TestHelper();
            helper.DeleteUser(userAccount, result => deleteResult = result);

            while (deleteResult == null)
            {
                yield return null;
            }
            
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
        }

        [Serializable]
        class ClassData
        {
            public int someInt;
            public float someFloat;
            public string someString;
        }
        
        [UnityTest]
        public IEnumerator TestSendEvent_WithClassData_ReturnsOK()
        {
            var userAccount = AccelBytePlugin.GetUser();
            Result emailLoginResult = null;

            Result<UserData> registerResult = null;
            var user = AccelBytePlugin.GetUser();
            user.Register("johndoe@example.com", "password", "johndoe_example_com",
                result => registerResult = result);

            while (registerResult == null)
            {
                yield return null;
            }

            userAccount.LoginWithUserName("johndoe@example.com", "password",
                result => { emailLoginResult = result; });

            while (emailLoginResult == null)
            {
                yield return null;
            }
            
            TestHelper.Assert(() => Assert.That(!emailLoginResult.IsError));

            var telemetry = AccelBytePlugin.GetTelemetry();
            Result telemetryResult = null;
            
            telemetry.SendEvent(new TelemetryEventTag(), new ClassData
            {
                someInt = 7,
                someFloat = 27.0f,
                someString = "someString"
            }, result => { telemetryResult = result; });

            while (telemetryResult == null)
            {
                yield return null;
            }

            TestHelper.Assert(() => Assert.That(!telemetryResult.IsError));        

            Result deleteResult = null;
            var helper = new TestHelper();
            helper.DeleteUser(userAccount, result => deleteResult = result);

            while (deleteResult == null)
            {
                yield return null;
            }
            
            TestHelper.Assert(() => Assert.That(!deleteResult.IsError));
        }
    }
}