// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            public static string redeemableItemTitle;
            public static string currencyItemTitle;
            public static string language;
            public static string region;
            public static string createdTemporaryStoreInfoId;
            public static string publishedStoreId;
            public static string accessToken;
            public static string userId;
            public static string campaignName;
            public static string expiredCampaignName;
            public static string notStartedCampaignName;
            public static TestHelper.FullItemInfo inGameItem;
            public static TestHelper.FullItemInfo redeemableItem;
            public static TestHelper.CampaignInfo campaignResult;
            public static TestHelper.CampaignInfo expiredCampaignResult;
            public static TestHelper.CampaignInfo notStartedCampaignResult;
            public static TestHelper.CodeInfo codeInfo;
            public static TestHelper.CodeInfo expiredCodeInfo;
            public static TestHelper.CodeInfo notStartedCodeInfo;
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
            [UnityTest, TestLog, Order(1), Timeout(120000)]
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
                yield return TestHelper.WaitForValue(() => getVtResult);

                if (getVtResult.IsError)
                {
                    Result<TestHelper.CurrencyInfoModel> createVtResult = null;
                    testHelper.CreateCurrency(TestVariables.accessToken, createVtCurrency, result => { createVtResult = result; });
                    yield return TestHelper.WaitForValue(() => createVtResult);

                    TestHelper.Assert.IsResultOk(createVtResult);
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
                yield return TestHelper.WaitForValue(() => getRealResult);

                if (getRealResult.IsError)
                {
                    Result<TestHelper.CurrencyInfoModel> createRealResult = null;
                    testHelper.CreateCurrency(TestVariables.accessToken, createRealCurrency, result => { createRealResult = result; });
                    yield return TestHelper.WaitForValue(() => createRealResult);

                    TestHelper.Assert.IsResultOk(createRealResult);
                }

                //Get published store
                Result<TestHelper.StoreInfoModel> publishedStoreInfo = null;
                testHelper.GetPublishedStore(AccelBytePlugin.Config.Namespace, TestVariables.accessToken, result => { publishedStoreInfo = result; });
                yield return TestHelper.WaitForValue(() => publishedStoreInfo);

                if (publishedStoreInfo.IsError)
                {
                    Debug.Log(publishedStoreInfo.Error.Code);
                    TestHelper.Assert.That(publishedStoreInfo.Error.Code == ErrorCode.PublisherStoreNotExist);
                    TestVariables.bPublishedStoreIsExist = false;
                }
                else
                {
                    TestHelper.Assert.IsResultOk(publishedStoreInfo);
                }

                if (TestVariables.bPublishedStoreIsExist) TestVariables.publishedStoreId = publishedStoreInfo.Value.storeId;

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
                yield return TestHelper.WaitForValue(() => createdTemporaryStoreInfo);

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
                yield return TestHelper.WaitForValue(() => createRootCategoryResult);

                TestHelper.Assert.IsResultOk(createRootCategoryResult);

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
                yield return TestHelper.WaitForValue(() => createChildCategoryResult);

                TestHelper.Assert.IsResultOk(createChildCategoryResult);

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
                yield return TestHelper.WaitForValue(() => createGrandChildCategoryResult);

                TestHelper.Assert.IsResultOk(createGrandChildCategoryResult);

                TestHelper.ItemCreateModel.Localization rootLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "UnityRootItem",
                    description = "Root item, virtual currency, not free"
                };

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
                    seasonType = SeasonType.PASS, //set as default
                    name = "UnityRootItem",
                    entitlementType = "CONSUMABLE",
                    useCount = 1,
                    categoryPath = TestVariables.expectedRootCategoryPath,
                    localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                    status = "ACTIVE",
                    sku = "skuRootItem",
                    regionData = new Dictionary<string, RegionDataItem[]>(),
                    maxCount = -1,
                    maxCountPerUser = -1
                };
                rootItemRequest.regionData.Add("US", rootRegionData);
                rootItemRequest.localizations.Add("en", rootLocalization);
                TestVariables.inGameItemTitle = rootItemRequest.localizations.ElementAt(0).Value.title;

                Result<TestHelper.FullItemInfo> createRootItemResult = null;
                testHelper.CreateItem(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    rootItemRequest,
                    result => { createRootItemResult = result; });
                yield return TestHelper.WaitForValue(() => createRootItemResult);

                TestHelper.Assert.IsResultOk(createRootItemResult);
                TestVariables.inGameItem = createRootItemResult.Value;

                TestHelper.ItemCreateModel.Localization childLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "UnityChildItem",
                    description = "Child item, real currency, free, USD"
                };
                Dictionary<string, TestHelper.ItemCreateModel.Localization> childLocalizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>();
                childLocalizations.Add("en", childLocalization);
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
                    seasonType = SeasonType.PASS, //set as default
                    name = "UnityChildItem",
                    entitlementType = "CONSUMABLE",
                    useCount = 20,
                    targetCurrencyCode = "SDKC",
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    status = "ACTIVE",
                    sku = "skuChildItem",
                    localizations = childLocalizations,
                    regionData = new Dictionary<string, RegionDataItem[]>(),
                    maxCount = -1,
                    maxCountPerUser = -1
                };
                childItem.regionData.Add("US", childRegionData);
                TestVariables.currencyItemTitle = childItem.localizations.ElementAt(0).Value.title;

                Result<TestHelper.FullItemInfo> createChildItemResult = null;
                testHelper.CreateItem(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    childItem,
                    result => { createChildItemResult = result; });
                yield return TestHelper.WaitForValue(() => createChildItemResult);

                TestHelper.Assert.IsResultOk(createChildItemResult);

                TestHelper.ItemCreateModel.Localization grandChildLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "Unity_GrandChildItem",
                    description = "Grandchild item, real currency, free, USD"
                };
                Dictionary<string, TestHelper.ItemCreateModel.Localization> grandChildLocalizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>();
                grandChildLocalizations.Add("en", grandChildLocalization);
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
                    seasonType = SeasonType.PASS, //set as default
                    name = "GrandChildItem",
                    entitlementType = "CONSUMABLE",
                    useCount = 10,
                    targetCurrencyCode = "SDKC",
                    categoryPath = TestVariables.expectedGrandChildCategoryPath,
                    status = "ACTIVE",
                    sku = "skuGrandChildItem",
                    localizations = grandChildLocalizations,
                    regionData = new Dictionary<string, RegionDataItem[]>(),
                    maxCount = -1,
                    maxCountPerUser = -1
                };
                grandChildItem.regionData.Add("US", grandChildRegionData);

                Result<TestHelper.FullItemInfo> createGrandChildItemResult = null;
                testHelper.CreateItem(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    grandChildItem,
                    result => { createGrandChildItemResult = result; });
                yield return TestHelper.WaitForValue(() => createGrandChildItemResult);

                TestHelper.Assert.IsResultOk(createGrandChildItemResult);

                Result<TestHelper.StoreInfoModel> clonePublishStoreInfo = null;
                testHelper.PublishStore(
                    AccelBytePlugin.Config.Namespace,
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    result => { clonePublishStoreInfo = result; });
                yield return TestHelper.WaitForValue(() => clonePublishStoreInfo);

                TestHelper.Assert.IsResultOk(clonePublishStoreInfo);

                #region Create Reedemable In Game Item

                TestVariables.redeemableItemTitle = "UnityRedeemableItem";

                TestHelper.ItemCreateModel.Localization redeemableItemLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = TestVariables.redeemableItemTitle,
                    description = "Redeemable item, virtual currency, free"
                };
                Dictionary<string, TestHelper.ItemCreateModel.Localization> redeemableItemLocalizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>();
                redeemableItemLocalizations.Add("en", redeemableItemLocalization);
                RegionDataItem[] redeemableItemRegionData = new RegionDataItem[1];
                redeemableItemRegionData[0] = new RegionDataItem
                {
                    price = 0,
                    discountPercentage = 0,
                    discountAmount = 0,
                    discountedPrice = 0,
                    currencyCode = "SDKC",
                    currencyType = "VIRTUAL",
                    currencyNamespace = AccelBytePlugin.Config.Namespace,
                    purchaseAt = DateTime.UtcNow,
                    expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                    discountPurchaseAt = DateTime.UtcNow,
                    discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000)
                };
                TestHelper.ItemCreateModel redeemableItemRequest = new TestHelper.ItemCreateModel
                {
                    itemType = "INGAMEITEM",
                    name = TestVariables.redeemableItemTitle,
                    entitlementType = "CONSUMABLE",
                    useCount = 1,
                    categoryPath = TestVariables.expectedRootCategoryPath,
                    localizations = redeemableItemLocalizations,
                    status = "ACTIVE",
                    sku = "skuReedemableItem",
                    regionData = new Dictionary<string, RegionDataItem[]>(),
                    maxCount = -1,
                    maxCountPerUser = -1
                };
                redeemableItemRequest.regionData.Add("US", redeemableItemRegionData);

                Result<TestHelper.FullItemInfo> createRedeemableItemResult = null;
                testHelper.CreateItem(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    redeemableItemRequest,
                    result => { createRedeemableItemResult = result; });

                TestHelper.WaitForValue(() => createRedeemableItemResult);

                while(createRedeemableItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!createRedeemableItemResult.IsError);

                TestVariables.redeemableItem = createRedeemableItemResult.Value;

                #endregion

                #region Create Campaign
                TestVariables.campaignName = "unityCampaignTest";

                Result<TestHelper.CampaignPagingSlicedResult> getCampaignResult = null;
                testHelper.GetCampaignByName(
                    TestVariables.accessToken,
                    TestVariables.campaignName,
                    result => { getCampaignResult = result; });

                while(getCampaignResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }

                TestHelper.Assert.That(!getCampaignResult.IsError);

                TestHelper.RedeemableItem redeemableItem = new TestHelper.RedeemableItem
                {
                    itemId = TestVariables.inGameItem.itemId,
                    itemName = TestVariables.inGameItemTitle,
                    quantity = 1
                };

                if (getCampaignResult.Value.data.Length == 0)
                {
                    TestHelper.CampaignCreateModel campaignCreate = new TestHelper.CampaignCreateModel
                    {
                        name = TestVariables.campaignName,
                        description = "Unity Campaign Test",
                        tags = new string[] { },
                        status = "ACTIVE",
                        maxRedeemCountPerCode = -1,
                        maxRedeemCountPerCodePerUser = 1,
                        maxRedeemCountPerCampaignPerUser = -1,
                        maxSaleCount = -1,
                        redeemStart = DateTime.UtcNow,
                        redeemEnd = DateTime.UtcNow + TimeSpan.FromDays(1000),
                        redeemType = "ITEM",
                        items = new TestHelper.RedeemableItem[] { redeemableItem }
                    };
                    Result<Tests.TestHelper.CampaignInfo> createCampaignResult = null;
                    testHelper.CreateCampaign(
                        TestVariables.accessToken,
                        campaignCreate,
                        result => { createCampaignResult = result; });

                    while (createCampaignResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!createCampaignResult.IsError);

                    TestVariables.campaignResult = createCampaignResult.Value;
                }
                else
                {
                    TestHelper.CampaignUpdateModel campaignUpdate = new TestHelper.CampaignUpdateModel
                    {
                        name = TestVariables.campaignName,
                        description = "Unity Campaign Test",
                        tags = new string[] { },
                        status = "ACTIVE",
                        maxRedeemCountPerCode = -1,
                        maxRedeemCountPerCodePerUser = 1,
                        maxRedeemCountPerCampaignPerUser = -1,
                        maxSaleCount = -1,
                        redeemStart = DateTime.UtcNow,
                        redeemEnd = DateTime.UtcNow + TimeSpan.FromDays(1000),
                        redeemType = "ITEM",
                        items = new TestHelper.RedeemableItem[] { redeemableItem }
                    };
                    Result<Tests.TestHelper.CampaignInfo> updateCampaignResult = null;
                    testHelper.UpdateCampaign(
                        TestVariables.accessToken,
                        getCampaignResult.Value.data[0].id,
                        campaignUpdate,
                        result => { updateCampaignResult = result; });

                    while (updateCampaignResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!updateCampaignResult.IsError);

                    TestVariables.campaignResult = updateCampaignResult.Value;
                }

                TestVariables.expiredCampaignName = "unityExpiredCampaignTest";

                getCampaignResult = null;
                testHelper.GetCampaignByName(
                    TestVariables.accessToken,
                    TestVariables.expiredCampaignName,
                    result => { getCampaignResult = result; });

                while (getCampaignResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Assert.That(!getCampaignResult.IsError);

                if (getCampaignResult.Value.data.Length == 0)
                {
                    TestHelper.CampaignCreateModel campaignCreate = new TestHelper.CampaignCreateModel
                    {
                        name = TestVariables.expiredCampaignName,
                        description = "Unity Expired Campaign Test",
                        tags = new string[] { },
                        status = "ACTIVE",
                        maxRedeemCountPerCode = -1,
                        maxRedeemCountPerCodePerUser = -1,
                        maxRedeemCountPerCampaignPerUser = -1,
                        maxSaleCount = -1,
                        redeemStart = DateTime.UtcNow - TimeSpan.FromDays(1000),
                        redeemEnd = DateTime.UtcNow - TimeSpan.FromDays(1000),
                        redeemType = "ITEM",
                        items = new TestHelper.RedeemableItem[] { }
                    };
                    Result<Tests.TestHelper.CampaignInfo> createCampaignResult = null;
                    testHelper.CreateCampaign(
                        TestVariables.accessToken,
                        campaignCreate,
                        result => { createCampaignResult = result; });

                    while (createCampaignResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    Assert.That(!createCampaignResult.IsError);

                    TestVariables.expiredCampaignResult = createCampaignResult.Value;
                }
                else
                {
                    TestVariables.expiredCampaignResult = getCampaignResult.Value.data[0];
                }

                TestVariables.notStartedCampaignName = "unityNotStartedCampaignTest";

                getCampaignResult = null;
                testHelper.GetCampaignByName(
                    TestVariables.accessToken,
                    TestVariables.notStartedCampaignName,
                    result => { getCampaignResult = result; });

                while (getCampaignResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Assert.That(!getCampaignResult.IsError);

                if (getCampaignResult.Value.data.Length == 0)
                {
                    TestHelper.CampaignCreateModel campaignCreate = new TestHelper.CampaignCreateModel
                    {
                        name = TestVariables.notStartedCampaignName,
                        description = "Unity Not Started Campaign Test",
                        tags = new string[] { },
                        status = "ACTIVE",
                        maxRedeemCountPerCode = -1,
                        maxRedeemCountPerCodePerUser = -1,
                        maxRedeemCountPerCampaignPerUser = -1,
                        maxSaleCount = -1,
                        redeemStart = DateTime.MaxValue,
                        redeemEnd = DateTime.MaxValue,
                        redeemType = "ITEM",
                        items = new TestHelper.RedeemableItem[] { }
                    };
                    Result<Tests.TestHelper.CampaignInfo> createCampaignResult = null;
                    testHelper.CreateCampaign(
                        TestVariables.accessToken,
                        campaignCreate,
                        result => { createCampaignResult = result; });

                    while (createCampaignResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    Assert.That(!createCampaignResult.IsError);

                    TestVariables.notStartedCampaignResult = createCampaignResult.Value;
                }
                else
                {
                    TestVariables.notStartedCampaignResult = getCampaignResult.Value.data[0];
                }

                #endregion

                #region Create Campaign Code

                TestHelper.CampaignCodeCreateModel campaignCodesCreate = new TestHelper.CampaignCodeCreateModel
                {
                    quantity = 1
                };
                Result<Tests.TestHelper.CampaignCodeCreateResult> createCampaignCodesResult = null;
                testHelper.CreateCampaignCodes(
                    TestVariables.accessToken,
                    TestVariables.campaignResult.id,
                    campaignCodesCreate,
                    result => { createCampaignCodesResult = result; });

                while (createCampaignCodesResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }

                TestHelper.Assert.That(!createCampaignCodesResult.IsError);

                Result<TestHelper.CodeInfoPagingSlicedResult> getCampaignCodesResult = null;
                testHelper.GetCampaignCodes(
                    TestVariables.accessToken,
                    TestVariables.campaignResult.id,
                    result => { getCampaignCodesResult = result; });

                while (getCampaignCodesResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!getCampaignCodesResult.IsError);

                TestVariables.codeInfo = getCampaignCodesResult.Value.data
                    .FirstOrDefault(code => code.items.Any(codeItem => codeItem.itemId == TestVariables.inGameItem.itemId));

                TestHelper.Assert.That(TestVariables.codeInfo != null);

                getCampaignCodesResult = null;
                testHelper.GetCampaignCodes(
                    TestVariables.accessToken,
                    TestVariables.expiredCampaignResult.id,
                    result => { getCampaignCodesResult = result; });

                while (getCampaignCodesResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!getCampaignCodesResult.IsError);

                if (getCampaignCodesResult.Value.data.Length == 0)
                {
                    campaignCodesCreate = new TestHelper.CampaignCodeCreateModel
                    {
                        quantity = 1
                    };
                    createCampaignCodesResult = null;
                    testHelper.CreateCampaignCodes(
                        TestVariables.accessToken,
                        TestVariables.expiredCampaignResult.id,
                        campaignCodesCreate,
                        result => { createCampaignCodesResult = result; });

                    while (createCampaignCodesResult == null)
                    {
                        Thread.Sleep(100);
                        yield return null;
                    }

                    TestHelper.Assert.That(!createCampaignCodesResult.IsError);

                    getCampaignCodesResult = null;
                    testHelper.GetCampaignCodes(
                        TestVariables.accessToken,
                        TestVariables.expiredCampaignResult.id,
                        result => { getCampaignCodesResult = result; });

                    while (getCampaignCodesResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!getCampaignCodesResult.IsError);

                    TestVariables.expiredCodeInfo = getCampaignCodesResult.Value.data[0];
                }
                else
                {
                    TestVariables.expiredCodeInfo = getCampaignCodesResult.Value.data[0];
                }

                getCampaignCodesResult = null;
                testHelper.GetCampaignCodes(
                    TestVariables.accessToken,
                    TestVariables.notStartedCampaignResult.id,
                    result => { getCampaignCodesResult = result; });

                while (getCampaignCodesResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.That(!getCampaignCodesResult.IsError);

                if (getCampaignCodesResult.Value.data.Length == 0)
                {
                    campaignCodesCreate = new TestHelper.CampaignCodeCreateModel
                    {
                        quantity = 1
                    };
                    createCampaignCodesResult = null;
                    testHelper.CreateCampaignCodes(
                        TestVariables.accessToken,
                        TestVariables.notStartedCampaignResult.id,
                        campaignCodesCreate,
                        result => { createCampaignCodesResult = result; });

                    while (createCampaignCodesResult == null)
                    {
                        Thread.Sleep(100);
                        yield return null;
                    }

                    TestHelper.Assert.That(!createCampaignCodesResult.IsError);

                    getCampaignCodesResult = null;
                    testHelper.GetCampaignCodes(
                        TestVariables.accessToken,
                        TestVariables.notStartedCampaignResult.id,
                        result => { getCampaignCodesResult = result; });

                    while (getCampaignCodesResult == null)
                    {
                        Thread.Sleep(100);

                        yield return null;
                    }

                    TestHelper.Assert.That(!getCampaignCodesResult.IsError);

                    TestVariables.notStartedCodeInfo = getCampaignCodesResult.Value.data[0];
                }
                else
                {
                    TestVariables.notStartedCodeInfo = getCampaignCodesResult.Value.data[0];
                }

                #endregion

            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator LoginTestUser()
            {
                #region LoginUser

                var user = AccelBytePlugin.GetUser();
                Result userLoginResult = null;
                user.LoginWithDeviceId(result => { userLoginResult = result; });
                yield return TestHelper.WaitForValue(() => userLoginResult);

                while (userLoginResult == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                TestHelper.Assert.IsTrue(!userLoginResult.IsError || userLoginResult.Error.Code == ErrorCode.InvalidRequest, "User cannot login.");

                TestVariables.userId = user.Session.UserId;

                #endregion


                TestHelper testHelper = new TestHelper();
                Result<TokenData> getAccessToken = null;
                testHelper.GetAccessToken(result => { getAccessToken = result; });
                yield return TestHelper.WaitForValue(() => getAccessToken);

                TestHelper.Assert.IsResultOk(getAccessToken, "Cannot get access token.");

                TestVariables.accessToken = getAccessToken.Value.access_token;

                DedicatedServer server = AccelByteServerPlugin.GetDedicatedServer();
                Result loginResult = null;
                server.LoginWithClientCredentials(result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                TestHelper.Assert.IsResultOk(loginResult, "Server cannot login.");
            }
        }

        [TestFixture]
        public class Z_TearDownEcommerce
        {
            [UnityTest, TestLog, Order(999), Timeout(120000)]
            public IEnumerator Z_DeleteDependencies()
            {
                TestHelper testHelper = new TestHelper();
                var user = AccelBytePlugin.GetUser();

                Result deleteResult = null;
                testHelper.DeleteUser(user, result => { deleteResult = result; });
                yield return TestHelper.WaitForValue(() => deleteResult);

                TestHelper.Assert.IsResultOk(deleteResult);

                Result<TestHelper.StoreInfoModel> storeDeleteResult = null;
                testHelper.DeleteStore(
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    result => { storeDeleteResult = result; });
                yield return TestHelper.WaitForValue(() => storeDeleteResult);

                TestHelper.Assert.IsResultOk(storeDeleteResult);

                if (TestVariables.bPublishedStoreIsExist)
                {
                    Result<TestHelper.StoreInfoModel> rollbackResult = null;
                    testHelper.RollbackStore(
                        TestVariables.accessToken,
                        result => { rollbackResult = result; });
                    yield return TestHelper.WaitForValue(() => rollbackResult);

                    TestHelper.Assert.IsResultOk(rollbackResult);
                }
                else
                {
                    storeDeleteResult = null;
                    testHelper.DeletePublishedStore(TestVariables.accessToken, result => { storeDeleteResult = result; });
                    yield return TestHelper.WaitForValue(() => storeDeleteResult);

                    TestHelper.Assert.IsResultOk(storeDeleteResult);
                }

                Result<TestHelper.CurrencyInfoModel> deleteCurrencyResult = null;
                testHelper.DeleteCurrency(
                    TestVariables.accessToken,
                    TestVariables.currencyCode,
                    result => { deleteCurrencyResult = result; });
                yield return TestHelper.WaitForValue(() => deleteCurrencyResult);

                TestHelper.Assert.IsResultOk(deleteCurrencyResult);
            }
        }
    }
}