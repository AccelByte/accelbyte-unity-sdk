// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
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
    namespace EcommerceTest
    {
        public static class TestVariables
        {
            public static string expectedRootCategoryPath;
            public static string expectedChildCategoryPath;
            public static string expectedGrandChildCategoryPath;
            public static string currencyCode;
            public static string inGameItemTitle;
            public static string currencyItemTitle;
            public static string language;
            public static string region;
            public static string createdTemporaryStoreInfoId;
            public static string createdArchiveStoreInfoId;
            public static string publishedStoreId;
            public static string accessToken;
            public static string userId;
            public static TestHelper.FullItemInfo inGameItem;
            public static bool bPublishedStoreIsExist = true;

            [DataContract]
            public class EcommerceArgumentsModel
            {
                [DataMember] public string rootCategoryPath { get; set; }
                [DataMember] public string childCategoryPath { get; set; }
                [DataMember] public string grandChildCategoryPath { get; set; }
                [DataMember] public string currencyCode { get; set; }
                [DataMember] public string inGameItemTitle { get; set; }
                [DataMember] public string currencyItemTitle { get; set; }
                [DataMember] public string SdkCloneStoreId { get; set; }
                [DataMember] public string ArchiveOriStoreId { get; set; }
            }
        }

        [TestFixture]
        public class A_SetupEcommerce
        {
            [UnityTest, Order(1), Timeout(120000)]
            public IEnumerator PrepareDependencies()
            {
                TestVariables.language = "en";
                TestVariables.region = "US";
                TestHelper testHelper = new TestHelper();

                TestHelper.CurrencyCreateModel createVtCurrency = new TestHelper.CurrencyCreateModel
                {
                    currencyCode = "SDKC",
                    currencySymbol = "SDKC",
                    currencyType = "VIRTUAL",
                    decimals = 0,
                    maxAmountPerTransaction = -1,
                    maxTransactionAmountPerDay = -1,
                    maxBalanceAmount = -1
                };

                TestHelper.CurrencyCreateModel createRealCurrency = new TestHelper.CurrencyCreateModel
                {
                    currencyCode = "USD",
                    currencySymbol = "USD",
                    currencyType = "REAL",
                    decimals = 2,
                    maxAmountPerTransaction = -1,
                    maxTransactionAmountPerDay = -1,
                    maxBalanceAmount = -1
                };

                //Check Virtual currency isNotExist? makeNew : use Existing;
                Result<TestHelper.CurrencySummaryModel> getVtResult = null;
                testHelper.GetCurrencySummary(TestVariables.accessToken, createVtCurrency.currencyCode, result => { getVtResult = result; });

                while (getVtResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                if (getVtResult.IsError)
                {
                    Result<TestHelper.CurrencyInfoModel> createVtResult = null;
                    testHelper.CreateCurrency(TestVariables.accessToken, createVtCurrency, result => { createVtResult = result; });

                    while (createVtResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!createVtResult.IsError);
                    TestVariables.currencyCode = createVtResult.Value.currencyCode;
                }
                else
                {
                    TestVariables.currencyCode = getVtResult.Value.currencyCode;
                }

                TestHelper.Assert.That(TestVariables.currencyCode != null);

                //Check Real currency isNotExist? makeNew : use Existing;
                Result<TestHelper.CurrencySummaryModel> getRealResult = null;
                testHelper.GetCurrencySummary(TestVariables.accessToken, createRealCurrency.currencyCode, result => { getRealResult = result; });

                while (getRealResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                if (getRealResult.IsError)
                {
                    Result<TestHelper.CurrencyInfoModel> createRealResult = null;
                    testHelper.CreateCurrency(TestVariables.accessToken, createRealCurrency, result => { createRealResult = result; });

                    while (createRealResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!createRealResult.IsError);
                }

                //Get published store
                Result<TestHelper.StoreInfoModel> publishedStoreInfo = null;
                testHelper.GetPublishedStore(TestVariables.accessToken, result => { publishedStoreInfo = result; });

                while (publishedStoreInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                if (publishedStoreInfo.IsError)
                {
                    Debug.Log(publishedStoreInfo.Error.Code);
                    TestHelper.Assert.That(publishedStoreInfo.Error.Code == ErrorCode.PublisherStoreNotExist);
                    TestVariables.bPublishedStoreIsExist = false;
                }
                else
                {
                    TestHelper.Assert.That(!publishedStoreInfo.IsError);
                }

                if (TestVariables.bPublishedStoreIsExist) TestVariables.publishedStoreId = publishedStoreInfo.Value.storeId;

                if (TestVariables.bPublishedStoreIsExist)
                {
                    //Create archive publisher store
                    TestHelper.StoreCreateModel archiveStore = new TestHelper.StoreCreateModel
                    {
                        title = "Unity-Store-Archive",
                        description = "keep the original store",
                        supportedLanguages = new string[] { "en" },
                        supportedRegions = new string[] { "US" },
                        defaultLanguage = "en",
                        defaultRegion = "US"
                    };

                    Result<TestHelper.StoreInfoModel> createdArchiveStoreInfo = null;
                    testHelper.CreateStore(TestVariables.accessToken, archiveStore, result => { createdArchiveStoreInfo = result; });

                    while (createdArchiveStoreInfo == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestVariables.createdArchiveStoreInfoId = createdArchiveStoreInfo.Value.storeId;
                    TestHelper.Assert.That(TestVariables.createdArchiveStoreInfoId != null);

                    Result<TestHelper.StoreInfoModel> cloneArchiveStoreInfo = null;
                    testHelper.CloneStore(
                        TestVariables.accessToken,
                        TestVariables.publishedStoreId,
                        TestVariables.createdArchiveStoreInfoId,
                        result => { cloneArchiveStoreInfo = result; });

                    while (cloneArchiveStoreInfo == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!cloneArchiveStoreInfo.IsError);
                }

                //Create temp store
                TestHelper.StoreCreateModel temporaryStore = new TestHelper.StoreCreateModel
                {
                    title = "Unity-Store-Temporary",
                    description = "for Unity SDK testing purpose",
                    supportedLanguages = new string[] { "en" },
                    supportedRegions = new string[] { "US" },
                    defaultLanguage = "en",
                    defaultRegion = "US"
                };

                Result<TestHelper.StoreInfoModel> createdTemporaryStoreInfo = null;
                testHelper.CreateStore(TestVariables.accessToken, temporaryStore, result => { createdTemporaryStoreInfo = result; });

                while (createdTemporaryStoreInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                };

                TestVariables.createdTemporaryStoreInfoId = createdTemporaryStoreInfo.Value.storeId;
                TestHelper.Assert.That(TestVariables.createdTemporaryStoreInfoId != null);

                TestHelper.CategoryCreateModel unityRootCategory = new TestHelper.CategoryCreateModel
                {
                    categoryPath = "/UnityRootCategory",
                    localizationDisplayNames = new Dictionary<string, string>()
                };
                unityRootCategory.localizationDisplayNames.Add("en", "Unity's ecommerce root category");
                TestVariables.expectedRootCategoryPath = unityRootCategory.categoryPath;

                Result<TestHelper.FullCategoryInfo> createRootCategoryResult = null;
                testHelper.CreateCategory(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    unityRootCategory,
                    result => { createRootCategoryResult = result; });

                while (createRootCategoryResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!createRootCategoryResult.IsError);

                TestHelper.CategoryCreateModel unityChildCategory = new TestHelper.CategoryCreateModel
                {
                    categoryPath = "/UnityRootCategory/UnityChildCategory",
                    localizationDisplayNames = new Dictionary<string, string>()
                };
                unityChildCategory.localizationDisplayNames.Add("en", "Unity's ecommerce child category");
                TestVariables.expectedChildCategoryPath = unityChildCategory.categoryPath;

                Result<TestHelper.FullCategoryInfo> createChildCategoryResult = null;
                testHelper.CreateCategory(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    unityChildCategory,
                    result => { createChildCategoryResult = result; });

                while (createChildCategoryResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!createChildCategoryResult.IsError);

                TestHelper.CategoryCreateModel unityGrandChildCategory = new TestHelper.CategoryCreateModel
                {
                    categoryPath = "/UnityRootCategory/UnityChildCategory/UnityGrandChildCategory",
                    localizationDisplayNames = new Dictionary<string, string>()
                };
                unityGrandChildCategory.localizationDisplayNames.Add("en", "Unity's ecommerce grand child category");
                TestVariables.expectedGrandChildCategoryPath = unityGrandChildCategory.categoryPath;

                Result<TestHelper.FullCategoryInfo> createGrandChildCategoryResult = null;
                testHelper.CreateCategory(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    unityGrandChildCategory,
                    result => { createGrandChildCategoryResult = result; });

                while (createGrandChildCategoryResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!createGrandChildCategoryResult.IsError);

                TestHelper.ItemCreateModel.Localization rootLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "UnityRootItem",
                    description = "Root item, virtual currency, not free"
                };
                TestHelper.ItemCreateModel.Localizations rootLocalizations = new TestHelper.ItemCreateModel.Localizations();
                rootLocalizations.en = rootLocalization;
                RegionDataItem[] rootRegionData = new RegionDataItem[1];
                rootRegionData[0] = new RegionDataItem
                {
                    price = 1,
                    discountPercentage = 0,
                    discountAmount = 0,
                    discountedPrice = 1,
                    currencyCode = "SDKC",
                    currencyType = "VIRTUAL",
                    currencyNamespace = AccelBytePlugin.Config.Namespace,
                    purchaseAt = DateTime.UtcNow,
                    expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                    discountPurchaseAt = DateTime.UtcNow,
                    discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000)
                };
                TestHelper.ItemCreateModel rootItemRequest = new TestHelper.ItemCreateModel
                {
                    itemType = "INGAMEITEM",
                    name = "UnityRootItem",
                    entitlementType = "CONSUMABLE",
                    useCount = 1,
                    categoryPath = TestVariables.expectedRootCategoryPath,
                    localizations = rootLocalizations,
                    status = "ACTIVE",
                    sku = "skuRootItem",
                    regionData = new TestHelper.ItemCreateModel.RegionDatas(),
                    maxCount = -1,
                    maxCountPerUser = -1
                };
                rootItemRequest.regionData.US = rootRegionData;
                TestVariables.inGameItemTitle = rootItemRequest.localizations.en.title;

                Result<TestHelper.FullItemInfo> createRootItemResult = null;
                testHelper.CreateItem(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    rootItemRequest,
                    result => { createRootItemResult = result; });

                while (createRootItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!createRootItemResult.IsError);

                TestVariables.inGameItem = createRootItemResult.Value;

                TestHelper.ItemCreateModel.Localization childLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "UnityChildItem",
                    description = "Child item, real currency, free, USD"
                };
                TestHelper.ItemCreateModel.Localizations childLocalizations = new TestHelper.ItemCreateModel.Localizations();
                childLocalizations.en = childLocalization;
                RegionDataItem[] childRegionData = new RegionDataItem[1];
                childRegionData[0] = new RegionDataItem
                {
                    price = 0,
                    discountPercentage = 0,
                    discountAmount = 0,
                    discountedPrice = 0,
                    currencyCode = "USD",
                    currencyType = "REAL",
                    currencyNamespace = AccelBytePlugin.Config.Namespace,
                    purchaseAt = DateTime.UtcNow,
                    expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                    discountPurchaseAt = DateTime.UtcNow,
                    discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000)
                };
                TestHelper.ItemCreateModel childItem = new TestHelper.ItemCreateModel
                {
                    itemType = "COINS",
                    name = "UnityChildItem",
                    entitlementType = "CONSUMABLE",
                    useCount = 20,
                    targetCurrencyCode = "SDKC",
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    status = "ACTIVE",
                    sku = "skuChildItem",
                    localizations = childLocalizations,
                    regionData = new TestHelper.ItemCreateModel.RegionDatas(),
                    maxCount = -1,
                    maxCountPerUser = -1
                };
                childItem.regionData.US = childRegionData;
                TestVariables.currencyItemTitle = childItem.localizations.en.title;

                Result<TestHelper.FullItemInfo> createChildItemResult = null;
                testHelper.CreateItem(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    childItem,
                    result => { createChildItemResult = result; });

                while (createChildItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!createChildItemResult.IsError);

                TestHelper.ItemCreateModel.Localization grandChildLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "Unity_GrandChildItem",
                    description = "Grandchild item, real currency, free, USD"
                };
                TestHelper.ItemCreateModel.Localizations grandChildLocalizations = new TestHelper.ItemCreateModel.Localizations();
                grandChildLocalizations.en = grandChildLocalization;
                RegionDataItem[] grandChildRegionData = new RegionDataItem[1];
                grandChildRegionData[0] = new RegionDataItem
                {
                    price = 0,
                    discountPercentage = 0,
                    discountAmount = 0,
                    discountedPrice = 0,
                    currencyCode = "USD",
                    currencyType = "REAL",
                    currencyNamespace = AccelBytePlugin.Config.Namespace,
                    purchaseAt = DateTime.UtcNow,
                    expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                    discountPurchaseAt = DateTime.UtcNow,
                    discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000)
                };
                TestHelper.ItemCreateModel grandChildItem = new TestHelper.ItemCreateModel
                {
                    itemType = "COINS",
                    name = "GrandChildItem",
                    entitlementType = "CONSUMABLE",
                    useCount = 10,
                    targetCurrencyCode = "SDKC",
                    categoryPath = TestVariables.expectedGrandChildCategoryPath,
                    status = "ACTIVE",
                    sku = "skuGrandChildItem",
                    localizations = grandChildLocalizations,
                    regionData = new TestHelper.ItemCreateModel.RegionDatas(),
                    maxCount = -1,
                    maxCountPerUser = -1
                };
                grandChildItem.regionData.US = grandChildRegionData;

                Result<TestHelper.FullItemInfo> createGrandChildItemResult = null;
                testHelper.CreateItem(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    grandChildItem,
                    result => { createGrandChildItemResult = result; });

                while (createGrandChildItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!createGrandChildItemResult.IsError);

                if (TestVariables.bPublishedStoreIsExist)
                {
                    Result<TestHelper.StoreInfoModel> cloneTemporaryStoreInfo = null;
                    testHelper.CloneStore(
                        TestVariables.accessToken,
                        TestVariables.createdTemporaryStoreInfoId,
                        TestVariables.publishedStoreId,
                        result => { cloneTemporaryStoreInfo = result; });

                    while (cloneTemporaryStoreInfo == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!cloneTemporaryStoreInfo.IsError);
                    TestVariables.publishedStoreId = cloneTemporaryStoreInfo.Value.storeId;
                }
                else
                {
                    Result<TestHelper.StoreInfoModel> clonePublishStoreInfo = null;
                    testHelper.PublishStore(
                        TestVariables.accessToken,
                        TestVariables.createdTemporaryStoreInfoId,
                        result => { clonePublishStoreInfo = result; });

                    while (clonePublishStoreInfo == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!clonePublishStoreInfo.IsError);
                }
            }

            [UnityTest, Order(0)]
            public IEnumerator LoginTestUser()
            {
                var user = AccelBytePlugin.GetUser();
                Result userLoginResult = null;
                user.LoginWithDeviceId(result => { userLoginResult = result; });

                while (userLoginResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                TestHelper.Assert.IsTrue(!userLoginResult.IsError, "User cannot login.");

                TestVariables.userId = user.Session.UserId;

                TestHelper testHelper = new TestHelper();
                Result<TokenData> getAccessToken = null;
                testHelper.GetAccessToken(result => { getAccessToken = result; });

                while (getAccessToken == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getAccessToken.IsError, "Cannot get access token.");

                TestVariables.accessToken = getAccessToken.Value.access_token;

                DedicatedServer server = AccelByteServerPlugin.GetDedicatedServer();
                Result loginResult = null;
                server.LoginWithClientCredentials(result => loginResult = result);

                while (loginResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!loginResult.IsError, "Server cannot login.");
            }
        }

        [TestFixture]
        public class Z_TearDownEcommerce
        {
            [UnityTest, Order(999), Timeout(120000)]
            public IEnumerator Z_DeleteDependencies()
            {
                TestHelper testHelper = new TestHelper();
                var user = AccelBytePlugin.GetUser();

                Result deleteResult = null;
                testHelper.DeleteUser(user, result => { deleteResult = result; });

                //			testHelper.DeleteUser(AccelBytePlugin.Config.Namespace, PlatformType.Device, "", result => { deleteResult = result; });
                while (deleteResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!deleteResult.IsError);

                Result<TestHelper.StoreInfoModel> storeDeleteResult = null;
                testHelper.DeleteStore(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    result => { storeDeleteResult = result; });

                while (storeDeleteResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!storeDeleteResult.IsError);

                if (TestVariables.bPublishedStoreIsExist)
                {
                    Result<TestHelper.StoreInfoModel> cloneResult = null;
                    testHelper.CloneStore(
                        TestVariables.accessToken,
                        TestVariables.createdArchiveStoreInfoId,
                        TestVariables.publishedStoreId,
                        result => { cloneResult = result; });

                    while (cloneResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!cloneResult.IsError);

                    storeDeleteResult = null;
                    testHelper.DeleteStore(
                        TestVariables.accessToken,
                        TestVariables.createdArchiveStoreInfoId,
                        result => { storeDeleteResult = result; });

                    while (storeDeleteResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!storeDeleteResult.IsError);
                }
                else
                {
                    storeDeleteResult = null;
                    testHelper.DeletePublishedStore(TestVariables.accessToken, result => { storeDeleteResult = result; });

                    while (storeDeleteResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!storeDeleteResult.IsError);
                }

                Result<TestHelper.CurrencyInfoModel> deleteCurrencyResult = null;
                testHelper.DeleteCurrency(
                    TestVariables.accessToken,
                    TestVariables.currencyCode,
                    result => { deleteCurrencyResult = result; });

                while (deleteCurrencyResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!deleteCurrencyResult.IsError);
            }
        }
    }
}