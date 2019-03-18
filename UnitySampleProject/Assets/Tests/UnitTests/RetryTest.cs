// Copyright (c) 2018 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.AUnitTests
{
    [TestFixture]
    public class RetryTest
    {
        private int numCalled;

        private IEnumerator<ITask> DoWorkSuccess(Action<IResult> resultCallback)
        {
            this.numCalled++;
            resultCallback(Result.CreateOk());
            yield break;
        }

        [UnityTest]
        public IEnumerator SendRequest_Success_NoRetry()
        {
            this.numCalled = 0;
            var dispatcher = new AsyncTaskDispatcher();

            IResult workResult = null;

            dispatcher.Start(
                Task.Retry(
                    DoWorkSuccess, result => workResult = result, null
                ));

            while (workResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.Assert(() =>Assert.That(!workResult.IsError));
            TestHelper.Assert(() =>Assert.That(this.numCalled, Is.EqualTo(1)));
        }

        private IEnumerator<ITask> DoWorkUnauthorized(Action<IResult> resultCallback)
        {
            this.numCalled++;

            if (this.numCalled == 1)
            {
                resultCallback(Result.CreateError(ErrorCode.Unauthorized, "DoWorkUnauthorized"));
                yield break;
            }

            resultCallback(Result.CreateOk());
        }

//        [UnityTest]
//        public IEnumerator SendRequest_Unauthorized_RefreshThenRetry()
//        {
//            this.numCalled = 0;
//            var dispatcher = new AsyncTaskDispatcher();
//
//            var user = new UserAccount(new TokenResponse
//            {
//                access_token = "somerandomstringasaccesstokenmock",
//                display_name = "some_display_name",
//                @namespace = "somenamespace",
//                expires_in = 1000,
//                permissions = null,
//                refresh_token = "somerandomstringasrefreshtokenmock",
//                user_id = "someuserid"
//            });
//
//            var nextHour = DateTime.UtcNow + TimeSpan.FromHours(1);
//            user.NextRefreshTime = nextHour;
//
//            IResult workResult = null;
//
//            dispatcher.Start(
//                Task.Retry(
//                    DoWorkUnauthorized, result => workResult = result, user
//                ));
//
//            while (workResult == null)
//            {
//                yield return new WaitForSeconds(0.1f);
//            }
//
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(user.NextRefreshTime < nextHour));
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(!workResult.IsError));
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(this.numCalled, Is.EqualTo(2)));
//        }

        private IEnumerator<ITask> DoWorkServerError(Action<IResult> resultCallback)
        {
            this.numCalled++;

            if (this.numCalled == 1)
            {
                resultCallback(Result.CreateError(ErrorCode.InternalServerError, "DoWorkServerError"));
                yield break;
            }

            resultCallback(Result.CreateOk());
        }

//        [UnityTest]
//        public IEnumerator SendRequest_ServerError_Retry()
//        {
//            this.numCalled = 0;
//            var dispatcher = new AsyncTaskDispatcher();
//
//            var user = new UserAccount(new TokenResponse
//            {
//                access_token = "somerandomstringasaccesstokenmock",
//                display_name = "some_display_name",
//                @namespace = "somenamespace",
//                expires_in = 1000,
//                permissions = null,
//                refresh_token = "somerandomstringasrefreshtokenmock",
//                user_id = "someuserid"
//            });
//
//            var nextHour = DateTime.UtcNow + TimeSpan.FromHours(1);
//            user.NextRefreshTime = nextHour;
//
//            IResult workResult = null;
//
//            dispatcher.Start(
//                Task.Retry(
//                    DoWorkServerError, result => workResult = result, user
//                ));
//
//            while (workResult == null)
//            {
//                yield return new WaitForSeconds(0.1f);
//            }
//
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(user.NextRefreshTime == nextHour));
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(!workResult.IsError));
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(this.numCalled, Is.EqualTo(2)));
//        }

        private IEnumerator<ITask> DoWorkMultipleServerError(Action<IResult> resultCallback)
        {
            this.numCalled++;

            if (this.numCalled < 4)
            {
                resultCallback(Result.CreateError(ErrorCode.InternalServerError, "DoWorkServerError"));
                yield break;
            }

            resultCallback(Result.CreateOk());
        }

//        [UnityTest]
//        [Timeout(10000000)]
//        public IEnumerator SendRequest_MultipleServerError_BackoffRetries()
//        {
//            var dispatcher = new AsyncTaskDispatcher();
//
//            var user = new UserAccount(new TokenResponse
//            {
//                access_token = "somerandomstringasaccesstokenmock",
//                display_name = "some_display_name",
//                @namespace = "somenamespace",
//                expires_in = 1000,
//                permissions = null,
//                refresh_token = "somerandomstringasrefreshtokenmock",
//                user_id = "someuserid"
//            });
//
//            var nextHour = DateTime.UtcNow + TimeSpan.FromHours(1);
//            user.NextRefreshTime = nextHour;
//
//            IResult workResult = null;
//            this.numCalled = 0;
//            var stopwatch = new Stopwatch();
//            stopwatch.Start();
//
//            dispatcher.Start(Task.Retry(DoWorkMultipleServerError, result => workResult = result, user));
//
//            while (workResult == null)
//            {
//                yield return new WaitForSeconds(0.1f);
//            }
//
//            stopwatch.Stop();
//
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(stopwatch.Elapsed, Is.GreaterThan(TimeSpan.FromMilliseconds(5250)))); //min time 0.75 + 1.5 + 3 s
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(user.NextRefreshTime == nextHour));
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(!workResult.IsError));
//            EditorRunTest.Assertion(MethodBase.GetCurrentMethod().DeclaringType.Name, () => Assert.That(this.numCalled, Is.EqualTo(4)));
//        }
    }
}