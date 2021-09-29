// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
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
        public class RewardTest
        {
            RewardInfo expectedRewardInfo = null;

            [UnityTest, TestLog, Order(0)]
            public IEnumerator Setup_ExpectedRewardStuff()
            {
                Reward reward = AccelBytePlugin.GetReward();
                Result<QueryRewardInfo> rewardResult = null;
                reward.QueryRewards("statistic", RewardSortBy.NAMESPACE, result =>
                {
                    if (result.Value.data[0] != null)
                    {
                        rewardResult = result;
                        this.expectedRewardInfo = result.Value.data[0];
                    }
                });
                yield return TestHelper.WaitForValue(() => rewardResult);
                TestHelper.Assert.IsResultOk(rewardResult);
            }

            [UnityTest, TestLog, Order(1)]
            public IEnumerator GetRewardByRewardCode_RewardCodeValid_Success()
            {
                Reward reward = AccelBytePlugin.GetReward();
                Result<RewardInfo> rewardResult = null;
                reward.GetRewardByRewardCode(expectedRewardInfo.rewardCode, result =>
                {
                    rewardResult = result;
                });
                yield return TestHelper.WaitForValue(() => rewardResult);
                TestHelper.Assert.IsResultOk(rewardResult);
            }

            [UnityTest, TestLog, Order(2)]
            public IEnumerator GetRewardByRewardCode_RewardCodeInvalid_Success()
            {
                Reward reward = AccelBytePlugin.GetReward();
                Result<RewardInfo> rewardResult = null;
                reward.GetRewardByRewardCode("null", result =>
                {
                    rewardResult = result;
                });
                yield return TestHelper.WaitForValue(() => rewardResult);
                TestHelper.Assert.IsTrue(rewardResult.IsError, "Get Reward failed when reward code is not amtched");
            }

            [UnityTest, TestLog, Order(3)]
            public IEnumerator GetRewardByRewardId_RewardIdValid_Success()
            {
                Reward reward = AccelBytePlugin.GetReward();
                Result<RewardInfo> rewardResult = null;
                reward.GetRewardByRewardId(expectedRewardInfo.rewardId, result =>
                {
                    rewardResult = result;
                });
                yield return TestHelper.WaitForValue(() => rewardResult);
                TestHelper.Assert.IsResultOk(rewardResult);
            }

            [UnityTest, TestLog, Order(4)]
            public IEnumerator GetRewardByRewardId_RewardIdInvalid_Success()
            {
                Reward reward = AccelBytePlugin.GetReward();
                Result<RewardInfo> rewardResult = null;
                reward.GetRewardByRewardId("null", result =>
                {
                    rewardResult = result;
                });
                yield return TestHelper.WaitForValue(() => rewardResult);
                TestHelper.Assert.IsTrue(rewardResult.IsError, "Get Reward failed when reward id is not matched");
            }
        }
    }
}