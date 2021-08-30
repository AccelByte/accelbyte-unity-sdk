// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine.TestTools;

// Copy from CloudSaveTest.cs
namespace Tests.IntegrationTests
{
    [TestFixture]
    public class ServerCloudSaveTest
    {
        private User user;
        private TestHelper helper;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        
        string keyUserTest = "ServerUnitySDKKeyUserTest";
        string keyUserPublic = "ServerUnitySDKKeyUserPublicTest";
        string keyGameTest = "ServerUnitySDKKeyGameTest";
        string keyReplaceTest = "ServerUnitySDKKeyReplaceTest";
        string keySaveTest = "ServerUnitySDKKeySaveTest";

        Dictionary<string, object> record1Test = new Dictionary<string, object>
        {
            {"numRegion", 6 }, {"oilsReserve", 125.10 }, {"islandName", "tartar friendly land" },
            {"buildings", new string[4] { "oilRefinery", "oilWell", "watchTower", "defendsTower" }},
            {"resources", new Dictionary<string, int>{{"gas", 20 }, {"water", 100 }, {"gold", 10 }}}
        };
        Dictionary<string, object> record2Test = new Dictionary<string, object>
        {
            {"numIsland", 2 }, {"waterReserve", 125.10 }, {"countryName", "happyland" },
            {"islands", new string[2] { "smile island", "dance island" }},
            {"population", new Dictionary<string, int>{{"smile island", 198 }, {"dance island", 77 }}}
        };
        Dictionary<string, object> newRecord1Test = new Dictionary<string, object>
        {
            {"numRegion", 10 }, {"oilsReserve", 100 }, {"islandName", "salad friendly land" },
            {"buildings", new string[4] { "gasRefinery", "gasWell", "waterTower", "mainTower" } },
            {"resources", new Dictionary<string, object>{{"gas", 50 }, {"water", 70}, {"gold", 30} }}
        };

        [UnityTest, TestLog, Order(0)]
        public IEnumerator Setup()
        {
            Result loginServerResult = null;
            AccelByteServerPlugin.GetDedicatedServer().LoginWithClientCredentials(result => loginServerResult = result);
            yield return TestHelper.WaitForValue(() => loginServerResult);
            
            this.user = AccelBytePlugin.GetUser();

            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;

                this.user.Logout(r => logoutResult = r);
                yield return TestHelper.WaitForValue(() => logoutResult);

                Assert.IsFalse(logoutResult.IsError, "User cannot log out.");
            }

            Result loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);

