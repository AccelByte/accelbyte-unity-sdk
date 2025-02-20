// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections.Generic;
using System.Globalization;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide function to let Challenge service wrapper to connect to endpoint.
    /// </summary>
    internal class ChallengeApi : ApiBase
    {
        private const string dateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ";

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
            var optionalParams = new GetChallengesOptionalParamenters()
            {
                Limit = limit,
                Offset = offset,
                SortBy = sortBy,
                Status = status
            };

            GetChallenges(optionalParams, callback);
        }

        internal void GetChallenges(GetChallengesOptionalParamenters optionalParameters
            , ResultCallback<ChallengeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var queries = new Dictionary<string, string>();
            if (optionalParameters?.SortBy != null)
            {
                queries["sortBy"] = ConverterUtils.EnumToDescription(optionalParameters.SortBy);
            }
            if (optionalParameters?.Status != null && optionalParameters?.Status.Value != ChallengeStatus.None)
            {
                queries["status"] = ConverterUtils.EnumToDescription(optionalParameters.Status);
            }
            if (optionalParameters?.Tags != null)
            {
                queries["tags"] = string.Join(',', optionalParameters.Tags);
            }
            if (!string.IsNullOrEmpty(optionalParameters?.Keyword))
            {
                queries["keyword"] = optionalParameters.Keyword;
            }
            if (optionalParameters?.Offset != null)
            {
                queries["offset"] = optionalParameters.Offset.Value.ToString();
            }
            if (optionalParameters?.Limit != null && optionalParameters?.Limit > 0)
            {
                queries["limit"] = optionalParameters.Limit.Value.ToString();
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/challenges")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", Config.Namespace)
                    .WithQueries(queries)
                    .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeResponse>();

                callback?.Try(result);
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

            HttpOperator.SendRequest(request, response =>
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

            HttpOperator.SendRequest(request, response =>
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

            HttpOperator.SendRequest(request, response =>
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

        public void GetChallengeProgress(string challengeCode
            , int rotationIndex
            , ResultCallback<GoalProgressionResponse> callback
            , string goalCode = ""
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(challengeCode);
            error = (rotationIndex < 0) ? new Error(ErrorCode.InvalidRequest) : null;

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/progress/{challengeCode}/index/{index}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithPathParam("index", rotationIndex.ToString())
                .WithQueryParam("goalCode", goalCode)
                .WithQueryParam("offset", offset.ToString())
                .WithQueryParam("limit", limit.ToString())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GoalProgressionResponse>();
                if (!result.IsError)
                {
                    callback?.Invoke(result);
                }
                else
                {
                    callback?.TryError(result.Error);
                };
            });
        }

        public void EvaluateChallengeProgress(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/progress/evaluate")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();
                if (!result.IsError)
                {
                    callback?.Invoke(result);
                }
                else
                {
                    callback?.TryError(result.Error);
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

            HttpOperator.SendRequest(request, response =>
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

        internal void ListScheduleByGoal(string challengeCode
            , string goalCode
            , ChallengeListScheduleByGoalOptionalParameters optionalParams
            , ResultCallback<ChallengeListScheduleByGoalResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(challengeCode
                , goalCode
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var queries = new Dictionary<string, string>();

            if (optionalParams?.Offset != null)
            {
                queries.Add("offset", optionalParams?.Offset.Value.ToString());
            }
            if (optionalParams?.Limit != null)
            {
                queries.Add("limit", optionalParams?.Limit.Value.ToString());
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/challenges/{challengeCode}/goals/{code}/schedules")
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithPathParam("code", goalCode)
                .WithQueries(queries)
                .WithBearerAuth(AuthToken)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeListScheduleByGoalResponse>();
                callback?.Try(result);
            });
        }

        internal void ListSchedules(string challengeCode
            , ChallengeListSchedulesOptionalParameters optionalParams
            , ResultCallback<ChallengeListSchedulesResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(challengeCode
                , Namespace_
                , AuthToken);

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var queries = new Dictionary<string, string>();

            if (optionalParams?.DateTime != null)
            {
                queries.Add("dateTime", optionalParams?.DateTime.Value.ToString(dateTimeFormat, DateTimeFormatInfo.InvariantInfo));
            }
            if (optionalParams?.Offset != null)
            {
                queries.Add("offset", optionalParams?.Offset.Value.ToString());
            }
            if (optionalParams?.Limit != null)
            {
                queries.Add("limit", optionalParams?.Limit.Value.ToString());
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/challenges/{challengeCode}/schedules")
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithQueries(queries)
                .WithBearerAuth(AuthToken)
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeListSchedulesResponse>();
                callback?.Try(result);
            });
        }
    }
}