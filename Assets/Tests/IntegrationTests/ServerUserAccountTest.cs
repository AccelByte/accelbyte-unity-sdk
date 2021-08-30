// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class ServerUserAccountTest
    {
        private User user;

        [UnityTest, TestLog, Order(0)]
        public IEnumerator A_TestSetup()
        {
            this.user = AccelBytePlugin.GetUser();

            if(this.user.Session.IsValid())
            {
                Result logoutResult = null;
                this.user.Logout(result => logoutResult = result);
                yield return TestHelper.WaitForValue(() => logoutResult, "Wait logout prev session");
            }

            Result loginResult = null;
            user.LoginWithDeviceId(result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult, "Wait login");

            Assert.IsFalse(loginResult.IsError, "Login with Device Failed");
        }

        [UnityTest, TestLog, Order(9999)]
        public IEnumerator Z_TestTeardown()
        {
            Result logoutResult = null;
            this.user.Logout(result => logoutResult = result);
            yield return TestHelper.WaitForValue(() => logoutResult, "Wait logout Teardown");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator B_GetUserData_Ok()
        {
            Result<UserData> getDataResult = null;
            user.GetData(result => getDataResult = result);
            yield return TestHelper.WaitForValue(() => getDataResult, "Waiting get data result");

            Result<UserData> serverGetDataResult = null;
            AccelByteServerPlugin.GetUserAccount().GetUserData(user.Session.AuthorizationToken, result => serverGetDataResult = result);
            yield return TestHelper.WaitForValue(() => serverGetDataResult, "Waiting server get data result");

            Assert.IsFalse(getDataResult.IsError, "Failed to get user data");
            Assert.IsFalse(serverGetDataResult.IsError, "Failed server get user data");
            Assert.AreEqual(getDataResult.Value.userId, serverGetDataResult.Value.userId);
        }
    }
}