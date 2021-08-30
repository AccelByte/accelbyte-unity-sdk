// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Server;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    [TestFixture]
    public class AchievementTest
    {
        private User user;

        string adminAccessToken = "";
        string userId = "";

        static TestHelper.AchievementRequest achievement1 = new TestHelper.AchievementRequest
        {
            achievementCode = "UnityAchievementCode1",
            defaultLanguage = "en",
            description = new Dictionary<string, string>
            {
                {"en", "This is achievement 1 en description"}
            },
            goalValue = 7.7f,
            hidden = true,
            incremental = false,
            lockedIcons = new AchievementIcon[]
            {
                new AchievementIcon
                {
                    url = "This is locked icon 1 url",
                    slug = "This is locked icon 1 description"
                },
                new AchievementIcon
                {
                    url = "This is locked icon 2 url",
                    slug = "This is locked icon 2 slug"
                }
            },
            name = new Dictionary<string, string>
            {
                {"en", "This is achievement 1 en name"}
            },
            statCode = "This is achievement 1 stat code",
            tags = new string[] { "Tags1", "Tags2" },
            unlockedIcons = new AchievementIcon[]
            {
                new AchievementIcon
                {
                    url = "This is unlocked icon 1 url",
                    slug = "This is unlocked icon 1 slug"
                },
                new AchievementIcon
                {
                    url = "This is unlocked icon 2 url",
                    slug = "This is unlocked icon 2 slug"
                }
            },
        };

        static TestHelper.AchievementRequest achievement2 = new TestHelper.AchievementRequest
        {
            achievementCode = "UnityAchievementCode2",
            defaultLanguage = "id",
            description = new Dictionary<string, string>
            {
                {"en", "This is achievement 2 en description"},
                {"id", "This is achievement 2 id description"},
            },
            goalValue = 77.77f,
            hidden = true,
            incremental = false,
            lockedIcons = new AchievementIcon[]
            {
                new AchievementIcon
                {
                    url = "This is locked icon 1 url",
                    slug = "This is locked icon 1 description"
                },
                new AchievementIcon
                {
                    url = "This is locked icon 2 url",
                    slug = "This is locked icon 2 slug"
                }
            },
            name = new Dictionary<string, string>
            {
                {"en", "This is achievement 2 en name"},
                {"id", "This is achievement 2 id name"},
            },
            statCode = "This is achievement 2 stat code",
            tags = new string[] { "Tags1", "Tags2" },
            unlockedIcons = new AchievementIcon[]
            {
                new AchievementIcon
                {
                    url = "This is unlocked icon 1 url",
                    slug = "This is unlocked icon 1 slug"
                },
                new AchievementIcon
                {
                    url = "This is unlocked icon 2 url",
                    slug = "This is unlocked icon 2 slug"
                }
            },
        };

        static readonly TestHelper.StatCreateModel achievementStatisticServerRequest = new TestHelper.StatCreateModel
        {
            defaultValue = 0,
            description = "Stat for SDK Test",
            incrementOnly = true,
            maximum = 999999,
            minimum = 0,
            name = "Achievement Incremental Testing Server",
            setAsGlobal = false,
            setBy = StatisticSetBy.SERVER,
            statCode = "incremental-testing-server",
            tags = new[] {"nothing"}
        };

        static readonly TestHelper.StatCreateModel achievementStatisticClientRequest = new TestHelper.StatCreateModel
        {
            defaultValue = 0,
            description = "Stat for SDK Test",
            incrementOnly = true,
            maximum = 999999,
            minimum = 0,
            name = "Achievement Incremental Testing Client",
            setAsGlobal = false,
            setBy = StatisticSetBy.CLIENT,
            statCode = "incremental-testing-client",
            tags = new[] {"nothing"}
        };
        
        // This achievement is used for UnityTest Order(6)
        private static readonly TestHelper.AchievementRequest achievementIncrementalServer = new TestHelper.AchievementRequest
        {
            achievementCode = "UnityAchievementCode3",
            defaultLanguage = "en",
            description = new Dictionary<string, string>
            {
                {"en", "This is achievement 3 en description"},
                {"id", "This is achievement 3 id description"},
            },
            goalValue = 500.005f,
            hidden = true,
            incremental = true,
            lockedIcons = new[]
            {
                new AchievementIcon
                {
                    url = "This is locked icon 3 url",
                    slug = "This is locked icon 3 description"
                },
                new AchievementIcon
                {
                    url = "This is locked icon 3 url",
                    slug = "This is locked icon 3 slug"
                }
            },
            name = new Dictionary<string, string>
            {
                {"en", "This is achievement 3 en name"},
                {"id", "This is achievement 3 id name"},
            },
            statCode = achievementStatisticServerRequest.statCode,
            tags = new[] { "Tags1", "Tags2" },
            unlockedIcons = new[]
            {
                new AchievementIcon
                {
                    url = "This is unlocked icon 3 url",
                    slug = "This is unlocked icon 3 slug"
                },
                new AchievementIcon
                {
                    url = "This is unlocked icon 3 url",
                    slug = "This is unlocked icon 3 slug"
                }
            },
        };
        private static readonly TestHelper.AchievementRequest achievementIncrementalClient = new TestHelper.AchievementRequest
        {
            achievementCode = "UnityAchievementCode4",
            defaultLanguage = "en",
            description = new Dictionary<string, string>
            {
                {"en", "This is achievement 4 en description"},
                {"id", "This is achievement 4 id description"},
            },
            goalValue = 100.001f,
            hidden = true,
            incremental = true,
            lockedIcons = new[]
            {
                new AchievementIcon
                {
                    url = "This is locked icon 4 url",
                    slug = "This is locked icon 4 description"
                },
                new AchievementIcon
                {
                    url = "This is locked icon 4 url",
                    slug = "This is locked icon 4 slug"
                }
            },
            name = new Dictionary<string, string>
            {
                {"en", "This is achievement 4 en name"},
                {"id", "This is achievement 4 id name"},
            },
            statCode = achievementStatisticClientRequest.statCode,
            tags = new[] { "Tags1", "Tags2" },
            unlockedIcons = new[]
            {
                new AchievementIcon
                {
                    url = "This is unlocked icon 4 url",
                    slug = "This is unlocked icon 4 slug"
                },
                new AchievementIcon
                {
                    url = "This is unlocked icon 4 url",
                    slug = "This is unlocked icon 4 slug"
                }
            },
        };
        
        private readonly TestHelper.StatCreateModel[] allStatisticRequests =
        {
            achievementStatisticClientRequest,
            achievementStatisticServerRequest
        };

        private readonly TestHelper.AchievementRequest[] allAchievementRequests =
        {
            achievement1, 
            achievement2, 
            achievementIncrementalClient,
            achievementIncrementalServer
        };

        [UnityTest, TestLog, Order(0), Timeout(100000)]
        public IEnumerator Setup()
        {
            this.user = AccelBytePlugin.GetUser();

            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;

                this.user.Logout(r => logoutResult = r);

                while (logoutResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsFalse(logoutResult.IsError, "User cannot log out.");
            }

            Result loginWithDeviceResult = null;
            this.user.LoginWithDeviceId(result => { loginWithDeviceResult = result; });

            while (loginWithDeviceResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            userId = user.Session.UserId;

            TestHelper testHelper = new TestHelper();
            Result<TokenData> getAccessTokenResult = null;
            testHelper.GetAccessToken(result => { getAccessTokenResult = result; });

            while (getAccessTokenResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            adminAccessToken = getAccessTokenResult.Value.access_token;

            // Setup Statistic StatCode
            Result<TokenData> getClientTokenResult = null;
            testHelper.GetAccessToken(result => { getClientTokenResult = result; }); 
            while (getClientTokenResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.Assert.IsFalse(getClientTokenResult.IsError, "Cannot get client access token.");

            foreach (var statisticRequest in allStatisticRequests)
            {
                Result<StatConfig> createStatConfigResult = null;
                testHelper.CreateStatConfig(getClientTokenResult.Value.access_token, statisticRequest, result => { createStatConfigResult = result; });
                while (createStatConfigResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                
                Result<StatItemOperationResult[]> createStatItemResult = null;
                CreateStatItemRequest userStatItemRequest = new CreateStatItemRequest{statCode = statisticRequest.statCode};
    
                AccelBytePlugin.GetStatistic().CreateUserStatItems(new[]{userStatItemRequest}, result => { createStatItemResult = result; }); 
                while (createStatItemResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                TestHelper.Assert.IsFalse(createStatItemResult.IsError, "Cannot create user stat item.");
            }


            // Setup Achievement
            foreach (var achievementRequest in allAchievementRequests)
            {
                Result deleteAchievementResult = null;
                testHelper.DeleteAchievement(adminAccessToken, achievementRequest.achievementCode, result => { deleteAchievementResult = result; });

                while (deleteAchievementResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }

                Result<TestHelper.AchievementResponse> createdAchievementResult = null;
                testHelper.CreateAchievement(adminAccessToken, achievementRequest, result => { createdAchievementResult = result; });
    
                while (createdAchievementResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }

                TestHelper.Assert.IsFalse(createdAchievementResult.IsError, "Cannot create achievement.");
            }

            DedicatedServer server = AccelByteServerPlugin.GetDedicatedServer();
            Result loginServerResult = null;
            server.LoginWithClientCredentials(result => loginServerResult = result);

            while (loginServerResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsFalse(getAccessTokenResult.IsError, "Cannot get access token.");
            TestHelper.Assert.IsFalse(loginWithDeviceResult.IsError, "User cannot log in with device.");
            TestHelper.Assert.IsTrue(!loginServerResult.IsError, "Server cannot login.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator QueryNamespaceAchievementsDefaultLanguage()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedPublicAchievement> paginatedAchievementsResult = null;
            achievement.QueryAchievements("", AchievementSortBy.NONE, result => { paginatedAchievementsResult = result; }, 0, 100);

            while (paginatedAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            bool isAchievement1Found = false;
            for (int i = 0; i < paginatedAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedAchievementsResult.Value.data[i].achievementCode == achievement1.achievementCode && paginatedAchievementsResult.Value.data[i].name == achievement1.name[achievement1.defaultLanguage])
                {
                    isAchievement1Found = true;
                    break;
                }
            }

            bool isAchievement2Found = false;
            for (int i = 0; i < paginatedAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedAchievementsResult.Value.data[i].achievementCode == achievement2.achievementCode && paginatedAchievementsResult.Value.data[i].name == achievement2.name[achievement2.defaultLanguage])
                {
                    isAchievement2Found = true;
                    break;
                }
            }

            TestHelper.Assert.IsFalse(paginatedAchievementsResult.IsError, "Cannot query achievements.");
            TestHelper.Assert.IsTrue(isAchievement1Found, "Expected achievement 1 is not found.");
            TestHelper.Assert.IsTrue(isAchievement2Found, "Expected achievement 2 is not found.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator QueryNamespaceAchievementsSpecificLanguage()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedPublicAchievement> paginatedEnAchievementsResult = null;
            achievement.QueryAchievements("en", AchievementSortBy.NONE, result => { paginatedEnAchievementsResult = result; }, 0, 100);

            while (paginatedEnAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            bool isEnAchievement1Found = false;
            for (int i = 0; i < paginatedEnAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedEnAchievementsResult.Value.data[i].achievementCode == achievement1.achievementCode && paginatedEnAchievementsResult.Value.data[i].name == achievement1.name["en"])
                {
                    isEnAchievement1Found = true;
                    break;
                }
            }

            bool isEnAchievement2Found = false;
            for (int i = 0; i < paginatedEnAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedEnAchievementsResult.Value.data[i].achievementCode == achievement2.achievementCode && paginatedEnAchievementsResult.Value.data[i].name == achievement2.name["en"])
                {
                    isEnAchievement2Found = true;
                    break;
                }
            }

            Result<PaginatedPublicAchievement> paginatedIdAchievementsResult = null;
            achievement.QueryAchievements("id", AchievementSortBy.NONE, result => { paginatedIdAchievementsResult = result; }, 0, 100);

            while (paginatedIdAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            bool isIdAchievement1Found = false;
            for (int i = 0; i < paginatedIdAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedIdAchievementsResult.Value.data[i].achievementCode == achievement1.achievementCode && paginatedIdAchievementsResult.Value.data[i].name == achievement1.name["en"])
                {
                    isIdAchievement1Found = true;
                    break;
                }
            }

            bool isIdAchievement2Found = false;
            for (int i = 0; i < paginatedIdAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedIdAchievementsResult.Value.data[i].achievementCode == achievement2.achievementCode && paginatedIdAchievementsResult.Value.data[i].name == achievement2.name["id"])
                {
                    isIdAchievement2Found = true;
                    break;
                }
            }

            TestHelper.Assert.IsFalse(paginatedIdAchievementsResult.IsError, "Cannot query en achievements.");
            TestHelper.Assert.IsTrue(isEnAchievement1Found, "Expected en achievement 1 is not found.");
            TestHelper.Assert.IsTrue(isEnAchievement2Found, "Expected en achievement 2 is not found.");
            TestHelper.Assert.IsFalse(paginatedIdAchievementsResult.IsError, "Cannot query id achievements.");
            TestHelper.Assert.IsTrue(isIdAchievement1Found, "Expected en achievement 1 is not found.");
            TestHelper.Assert.IsTrue(isIdAchievement2Found, "Expected id achievement 2 is not found.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator QueryNamespaceAchievementsCreatedAsc()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedPublicAchievement> paginatedAchievementsResult = null;
            achievement.QueryAchievements("", AchievementSortBy.CREATED_AT_ASC, result => { paginatedAchievementsResult = result; }, 0, 100);

            while (paginatedAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            int achievement1Order = 0;
            bool isAchievement1Found = false;
            for (int i = 0; i < paginatedAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedAchievementsResult.Value.data[i].achievementCode == achievement1.achievementCode)
                {
                    isAchievement1Found = true;
                    achievement1Order = i;
                    break;
                }
            }

            int achievement2Order = 0;
            bool isAchievement2Found = false;
            for (int i = 0; i < paginatedAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedAchievementsResult.Value.data[i].achievementCode == achievement2.achievementCode && paginatedAchievementsResult.Value.data[i].name == achievement2.name[achievement2.defaultLanguage])
                {
                    isAchievement2Found = true;
                    achievement2Order = i;
                    break;
                }
            }

            TestHelper.Assert.IsFalse(paginatedAchievementsResult.IsError, "Cannot query achievements.");
            TestHelper.Assert.IsTrue(isAchievement1Found, "Expected achievement 1 is not found.");
            TestHelper.Assert.IsTrue(isAchievement2Found, "Expected achievement 2 is not found.");
            TestHelper.Assert.IsTrue(achievement1Order < achievement2Order, "Achievement 1 number is higher than achievement 2.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator QueryNamespaceAchievementsCreatedDesc()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedPublicAchievement> paginatedAchievementsResult = null;
            achievement.QueryAchievements("", AchievementSortBy.CREATED_AT_DESC, result => { paginatedAchievementsResult = result; }, 0, 100);

            while (paginatedAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            int achievement1Order = 0;
            bool isAchievement1Found = false;
            for (int i = 0; i < paginatedAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedAchievementsResult.Value.data[i].achievementCode == achievement1.achievementCode)
                {
                    isAchievement1Found = true;
                    achievement1Order = i;
                    break;
                }
            }

            int achievement2Order = 0;
            bool isAchievement2Found = false;
            for (int i = 0; i < paginatedAchievementsResult.Value.data.Length; i++)
            {
                if (paginatedAchievementsResult.Value.data[i].achievementCode == achievement2.achievementCode)
                {
                    isAchievement2Found = true;
                    achievement2Order = i;
                    break;
                }
            }

            TestHelper.Assert.IsFalse(paginatedAchievementsResult.IsError, "Cannot query achievements.");
            TestHelper.Assert.IsTrue(isAchievement1Found, "Expected achievement 1 is not found.");
            TestHelper.Assert.IsTrue(isAchievement2Found, "Expected achievement 2 is not found.");
            TestHelper.Assert.IsTrue(achievement1Order > achievement2Order, "Achievement 1 number is lower than achievement 2.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator QueryNamespaceAchievementsLimit1()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedPublicAchievement> paginatedAchievementsResult = null;
            achievement.QueryAchievements("", AchievementSortBy.NONE, result => { paginatedAchievementsResult = result; }, 0, 1);

            while (paginatedAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsFalse(paginatedAchievementsResult.IsError, "Cannot query achievements.");
            TestHelper.Assert.That(paginatedAchievementsResult.Value.data.Length, Is.EqualTo(1));
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator QueryNamespaceAchievementsOffset1()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedPublicAchievement> paginatedAchievementsNoOffsetResult = null;
            achievement.QueryAchievements("", AchievementSortBy.CREATED_AT_ASC, result => { paginatedAchievementsNoOffsetResult = result; });

            while (paginatedAchievementsNoOffsetResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<PaginatedPublicAchievement> paginatedAchievementsResult = null;
            achievement.QueryAchievements("", AchievementSortBy.CREATED_AT_ASC, result => { paginatedAchievementsResult = result; }, 1);

            while (paginatedAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsFalse(paginatedAchievementsNoOffsetResult.IsError, "Cannot query achievements no offset.");
            TestHelper.Assert.IsFalse(paginatedAchievementsResult.IsError, "Cannot query achievements.");
            TestHelper.Assert.That(paginatedAchievementsResult.Value.data[0].achievementCode, Is.EqualTo(paginatedAchievementsNoOffsetResult.Value.data[1].achievementCode));
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator GetAchievement()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<MultiLanguageAchievement> getAchievement1Result = null;
            achievement.GetAchievement(achievement1.achievementCode, result => { getAchievement1Result = result; });

            while (getAchievement1Result == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            Result<MultiLanguageAchievement> getAchievement2Result = null;
            achievement.GetAchievement(achievement2.achievementCode, result => { getAchievement2Result = result; });

            while (getAchievement2Result == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsFalse(getAchievement1Result.IsError, "Cannot get achievement 1.");
            TestHelper.Assert.That(getAchievement1Result.Value.achievementCode, Is.EqualTo(achievement1.achievementCode));
            TestHelper.Assert.IsFalse(getAchievement2Result.IsError, "Cannot get achievement 2.");
            TestHelper.Assert.That(getAchievement2Result.Value.achievementCode, Is.EqualTo(achievement2.achievementCode));
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator GetInvalidAchievementId()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<MultiLanguageAchievement> getAchievement1Result = null;
            achievement.GetAchievement("Invalid", result => { getAchievement1Result = result; });

            while (getAchievement1Result == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsTrue(getAchievement1Result.IsError, "Get invalid achievement id success.");
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator QueryUserAchievementsEmptyData()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedUserAchievement> paginatedUserAchievementsResult = null;
            achievement.QueryUserAchievements(AchievementSortBy.NONE, result => { paginatedUserAchievementsResult = result; });

            while (paginatedUserAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsFalse(paginatedUserAchievementsResult.IsError, "Cannot query user achievements.");
            TestHelper.Assert.That(paginatedUserAchievementsResult.Value.data.Length, Is.EqualTo(0));
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator UnlockAchievement()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result unlockAchievementResult = null;
            achievement.UnlockAchievement(achievement1.achievementCode, result => { unlockAchievementResult = result; });

            while (unlockAchievementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsFalse(unlockAchievementResult.IsError, "Cannot unlock achievement.");
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator QueryUserAchievements()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedUserAchievement> paginatedUserAchievementsResult = null;
            achievement.QueryUserAchievements(AchievementSortBy.NONE, result => { paginatedUserAchievementsResult = result; });

            while (paginatedUserAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            bool isAchievement1Found = false;
            if (paginatedUserAchievementsResult.Value.data[0].achievementCode == achievement1.achievementCode)
            {
                isAchievement1Found = true;
            }

            TestHelper.Assert.IsFalse(paginatedUserAchievementsResult.IsError, "Cannot query user achievements.");
            TestHelper.Assert.That(paginatedUserAchievementsResult.Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.IsTrue(isAchievement1Found, "Expected achievement 1 is not found.");
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator ServerUnlockAchievement()
        {
            ServerAchievement serverAchievement = AccelByteServerPlugin.GetAchievement();

            Result unlockAchievementResult = null;
            serverAchievement.UnlockAchievement(userId, achievement2.achievementCode, result => { unlockAchievementResult = result; });

            while (unlockAchievementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsFalse(unlockAchievementResult.IsError, "Cannot unlock achievement.");
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator ServerUnlockAchievementInvalidUserId()
        {
            ServerAchievement serverAchievement = AccelByteServerPlugin.GetAchievement();

            Result unlockAchievementResult = null;
            serverAchievement.UnlockAchievement("Invalid", achievement2.achievementCode, result => { unlockAchievementResult = result; });

            while (unlockAchievementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            // TODO: Wait for user id validation fix in the backend.
            //TestHelper.Assert.IsTrue(unlockAchievementResult.IsError, "Unlock achievement success.");
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator ServerUnlockInvalidAchievementCode()
        {
            ServerAchievement serverAchievement = AccelByteServerPlugin.GetAchievement();

            Result unlockAchievementResult = null;
            serverAchievement.UnlockAchievement(userId, "Invalid", result => { unlockAchievementResult = result; });

            while (unlockAchievementResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            TestHelper.Assert.IsTrue(unlockAchievementResult.IsError, "Unlock achievement success.");
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator QueryUserAchievementsUnlockedAll()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedUserAchievement> paginatedUserAchievementsResult = null;
            achievement.QueryUserAchievements(AchievementSortBy.CREATED_AT_ASC, result => { paginatedUserAchievementsResult = result; });

            while (paginatedUserAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            bool isAchievement1Found = false;
            if (paginatedUserAchievementsResult.Value.data[0].achievementCode == achievement1.achievementCode)
            {
                isAchievement1Found = true;
            }

            bool isAchievement2Found = false;
            if (paginatedUserAchievementsResult.Value.data[1].achievementCode == achievement2.achievementCode)
            {
                isAchievement2Found = true;
            }

            TestHelper.Assert.IsFalse(paginatedUserAchievementsResult.IsError, "Cannot query user achievements.");
            TestHelper.Assert.That(paginatedUserAchievementsResult.Value.data.Length, Is.EqualTo(2));
            TestHelper.Assert.IsTrue(isAchievement1Found, "Expected achievement 1 is not found.");
            TestHelper.Assert.IsTrue(isAchievement2Found, "Expected achievement 2 is not found.");
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator QueryUserAchievementsOffset1()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedUserAchievement> paginatedUserAchievementsResult = null;
            achievement.QueryUserAchievements(AchievementSortBy.CREATED_AT_ASC, result => { paginatedUserAchievementsResult = result; }, 1);

            while (paginatedUserAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            bool isAchievement2Found = false;
            if (paginatedUserAchievementsResult.Value.data[0].achievementCode == achievement2.achievementCode)
            {
                isAchievement2Found = true;
            }

            TestHelper.Assert.IsFalse(paginatedUserAchievementsResult.IsError, "Cannot query user achievements.");
            TestHelper.Assert.That(paginatedUserAchievementsResult.Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.IsTrue(isAchievement2Found, "Expected achievement 2 is not found.");
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator QueryUserAchievementsLimit1()
        {
            Achievement achievement = AccelBytePlugin.GetAchievement();

            Result<PaginatedUserAchievement> paginatedUserAchievementsResult = null;
            achievement.QueryUserAchievements(AchievementSortBy.CREATED_AT_ASC, result => { paginatedUserAchievementsResult = result; }, 0, 1);

            while (paginatedUserAchievementsResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            bool isAchievement1Found = false;
            if (paginatedUserAchievementsResult.Value.data[0].achievementCode == achievement1.achievementCode)
            {
                isAchievement1Found = true;
            }

            TestHelper.Assert.IsFalse(paginatedUserAchievementsResult.IsError, "Cannot query user achievements.");
            TestHelper.Assert.That(paginatedUserAchievementsResult.Value.data.Length, Is.EqualTo(1));
            TestHelper.Assert.IsTrue(isAchievement1Found, "Expected achievement 1 is not found.");
        }

        [UnityTest, TestLog, Order(6)]
        public IEnumerator Unlock_IncrementalAchievement_ClientUpdateStat_OK()
        {
            // Arrange
            Statistic statistic = AccelBytePlugin.GetStatistic();
            Achievement achievement = AccelBytePlugin.GetAchievement();

            string currentStatCode = achievementIncrementalClient.statCode;
            TestHelper.AchievementRequest currentAchievementRequest = achievementIncrementalClient;

            Result<PagedStatItems> getUserStatItemsResult = null;
            statistic.GetUserStatItems(new[] {currentStatCode}, new List<string>(), result =>
            {
                getUserStatItemsResult = result;
            });
            while (getUserStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            foreach (var statItem in getUserStatItemsResult.Value.data)
            {
                if (statItem.statCode == currentStatCode)
                {
                    TestHelper.Assert.IsFalse(statItem.value > 0.0f, "Cannot proceed to testing since the user's statistic value is not empty."); 
                }
            }

            // Act
            Result<StatItemOperationResult[]> incrementUserStatItemsResult = null;
            statistic.IncrementUserStatItems(new[]{new StatItemIncrement
            {
                statCode = currentStatCode, 
                inc = currentAchievementRequest.goalValue
            }}, result => { incrementUserStatItemsResult = result; });
            while (incrementUserStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            
            // WAITING FOR KAFKA!!
            yield return new WaitForSeconds(6.66f);
            
            Result<PaginatedUserAchievement> getUserAchievementResult = null;
            achievement.QueryUserAchievements(AchievementSortBy.NONE, result =>
            {
                getUserAchievementResult = result;
            });
            while (getUserAchievementResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            
            // Assert
            bool isIncrementalAchievementFound = false;
            foreach (var userAchievement in getUserAchievementResult.Value.data)
            {
                if (userAchievement.achievementCode == currentAchievementRequest.achievementCode
                    && userAchievement.latestValue == currentAchievementRequest.goalValue)
                {
                    isIncrementalAchievementFound = true;
                }
            }
            
            TestHelper.Assert.IsFalse(getUserStatItemsResult.IsError, "Cannot get user stat items.");
            TestHelper.Assert.IsFalse(incrementUserStatItemsResult.IsError, "Cannot increment user stat items.");
            TestHelper.Assert.IsFalse(getUserAchievementResult.IsError, "Cannot get user achievement.");
            TestHelper.Assert.IsTrue(isIncrementalAchievementFound, "Incremental achievement does not work.");
        }

        [UnityTest, TestLog, Order(6)]
        public IEnumerator Unlock_IncrementalAchievement_ServerUpdateStat_OK()
        {
            // ServerLogin
            Result serverLoginResult = null;
            DedicatedServer server = AccelByteServerPlugin.GetDedicatedServer();
            server.LoginWithClientCredentials(result =>
            {
                serverLoginResult = result;
            });
            while (serverLoginResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            
            // Arrange
            ServerStatistic statistic = AccelByteServerPlugin.GetStatistic();
            Achievement achievement = AccelBytePlugin.GetAchievement();

            string currentStatCode = achievementIncrementalServer.statCode;
            TestHelper.AchievementRequest currentAchievementRequest = achievementIncrementalServer;

            Result<PagedStatItems> getUserStatItemsResult = null;
            statistic.GetUserStatItems(userId, new[] {currentStatCode}, new List<string>(), result =>
            {
                getUserStatItemsResult = result;
            });
            while (getUserStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }

            foreach (var statItem in getUserStatItemsResult.Value.data)
            {
                if (statItem.statCode == currentStatCode)
                {
                    TestHelper.Assert.IsFalse(statItem.value > 0.0f, "Cannot proceed to testing since the user's statistic value is not empty."); 
                }
            }

            // Act
            Result<StatItemOperationResult[]> incrementUserStatItemsResult = null;
            statistic.IncrementUserStatItems(userId, new[]{new StatItemIncrement
            {
                statCode = currentStatCode, 
                inc = currentAchievementRequest.goalValue
            }}, result => { incrementUserStatItemsResult = result; });
            while (incrementUserStatItemsResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            
            // WAITING FOR KAFKA!!
            yield return new WaitForSeconds(6.66f);
            
            Result<PaginatedUserAchievement> getUserAchievementResult = null;
            achievement.QueryUserAchievements(AchievementSortBy.NONE, result =>
            {
                getUserAchievementResult = result;
            });
            while (getUserAchievementResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            
            // Assert
            bool isIncrementalAchievementFound = false;
            foreach (var userAchievement in getUserAchievementResult.Value.data)
            {
                if (userAchievement.achievementCode == currentAchievementRequest.achievementCode
                    && userAchievement.latestValue == currentAchievementRequest.goalValue)
                {
                    isIncrementalAchievementFound = true;
                }
            }
            
            TestHelper.Assert.IsFalse(serverLoginResult.IsError, "Cannot log in server items.");
            TestHelper.Assert.IsFalse(getUserStatItemsResult.IsError, "Cannot get user stat items.");
            TestHelper.Assert.IsFalse(incrementUserStatItemsResult.IsError, "Cannot increment user stat items.");
            TestHelper.Assert.IsFalse(getUserAchievementResult.IsError, "Cannot get user achievement.");
            TestHelper.Assert.IsTrue(isIncrementalAchievementFound, "Incremental achievement does not work.");
        }
        
        [UnityTest, TestLog, Order(999)]
        public IEnumerator TearDown()
        {
            TestHelper testHelper = new TestHelper();

            List<Result> deleteAchievementResults = new List<Result>();
            foreach (var achievementRequest in allAchievementRequests)
            {
                Result deleteAchievementResult = null;
                testHelper.DeleteAchievement(adminAccessToken, achievementRequest.achievementCode, result => { deleteAchievementResult = result; });
    
                while (deleteAchievementResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                
                deleteAchievementResults.Add(deleteAchievementResult);
            }

            Result deleteUserResult = null;
            testHelper.DeleteUser(AccelBytePlugin.GetUser(), result => { deleteUserResult = result; });

            while (deleteUserResult == null)
            {
                Thread.Sleep(100);

                yield return null;
            }

            AccelBytePlugin.GetUser().Logout(result => { });

            foreach (var deleteAchievementResult in deleteAchievementResults)
            {
                TestHelper.Assert.IsFalse(deleteAchievementResult.IsError, "Cannot delete achievement.");
            }
            
            TestHelper.Assert.IsFalse(deleteUserResult.IsError, "Cannot delete user.");
        }
    }
}
