// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

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
    public class GameTelemetryTest
    {
        class PayloadSample
        {
            public string someString;
            public float someFloat;
            public int someInt;
            public bool someBool;
        }
        
        [UnityTest, TestLog, Ignore("JIB server can't handle the request")]
        public IEnumerator Send_BatchTelemetryEvent_ReturnsOK()
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
            yield return TestHelper.WaitForValue(() => registerResult);

            userAccount.LoginWithUsername(
                "testeraccelbyte@example.com",
                "Password123",
                result => { emailLoginResult = result; }); 
            yield return TestHelper.WaitForValue(() => emailLoginResult);

            if (emailLoginResult.Error == null || emailLoginResult.Error.Code != ErrorCode.InvalidRequest)
            {
                TestHelper.Assert.IsResultOk(emailLoginResult, "Login password failed.");
            }


            var serverGameTelemetry = AccelBytePlugin.GetGameTelemetry();
            serverGameTelemetry.SetBatchFrequency(TimeSpan.FromSeconds(6));
            serverGameTelemetry.SetImmediateEventList(new List<string>());

            const int EVENT_COUNT = 999;

            List<Result> telemetryResults = new List<Result>();
            for (int i = 0; i < EVENT_COUNT; i++)
            {
                TelemetryBody payload = new TelemetryBody();
                payload.Payload = new PayloadSample
                {
                    someInt = i,
                    someBool = true,
                    someFloat = 4.04f,
                    someString = "this is a string"
                };
                payload.EventName = "Send_BatchTelemetryEvent_ReturnsOK";
                payload.EventNamespace = "sdkTest";
                
                serverGameTelemetry.Send(payload, result => { telemetryResults.Add(result); });
            }

            yield return TestHelper.WaitForValue(() => telemetryResults.Count < EVENT_COUNT);

            foreach (var result in telemetryResults)
            {
                TestHelper.Assert.IsFalse(result.IsError);
            }

            Result deleteResult = null;
            var helper = new TestHelper();
            helper.DeleteUser(userAccount, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            Result logoutResult = null;
            userAccount.Logout(r => logoutResult = r);
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.Assert.IsResultOk(deleteResult, "Delete error");
            TestHelper.Assert.IsResultOk(logoutResult, "Logout error");
        }
        
        [UnityTest, TestLog, Ignore("JIB server can't handle the request")]
        public IEnumerator Send_ImmediateEvent_ReturnsOK()
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
            yield return TestHelper.WaitForValue(() => registerResult);

            userAccount.LoginWithUsername(
                "testeraccelbyte@example.com",
                "Password123",
                result => { emailLoginResult = result; });
            yield return TestHelper.WaitForValue(() => emailLoginResult);

            TestHelper.Assert.IsResultOk(emailLoginResult, "Login password failed.");

            const string CURRENT_EVENTNAME = "Send_ImmediateEvent_ReturnsOK";
            var serverGameTelemetry = AccelBytePlugin.GetGameTelemetry();
            serverGameTelemetry.SetBatchFrequency(TimeSpan.FromSeconds(6));
            serverGameTelemetry.SetImmediateEventList(new List<string> {CURRENT_EVENTNAME});

            const int EVENT_COUNT = 1;

            List<Result> telemetryResults = new List<Result>();
            for (int i = 0; i < EVENT_COUNT; i++)
            {
                TelemetryBody payload = new TelemetryBody();
                payload.Payload = new PayloadSample
                {
                    someInt = i,
                    someBool = true,
                    someFloat = 4.04f,
                    someString = "this is a string"
                };
                payload.EventName = CURRENT_EVENTNAME;
                payload.EventNamespace = "sdkTest";
                
                serverGameTelemetry.Send(payload, result => { telemetryResults.Add(result); });
            }

            yield return TestHelper.WaitForValue(() => telemetryResults.Count < EVENT_COUNT);

            foreach (var result in telemetryResults)
            {
                TestHelper.Assert.IsFalse(result.IsError);
            }

            Result deleteResult = null;
            var helper = new TestHelper();
            helper.DeleteUser(userAccount, result => deleteResult = result); 
            yield return TestHelper.WaitForValue(() => deleteResult);

            Result logoutResult = null;
            userAccount.Logout(r => logoutResult = r);

            Debug.Log("Logging out..."); 
            yield return TestHelper.WaitForValue(() => logoutResult);

            TestHelper.Assert.IsResultOk(deleteResult, "Delete error");
            TestHelper.Assert.IsResultOk(logoutResult, "Logout error");
        }
    }
}