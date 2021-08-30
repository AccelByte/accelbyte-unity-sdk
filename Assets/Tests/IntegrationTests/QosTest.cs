// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#if !DISABLESTEAMWORKS
#endif

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class QosTest
    {
        [UnityTest, TestLog]
        public IEnumerator GetServerLatencies_QosServersAvailables_ReturnServerLatencies_Client()
        {
            var qos = AccelBytePlugin.GetQos();
            Result<Dictionary<string, int>> latenciesResult = null;
            qos.GetServerLatencies(result => latenciesResult = result); 
            yield return TestHelper.WaitForValue(() => latenciesResult);

            TestHelper.Assert.IsResultOk(latenciesResult);
            TestHelper.Assert.That(latenciesResult.Value.Count, Is.GreaterThan(0));
        }

        [UnityTest, TestLog]
        public IEnumerator GetServerLatencies_QosServersAvailables_ReturnServerLatencies_Server()
        {
            var qos = AccelByteServerPlugin.GetQos();
            Result<Dictionary<string, int>> latenciesResult = null;
            qos.GetServerLatencies(result => latenciesResult = result); 
            yield return TestHelper.WaitForValue(() => latenciesResult);

            foreach(KeyValuePair<string, int> latency in latenciesResult.Value)
            {
                Debug.Log("Region: " + latency.Key + " | Latency: " + latency.Value);
            }

            TestHelper.Assert.IsResultOk(latenciesResult);
            TestHelper.Assert.That(latenciesResult.Value.Count, Is.GreaterThan(0));
        }
    }
}