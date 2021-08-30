// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    namespace EcommerceTest
    {
        [TestFixture]
        public class ItemTest
        {
            string expectedItemId = "";
            string expectedCategoryName = "";

            [UnityTest, TestLog, Order(0)]
            public IEnumerator SetUp_ExpectedEcommerceStuff()
            {
                Items items = AccelBytePlugin.GetItems();
                Categories categories = AccelBytePlugin.GetCategories();
                Result<ItemPagingSlicedResult> getItemResult = null;
                Result<CategoryInfo[]> getChildCategoryResult = null;
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    region = TestVariables.region,
                    language = TestVariables.language
                };

                items.GetItemsByCriteria(
                    itemCriteria,
                    result =>
                    {
                        getItemResult = result;
                        this.expectedItemId = result.Value.data[0].itemId;
                    });
                yield return TestHelper.WaitForValue(() => getItemResult);

                categories.GetChildCategories(
                    TestVariables.expectedChildCategoryPath,
                    TestVariables.language,
                    result => { getChildCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getChildCategoryResult);

                TestHelper.Assert.IsResultOk(getItemResult);
                TestHelper.Assert.IsResultOk(getChildCategoryResult);
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItem_ItemValid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<PopulatedItemInfo> getItemResult = null;

                items.GetItemById(
                    this.expectedItemId,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemResult = result; });
                yield return TestHelper.WaitForValue(() => getItemResult);

                TestHelper.Assert.IsResultOk(getItemResult, "Get item failed.");
                TestHelper.Assert.IsTrue(
                        getItemResult.Value.categoryPath.Contains(this.expectedCategoryName),
                        "Get item failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItem_ItemIdInvalid_ItemNotFound()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<PopulatedItemInfo> getItemResult = null;
                const string invalidItemId = "000000000";

                items.GetItemById(
                    invalidItemId,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemResult = result; });
                yield return TestHelper.WaitForValue(() => getItemResult);

                TestHelper.Assert.IsTrue(getItemResult.IsError, "Request error on get item failed.");
                TestHelper.Assert.IsTrue(
                        getItemResult.Error.Code.Equals(ErrorCode.ItemNotFound),
                        "Request error on get item failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItem_ItemRegionInvalid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<PopulatedItemInfo> getItemResult = null;
                const string invalidItemRegion = "ID";

                items.GetItemById(
                    this.expectedItemId,
                    invalidItemRegion,
                    TestVariables.language,
                    result => { getItemResult = result; });
                yield return TestHelper.WaitForValue(() => getItemResult);

                TestHelper.Assert.IsResultOk(getItemResult, "Get item failed with invalid region failed.");
                TestHelper.Assert.That(getItemResult.Value, Is.Not.Null, "Get item failed with invalid region failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItem_ItemRegionEmpty_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<PopulatedItemInfo> getItemResult = null;

                items.GetItemById(
                    this.expectedItemId,
                    "",
                    TestVariables.language,
                    result => { getItemResult = result; });
                yield return TestHelper.WaitForValue(() => getItemResult);

                TestHelper.Assert.IsResultOk(getItemResult, "Get item with empty region failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItem_ItemLanguageInvalid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<PopulatedItemInfo> getItemResult = null;
                const string invalidItemLanguage = "id";

                items.GetItemById(
                    this.expectedItemId,
                    TestVariables.region,
                    invalidItemLanguage,
                    result => { getItemResult = result; });
                yield return TestHelper.WaitForValue(() => getItemResult);

                TestHelper.Assert.IsResultOk(getItemResult, "Get item with invalid language failed.");
                TestHelper.Assert.IsTrue(
                        getItemResult.Value.categoryPath.Contains(this.expectedCategoryName),
                        "Get item with invalid language failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItem_ItemLanguageEmpty_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<PopulatedItemInfo> getItemResult = null;

                items.GetItemById(this.expectedItemId, TestVariables.region, "", result => { getItemResult = result; });
                yield return TestHelper.WaitForValue(() => getItemResult);

                TestHelper.Assert.IsResultOk(getItemResult, "Get item with empty language failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_CategoryPathValid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {categoryPath = TestVariables.expectedChildCategoryPath };
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by valid criteria failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data[0].categoryPath.Contains(TestVariables.expectedChildCategoryPath),
                        "Get item by valid criteria failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_CategoryPathUnspecified_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria();
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by valid criteria failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data.Length > 0,
                        "Get item by valid criteria failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_CategoryPathInvalid_SuccessButEmpty()
            {
                Items items = AccelBytePlugin.GetItems();
                const string invalidCategoryPath = "/invalidPath";
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;
                ItemCriteria itemCriteria = new ItemCriteria {categoryPath = invalidCategoryPath};

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by invalid category path failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data.Length == 0,
                        "Get item by invalid category path failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_ItemTypeCOINS_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {itemType = ItemType.COINS};
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by item type COINS failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_ItemTypeINGAMEITEM_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {itemType = ItemType.INGAMEITEM};
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by item type INGAMEITEM failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_ItemTypeBUNDLE_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {itemType = ItemType.BUNDLE};
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by item type BUNDLE failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_LanguageInvalid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                const string invalidCategoryLanguage = "id";
                ItemCriteria itemCriteria = new ItemCriteria {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    language = invalidCategoryLanguage
                };
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by invalid language failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data[0].categoryPath.Contains(TestVariables.expectedChildCategoryPath),
                        "Get item by invalid language failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_LanguageEmpty_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    language = ""
                };
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });
                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by empty language failed.");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_AppTypeGAME_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {appType = EntitlementAppType.GAME};
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });

                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by item appType GAME failed.");
                if(getItemByCriteriaResult.Value.data.Length > 0)
                {
                    TestHelper.Assert.IsTrue(getItemByCriteriaResult.Value.data[0].appType == EntitlementAppType.GAME, "Get item by item appType GAME failed.");
                }
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetItemByCriteria_byTags_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                string[] tags = new string[] { "SDK", "GAME" };
                ItemCriteria itemCriteria = new ItemCriteria { tags = tags };
                Result<ItemPagingSlicedResult> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    result => { getItemByCriteriaResult = result; });

                yield return TestHelper.WaitForValue(() => getItemByCriteriaResult);

                TestHelper.Assert.IsResultOk(getItemByCriteriaResult, "Get item by item by tags failed.");
                if(getItemByCriteriaResult.Value.data.Length > 0)
                {
                    TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data[0].tags[0] == tags[0] ||
                        getItemByCriteriaResult.Value.data[0].tags[0] == tags[1], 
                        "Get item by item by tags failed.");
                }
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetCurrencyList_Success()
            {
                User abUser;
                abUser = AccelBytePlugin.GetUser();
                Result loginUser = null;
                abUser.LoginWithUsername("accelbytetestingadmin@accelbyte.net", "LfUxTobIR6SzqUKo2ULzuNC8vL3rO9Eo", result =>
                {
                    loginUser = result;
                });
                yield return TestHelper.WaitForValue(() => loginUser);

                Currencies abCurrencies;
                abCurrencies = AccelBytePlugin.GetCurrencies();
                Result<CurrencyList[]> currencyListResult = null;
                abCurrencies.GetCurrencyList(result =>
                {
                    currencyListResult = result;
                });
                yield return TestHelper.WaitForValue(() => currencyListResult);

                TestHelper.Assert.IsResultOk(currencyListResult, "Get CurrencyList failed");
                TestHelper.Assert.IsTrue(currencyListResult.Value.Length >= 0, "Get CurrencyList failed");
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetCurrencyList_Failed()
            {
                User abUser;
                abUser = AccelBytePlugin.GetUser();
                Result loginUser = null;
                abUser.LoginWithUsername("accelbytetestingadmin@accelbyte.net", "LfUxTobIR6SzqUKo2ULzuNC8vL3rO9Eo", result =>
                {
                    loginUser = result;
                });
                yield return TestHelper.WaitForValue(() => loginUser);

                Currencies abCurrencies;
                abCurrencies = AccelBytePlugin.GetCurrencies();
                Result<CurrencyList[]> currencyListResult = null;
                abCurrencies.GetCurrencyList(result =>
                {
                    currencyListResult = result;
                }, "Set the Wrong Namespace");//Set the the value of namespace to the Wrong Namespace.
                yield return TestHelper.WaitForValue(() => currencyListResult);

                TestHelper.Assert.IsResultOk(currencyListResult, "Get CurrencyList failed");
                TestHelper.Assert.IsTrue(currencyListResult.Value.Length == 0, "Get CurrencyList failed");
            }
        }
    }
}
