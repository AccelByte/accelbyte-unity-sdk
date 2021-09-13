// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Time = AccelByte.Models.Time;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class MiscellaneousTest
    {
        [UnityTest, TestLog, Timeout(150000)]
        public IEnumerator GetCurrentTime_NoParams_ReturnsTime()
        {
            var misc = AccelBytePlugin.GetMiscellaneous();

            Result<Time> timeResult = null;
            misc.GetCurrentTime(result => timeResult = result);

            while (timeResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Assert.False(timeResult.IsError);
            Assert.That(timeResult.Value.currentTime, Is.Not.EqualTo(default(DateTime)));
        }
    }
}