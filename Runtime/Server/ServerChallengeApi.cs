// Copyright (c) 2024 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System;
using System.Collections.Generic;

namespace AccelByte.Server
{
    /// <summary>
    /// Provide function to let Challenge service wrapper to connect to endpoint.
    /// </summary>
    internal class ServerChallengeApi : ServerApiBase
    {
        [UnityEngine.Scripting.Preserve]
        internal ServerChallengeApi(IHttpClient httpClient
            , ServerConfig config
            , ISession session)
            : base(httpClient, config, config.ChallengeServerUrl, session)
        {
        }

        public void ClaimReward(ChallengeBulkClaimRewardRequest[] challengeBulkClaimRewardRequest
            , ResultCallback<ChallengeBulkClaimRewardResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkClaimRewardOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ClaimReward(challengeBulkClaimRewardRequest, optionalParameters, callback);
        }

        internal void ClaimReward(ChallengeBulkClaimRewardRequest[] challengeBulkClaimRewardRequest
            , BulkClaimRewardOptionalParameters optionalParameters
            , ResultCallback<ChallengeBulkClaimRewardResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken, Namespace_, challengeBulkClaimRewardRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/rewards/claim")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(challengeBulkClaimRewardRequest.ToUtf8Json())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ChallengeBulkClaimRewardResponse[]>();

                callback?.Try(result);
            });
        }

        public void ClaimReward(string userId
            , ClaimRewardRequest claimRewardRequest
            , ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ClaimRewardOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ClaimReward(userId, claimRewardRequest, optionalParameters, callback);
        }

        internal void ClaimReward(string userId
            , ClaimRewardRequest claimRewardRequest
            , ClaimRewardOptionalParameters optionalParameters
            , ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , userId
                , claimRewardRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/rewards/claim")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId)
                .WithBody(claimRewardRequest.ToUtf8Json())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserReward[]>();

                callback?.Try(result);
            });
        }

        public void CreateChallenge(CreateChallengeRequest createChallengeRequest
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateChallenge(createChallengeRequest, optionalParameters, callback);
        }

        internal void CreateChallenge(CreateChallengeRequest createChallengeRequest
            , CreateChallengeOptionalParameters optionalParameters
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , createChallengeRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithBody(createChallengeRequest.ToUtf8Json())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ChallengeResponseInfo>();

                callback?.Try(result);
            });
        }

        public void CreateChallengeGoal(string challengeCode
            , CreateChallengeGoalRequest createChallengeGoalRequest
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateChallengeGoal(challengeCode, createChallengeGoalRequest, optionalParameters, callback);
        }

        internal void CreateChallengeGoal(string challengeCode
            , CreateChallengeGoalRequest createChallengeGoalRequest
            , CreateChallengeGoalOptionalParameters optionalParameters
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode
                , createChallengeGoalRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/goals")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .WithBody(createChallengeGoalRequest.ToUtf8Json())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GoalResponseInfo>();

                callback?.Try(result);
            });
        }

        public void DeleteChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteChallenge(challengeCode, optionalParameters, callback);
        }

        internal void DeleteChallenge(string challengeCode, DeleteChallengeOptionalParameters optionalParameters, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void DeleteChallengeGoal(string challengeCode
            , string goalCode
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteChallengeGoal(challengeCode, goalCode, optionalParameters, callback);
        }

        internal void DeleteChallengeGoal(string challengeCode
            , string goalCode
            , DeleteChallengeGoalOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode
                , goalCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/goals/{goalCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .WithPathParam("goalCode", goalCode)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void DeleteTiedChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteTiedChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteTiedChallenge(challengeCode, optionalParameters, callback);
        }

        internal void DeleteTiedChallenge(string challengeCode
            , DeleteTiedChallengeOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateDelete(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/tied")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void EvaluateChallengeProgress(ChallengeEvaluatePlayerProgressionRequest requestBody
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new EvaluateChallengeProgressOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            EvaluateChallengeProgress(requestBody, optionalParameters, callback);
        }

        internal void EvaluateChallengeProgress(ChallengeEvaluatePlayerProgressionRequest requestBody
            , EvaluateChallengeProgressOptionalParameters optionalParameters
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/progress/evaluate")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", ServerConfig.Namespace)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void GetChallenge(string challengeCode
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetChallenge(challengeCode, optionalParameters, callback);
        }

        internal void GetChallenge(string challengeCode
            , GetChallengeOptionalParameters optionalParameters
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ChallengeResponseInfo>();

                callback?.Try(result);
            });
        }

        public void GetChallengeGoal(string challengeCode
            , string goalCode
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetChallengeGoal(challengeCode, goalCode, optionalParameters, callback);
        }

        internal void GetChallengeGoal(string challengeCode
            , string goalCode
            , GetChallengeGoalOptionalParameters optionalParameters
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode
                , goalCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/goals/{goalCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .WithPathParam("goalCode", goalCode)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GoalResponseInfo>();

                callback?.Try(result);
            });
        }

        public void GetChallengeGoals(string challengeCode
            , ResultCallback<GoalResponse> callback
            , ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengeGoalsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                SortBy = challengeSortBy
            };

            GetChallengeGoals(challengeCode, optionalParameters, callback);
        }

        internal void GetChallengeGoals(string challengeCode
            , GetChallengeGoalsOptionalParameters optionalParameters
            , ResultCallback<GoalResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/goals")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(optionalParameters.SortBy))
                    .WithQueryParam("offset", optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit.ToString());
            }

            var request = requestBuilder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GoalResponse>();

                callback?.Try(result);
            });
        }

        public void GetChallengePeriods(string challengeCode
            , ResultCallback<ChallengePeriodResponse> callback
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetChallengePeriodsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset
            };

            GetChallengePeriods(challengeCode, optionalParameters, callback);
        }

        internal void GetChallengePeriods(string challengeCode
            , GetChallengePeriodsOptionalParameters optionalParameters
            , ResultCallback<ChallengePeriodResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/periods")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode);

            if (optionalParameters != null)
            {
                requestBuilder
                    .WithQueryParam("offset", optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit.ToString());
            }

            var request = requestBuilder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ChallengePeriodResponse>();

                callback?.Try(result);
            });
        }

        [Obsolete("This interface is deprecated, and will be removed on AGS 2025.4. Please use GetChallenges(optionalParameters, callback).")]
        public void GetChallenges(ResultCallback<ChallengeResponse> callback
            , ChallengeStatus status = ChallengeStatus.None
            , ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20)
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

        public void GetChallenges(GetChallengesOptionalParamenters optionalParameters
            , ResultCallback<ChallengeResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

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

            var request = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithQueries(queries)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ChallengeResponse>();

                callback?.Try(result);
            });
        }

        public void GetUserRewards(string userId
            , ResultCallback<UserRewards> callback
            , ChallengeRewardStatus challengeRewardStatus = ChallengeRewardStatus.None
            , ChallengeSortBy challengeSortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserRewardsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                Limit = limit,
                Offset = offset,
                RewardStatus = challengeRewardStatus,
                SortBy = challengeSortBy
            };

            GetUserRewards(userId, optionalParameters, callback);
        }

        internal void GetUserRewards(string userId
            , GetUserRewardsOptionalParameters optionalParameters
            , ResultCallback<UserRewards> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/users/{userId}/rewards")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("userId", userId);

            if (optionalParameters != null)
            {
                if (optionalParameters.RewardStatus != null && optionalParameters.RewardStatus != ChallengeRewardStatus.None)
                {
                    requestBuilder.WithQueryParam("status", ConverterUtils.EnumToDescription(optionalParameters.RewardStatus));
                }

                requestBuilder
                    .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(optionalParameters.SortBy))
                    .WithQueryParam("offset", optionalParameters.Offset.ToString())
                    .WithQueryParam("limit", optionalParameters.Limit.ToString());
            }

            var request = requestBuilder.GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserRewards>();

                callback?.Try(result);
            });
        }

        public void RandomizeChallengeGoals(string challengeCode
            , ResultCallback<RandomizedChallengeResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new RandomizeChallengeGoalsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            RandomizeChallengeGoals(challengeCode, optionalParameters, callback);
        }

        internal void RandomizeChallengeGoals(string challengeCode
            , RandomizeChallengeGoalsOptionalParameters optionalParameters
            , ResultCallback<RandomizedChallengeResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/randomize")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<RandomizedChallengeResponse[]>();

                callback?.Try(result);
            });
        }

        public void UpdateChallenge(string challengeCode
            , UpdateChallengeRequest updateChallengeRequest
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateChallengeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateChallenge(challengeCode, updateChallengeRequest, optionalParameters, callback);
        }

        internal void UpdateChallenge(string challengeCode
            , UpdateChallengeRequest updateChallengeRequest
            , UpdateChallengeOptionalParameters optionalParameters
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode
                , updateChallengeRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .WithBody(updateChallengeRequest.ToUtf8Json())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<ChallengeResponseInfo>();

                callback?.Try(result);
            });
        }

        public void UpdateChallengeGoal(string challengeCode
            , string goalCode
            , UpdateChallengeGoalRequest updateChallengeGoalRequest
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateChallengeGoalOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateChallengeGoal(challengeCode, goalCode, updateChallengeGoalRequest, optionalParameters, callback);
        }

        internal void UpdateChallengeGoal(string challengeCode
            , string goalCode
            , UpdateChallengeGoalRequest updateChallengeGoalRequest
            , UpdateChallengeGoalOptionalParameters optionalParameters
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_
                , challengeCode
                , updateChallengeGoalRequest);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder.CreatePut(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges/{challengeCode}/goals/{goalCode}")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithContentType(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("challengeCode", challengeCode)
                .WithPathParam("goalCode", goalCode)
                .WithBody(updateChallengeGoalRequest.ToUtf8Json())
                .GetResult();
            var additionalParameters = new AdditionalHttpParameters()
            {
                Logger = optionalParameters?.Logger
            };

            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<GoalResponseInfo>();

                callback?.Try(result);
            });
        }
    }
}