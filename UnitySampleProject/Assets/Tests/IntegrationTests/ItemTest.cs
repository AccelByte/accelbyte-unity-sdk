// Copyright (c) 2018-2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Threading;
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
        public class GetItem
        {
            string expectedItemId = "";
            string expectedCategoryName = "";

            [UnityTest]
            public IEnumerator A_SetUp_ExpectedEcommerceStuff()
            {
                Items items = AccelBytePlugin.GetItems();
                Categories categories = AccelBytePlugin.GetCategories();
                Result<PagedItems> getItemResult = null;
                Result<Category[]> getChildCategoryResult = null;
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    CategoryPath = TestVariables.childCategoryPath, ItemStatus = ItemStatus.Active
                };

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result =>
                    {
                        getItemResult = result;
                        this.expectedItemId = result.Value.data[0].itemId;
                    });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                categories.GetChildCategories(
                    TestVariables.childCategoryPath,
                    TestVariables.language,
                    result => { getChildCategoryResult = result; });

                while (getChildCategoryResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Assert.That(!getItemResult.IsError);
                Assert.That(!getChildCategoryResult.IsError);
            }

            [UnityTest]
            public IEnumerator GetItem_ItemValid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<Item> getItemResult = null;

                items.GetItemById(
                    this.expectedItemId,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemResult = result; });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemResult.IsError, "Get item failed.");
                TestHelper.Assert.IsTrue(
                        getItemResult.Value.categoryPath.Contains(this.expectedCategoryName),
                        "Get item failed.");
            }

            [UnityTest]
            public IEnumerator GetItem_ItemIdInvalid_ItemNotFound()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<Item> getItemResult = null;
                const string invalidItemId = "000000000";

                items.GetItemById(
                    invalidItemId,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemResult = result; });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getItemResult.IsError, "Request error on get item failed.");
                TestHelper.Assert.IsTrue(
                        getItemResult.Error.Code.Equals(ErrorCode.ItemNotFound),
                        "Request error on get item failed.");
            }

            [UnityTest]
            public IEnumerator GetItem_ItemIdEmpty_ItemNotFound()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<Item> getItemResult = null;

                items.GetItemById(
                    "",
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemResult = result; });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getItemResult.IsError, "Request error on get item failed.");
                TestHelper.Assert.IsTrue(
                        getItemResult.Error.Code.Equals(ErrorCode.NotFound),
                        "Request error on get get item failed.");
            }

            [UnityTest]
            public IEnumerator GetItem_ItemRegionInvalid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<Item> getItemResult = null;
                const string invalidItemRegion = "ID";

                items.GetItemById(
                    this.expectedItemId,
                    invalidItemRegion,
                    TestVariables.language,
                    result => { getItemResult = result; });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemResult.IsError, "Get item failed with invalid region failed.");
                TestHelper.Assert.That(getItemResult.Value, Is.Not.Null, "Get item failed with invalid region failed.");
            }

            [UnityTest]
            public IEnumerator GetItem_ItemRegionEmpty_Fail()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<Item> getItemResult = null;

                items.GetItemById(
                    this.expectedItemId,
                    "",
                    TestVariables.language,
                    result => { getItemResult = result; });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getItemResult.IsError, "Get item with empty region not failed.");
            }

            [UnityTest]
            public IEnumerator GetItem_ItemLanguageInvalid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<Item> getItemResult = null;
                const string invalidItemLanguage = "id";

                items.GetItemById(
                    this.expectedItemId,
                    TestVariables.region,
                    invalidItemLanguage,
                    result => { getItemResult = result; });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemResult.IsError, "Get item with invalid language failed.");
                TestHelper.Assert.IsTrue(
                        getItemResult.Value.categoryPath.Contains(this.expectedCategoryName),
                        "Get item with invalid language failed.");
            }

            [UnityTest]
            public IEnumerator GetItem_ItemLanguageEmpty_Fail()
            {
                Items items = AccelBytePlugin.GetItems();
                Result<Item> getItemResult = null;

                items.GetItemById(this.expectedItemId, TestVariables.region, "", result => { getItemResult = result; });

                while (getItemResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getItemResult.IsError, "Get item with empty language not failed.");
            }
        }

        [TestFixture]
        public class GetItemByCriteria
        {
            [UnityTest]
            public IEnumerator GetItemByCriteria_CategoryPathValid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {CategoryPath = TestVariables.childCategoryPath};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by valid criteria failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data[0].categoryPath.Contains(TestVariables.childCategoryPath),
                        "Get item by valid criteria failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_CategoryPathUnspecified_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria();
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by valid criteria failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data.Length > 0,
                        "Get item by valid criteria failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_CategoryPathInvalid_SuccessButEmpty()
            {
                Items items = AccelBytePlugin.GetItems();
                const string invalidCategoryPath = "/invalidPath";
                Result<PagedItems> getItemByCriteriaResult = null;
                ItemCriteria itemCriteria = new ItemCriteria {CategoryPath = invalidCategoryPath};

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by invalid category path failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data.Length == 0,
                        "Get item by invalid category path failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_ItemTypeCOINS_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {ItemType = ItemType.Coins};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by item type COINS failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_ItemTypeINGAMEITEM_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {ItemType = ItemType.InGameItem};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by item type INGAMEITEM failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_ItemTypeBUNDLE_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {ItemType = ItemType.Bundle};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by item type BUNDLE failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_LanguageInvalid_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                const string invalidCategoryLanguage = "id";
                ItemCriteria itemCriteria = new ItemCriteria {CategoryPath = TestVariables.childCategoryPath};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    invalidCategoryLanguage,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by invalid language failed.");
                TestHelper.Assert.IsTrue(
                        getItemByCriteriaResult.Value.data[0].categoryPath.Contains(TestVariables.childCategoryPath),
                        "Get item by invalid language failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_LanguageEmpty_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {CategoryPath = TestVariables.childCategoryPath};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    "",
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getItemByCriteriaResult.IsError, "Get item by empty language not failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_StatusACTIVE_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {ItemStatus = ItemStatus.Active};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null) { yield return null; }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by item status ACTIVE failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_StatusINACTIVE_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {ItemStatus = ItemStatus.Inactive};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null) { yield return null; }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by item status INACTIVE failed.");
            }

            [UnityTest]
            public IEnumerator GetItemByCriteria_StatusDELETED_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria {ItemStatus = ItemStatus.Deleted};
                Result<PagedItems> getItemByCriteriaResult = null;

                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result => { getItemByCriteriaResult = result; });

                while (getItemByCriteriaResult == null) { yield return null; }

                TestHelper.Assert.IsTrue(!getItemByCriteriaResult.IsError, "Get item by item status DELETED failed.");
            }
        }
    }
}