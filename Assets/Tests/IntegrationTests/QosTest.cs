// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using Steamworks;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class QosTest
    {
        [UnityTest]
        public IEnumerator GetServerLatencies_QosServersAvailables_ReturnServerLatencies()
        {
            var qos = AccelBytePlugin.GetQos();
            Result<Dictionary<string, int>> latenciesResult = null;
            qos.GetServerLatencies(result => latenciesResult = result);

            while (latenciesResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.Assert.That(latenciesResult.IsError, Is.False);
            TestHelper.Assert.That(latenciesResult.Value.Count, Is.GreaterThan(0));
        }
    }
}