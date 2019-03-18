// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests.EcommerceTest
{
	public static class TestVariables
	{
		public static string rootCategoryPath;
		public static string childCategoryPath;
		public static string grandChildCategoryPath;
		public static string currencyCode;
		public static string inGameItemTitle;
		public static string currencyItemTitle;
		public static string language;
		public static string region;
		public static string SdkCloneStoreId;
		public static string ArchiveOriStoreId;
		public static string publishedStoreId;
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

		[UnityTest]
		public IEnumerator A2_PrepareDependencies()
		{
			TestVariables.language = "en";
			TestVariables.region = "US";
			TestHelper testHelper = new TestHelper();
			var user = AccelBytePlugin.GetUser();

			//Get AccessToken
			Result<TokenData> GetAccessToken = null;
			testHelper.GetAccessToken(result => { GetAccessToken = result; });
			while (GetAccessToken == null) { yield return null; }
			string accessToken = GetAccessToken.Value.access_token;

			TestHelper.CurrencyCreateModel createCurrency = new TestHelper.CurrencyCreateModel {
				currencyCode = "SdkCoin",
				currencySymbol = "SDKC",
				currencyType = "VIRTUAL",
				decimals = 0,
				maxAmountPerTransaction = -1,
				maxTransactionAmountPerDay = -1,
				maxBalanceAmount = -1
			};

			AuthenticationApi authenticationApi = new AuthenticationApi(AccelBytePlugin.Config.PlatformServerUrl);

			//Check currency isNotExist? makeNew : use Existing;
			Result<TestHelper.CurrencySummaryModel> getResult = null;
			testHelper.GetCurrencySummary(accessToken, createCurrency.currencyCode, result => { getResult = result; });
			while (getResult == null) { yield return null; }
			if (getResult.IsError)
			{
				Result<TestHelper.CurrencyInfoModel> createResult = null;
				testHelper.CreateCurrency(accessToken, createCurrency, result => { createResult = result; });
				while (createResult == null) { yield return null; }
				Assert.That(!createResult.IsError);
				TestVariables.currencyCode = createResult.Value.currencyCode;
			}
			else
			{
				TestVariables.currencyCode = getResult.Value.currencyCode;
			}
			Assert.That(TestVariables.currencyCode != null);

			//Get published store
			Result<TestHelper.StoreInfoModel> publishedStore = null;
			testHelper.GetPublishedStore(accessToken, result => { publishedStore = result; });
			while (publishedStore == null) { yield return null; }
			if(publishedStore.IsError)
				if (publishedStore.Error.Code.ToString() == "3044")
					TestVariables.bPublishedStoreIsExist = false;
			Assert.That(!publishedStore.IsError || publishedStore.Error.Code.ToString() == "3044");
			if(TestVariables.bPublishedStoreIsExist)
				TestVariables.publishedStoreId = publishedStore.Value.storeId;

			//Create temp store
			TestHelper.StoreCreateModel SdkClone = new TestHelper.StoreCreateModel {
				title = "SdkCloneTemporary",
				description = "SdkIntegrationTesting",
				supportedLanguages = new string[] { "en" },
				supportedRegions = new string[] { "US" },
				defaultLanguage = "en",
				defaultRegion = "US"
			};

			Result<TestHelper.StoreInfoModel> CreateStoreResult = null;
			testHelper.CreateStore(accessToken, SdkClone, result => { CreateStoreResult = result; });
			while (CreateStoreResult == null) { yield return null; }
			TestVariables.SdkCloneStoreId = CreateStoreResult.Value.storeId;
			Assert.That(TestVariables.SdkCloneStoreId != null);

			Result<TestHelper.StoreInfoModel> cloneResult = null;
			if (TestVariables.bPublishedStoreIsExist)
			{
				//Create archive publisher store
				TestHelper.StoreCreateModel ArchiveOri = new TestHelper.StoreCreateModel
				{
					title = "ArchiveOriginalStore",
					description = "PublisheStoreStateBeforeECommerceTest",
					supportedLanguages = new string[] { "en" },
					supportedRegions = new string[] { "US" },
					defaultLanguage = "en",
					defaultRegion = "US"
				};

				CreateStoreResult = null;
				testHelper.CreateStore(accessToken, ArchiveOri, result => { CreateStoreResult = result; });
				while (CreateStoreResult == null) { yield return null; }
				TestVariables.ArchiveOriStoreId = CreateStoreResult.Value.storeId;
				Assert.That(TestVariables.ArchiveOriStoreId != null);

				testHelper.CloneStore(accessToken, TestVariables.publishedStoreId, TestVariables.ArchiveOriStoreId, result => { cloneResult = result; });
				while (cloneResult == null) { yield return null; }
				Assert.That(!cloneResult.IsError);
			}

			TestHelper.CategoryCreateModel SdkRootCategory = new TestHelper.CategoryCreateModel {
				categoryPath = "/SdkRootCategory",
				localizationDisplayNames = new Dictionary<string, string>()
			};
			SdkRootCategory.localizationDisplayNames.Add("en", "TestingRootCategory");
			TestVariables.rootCategoryPath = SdkRootCategory.categoryPath;

			TestHelper.CategoryCreateModel SdkChildCategory = new TestHelper.CategoryCreateModel {
				categoryPath = "/SdkRootCategory/SdkChildCategory",
				localizationDisplayNames = new Dictionary<string, string>()
			};
			SdkChildCategory.localizationDisplayNames.Add("en", "TestingChildCategory");
			TestVariables.childCategoryPath = SdkChildCategory.categoryPath;

			TestHelper.CategoryCreateModel SdkGrandChildCategory = new TestHelper.CategoryCreateModel {
				categoryPath = "/SdkRootCategory/SdkChildCategory/GrandChildCategory",
				localizationDisplayNames = new Dictionary<string, string>()
				};
			SdkGrandChildCategory.localizationDisplayNames.Add("en", "TestingGrandChildCategory");
			TestVariables.grandChildCategoryPath = SdkGrandChildCategory.categoryPath;

			Result<TestHelper.FullCategoryInfo> createCategoryResult = null;
			testHelper.CreateCategory(accessToken, TestVariables.SdkCloneStoreId, SdkRootCategory, result => { createCategoryResult = result; });
			while (createCategoryResult == null) { yield return null; }
			Assert.That(!createCategoryResult.IsError);

			createCategoryResult = null;
			testHelper.CreateCategory(accessToken, TestVariables.SdkCloneStoreId, SdkChildCategory, result => { createCategoryResult = result; });
			while (createCategoryResult == null) { yield return null; }
			Assert.That(!createCategoryResult.IsError);

			createCategoryResult = null;
			testHelper.CreateCategory(accessToken, TestVariables.SdkCloneStoreId, SdkGrandChildCategory, result => { createCategoryResult = result; });
			while (createCategoryResult == null) { yield return null; }
			Assert.That(!createCategoryResult.IsError);

			TestHelper.ItemCreateModel.Localization localization = new TestHelper.ItemCreateModel.Localization {
				title = "RootItem",
				description = "testing item for SDK"
			};
			TestHelper.ItemCreateModel.Localizations localizations = new TestHelper.ItemCreateModel.Localizations();
			localizations.en = localization;
			RegionData[] regionData = new RegionData[1];
			regionData[0] = new RegionData {
				price = 2,
				discountPercentage = 0,
				discountAmount = 0,
				currencyCode = "SdkCoin",
				currencyType = "VIRTUAL",
				currencyNamespace = AccelBytePlugin.Config.Namespace,
				purchaseAt = DateTime.Now,
				expireAt = DateTime.MaxValue,
				discountPurchaseAt = DateTime.Now,
				discountExpireAt = DateTime.MaxValue
			};
			TestHelper.ItemCreateModel rootItem = new TestHelper.ItemCreateModel
			{
				itemType = "INGAMEITEM",
				name = "RootItem",
				entitlementType = "DURABLE",
				useCount = 0,
				categoryPath = TestVariables.rootCategoryPath,
				status = "ACTIVE",
				sku = "skuRootItem",
				localizations = localizations,
				regionData = new TestHelper.ItemCreateModel.RegionDatas(),
				maxCount = -1,
				maxCountPerUser = -1
			};
			rootItem.regionData.US = regionData;
			TestVariables.inGameItemTitle = rootItem.localizations.en.title;

			Result<TestHelper.FullItemInfo> createItemResult = null;
			testHelper.CreateItem(accessToken, TestVariables.SdkCloneStoreId, rootItem, result => { createItemResult = result; });
			while (createItemResult == null) { yield return null; }
			Assert.That(!createItemResult.IsError);

			createItemResult = null;
			localization = null;
			localization = new TestHelper.ItemCreateModel.Localization
			{
				title = "ChildItem",
				description = "testing item for SDK"
			};
			localizations = null;
			localizations = new TestHelper.ItemCreateModel.Localizations();
			localizations.en = localization;
			regionData = null;
			regionData = new RegionData[1];
			regionData[0] = new RegionData {
				price = 0,
				discountPercentage = 0,
				discountAmount = 0,
				currencyCode = "USD",
				currencyType = "REAL",
				currencyNamespace = AccelBytePlugin.Config.Namespace,
				purchaseAt = DateTime.Now,
				expireAt = DateTime.MaxValue,
				discountPurchaseAt = DateTime.Now,
				discountExpireAt = DateTime.MaxValue
			};
			TestHelper.ItemCreateModel childItem = new TestHelper.ItemCreateModel {
				itemType = "COINS",
				name = "ChildItem",
				entitlementType = "CONSUMABLE",
				useCount = 20,
				targetCurrencyCode = "SdkCoin",
				categoryPath = TestVariables.childCategoryPath,
				status = "ACTIVE",
				sku = "skuChildItem",
				localizations = localizations,
				regionData = new TestHelper.ItemCreateModel.RegionDatas(),
				maxCount = -1,
				maxCountPerUser = -1
			};
			childItem.regionData.US = regionData;
			TestVariables.currencyItemTitle = childItem.localizations.en.title;

			createItemResult = null;
			testHelper.CreateItem(accessToken, TestVariables.SdkCloneStoreId, childItem, result => { createItemResult = result; });
			while(createItemResult == null) { yield return null; }
			Assert.That(!createItemResult.IsError);

			createItemResult = null;
			localization = null;
			localization = new TestHelper.ItemCreateModel.Localization
			{
				title = "GrandChildItem",
				description = "testing item for SDK"
			};
			localizations = null;
			localizations = new TestHelper.ItemCreateModel.Localizations();
			localizations.en = localization;
			regionData = null;
			regionData = new RegionData[1];
			regionData[0] = new RegionData
			{
				price = 0,
				discountPercentage = 0,
				discountAmount = 0,
				currencyCode = "USD",
				currencyType = "REAL",
				currencyNamespace = AccelBytePlugin.Config.Namespace,
				purchaseAt = DateTime.Now,
				expireAt = DateTime.MaxValue,
				discountPurchaseAt = DateTime.Now,
				discountExpireAt = DateTime.MaxValue
			};
			TestHelper.ItemCreateModel grandChildItem = new TestHelper.ItemCreateModel
			{
				itemType = "COINS",
				name = "GrandChildItem",
				entitlementType = "CONSUMABLE",
				useCount = 20,
				targetCurrencyCode = "SdkCoin",
				categoryPath = TestVariables.grandChildCategoryPath,
				status = "ACTIVE",
				sku = "skuGrandChildItem",
				localizations = localizations,
				regionData = new TestHelper.ItemCreateModel.RegionDatas(),
				maxCount = -1,
				maxCountPerUser = -1
			};
			grandChildItem.regionData.US = regionData;

			createItemResult = null;
			testHelper.CreateItem(accessToken, TestVariables.SdkCloneStoreId, grandChildItem, result => { createItemResult = result; });
			while (createItemResult == null) { yield return null; }
			Assert.That(!createItemResult.IsError);

			if (TestVariables.bPublishedStoreIsExist)
			{
				cloneResult = null;
				testHelper.CloneStore(accessToken, TestVariables.SdkCloneStoreId, TestVariables.publishedStoreId, result => { cloneResult = result; });
				while (cloneResult == null) { yield return null; }
				Assert.That(!cloneResult.IsError);
				TestVariables.publishedStoreId = cloneResult.Value.storeId;
			}
			else
			{
				cloneResult = null;
				testHelper.PublishStore(accessToken, TestVariables.SdkCloneStoreId, result => { cloneResult = result; });
				while (cloneResult == null) { yield return null; }
				Assert.That(!cloneResult.IsError);
			}

			TestHelper.CreditRequestModel creditRequest = new TestHelper.CreditRequestModel
			{
				amount = 10,
				source = "OTHER",
				reason = "testing"
			};
			Result<WalletInfo> creditWalletResult = null;
			testHelper.CreditWallet(accessToken, user.UserId, TestVariables.currencyCode, creditRequest, result => { creditWalletResult = result; });
			while(creditWalletResult == null) { yield return null; }
			Assert.That(!creditWalletResult.IsError);
		}

		[UnityTest]
		public IEnumerator A1_LoginTestUser()
		{
			Result userLoginResult = null;		
			var user = AccelBytePlugin.GetUser();
			user.LoginWithDeviceId(result => { userLoginResult = result; });
			
			while (userLoginResult == null)
			{
				yield return new WaitForSeconds(0.1f);
			}

			Result<UserData> updateResult = null;
			user.Update(new UpdateUserRequest {Country = "US"},
				result => updateResult = result);
			
			while (updateResult == null) 
			{
				yield return new WaitForSeconds(0.1f);
			}
			
			Assert.That(!updateResult.IsError);
			Assert.That(!userLoginResult.IsError);	
		}
	}

	[TestFixture]
	public class Z_TearDownEcommerce
	{
		[UnityTest]
		public IEnumerator Z_DeleteDependencies()
		{
			TestHelper testHelper = new TestHelper();
			var user = AccelBytePlugin.GetUser();
			Result loginResult = null;
			user.LoginWithDeviceId(result => { loginResult = result; });
			while (loginResult == null) { yield return null; }

			Result<TokenData> GetAccessToken = null;
			testHelper.GetAccessToken(result => { GetAccessToken = result; });
			while (GetAccessToken == null) { yield return null; }
			string accessToken = GetAccessToken.Value.access_token;

			Result deleteResult = null;
			testHelper.DeleteUser(user, result => { deleteResult = result; });
//			testHelper.DeleteUser(AccelBytePlugin.Config.Namespace, PlatformType.Device, "", result => { deleteResult = result; });
			while (deleteResult == null) { yield return null; }
			Assert.That(!deleteResult.IsError);

			Result<TestHelper.StoreInfoModel> storeDeleteResult = null;
			testHelper.DeleteStore(accessToken, TestVariables.SdkCloneStoreId, result => { storeDeleteResult = result; });
			while(storeDeleteResult == null) { yield return null; }
			Assert.That(!storeDeleteResult.IsError);

			if (TestVariables.bPublishedStoreIsExist)
			{
				Result<TestHelper.StoreInfoModel> cloneResult = null;
				testHelper.CloneStore(accessToken, TestVariables.ArchiveOriStoreId, TestVariables.publishedStoreId, result => { cloneResult = result; });
				while (cloneResult == null) { yield return null; }
				Assert.That(!cloneResult.IsError);

				storeDeleteResult = null;
				testHelper.DeleteStore(accessToken, TestVariables.ArchiveOriStoreId, result => { storeDeleteResult = result; });
				while (storeDeleteResult == null) { yield return null; }
				Assert.That(!storeDeleteResult.IsError);
			}
			else
			{
				storeDeleteResult = null;
				testHelper.DeletePublishedStore(accessToken, result => { storeDeleteResult = result; });
				while (storeDeleteResult == null) { yield return null; }
				Assert.That(!storeDeleteResult.IsError);
			}

			Result<TestHelper.CurrencyInfoModel> deleteCurrencyResult = null;
			testHelper.DeleteCurrency(accessToken, TestVariables.currencyCode, result => { deleteCurrencyResult = result; });
			while (deleteCurrencyResult == null) { yield return null; }
			Assert.That(!deleteCurrencyResult.IsError);
		}
	}
}
