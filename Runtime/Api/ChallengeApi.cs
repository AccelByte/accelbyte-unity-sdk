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
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

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

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetScheduledChallengeGoalsOptionalParameters()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset,
                Tags = tags
            };

            GetScheduledChallengeGoals(challengeCode, optionalParameters, callback);
        }

        internal void GetScheduledChallengeGoals(string challengeCode
            , GetScheduledChallengeGoalsOptionalParameters optionalParameters
            , ResultCallback<GoalResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/challenges/{challengeCode}/goals")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithQueryParam("offset", optionalParameters?.Offset?.ToString())
                .WithQueryParam("limit", optionalParameters?.Limit?.ToString());

            if (optionalParameters?.Tags != null && optionalParameters?.Tags.Length > 0)
            {
                builder.WithQueryParam("tags", optionalParameters?.Tags);
            }

            IHttpRequest request = builder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GoalResponse>();
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

        public void GetChallengeProgress(string challengeCode
            , string goalCode
            , int offset
            , int limit
            , ResultCallback<GoalProgressionResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeProgressOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset
            };

            GetChallengeProgress(challengeCode, goalCode, optionalParameters, callback);
        }

        internal void GetChallengeProgress(string challengeCode
            , string goalCode
            , GetChallengeProgressOptionalParameters optionalParameters
            , ResultCallback<GoalProgressionResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/progress/{challengeCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithQueryParam("goalCode", goalCode)
                .WithQueryParam("offset", optionalParameters?.Offset.ToString())
                .WithQueryParam("limit", optionalParameters?.Limit.ToString())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
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

        public void GetRewards(ChallengeRewardStatus status
            , ChallengeSortBy sortBy
            , int offset
            , int limit
            , ResultCallback<UserRewards> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetRewardsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                SortBy = sortBy
            };

            GetRewards(status, optionalParameters, callback);
        }

        internal void GetRewards(ChallengeRewardStatus status
            , GetRewardsOptionalParameters optionalParameters
            , ResultCallback<UserRewards> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/rewards")
                    .WithBearerAuth(AuthToken)
                    .Accepts(MediaType.ApplicationJson)
                    .WithPathParam("namespace", Config.Namespace);

            if (optionalParameters != null)
            {
                if (optionalParameters.SortBy != null)
                {
                    builder.WithQueryParam("sortBy", ConverterUtils.EnumToDescription(optionalParameters.SortBy));
                }

                builder
                    .WithQueryParam("offset", optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit.ToString());
            }

            if (status != ChallengeRewardStatus.None)
            {
                builder.WithQueryParam("status", ConverterUtils.EnumToDescription(status));
            }

            IHttpRequest request = builder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserRewards>();
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

        public void GetChallengeProgress(string challengeCode
            , int rotationIndex
            , ResultCallback<GoalProgressionResponse> callback
            , string goalCode = ""
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeProgressWithRotationIndexOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                GoalCode = goalCode,
                Limit = limit,
                Offset = offset
            };

            GetChallengeProgress(challengeCode, rotationIndex, optionalParameters, callback);
        }

        internal void GetChallengeProgress(string challengeCode
            , int rotationIndex
            , GetChallengeProgressWithRotationIndexOptionalParameters optionalParameters
            , ResultCallback<GoalProgressionResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(challengeCode);
            error = (rotationIndex < 0) ? new Error(ErrorCode.InvalidRequest) : null;

            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var builder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/progress/{challengeCode}/index/{index}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithPathParam("challengeCode", challengeCode)
                .WithPathParam("index", rotationIndex.ToString());

            if (optionalParameters != null)
            {
                builder
                    .WithQueryParam("goalCode", optionalParameters.GoalCode)
                    .WithQueryParam("offset", optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit.ToString());
            }

            var request = builder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new EvaluateChallengeProgressOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            EvaluateChallengeProgress(optionalParameters, callback);
        }

        internal void EvaluateChallengeProgress(EvaluateChallengeProgressOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var requestBuilder = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/progress/evaluate")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace);

            if (optionalParameters?.ChallengeCodesToEvaluate?.Length > 0)
            {
                requestBuilder.WithQueryParam("challengeCode", string.Join(",", optionalParameters.ChallengeCodesToEvaluate));
            };

            var request = requestBuilder.GetResult();

            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ClaimRewardOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ClaimReward(body, optionalParameters, callback);
        }

        internal void ClaimReward(ClaimRewardRequest body
            , ClaimRewardOptionalParameters optionalParameters
            , ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/public/namespaces/{namespace}/users/me/rewards/claim")
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Config.Namespace)
                .WithBody(body.ToUtf8Json())
                .WithBearerAuth(AuthToken)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserReward[]>();
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

        internal void ListScheduleByGoal(string challengeCode
            , string goalCode
            , ChallengeListScheduleByGoalOptionalParameters optionalParams
            , ResultCallback<ChallengeListScheduleByGoalResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

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

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParams?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParseJson<ChallengeListScheduleByGoalResponse>();
                callback?.Try(result);
            });
        }

        internal void ListSchedules(string challengeCode
            , ChallengeListSchedulesOptionalParameters optionalParams
            , ResultCallback<ChallengeListSchedulesResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParams?.Logger);

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

            var additionalParams = new AdditionalHttpParameters()
            {
                Logger = optionalParams?.Logger
            };

            HttpOperator.SendRequest(additionalParams, request, response =>
            {
                var result = response.TryParseJson<ChallengeListSchedulesResponse>();
                callback?.Try(result);
            });
        }
    }
}