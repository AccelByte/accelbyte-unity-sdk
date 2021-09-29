// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class RewardApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        internal RewardApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        string ConvertRewardSortByToString(RewardSortBy sortBy)
        {
            switch (sortBy)
            {
                case RewardSortBy.NAMESPACE:
                    return "namespace";
                case RewardSortBy.NAMESPACE_ASC:
                    return "namespace:asc";
                case RewardSortBy.NAMESPACE_DESC:
                    return "namespace:desc";
                case RewardSortBy.REWARDCODE:
                    return "rewardcode";
                case RewardSortBy.REWARDCODE_ASC:
                    return "rewardcode:asc";
                case RewardSortBy.REWARDCODE_DESC:
                    return "rewardcode:desc";
            }
            return "";
        }

        public IEnumerator GetRewardByRewardCode(string @namespace, string accessToken, string rewardCode, ResultCallback<RewardInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Get Reward By Reward Code! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Get Reward By Reward Code! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/rewards/byCode")
                .WithPathParam("namepsace", @namespace)
                .WithQueryParam("rewardCode", rewardCode)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<RewardInfo>();

            callback.Try(result);
        }

        public IEnumerator GetRewardByRewardId(string @namespace, string accessToken, string rewardId, ResultCallback<RewardInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Get Reward By Reward Code! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't Get Reward By Reward Code! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/rewards/{rewardId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("rewardId", rewardId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<RewardInfo>();

            callback.Try(result);
        }

        public IEnumerator QueryRewards(string @namespace, string accessToken, string eventTopic, int offset, int limit, RewardSortBy sortBy, ResultCallback<QueryRewardInfo> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't Query Rewards! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't QueryRewards! accessToken parameter is null!");

            var request = HttpRequestBuilder.CreateGet(this.baseUrl + "/public/namespaces/{namespace}/rewards/byCriteria")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("eventTopic", eventTopic)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("sortBy", ConvertRewardSortByToString(sortBy))
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<QueryRewardInfo>();

            callback.Try(result);
        }
    }
}