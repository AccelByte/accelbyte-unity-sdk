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
            public static TestHelper.FullItemInfo inGameItem = new TestHelper.FullItemInfo();
            public static TestHelper.FullItemInfo redeemableItem;
            public static TestHelper.CampaignInfo campaignResult;
            public static TestHelper.CampaignInfo expiredCampaignResult;
            public static TestHelper.CampaignInfo notStartedCampaignResult;
            public static TestHelper.CodeInfo codeInfo;
            public static TestHelper.CodeInfo expiredCodeInfo;
            public static TestHelper.CodeInfo notStartedCodeInfo;
            public static bool bPublishedStoreIsExist = false;
            public static TestHelper.RewardInfo rewardInfo = new TestHelper.RewardInfo();

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
                Categories categories = AccelBytePlugin.GetCategories();
                Items items = AccelBytePlugin.GetItems();

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
                }
                else
                {
                    TestHelper.Assert.IsResultOk(publishedStoreInfo);
                    TestVariables.bPublishedStoreIsExist = true;
                }

                if (TestVariables.bPublishedStoreIsExist)
                {
                    TestVariables.publishedStoreId = publishedStoreInfo.Value.storeId;
                }
                
                //check draft store list
                Result<TestHelper.StoreInfoModel[]> getStoresResult = null;
                testHelper.GetStoreList(TestVariables.accessToken, AccelBytePlugin.Config.Namespace, result => getStoresResult = result);
                yield return TestHelper.WaitForValue(() => getStoresResult);
                TestHelper.Assert.IsResultOk(getStoresResult, "Get Store");
                TestHelper.Assert.That(getStoresResult.Value, Is.Not.Null);

                int draftStoreNum = 0;
                for (int i = 0; i < getStoresResult.Value.Length; i++)
                {
                    draftStoreNum += getStoresResult.Value[i].published ? 0 : 1;
                }

                //Create temp store
                TestHelper.StoreCreateModel temporaryStore = new TestHelper.StoreCreateModel
                {
                    title = "ACCELBYTE STORE",
                    description = "Created from Unity SDK Ecommerce Test",
                    supportedLanguages = new string[] { "en" },
                    supportedRegions = new string[] { "US" },
                    defaultLanguage = "en",
                    defaultRegion = "US"
                };

                if (draftStoreNum > 0)
                {
                    for (int i = 0; i < getStoresResult.Value.Length; i++)
                    {
                        if (getStoresResult.Value[i].title == temporaryStore.title)
                        {
                            TestVariables.createdTemporaryStoreInfoId = getStoresResult.Value[i].storeId;
                            break;
                        }
                    }

                    if (string.IsNullOrEmpty(TestVariables.createdTemporaryStoreInfoId))
                    {
                        for (int i = 0; i < getStoresResult.Value.Length; i++)
                        {
                            if (getStoresResult.Value[i].published == false)
                            {
                                TestVariables.createdTemporaryStoreInfoId = getStoresResult.Value[i].storeId;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Result<TestHelper.StoreInfoModel> createStoreResult = null;
                    testHelper.CreateStore(TestVariables.accessToken, temporaryStore, result => createStoreResult = result);
                    yield return TestHelper.WaitForValue(() => createStoreResult);

                    TestHelper.Assert.IsResultOk(createStoreResult, "Create Store");
                    TestHelper.Assert.That(createStoreResult.Value, Is.Not.Null);

                    TestVariables.createdTemporaryStoreInfoId = createStoreResult.Value.storeId;
                    TestHelper.Assert.That(TestVariables.createdTemporaryStoreInfoId != null);
                }

                if (TestVariables.bPublishedStoreIsExist)
                {
                    //Clone published store
                    Result<TestHelper.StoreInfoModel> cloneStoreResult = null;
                    testHelper.CloneStore(AccelBytePlugin.Config.Namespace, TestVariables.accessToken, TestVariables.publishedStoreId, TestVariables.createdTemporaryStoreInfoId, result => cloneStoreResult = result);
                    yield return TestHelper.WaitForValue(() => cloneStoreResult);
                    TestHelper.LogResult(cloneStoreResult, "Clone Published Store");
                    TestHelper.Assert.IsResultOk(cloneStoreResult, "Clone Published Store");
                    TestHelper.Assert.That(cloneStoreResult, Is.Not.Null);
                }
                
                #region create category

                TestHelper.CategoryCreateModel unityRootCategory = new TestHelper.CategoryCreateModel
                {
                    categoryPath = "/UnityRootCategory",
                    localizationDisplayNames = new Dictionary<string, string>()
                };
                unityRootCategory.localizationDisplayNames.Add("en", "Unity's ecommerce root category");
                TestVariables.expectedRootCategoryPath = unityRootCategory.categoryPath;

                TestHelper.CategoryCreateModel unityChildCategory = new TestHelper.CategoryCreateModel
                {
                    categoryPath = "/UnityRootCategory/UnityChildCategory",
                    localizationDisplayNames = new Dictionary<string, string>()
                };
                unityChildCategory.localizationDisplayNames.Add("en", "Unity's ecommerce child category");
                TestVariables.expectedChildCategoryPath = unityChildCategory.categoryPath;

                TestHelper.CategoryCreateModel unityGrandChildCategory = new TestHelper.CategoryCreateModel
                {
                    categoryPath = "/UnityRootCategory/UnityChildCategory/UnityGrandChildCategory",
                    localizationDisplayNames = new Dictionary<string, string>()
                };
                unityGrandChildCategory.localizationDisplayNames.Add("en", "Unity's ecommerce grand child category");
                TestVariables.expectedGrandChildCategoryPath = unityGrandChildCategory.categoryPath;

                TestHelper.CategoryCreateModel[] categoriesModels = new TestHelper.CategoryCreateModel[3];
                categoriesModels[0] = unityRootCategory;
                categoriesModels[1] = unityChildCategory;
                categoriesModels[2] = unityGrandChildCategory;

                for (int i = 0; i < categoriesModels.Length; i++)
                {
                    Result<CategoryInfo> getCategoryResult = null;
                    categories.GetCategory(
                        categoriesModels[i].categoryPath,
                        TestVariables.language,
                        result => { getCategoryResult = result; });
                    yield return TestHelper.WaitForValue(() => getCategoryResult);

                    if (getCategoryResult.IsError)
                    {
                        Result<TestHelper.FullCategoryInfo> createCategoriesResult = null;
                        testHelper.CreateCategory(
                            TestVariables.accessToken,
                            TestVariables.createdTemporaryStoreInfoId,
                            categoriesModels[i],
                            result => { createCategoriesResult = result; });
                        yield return TestHelper.WaitForValue(() => createCategoriesResult);
                        if (createCategoriesResult.IsError)
                        {
                            Debug.Log("Failed create category, error is " + createCategoriesResult.Error.Message);
                        }
                    }
                }

                #endregion

                #region create item
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
                TestHelper.ItemCreateModel childItemRequest = new TestHelper.ItemCreateModel
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
                childItemRequest.regionData.Add("US", childRegionData);
                TestVariables.currencyItemTitle = childItemRequest.localizations.ElementAt(0).Value.title;

                TestHelper.ItemCreateModel.Localization grandChildLocalization = new TestHelper.ItemCreateModel.Localization
                {
                    title = "Unity_GrandChildItem",
                    description = "Grandchild item, real currency, not free, USD"
                };
                Dictionary<string, TestHelper.ItemCreateModel.Localization> grandChildLocalizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>();
                grandChildLocalizations.Add("en", grandChildLocalization);
                RegionDataItem[] grandChildRegionData = new RegionDataItem[1];
                grandChildRegionData[0] = new RegionDataItem
                {
                    price = 1,
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
                TestHelper.ItemCreateModel grandChildItemRequest = new TestHelper.ItemCreateModel
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
                grandChildItemRequest.regionData.Add("US", grandChildRegionData);

                TestHelper.ItemCreateModel[] itemRequests = new TestHelper.ItemCreateModel[3];
                itemRequests[0] = rootItemRequest;
                itemRequests[1] = childItemRequest;
                itemRequests[2] = grandChildItemRequest;
                
                for (int i = 0; i < itemRequests.Length; i++)
                {
                    Result<ItemPagingSlicedResult> getItems = null;
                    ItemCriteria criteria = new ItemCriteria
                    {
                        categoryPath = itemRequests[i].categoryPath,
                        sortBy = "createdAt:desc",
                    };
                    items.GetItemsByCriteria(criteria, result =>
                    {
                        getItems = result;
                    });
                    yield return TestHelper.WaitForValue(() => getItems);

                    if (getItems.IsError != true && i == 0)
                    {
                        TestVariables.inGameItem.itemId = getItems.Value.data[0].itemId; //only for root category
                    }

                    if (getItems.IsError)
                    {
                        Result<TestHelper.FullItemInfo> createItemResults = null;
                        testHelper.CreateItem(
                            TestVariables.accessToken,
                            TestVariables.createdTemporaryStoreInfoId,
                            itemRequests[i],
                            result => { createItemResults = result; });
                        yield return TestHelper.WaitForValue(() => createItemResults);
                        if (createItemResults.IsError)
                        {
                            Debug.Log("Can not create item, error is : " + createItemResults.Error.Message);
                        }                       
                        if (i == 0) TestVariables.inGameItem = createItemResults.Value; //only for root category
                    }
                }

                Result<TestHelper.StoreInfoModel> clonePublishStoreInfo = null;
                testHelper.PublishStore(
                    AccelBytePlugin.Config.Namespace,
                    TestVariables.accessToken,
                    TestVariables.createdTemporaryStoreInfoId,
                    result => { clonePublishStoreInfo = result; });
                yield return TestHelper.WaitForValue(() => clonePublishStoreInfo);
                TestHelper.Assert.IsResultOk(clonePublishStoreInfo);

                #endregion

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

                Result<ItemPagingSlicedResult> getRedeemableItems = null;
                ItemCriteria redeemableCriteria = new ItemCriteria
                {
                    categoryPath = redeemableItemRequest.categoryPath,
                    sortBy = "createdAt:desc",
                };
                items.GetItemsByCriteria(redeemableCriteria, result =>
                {
                    getRedeemableItems = result;
                });
                yield return TestHelper.WaitForValue(() => getRedeemableItems);

                if (getRedeemableItems.IsError)
                {
                    Result<TestHelper.FullItemInfo> createRedeemableItemResult = null;
                    testHelper.CreateItem(
                        TestVariables.accessToken,
                        TestVariables.createdTemporaryStoreInfoId,
                        redeemableItemRequest,
                        result => { createRedeemableItemResult = result; });
                    yield return TestHelper.WaitForValue(() => createRedeemableItemResult);
                    TestHelper.Assert.IsResultOk(createRedeemableItemResult);
                    TestVariables.redeemableItem = createRedeemableItemResult.Value;
                }

                #endregion

                #region Create Campaign
                TestVariables.campaignName = "unityCampaignTest";

                Result<TestHelper.CampaignPagingSlicedResult> getCampaignResult = null;
                testHelper.GetCampaignByName(
                    TestVariables.accessToken,
                    TestVariables.campaignName,
                    result => { getCampaignResult = result; });

                while (getCampaignResult == null)
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

                #region Create Reward

                TestHelper.RewardItem rewardItem = new TestHelper.RewardItem
                {
                    itemId = TestVariables.inGameItem.itemId,
                    quantity = 1
                };
                TestHelper.RewardItem[] rewardItems = new TestHelper.RewardItem[1];
                rewardItems[0] = rewardItem;

                TestHelper.RewardCondition rewardCondition = new TestHelper.RewardCondition
                {
                    eventName = "EventNameTest",
                    conditionName = "ConditionNameTest",
                    condition = "$.[?(@.statCode == \"statcodetest\" && @.latestValue == 5)]",
                    rewardItems = rewardItems,
                };
                TestHelper.RewardCondition[] rewardConditions = new TestHelper.RewardCondition[1];
                rewardConditions[0] = rewardCondition;

                TestHelper.RewardCreateModel rewardCreateModel = new TestHelper.RewardCreateModel
                {
                    rewardCode = "unityrewardtest",
                    description = "RewardDescriptionTest",
                    eventTopic = "statistic",
                    rewardConditions = rewardConditions,
                    maxAwardedPerUser = -1,
                    maxAwarded = -1
                };

                Result<TestHelper.QueryRewardInfo> queryRewardResult = null;
                testHelper.QueryRewards(
                    TestVariables.accessToken,
                    "statistic",
                    0,
                    99,
                    "namespace",
                    result => { queryRewardResult = result; });

                while (queryRewardResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                TestHelper.Assert.That(!queryRewardResult.IsError);

                TestHelper.RewardInfo getReward = null;

                bool isRewardExist = false;
                if (queryRewardResult.Value.data.Length != 0)
                {
                    foreach (var rewardResult in queryRewardResult.Value.data)
                    {
                        if (rewardResult.rewardCode == rewardCreateModel.rewardCode)
                        {
                            isRewardExist = true;
                            getReward = rewardResult;
                            break;
                        }
                    }
                }

                Result<TestHelper.RewardInfo> createRewardResult = null;
                if (!isRewardExist)
                {
                    testHelper.CreateReward(
                        TestVariables.accessToken,
                        rewardCreateModel,
                        result => { createRewardResult = result; });

                    while (createRewardResult == null)
                    {
                        Thread.Sleep(100);
                        yield return null;
                    }
                    TestHelper.Assert.That(!createRewardResult.IsError);
                    getReward = createRewardResult.Value;
                }
                TestVariables.rewardInfo = getReward;

                #endregion

            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator LoginTestUser()
            {
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

                TestHelper testHelper = new TestHelper();
                Result<TokenData> getAccessToken = null;
                testHelper.GetSuperUserAccessToken(result => { getAccessToken = result; });
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
                    Result<TestHelper.StoreInfoModel> storeDeleteResult = null;
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

                Result<Tests.TestHelper.QueryRewardInfo> deleteRewardResult = null;
                testHelper.DeleteReward(
                    TestVariables.accessToken,
                    TestVariables.rewardInfo.rewardId,
                    result => { deleteRewardResult = result; });

                while (deleteRewardResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                TestHelper.Assert.That(!deleteRewardResult.IsError);
            }
        }
    }
}