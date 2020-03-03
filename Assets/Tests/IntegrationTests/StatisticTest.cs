// Copyright (c) 2019 - 2020 AccelByte Inc. All Rights Reserved.
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
using System.Linq;
using UnityEngine;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class StatisticTest
    {
        private Statistic statistic = null;
        private readonly TestHelper helper = new TestHelper();
        private string helperAccessToken = null;
        private User user;

        private readonly string[] statCodes =
        {
            "client-unity-1", "client-unity-2", "client-unity-3", "client-unity-4", "client-unity-5", "client-unity-6"
        };

        private readonly string[] tags = { "client_tag_1", "client_tag_1", "client_tag_2", "client_tag_2", "client_tag_3", "client_tag_3" };

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
            this.statistic = AccelBytePlugin.GetStatistic();

            //Get AccessToken
            Result<TokenData> GetAccessToken = null;
            this.helper.GetAccessToken(result => { GetAccessToken = result; });

            while (GetAccessToken == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            this.helperAccessToken = GetAccessToken.Value.access_token;

            for (int i = 0; i < this.statCodes.Length - 1; i++)
            {
                Debug.Log("Start to create stat! " + this.statCodes[i]);
                Result<StatConfig> createStatResult = null;
                TestHelper.StatCreateModel createStat = new TestHelper.StatCreateModel
                {
                    defaultValue = 0,
                    description = "Stat for SDK Test",
                    incrementOnly = true,
                    maximum = 999999,
                    minimum = 0,
                    name = this.statCodes[i],
                    setAsGlobal = false,
                    setBy = StatisticSetBy.CLIENT,
                    statCode = this.statCodes[i],
                    tags = new[] {this.tags[i]}
                };

                this.helper.CreateStatConfig(
                    this.helperAccessToken,
                    createStat,
                    result => { createStatResult = result; });

                while (createStatResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

            Debug.Log("Start to create stat! " + this.statCodes[5]);
            Result<StatConfig> createStat6Result = null;
            TestHelper.StatCreateModel createStat6 = new TestHelper.StatCreateModel
            {
                defaultValue = 0,
                description = "Stat for SDK Test",
                incrementOnly = false,
                maximum = 999999,
                minimum = 0,
                name = this.statCodes[5],
                setAsGlobal = false,
                setBy = StatisticSetBy.CLIENT,
                statCode = this.statCodes[5],
                tags = new[] { this.tags[5] }
            };

            this.helper.CreateStatConfig(
                this.helperAccessToken,
                createStat6,
                result => { createStat6Result = result; });

            while (createStat6Result == null)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        [UnityTest, Order(1), Timeout(10000)]
        public IEnumerator CreateUserStatItems_ValidStatConfig_StatItemCreated()
        {
            //Arrange
            foreach (string statCode in this.statCodes)
            {
                Result deleteResult = null;
                this.helper.DeleteStatItem(
                    this.helperAccessToken,
                    this.user.Session.UserId,
                    statCode,
                    result => deleteResult = result);

                while (deleteResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                TestHelper.LogResult(deleteResult, "Delete StatItem");
            }

            //Act
            Result<StatItemOperationResult[]> createStatItemResult = null;
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();

            this.statistic.CreateUserStatItems(request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(createStatItemResult, "Create StatItem");
            
            //Assert
            Assert.IsFalse(createStatItemResult.IsError);
            Assert.That(createStatItemResult.Value.All(result => result.success));
        }

        [UnityTest, Order(2)]
        public IEnumerator GetAllUserStatItems_ReturnsAllCreatedUserStatItems()
        {
            //Arrange
            Result<StatItemOperationResult[]> createStatItemResult = null;
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();

            this.statistic.CreateUserStatItems(request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<PagedStatItems> getAllStatItemsResult = null;
            this.statistic.GetAllUserStatItems(result => { getAllStatItemsResult = result; });

            while (getAllStatItemsResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            TestHelper.LogResult(getAllStatItemsResult, "Get All StatItems");
            
            //Assert
            Assert.IsFalse(getAllStatItemsResult.IsError);
            Assert.That(getAllStatItemsResult.Value.data.Length, Is.GreaterThanOrEqualTo(this.statCodes.Length));
            Assert.That(
                getAllStatItemsResult.Value.data.Select(statItem => statItem.statCode)
                    .Join(this.statCodes, s => s, s => s, (left, right) => left == right)
                    .All(found => found));
        }

        [UnityTest, Order(2)]
        public IEnumerator GetUserStatItems_ByStatCodes_FoundExactMatches()
        {
            //Arrange
            Result<StatItemOperationResult[]> createStatItemResult = null;
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();

            this.statistic.CreateUserStatItems(request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<PagedStatItems> getStatItemsResult = null;
            string[] filterStatCodes = {this.statCodes[3], this.statCodes[4]};
            this.statistic.GetUserStatItems(filterStatCodes, null, result => { getStatItemsResult = result; });

            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(getStatItemsResult, "Get StatItems by StatCodes");
            
            //Assert
            Assert.IsFalse(getStatItemsResult.IsError);
            Assert.That(getStatItemsResult.Value.data.Length, Is.EqualTo(2));
            Assert.That(getStatItemsResult.Value.data.Count(x => x.statCode == this.statCodes[3]), Is.EqualTo(1));
            Assert.That(getStatItemsResult.Value.data.Count(x => x.statCode == this.statCodes[4]), Is.EqualTo(1));
        }

        [UnityTest, Order(2)]
        public IEnumerator GetUserStatItems_ByTags_FoundAllStatItemInTags()
        {
            //Arrange
            Result<StatItemOperationResult[]> createStatItemResult = null;
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();
            this.statistic.CreateUserStatItems(request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<PagedStatItems> getStatItemsResult = null;
            string[] filterTags = {this.tags[0]};
            this.statistic.GetUserStatItems(null, filterTags, result => { getStatItemsResult = result; });

            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(getStatItemsResult, "Get StatItems by Tags");
            
            //Assert
            Assert.IsFalse(getStatItemsResult.IsError);
            Assert.That(
                getStatItemsResult.Value.data.Length,
                Is.EqualTo(this.tags.Count(tag => tag == this.tags[0])));
            Assert.That(
                getStatItemsResult.Value.data.Count(x => x.tags.Any(tag => tag == this.tags[0])),
                Is.GreaterThan(1));
        }

        [UnityTest, Order(3)]
        public IEnumerator IncrementUserStatItems_By777_StatItemsIncreased()
        {
            //Arrange
            Result<StatItemOperationResult[]> createStatItemResult = null;
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();

            this.statistic.CreateUserStatItems(request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<StatItemOperationResult[]> incrementResult = null;
            StatItemIncrement sdktest = new StatItemIncrement {inc = 777, statCode = this.statCodes[0]};
            StatItemIncrement[] data = {sdktest};
            this.statistic.IncrementUserStatItems(data, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult = null;
            this.statistic.GetUserStatItems(
                new[] {this.statCodes[0]},
                null,
                result => { getStatItemsResult = result; });

            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(incrementResult, "Increment User StatItems");
            Assert.IsFalse(incrementResult.IsError);
            Assert.That(incrementResult.Value[0].statCode, Is.EqualTo(sdktest.statCode));
            Assert.IsTrue(incrementResult.Value[0].success);
            Assert.That(getStatItemsResult.Value.data[0].value, Is.EqualTo(777));
        }

        [UnityTest, Order(4)]
        public IEnumerator IncrementUserStatItems_ByNegative_Failed()
        {
            //Arrange
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() { statCode = statCode })
                .ToArray();
            Result<StatItemOperationResult[]> createStatItemResult = null;
            this.statistic.CreateUserStatItems(request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            StatItemIncrement sdktest = new StatItemIncrement { inc = -777, statCode = this.statCodes[0] };
            StatItemIncrement[] data = { sdktest };
            Result<StatItemOperationResult[]> incrementResult = null;
            this.statistic.IncrementUserStatItems(data, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult = null;
            this.statistic.GetUserStatItems(
                new[] { this.statCodes[0] },
                null,
                result => { getStatItemsResult = result; });

            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(incrementResult, "Increment User StatItems");
            Assert.IsFalse(incrementResult.IsError);
            Assert.That(incrementResult.Value[0].statCode, Is.EqualTo(sdktest.statCode));
            Assert.IsFalse(incrementResult.Value[0].success);
            Assert.That(getStatItemsResult.Value.data[0].value, Is.EqualTo(777));
        }

        [UnityTest, Order(5)]
        public IEnumerator DecrementUserStatItems_By777_StatItemsDecreased()
        {
            //Arrange
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() { statCode = statCode })
                .ToArray();
            Result<StatItemOperationResult[]> createStatItemResult = null;
            this.statistic.CreateUserStatItems(request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            StatItemIncrement sdktestIncrement = new StatItemIncrement { inc = 777, statCode = this.statCodes[5] };
            StatItemIncrement[] dataIncrement = { sdktestIncrement };
            Result<StatItemOperationResult[]> incrementResult = null;
            this.statistic.IncrementUserStatItems(dataIncrement, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Act
            StatItemIncrement sdktestDecrement = new StatItemIncrement { inc = -777, statCode = this.statCodes[5] };
            StatItemIncrement[] dataDecrement = { sdktestDecrement };
            Result<StatItemOperationResult[]> decrementResult = null;
            this.statistic.IncrementUserStatItems(dataDecrement, result => { decrementResult = result; });

            while (decrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult = null;
            this.statistic.GetUserStatItems(
                new[] { this.statCodes[5] },
                null,
                result => { getStatItemsResult = result; });

            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(incrementResult, "Increment User StatItems");
            Assert.IsFalse(incrementResult.IsError);
            Assert.That(incrementResult.Value[0].statCode, Is.EqualTo(sdktestIncrement.statCode));
            Assert.IsTrue(incrementResult.Value[0].success);
            TestHelper.LogResult(incrementResult, "Decrement User StatItems");
            Assert.IsFalse(decrementResult.IsError);
            Assert.That(decrementResult.Value[0].statCode, Is.EqualTo(sdktestDecrement.statCode));
            Assert.IsTrue(decrementResult.Value[0].success);
            Assert.That(getStatItemsResult.Value.data[0].value, Is.EqualTo(0));
        }

        [UnityTest, Order(999)]
        public IEnumerator Teardown()
        {
            foreach (string statCode in this.statCodes)
            {
                Result deleteResult = null;

                this.helper.DeleteStatItem(
                    this.helperAccessToken,
                    this.user.Session.UserId,
                    statCode,
                    result => deleteResult = result);

                while (deleteResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }

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
