// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class ServerGameTelemetryTest
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
            Result serverLoginResult = null;
            var server = AccelByteServerPlugin.GetDedicatedServer();
            server.LoginWithClientCredentials(result => serverLoginResult = result);
            
            while (serverLoginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }
            TestHelper.Assert.IsResultOk(serverLoginResult, "Server login failed");

            var serverGameTelemetry = AccelByteServerPlugin.GetGameTelemetry();
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

            while (telemetryResults.Count < EVENT_COUNT)
            {
                Thread.Sleep(100);
                yield return null;
            }

            foreach (var result in telemetryResults)
            {
                TestHelper.Assert.IsFalse(result.IsError);
            }
        }
        
        [UnityTest, TestLog, Ignore("JIB server can't handle the request")]
        public IEnumerator Send_ImmediateEvent_ReturnsOK()
        {
            Result serverLoginResult = null;
            var server = AccelByteServerPlugin.GetDedicatedServer();
            server.LoginWithClientCredentials(result => serverLoginResult = result);
            
            while (serverLoginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }
            TestHelper.Assert.IsResultOk(serverLoginResult, "Server login failed");

            const string CURRENT_EVENTNAME = "Send_ImmediateEvent_ReturnsOK";
            var serverGameTelemetry = AccelByteServerPlugin.GetGameTelemetry();
            serverGameTelemetry.SetBatchFrequency(TimeSpan.FromSeconds(6));
            serverGameTelemetry.SetImmediateEventList(new List<string>{CURRENT_EVENTNAME});

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

            while (telemetryResults.Count < EVENT_COUNT)
            {
                Thread.Sleep(100);
                yield return null;
            }

            foreach (var result in telemetryResults)
            {
                TestHelper.Assert.IsFalse(result.IsError);
            }
        }
    }
}