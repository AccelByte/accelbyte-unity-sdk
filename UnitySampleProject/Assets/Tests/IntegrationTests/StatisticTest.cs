// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Models;
using AccelByte.Api;
using AccelByte.Core;
using NUnit.Framework;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;
using System.Threading;
using System.Collections.Generic;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class StatisticTest
    {
        private GameProfile gameProfile = null;
        private Statistic statistic = null;
        private TestHelper helper = new TestHelper();
        private string accessToken = null;
        private User user;

        [UnityTest, Order(0)]
        public IEnumerator Setup()
        {
            this.user = AccelBytePlugin.GetUser();

            Result loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });

            while (loginWithDevice == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Debug.Log(this.user.Session.UserId);
            Assert.That(!loginWithDevice.IsError);

            GameProfiles gameProfiles = AccelBytePlugin.GetGameProfiles();
            statistic = AccelBytePlugin.GetStatistic();

            Result<GameProfile[]> getGameProfileResult = null;
            gameProfiles.GetAllGameProfiles(result => { getGameProfileResult = result; });

            while(getGameProfileResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            if (getGameProfileResult.Value.Length == 0)
            {
                Result<GameProfile> createGameProfileResult = null;
                gameProfiles.CreateGameProfile(
                    new GameProfileRequest
                    {
                        label = "Statistic Test",
                        profileName = "ProfileName Test",
                        tags = new string[] { "tag1", "tag2", "tag3" },
                        attributes = new Dictionary<string, string>() { { "test", "test123" }, { "name", "testName" } }
                    },
                    result => { createGameProfileResult = result; });

                while (createGameProfileResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(createGameProfileResult, "Create game profile");
                Assert.That(!createGameProfileResult.IsError);
                Assert.NotNull(createGameProfileResult.Value);
                this.gameProfile = createGameProfileResult.Value;
            }
            else
            {
                this.gameProfile = getGameProfileResult.Value[0];
            }
            Assert.NotNull(gameProfile);

            //Get AccessToken
            Result<TokenData> GetAccessToken = null;
            helper.GetAccessToken(result => { GetAccessToken = result; });

            while (GetAccessToken == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            accessToken = GetAccessToken.Value.access_token;

            //Get Stat
            bool StatIsExist = false;
            Result<StatInfo> GetStatResult = null;
            helper.GetStatByStatCode("MVP", accessToken, result => { GetStatResult = result; });

            while(GetStatResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            TestHelper.LogResult(GetStatResult, "Get Stat.");
            if (GetStatResult.IsError)
            {
                Debug.Log("Fail to get Stat!");
                if (GetStatResult.Error.Code == ErrorCode.StatisticNotFound)
                {
                    Debug.Log("Start to create stat!");
                    Result<StatInfo> CreateStatResult = null;
                    TestHelper.StatCreateModel createStat = new TestHelper.StatCreateModel
                    {
                        defaultValue = 0,
                        description = "Player MVP",
                        incrementOnly = true,
                        maximum = 999999,
                        minimum = 0,
                        name = "MVP",
                        setAsGlobal = true,
                        setBy = StatisticSetBy.CLIENT,
                        statCode = "MVP"
                    };
                    helper.createStat(accessToken, createStat, result => { CreateStatResult = result; });

                    while (CreateStatResult == null)
                    {
                        Thread.Sleep(100);
                        yield return null;
                    }
                    if (!CreateStatResult.IsError)
                    {
                        Debug.Log("Stat Created!");
                        StatIsExist = true;
                    }
                    TestHelper.LogResult(CreateStatResult, "Stat Creation.");
                }
            }
            else {
                StatIsExist = true;
            }
            Assert.IsTrue(StatIsExist);

            bool StatItemIsExist = false;
            Result<StatItemInfo[]> GetStatItemResult = null;
            statistic.GetStatItemsByStatCodes(gameProfile.profileId, new string[] { "MVP" }, result => { GetStatItemResult = result; });
            while(GetStatItemResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            if(GetStatItemResult.Value.Length == 0)
            {
                Result<BulkStatItemOperationResult[]> CreateStatItemResult = null;
                helper.BulkCreateStatItem(gameProfile.userId, gameProfile.profileId, new string[] { "MVP" }, accessToken, result => { CreateStatItemResult = result; });
                while(CreateStatItemResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                if (!CreateStatItemResult.IsError)
                {
                    StatItemIsExist = true;
                }
            }
            else
            {
                StatItemIsExist = true;
            }
            Assert.IsTrue(StatItemIsExist);
        }

        [UnityTest, Order(1)]
        public IEnumerator GetAllStatItems()
        {
            Result<StatItemPagingSlicedResult> getAllStatItemsResult = null;
            statistic.GetAllStatItems(gameProfile.profileId, result => { getAllStatItemsResult = result; });
            while(getAllStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(getAllStatItemsResult, "Get All StatItems");
            Assert.That(!getAllStatItemsResult.IsError);
        }

        [UnityTest, Order(2)]
        public IEnumerator GetStatItemsByStatCodes()
        {
            Result<StatItemInfo[]> getStatItemsResult = null;
            string[] statCodes = new string[] { "MVP", "TOTAL_ASSISTS" };
            statistic.GetStatItemsByStatCodes(gameProfile.profileId, statCodes, result => { getStatItemsResult = result; });
            while(getStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(getStatItemsResult, "Get StatItems by StatCodes");
            Assert.That(!getStatItemsResult.IsError);
        }

        [UnityTest, Order(3)]
        public IEnumerator BulkAddStatItemValue()
        {
            Result<BulkStatItemOperationResult[]> BulkAddStatItemValueResult = null;
            BulkUserStatItemInc MVP = new BulkUserStatItemInc
            {
                inc = 1,
                profileId = gameProfile.profileId,
                statCode = "MVP"
            };
           
            BulkUserStatItemInc[] data = new BulkUserStatItemInc[] { MVP};

            statistic.BulkAddStatItemValue(data, result => { BulkAddStatItemValueResult = result; });
            while(BulkAddStatItemValueResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(BulkAddStatItemValueResult, "Bulk Add StatItem Value");
            Assert.IsTrue(!BulkAddStatItemValueResult.IsError);
            Assert.IsTrue(BulkAddStatItemValueResult.Value[0].success);
        }

        [UnityTest, Order(4)]
        public IEnumerator BulkAddUserStatItemValue()
        {
            Result<BulkStatItemOperationResult[]> BulkAddStatItemValueResult = null;
            BulkStatItemInc MVP = new BulkStatItemInc
            {
                inc = 1,
                statCode = "MVP"
            };

            BulkStatItemInc[] data = new BulkStatItemInc[] { MVP };

            statistic.BulkAddUserStatItemValue(gameProfile.profileId, data, result => { BulkAddStatItemValueResult = result; });
            while (BulkAddStatItemValueResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(BulkAddStatItemValueResult, "Bulk Add StatItem Value");
            Assert.IsTrue(!BulkAddStatItemValueResult.IsError);
            Assert.IsTrue(BulkAddStatItemValueResult.Value[0].success);
        }

        [UnityTest, Order(5)]
        public IEnumerator AddUserStatItemValue()
        {
            Result<StatItemInfo[]> getStatItemsResult = null;
            string statCode = "MVP";
            statistic.GetStatItemsByStatCodes(gameProfile.profileId, new string[] { statCode }, result => { getStatItemsResult = result; });
            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            Result<StatItemIncResult> statItemIncResult = null;
            float increasedValue = 1;
            

            statistic.AddUserStatItemValue(gameProfile.profileId, statCode, increasedValue, result => { statItemIncResult = result; });
            while (statItemIncResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(statItemIncResult, "Add StatItem Value");
            Assert.IsTrue(!statItemIncResult.IsError);
            Assert.IsTrue(statItemIncResult.Value.currentValue == (getStatItemsResult.Value[0].value+increasedValue));
        }
        
        [UnityTest, Order(999)]
        public IEnumerator Teardown()
        {
            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;
                
                this.user.Logout(r => logoutResult = r);

                while (logoutResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }
            }
        }
        
    }
}
