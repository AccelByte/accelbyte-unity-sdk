// Copyright (c) 2018 - 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using System.Threading;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.IntegrationTests
{
    namespace EcommerceTest
    {
        [TestFixture]
        public class Wallet_C_GetWalletByCurrencyCode
        {
            [UnityTest, Order(2)]
            public IEnumerator GetWalletByCurrencyCode_CurrencyValid_Success()
            {
                Wallet wallet = AccelBytePlugin.GetWallet();
                Result<WalletInfo> getWalletInfoResult = null;

                wallet.GetWalletInfoByCurrencyCode(
                    TestVariables.currencyCode,
                    result => { getWalletInfoResult = result; });

                while (getWalletInfoResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getWalletInfoResult.IsError, "Get wallet failed.");
                TestHelper.Assert.IsTrue(getWalletInfoResult.Value.balance > 0, "Wallet balance isn't correct.");
            }

            [UnityTest, Order(2)]
            public IEnumerator GetWalletByCurrencyCode_CurrencyInvalid_Success()
            {
                Wallet wallet = AccelBytePlugin.GetWallet();
                const string invalidCurrencyCode = "INVALID";
                Result<WalletInfo> getWalletInfoResult = null;

                wallet.GetWalletInfoByCurrencyCode(invalidCurrencyCode, result => { getWalletInfoResult = result; });

                while (getWalletInfoResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getWalletInfoResult.IsError, "Get wallet with invalid currency failed.");
            }

            [UnityTest, Order(2)]
            public IEnumerator GetWalletByCurrencyCode_CurrencyEmpty_Success()
            {
                Wallet wallet = AccelBytePlugin.GetWallet();
                Result<WalletInfo> getWalletInfoResult = null;

                wallet.GetWalletInfoByCurrencyCode("", result => { getWalletInfoResult = result; });

                while (getWalletInfoResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getWalletInfoResult.IsError, "Get wallet with empty currency failed.");
            }
        }

        //TODO: Need to re-enable this test if get wallet transactions support single global account
        [TestFixture, Ignore("Get Wallet transactions doesn't support single global account yet")]
        public class Wallet_B_GetWalletTransactions
        {
//			[UnityTest, Order(2)]
//			public IEnumerator GetWalletTransactions_CurrencyValid_UserDidATransaction_Success()
//			{
//				Wallet wallet = AccelBytePlugin.GetWallet();
//				Result<PagedWalletTransactions> getWalletTransactionResult = null;
//				wallet.GetTransactions(TestVariables.currencyCode, 0, 1, result => { getWalletTransactionResult = result; });
//				while (getWalletTransactionResult == null) {                 Thread.Sleep(100);

//            yield return null;
//        }
//
//				TestHelper.Assert.IsTrue(!getWalletTransactionResult.IsError, "Get wallet transaction(s) doesn't failed.");
//				TestHelper.Assert.IsNotNull(getWalletTransactionResult.Value, "Get wallet transaction(s)");
//			}
//			
//			[UnityTest, Order(2)]
//			public IEnumerator GetWalletTransactions_CurrencyInvalid_UserDidAnotherCurrencyTransaction_Success()
//			{
//				Wallet wallet = AccelBytePlugin.GetWallet();
//				Result<PagedWalletTransactions> getWalletTransactionResult = null;
//				wallet.GetTransactions("IDR", 0, 1, result => { getWalletTransactionResult = result; });
//				while (getWalletTransactionResult == null) {                 Thread.Sleep(100);

//        yield return null;
//    }
//
//				TestHelper.Assert.That(!getWalletTransactionResult.IsError, "Get wallet transaction(s) with invalid currency failed.");
//			}
//			
//			[UnityTest, Order(2)]
//			public IEnumerator GetWalletTransactions_CurrencyEmpty_UserDidATransaction_Failed()
//			{
//				Wallet wallet = AccelBytePlugin.GetWallet();
//				Result<PagedWalletTransactions> getWalletTransactionResult = null;
//				wallet.GetTransactions("", 0, 1, result => { getWalletTransactionResult = result; });
//				while (getWalletTransactionResult == null) {                 Thread.Sleep(100);

//    yield return null;
//}

//				TestHelper.Assert.That(getWalletTransactionResult.IsError, "Get wallet transaction(s) with empty currency not failed.");
//			}
        }

        [TestFixture]
        public class Wallet_A_CreateOrder
        {
            [UnityTest, Order(2)]
            public IEnumerator CreateOrder_OrderValid_WalletCreatedFree_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    CategoryPath = TestVariables.childCategoryPath, ItemStatus = ItemStatus.Active, SortBy="createdAt:desc"
                };
                Result<PagedItems> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result =>
                    {
                        getItemsByCriteria = result;
                    });

                while (getItemsByCriteria == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Orders orders = AccelBytePlugin.GetOrders();
                OrderRequest orderRequest = new OrderRequest
                {
                    currencyCode = getItemsByCriteria.Value.data[0].regionData[0].currencyCode,
                    discountedPrice = getItemsByCriteria.Value.data[0].regionData[0].discountedPrice,
                    itemId = getItemsByCriteria.Value.data[0].itemId,
                    price = getItemsByCriteria.Value.data[0].regionData[0].price,
                    quantity = 1,
                    returnUrl = "https://www.example.com",
                    region = TestVariables.region
                };
                Result<OrderInfo> createOrderResult = null;
                orders.CreateOrder(orderRequest, result => { createOrderResult = result; });

                while (createOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!createOrderResult.IsError, "Create order failed.");
            }

            [UnityTest, Order(3)]
            public IEnumerator CreateOrder_OrderValid_InGameItem_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    CategoryPath = TestVariables.rootCategoryPath, ItemStatus = ItemStatus.Active,
                    SortBy = "createdAt:desc"
                };
                Result<PagedItems> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result =>
                    {
                        getItemsByCriteria = result;
                    });

                while (getItemsByCriteria == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                int quantity = 1;
                Orders orders = AccelBytePlugin.GetOrders();
                OrderRequest orderRequest = new OrderRequest
                {
                    currencyCode = getItemsByCriteria.Value.data[0].regionData[0].currencyCode,
                    discountedPrice = getItemsByCriteria.Value.data[0].regionData[0].discountedPrice * quantity,
                    itemId = getItemsByCriteria.Value.data[0].itemId,
                    price = getItemsByCriteria.Value.data[0].regionData[0].price * quantity,
                    quantity = quantity,
                    returnUrl = "https://www.example.com"
                };
                Result<OrderInfo> createOrderResult = null;
                orders.CreateOrder(orderRequest, result => { createOrderResult = result; });

                while (createOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!createOrderResult.IsError, "Create order failed.");
            }

            [UnityTest, Order(4)]
            public IEnumerator CreateOrder_PriceMismatch_Error()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    CategoryPath = TestVariables.childCategoryPath, ItemStatus = ItemStatus.Active,
                    SortBy = "createdAt:desc"
                };
                Result<PagedItems> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result =>
                    {
                        getItemsByCriteria = result;
                    });

                while (getItemsByCriteria == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                int quantity = 1;
                Orders orders = AccelBytePlugin.GetOrders();
                OrderRequest orderRequest = new OrderRequest
                {
                    currencyCode = getItemsByCriteria.Value.data[0].regionData[0].currencyCode,
                    discountedPrice = getItemsByCriteria.Value.data[0].regionData[0].discountedPrice * quantity,
                    itemId = getItemsByCriteria.Value.data[0].itemId,
                    price = 123456789,
                    quantity = quantity,
                    returnUrl = "https://www.example.com"
                };
                Result<OrderInfo> createOrderResult = null;
                orders.CreateOrder(orderRequest, result => { createOrderResult = result; });

                while (createOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(createOrderResult.IsError, "Create order with mismatch price failed.");
            }

            [UnityTest, Order(4)]
            public IEnumerator CreateOrder_MismatchCurrencyCode_Error()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    CategoryPath = TestVariables.childCategoryPath, ItemStatus = ItemStatus.Active,
                    SortBy = "createdAt:desc"
                };
                Result<PagedItems> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result =>
                    {
                        getItemsByCriteria = result;
                    });

                while (getItemsByCriteria == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                int quantity = 1;
                Orders orders = AccelBytePlugin.GetOrders();
                OrderRequest orderRequest = new OrderRequest
                {
                    currencyCode = "IDR",
                    discountedPrice = getItemsByCriteria.Value.data[0].regionData[0].discountedPrice * quantity,
                    itemId = getItemsByCriteria.Value.data[0].itemId,
                    price = getItemsByCriteria.Value.data[0].regionData[0].discountedPrice * quantity,
                    quantity = quantity,
                    returnUrl = "https://www.example.com"
                };
                Result<OrderInfo> createOrderResult = null;
                orders.CreateOrder(orderRequest, result => { createOrderResult = result; });

                while (createOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(createOrderResult.IsError, "Create order with mismatch currency code failed.");
            }

            [UnityTest, Order(4)]
            public IEnumerator CreateOrder_FictionalItem_Error()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                OrderRequest orderRequest;
                Result<OrderInfo> createOrderResult = null;
                orderRequest = new OrderRequest
                {
                    currencyCode = "JPY",
                    discountedPrice = 5,
                    itemId = "abcde12345",
                    price = 5,
                    quantity = 1,
                    returnUrl = "https://www.example.com"
                };
                orders.CreateOrder(orderRequest, result => { createOrderResult = result; });

                while (createOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(createOrderResult.IsError, "Create an order with a non-existing item failed.");
            }
        }

        [TestFixture]
        public class Wallet_D_GetUserOrder
        {
            [UnityTest, Order(2)]
            public IEnumerator GetUserOrder_OrderExists_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    CategoryPath = TestVariables.childCategoryPath, ItemStatus = ItemStatus.Active,
                    SortBy = "createdAt:desc"
                };
                Result<PagedItems> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result =>
                    {
                        getItemsByCriteria = result;
                    });

                while (getItemsByCriteria == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                int quantity = 1;
                Orders orders = AccelBytePlugin.GetOrders();
                OrderRequest orderRequest = new OrderRequest
                {
                    currencyCode = getItemsByCriteria.Value.data[0].regionData[0].currencyCode,
                    discountedPrice = getItemsByCriteria.Value.data[0].regionData[0].discountedPrice * quantity,
                    itemId = getItemsByCriteria.Value.data[0].itemId,
                    price = getItemsByCriteria.Value.data[0].regionData[0].price * quantity,
                    quantity = quantity,
                    returnUrl = "https://www.example.com"
                };
                Result<OrderInfo> createOrderResult = null;
                orders.CreateOrder(orderRequest, result => { createOrderResult = result; });

                while (createOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Result<OrderInfo> getUserOrderResult = null;
                orders.GetUserOrder(createOrderResult.Value.orderNo, result => { getUserOrderResult = result; });

                while (getUserOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getUserOrderResult.IsError, "Get user order failed.");
            }

            [UnityTest, Order(2)]
            public IEnumerator GetUserOrder_OrderDoesNotExist_Failed()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                const string orderNumberDoesNotExist = "15550000";
                Result<OrderInfo> getUserOrderResult = null;
                orders.GetUserOrder(orderNumberDoesNotExist, result => { getUserOrderResult = result; });

                while (getUserOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getUserOrderResult.IsError, "Get user's invalid order does not failed.");
            }
        }

        [TestFixture]
        public class Wallet_E_GetUserOrders
        {
            [UnityTest, Order(2)]
            public IEnumerator GetUserOrders_UserHasOrderHistory_Success()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                Result<PagedOrderInfo> getUserOrdersResult = null;
                orders.GetUserOrders(0, 3, result => { getUserOrdersResult = result; });

                while (getUserOrdersResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getUserOrdersResult.IsError, "Get user orders failed.");
            }
        }

        [TestFixture]
        public class Wallet_F_GetUserOrderHistory
        {
            [UnityTest, Order(2)]
            public IEnumerator GetUserOrderHistory_UserHasOrderHistory_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    CategoryPath = TestVariables.childCategoryPath, ItemStatus = ItemStatus.Active,
                    SortBy = "createdAt:desc"
                };
                Result<PagedItems> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
                    TestVariables.region,
                    TestVariables.language,
                    result =>
                    {
                        getItemsByCriteria = result;
                    });

                while (getItemsByCriteria == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                int quantity = 1;
                Orders orders = AccelBytePlugin.GetOrders();
                OrderRequest orderRequest = new OrderRequest
                {
                    currencyCode = getItemsByCriteria.Value.data[0].regionData[0].currencyCode,
                    discountedPrice = getItemsByCriteria.Value.data[0].regionData[0].discountedPrice * quantity,
                    itemId = getItemsByCriteria.Value.data[0].itemId,
                    price = getItemsByCriteria.Value.data[0].regionData[0].price * quantity,
                    quantity = quantity,
                    returnUrl = "https://www.example.com"
                };
                Result<OrderInfo> createOrderResult = null;
                orders.CreateOrder(orderRequest, result => { createOrderResult = result; });

                while (createOrderResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                Result<OrderHistoryInfo[]> getUserOrderHistoryResult = null;
                orders.GetUserOrderHistory(
                    createOrderResult.Value.orderNo,
                    result => { getUserOrderHistoryResult = result; });

                while (getUserOrderHistoryResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getUserOrderHistoryResult.IsError, "Get user order history failed.");
                TestHelper.Assert.That(getUserOrderHistoryResult.Value, Is.Not.Null);
            }

            [UnityTest, Order(2)]
            public IEnumerator GetUserOrderHistory_OrderNumberDoesNotExist_Success()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                const string orderNumberDoesNotExist = "1555";
                Result<OrderHistoryInfo[]> getUserOrderHistoryResult = null;
                orders.GetUserOrderHistory(orderNumberDoesNotExist, result => { getUserOrderHistoryResult = result; });

                while (getUserOrderHistoryResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getUserOrderHistoryResult.IsError, "Get user order history failed.");
            }
        }

        [TestFixture]
        public class EntitlementTest
        {
            [UnityTest, Order(2)]
            public IEnumerator GetUserEntitlement_Success()
            {
                Entitlements entitlements = AccelBytePlugin.GetEntitlements();
                Result<PagedEntitlements> getPagedEntitlementsResult = null;
                entitlements.GetUserEntitlements(0, 20, result => { getPagedEntitlementsResult = result; });

                while (getPagedEntitlementsResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getPagedEntitlementsResult.IsError, "Get user entitlements failed.");
            }
        }
    }
}