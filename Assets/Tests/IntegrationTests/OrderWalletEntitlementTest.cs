// Copyright (c) 2018 - 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
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
        [TestFixture]
        public class OrderWalletEntitlementTest
        {
            [UnityTest, Order(0)]
            public IEnumerator CreateOrder_OrderValid_WalletCreatedFree_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    sortBy = "createdAt:desc"
                };
                Result<ItemPagingSlicedResult> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
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

            [UnityTest, Order(1)]
            public IEnumerator CreateOrder_OrderValid_InGameItem_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedRootCategoryPath,
                    sortBy = "createdAt:desc"
                };
                Result<ItemPagingSlicedResult> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
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

            [UnityTest, Order(2)]
            public IEnumerator CreateOrder_PriceMismatch_Error()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    sortBy = "createdAt:desc"
                };
                Result<ItemPagingSlicedResult> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
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

            [UnityTest, Order(2)]
            public IEnumerator CreateOrder_MismatchCurrencyCode_Error()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    sortBy = "createdAt:desc"
                };
                Result<ItemPagingSlicedResult> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
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

            [UnityTest, Order(2)]
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

            int walletBalance = 0;

            [UnityTest, Order(3)]
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

                walletBalance = getWalletInfoResult.Value.balance;
            }

            [UnityTest, Order(3)]
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

            [UnityTest, Order(3)]
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

            [UnityTest, Order(4)]
            public IEnumerator CreditUserWallet_Success()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();
                CreditUserWalletRequest creditUserWalletRequest = new CreditUserWalletRequest
                {
                    amount = 1000,
                    source = CreditUserWalletSource.PURCHASE,
                    reason = "Unity GameServer Ecommerce CreditUserWallet test."
                };
                int expectedBalance = walletBalance + creditUserWalletRequest.amount;
                Result<WalletInfo> walletInfo = null;
                serverEcommerce.CreditUserWallet(TestVariables.userId, TestVariables.currencyCode, creditUserWalletRequest, result => { walletInfo = result; });

                while (walletInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!walletInfo.IsError, "Credit user wallet failed.");
                TestHelper.Assert.IsTrue(walletInfo.Value.balance == expectedBalance, "Balance is not increased.");
            }

            [UnityTest, Order(5)]
            public IEnumerator GetUserOrder_OrderExists_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    sortBy = "createdAt:desc"
                };
                Result<ItemPagingSlicedResult> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
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

            [UnityTest, Order(5)]
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

            [UnityTest, Order(6)]
            public IEnumerator GetUserOrders_UserHasOrderHistory_Success()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                Result<OrderPagingSlicedResult> getUserOrdersResult = null;
                orders.GetUserOrders(0, 3, result => { getUserOrdersResult = result; });

                while (getUserOrdersResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getUserOrdersResult.IsError, "Get user orders failed.");
            }

            //TODO: Need to re-enable this test if get wallet transactions support single global account
            //        [TestFixture, Ignore("Get Wallet transactions doesn't support single global account yet")]
            //        public class E2_GetWalletTransactions
            //        {
            //			[UnityTest, Order(2)]
            //			public IEnumerator GetWalletTransactions_CurrencyValid_UserDidATransaction_Success()
            //			{
            //				Wallet wallet = AccelBytePlugin.GetWallet();
            //				Result<WalletTransactionPagingSlicedResult> getWalletTransactionResult = null;
            //				wallet.GetTransactions(TestVariables.currencyCode, 0, 1, result => { getWalletTransactionResult = result; });
            //				while (getWalletTransactionResult == null)
            //              {
            //                  Thread.Sleep(100);

            //                  yield return null;
            //              }
            //
            //				TestHelper.Assert.IsTrue(!getWalletTransactionResult.IsError, "Get wallet transaction(s) doesn't failed.");
            //				TestHelper.Assert.IsNotNull(getWalletTransactionResult.Value, "Get wallet transaction(s)");
            //			}
            //			
            //			[UnityTest, Order(2)]
            //			public IEnumerator GetWalletTransactions_CurrencyInvalid_UserDidAnotherCurrencyTransaction_Success()
            //			{
            //				Wallet wallet = AccelBytePlugin.GetWallet();
            //				Result<WalletTransactionPagingSlicedResult> getWalletTransactionResult = null;
            //				wallet.GetTransactions("IDR", 0, 1, result => { getWalletTransactionResult = result; });
            //				while (getWalletTransactionResult == null)
            //              {
            //                  Thread.Sleep(100);

            //                  yield return null;
            //              }
            //
            //				TestHelper.Assert.That(!getWalletTransactionResult.IsError, "Get wallet transaction(s) with invalid currency failed.");
            //			}
            //			
            //			[UnityTest, Order(2)]
            //			public IEnumerator GetWalletTransactions_CurrencyEmpty_UserDidATransaction_Failed()
            //			{
            //				Wallet wallet = AccelBytePlugin.GetWallet();
            //				Result<WalletTransactionPagingSlicedResult> getWalletTransactionResult = null;
            //				wallet.GetTransactions("", 0, 1, result => { getWalletTransactionResult = result; });
            //				while (getWalletTransactionResult == null)
            //              {
            //                  Thread.Sleep(100);

            //                  yield return null;
            //              }

            //				TestHelper.Assert.That(getWalletTransactionResult.IsError, "Get wallet transaction(s) with empty currency not failed.");
            //			}
            //        }

            [UnityTest, Order(7)]
            public IEnumerator GetUserOrderHistory_UserHasOrderHistory_Success()
            {
                Items items = AccelBytePlugin.GetItems();
                ItemCriteria itemCriteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedChildCategoryPath,
                    sortBy = "createdAt:desc"
                };
                Result<ItemPagingSlicedResult> getItemsByCriteria = null;
                items.GetItemsByCriteria(
                    itemCriteria,
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

            [UnityTest, Order(7)]
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

            [UnityTest, Order(8)]
            public IEnumerator GrantUserEntitlement_Success()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlements();
                Result<EntitlementPagingSlicedResult> getEmptyPagedEntitlementsResult = null;
                entitlements.QueryUserEntitlements("", TestVariables.inGameItem.itemId, 0, 20, result => { getEmptyPagedEntitlementsResult = result; });

                while (getEmptyPagedEntitlementsResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getEmptyPagedEntitlementsResult.IsError, "Get user entitlements failed.");

                GrantUserEntitlementRequest grantUserEntitlementRequest = new GrantUserEntitlementRequest
                {
                    itemId = TestVariables.inGameItem.itemId,
                    itemNamespace = TestVariables.inGameItem.Namespace,
                    grantedCode = "123456789",
                    quantity = 1,
                    source = EntitlementSource.ACHIEVEMENT,
                    region = TestVariables.region,
                    language = TestVariables.language
                };

                GrantUserEntitlementRequest[] grantUserEntitlementsRequest = { grantUserEntitlementRequest };

                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                Result<StackableEntitlementInfo[]> stackableEntitlementInfo = null;
                serverEcommerce.GrantUserEntitlement(TestVariables.userId, grantUserEntitlementsRequest, result => { stackableEntitlementInfo = result; });

                while (stackableEntitlementInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!stackableEntitlementInfo.IsError, "Grant user entitlement failed.");
                TestHelper.Assert.IsTrue(stackableEntitlementInfo.Value[0].itemId == TestVariables.inGameItem.itemId, "Item is not found.");

                Result<EntitlementPagingSlicedResult> getPagedEntitlementsResult = null;
                entitlements.QueryUserEntitlements("", TestVariables.inGameItem.itemId, 0, 20, result => { getPagedEntitlementsResult = result; });

                while (getPagedEntitlementsResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                bool grantedEntitlementFound = false;
                for (int i = 0; i < getPagedEntitlementsResult.Value.data.Length; i++)
                {
                    if (getPagedEntitlementsResult.Value.data[i].grantedCode == grantUserEntitlementRequest.grantedCode)
                    {
                        grantedEntitlementFound = true;
                    }
                }

                TestHelper.Assert.IsTrue(!getPagedEntitlementsResult.IsError, "Get user entitlements failed.");
                TestHelper.Assert.IsTrue(grantedEntitlementFound, "Granted Item is not found.");
            }

            string expectedEntitlementId = "";

            [UnityTest, Order(9)]
            public IEnumerator QueryUserEntitlement_Success()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlements();
                Result<EntitlementPagingSlicedResult> getPagedEntitlementsResult = null;
                entitlements.QueryUserEntitlements("", "", 0, 20, result => { getPagedEntitlementsResult = result; });

                while (getPagedEntitlementsResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getPagedEntitlementsResult.IsError, "Get user entitlements failed.");
                expectedEntitlementId = getPagedEntitlementsResult.Value.data[0].id;
            }

            [UnityTest, Order(10)]
            public IEnumerator ConsumeUserEntitlementNegative_Failed()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlements();

                Result<EntitlementInfo> getEntitlementInfoResult = null;
                entitlements.ConsumeUserEntitlement(expectedEntitlementId, -1, result => { getEntitlementInfoResult = result; });

                while (getEntitlementInfoResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getEntitlementInfoResult.IsError, "Consume user entitlement success.");
            }

            [UnityTest, Order(10)]
            public IEnumerator ConsumeUserEntitlementZero_Failed()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlements();

                Result<EntitlementInfo> getEntitlementInfoResult = null;
                entitlements.ConsumeUserEntitlement(expectedEntitlementId, 0, result => { getEntitlementInfoResult = result; });

                while (getEntitlementInfoResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(getEntitlementInfoResult.IsError, "Consume user entitlement success.");
            }

            [UnityTest, Order(11)]
            public IEnumerator ConsumeUserEntitlement_Success()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlements();

                Result<EntitlementInfo> getEntitlementInfoResult = null;
                entitlements.ConsumeUserEntitlement(expectedEntitlementId, 1, result => { getEntitlementInfoResult = result; });

                while (getEntitlementInfoResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                bool bConsumeUserEntitlement = (getEntitlementInfoResult.Value.status == EntitlementStatus.CONSUMED);

                TestHelper.Assert.IsTrue(!getEntitlementInfoResult.IsError, "Consume user entitlement failed.");
                TestHelper.Assert.IsTrue(bConsumeUserEntitlement, "Consume user entitlement failed.");
            }

            [UnityTest, Order(11)]
            public IEnumerator GetUserEntitlementById_Success()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlements();

                Result<EntitlementInfo> entitlementInfo = null;
                entitlements.GetUserEntitlementById(expectedEntitlementId, result => { entitlementInfo = result; });

                while (entitlementInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!entitlementInfo.IsError, "Get user entitlement by id failed.");
                TestHelper.Assert.IsTrue(entitlementInfo.Value.id == expectedEntitlementId, "Expected entitlement is not found.");
            }

            [UnityTest, Order(11)]
            public IEnumerator GetUserEntitlementById_Failed()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlements();

                Result<EntitlementInfo> entitlementInfo = null;
                entitlements.GetUserEntitlementById("Invalid", result => { entitlementInfo = result; });

                while (entitlementInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(entitlementInfo.IsError, "Get user entitlement by invalid id is success.");
            }

            [UnityTest, Order(11)]
            public IEnumerator ServerGetUserEntitlementById_Success()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                Result<EntitlementInfo> entitlementInfo = null;
                serverEcommerce.GetUserEntitlementById(expectedEntitlementId, result => { entitlementInfo = result; });

                while (entitlementInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!entitlementInfo.IsError, "Server get user entitlement by id failed.");
                TestHelper.Assert.IsTrue(entitlementInfo.Value.id == expectedEntitlementId, "Expected entitlement is not found.");
            }

            [UnityTest, Order(11)]
            public IEnumerator ServerGetUserEntitlementById_Failed()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                Result<EntitlementInfo> entitlementInfo = null;
                serverEcommerce.GetUserEntitlementById("Invalid", result => { entitlementInfo = result; });

                while (entitlementInfo == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(entitlementInfo.IsError, "Server get user entitlement by invalid id is success.");
            }
        }
    }
}
