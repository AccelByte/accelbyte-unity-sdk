// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Linq;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using NUnit.Framework.Api;
using Steamworks;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using System.Threading;
using NUnit.Framework.Internal.Commands;
using System.Collections.Generic;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class GameProfileTest
    {
        private GameProfile gameProfile = null;
        private User user;

        [UnityTest, Order(0)]
        public IEnumerator Setup()
        {
            this.user = AccelBytePlugin.GetUser();

            Result loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            while (loginWithDevice == null) { yield return new WaitForSeconds(.1f); }

            Debug.Log(this.user.UserId);
            Assert.That(!loginWithDevice.IsError);
        }

        [UnityTest, Order(1), Timeout(70000)]
        public IEnumerator CreateGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();

            Result<GameProfile> createGameProfileResult = null;
            gameProfiles.CreateGameProfile(
                new GameProfileRequest
                {
                    label = "GameProfile Test", profileName = "ProfileName Test",
                    tags = new string[] { "tag1", "tag2", "tag3" },
                    attributes = new Dictionary<string, string>()
                    {
                        { "test", "test123" },
                        { "name", "testName" }
                    }
                },
                result => { createGameProfileResult = result; });
            while (createGameProfileResult == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(createGameProfileResult, "Create game profile");
            Assert.That(!createGameProfileResult.IsError);
            Assert.NotNull(createGameProfileResult.Value);
            this.gameProfile = createGameProfileResult.Value;
        }

        [UnityTest, Order(2), Timeout(70000)]
        public IEnumerator GetAllGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();
            Result<GameProfile[]> getAllGameProfilesResults = null;

            gameProfiles.GetAllGameProfiles(result => { getAllGameProfilesResults = result; });
            while (getAllGameProfilesResults == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(getAllGameProfilesResults, "Get all game profiles, after created");
            Assert.That(!getAllGameProfilesResults.IsError);
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

        [UnityTest, Order(3), Timeout(70000)]
        public IEnumerator BatchGetGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();
            Result<UserGameProfiles[]> batchGetGameProfilesResults = null;

            string[] userIds = new string[] {this.user.UserId, "some_random_user_id", "not_exist_user_id" };

            gameProfiles.BatchGetGameProfiles( userIds, result => { batchGetGameProfilesResults = result; });
            while (batchGetGameProfilesResults == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(batchGetGameProfilesResults, "Batch get game profiles");
            Assert.That(!batchGetGameProfilesResults.IsError);
            bool gameProfileFound = false;
            bool notExistEmptyProfile = true;
            int notExistUserIdCount = 0;
            foreach (UserGameProfiles userGameProfile in batchGetGameProfilesResults.Value)
            {
                if (userGameProfile.userId == this.gameProfile.userId)
                {
                    foreach(GameProfilePublicInfo profile in userGameProfile.gameProfiles)
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

        [UnityTest, Order(4), Timeout(70000)]
        public IEnumerator GetCreatedGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();
        
            Result<GameProfile> getGameProfileResult = null;
            gameProfiles.GetGameProfile(this.gameProfile.profileId, result => { getGameProfileResult = result; });
            while (getGameProfileResult == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(getGameProfileResult, "Get game profile, after created");
            Assert.That(!getGameProfileResult.IsError);
        }

        [UnityTest, Order(5), Timeout(70000)]
        public IEnumerator UpdateGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();

            this.gameProfile.label = "label test";
            this.gameProfile.profileName = "profile name test";

            Result<GameProfile> updateGameProfileResult = null;
            gameProfiles.UpdateGameProfile(this.gameProfile, result => { updateGameProfileResult = result; });
            while (updateGameProfileResult == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(updateGameProfileResult, "Update game profile");
            Assert.That(!updateGameProfileResult.IsError);

            var gameProfileResult = updateGameProfileResult.Value;

            Assert.AreEqual(this.gameProfile.label, gameProfileResult.label);
            Assert.AreEqual(this.gameProfile.profileName, gameProfileResult.profileName);
        }

        [UnityTest, Order(6), Timeout(70000)]
        public IEnumerator GetGameProfileAttribute_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();

            Result<GameProfileAttribute> getGameProfileAttributeResult = null;
            gameProfiles.GetGameProfileAttribute(this.gameProfile.profileId, "test", result => { getGameProfileAttributeResult = result; });
            while (getGameProfileAttributeResult == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(getGameProfileAttributeResult, "Get game profile attribute");
            Assert.That(!getGameProfileAttributeResult.IsError);
            Assert.AreEqual(getGameProfileAttributeResult.Value.name, "test");
            Assert.AreEqual(this.gameProfile.attributes["test"], getGameProfileAttributeResult.Value.value);
        }

        [UnityTest, Order(7), Timeout(70000)]
        public IEnumerator UpdateGameProfileAttribute_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();

            Result<GameProfile> updateGameProfileAttributeResult = null;
            gameProfiles.UpdateGameProfileAttribute(
                this.gameProfile.profileId, new GameProfileAttribute { name = "test", value = "updated value" },
                result => { updateGameProfileAttributeResult = result; });
            while (updateGameProfileAttributeResult == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(updateGameProfileAttributeResult, "Get game profile attribute");
            Assert.That(!updateGameProfileAttributeResult.IsError);
            Assert.That(updateGameProfileAttributeResult.Value.attributes.ContainsKey("test"));
            Assert.AreEqual(updateGameProfileAttributeResult.Value.attributes["test"], "updated value");
        }

        [UnityTest, Order(8), Timeout(70000)]
        public IEnumerator DeleteGameProfile_Success()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();

            Result deleteGameProfileResult = null;
            gameProfiles.DeleteGameProfile(this.gameProfile.profileId, result => { deleteGameProfileResult = result; });
            while (deleteGameProfileResult == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(deleteGameProfileResult, "Delete game profile");
            Assert.That(!deleteGameProfileResult.IsError);
        }

        [UnityTest, Order(9), Timeout(70000)]
        public IEnumerator GetAllGameProfile_DoesntContainsDeletedProfile()
        {
            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            TestHelper helper = new TestHelper();
            Result<GameProfile[]> getAllGameProfilesResults = null;

            gameProfiles.GetAllGameProfiles(result => { getAllGameProfilesResults = result; });
            while (getAllGameProfilesResults == null) { yield return new WaitForSeconds(.1f); }
            TestHelper.LogResult(getAllGameProfilesResults, "Get all game profiles, doesn't contains deleted profile");
            Assert.That(!getAllGameProfilesResults.IsError);
            bool gameProfileFound = false;
            foreach (GameProfile gameProfile in getAllGameProfilesResults.Value)
            {
                if (gameProfile.profileId == this.gameProfile.profileId)
                {
                    gameProfileFound = true;
                    break;
                }
            }
            Assert.That(!gameProfileFound);
        }

        [UnityTest, Order(999)]
        public IEnumerator Teardown()
        {
            yield return null;
        }
    }
}
