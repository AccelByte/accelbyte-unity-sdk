// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class LeaderboardApi
    {
        private readonly string baseUrl;
        private readonly IHttpClient httpClient;

        internal LeaderboardApi(string baseUrl, IHttpClient httpClient)
        {
            Assert.IsNotNull(baseUrl, "Creating " + GetType().Name + " failed. Parameter baseUrl is null");
            Assert.IsNotNull(httpClient, "Creating " + GetType().Name + " failed. Parameter httpWorker is null");

            this.baseUrl = baseUrl;
            this.httpClient = httpClient;
        }

        public IEnumerator GetRankings(string @namespace, string accessToken, string leaderboardCode, LeaderboardTimeFrame timeFrame, int offset, int limit,
            ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get ranking! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get ranking! AccessToken parameter is null!");
            Assert.IsNotNull(leaderboardCode, "Can't get ranking! Leaderboard Code parameter is null!");

            string timeFrameString = "";
            switch (timeFrame)
            {
                case LeaderboardTimeFrame.ALL_TIME:
                    timeFrameString = "alltime";
                    break;
                case LeaderboardTimeFrame.CURRENT_SEASON:
                    timeFrameString = "season";
                    break;
                case LeaderboardTimeFrame.CURRENT_MONTH:
                    timeFrameString = "month";
                    break;
                case LeaderboardTimeFrame.CURRENT_WEEK:
                    timeFrameString = "week";
                    break;
                case LeaderboardTimeFrame.TODAY:
                    timeFrameString = "today";
                    break;
                default:
                    break;
            }

            var builder = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/{timeFrame}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("leaderboardCode", leaderboardCode)
                .WithPathParam("timeFrame", timeFrameString)
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit >= 0) ? limit.ToString() : "")
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<LeaderboardRankingResult>();

            callback.Try(result);
        }

        public IEnumerator GetUserRanking(string @namespace, string accessToken, string leaderboardCode, string userId,
            ResultCallback<UserRankingData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get item! Namespace parameter is null!");
            Assert.IsNotNull(accessToken, "Can't get item! AccessToken parameter is null!");
            Assert.IsNotNull(leaderboardCode, "Can't get item! Leaderboard Code parameter is null!");
            Assert.IsNotNull(userId, "Can't get item! UserId parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/users/{userId}")
                .WithPathParam("namespace", @namespace)
                .WithPathParam("leaderboardCode", leaderboardCode)
                .WithPathParam("userId", userId)
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<UserRankingData>();

            callback.Try(result);
        }

        public IEnumerator GetLeaderboardList(string @namespace, string accessToken, int offset, int limit, ResultCallback<LeaderboardPagedList> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);
            Assert.IsNotNull(@namespace, "Can't get leaderboard! Namespace parameter is null!");

            var builder = HttpRequestBuilder
                .CreateGet(this.baseUrl + "/v1/public/namespaces/{namespace}/leaderboards")
                .WithPathParam("namespace", @namespace)
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithBearerAuth(accessToken)
                .Accepts(MediaType.ApplicationJson);

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return this.httpClient.SendRequest(request, rsp => response = rsp);

            var result = response.TryParseJson<LeaderboardPagedList>();

            callback.Try(result);
        }
    }
}
