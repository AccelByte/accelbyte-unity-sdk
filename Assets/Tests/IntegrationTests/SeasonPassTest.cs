// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class SeasonPassTest
    {
        private TestHelper helper;
        private AccelByteHttpClient httpClient;
        private CoroutineRunner coroutineRunner;
        private Items item = null;
        private Orders order = null;
        private DedicatedServer server = null;
        private ServerEcommerce serverEcommerce = null;
        private ServerSeasonPass serverSeasonPass = null;
        private SeasonPass seasonPass = null;
        private User user;
        private Wallet wallet = null;
        private Entitlement entitlement = null;
        private SeasonPassTestAdmin admin;
        private string helperAccessToken = null;
        private string accountNamespace = "sdktest";

        private string seasonItemCategoryPath = "/Season";
        private string currencyCode = "SDKC";
        private int currencyRewardQuantity = 5;
        private string currencyRewardCode = "coins-reward";
        private string itemRewardCode = "premium-reward";
        private int seasonTierCount = 6;
        private int seasonTierRequiredExp = 100;
        private int premiumPassPrice = 100;
        private int seasonTierPrice = 10;
        private string language = "en";

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

        SeasonPassTestAdmin.SeasonPassCreateSeasonResponse SeasonResponse;
        SeasonPassTestAdmin.SeasonPassCreatePassResponse FreePassResponse, PremiumPassResponse;
        SeasonPassTestAdmin.SeasonPassCreateRewardResponse CurrencyRewardResponse, ItemRewardResponse;
        TestHelper.StoreInfoModel SeasonStoreInfo;
        TestHelper.FullItemInfo FreeSeasonPassItem, PremiumSeasonPassItem, TierSeasonPassItem, SeasonCurrencyItemInfo, SeasonRewardItemInfo;

        public bool CheckCurrentSeason(SeasonInfo seasonInfo)
        {
            return (seasonInfo.title != null 
                && seasonInfo.description != null
                && seasonInfo.Namespace != null
                && seasonInfo.id != null 
                && seasonInfo.name != null 
                && seasonInfo.start != null
                && seasonInfo.end != null
                && seasonInfo.tierItemId != null
                && (seasonInfo.autoClaim == false || seasonInfo.autoClaim == true)
                && seasonInfo.images.Length > 0
                && seasonInfo.passCodes.Length > 0
                && (seasonInfo.status == SeasonPassStatus.DRAFT || seasonInfo.status == SeasonPassStatus.PUBLISHED || seasonInfo.status == SeasonPassStatus.RETIRED)
                && seasonInfo.publishedAt != null
                && seasonInfo.createdAt != null 
                && seasonInfo.updatedAt != null
                && seasonInfo.passes.Length > 0
                && seasonInfo.rewards.Values != null
                && seasonInfo.tiers.Length > 0);
        }

        public bool CheckUserSeason(UserSeasonInfo userSeasonInfo)
        {
            return (userSeasonInfo.id != null
                && userSeasonInfo.Namespace != null
                && userSeasonInfo.userId != null
                && userSeasonInfo.seasonId != null
                && userSeasonInfo.enrolledAt != null
                && userSeasonInfo.enrolledPasses.Length > 0
                && userSeasonInfo.currentTierIndex >= 0
                && userSeasonInfo.lastTierIndex >= 0
                && userSeasonInfo.currentExp >= 0
                && (userSeasonInfo.cleared == true || userSeasonInfo.cleared == false)
                && userSeasonInfo.createdAt != null
                && userSeasonInfo.updatedAt != null
                && userSeasonInfo.season != null
                && userSeasonInfo.toClaimRewards.Values != null
                && userSeasonInfo.claimingRewards.Values != null);
        }

        public bool CheckClaimReward(SeasonClaimRewardResponse rewardResponse)
        {
            return (rewardResponse.toClaimRewards.Values != null && rewardResponse.claimingRewards.Values != null);
        }

        [UnityTest, TestLog, Order(0), Timeout(150000)]
        public IEnumerator Setup()
        {
            if (this.httpClient == null)
            {
                this.httpClient = new AccelByteHttpClient();
                this.httpClient.SetCredentials(AccelBytePlugin.Config.ClientId, AccelBytePlugin.Config.ClientSecret);
            }

            if (this.coroutineRunner == null)
            {
                this.coroutineRunner = new CoroutineRunner();
            }
                
            if (this.helper == null)
            {
                this.helper = new TestHelper();
            }

            if (this.admin == null)
            {
                this.admin = new SeasonPassTestAdmin();
            }

            if (this.user != null) yield break;


            this.user = AccelBytePlugin.GetUser();
            this.seasonPass = AccelBytePlugin.GetSeasonPass();
            this.serverSeasonPass = AccelByteServerPlugin.GetSeasonPass();
            this.item = AccelBytePlugin.GetItems();
            this.server = AccelByteServerPlugin.GetDedicatedServer();
            this.serverEcommerce = AccelByteServerPlugin.GetEcommerce();
            this.order = AccelBytePlugin.GetOrders();
            this.wallet = AccelBytePlugin.GetWallet();
            this.entitlement = AccelBytePlugin.GetEntitlement();

            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;

                this.user.Logout(result => logoutResult = result);

                while (logoutResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsFalse(logoutResult.IsError, "User cannot log out.");
            }

            Result loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);
            TestHelper.Assert.IsTrue(!loginWithDevice.IsError || loginWithDevice.Error.Code == ErrorCode.InvalidRequest, "User cannot login.");

            Result<TokenData> getAccessToken = null;
            this.helper.GetSuperUserAccessToken(result => { getAccessToken = result; });
            yield return TestHelper.WaitForValue(() => getAccessToken);
            TestHelper.Assert.IsResultOk(getAccessToken);
            this.helperAccessToken = getAccessToken.Value.access_token;

            //clean up previous season
            Result<SeasonPassTestAdmin.SeasonPassCreateGetSeasonPagingRespon> seasonsResponse = null;
            SeasonPassStatus[] seasonPassStatuses = { SeasonPassStatus.DRAFT, SeasonPassStatus.PUBLISHED };
            this.admin.SeasonQuerySeason(accountNamespace, this.helperAccessToken, seasonPassStatuses, result => { seasonsResponse = result; });
            yield return TestHelper.WaitForValue(() => seasonsResponse);
            TestHelper.Assert.IsResultOk(seasonsResponse);

            foreach (SeasonPassTestAdmin.SeasonPassCreateGetSeasonResponse season in seasonsResponse.Value.data)
            {
                if (season.status == SeasonPassStatus.PUBLISHED)
                {
                    //unpublish before delete
                    Result<SeasonPassTestAdmin.SeasonPassCreateSeasonResponse> unpublishSeasonResponse = null;
                    this.admin.SeasonForceUnpublishSeason(accountNamespace, this.helperAccessToken, season.id, result => { unpublishSeasonResponse = result; });
                    yield return TestHelper.WaitForValue(() => unpublishSeasonResponse);
                    TestHelper.Assert.IsResultOk(unpublishSeasonResponse);
                }

                Result deleteSeason = null;
                this.admin.SeasonDeleteSeason(accountNamespace, this.helperAccessToken, season.id, result => { deleteSeason = result; });
                yield return TestHelper.WaitForValue(() => deleteSeason);
                TestHelper.Assert.IsResultOk(deleteSeason);
            }

            /*
	        Check the currency that expected for integration test. If it's already created, it doesn't need to be created again.
	        If it doesn't exist, then it will be created.
	        */
            Result<TestHelper.CurrencySummaryModel> getVtResult = null;
            this.helper.GetCurrencySummary(this.helperAccessToken, createVtCurrency.currencyCode, result => { getVtResult = result; });
            yield return TestHelper.WaitForValue(() => getVtResult);
            Debug.Log(getVtResult);

            if (getVtResult.IsError)
            {
                Result<TestHelper.CurrencyInfoModel> createVtResult = null;
                this.helper.CreateCurrency(this.helperAccessToken, createVtCurrency, result => { createVtResult = result; });
                yield return TestHelper.WaitForValue(() => createVtResult);
                TestHelper.Assert.IsResultOk(createVtResult);
                createVtCurrency.currencyCode = createVtResult.Value.currencyCode;
            }
            else
            {
                createVtCurrency.currencyCode = getVtResult.Value.currencyCode;
            }

            //delete store if any, only allowed 1
            Result<TestHelper.StoreInfoModel> deletePublishedStore = null;
            this.helper.DeletePublishedStore(this.helperAccessToken, result => { deletePublishedStore = result; });
            yield return TestHelper.WaitForValue(() => deletePublishedStore);
            if (deletePublishedStore.IsError)
            {
                Debug.Log("Nothing published store");
            }
            else
            {
                Debug.Log("Published store deleted");
                TestHelper.Assert.IsResultOk(deletePublishedStore);
            }

            //get all draft store and delete
            Result<TestHelper.StoreInfoModel[]> getStoreList = null;
            this.helper.GetStoreList(this.helperAccessToken, result => { getStoreList = result; });
            yield return TestHelper.WaitForValue(() => getStoreList);
            TestHelper.Assert.IsResultOk(getStoreList);

            for (int i = 0; i < getStoreList.Value.Length; i++)
            {
                Result<TestHelper.StoreInfoModel> deleteStoreResponse = null;
                this.helper.DeleteStore(this.helperAccessToken, getStoreList.Value[i].storeId, result => { deleteStoreResponse = result; });
                yield return TestHelper.WaitForValue(() => deleteStoreResponse);
                TestHelper.Assert.IsResultOk(deleteStoreResponse);
            }

            //create new store for testing (temporary store)
            TestHelper.StoreCreateModel storeCreateRequest = new TestHelper.StoreCreateModel
            {
                title = "Integration Test Unity Season Pass",
                description = "for Unity SDK Purpose",
                supportedLanguages = new string[] { "en" },
                supportedRegions = new string[] { "US" },
                defaultLanguage = "en",
                defaultRegion = "US",

            };

            Result<TestHelper.StoreInfoModel> createTemporaryStore = null;
            this.helper.CreateStore(this.helperAccessToken, storeCreateRequest, result => 
            { 
                createTemporaryStore = result;
                SeasonStoreInfo = createTemporaryStore.Value;
            });
            yield return TestHelper.WaitForValue(() => createTemporaryStore);
            TestHelper.Assert.IsResultOk(createTemporaryStore);

            Dictionary<string, string> localizationData = new Dictionary<string, string>();
            localizationData.Add("en", seasonItemCategoryPath);
            TestHelper.CategoryCreateModel createCategoryRequest = new TestHelper.CategoryCreateModel
            {
                categoryPath = seasonItemCategoryPath,
                localizationDisplayNames = localizationData, 
            };
            Result<TestHelper.FullCategoryInfo> createCategoryResponse = null;
            this.helper.CreateCategory(this.helperAccessToken, SeasonStoreInfo.storeId, createCategoryRequest,
                result => { createCategoryResponse = result; });
            yield return TestHelper.WaitForValue(() => createCategoryResponse);
            TestHelper.Assert.IsResultOk(createCategoryResponse);

            TestHelper.ItemCreateModel.Localization itemLocalization = new TestHelper.ItemCreateModel.Localization
            {
                title = "season item",
                description = "season item",
                longDescription = "season item"
            };

            Image[] itemImage = new Image[1];
            itemImage[0] = new Image
            {
                As = "image",
                caption = "itemImage",
                height = 32,
                width = 32,
                imageUrl = "http://example.com",
                smallImageUrl = "http://example.com",
            };

            RegionDataItem[] freeRegionDataItem = new RegionDataItem[1];
            freeRegionDataItem[0] = new RegionDataItem
            {
                price = 0,
                discountPercentage = 0,
                discountAmount = 0,
                discountedPrice = 0,
                currencyCode = currencyCode,
                currencyType = CurrencyType.VIRTUAL.ToString(),
                currencyNamespace = accountNamespace,
                purchaseAt = DateTime.UtcNow,
                expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                discountPurchaseAt = DateTime.UtcNow,
                discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
            };

            TestHelper.ItemCreateModel freeSeasonPassItemRequest = new TestHelper.ItemCreateModel
            {
                itemType = ItemType.SEASON.ToString(),
                seasonType = SeasonType.PASS,
                name = "Free Season Pass Unity Integration Test",
                entitlementType = EntitlementType.CONSUMABLE.ToString(),
                useCount = 1,
                stackable = false,
                //appId = "",
                appType = EntitlementAppType.GAME.ToString(),
                baseAppId = "",
                targetCurrencyCode = currencyCode,
                targetNamespace = accountNamespace,
                categoryPath = seasonItemCategoryPath,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                status = ItemStatus.ACTIVE.ToString(),
                sku = "FreeSeasonPassSkuUnity",
                images = itemImage,
                thumbnailUrl = "",
                regionData = new Dictionary<string, RegionDataItem[]>(),
                itemIds = { },
                tags = { },
                maxCountPerUser = 1,
                maxCount = 1,
                boothName = "",
                displayOrder = 0,
                clazz = "classless",
            };
            freeSeasonPassItemRequest.regionData.Add("US", freeRegionDataItem);
            freeSeasonPassItemRequest.localizations.Add("en", itemLocalization);

            Result<TestHelper.FullItemInfo> freeItemInfoResponse = null;
            this.helper.CreateItem(this.helperAccessToken, SeasonStoreInfo.storeId, freeSeasonPassItemRequest, result => 
            { 
                freeItemInfoResponse = result;
                FreeSeasonPassItem = freeItemInfoResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => freeItemInfoResponse);
            TestHelper.Assert.IsResultOk(freeItemInfoResponse);

            RegionDataItem[] premiumRegionDataItem = new RegionDataItem[1];
            premiumRegionDataItem[0] = new RegionDataItem
            {
                price = 0,
                discountPercentage = 0,
                discountAmount = 0,
                discountedPrice = 0,
                currencyCode = currencyCode,
                currencyType = CurrencyType.VIRTUAL.ToString(),
                currencyNamespace = accountNamespace,
                purchaseAt = DateTime.UtcNow,
                expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                discountPurchaseAt = DateTime.UtcNow,
                discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
            };

            TestHelper.ItemCreateModel premiumSeasonPassRequest = new TestHelper.ItemCreateModel
            {
                itemType = ItemType.SEASON.ToString(),
                seasonType = SeasonType.PASS,
                name = "Premium Season Pass Unity Integration Test",
                entitlementType = EntitlementType.CONSUMABLE.ToString(),
                useCount = 1,
                stackable = false,
                appId = "",
                appType = EntitlementAppType.GAME.ToString(),
                baseAppId = "",
                targetCurrencyCode = currencyCode,
                targetNamespace = accountNamespace,
                categoryPath = seasonItemCategoryPath,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                status = ItemStatus.ACTIVE.ToString(),
                sku = "PremiumSeasonPassSkuUnity",
                images = itemImage,
                thumbnailUrl = "",
                regionData = new Dictionary<string, RegionDataItem[]>(),
                itemIds = { },
                tags = { },
                maxCountPerUser = -1,
                maxCount = 1,
                boothName = "",
                displayOrder = 0,
                clazz = "classless",
            };
            premiumSeasonPassRequest.regionData.Add("US", premiumRegionDataItem);
            premiumSeasonPassRequest.localizations.Add("en", itemLocalization);

            Result<TestHelper.FullItemInfo> premiumItemInfoResponse = null;
            this.helper.CreateItem(this.helperAccessToken, SeasonStoreInfo.storeId, premiumSeasonPassRequest, result => 
            { 
                premiumItemInfoResponse = result;
                PremiumSeasonPassItem = premiumItemInfoResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => premiumItemInfoResponse);
            TestHelper.Assert.IsResultOk(premiumItemInfoResponse);

            RegionDataItem[] tierRegionDataItem = new RegionDataItem[1];
            tierRegionDataItem[0] = new RegionDataItem
            {
                price = seasonTierPrice,
                discountPercentage = 0,
                discountAmount = 0,
                discountedPrice = 0,
                currencyCode = currencyCode,
                currencyType = CurrencyType.VIRTUAL.ToString(),
                currencyNamespace = accountNamespace,
                purchaseAt = DateTime.UtcNow,
                expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                discountPurchaseAt = DateTime.UtcNow,
                discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
            };

            TestHelper.ItemCreateModel tierSeasonPassRequest = new TestHelper.ItemCreateModel
            {
                itemType = ItemType.SEASON.ToString(),
                seasonType = SeasonType.TIER,
                name = "Tier Season Pass Unity Integration Test",
                entitlementType = EntitlementType.CONSUMABLE.ToString(),
                useCount = 1,
                stackable = false,
                appId = "",
                appType = EntitlementAppType.GAME.ToString(),
                baseAppId = "",
                targetCurrencyCode = currencyCode,
                targetNamespace = accountNamespace,
                categoryPath = seasonItemCategoryPath,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                status = ItemStatus.ACTIVE.ToString(),
                sku = "TierSeasonPassSkuUnity",
                images = itemImage,
                thumbnailUrl = "",
                regionData = new Dictionary<string, RegionDataItem[]>(),
                itemIds = { },
                tags = { },
                maxCountPerUser = -1,
                maxCount = 1,
                boothName = "",
                displayOrder = 0,
                clazz = "classless",
            };
            tierSeasonPassRequest.regionData.Add("US", tierRegionDataItem);
            tierSeasonPassRequest.localizations.Add("en", itemLocalization);

            Result<TestHelper.FullItemInfo> tierItemInfoResponse = null;
            this.helper.CreateItem(this.helperAccessToken, SeasonStoreInfo.storeId, tierSeasonPassRequest, result => 
            { 
                tierItemInfoResponse = result;
                TierSeasonPassItem = tierItemInfoResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => tierItemInfoResponse);
            TestHelper.Assert.IsResultOk(tierItemInfoResponse);

            RegionDataItem[] currencyItemRegionDataItem = new RegionDataItem[1];
            currencyItemRegionDataItem[0] = new RegionDataItem
            {
                price = 0,
                discountPercentage = 0,
                discountAmount = 0,
                discountedPrice = 0,
                currencyCode = currencyCode,
                currencyType = CurrencyType.VIRTUAL.ToString(),
                currencyNamespace = accountNamespace,
                purchaseAt = DateTime.UtcNow,
                expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                discountPurchaseAt = DateTime.UtcNow,
                discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
            };

            TestHelper.ItemCreateModel currencyItemRequest = new TestHelper.ItemCreateModel
            {
                itemType = ItemType.COINS.ToString(),
                seasonType = SeasonType.PASS,
                name = "Currency Coin Pass Unity Integration Test",
                entitlementType = EntitlementType.CONSUMABLE.ToString(),
                useCount = 1,
                stackable = true,
                appId = "",
                appType = EntitlementAppType.GAME.ToString(),
                baseAppId = "",
                targetCurrencyCode = currencyCode,
                targetNamespace = accountNamespace,
                categoryPath = seasonItemCategoryPath,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                status = ItemStatus.ACTIVE.ToString(),
                sku = "CoinSeasonPassSkuUnity",
                images = itemImage,
                thumbnailUrl = "",
                regionData = new Dictionary<string, RegionDataItem[]>(),
                itemIds = { },
                tags = { },
                maxCountPerUser = -1,
                maxCount = -1,
                boothName = "",
                displayOrder = 0,
                clazz = "classless",
            };
            currencyItemRequest.regionData.Add("US", currencyItemRegionDataItem);
            currencyItemRequest.localizations.Add("en", itemLocalization);

            Result<TestHelper.FullItemInfo> createCurrencyResponse = null;
            this.helper.CreateItem(this.helperAccessToken, SeasonStoreInfo.storeId, currencyItemRequest, result => 
            { 
                createCurrencyResponse = result;
                SeasonCurrencyItemInfo = createCurrencyResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => createCurrencyResponse);
            TestHelper.Assert.IsResultOk(createCurrencyResponse);

            RegionDataItem[] itemRewardRegionDataItem = new RegionDataItem[1];
            itemRewardRegionDataItem[0] = new RegionDataItem
            {
                price = 0,
                discountPercentage = 0,
                discountAmount = 0,
                discountedPrice = 0,
                currencyCode = currencyCode,
                currencyType = CurrencyType.VIRTUAL.ToString(),
                currencyNamespace = accountNamespace,
                purchaseAt = DateTime.UtcNow,
                expireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
                discountPurchaseAt = DateTime.UtcNow,
                discountExpireAt = DateTime.UtcNow + TimeSpan.FromDays(1000),
            };

            TestHelper.ItemCreateModel itemRewardRequest = new TestHelper.ItemCreateModel
            {
                itemType = ItemType.INGAMEITEM.ToString(),
                seasonType = SeasonType.PASS,
                name = "Item Reward Unity Integration Test",
                entitlementType = EntitlementType.CONSUMABLE.ToString(),
                useCount = 1,
                stackable = true,
                appId = "",
                appType = EntitlementAppType.GAME.ToString(),
                baseAppId = "",
                targetCurrencyCode = currencyCode,
                targetNamespace = accountNamespace,
                categoryPath = seasonItemCategoryPath,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                status = ItemStatus.ACTIVE.ToString(),
                sku = "ItemRewardSeasonPassSkuUnity",
                images = itemImage,
                thumbnailUrl = "",
                regionData = new Dictionary<string, RegionDataItem[]>(),
                itemIds = { },
                tags = { },
                maxCountPerUser = -1,
                maxCount = 1,
                boothName = "",
                displayOrder = 0,
                clazz = "classless",
            };
            itemRewardRequest.regionData.Add("US", itemRewardRegionDataItem);
            itemRewardRequest.localizations.Add("en", itemLocalization);

            Result<TestHelper.FullItemInfo> itemRewardResponse = null;
            this.helper.CreateItem(this.helperAccessToken, SeasonStoreInfo.storeId, itemRewardRequest, result => 
            { 
                itemRewardResponse = result;
                SeasonRewardItemInfo = itemRewardResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => itemRewardResponse);
            TestHelper.Assert.IsResultOk(itemRewardResponse);

            SeasonPassExcessStrategy excessStrategy = new SeasonPassExcessStrategy
            {
                method = SeasonPassStrategyMethod.NONE,
                currency = currencyCode,
                percentPerExp = 1,
            };

            TestHelper.ItemCreateModel.Localization seasonLocalization = new TestHelper.ItemCreateModel.Localization
            {
                title = "Season Pass",
                description = "Level up your season pass to earn the rewards",
                longDescription = "Level up your season pass to earn the rewards"
            };

            Image[] seasonImage = new Image[1];
            seasonImage[0] = new Image
            {
                As = "image",
                caption = "seasonImage",
                height = 32,
                width = 32,
                imageUrl = "http://example.com",
                smallImageUrl = "http://example.com",
            };

            string start = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string end = (DateTime.UtcNow + TimeSpan.FromDays(1000)).ToString("yyyy-MM-ddTHH:mm:ssZ");

            SeasonPassTestAdmin.SeasonPassCreateSeasonRequest seasonPassCreateSeasonRequest = new SeasonPassTestAdmin.SeasonPassCreateSeasonRequest
            {
                name = "Season Pass Unity",
                start = start,
                end = end,
                defaultLanguage = "en",
                defaultRequiredExp = seasonTierRequiredExp,
                draftStoreId = SeasonStoreInfo.storeId,
                tierItemId = TierSeasonPassItem.itemId,
                autoClaim = false,
                excessStrategy = excessStrategy,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                images = seasonImage,
            };
            seasonPassCreateSeasonRequest.localizations.Add("en", seasonLocalization);

            Result<SeasonPassTestAdmin.SeasonPassCreateSeasonResponse> seasonPassCreateResponse = null;
            this.admin.SeasonCreateSeason(accountNamespace, this.helperAccessToken, seasonPassCreateSeasonRequest, result => 
            { 
                seasonPassCreateResponse = result;
                SeasonResponse = seasonPassCreateResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => seasonPassCreateResponse);
            TestHelper.Assert.IsResultOk(seasonPassCreateResponse);
            
            //free pass
            TestHelper.ItemCreateModel.Localization freePassLocalization = new TestHelper.ItemCreateModel.Localization
            {
                title = "Free Pass",
                description = "Level up your free pass to earn the rewards for free",
                longDescription = "Level up your free pass to earn the rewards for free"
            };

            SeasonPassTestAdmin.SeasonPassCreatePassRequest freePassRequest = new SeasonPassTestAdmin.SeasonPassCreatePassRequest
            {
                code = "free-pass-unity",
                displayOrder = 0,
                autoEnroll = true,
                passItemId = FreeSeasonPassItem.itemId,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                images = seasonImage,
            };
            freePassRequest.localizations.Add("en", freePassLocalization);

            Result<SeasonPassTestAdmin.SeasonPassCreatePassResponse> createFreePassResponse = null;
            this.admin.SeasonCreatePass(accountNamespace, this.helperAccessToken, SeasonResponse.id, freePassRequest, result => 
            { 
                createFreePassResponse = result; 
                FreePassResponse = createFreePassResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => createFreePassResponse);
            TestHelper.Assert.IsResultOk(createFreePassResponse);

            //premium pass
            TestHelper.ItemCreateModel.Localization premiumPassLocalization = new TestHelper.ItemCreateModel.Localization
            {
                title = "Premium Pass",
                description = "Level up your premium pass to earn the rewards for only 100 coins",
                longDescription = "Level up your premium pass to earn the rewards for only 100 coins"
            };

            SeasonPassTestAdmin.SeasonPassCreatePassRequest premiumPassRequest = new SeasonPassTestAdmin.SeasonPassCreatePassRequest
            {
                code = "premium-pass-unity",
                displayOrder = 0,
                autoEnroll = false,
                passItemId = PremiumSeasonPassItem.itemId,
                localizations = new Dictionary<string, TestHelper.ItemCreateModel.Localization>(),
                images = seasonImage,
            };
            premiumPassRequest.localizations.Add("en", premiumPassLocalization);

            Result<SeasonPassTestAdmin.SeasonPassCreatePassResponse> createPremiumPassResponse = null;
            this.admin.SeasonCreatePass(accountNamespace, this.helperAccessToken, seasonPassCreateResponse.Value.id, premiumPassRequest, result => 
            { 
                createPremiumPassResponse = result; 
                PremiumPassResponse = createPremiumPassResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => createPremiumPassResponse);
            TestHelper.Assert.IsResultOk(createPremiumPassResponse);

            //free reward
            SeasonPassTestAdmin.SeasonPassCreateRewardRequest coinsReward = new SeasonPassTestAdmin.SeasonPassCreateRewardRequest
            {
                code = currencyRewardCode,
                type = "ITEM",
                itemId = SeasonCurrencyItemInfo.itemId,
                quantity = currencyRewardQuantity,
                images = seasonImage,
            };

            Result<SeasonPassTestAdmin.SeasonPassCreateRewardResponse> createFreeRewardResponse = null;
            this.admin.SeasonCreateReward(accountNamespace, this.helperAccessToken, seasonPassCreateResponse.Value.id, coinsReward, result => 
            { 
                createFreeRewardResponse = result;
                CurrencyRewardResponse = createFreeRewardResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => createFreeRewardResponse);
            TestHelper.Assert.IsResultOk(createFreeRewardResponse);

            //premium reward
            SeasonPassTestAdmin.SeasonPassCreateRewardRequest premiumReward = new SeasonPassTestAdmin.SeasonPassCreateRewardRequest
            {
                code = itemRewardCode,
                type = "ITEM",
                itemId = SeasonRewardItemInfo.itemId,
                quantity = 1,
                images = seasonImage,
            };

            Result<SeasonPassTestAdmin.SeasonPassCreateRewardResponse> createPremiumRewardResponse = null;
            this.admin.SeasonCreateReward(accountNamespace, this.helperAccessToken, SeasonResponse.id, premiumReward, result => 
            { 
                createPremiumRewardResponse = result;
                ItemRewardResponse = createPremiumRewardResponse.Value;
            });
            yield return TestHelper.WaitForValue(() => createPremiumRewardResponse);
            TestHelper.Assert.IsResultOk(createPremiumRewardResponse);

            // make sure SeasonTierCount is not less than 4! If less, it will not coverage all the test. //set to 4
            if (seasonTierCount < 4)
            {
                seasonTierCount = 4;
            }

            object[] seasonTierRewards = new object[1];
            seasonTierRewards = null;
            object[] freeRewardCodes = new object[] { currencyRewardCode = CurrencyRewardResponse.code };
            object[] premiumRewardCodes = new object[] { currencyRewardCode = CurrencyRewardResponse.code, itemRewardCode = ItemRewardResponse.code };

            Dictionary<string, object> RewardValue = new Dictionary<string, object>();
            RewardValue.Add(FreePassResponse.code, freeRewardCodes);
            RewardValue.Add(PremiumPassResponse.code, premiumRewardCodes);

            SeasonPassTestAdmin.SeasonPassTierRequest Tier = new SeasonPassTestAdmin.SeasonPassTierRequest
            {
                requiredExp = seasonTierRequiredExp,
                rewards = RewardValue,
            };

            SeasonPassTestAdmin.SeasonPassCreateTierRequest TierRequest = new SeasonPassTestAdmin.SeasonPassCreateTierRequest
            {
                index = 0,
                quantity = 6,
                tier = Tier,
            };

            Result<SeasonPassTierJsonObject[]> createTierResponse = null;
            this.admin.SeasonCreateTier(accountNamespace, this.helperAccessToken, SeasonResponse.id, TierRequest, result => 
            { 
                createTierResponse = result;
            });
            yield return TestHelper.WaitForValue(() => createTierResponse);
            TestHelper.Assert.IsResultOk(createTierResponse);
            
            //publish store and season pass
            //publish this store, CreatedTemporarySore
            Result<TestHelper.StoreInfoModel> publishStoreResponse = null;
            this.helper.CloneStore(accountNamespace, this.helperAccessToken, SeasonStoreInfo.storeId, "", result => { publishStoreResponse = result; });
            yield return TestHelper.WaitForValue(() => publishStoreResponse);
            TestHelper.Assert.IsResultOk(publishStoreResponse);

            Result<SeasonPassTestAdmin.SeasonPassCreateSeasonResponse> publishSeasonResponse = null;
            this.admin.SeasonPublishSeason(accountNamespace, this.helperAccessToken, SeasonResponse.id, result => { publishSeasonResponse = result; });
            yield return TestHelper.WaitForValue(() => publishSeasonResponse);
            TestHelper.Assert.IsResultOk(publishSeasonResponse);

            loginWithDevice = null;
            this.user.LoginWithDeviceId(result => { loginWithDevice = result; });
            yield return TestHelper.WaitForValue(() => loginWithDevice);
            TestHelper.Assert.IsTrue(!loginWithDevice.IsError || loginWithDevice.Error.Code == ErrorCode.InvalidRequest, "User cannot login.");
        }

        [UnityTest, TestLog, Order(999), Timeout(150000)]
        public IEnumerator Teardown()
        {
            //query, unpublish and delete season
            Result<SeasonPassTestAdmin.SeasonPassCreateGetSeasonPagingRespon> seasonResponse = null;
            SeasonPassStatus[] seasonPassStatuses = { SeasonPassStatus.DRAFT, SeasonPassStatus.PUBLISHED };
            this.admin.SeasonQuerySeason(accountNamespace, this.helperAccessToken, seasonPassStatuses, result => { seasonResponse = result; });
            yield return TestHelper.WaitForValue(() => seasonResponse);
            TestHelper.Assert.IsResultOk(seasonResponse);

            foreach (SeasonPassTestAdmin.SeasonPassCreateGetSeasonResponse season in seasonResponse.Value.data)
            {
                if (season.status == SeasonPassStatus.PUBLISHED)
                {
                    //unpublish before delete
                    Result<SeasonPassTestAdmin.SeasonPassCreateSeasonResponse> unpublishSeasonResponse = null;
                    this.admin.SeasonForceUnpublishSeason(accountNamespace, this.helperAccessToken, season.id, result => { unpublishSeasonResponse = result; });
                    yield return TestHelper.WaitForValue(() => unpublishSeasonResponse);
                    TestHelper.Assert.IsResultOk(unpublishSeasonResponse);
                }

                Result deleteSeason = null;
                this.admin.SeasonDeleteSeason(accountNamespace, this.helperAccessToken, season.id, result => { deleteSeason = result; });
                yield return TestHelper.WaitForValue(() => deleteSeason);
                TestHelper.Assert.IsResultOk(deleteSeason);
            }

            //delete testing currency
            Result<TestHelper.CurrencyInfoModel> deleteCurrencyResponse = null;
            this.helper.DeleteCurrency(this.helperAccessToken, currencyCode, result => { deleteCurrencyResponse = result; });
            yield return TestHelper.WaitForValue(() => deleteCurrencyResponse);
            TestHelper.Assert.IsResultOk(deleteCurrencyResponse);

            //delete published testing store and delete all draft store
            Result<TestHelper.StoreInfoModel> deletePublishedStore = null;
            this.helper.DeletePublishedStore(this.helperAccessToken, result => { deletePublishedStore = result; });
            yield return TestHelper.WaitForValue(() => deletePublishedStore);
            TestHelper.Assert.IsResultOk(deletePublishedStore);

            Result<TestHelper.StoreInfoModel[]> getStoreList = null;
            this.helper.GetStoreList(this.helperAccessToken, result => { getStoreList = result; });
            yield return TestHelper.WaitForValue(() => getStoreList);
            TestHelper.Assert.IsResultOk(getStoreList);

            for (int i = 0; i < getStoreList.Value.Length; i++)
            {
                Result<TestHelper.StoreInfoModel> deleteStoreResponse = null;
                this.helper.DeleteStore(this.helperAccessToken, getStoreList.Value[i].storeId, result => { deleteStoreResponse = result; });
                yield return TestHelper.WaitForValue(() => deleteStoreResponse);
                TestHelper.Assert.IsResultOk(deleteStoreResponse);
            }

            if (this.user.Session.IsValid())
            {
                Result logoutResult = null;

                this.user.Logout(result => logoutResult = result);

                while (logoutResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsFalse(logoutResult.IsError, "User cannot log out.");
            }
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator Get_Current_Season()
        {
            Result<SeasonInfo> seasonResponse = null;
            this.seasonPass.GetCurrentSeason(language, result => { seasonResponse = result; });
            yield return TestHelper.WaitForValue(() => seasonResponse);
            TestHelper.Assert.IsResultOk(seasonResponse);
            TestHelper.Assert.IsTrue(CheckCurrentSeason(seasonResponse.Value));

            Result<UserSeasonInfo> userSeasonResponse = null;
            this.seasonPass.GetCurrentUserSeason(result => { userSeasonResponse = result; });
            yield return TestHelper.WaitForValue(() => userSeasonResponse);
            TestHelper.Assert.IsResultOk(userSeasonResponse);
            TestHelper.Assert.IsTrue(CheckUserSeason(userSeasonResponse.Value));

            userSeasonResponse = null;
            this.seasonPass.GetUserSeason(SeasonResponse.id, result => 
            { 
                userSeasonResponse = result;
            });
            yield return TestHelper.WaitForValue(() => userSeasonResponse);
            TestHelper.Assert.IsResultOk(userSeasonResponse);
            TestHelper.Assert.IsTrue(CheckUserSeason(userSeasonResponse.Value));
        }

        [UnityTest, TestLog, Order(1)]
        public IEnumerator Get_Invalid_User_Season()
        {
            Result<UserSeasonInfo> userSeasonResponse = null;
            this.seasonPass.GetUserSeason("INVALID_ID", result => { userSeasonResponse = result; });
            yield return TestHelper.WaitForValue(() => userSeasonResponse);
            TestHelper.Assert.IsTrue(userSeasonResponse.IsError, "Get user season with invalid session id failed.");
        }

        [UnityTest, TestLog, Order(2)]
        public IEnumerator Enroll_PremiumPass()
        {
            
            ItemCriteria itemCriteria = new ItemCriteria
            {
                language = "en",
                region = "US",
                itemType = ItemType.SEASON,
                categoryPath = seasonItemCategoryPath,
            };

            ItemInfo premiumPassItemInfo = new ItemInfo();
            Result<ItemPagingSlicedResult> res = null;
            bool expectedItemFound = false;
            this.item.GetItemsByCriteria(itemCriteria, result =>
            {
                res = result;

                for (int i = 0; i < res.Value.data.Length; i++)
                {
                    if (res.Value.data[i].name == PremiumSeasonPassItem.name)
                    {
                        premiumPassItemInfo = res.Value.data[i];
                        expectedItemFound = true;
                    }
                }
            });
            yield return TestHelper.WaitForValue(() => res);
            TestHelper.Assert.IsTrue(expectedItemFound);

            Result loginServerResult = null;
            this.server.LoginWithClientCredentials(result => loginServerResult = result);
            while (loginServerResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.Assert.IsTrue(!loginServerResult.IsError, "Server cannot login.");

            //grant currency
            CreditUserWalletRequest creditUserWalletRequest = new CreditUserWalletRequest
            {
                amount = premiumPassPrice,
                source = CreditUserWalletSource.OTHER,
                reason = "Unity integration test",
            };

            Result<WalletInfo> walletResponse = null;
            this.serverEcommerce.CreditUserWallet(user.Session.UserId, currencyCode, creditUserWalletRequest, result => { walletResponse = result; });
            yield return TestHelper.WaitForValue(() => walletResponse);
            TestHelper.Assert.IsResultOk(walletResponse);

            int quantity = 1;
            OrderRequest orderRequest = new OrderRequest
            {
                itemId = premiumPassItemInfo.itemId,
                quantity = quantity,
                price = premiumPassItemInfo.regionData[0].price * quantity,
                discountedPrice = premiumPassItemInfo.regionData[0].discountedPrice * quantity,
                currencyCode = premiumPassItemInfo.regionData[0].currencyCode,
                region = "US",
                language = "en",
                returnUrl = "https://sdk.example.com",
            };

            
            Result<OrderInfo> orderInfo = null;
            this.order.CreateOrder(orderRequest, result => { orderInfo = result; });
            yield return TestHelper.WaitForValue(() => orderInfo);
            TestHelper.Assert.IsResultOk(orderInfo);

            Result<UserSeasonInfo> userSeasonResponse = null;
            this.seasonPass.GetUserSeason(SeasonResponse.id, result => { userSeasonResponse = result; });
            yield return TestHelper.WaitForValue(() => userSeasonResponse);
            TestHelper.Assert.IsResultOk(userSeasonResponse);
            TestHelper.Assert.IsTrue(userSeasonResponse.Value.seasonId == SeasonResponse.id);
            TestHelper.Assert.IsTrue(userSeasonResponse.Value.requiredExp == seasonTierRequiredExp);
            bool enrollPassed = false;
            foreach (string enrollPass in userSeasonResponse.Value.enrolledPasses)
            {
                if (enrollPass == PremiumPassResponse.code)
                {
                    enrollPassed = true;
                }
            }
            TestHelper.Assert.IsTrue(enrollPassed);
        }

        [UnityTest, TestLog, Order(3)]
        public IEnumerator Gain_Experience()
        {
            Result loginServerResult = null;
            this.server.LoginWithClientCredentials(result => { loginServerResult = result; });
            while (loginServerResult == null)
            {
                Thread.Sleep(100);
                yield return null;
            }
            TestHelper.Assert.IsTrue(!loginServerResult.IsError, "Server cannot login.");

            //gain exp by one level
            //using ServerApi
            int grantedExp = System.Convert.ToInt32(seasonTierRequiredExp * 0.5);
            Result<UserSeasonInfoWithoutReward> userSeasonAfterGrantedExp = null;
            this.serverSeasonPass.GrantExpToUser(user.Session.UserId, grantedExp, result => { userSeasonAfterGrantedExp = result; });
            yield return TestHelper.WaitForValue(() => userSeasonAfterGrantedExp);
            TestHelper.Assert.IsResultOk(userSeasonAfterGrantedExp);
            TestHelper.Assert.IsFalse(default(int) == userSeasonAfterGrantedExp.Value.currentExp);
        }

        [UnityTest, TestLog, Order(4)]
        public IEnumerator Get_Season_Tier()
        {
            ItemCriteria itemCriteria = new ItemCriteria
            {
                language = "en",
                region = "US",
                itemType = ItemType.SEASON,
                categoryPath = seasonItemCategoryPath,
            };

            ItemInfo tierItemInfo = null;
            Result<ItemPagingSlicedResult> res = null;
            bool expectedItemFound = false;
            this.item.GetItemsByCriteria(itemCriteria, result =>
            {
                res = result;

                for (int i = 0; i < res.Value.data.Length; i++)
                {
                    if (res.Value.data[i].name == TierSeasonPassItem.name)
                    {
                        tierItemInfo = res.Value.data[i];
                        expectedItemFound = true;
                    }
                }
            });
            yield return TestHelper.WaitForValue(() => res);
            TestHelper.Assert.IsTrue(expectedItemFound);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator Claim_Tier_Rewards()
        {
            Result<UserSeasonInfo> userSeasonResponse = null;
            this.seasonPass.GetUserSeason(SeasonResponse.id, result => { userSeasonResponse = result; });
            yield return TestHelper.WaitForValue(() => userSeasonResponse);
            TestHelper.Assert.IsResultOk(userSeasonResponse);
            TestHelper.Assert.IsTrue(CheckUserSeason(userSeasonResponse.Value));

            while (userSeasonResponse.Value.toClaimRewards.ToArray().Length == 0)
            {
                //grant exp to level up
                Result loginServerResult = null;
                this.server.LoginWithClientCredentials(result => loginServerResult = result);
                while (loginServerResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                TestHelper.Assert.IsTrue(!loginServerResult.IsError, "Server cannot login.");

                //gain exp by one level
                //using ServerApi
                Result<UserSeasonInfoWithoutReward> userSeasonAfterGrantedExp = null;
                this.serverSeasonPass.GrantExpToUser(user.Session.UserId, seasonTierRequiredExp, result => { userSeasonAfterGrantedExp = result; });
                yield return TestHelper.WaitForValue(() => userSeasonAfterGrantedExp);
                TestHelper.Assert.IsResultOk(userSeasonAfterGrantedExp);

                userSeasonResponse = null;
                this.seasonPass.GetCurrentUserSeason(result => { userSeasonResponse = result; });
                yield return TestHelper.WaitForValue(() => userSeasonResponse);
                TestHelper.Assert.IsResultOk(userSeasonResponse);
                TestHelper.Assert.IsTrue(CheckUserSeason(userSeasonResponse.Value));
            }

            var claimRewardKey = from claimRewards in userSeasonResponse.Value.toClaimRewards.Keys
                                 select claimRewards;

            var passRewardKey = from UserSeasonInfo in userSeasonResponse.Value.toClaimRewards.Values
                                from passReward in UserSeasonInfo.Keys
                                select passReward;

            var passRewardValue = from UserSeasonInfo in userSeasonResponse.Value.toClaimRewards.Values
                          from passReward in UserSeasonInfo.Values
                          from rewardCodes in passReward
                          select rewardCodes;

            SeasonClaimRewardRequest claimRewardRequest = new SeasonClaimRewardRequest
            {
                passCode = passRewardKey.First(),
                tierIndex = claimRewardKey.First(),
                rewardCode = passRewardValue.First(),
            };

            Result <SeasonClaimRewardResponse> claimRewardResponse = null;
            this.seasonPass.ClaimRewards(claimRewardRequest, result => { claimRewardResponse = result; });
            yield return TestHelper.WaitForValue(() => claimRewardResponse);
            TestHelper.Assert.IsResultOk(claimRewardResponse);
            CheckClaimReward(claimRewardResponse.Value);
        }

        [UnityTest, TestLog, Order(5)]
        public IEnumerator Bulk_Claim_Tier_Rewards()
        {
            int lastBallanceCount = 0;
            Result<WalletInfo> walletInfo = null;
            this.wallet.GetWalletInfoByCurrencyCode(currencyCode, result =>
            {
                walletInfo = result;
                lastBallanceCount = walletInfo.Value.balance;
            });
            yield return TestHelper.WaitForValue(() => walletInfo);
            TestHelper.Assert.IsResultOk(walletInfo);

            int entitlementUseCount = 0;
            Result<EntitlementPagingSlicedResult> entitlementResponse = null;
            this.entitlement.QueryUserEntitlements("", ItemRewardResponse.itemId, 0, 20, result =>
            {
                entitlementResponse = result;
                foreach (EntitlementInfo ent in entitlementResponse.Value.data)
                {
                    entitlementUseCount = ent.useCount;
                    break;
                }
            });
            yield return TestHelper.WaitForValue(() => entitlementResponse);
            TestHelper.Assert.IsResultOk(entitlementResponse);

            Result<UserSeasonInfo> userSeasonResponse = null;
            this.seasonPass.GetUserSeason(SeasonResponse.id, result => { userSeasonResponse = result; });
            yield return TestHelper.WaitForValue(() => userSeasonResponse);
            TestHelper.Assert.IsResultOk(userSeasonResponse);
            TestHelper.Assert.IsTrue(CheckUserSeason(userSeasonResponse.Value));

            //loop until test have claim reward more than 1
            while (userSeasonResponse.Value.toClaimRewards.ToArray().Length < 2)
            {
                //prevent infinite loop
                TestHelper.Assert.IsTrue(userSeasonResponse.Value.currentTierIndex < userSeasonResponse.Value.lastTierIndex);
                //grant exp to level up
                Result loginServerResult = null;
                this.server.LoginWithClientCredentials(result => loginServerResult = result);
                while (loginServerResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }
                TestHelper.Assert.IsTrue(!loginServerResult.IsError, "Server cannot login.");

                int levelUp = 2 - userSeasonResponse.Value.toClaimRewards.ToArray().Length;
                int valueGranted = seasonTierRequiredExp * levelUp;
                //gain exp by one level
                //using ServerApi
                Result<UserSeasonInfoWithoutReward> userSeasonAfterGrantedExp = null;
                this.serverSeasonPass.GrantExpToUser(user.Session.UserId, valueGranted, result => { userSeasonAfterGrantedExp = result; });
                yield return TestHelper.WaitForValue(() => userSeasonAfterGrantedExp);
                TestHelper.Assert.IsResultOk(userSeasonAfterGrantedExp);

                userSeasonResponse = null;
                this.seasonPass.GetCurrentUserSeason(result => { userSeasonResponse = result; });
                yield return TestHelper.WaitForValue(() => userSeasonResponse);
                TestHelper.Assert.IsResultOk(userSeasonResponse);
                TestHelper.Assert.IsTrue(CheckUserSeason(userSeasonResponse.Value));
            }

            Result<SeasonClaimRewardResponse> claimRewardResponse = null;
            this.seasonPass.BulkClaimRewards(result => { claimRewardResponse = result; });
            yield return TestHelper.WaitForValue(() => claimRewardResponse);
            TestHelper.Assert.IsResultOk(claimRewardResponse);
            CheckClaimReward(claimRewardResponse.Value);
        }

    }
}
