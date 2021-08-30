// Copyright (c) 2018 - 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
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
        public class FulfillmentTest
        {
            [UnityTest, Order(1)]
            public IEnumerator RedeemCode_Success()
            {
                Fulfillment fulfillment = AccelBytePlugin.GetFulfillment();

                Result<FulfillmentResult> fulfillmentResult = null;

                fulfillment.RedeemCode(
                    TestVariables.codeInfo.value, 
                    TestVariables.region,
                    TestVariables.language,
                    result => { fulfillmentResult = result; });

                while (fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsFalse(fulfillmentResult.IsError, "Redeem code failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Value.entitlementSummaries[0].itemId == TestVariables.inGameItem.itemId, "Redeemable item is not found.");
                
                Entitlement entitlement = AccelBytePlugin.GetEntitlement();

                Result<EntitlementInfo> getUserEntitlementByIdResult = null;
                entitlement.GetUserEntitlementById(fulfillmentResult.Value.entitlementSummaries[0].id, result => { getUserEntitlementByIdResult = result; });

                while (getUserEntitlementByIdResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsFalse(getUserEntitlementByIdResult.IsError, "Get user entitlements by entitlement id failed.");
                TestHelper.Assert.IsTrue(getUserEntitlementByIdResult.Value.id == fulfillmentResult.Value.entitlementSummaries[0].id, "Entitlements is not found.");
                TestHelper.Assert.IsTrue(getUserEntitlementByIdResult.Value.itemId == TestVariables.inGameItem.itemId, "Redeemable item is not found.");
                TestHelper.Assert.IsTrue(getUserEntitlementByIdResult.Value.source == EntitlementSource.REDEEM_CODE, "Item is not from REDEEM CODE.");
            }

            [UnityTest, Order(2)]
            public IEnumerator RedeemCode_MaxRedeemCountPerCodePerUserExceeded_Failed()
            {
                Fulfillment fulfillment = AccelBytePlugin.GetFulfillment();

                Result<FulfillmentResult> fulfillmentResult = null;

                fulfillment.RedeemCode(
                    TestVariables.codeInfo.value, 
                    TestVariables.region,
                    TestVariables.language,
                    result => { fulfillmentResult = result; });

                while (fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.LogResult(fulfillmentResult, "Fulfilment result");
                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Redeem code when max redeem count per code per user exceeded not failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Error.Code == ErrorCode.MaxRedeemCountPerCodePerUserExceeded, "Error is not due to max redeem count per code per user exceeded.");
            }

            [UnityTest, Order(3)]
            public IEnumerator RedeemCode_CampaignInactive_Failed()
            {
                TestHelper testHelper = new TestHelper();

                 TestHelper.CampaignUpdateModel campaignUpdate = new TestHelper.CampaignUpdateModel 
                {
                    name = TestVariables.campaignName,
                    description = "Unity Campaign Test",
                    tags = new string[]{}, 
                    status = "INACTIVE",
                    maxRedeemCountPerCode = -1,
                    maxRedeemCountPerCodePerUser = 1,
                    maxRedeemCountPerCampaignPerUser = -1,
                    maxSaleCount = -1,
                    redeemStart = DateTime.UtcNow,
                    redeemEnd = DateTime.UtcNow + TimeSpan.FromDays(1000),
                    redeemType = "ITEM",
                    items = new TestHelper.RedeemableItem[]{}
                };
                Result<Tests.TestHelper.CampaignInfo> updateCampaignResult = null;
                testHelper.UpdateCampaign(
                    TestVariables.accessToken,
                    TestVariables.campaignResult.id,
                    campaignUpdate, 
                    result => {updateCampaignResult = result;});
                
                while (updateCampaignResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }

                Assert.IsFalse(updateCampaignResult.IsError);

                Fulfillment fulfillment = AccelBytePlugin.GetFulfillment();

                Result<FulfillmentResult> fulfillmentResult = null;

                fulfillment.RedeemCode(
                    TestVariables.codeInfo.value, 
                    TestVariables.region,
                    TestVariables.language,
                    result => { fulfillmentResult = result; });

                while (fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Redeem code when campaign inactive not failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Error.Code == ErrorCode.CampaignInactive, "Error is not due to campaign inactive.");
            }

            [UnityTest, Order(4)]
            public IEnumerator RedeemCode_CampaignCodeInactive_Failed()
            {
                TestHelper testHelper = new TestHelper();
                Result<Tests.TestHelper.CodeInfo> disableCodeResult = null;
                testHelper.DisableCampaignCode(
                    TestVariables.accessToken,
                    TestVariables.codeInfo.value,
                    result => {disableCodeResult = result;});
                
                while (disableCodeResult == null)
                {
                    Thread.Sleep(100);
                    yield return null;
                }

                Assert.IsFalse(disableCodeResult.IsError);

                Fulfillment fulfillment = AccelBytePlugin.GetFulfillment();

                Result<FulfillmentResult> fulfillmentResult = null;

                fulfillment.RedeemCode(
                    TestVariables.codeInfo.value, 
                    TestVariables.region,
                    TestVariables.language,
                    result => { fulfillmentResult = result; });

                while (fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Redeem code when campaign code inactive not failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Error.Code == ErrorCode.CodeInactive, "Error is not due to campaign inactive.");
            }

            [UnityTest, Order(1)]
            public IEnumerator RedeemCode_CodeDoesNotExistInNamespace_Failed()
            {
                Fulfillment fulfillment = AccelBytePlugin.GetFulfillment();

                Result<FulfillmentResult> fulfillmentResult = null;

                fulfillment.RedeemCode(
                    "InvalidCode", 
                    TestVariables.region,
                    TestVariables.language,
                    result => { fulfillmentResult = result; });

                while (fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Redeem code when code not exist failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Error.Code == ErrorCode.CodeNotFound, "Error is not due to code not found.");
            }

            [UnityTest, Order(1)]
            public IEnumerator RedeemCode_RedeemptionNotStarted_Failed()
            {
                Fulfillment fulfillment = AccelBytePlugin.GetFulfillment();

                Result<FulfillmentResult> fulfillmentResult = null;

                fulfillment.RedeemCode(
                    TestVariables.notStartedCodeInfo.value, 
                    TestVariables.region,
                    TestVariables.language,
                    result => { fulfillmentResult = result; });

                while (fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Redeem code when redemption not started not failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Error.Code == ErrorCode.CodeRedeemptionNotStarted, "Error is not due to redemption not started.");
            }

            [UnityTest, Order(1)]
            public IEnumerator RedeemCode_RedeemptionAlreadyEnded_Failed()
            {
                Fulfillment fulfillment = AccelBytePlugin.GetFulfillment();

                Result<FulfillmentResult> fulfillmentResult = null;

                fulfillment.RedeemCode(
                    TestVariables.expiredCodeInfo.value, 
                    TestVariables.region,
                    TestVariables.language,
                    result => { fulfillmentResult = result; });

                while (fulfillmentResult == null)
                {
                    Thread.Sleep(100);

                    yield return null;
                }

                TestHelper.Assert.IsTrue(fulfillmentResult.IsError, "Redeem code when redemption already ended not failed.");
                TestHelper.Assert.IsTrue(fulfillmentResult.Error.Code == ErrorCode.CodeRedeemptionAlreadyEnded, "Error is not due to redemption already ended.");
            }
        }
    }
}
