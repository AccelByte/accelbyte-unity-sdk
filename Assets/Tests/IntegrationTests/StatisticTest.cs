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

            statistic = AccelBytePlugin.GetStatistic();

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
            helper.GetStatByStatCode("SDKTEST", accessToken, result => { GetStatResult = result; });

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
                        description = "Stat for SDK Test",
                        incrementOnly = true,
                        maximum = 999999,
                        minimum = 0,
                        name = "SDKTEST",
                        setAsGlobal = true,
                        setBy = StatisticSetBy.CLIENT,
                        statCode = "SDKTEST",
                        tags = new string[] {"SDKTEST"}
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

            bool UserStatItemIsExist = false;
            Result<StatItemPagingSlicedResult> GetStatItemResult = null;
            statistic.GetUserStatItemsByStatCodes(new string[] { "SDKTEST" }, result => { GetStatItemResult = result; });
            while (GetStatItemResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            if (GetStatItemResult.Value.data.Length == 0)
            {
                Result<BulkStatItemOperationResult[]> CreateStatItemResult = null;
                helper.BulkCreateUserStatItem(user.Session.UserId, new string[] { "SDKTEST" }, accessToken, result => { CreateStatItemResult = result; });
                while (CreateStatItemResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                if (!CreateStatItemResult.IsError)
                {
                    UserStatItemIsExist = true;
                }
            }
            else
            {
                UserStatItemIsExist = true;
            }
            Assert.IsTrue(UserStatItemIsExist);
        }

        [UnityTest, Order(1)]
        public IEnumerator GetAllUserStatItems()
        {
            Result<StatItemPagingSlicedResult> getAllStatItemsResult = null;
            statistic.GetAllUserStatItems(result => { getAllStatItemsResult = result; });
            while (getAllStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(getAllStatItemsResult, "Get All StatItems");
            Assert.That(!getAllStatItemsResult.IsError);
            Assert.That(getAllStatItemsResult.Value.data.Length > 0);
        }

        [UnityTest, Order(1)]
        public IEnumerator GetUserStatItemsByStatCodes()
        {
            Result<StatItemPagingSlicedResult> getStatItemsResult = null;
            string[] statCodes = new string[] { "SDKTEST", "TOTAL_ASSISTS" };
            statistic.GetUserStatItemsByStatCodes(statCodes, result => { getStatItemsResult = result; });
            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(getStatItemsResult, "Get StatItems by StatCodes");
            Assert.That(!getStatItemsResult.IsError);
            Assert.That(getStatItemsResult.Value.data.Length > 0);
            Assert.That(getStatItemsResult.Value.data[0].statCode == statCodes[0]);
        }

        [UnityTest, Order(1)]
        public IEnumerator GetUserStatItemsByTags()
        {
            Result<StatItemPagingSlicedResult> getStatItemsResult = null;
            string[] Tags = new string[] { "SDKTEST" };
            statistic.GetUserStatItemsByTags(Tags, result => { getStatItemsResult = result; });
            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(getStatItemsResult, "Get StatItems by Tags");
            Assert.That(!getStatItemsResult.IsError);
            Assert.That(getStatItemsResult.Value.data.Length > 0);
            Assert.That(getStatItemsResult.Value.data[0].tags[0] == Tags[0]);
        }

        [UnityTest, Order(1)]
        public IEnumerator BulkAddStatItemValue()
        {
            Result<BulkStatItemOperationResult[]> BulkAddStatItemValueResult = null;
            BulkUserStatItemInc SDKTEST = new BulkUserStatItemInc
            {
                inc = 1,
                userId = user.Session.UserId,
                statCode = "SDKTEST"
            };
           
            BulkUserStatItemInc[] data = new BulkUserStatItemInc[] { SDKTEST};

            statistic.BulkAddStatItemValue(data, result => { BulkAddStatItemValueResult = result; });
            while(BulkAddStatItemValueResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(BulkAddStatItemValueResult, "Bulk Add StatItem Value");
            Assert.IsTrue(!BulkAddStatItemValueResult.IsError);
            Assert.IsTrue(BulkAddStatItemValueResult.Value[0].success);
            Assert.That(BulkAddStatItemValueResult.Value[0].statCode == SDKTEST.statCode);
        }

        [UnityTest, Order(1)]
        public IEnumerator BulkAddUserStatItemValue()
        {
            Result<BulkStatItemOperationResult[]> BulkAddStatItemValueResult = null;
            BulkStatItemInc SDKTEST = new BulkStatItemInc
            {
                inc = 1,
                statCode = "SDKTEST"
            };

            BulkStatItemInc[] data = new BulkStatItemInc[] { SDKTEST };

            statistic.BulkAddUserStatItemValue(data, result => { BulkAddStatItemValueResult = result; });
            while (BulkAddStatItemValueResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(BulkAddStatItemValueResult, "Bulk Add StatItem Value");
            Assert.IsTrue(!BulkAddStatItemValueResult.IsError);
            Assert.IsTrue(BulkAddStatItemValueResult.Value[0].success);
            Assert.That(BulkAddStatItemValueResult.Value[0].statCode == SDKTEST.statCode);
        }

        [UnityTest, Order(1)]
        public IEnumerator AddUserStatItemValue()
        {
            Result<StatItemPagingSlicedResult> getStatItemsResult = null;
            string statCode = "SDKTEST";
            statistic.GetUserStatItemsByStatCodes(new string[] { statCode }, result => { getStatItemsResult = result; });
            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            Result<StatItemIncResult> statItemIncResult = null;
            float increasedValue = 1;


            statistic.AddUserStatItemValue(statCode, increasedValue, result => { statItemIncResult = result; });
            while (statItemIncResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.LogResult(statItemIncResult, "Add StatItem Value");
            Assert.IsTrue(!statItemIncResult.IsError);
            Assert.IsTrue(statItemIncResult.Value.currentValue == (getStatItemsResult.Value.data[0].value + increasedValue));
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
