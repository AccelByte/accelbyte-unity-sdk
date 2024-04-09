// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide function to let Challenge service wrapper to connect to endpoint.
    /// </summary>
    internal class ChallengeApi : ApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal ChallengeApi(IHttpClient httpClient
            , Config config
            , ISession session)
            : base(httpClient, config, config.ChallengeServerUrl, session)
        {
        }

        public void GetChallenges(ChallengeSortBy sortBy
            , ChallengeStatus status
            , int offset
            , int limit
            , ResultCallback<ChallengeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/challenges")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", Config.Namespace)
                    .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                    .WithQueryParam("offset", offset.ToString())
                    .WithQueryParam("limit", limit.ToString());

            if(status != ChallengeStatus.None)
            {
                builder.WithQueryParam("status", ConverterUtils.EnumToDescription(status));
            }

            IHttpRequest request = builder.GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeResponse>();
                if (!result.IsError)
                {
                    callback?.TryOk(result.Value);
                }
                else
                {
                    callback?.TryError(result.Error);
                };
            });
        }

        public void GetScheduledChallengeGoals(string challengeCode
            , string[] tags
            , int offset
            , int limit
            , ResultCallback<GoalResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/challenges/{challengeCode}/goals")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString());

            if (tags != null && tags.Length > 0)
            {
                builder.WithQueryParam("tags", tags);
            }

            IHttpRequest request = builder.GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GoalResponse>();
                if (!result.IsError)
                {
                    callback?.Invoke(result);
                }
                else
                {
                    callback.TryError(result.Error);
                };
            });
        }

        public void GetChallengeProgress(string challengeCode
            , string goalCode
            , int offset
            , int limit
            , ResultCallback<GoalProgressionResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/progress/{challengeCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithQueryParam("goalCode", goalCode)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GoalProgressionResponse>();
                if (!result.IsError)
                {
                    callback?.Invoke(result);
                }
                else
                {
                    callback.TryError(result.Error);
                };
            });
        }

        public void GetRewards(ChallengeRewardStatus status
            , ChallengeSortBy sortBy
            , int offset
            , int limit
            , ResultCallback<UserRewards> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/rewards")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", Config.Namespace)
                    .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy))
                    .WithQueryParam("offset", offset.ToString())
                    .WithQueryParam("limit", limit.ToString());

            if (status != ChallengeRewardStatus.None)
            {
                builder.WithQueryParam("status", ConverterUtils.EnumToDescription(status));
            }

            IHttpRequest request = builder.GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserRewards>();
                if (!result.IsError)
                {
                    callback?.Invoke(result);
                }
                else
                {
                    callback.TryError(result.Error);
                };
            });
        }

        internal void ClaimReward(ClaimRewardRequest body
            , ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/rewards/claim")
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithBody(body.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .GetResult();

            httpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserReward[]>();
                if (!result.IsError)
                {
                    callback?.Invoke(result);
                }
                else
                {
                    callback.TryError(result.Error);
                };
            });
        }
    }
}