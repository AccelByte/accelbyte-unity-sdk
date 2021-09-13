// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
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
            [UnityTest, TestLog, Order(0)]
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
                yield return TestHelper.WaitForValue(() => getItemsByCriteria);

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
                yield return TestHelper.WaitForValue(() => createOrderResult);

                TestHelper.Assert.IsResultOk(createOrderResult, "Create order failed.");
            }

            [UnityTest, TestLog, Order(1)]
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
                yield return TestHelper.WaitForValue(() => getItemsByCriteria);

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
                yield return TestHelper.WaitForValue(() => createOrderResult);

                TestHelper.Assert.IsResultOk(createOrderResult, "Create order failed.");
            }

            [UnityTest, TestLog, Order(2)]
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
                yield return TestHelper.WaitForValue(() => getItemsByCriteria);

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
                yield return TestHelper.WaitForValue(() => createOrderResult);

                TestHelper.Assert.IsTrue(createOrderResult.IsError, "Create order with mismatch price failed.");
            }

            [UnityTest, TestLog, Order(2)]
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
                yield return TestHelper.WaitForValue(() => getItemsByCriteria);

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
                yield return TestHelper.WaitForValue(() => createOrderResult);

                TestHelper.Assert.IsTrue(createOrderResult.IsError, "Create order with mismatch currency code failed.");
            }

            [UnityTest, TestLog, Order(2)]
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
                yield return TestHelper.WaitForValue(() => createOrderResult);

                TestHelper.Assert.IsTrue(createOrderResult.IsError, "Create an order with a non-existing item failed.");
            }

            [UnityTest, TestLog, Order(3)]
            public IEnumerator CreateOrder_CancelOrder_Success()
            {
                User abUser;
                abUser = AccelBytePlugin.GetUser();
                Result loginUser = null;
                abUser.LoginWithUsername("accelbytetestingadmin@accelbyte.net", "LfUxTobIR6SzqUKo2ULzuNC8vL3rO9Eo", result =>
                {
                    loginUser = result;
                });
                yield return TestHelper.WaitForValue(() => loginUser);

                Items abItemStore;
                abItemStore = AccelBytePlugin.GetItems();
                Result<ItemPagingSlicedResult> getItems = null;
                ItemCriteria criteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedGrandChildCategoryPath,
                    sortBy = "createdAt:desc",
                    itemType = ItemType.COINS
                };
                abItemStore.GetItemsByCriteria(criteria, result =>
                {
                    getItems = result;
                });
                yield return TestHelper.WaitForValue(() => getItems);

                Orders abOrder;
                abOrder = AccelBytePlugin.GetOrders();
                int quantity = 1;
                OrderRequest orderRequest = new OrderRequest
                {
                    itemId = getItems.Value.data[0].itemId,
                    currencyCode = getItems.Value.data[0].regionData[0].currencyCode,
                    discountedPrice = getItems.Value.data[0].regionData[0].discountedPrice,
                    language = getItems.Value.data[0].language,
                    price = getItems.Value.data[0].regionData[0].price,
                    quantity = quantity,
                    region = getItems.Value.data[0].region,
                    returnUrl = "https://www.example.com"
                };

                Result<OrderInfo> orderInfo = null;
                abOrder.CreateOrder(orderRequest, result =>
                {
                    orderInfo = result;
                });
                yield return TestHelper.WaitForValue(() => orderInfo);

                Result<OrderInfo> cancelOrderInfo = null;
                abOrder.CancelOrder(orderInfo.Value.orderNo, result =>
                {
                    cancelOrderInfo = result;
                });
                yield return TestHelper.WaitForValue(() => cancelOrderInfo);

                TestHelper.Assert.IsResultOk(cancelOrderInfo, "Cancel Order Info is Success");
                TestHelper.Assert.IsFalse(cancelOrderInfo.IsError, "Success Cancel Order");
            }

            [UnityTest, TestLog, Order(3)]
            public IEnumerator CreateOrder_CancelOrder_Failed()
            {
                User abUser;
                abUser = AccelBytePlugin.GetUser();
                Result loginUser = null;
                abUser.LoginWithUsername("accelbytetestingadmin@accelbyte.net", "LfUxTobIR6SzqUKo2ULzuNC8vL3rO9Eo", result =>
                {
                    loginUser = result;
                });
                yield return TestHelper.WaitForValue(() => loginUser);

                Items abItemStore;
                abItemStore = AccelBytePlugin.GetItems();
                Result<ItemPagingSlicedResult> getItems = null;
                ItemCriteria criteria = new ItemCriteria
                {
                    categoryPath = TestVariables.expectedGrandChildCategoryPath,
                    sortBy = "createdAt:desc",
                    itemType = ItemType.COINS
                };
                abItemStore.GetItemsByCriteria(criteria, result =>
                {
                    getItems = result;
                });
                yield return TestHelper.WaitForValue(() => getItems);

                Orders abOrder;
                abOrder = AccelBytePlugin.GetOrders();
                int quantity = 1;
                OrderRequest orderRequest = new OrderRequest
                {
                    itemId = getItems.Value.data[0].itemId,
                    currencyCode = getItems.Value.data[0].regionData[0].currencyCode,
                    discountedPrice = getItems.Value.data[0].regionData[0].discountedPrice,
                    language = getItems.Value.data[0].language,
                    price = getItems.Value.data[0].regionData[0].price,
                    quantity = quantity,
                    region = getItems.Value.data[0].region,
                    returnUrl = "https://www.example.com"
                };

                Result<OrderInfo> orderInfo = null;
                abOrder.CreateOrder(orderRequest, result =>
                {
                    orderInfo = result;
                });
                yield return TestHelper.WaitForValue(() => orderInfo);

                Result<OrderInfo> cancelOrderInfo = null;
                abOrder.CancelOrder("Wrong Order Number", result =>
                {
                    cancelOrderInfo = result;
                });
                yield return TestHelper.WaitForValue(() => cancelOrderInfo);

                TestHelper.Assert.IsTrue(cancelOrderInfo.IsError, "Wrong Order Number, Failed to Cancel the Order");
            }

            int walletBalance = 0;

            [UnityTest, TestLog, Order(3)]
            public IEnumerator GetWalletByCurrencyCode_CurrencyValid_Success()
            {
                Wallet wallet = AccelBytePlugin.GetWallet();
                Result<WalletInfo> getWalletInfoResult = null;

                wallet.GetWalletInfoByCurrencyCode(
                    TestVariables.currencyCode,
                    result => { getWalletInfoResult = result; });
                yield return TestHelper.WaitForValue(() => getWalletInfoResult);

                TestHelper.Assert.IsResultOk(getWalletInfoResult, "Get wallet failed.");
                TestHelper.Assert.IsTrue(getWalletInfoResult.Value.balance > 0, "Wallet balance isn't correct.");

                walletBalance = getWalletInfoResult.Value.balance;
            }

            [UnityTest, TestLog, Order(3)]
            public IEnumerator GetWalletByCurrencyCode_CurrencyInvalid_Success()
            {
                Wallet wallet = AccelBytePlugin.GetWallet();
                const string invalidCurrencyCode = "INVALID";
                Result<WalletInfo> getWalletInfoResult = null;

                wallet.GetWalletInfoByCurrencyCode(invalidCurrencyCode, result => { getWalletInfoResult = result; });
                yield return TestHelper.WaitForValue(() => getWalletInfoResult);

                TestHelper.Assert.IsTrue(getWalletInfoResult.IsError, "Get wallet with invalid currency failed.");
            }

            [UnityTest, TestLog, Order(4)]
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
                yield return TestHelper.WaitForValue(() => walletInfo);

                TestHelper.Assert.IsResultOk(walletInfo, "Credit user wallet failed.");
                TestHelper.Assert.IsTrue(walletInfo.Value.balance == expectedBalance, "Balance is not increased.");
            }

            [UnityTest, TestLog, Order(5)]
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
                yield return TestHelper.WaitForValue(() => getItemsByCriteria);

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
                yield return TestHelper.WaitForValue(() => createOrderResult);

                Result<OrderInfo> getUserOrderResult = null;
                orders.GetUserOrder(createOrderResult.Value.orderNo, result => { getUserOrderResult = result; });
                yield return TestHelper.WaitForValue(() => getUserOrderResult);

                TestHelper.Assert.IsResultOk(getUserOrderResult, "Get user order failed.");
            }

            [UnityTest, TestLog, Order(5)]
            public IEnumerator GetUserOrder_OrderDoesNotExist_Failed()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                const string orderNumberDoesNotExist = "15550000";
                Result<OrderInfo> getUserOrderResult = null;
                orders.GetUserOrder(orderNumberDoesNotExist, result => { getUserOrderResult = result; });
                yield return TestHelper.WaitForValue(() => getUserOrderResult);

                TestHelper.Assert.IsTrue(getUserOrderResult.IsError, "Get user's invalid order does not failed.");
            }

            [UnityTest, TestLog, Order(6)]
            public IEnumerator GetUserOrders_UserHasOrderHistory_Success()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                Result<OrderPagingSlicedResult> getUserOrdersResult = null;
                orders.GetUserOrders(0, 3, result => { getUserOrdersResult = result; });
                yield return TestHelper.WaitForValue(() => getUserOrdersResult);

                TestHelper.Assert.IsResultOk(getUserOrdersResult, "Get user orders failed.");
            }

            //TODO: Need to re-enable this test if get wallet transactions support single global account
            //        [TestFixture, Ignore("Get Wallet transactions doesn't support single global account yet")]
            //        public class E2_GetWalletTransactions
            //        {
            //			[UnityTest, TestLog, Order(2)]
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
            //				TestHelper.Assert.IsResultOk(getWalletTransactionResult, "Get wallet transaction(s) doesn't failed.");
            //				TestHelper.Assert.IsNotNull(getWalletTransactionResult.Value, "Get wallet transaction(s)");
            //			}
            //			
            //			[UnityTest, TestLog, Order(2)]
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
            //				TestHelper.Assert.IsResultOk(getWalletTransactionResult, "Get wallet transaction(s) with invalid currency failed.");
            //			}
            //			
            //			[UnityTest, TestLog, Order(2)]
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

            [UnityTest, TestLog, Order(7)]
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
                yield return TestHelper.WaitForValue(() => getItemsByCriteria);

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
                yield return TestHelper.WaitForValue(() => createOrderResult);

                Result<OrderHistoryInfo[]> getUserOrderHistoryResult = null;
                orders.GetUserOrderHistory(
                    createOrderResult.Value.orderNo,
                    result => { getUserOrderHistoryResult = result; });
                yield return TestHelper.WaitForValue(() => getUserOrderHistoryResult);

                TestHelper.Assert.IsResultOk(getUserOrderHistoryResult, "Get user order history failed.");
                TestHelper.Assert.That(getUserOrderHistoryResult.Value, Is.Not.Null);
            }

            [UnityTest, TestLog, Order(7)]
            public IEnumerator GetUserOrderHistory_OrderNumberDoesNotExist_Success()
            {
                Orders orders = AccelBytePlugin.GetOrders();
                const string orderNumberDoesNotExist = "1555";
                Result<OrderHistoryInfo[]> getUserOrderHistoryResult = null;
                orders.GetUserOrderHistory(orderNumberDoesNotExist, result => { getUserOrderHistoryResult = result; });
                yield return TestHelper.WaitForValue(() => getUserOrderHistoryResult);

                TestHelper.Assert.IsResultOk(getUserOrderHistoryResult, "Get user order history failed.");
            }

            [UnityTest, TestLog, Order(8)]
            public IEnumerator GrantUserEntitlement_Success()
            {
                Entitlement entitlement = AccelBytePlugin.GetEntitlement();
                Result<EntitlementPagingSlicedResult> getEmptyPagedEntitlementsResult = null;
                entitlement.QueryUserEntitlements("", TestVariables.inGameItem.itemId, 0, 20, result => { getEmptyPagedEntitlementsResult = result; });
                yield return TestHelper.WaitForValue(() => getEmptyPagedEntitlementsResult);

                TestHelper.Assert.IsResultOk(getEmptyPagedEntitlementsResult, "Get user entitlements failed.");

                GrantUserEntitlementRequest grantUserEntitlementRequest = new GrantUserEntitlementRequest
                {
                    itemId = TestVariables.inGameItem.itemId,
                    itemNamespace = AccelBytePlugin.Config.Namespace,
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
                yield return TestHelper.WaitForValue(() => stackableEntitlementInfo);

                TestHelper.Assert.IsResultOk(stackableEntitlementInfo, "Grant user entitlement failed.");
                TestHelper.Assert.IsTrue(stackableEntitlementInfo.Value[0].itemId == TestVariables.inGameItem.itemId, "Item is not found.");

                Result<EntitlementPagingSlicedResult> getPagedEntitlementsResult = null;
                entitlement.QueryUserEntitlements("", TestVariables.inGameItem.itemId, 0, 20, result => { getPagedEntitlementsResult = result; });
                yield return TestHelper.WaitForValue(() => getPagedEntitlementsResult);

                bool grantedEntitlementFound = false;
                for (int i = 0; i < getPagedEntitlementsResult.Value.data.Length; i++)
                {
                    if (getPagedEntitlementsResult.Value.data[i].grantedCode == grantUserEntitlementRequest.grantedCode)
                    {
                        grantedEntitlementFound = true;
                    }
                }

                TestHelper.Assert.IsResultOk(getPagedEntitlementsResult, "Get user entitlements failed.");
                TestHelper.Assert.IsTrue(grantedEntitlementFound, "Granted Item is not found.");
            }

            string expectedEntitlementId = "";

            [UnityTest, TestLog, Order(9)]
            public IEnumerator QueryUserEntitlement_Success()
            {
                Entitlement entitlements = AccelBytePlugin.GetEntitlement();
                Result<EntitlementPagingSlicedResult> getPagedEntitlementsResult = null;
                entitlements.QueryUserEntitlements("", "", 0, 20, result => { getPagedEntitlementsResult = result; });
                yield return TestHelper.WaitForValue(() => getPagedEntitlementsResult);

                TestHelper.Assert.IsResultOk(getPagedEntitlementsResult, "Get user entitlements failed");
                TestHelper.Assert.IsTrue(getPagedEntitlementsResult.Value.data.Length > 0, "Empty entitlement");
                expectedEntitlementId = getPagedEntitlementsResult.Value.data[0].id;
            }

            [UnityTest, TestLog, Order(10)]
            public IEnumerator ConsumeUserEntitlementNegative_Failed()
            {
                Entitlement entitlement = AccelBytePlugin.GetEntitlement();

                Result<EntitlementInfo> getEntitlementInfoResult = null;
                entitlement.ConsumeUserEntitlement(expectedEntitlementId, -1, result => { getEntitlementInfoResult = result; });
                yield return TestHelper.WaitForValue(() => getEntitlementInfoResult);

                TestHelper.Assert.IsTrue(getEntitlementInfoResult.IsError, "Consume user entitlement success.");
            }

            [UnityTest, TestLog, Order(10)]
            public IEnumerator ConsumeUserEntitlementZero_Failed()
            {
                Entitlement entitlement = AccelBytePlugin.GetEntitlement();

                Result<EntitlementInfo> getEntitlementInfoResult = null;
                entitlement.ConsumeUserEntitlement(expectedEntitlementId, 0, result => { getEntitlementInfoResult = result; });
                yield return TestHelper.WaitForValue(() => getEntitlementInfoResult);

                TestHelper.Assert.IsTrue(getEntitlementInfoResult.IsError, "Consume user entitlement success.");
            }

            [UnityTest, TestLog, Order(11)]
            public IEnumerator ConsumeUserEntitlement_Success()
            {
                Entitlement entitlement = AccelBytePlugin.GetEntitlement();

                Result<EntitlementInfo> getEntitlementInfoResult = null;
                entitlement.ConsumeUserEntitlement(expectedEntitlementId, 1, result => { getEntitlementInfoResult = result; });
                yield return TestHelper.WaitForValue(() => getEntitlementInfoResult);
                
                TestHelper.Assert.IsResultOk(getEntitlementInfoResult, "Consume user entitlement failed.");
                bool bConsumeUserEntitlement = (getEntitlementInfoResult.Value.status == EntitlementStatus.CONSUMED);
                TestHelper.Assert.IsTrue(bConsumeUserEntitlement, "Consume user entitlement failed.");
            }

            [UnityTest, TestLog, Order(11)]
            public IEnumerator GetUserEntitlementById_Success()
            {
                Entitlement entitlement = AccelBytePlugin.GetEntitlement();

                Result<EntitlementInfo> entitlementInfo = null;
                entitlement.GetUserEntitlementById(expectedEntitlementId, result => { entitlementInfo = result; });
                yield return TestHelper.WaitForValue(() => entitlementInfo);

                TestHelper.Assert.IsResultOk(entitlementInfo, "Get user entitlement by id failed.");
                TestHelper.Assert.IsTrue(entitlementInfo.Value.id == expectedEntitlementId, "Expected entitlement is not found.");
            }

            [UnityTest, TestLog, Order(11)]
            public IEnumerator GetUserEntitlementById_Failed()
            {
                Entitlement entitlement = AccelBytePlugin.GetEntitlement();

                Result<EntitlementInfo> entitlementInfo = null;
                entitlement.GetUserEntitlementById("Invalid", result => { entitlementInfo = result; });
                yield return TestHelper.WaitForValue(() => entitlementInfo);

                TestHelper.Assert.IsTrue(entitlementInfo.IsError, "Get user entitlement by invalid id is success.");
            }

            [UnityTest, TestLog, Order(11)]
            public IEnumerator ServerGetUserEntitlementById_Success()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                Result<EntitlementInfo> entitlementInfo = null;
                serverEcommerce.GetUserEntitlementById(expectedEntitlementId, result => { entitlementInfo = result; });
                yield return TestHelper.WaitForValue(() => entitlementInfo);

                TestHelper.Assert.IsResultOk(entitlementInfo, "Server get user entitlement by id failed.");
                TestHelper.Assert.IsTrue(entitlementInfo.Value.id == expectedEntitlementId, "Expected entitlement is not found.");
            }

            [UnityTest, TestLog, Order(11)]
            public IEnumerator ServerGetUserEntitlementById_Failed()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                Result<EntitlementInfo> entitlementInfo = null;
                serverEcommerce.GetUserEntitlementById("Invalid", result => { entitlementInfo = result; });
                yield return TestHelper.WaitForValue(() => entitlementInfo);

                TestHelper.Assert.IsTrue(entitlementInfo.IsError, "Server get user entitlement by invalid id is success.");
            }

            [UnityTest, TestLog, Order(2)]
            public IEnumerator CreateDistributionReceiver_Success()
            {
                User user = AccelBytePlugin.GetUser();
                string extUserId = "55n8dj2jqgr5s3ryg9cpm4bm7k7vr33t";
                Attributes attributes = new Attributes
                {
                    serverId = "70391cb5af52427e896e05290bc65832",
                    serverName = "testserver",
                    characterId = "32aaf2eabcbb45d096e06be8a4584320",
                    characterName = "newcharacter"
                };

                Result createDistributionResult = null;
                Entitlement entitlements = AccelBytePlugin.GetEntitlement();
                entitlements.CreateDistributionReceiver(extUserId, attributes, result => createDistributionResult = result);
                yield return TestHelper.WaitForValue(() => createDistributionResult);

                TestHelper.LogResult(createDistributionResult, "Create distribution receiver");

                Result<DistributionReceiver[]> getDistributionResult = null;
                entitlements.GetDistributionReceiver(AccelBytePlugin.Config.Namespace, user.Session.UserId, result => getDistributionResult = result);
                yield return TestHelper.WaitForValue(() => getDistributionResult);

                TestHelper.LogResult(getDistributionResult, "Get distribution receiver");

                Debug.Log(
                    "\nserverId: " + getDistributionResult.Value[0].attributes.serverId +
                    "\nserverName: " + getDistributionResult.Value[0].attributes.serverName +
                    "\ncharacterId: " + getDistributionResult.Value[0].attributes.characterId +
                    "\ncharacterName: " + getDistributionResult.Value[0].attributes.characterName + "\n"
                    );

                Result deleteDistributionResult = null;
                entitlements.DeleteDistributionReceiver(extUserId, result => deleteDistributionResult = result);
                yield return TestHelper.WaitForValue(() => deleteDistributionResult);

                TestHelper.LogResult(deleteDistributionResult, "Delete distribution receiver");

                TestHelper.Assert.IsFalse(createDistributionResult.IsError, "Create distribution receiver failed.");
                TestHelper.Assert.IsFalse(getDistributionResult.IsError, "Get distribution receiver failed.");
                TestHelper.Assert.IsFalse(deleteDistributionResult.IsError, "Delete distribution receiver failed.");
            }

            [UnityTest, TestLog, Order(2)]
            public IEnumerator UpdateDistributionReceiver_Success()
            {
                User user = AccelBytePlugin.GetUser();
                string extUserId = "55n8dj2jqgr5s3ryg9cpm4bm7k7vr33t";
                Attributes attributes = new Attributes
                {
                    serverId = "70391cb5af52427e896e05290bc65832",
                    serverName = "testserver",
                    characterId = "32aaf2eabcbb45d096e06be8a4584320",
                    characterName = "newcharacter"
                };

                Result createDistributionResult = null;
                Entitlement entitlements = AccelBytePlugin.GetEntitlement();
                entitlements.CreateDistributionReceiver(extUserId, attributes, result => createDistributionResult = result);
                yield return TestHelper.WaitForValue(() => createDistributionResult);

                TestHelper.LogResult(createDistributionResult, "Create distribution receiver");

                Result<DistributionReceiver[]> getDistributionResult = null;
                entitlements.GetDistributionReceiver(AccelBytePlugin.Config.Namespace, user.Session.UserId, result => getDistributionResult = result);
                yield return TestHelper.WaitForValue(() => getDistributionResult);

                TestHelper.LogResult(getDistributionResult, "Get distribution receiver");

                UnityEngine.Debug.Log(
                    "\nServerId: " + getDistributionResult.Value[0].attributes.serverId +
                    "\nServerName: " + getDistributionResult.Value[0].attributes.serverName +
                    "\nCharacterId: " + getDistributionResult.Value[0].attributes.characterId +
                    "\nCharacterName: " + getDistributionResult.Value[0].attributes.characterName + "\n"
                    );

                Attributes newAttributes = new Attributes
                {
                    serverId = "70391cb5af52427e896e05290bc65832",
                    serverName = "updatedtestserver",
                    characterId = "32aaf2eabcbb45d096e06be8a4584320",
                    characterName = "updatednewcharacter"
                };

                Result updateDistributionResult = null;
                entitlements.UpdateDistributionReceiver(extUserId, newAttributes, result => updateDistributionResult = result);
                yield return TestHelper.WaitForValue(() => updateDistributionResult);

                TestHelper.LogResult(updateDistributionResult, "Update distribution receiver");

                Result<DistributionReceiver[]> getDistributionUpdateResult = null;
                entitlements.GetDistributionReceiver(AccelBytePlugin.Config.Namespace, user.Session.UserId, result => getDistributionUpdateResult = result);
                yield return TestHelper.WaitForValue(() => getDistributionUpdateResult);

                TestHelper.LogResult(getDistributionUpdateResult, "Get distribution receiver after update");

                UnityEngine.Debug.Log(
                    "\nServerId: " + getDistributionUpdateResult.Value[0].attributes.serverId +
                    "\nServerName: " + getDistributionUpdateResult.Value[0].attributes.serverName +
                    "\nCharacterId: " + getDistributionUpdateResult.Value[0].attributes.characterId +
                    "\nCharacterName: " + getDistributionUpdateResult.Value[0].attributes.characterName + "\n"
                    );

                Result deleteDistributionResult = null;
                entitlements.DeleteDistributionReceiver(extUserId, result => deleteDistributionResult = result);
                yield return TestHelper.WaitForValue(() => deleteDistributionResult);

                TestHelper.LogResult(deleteDistributionResult, "Delete distribution receiver");

                TestHelper.Assert.IsFalse(createDistributionResult.IsError, "Create distribution receiver failed.");
                TestHelper.Assert.IsFalse(getDistributionResult.IsError, "Get distribution receiver failed.");
                TestHelper.Assert.IsFalse(updateDistributionResult.IsError, "Update distribution receiver failed.");
                TestHelper.Assert.IsFalse(getDistributionUpdateResult.IsError, "Get distribution receiver after update failed.");
                TestHelper.Assert.IsFalse(deleteDistributionResult.IsError, "Delete distribution receiver failed.");
            }


            [UnityTest, TestLog, Order(11)]
            public IEnumerator ServerFullfillInvalidItemId_Failed()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                FulfillmentRequest fulfillmentRequest = new FulfillmentRequest
                {
                    itemId = "Invalid",
                    language = "en",
                    region = "US",
                    orderNo = "123456789",
                    quantity = 1,
                    source = ItemSource.ACHIEVEMENT
                };
                Result<FulfillmentResult> fulfillmentResult = null;
                serverEcommerce.FulfillUserItem(TestVariables.userId, fulfillmentRequest, result => { fulfillmentResult = result; });

                yield return TestHelper.WaitForValue(() => fulfillmentResult);

                while(fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(fulfillmentResult);
                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Server fulfill user invalid loot success.");
            }

            [UnityTest,TestLog,Order(11)]
            public IEnumerator ServerFulfilledNegativeQuantity_Failed()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                FulfillmentRequest fulfillmentRequest = new FulfillmentRequest
                {
                    itemId = "invalid",
                    language = "en",
                    region = "US",
                    orderNo = "123456789",
                    quantity = -1,
                    source = ItemSource.ACHIEVEMENT
                };
                Result<FulfillmentResult> fulfillmentResult = null;
                serverEcommerce.FulfillUserItem(TestVariables.userId, fulfillmentRequest, result => { fulfillmentResult = result; });
                
                while(fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(fulfillmentResult, "Fulfillment result");
                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Server fulfill user negative quantity loot coin success.");
            }

            [UnityTest,TestLog,Order(11)]
            public IEnumerator ServerFulfillInGameItem_Success()
            {
                ServerEcommerce serverEcommerce = AccelByteServerPlugin.GetEcommerce();

                FulfillmentRequest fulfillmentRequest = new FulfillmentRequest
                {
                    itemId = TestVariables.inGameItem.itemId,
                    language = "en",
                    region = "US",
                    orderNo = "123456789",
                    quantity = 1,
                    source = ItemSource.OTHER
                };
                Result<FulfillmentResult> fulfillmentResult = null;
                serverEcommerce.FulfillUserItem(TestVariables.userId, fulfillmentRequest, result => { fulfillmentResult = result; });

                yield return TestHelper.WaitForValue(() => fulfillmentResult);

                TestHelper.Assert.IsTrue(!fulfillmentResult.IsError, "Server fulfill user loot item failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Value.entitlementSummaries[0].itemId == TestVariables.inGameItem.itemId, "Loot item is not found.");

                Entitlement entitlement = AccelBytePlugin.GetEntitlement();

                Result<EntitlementInfo> getUserEntitlementByIdResult = null;
                entitlement.GetUserEntitlementById(fulfillmentResult.Value.entitlementSummaries[0].id, result => { getUserEntitlementByIdResult = result; });

                TestHelper.WaitForValue(() => getUserEntitlementByIdResult);
                while (getUserEntitlementByIdResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(!getUserEntitlementByIdResult.IsError, "Get user entitlements by entitlement id failed.");
                TestHelper.Assert.IsTrue(getUserEntitlementByIdResult.Value.id == fulfillmentResult.Value.entitlementSummaries[0].id, "Entitlements is not found.");
                TestHelper.Assert.IsTrue(getUserEntitlementByIdResult.Value.itemId == TestVariables.inGameItem.itemId, "Loot item is not found.");
            }
        }
    }
}
