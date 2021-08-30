// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class CloudSaveTest
    {
        private User user;
        private User user2;
        private TestHelper helper;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private UserData userData2;

        [UnityTest, TestLog, Order(0)]
        public IEnumerator Setup()
        {
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

            if (this.httpClient == null)
            {
                this.httpClient = new AccelByteHttpClient();
                this.httpClient.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            }

            if (this.coroutineRunner == null)
            {
                this.coroutineRunner = new CoroutineRunner();
            }

            if (this.helper == null)
            {
                this.helper = new TestHelper();
            }

            if (this.user2 != null) yield break;

            Result<RegisterUserResponse> registerResult = null;
            LoginSession loginSession;

            loginSession = new LoginSession(
                AccelBytePlugin.Config.IamServerUrl,
                AccelBytePlugin.Config.Namespace,
                AccelBytePlugin.Config.RedirectUri,
                this.httpClient,
                this.coroutineRunner);

            var userAccount = new UserAccount(
                AccelBytePlugin.Config.IamServerUrl,
                AccelBytePlugin.Config.Namespace,
                loginSession,
                this.httpClient);

            user2 = new User(
                loginSession,
                userAccount,
                this.coroutineRunner);

            user2.Register(
                    "cloudSaveUser2+accelbyteunitysdk@example.com",
                    "Password123",
                    "cloudSaveUser2",
                    "US",
                    System.DateTime.Now.AddYears(-22),
                    result => registerResult = result);
            yield return TestHelper.WaitForValue(() => registerResult);

            TestHelper.LogResult(registerResult, "Setup: Registered cloudSaveUser2");

            Result loginResult = null;

            user2.LoginWithUsername(
                    "cloudSaveUser2+accelbyteunitysdk@example.com",
                    "Password123",
                    result => loginResult = result);
            yield return TestHelper.WaitForValue(() => loginResult);

            Result<UserData> userResult = null;
            user2.GetData(r => userResult = r);
            yield return TestHelper.WaitForValue(() => userResult);

            userData2 = userResult.Value;

            TestHelper.LogResult(loginResult, "Setup: Logged in " + userResult.Value.displayName);

            yield return new UnityEngine.WaitForSeconds(0.1f);
        }

        string keyUserTest = "UnitySDKKeyUserTest";
        string keyUserPublic = "UnitySDKKeyUserPublicTest";
        string keyGameTest = "UnitySDKKeyGameTest";
        string keyReplaceTest = "UnitySDKKeyReplaceTest";
        string keySaveTest = "UnitySDKKeySaveTest";

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
        Dictionary<string, object> firstReplaceTest = new Dictionary<string, object>
        {
            {"first", 1 }
        };
        Dictionary<string, object> secondReplaceTest = new Dictionary<string, object>
        {
            {"second", 2 }
        };

        [UnityTest, TestLog, Order(1)]
        public IEnumerator SaveUserRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result record1Result = null;
            cloudSave.SaveUserRecord(keyUserTest, record1Test, false, result => { record1Result = result; });
            yield return TestHelper.WaitForValue(() => record1Result);

            Assert.IsFalse(record1Result.IsError, "Save user 1st record failed.");

            Result record2Result = null;
            cloudSave.SaveUserRecord(keyUserTest, record2Test, false, result => { record2Result = result; });
            yield return TestHelper.WaitForValue(() => record2Result);

            Assert.IsFalse(record2Result.IsError, "Save user 2nd record failed.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator SaveUserPublicRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result record1Result = null;
            cloudSave.SaveUserRecord(keyUserPublic, record1Test, true, result => { record1Result = result; });
            yield return TestHelper.WaitForValue(() => record1Result);

            Assert.IsFalse(record1Result.IsError, "Save user 1st public record failed.");

            Result record2Result = null;
            cloudSave.SaveUserRecord(keyUserPublic, record2Test, true, result => { record2Result = result; });
            yield return TestHelper.WaitForValue(() => record2Result);

            Assert.IsFalse(record2Result.IsError, "Save user 2nd public record failed.");
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetUserRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result<UserRecord> recordResult = null;
            cloudSave.GetUserRecord(keyUserTest, result => { recordResult = result; });
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
        public IEnumerator GetUserOwnPublicRecordPrivate_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result<UserRecord> recordResult = null;
            cloudSave.GetUserRecord(keyUserPublic, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(recordResult.Value.key, Is.EqualTo(keyUserPublic));
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
        public IEnumerator GetUserOwnPublicRecordPublic_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result<UserRecord> recordResult = null;
            cloudSave.GetPublicUserRecord(keyUserPublic, user.Session.UserId, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(recordResult.Value.key, Is.EqualTo(keyUserPublic));
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
        public IEnumerator GetOtherUserPublicRecord_Success()
        {
            CloudSave cloudSave = new CloudSave(
                    new CloudSaveApi(AccelBytePlugin.Config.CloudSaveServerUrl, this.httpClient),
                    user2.Session,
                    AccelBytePlugin.Config.Namespace,
                    coroutineRunner);

            Result<UserRecord> recordResult = null;
            cloudSave.GetPublicUserRecord(keyUserPublic, user.Session.UserId, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(recordResult.Value.key, Is.EqualTo(keyUserPublic));
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
        public IEnumerator GetOtherUserRecord_Failed()
        {
            CloudSave cloudSave = new CloudSave(
                    new CloudSaveApi(AccelBytePlugin.Config.CloudSaveServerUrl, this.httpClient),
                    user2.Session,
                    AccelBytePlugin.Config.Namespace,
                    coroutineRunner);

            Result<UserRecord> recordResult = null;
            cloudSave.GetPublicUserRecord(keyUserTest, user.Session.UserId, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsTrue(recordResult.IsError, "Get user record failed.");
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator GetInvalidKeyUserRecord_Failed()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result<UserRecord> recordResult = null;
            cloudSave.GetUserRecord("Invalid", result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsTrue(recordResult.IsError, "Get user record success.");
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator ReplaceUserRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result replaceRecordResult = null;
            cloudSave.ReplaceUserRecord(keyUserTest, newRecord1Test, false, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(keyUserTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(newRecord1Test["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(newRecord1Test["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["islandName"], Is.EqualTo(newRecord1Test["islandName"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(newRecord1Test["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(newRecord1Test["resources"]));
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator ReplaceUserPublicRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result replaceRecordResult = null;
            cloudSave.ReplaceUserRecord(keyUserPublic, newRecord1Test, true, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetPublicUserRecord(keyUserPublic, user.Session.UserId, result => { getRecordResult = result; });
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
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result deleteRecordResult = null;
            cloudSave.DeleteUserRecord(keyUserTest, result => { deleteRecordResult = result; });
            yield return TestHelper.WaitForValue(() => deleteRecordResult);

            Assert.IsFalse(deleteRecordResult.IsError, "Delete user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(keyUserTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsTrue(getRecordResult.IsError, "Get user record success.");
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator DeleteInvalidKeyUserRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result recordResult = null;
            cloudSave.DeleteUserRecord("Invalid", result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete user record failed.");
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator ReplaceUnexistKeyUserRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            string key = "UnexistUnitySDKKeyUserTest";
            Result replaceRecordResult = null;
            cloudSave.ReplaceUserRecord(key, record1Test, false, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace unexist key user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(key, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(record1Test["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(record1Test["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["islandName"], Is.EqualTo(record1Test["islandName"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(record1Test["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(record1Test["resources"]));

            Result deleteRecordResult = null;
            cloudSave.DeleteUserRecord(key, result => { deleteRecordResult = result; });
            yield return TestHelper.WaitForValue(() => deleteRecordResult);

            Assert.IsFalse(deleteRecordResult.IsError, "Delete user record failed.");
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator ReplaceWithSaveUserRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Dictionary<string, object> record = new Dictionary<string, object>
            {
                {"numRegion", 8 }, {"oilsReserve", 10},
                {"buildings", new string[2] { "towerA", "towerB" }},
                {"resources", new Dictionary<string, int>{{"gas", 0 }, {"water", 0 }, {"gold", 0 }}}
            };
            Result saveRecordResult = null;
            cloudSave.SaveUserRecord(keyUserTest, record, false, result => { saveRecordResult = result; });
            yield return TestHelper.WaitForValue(() => saveRecordResult);

            Assert.IsFalse(saveRecordResult.IsError, "Replace with save user record failed.");

            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(keyUserTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get user record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(record["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(record["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(record["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(record["resources"]));
        }

        [UnityTest, TestLog, Order(5), Timeout(30000), Ignore("Concurrent record update is not actually concurrent right now")]
        public IEnumerator ReplaceUserRecord_RacingCondition_ManyRequestAtOnce_Success()
        {
            var cloudSave = AccelBytePlugin.GetCloudSave();

            // update user record
            int concurrentWriteCount = 10;
            int updateSuccessCount = 0;
            int updateDone = 0;
            const int tryAttemp = 10;
            const string concurrentKey = "UnityKeyConcurrentUserRecordTest";
            for (int i = 0; i < concurrentWriteCount; i++)
            {
                int dictIndex = i;
                cloudSave.ReplaceUserRecordCheckLatest(tryAttemp, concurrentKey, new Dictionary<string, object> { {$"key_{dictIndex}", dictIndex} }, result =>
                {
                    Interlocked.Increment(ref updateDone);
                    if (!result.IsError)
                    {
                        Interlocked.Increment(ref updateSuccessCount);
                    }
                },
                oldData =>
                {
                    new WaitForSeconds(1.5f);
                    Thread.Sleep(200);
                    if (!oldData.ContainsKey($"key_{dictIndex}"))
                    {
                        oldData.Add($"key_{dictIndex}", dictIndex);
                    }
                    return oldData;
                });
                if (i == 0)
                {
                    yield return new WaitForSeconds(1f);
                }
            }

            while (updateDone < concurrentWriteCount) yield return new WaitForSeconds(0.2f);

            // get the latest record
            Result<UserRecord> getRecordResult = null;
            cloudSave.GetUserRecord(concurrentKey, result =>
            {
                getRecordResult = result;
            });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            bool assertValueCount = getRecordResult.Value.value.Count == updateSuccessCount;

            Result deleteResult = null;
            cloudSave.DeleteUserRecord(concurrentKey, result =>
            {
                deleteResult = result;
            });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.Assert.IsTrue(assertValueCount);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator ReplaceUserRecord_RacingCondition_Manual_Ejected()
        {
            var cloudSave = AccelBytePlugin.GetCloudSave();

            const string concurrentKey = "UnityKeyConcurrentManualUserRecordTest";
            int dictIndex = 0;
            Result replaceRecordResult = null;
            cloudSave.ReplaceUserRecordCheckLatest(concurrentKey, DateTime.Now, new Dictionary<string, object> { { $"key_{dictIndex}", dictIndex++ } }, result =>
            {
                replaceRecordResult = result;
            });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Result replaceRecordResult2 = null;
            cloudSave.ReplaceUserRecordCheckLatest(concurrentKey, DateTime.Now, new Dictionary<string, object> { { $"key_{dictIndex}", dictIndex++ } }, result =>
            {
                replaceRecordResult2 = result;
            });
            yield return TestHelper.WaitForValue(() => replaceRecordResult2);

            Result deleteResult = null;
            cloudSave.DeleteUserRecord(concurrentKey, result =>
            {
                deleteResult = result;
            });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.Assert.IsResultOk(replaceRecordResult);
            TestHelper.Assert.IsTrue(replaceRecordResult2.IsError);
            TestHelper.Assert.IsTrue(replaceRecordResult2.Error.Code == ErrorCode.PlayerRecordPreconditionFailed);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Order(7)]
        public IEnumerator SaveGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result record1Result = null;
            cloudSave.SaveGameRecord(keyGameTest, record1Test, result => { record1Result = result; });
            yield return TestHelper.WaitForValue(() => record1Result);

            Assert.IsFalse(record1Result.IsError, "Save game 1st record failed.");

            Result record2Result = null;
            cloudSave.SaveGameRecord(keyGameTest, record2Test, result => { record2Result = result; });
            yield return TestHelper.WaitForValue(() => record2Result);

            Assert.IsFalse(record2Result.IsError, "Save game 2nd record failed.");
        }

        [UnityTest, TestLog, Order(8)]
        public IEnumerator GetGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result<GameRecord> recordResult = null;
            cloudSave.GetGameRecord(keyGameTest, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(recordResult.Value.key, Is.EqualTo(keyGameTest));
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

        [UnityTest, TestLog, Order(8)]
        public IEnumerator GetInvalidKeyGameRecord_Failed()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result<GameRecord> recordResult = null;
            cloudSave.GetGameRecord("Invalid", result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsTrue(recordResult.IsError, "Get game record success.");
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator ReplaceGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result replaceRecordResult = null;
            cloudSave.ReplaceGameRecord(keyGameTest, newRecord1Test, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace game record failed.");

            Result<GameRecord> getRecordResult = null;
            cloudSave.GetGameRecord(keyGameTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(newRecord1Test["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(newRecord1Test["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["islandName"], Is.EqualTo(newRecord1Test["islandName"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(newRecord1Test["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(newRecord1Test["resources"]));
            TestHelper.Assert.That(!getRecordResult.Value.value.ContainsKey("numIsland"));
            TestHelper.Assert.That(!getRecordResult.Value.value.ContainsKey("waterReserve"));
            TestHelper.Assert.That(!getRecordResult.Value.value.ContainsKey("islands"));
            TestHelper.Assert.That(!getRecordResult.Value.value.ContainsKey("population"));
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator ReplaceExistingGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result replaceRecordResult = null;
            cloudSave.ReplaceGameRecord(keyReplaceTest, firstReplaceTest, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace game record failed.");

            Result<GameRecord> getFirstRecordResult = null;
            cloudSave.GetGameRecord(keyReplaceTest, result => { getFirstRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getFirstRecordResult);

            Assert.IsFalse(getFirstRecordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(getFirstRecordResult.Value.value, Is.EqualTo(firstReplaceTest));
            TestHelper.Assert.That(getFirstRecordResult.Value.value, !Is.EqualTo(secondReplaceTest));
            TestHelper.Assert.IsTrue(getFirstRecordResult.Value.value.Count == 1);


            replaceRecordResult = null;
            cloudSave.ReplaceGameRecord(keyReplaceTest, secondReplaceTest, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace game record failed.");

            Result<GameRecord> getSecondRecordResult = null;
            cloudSave.GetGameRecord(keyReplaceTest, result => { getSecondRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getSecondRecordResult);

            Assert.IsFalse(getSecondRecordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(getSecondRecordResult.Value.value, !Is.EqualTo(firstReplaceTest));
            TestHelper.Assert.That(getSecondRecordResult.Value.value, Is.EqualTo(secondReplaceTest));
            TestHelper.Assert.IsTrue(getSecondRecordResult.Value.value.Count == 1);
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator SaveExistingGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result saveRecordResult = null;
            cloudSave.SaveGameRecord(keySaveTest, firstReplaceTest, result => { saveRecordResult = result; });
            yield return TestHelper.WaitForValue(() => saveRecordResult);

            Assert.IsFalse(saveRecordResult.IsError, "Save game record failed.");

            Result<GameRecord> getFirstRecordResult = null;
            cloudSave.GetGameRecord(keySaveTest, result => { getFirstRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getFirstRecordResult);

            Assert.IsFalse(getFirstRecordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(getFirstRecordResult.Value.value, Is.EqualTo(firstReplaceTest));
            TestHelper.Assert.That(getFirstRecordResult.Value.value, !Is.EqualTo(secondReplaceTest));
            TestHelper.Assert.IsTrue(getFirstRecordResult.Value.value.Count == 1);


            saveRecordResult = null;
            cloudSave.SaveGameRecord(keySaveTest, secondReplaceTest, result => { saveRecordResult = result; });
            yield return TestHelper.WaitForValue(() => saveRecordResult);

            Assert.IsFalse(saveRecordResult.IsError, "Save game record failed.");

            Result<GameRecord> getSecondRecordResult = null;
            cloudSave.GetGameRecord(keySaveTest, result => { getSecondRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getSecondRecordResult);

            Assert.IsFalse(getSecondRecordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(getSecondRecordResult.Value.value["first"], Is.EqualTo(firstReplaceTest["first"]));
            TestHelper.Assert.That(getSecondRecordResult.Value.value["second"], Is.EqualTo(secondReplaceTest["second"]));
            TestHelper.Assert.IsTrue(getSecondRecordResult.Value.value.Count == 2);
        }

        [UnityTest, TestLog, Order(9), Timeout(30000), Ignore("Concurrent record update is not actually concurrent right now")]
        public IEnumerator ReplaceGameRecord_RacingCondition_ManyRequestAtOnce_Success()
        {
            var cloudSave = AccelBytePlugin.GetCloudSave();

            // update user record
            int concurrentWriteCount = 10;
            int updateSuccessCount = 0;
            int updateDone = 0;
            const int tryAttemp = 10;
            const string concurrentKey = "UnityKeyConcurrentGameRecordTest";
            for (int i = 0; i < concurrentWriteCount; i++)
            {
                int dictIndex = i;
                cloudSave.ReplaceGameRecordCheckLatest(tryAttemp, concurrentKey, new Dictionary<string, object> { { $"key_{dictIndex}", dictIndex } }, result =>
                {
                    Interlocked.Increment(ref updateDone);
                    if (!result.IsError)
                    {
                        Interlocked.Increment(ref updateSuccessCount);
                    }
                },
                oldData =>
                {
                    new WaitForSeconds(1.5f);
                    Thread.Sleep(200);
                    if (!oldData.ContainsKey($"key_{dictIndex}"))
                    {
                        oldData.Add($"key_{dictIndex}", dictIndex);
                    }
                    return oldData;
                });
                if (i == 0)
                {
                    yield return new WaitForSeconds(1f);
                }
            }

            while (updateDone < concurrentWriteCount) yield return new WaitForSeconds(0.2f);

            // get the latest record
            Result<GameRecord> getRecordResult = null;
            cloudSave.GetGameRecord(concurrentKey, result =>
            {
                getRecordResult = result;
            });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            bool assertValueCount = getRecordResult.Value.value.Count == updateSuccessCount;

            Result deleteResult = null;
            cloudSave.DeleteGameRecord(concurrentKey, result =>
            {
                deleteResult = result;
            });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.Assert.IsTrue(assertValueCount);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Order(9)]
        public IEnumerator ReplaceGameRecord_RacingCondition_Manual_Ejected()
        {
            var cloudSave = AccelBytePlugin.GetCloudSave();

            const string concurrentKey = "UnityKeyConcurrentManualGameRecordTest";
            int dictIndex = 0;
            Result replaceRecordResult = null;
            cloudSave.ReplaceGameRecordCheckLatest(concurrentKey, DateTime.Now, new Dictionary<string, object> { { $"key_{dictIndex}", dictIndex++ } }, result =>
            {
                replaceRecordResult = result;
            });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Result replaceRecordResult2 = null;
            cloudSave.ReplaceGameRecordCheckLatest(concurrentKey, DateTime.Now, new Dictionary<string, object> { { $"key_{dictIndex}", dictIndex++ } }, result =>
            {
                replaceRecordResult2 = result;
            });
            yield return TestHelper.WaitForValue(() => replaceRecordResult2);

            Result deleteResult = null;
            cloudSave.DeleteGameRecord(concurrentKey, result =>
            {
                deleteResult = result;
            });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.Assert.IsResultOk(replaceRecordResult);
            TestHelper.Assert.IsTrue(replaceRecordResult2.IsError);
            TestHelper.Assert.IsTrue(replaceRecordResult2.Error.Code == ErrorCode.GameRecordPreconditionFailed);
            TestHelper.Assert.IsResultOk(deleteResult);
        }

        [UnityTest, TestLog, Order(10)]
        public IEnumerator DeleteGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result deleteRecordResult = null;
            cloudSave.DeleteGameRecord(keyGameTest, result => { deleteRecordResult = result; });
            yield return TestHelper.WaitForValue(() => deleteRecordResult);

            Assert.IsFalse(deleteRecordResult.IsError, "Delete game record failed.");

            Result<GameRecord> getRecordResult = null;
            cloudSave.GetGameRecord(keyGameTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsTrue(getRecordResult.IsError, "Get game record success.");
        }

        [UnityTest, TestLog, Order(10)]
        public IEnumerator DeleteInvalidKeyGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result recordResult = null;
            cloudSave.DeleteGameRecord("Invalid", result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete game record failed.");
        }

        [UnityTest, TestLog, Order(11)]
        public IEnumerator ReplaceGameUnexistKey_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            string key = "UnexistUnitySDKKeyGameTest";

            Result replaceRecordResult = null;
            cloudSave.ReplaceGameRecord(key, record1Test, result => { replaceRecordResult = result; });
            yield return TestHelper.WaitForValue(() => replaceRecordResult);

            Assert.IsFalse(replaceRecordResult.IsError, "Replace game record failed.");

            Result<GameRecord> getRecordResult = null;
            cloudSave.GetGameRecord(key, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(record1Test["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(record1Test["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["islandName"], Is.EqualTo(record1Test["islandName"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(record1Test["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(record1Test["resources"]));

            Result deleteRecordResult = null;
            cloudSave.DeleteGameRecord(key, result => { deleteRecordResult = result; });
            yield return TestHelper.WaitForValue(() => deleteRecordResult);

            Assert.IsFalse(deleteRecordResult.IsError, "Delete game record failed.");
        }

        [UnityTest, TestLog, Order(11)]
        public IEnumerator ReplaceWithSaveGameRecord_Success()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Dictionary<string, object> record = new Dictionary<string, object>
            {
                {"numRegion", 8 }, {"oilsReserve", 10},
                {"buildings", new string[2] { "towerA", "towerB" }},
                {"resources", new Dictionary<string, int>{{"gas", 0 }, {"water", 0 }, {"gold", 0 }}}
            };
            Result saveRecordResult = null;
            cloudSave.SaveGameRecord(keyGameTest, record, result => { saveRecordResult = result; });
            yield return TestHelper.WaitForValue(() => saveRecordResult);

            Assert.IsFalse(saveRecordResult.IsError, "Replace with save game record failed.");

            Result<GameRecord> getRecordResult = null;
            cloudSave.GetGameRecord(keyGameTest, result => { getRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getRecordResult);

            Assert.IsFalse(getRecordResult.IsError, "Get game record failed.");
            TestHelper.Assert.That(getRecordResult.Value.value["numRegion"], Is.EqualTo(record["numRegion"]));
            TestHelper.Assert.That(getRecordResult.Value.value["oilsReserve"], Is.EqualTo(record["oilsReserve"]));
            TestHelper.Assert.That(getRecordResult.Value.value["buildings"], Is.EqualTo(record["buildings"]));
            TestHelper.Assert.That(getRecordResult.Value.value["resources"], Is.EqualTo(record["resources"]));
        }

        [UnityTest, TestLog, Order(12)]
        public IEnumerator GetRecordDifferentLevel_Failed()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result<UserRecord> getUserRecordResult = null;
            cloudSave.GetUserRecord(keyGameTest, result => { getUserRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getUserRecordResult);

            Assert.IsTrue(getUserRecordResult.IsError, "Get record in game level success.");

            Result<GameRecord> getGameRecordResult = null;
            cloudSave.GetGameRecord(keyUserTest, result => { getGameRecordResult = result; });
            yield return TestHelper.WaitForValue(() => getGameRecordResult);

            Assert.IsTrue(getGameRecordResult.IsError, "Get record in user level success.");
        }

        [UnityTest, TestLog, Order(999)]
        public IEnumerator Teardown()
        {
            CloudSave cloudSave = AccelBytePlugin.GetCloudSave();

            Result recordResult = null;
            cloudSave.DeleteUserRecord(keyUserTest, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete user record failed.");

            recordResult = null;
            cloudSave.DeleteGameRecord(keyGameTest, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete game record failed.");

            recordResult = null;
            cloudSave.DeleteGameRecord(keyUserPublic, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete game record failed.");

            recordResult = null;
            cloudSave.DeleteGameRecord(keySaveTest, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete game record failed.");

            recordResult = null;
            cloudSave.DeleteGameRecord(keyReplaceTest, result => { recordResult = result; });
            yield return TestHelper.WaitForValue(() => recordResult);

            Assert.IsFalse(recordResult.IsError, "Delete game record failed.");

            TestHelper testHelper = new TestHelper();

            Result deleteResult = null;
            testHelper.DeleteUser(AccelBytePlugin.GetUser(), result => { deleteResult = result; });
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.Assert.IsResultOk(deleteResult);

            deleteResult = null;

            this.helper.DeleteUser(this.user2, result => deleteResult = result);
            yield return TestHelper.WaitForValue(() => deleteResult);

            TestHelper.LogResult(deleteResult, "Setup: Deleted cloudSaveUser2");
            TestHelper.Assert.IsResultOk(deleteResult);

            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;
                this.user.Logout(r => logoutResult = r); 
                yield return TestHelper.WaitForValue(() => logoutResult);
                Assert.IsFalse(logoutResult.IsError, "User cannot log out.");
            }
            
            if (this.user2.Session.IsValid())
            {
                Result logoutResult = null;
                this.user2.Logout(r => logoutResult = r);
                yield return TestHelper.WaitForValue(() => logoutResult);
                Assert.IsFalse(logoutResult.IsError, "User cannot log out.");
            }
            
            this.user2 = null;
        }
    }
}