            Assert.IsFalse(loginWithDevice.IsError, "User cannot log in with device.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator SaveUserRecord_Success()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Result record1Result = null;
            cloudSave.SaveUserRecord(userId, keyUserTest, record1Test, result => { record1Result = result; });
            yield return TestHelper.WaitForValue(() => record1Result);

            Assert.IsFalse(record1Result.IsError, "Save user 1st record failed.");

            Result record2Result = null;
            cloudSave.SaveUserRecord(userId, keyUserTest, record2Test, result => { record2Result = result; });
            yield return TestHelper.WaitForValue(() => record2Result);

            Assert.IsFalse(record2Result.IsError, "Save user 2nd record failed.");
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetUserRecord_Success()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Result<UserRecord> recordResult = null;
            cloudSave.GetUserRecord(userId, keyUserTest, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(recordResult.Value.key, Is.EqualTo(keyUserTest));
            TestHelper.Assert.That(recordResult.Value.user_id, Is.EqualTo(this.user.Session.UserId));
            TestHelper.Assert.That(recordResult.Value.value["numRegion"], Is.EqualTo(record1Test["numRegion"]));
            TestHelper.Assert.That(recordResult.Value.value["oilsReserve"], Is.EqualTo(record1Test["oilsReserve"]));
            TestHelper.Assert.That(recordResult.Value.value["islandName"], Is.EqualTo(record1Test["islandName"]));
            TestHelper.Assert.That(recordResult.Value.value["buildings"], Is.EqualTo(record1Test["buildings"]));
            TestHelper.Assert.That(recordResult.Value.value["resources"], Is.EqualTo(record1Test["resources"]));
            TestHelper.Assert.That(recordResult.Value.value["numIsland"], Is.EqualTo(record2Test["numIsland"]));
            TestHelper.Assert.That(recordResult.Value.value["waterReserve"], Is.EqualTo(record2Test["waterReserve"]));
            TestHelper.Assert.That(recordResult.Value.value["islands"], Is.EqualTo(record2Test["islands"]));
            TestHelper.Assert.That(recordResult.Value.value["numIsland"], Is.EqualTo(record2Test["numIsland"]));
            TestHelper.Assert.That(recordResult.Value.value["population"], Is.EqualTo(record2Test["population"]));
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetInvalidKeyUserRecord_Failed()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Result<UserRecord> recordResult = null;
            cloudSave.GetUserRecord(userId, "Invalid", result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsTrue(recordResult.IsError, "Get user record success.");
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator ReplaceUserRecord_Success()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Result replaceRecordResult = null;
            cloudSave.ReplaceUserRecord(userId, keyUserTest, newRecord1Test, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(userId, keyUserTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(newRecord1Test["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(newRecord1Test["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["islandName"], Is.EqualTo(newRecord1Test["islandName"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(newRecord1Test["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(newRecord1Test["resources"]));
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator DeleteUserRecord_Success()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Result deleteRecordResult = null;
            cloudSave.DeleteUserRecord(userId, keyUserTest, result => { deleteRecordResult = result; });
            yield return TestHelper.WaitForValue(() => deleteRecordResult);

            Assert.IsFalse(deleteRecordResult.IsError, "Delete user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(userId, keyUserTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsTrue(getRecordResult.IsError, "Get user record success.");
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator DeleteInvalidKeyUserRecord_Success()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Result recordResult = null;
            cloudSave.DeleteUserRecord(userId, "Invalid", result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete user record failed.");
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator ReplaceUnexistKeyUserRecord_Success()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            string key = "UnexistUnitySDKKeyUserTest";
            Result replaceRecordResult = null;
            cloudSave.ReplaceUserRecord(userId, key, record1Test, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace unexist key user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(userId, key, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(record1Test["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(record1Test["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["islandName"], Is.EqualTo(record1Test["islandName"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(record1Test["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(record1Test["resources"]));

            Result deleteRecordResult = null;
            cloudSave.DeleteUserRecord(userId, key, result => { deleteRecordResult = result; });
            yield return TestHelper.WaitForValue(() => deleteRecordResult);

            Assert.IsFalse(deleteRecordResult.IsError, "Delete user record failed.");
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator ReplaceWithSaveUserRecord_Success()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Dictionary<string, object> record = new Dictionary<string, object>
            {
                {"numRegion", 8 }, {"oilsReserve", 10},
                {"buildings", new string[2] { "towerA", "towerB" }},
                {"resources", new Dictionary<string, int>{{"gas", 0 }, {"water", 0 }, {"gold", 0 }}}
            };
            Result saveRecordResult = null;
            cloudSave.SaveUserRecord(userId, keyUserTest, record, result => { saveRecordResult = result; });
            yield return TestHelper.WaitForValue(() => saveRecordResult);

            Assert.IsFalse(saveRecordResult.IsError, "Replace with save user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(userId, keyUserTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(record["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(record["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(record["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(record["resources"]));
        }

        [UnityTest, TestLog, Order(999)]
        public IEnumerator Teardown()
        {
            ServerCloudSave cloudSave = AccelByteServerPlugin.GetCloudSave();
            string userId = user.Session.UserId;

            Result recordResult = null;
            cloudSave.DeleteUserRecord(userId, keyUserTest, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete user record failed.");
            
            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;
                this.user.Logout(r => logoutResult = r); 
                yield return TestHelper.WaitForValue(() => logoutResult);
                Assert.IsFalse(logoutResult.IsError, "User cannot log out.");
            }
            
            TestHelper testHelper = new TestHelper();

            Result deleteResult = null;
            testHelper.DeleteUser(userId, result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Setup: Deleted cloudSaveUser2");
            TestHelper.Assert.IsResultOk(deleteResult);
        }
    }
}
