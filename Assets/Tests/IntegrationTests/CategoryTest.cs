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
        public class CategoryTest
        {
            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetCategory_CategoryValid_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo> getCategoryResult = null;

                categories.GetCategory(
                    TestVariables.expectedRootCategoryPath,
                    TestVariables.language,
                    result => { getCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getCategoryResult);

                TestHelper.Assert.IsResultOk(getCategoryResult, "Get category failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetCategory_CategoryInvalid_NotFound()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string invalidCategoryPath = "/invalidPath";
                Result<CategoryInfo> getCategoryResult = null;

                categories.GetCategory(
                    invalidCategoryPath,
                    TestVariables.language,
                    result => { getCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getCategoryResult);

                TestHelper.Assert.IsTrue(getCategoryResult.IsError, "Get invalid category failed.");
                TestHelper.Assert.IsTrue(
                        getCategoryResult.Error.Code.Equals(ErrorCode.CategoryNotFound),
                        "Get invalid category failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetCategory_CategoryEmpty_InvalidRequest()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string invalidCategoryPath = "invalid";
                Result<CategoryInfo> getCategoryResult = null;

                categories.GetCategory(
                    invalidCategoryPath,
                    TestVariables.language,
                    result => { getCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getCategoryResult);

                TestHelper.Assert.IsTrue(
                        !getCategoryResult.Error.Code.Equals(ErrorCode.BadRequest),
                        "Get invalid category failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetCategory_LanguageInvalid_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string invalidLanguage = "english";
                Result<CategoryInfo> getCategoryResult = null;

                categories.GetCategory(
                    TestVariables.expectedRootCategoryPath,
                    invalidLanguage,
                    result => { getCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getCategoryResult);

                TestHelper.Assert.IsResultOk(getCategoryResult, "Get category with invalid language failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetCategory_LanguageEmpty_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo> getCategoryResult = null;

                categories.GetCategory(TestVariables.expectedRootCategoryPath, "", result => { getCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getCategoryResult);

                TestHelper.Assert.IsResultOk(
                        getCategoryResult,
                        "Get category with empty language should list the default language.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetRootCategory_LanguageValid_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo[]> getRootCategoryResult = null;

                categories.GetRootCategories(
                    TestVariables.language,
                    result => { getRootCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getRootCategoryResult);

                TestHelper.Assert.IsResultOk(getRootCategoryResult, "Get root category failed.");
                TestHelper.Assert.That(getRootCategoryResult.Value, Is.Not.Null, "Get root category return empty.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetRootCategory_LanguageInvalid_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string invalidLanguage = "unknown";
                Result<CategoryInfo[]> getRootCategoryResult = null;

                categories.GetRootCategories(invalidLanguage, result => { getRootCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getRootCategoryResult);

                TestHelper.Assert.IsResultOk(
                        getRootCategoryResult,
                        "Get root category with invalid language failed.");
                TestHelper.Assert.That(
                        getRootCategoryResult.Value,
                        Is.Not.Null,
                        "Get root category with invalid language return empty.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetRootCategory_LanguageEmpty_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string emptyLanguage = "";
                Result<CategoryInfo[]> getRootCategoryResult = null;

                categories.GetRootCategories(emptyLanguage, result => { getRootCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getRootCategoryResult);

                TestHelper.Assert.IsResultOk(
                        getRootCategoryResult,
                        "Get root category with empty language failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetChildCategory_CategoryValid_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo[]> getChildCategoryResult = null;
                bool containDogeCoin = false;

                categories.GetChildCategories(
                    TestVariables.expectedRootCategoryPath,
                    TestVariables.language,
                    result => { getChildCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getChildCategoryResult);

                foreach (CategoryInfo child in getChildCategoryResult.Value)
                {
                    if (child.categoryPath.Contains(TestVariables.expectedChildCategoryPath))
                    {
                        containDogeCoin = true;
                    }
                }

                TestHelper.Assert.IsResultOk(getChildCategoryResult, "Get child category failed.");
                TestHelper.Assert.IsTrue(containDogeCoin, "Get child category failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetChildCategory_CategoryInvalid_ReturnAnEmptyArray()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string invalidCategoryPath = "/invalidPath";
                Result<CategoryInfo[]> getChildCategoryResult = null;

                categories.GetChildCategories(
                    invalidCategoryPath,
                    TestVariables.language,
                    result => { getChildCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getChildCategoryResult);

                TestHelper.Assert.IsResultOk(
                        getChildCategoryResult,
                        "Get child category with invalid path not return an empty array.");
                TestHelper.Assert.IsTrue(
                        getChildCategoryResult.Value.Length == 0,
                        "Get child category with invalid path not return an empty array.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetChildCategory_CategoryValid_LanguageInvalid_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string invalidLanguage = "unknown";
                Result<CategoryInfo[]> getChildCategoryResult = null;
                bool containDogeCoin = false;

                categories.GetChildCategories(
                    TestVariables.expectedRootCategoryPath,
                    invalidLanguage,
                    result => { getChildCategoryResult = result; });
                
                yield return TestHelper.WaitForValue(() => getChildCategoryResult);

                foreach (CategoryInfo child in getChildCategoryResult.Value)
                {
                    if (child.categoryPath.Contains(TestVariables.expectedChildCategoryPath))
                    {
                        containDogeCoin = true;
                    }
                }

                TestHelper.Assert.IsResultOk(
                        getChildCategoryResult,
                        "Get child category with invalid language failed.");
                TestHelper.Assert.IsTrue(containDogeCoin, "Get child category with invalid language failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetChildCategory_CategoryValid_LanguageEmpty_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                const string emptyLanguage = "";
                Result<CategoryInfo[]> getChildCategoryResult = null;

                categories.GetChildCategories(
                    TestVariables.expectedRootCategoryPath,
                    emptyLanguage,
                    result => { getChildCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getChildCategoryResult);

                TestHelper.Assert.IsResultOk(
                        getChildCategoryResult,
                        "Get child category with empty language failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetDescendantCategory_CategoryValid_Success()
            {
                var user = AccelBytePlugin.GetUser();
                Result loginResult = null;
                user.LoginWithDeviceId(result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo[]> getDescendantCategoryResult = null;
                bool containDogeCoin = false;

                categories.GetDescendantCategories(
                    TestVariables.expectedRootCategoryPath,
                    TestVariables.language,
                    result => { getDescendantCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getDescendantCategoryResult);

                foreach (CategoryInfo child in getDescendantCategoryResult.Value)
                {
                    if (child.categoryPath.Contains(TestVariables.expectedGrandChildCategoryPath))
                    {
                        containDogeCoin = true;
                    }
                }

                TestHelper.Assert.IsResultOk(getDescendantCategoryResult, "Get descendant category failed.");
                TestHelper.Assert.IsTrue(containDogeCoin, "Get descendant category failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetDescendantCategory_CategoryInvalid_ReturnAnEmptyArray()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo[]> getDescendantCategoryResult = null;
                const string invalidCategory = "invalid";

                categories.GetDescendantCategories(
                    invalidCategory,
                    TestVariables.language,
                    result => { getDescendantCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getDescendantCategoryResult);

                TestHelper.Assert.IsResultOk(
                        getDescendantCategoryResult,
                        "Get descendant category with invalid category failed.");
                TestHelper.Assert.IsTrue(
                        getDescendantCategoryResult.Value.Length == 0,
                        "Get descendant category with invalid category not return an empty array.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetDescendantCategory_CategoryValid_LanguageInvalid_Success()
            {
                var user = AccelBytePlugin.GetUser();
                Result loginResult = null;
                user.LoginWithDeviceId(result => loginResult = result);
                yield return TestHelper.WaitForValue(() => loginResult);

                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo[]> getDescendantCategoryResult = null;
                bool containDogeCoin = false;

                categories.GetDescendantCategories(
                    TestVariables.expectedRootCategoryPath,
                    "unknown",
                    result => { getDescendantCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getDescendantCategoryResult);

                foreach (CategoryInfo child in getDescendantCategoryResult.Value)
                {
                    if (child.categoryPath.Contains(TestVariables.expectedGrandChildCategoryPath))
                    {
                        containDogeCoin = true;
                    }
                }

                TestHelper.Assert.IsResultOk(
                        getDescendantCategoryResult,
                        "Get descendant category with invalid language failed.");
                TestHelper.Assert.IsTrue(containDogeCoin, "Get descendant category with invalid language failed.");
            }

            [UnityTest, TestLog, Order(0)]
            public IEnumerator GetDescendantCategory_CategoryValid_LanguageEmpty_Success()
            {
                Categories categories = AccelBytePlugin.GetCategories();
                Result<CategoryInfo[]> getDescendantCategoryResult = null;

                categories.GetDescendantCategories(
                    TestVariables.expectedRootCategoryPath,
                    "",
                    result => { getDescendantCategoryResult = result; });
                yield return TestHelper.WaitForValue(() => getDescendantCategoryResult);

                TestHelper.Assert.IsResultOk(
                        getDescendantCategoryResult,
                        "Get descendant category with empty language failed.");
            }
        }
    }
}
