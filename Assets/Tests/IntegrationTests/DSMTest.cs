// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#if !DISABLESTEAMWORKS
#endif

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class DSMTest
    {
        [UnityTest, TestLog, Ignore("Register static / cloud server must be launched via DSM.")]//Ignore this in Editor mode, "need to be run with cli with adding arg -provider=YOUR_PROVIDER_TEST"
        public IEnumerator Register_Static_Server()
        {
            var ds = AccelByteServerPlugin.GetDedicatedServer();
            var dsm = AccelByteServerPlugin.GetDedicatedServerManager();
            Result loginResult = null;
            ds.LoginWithClientCredentials(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result registerResult = null;
            dsm.RegisterServer(7777, result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            TestHelper.Assert.IsResultOk(registerResult);

            yield return new WaitForSeconds(10);

            Result shutdownResult = null;
            dsm.ShutdownServer(true, result => shutdownResult = result);
            yield return TestHelper.WaitForValue(() => shutdownResult);
            TestHelper.Assert.IsResultOk(shutdownResult);
        }

        [UnityTest, TestLog]
        public IEnumerator Register_Local_Server()
        {
            var ds = AccelByteServerPlugin.GetDedicatedServer();
            var dsm = AccelByteServerPlugin.GetDedicatedServerManager();
            Result loginResult = null;
            ds.LoginWithClientCredentials(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result registerResult = null;
            string serverName = TestHelper.GenerateUnique("unity_local_ds_test_");
            dsm.RegisterLocalServer("127.0.0.1", 7777, serverName, result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            yield return new WaitForSeconds(10);

            Result shutdownResult = null;
            dsm.DeregisterLocalServer(result => shutdownResult = result);
            yield return TestHelper.WaitForValue(() => shutdownResult);

            TestHelper.Assert.IsResultOk(registerResult);
            TestHelper.Assert.IsResultOk(shutdownResult);
        }

        [UnityTest, TestLog]
        public IEnumerator Register_Local_Server_WithPublicIP()
        {
            var ds = AccelByteServerPlugin.GetDedicatedServer();
            var dsm = AccelByteServerPlugin.GetDedicatedServerManager();
            Result loginResult = null;
            ds.LoginWithClientCredentials(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result registerResult = null;
            string serverName = TestHelper.GenerateUnique("unity_local_ds_test_");
            dsm.RegisterLocalServer(7777, serverName, result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            yield return new WaitForSeconds(10);

            Result shutdownResult = null;
            dsm.DeregisterLocalServer(result => shutdownResult = result);
            yield return TestHelper.WaitForValue(() => shutdownResult);

            TestHelper.Assert.IsResultOk(registerResult);
            TestHelper.Assert.IsResultOk(shutdownResult);
        }

        [UnityTest, TestLog, Ignore("Register static / cloud server must be launched via DSM.")]//Ignore this in Editor mode, "need to be run with cli with adding arg -provider=YOUR_PROVIDER_TEST"
        public IEnumerator Register_Server_Twice()
        {
            var ds = AccelByteServerPlugin.GetDedicatedServer();
            var dsm = AccelByteServerPlugin.GetDedicatedServerManager();
            Result loginResult = null;
            ds.LoginWithClientCredentials(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result registerResult = null;
            dsm.RegisterServer(7777, result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);
            TestHelper.Assert.IsResultOk(registerResult);

            registerResult = null;
            dsm.RegisterLocalServer("127.0.0.1", 7777, "unity_local_ds_test", result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            Result shutdownResult = null;
            dsm.DeregisterLocalServer(result => shutdownResult = result);
            yield return TestHelper.WaitForValue(() => shutdownResult);

            TestHelper.Assert.That(registerResult.IsError, Is.True);
            TestHelper.Assert.That(shutdownResult.IsError, Is.True);

            shutdownResult = null;
            dsm.ShutdownServer(true, result => shutdownResult = result);
            yield return TestHelper.WaitForValue(() => shutdownResult);
            TestHelper.Assert.IsResultOk(shutdownResult);
        }

        [UnityTest,TestLog, Ignore("Run locally on your own machine, setup the env var value.")]
        public IEnumerator ServerSetup_ParseArgsTest()
        {
            string testRegion = Environment.GetEnvironmentVariable("SDK_TEST_REGION");
            string testProvider = Environment.GetEnvironmentVariable("SDK_TEST_PROVIDER");
            string testGameVersion = Environment.GetEnvironmentVariable("SDK_TEST_GAME_VER");
            testRegion = string.Format("-region={0}", testRegion);
            testProvider = string.Format("-providers={0}", testProvider);
            testGameVersion = string.Format("-game_version={0}", testGameVersion);

            string[] args = new string[3];
            args[0] = testRegion;
            args[1] = testProvider;
            args[2] = testGameVersion;

            ServerSetupData serverSetupData;

            serverSetupData = DedicatedServerManagerApi.ParseCommandLine(args);

            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(serverSetupData.region));
            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(serverSetupData.provider));
            TestHelper.Assert.IsFalse(string.IsNullOrEmpty(serverSetupData.gameVersion));

            yield return null;
        }
    }
}
