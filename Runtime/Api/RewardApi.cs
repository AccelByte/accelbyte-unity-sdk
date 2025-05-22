// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;

namespace AccelByte.Api
{
    public class RewardApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==PlatformServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal RewardApi( IHttpClient httpClient
            , Config config
            , ISession session ) 
            : base( httpClient, config, config.PlatformServerUrl, session )
        {
        }

        string ConvertRewardSortByToString( RewardSortBy sortBy )
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

        public IEnumerator GetRewardByRewardCode( string rewardCode
            , ResultCallback<RewardInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/rewards/byCode")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("rewardCode", rewardCode)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<RewardInfo>();

            callback?.Try(result);
        }

        public IEnumerator GetRewardByRewardId( string rewardId
            , ResultCallback<RewardInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, rewardId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/rewards/{rewardId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("rewardId", rewardId)
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<RewardInfo>();

            callback?.Try(result);
        }

        public IEnumerator QueryRewards( string eventTopic
            , int offset
            , int limit
            , RewardSortBy sortBy
            , ResultCallback<QueryRewardInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/public/namespaces/{namespace}/rewards/byCriteria")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam("eventTopic", eventTopic)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .WithQueryParam("sortBy", ConvertRewardSortByToString(sortBy))
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request, 
                rsp => response = rsp);

            var result = response.TryParseJson<QueryRewardInfo>();

            callback?.Try(result);
        }
    }
}