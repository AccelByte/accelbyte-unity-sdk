// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

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
    public class GameProfileTest
    {
        private GameProfile gameProfile = null;
        private User user;

        [UnityTest, TestLog, Order(0)]
        public IEnumerator Setup()
        {
            this.user = AccelBytePlugin.GetUser();

            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;
                
                this.user.Logout(r => logoutResult = r);
                yield return TestHelper.WaitForValue(() => logoutResult);
            }

            Result loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);

            Debug.Log(this.user.Session.UserId);
            TestHelper.Assert.IsResultOk(loginWithDevice);
        }

        [UnityTest, TestLog, Order(1), Timeout(70000)]
        public IEnumerator CreateGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();

            Result<GameProfile> createGameProfileResult = null;
            gameProfiles.CreateGameProfile(
                new GameProfileRequest
                {
                    label = "GameProfile Test",
                    profileName = "ProfileName Test",
                    tags = new string[] {"tag1", "tag2", "tag3"},
                    attributes = new Dictionary<string, string>() {{"test", "test123"}, {"name", "testName"}}
                },
                result => { createGameProfileResult = result; });
            yield return TestHelper.WaitForValue(() => createGameProfileResult);

            TestHelper.LogResult(createGameProfileResult, "Create game profile");
            TestHelper.Assert.IsResultOk(createGameProfileResult);
            Assert.NotNull(createGameProfileResult.Value);
            this.gameProfile = createGameProfileResult.Value;
        }

        [UnityTest, TestLog, Order(2), Timeout(70000)]
        public IEnumerator GetAllGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            Result<GameProfile[]> getAllGameProfilesResults = null;

            gameProfiles.GetAllGameProfiles(result => { getAllGameProfilesResults = result; });
            yield return TestHelper.WaitForValue(() => getAllGameProfilesResults);

            TestHelper.LogResult(getAllGameProfilesResults, "Get all game profiles, after created");
            TestHelper.Assert.IsResultOk(getAllGameProfilesResults);
            bool gameProfileFound = false;

            foreach (GameProfile gameProfile in getAllGameProfilesResults.Value)
            {
                if (gameProfile.profileId == this.gameProfile.profileId)
                {
                    gameProfileFound = true;

                    break;
                }
            }

            Assert.That(gameProfileFound);
        }

        [UnityTest, TestLog, Order(3), Timeout(70000)]
        public IEnumerator BatchGetGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            Result<UserGameProfiles[]> batchGetGameProfilesResults = null;

            string[] userIds = new string[] {this.user.Session.UserId, "some_random_user_id", "not_exist_user_id"};

            gameProfiles.BatchGetGameProfiles(userIds, result => { batchGetGameProfilesResults = result; });
            yield return TestHelper.WaitForValue(() => batchGetGameProfilesResults);

            TestHelper.LogResult(batchGetGameProfilesResults, "Batch get game profiles");
            TestHelper.Assert.IsResultOk(batchGetGameProfilesResults);
            bool gameProfileFound = false;
            bool notExistEmptyProfile = true;
            int notExistUserIdCount = 0;

            foreach (UserGameProfiles userGameProfile in batchGetGameProfilesResults.Value)
            {
                if (userGameProfile.userId == this.gameProfile.userId)
                {
                    foreach (GameProfilePublicInfo profile in userGameProfile.gameProfiles)
                    {
                        if (profile.profileId == this.gameProfile.profileId)
                        {
                            gameProfileFound = true;
                        }
                    }
                }
                else if (userGameProfile.userId == "some_random_user_id")
                {
                    notExistUserIdCount++;

                    if (userGameProfile.gameProfiles.Length > 0)
                    {
                        notExistEmptyProfile = false;
                    }
                }
                else if (userGameProfile.userId == "not_exist_user_id")
                {
                    notExistUserIdCount++;

                    if (userGameProfile.gameProfiles.Length > 0)
                    {
                        notExistEmptyProfile = false;
                    }
                }
            }

            Assert.That(gameProfileFound);
            Assert.AreEqual(notExistUserIdCount, 2);
            Assert.That(notExistEmptyProfile);
        }

        [UnityTest, TestLog, Order(4), Timeout(70000)]
        public IEnumerator GetCreatedGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();

            Result<GameProfile> getGameProfileResult = null;
            gameProfiles.GetGameProfile(this.gameProfile.profileId, result => { getGameProfileResult = result; });
            yield return TestHelper.WaitForValue(() => getGameProfileResult);

            TestHelper.LogResult(getGameProfileResult, "Get game profile, after created");
            TestHelper.Assert.IsResultOk(getGameProfileResult);
        }

        [UnityTest, TestLog, Order(5), Timeout(70000)]
        public IEnumerator UpdateGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();

            this.gameProfile.label = "label test";
            this.gameProfile.profileName = "profile name test";

            Result<GameProfile> updateGameProfileResult = null;
            gameProfiles.UpdateGameProfile(this.gameProfile, result => { updateGameProfileResult = result; });
            yield return TestHelper.WaitForValue(() => updateGameProfileResult);

            TestHelper.LogResult(updateGameProfileResult, "Update game profile");
            TestHelper.Assert.IsResultOk(updateGameProfileResult);

            var gameProfileResult = updateGameProfileResult.Value;

            TestHelper.Assert.That(this.gameProfile.label, Is.EqualTo(gameProfileResult.label));
            TestHelper.Assert.That(this.gameProfile.profileName, Is.EqualTo(gameProfileResult.profileName));
        }

        [UnityTest, TestLog, Order(6), Timeout(70000)]
        public IEnumerator GetGameProfileAttribute_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();

            Result<GameProfileAttribute> getGameProfileAttributeResult = null;
            gameProfiles.GetGameProfileAttribute(
                this.gameProfile.profileId,
                "test",
                result => { getGameProfileAttributeResult = result; });
            yield return TestHelper.WaitForValue(() => getGameProfileAttributeResult);

            TestHelper.LogResult(getGameProfileAttributeResult, "Get game profile attribute");
            TestHelper.Assert.IsResultOk(getGameProfileAttributeResult);
            TestHelper.Assert.That(getGameProfileAttributeResult.Value.name, Is.EqualTo("test"));
            TestHelper.Assert.That(
                this.gameProfile.attributes["test"],
                Is.EqualTo(getGameProfileAttributeResult.Value.value));
        }

        [UnityTest, TestLog, Order(7), Timeout(70000)]
        public IEnumerator UpdateGameProfileAttribute_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();

            Result<GameProfile> updateGameProfileAttributeResult = null;
            gameProfiles.UpdateGameProfileAttribute(
                this.gameProfile.profileId,
                new GameProfileAttribute {name = "test", value = "updated value"},
                result => { updateGameProfileAttributeResult = result; });
            yield return TestHelper.WaitForValue(() => updateGameProfileAttributeResult);

            TestHelper.LogResult(updateGameProfileAttributeResult, "Get game profile attribute");
            TestHelper.Assert.IsResultOk(updateGameProfileAttributeResult);
            TestHelper.Assert.That(updateGameProfileAttributeResult.Value.attributes.ContainsKey("test"));
            TestHelper.Assert.That(
                updateGameProfileAttributeResult.Value.attributes["test"],
                Is.EqualTo("updated value"));
        }

        [UnityTest, TestLog, Order(8), Timeout(70000)]
        public IEnumerator DeleteGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();

            Result deleteGameProfileResult = null;
            gameProfiles.DeleteGameProfile(this.gameProfile.profileId, result => { deleteGameProfileResult = result; });
            yield return TestHelper.WaitForValue(() => deleteGameProfileResult);

            TestHelper.LogResult(deleteGameProfileResult, "Delete game profile");
            TestHelper.Assert.IsResultOk(deleteGameProfileResult);
        }

        [UnityTest, TestLog, Order(9), Timeout(70000)]
        public IEnumerator GetAllGameProfile_DoesntContainsDeletedProfile()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            Result<GameProfile[]> getAllGameProfilesResults = null;

            gameProfiles.GetAllGameProfiles(result => { getAllGameProfilesResults = result; });
            yield return TestHelper.WaitForValue(() => getAllGameProfilesResults);

            TestHelper.LogResult(getAllGameProfilesResults, "Get all game profiles, doesn't contains deleted profile");
            TestHelper.Assert.IsResultOk(getAllGameProfilesResults);
            bool gameProfileFound = false;

            foreach (GameProfile gameProfile in getAllGameProfilesResults.Value)
            {
                if (gameProfile.profileId == this.gameProfile.profileId)
                {
                    gameProfileFound = true;

                    break;
                }
            }

            TestHelper.Assert.That(!gameProfileFound);
        }

        [UnityTest, TestLog, Order(999)]
        public IEnumerator Teardown()
        {
            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;
                
                this.user.Logout(r => logoutResult = r);
                yield return TestHelper.WaitForValue(() => logoutResult);
            }
        }
    }
}