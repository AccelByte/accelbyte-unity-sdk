// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using Steamworks;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class ServerStatisticTest
    {
        private ServerStatistic statistic = null;
        private readonly TestHelper helper = new TestHelper();

        private string helperAccessToken = null;

        // private User user;
        private string userId1;
        private string userId2;
        private DedicatedServer server;

        private readonly string[] statCodes =
        {
            "server-unity-1", "server-unity-2", "server-unity-3", "server-unity-4", "server-unity-5", "server-unity-7"
        };

        private readonly string[] tags =
        {
            "server_tag_1", "server_tag_1", "server_tag_2", "server_tag_2", "server_tag_3", "server_tag_3"
        };

        [UnityTest, Order(0)]
        public IEnumerator Setup()
        {
            var user = AccelBytePlugin.GetUser();
            this.server = AccelByteServerPlugin.GetDedicatedServer();

            Result loginResult = null;

            this.server.LoginWithClientCredentials(result => loginResult = result);

            while (loginResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result loginWithDevice = null;
            user.LoginWithDeviceId(result => { loginWithDevice = result; });

            while (loginWithDevice == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            this.userId1 = user.Session.UserId;

            Result logoutResult = null;
            user.Logout(result => logoutResult = result);

            while (logoutResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            var steamTicketBuilder = new StringBuilder();

            if (SteamManager.Initialized)
            {
                var ticket = new byte[1024];
                uint actualTicketLength;
                SteamUser.GetAuthSessionTicket(ticket, ticket.Length, out actualTicketLength);
                Array.Resize(ref ticket, (int) actualTicketLength);

                foreach (byte b in ticket)
                {
                    steamTicketBuilder.AppendFormat("{0:x2}", b);
                }
            }

            Result steamLoginResult = null;
            user.LoginWithOtherPlatform(
                PlatformType.Steam,
                steamTicketBuilder.ToString(),
                result => steamLoginResult = result);

            while (steamLoginResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            this.userId2 = user.Session.UserId;

            this.statistic = AccelByteServerPlugin.GetStatistic();

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
                    setBy = StatisticSetBy.SERVER,
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
                setBy = StatisticSetBy.SERVER,
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

            foreach (string statCode in this.statCodes)
            {
                Result deleteResult = null;

                this.helper.DeleteStatItem(
                    this.helperAccessToken,
                    this.userId1,
                    statCode,
                    result => deleteResult = result);

                while (deleteResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                deleteResult = null;

                this.helper.DeleteStatItem(
                    this.helperAccessToken,
                    this.userId2,
                    statCode,
                    result => deleteResult = result);

                while (deleteResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
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
                    this.userId1,
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
                .Select(statCode => new CreateStatItemRequest {statCode = statCode})
                .ToArray();

            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

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

            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<PagedStatItems> getAllStatItemsResult = null;
            this.statistic.GetAllUserStatItems(this.userId1, result => { getAllStatItemsResult = result; });

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

            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<PagedStatItems> getStatItemsResult = null;
            string[] filterStatCodes = {this.statCodes[3], this.statCodes[4]};
            this.statistic.GetUserStatItems(
                this.userId1,
                filterStatCodes,
                null,
                result => { getStatItemsResult = result; });

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

        [UnityTest, Order(3)]
        public IEnumerator GetUserStatItems_ByTags_FoundAllStatItemInTags()
        {
            //Arrange
            Result<StatItemOperationResult[]> createStatItemResult = null;
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();

            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<PagedStatItems> getStatItemsResult = null;
            string[] filterTags = {this.tags[0]};
            this.statistic.GetUserStatItems(this.userId1, null, filterTags, result => { getStatItemsResult = result; });

            while (getStatItemsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(getStatItemsResult, "Get StatItems by Tags");

            //Assert
            Assert.IsFalse(getStatItemsResult.IsError);
            Assert.That(getStatItemsResult.Value.data.Length, Is.EqualTo(this.tags.Count(tag => tag == this.tags[0])));
            Assert.That(
                getStatItemsResult.Value.data.Count(x => x.tags.Any(tag => tag == this.tags[0])),
                Is.GreaterThan(1));
        }

        [UnityTest, Order(4)]
        public IEnumerator IncrementUserStatItems_By777_StatItemsIncreased()
        {
            //Arrange
            Result<StatItemOperationResult[]> createStatItemResult = null;
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();

            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            Result<StatItemOperationResult[]> incrementResult = null;
            StatItemIncrement sdktest = new StatItemIncrement {inc = 777, statCode = this.statCodes[0]};
            StatItemIncrement[] data = {sdktest};
            this.statistic.IncrementUserStatItems(this.userId1, data, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult = null;
            this.statistic.GetUserStatItems(
                this.userId1,
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

        [UnityTest, Order(5)]
        public IEnumerator IncrementUserStatItems_ByNegative_Failed()
        {
            //Arrange
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() { statCode = statCode })
                .ToArray();
            Result<StatItemOperationResult[]> createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            StatItemIncrement sdktest = new StatItemIncrement { inc = -777, statCode = this.statCodes[0] };
            StatItemIncrement[] data = { sdktest };
            Result<StatItemOperationResult[]> incrementResult = null;
            this.statistic.IncrementUserStatItems(this.userId1, data, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult = null;
            this.statistic.GetUserStatItems(
                this.userId1,
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

        [UnityTest, Order(6)]
        public IEnumerator DecrementUserStatItems_By777_StatItemsDecreased()
        {
            //Arrange
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest() {statCode = statCode})
                .ToArray();
            Result<StatItemOperationResult[]> createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            StatItemIncrement sdktestIncrement = new StatItemIncrement { inc = 777, statCode = this.statCodes[5] };
            StatItemIncrement[] dataIncrement = { sdktestIncrement };
            Result<StatItemOperationResult[]> incrementResult = null;
            this.statistic.IncrementUserStatItems(this.userId1, dataIncrement, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Act
            StatItemIncrement sdktestDecrement = new StatItemIncrement { inc = -777, statCode = this.statCodes[5] };
            StatItemIncrement[] dataDecrement = { sdktestDecrement };
            Result<StatItemOperationResult[]> decrementResult = null;
            this.statistic.IncrementUserStatItems(this.userId1, dataDecrement, result => { decrementResult = result; });

            while (decrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult = null;
            this.statistic.GetUserStatItems(
                this.userId1,
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
            Assert.IsFalse(decrementResult.IsError);
            Assert.That(decrementResult.Value[0].statCode, Is.EqualTo(sdktestDecrement.statCode));
            Assert.IsTrue(decrementResult.Value[0].success);
            Assert.That(getStatItemsResult.Value.data[0].value, Is.EqualTo(0));
        }

        [UnityTest, Order(7)]
        public IEnumerator IncrementManyUsersStatItems_By777_StatItemsIncreased()
        {
            //Arrange
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest { statCode = statCode })
                .ToArray();
            Result<StatItemOperationResult[]> createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId2, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Act
            UserStatItemIncrement[] data =
            {
                new UserStatItemIncrement {userId = userId1, inc = 777, statCode = this.statCodes[1]},
                new UserStatItemIncrement {userId = userId2, inc = 777, statCode = this.statCodes[1]}
            };
            Result<StatItemOperationResult[]> incrementResult = null;
            this.statistic.IncrementManyUsersStatItems(data, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult1 = null;
            this.statistic.GetUserStatItems(
                this.userId1,
                new[] { this.statCodes[1] },
                null,
                result => { getStatItemsResult1 = result; });

            while (getStatItemsResult1 == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<PagedStatItems> getStatItemsResult2 = null;
            this.statistic.GetUserStatItems(
                this.userId1,
                new[] { this.statCodes[1] },
                null,
                result => { getStatItemsResult2 = result; });

            while (getStatItemsResult2 == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(incrementResult, "Increment User StatItems");
            Assert.IsFalse(incrementResult.IsError);
            Assert.That(getStatItemsResult1.Value.data[0].value, Is.EqualTo(777));
            Assert.That(getStatItemsResult2.Value.data[0].value, Is.EqualTo(777));
        }

        [UnityTest, Order(8)]
        public IEnumerator IncrementManyUsersStatItems_ByNegative_Failed()
        {
            //Arrange
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest { statCode = statCode })
                .ToArray();
            Result<StatItemOperationResult[]> createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId2, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }


            //Act
            UserStatItemIncrement[] data =
            {
                new UserStatItemIncrement {userId = userId1, inc = -777, statCode = this.statCodes[1]},
                new UserStatItemIncrement {userId = userId2, inc = -777, statCode = this.statCodes[1]}
            };
            Result<StatItemOperationResult[]> incrementResult = null;
            this.statistic.IncrementManyUsersStatItems(data, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult1 = null;
            this.statistic.GetUserStatItems(
                this.userId1,
                new[] {this.statCodes[1]},
                null,
                result => { getStatItemsResult1 = result; });

            while (getStatItemsResult1 == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<PagedStatItems> getStatItemsResult2 = null;
            this.statistic.GetUserStatItems(
                this.userId1,
                new[] {this.statCodes[1]},
                null,
                result => { getStatItemsResult2 = result; });

            while (getStatItemsResult2 == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(incrementResult, "Increment User StatItems");
            Assert.IsFalse(incrementResult.IsError);
            for (int i = 0; i < incrementResult.Value.Length; i++)
            {
                Assert.That(incrementResult.Value[i].statCode, Is.EqualTo(data[i].statCode));
                Assert.IsFalse(incrementResult.Value[i].success);
            }
            Assert.That(getStatItemsResult1.Value.data[0].value, Is.EqualTo(777));
            Assert.That(getStatItemsResult2.Value.data[0].value, Is.EqualTo(777));
        }

        [UnityTest, Order(9)]
        public IEnumerator DecrementManyUsersStatItems_By777_StatItemsDecrease()
        {
            //Arrange
            CreateStatItemRequest[] request = this.statCodes
                .Select(statCode => new CreateStatItemRequest { statCode = statCode })
                .ToArray();
            Result<StatItemOperationResult[]> createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId1, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            createStatItemResult = null;
            this.statistic.CreateUserStatItems(this.userId2, request, result => { createStatItemResult = result; });

            while (createStatItemResult == null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            UserStatItemIncrement[] dataIncrement =
            {
                new UserStatItemIncrement {userId = userId1, inc = 777, statCode = this.statCodes[5]},
                new UserStatItemIncrement {userId = userId2, inc = 777, statCode = this.statCodes[5]}
            };
            Result<StatItemOperationResult[]> incrementResult = null;
            this.statistic.IncrementManyUsersStatItems(dataIncrement, result => { incrementResult = result; });

            while (incrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Act
            UserStatItemIncrement[] dataDecrement =
            {
                new UserStatItemIncrement {userId = userId1, inc = -777, statCode = this.statCodes[5]},
                new UserStatItemIncrement {userId = userId2, inc = -777, statCode = this.statCodes[5]}
            };
            Result<StatItemOperationResult[]> decrementResult = null;
            this.statistic.IncrementManyUsersStatItems(dataDecrement, result => { decrementResult = result; });

            while (decrementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            //Assert
            Result<PagedStatItems> getStatItemsResult1 = null;
            this.statistic.GetUserStatItems(
                this.userId1,
                new[] { this.statCodes[5] },
                null,
                result => { getStatItemsResult1 = result; });

            while (getStatItemsResult1 == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<PagedStatItems> getStatItemsResult2 = null;
            this.statistic.GetUserStatItems(
                this.userId1,
                new[] { this.statCodes[5] },
                null,
                result => { getStatItemsResult2 = result; });

            while (getStatItemsResult2 == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.LogResult(incrementResult, "Increment User StatItems");
            Assert.IsFalse(incrementResult.IsError);
            for (int i = 0; i < incrementResult.Value.Length; i++)
            {
                Assert.That(incrementResult.Value[i].statCode, Is.EqualTo(dataIncrement[i].statCode));
                Assert.IsTrue(incrementResult.Value[i].success);
            }
            Assert.IsFalse(decrementResult.IsError);
            for (int i = 0; i < decrementResult.Value.Length; i++)
            {
                Assert.That(decrementResult.Value[i].statCode, Is.EqualTo(dataDecrement[i].statCode));
                Assert.IsTrue(decrementResult.Value[i].success);
            }
            Assert.That(getStatItemsResult1.Value.data[0].value, Is.EqualTo(0));
            Assert.That(getStatItemsResult2.Value.data[0].value, Is.EqualTo(0));
        }
    }
}
