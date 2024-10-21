// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;

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
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeBulkClaimRewardResponse[]>();

                callback?.Try(result);
            });
        }

        public void ClaimReward(string userId
            , ClaimRewardRequest claimRewardRequest
            , ResultCallback<UserReward[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserReward[]>();

                callback?.Try(result);
            });
        }

        public void CreateChallenge(CreateChallengeRequest createChallengeRequest
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeResponseInfo>();

                callback?.Try(result);
            });
        }

        public void CreateChallengeGoal(string challengeCode
            , CreateChallengeGoalRequest createChallengeGoalRequest
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GoalResponseInfo>();

                callback?.Try(result);
            });
        }

        public void DeleteChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void DeleteChallengeGoal(string challengeCode
            , string goalCode
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void DeleteTiedChallenge(string challengeCode, ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void EvaluateChallengeProgress(ChallengeEvaluatePlayerProgressionRequest requestBody
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var request = HttpRequestBuilder.CreatePost(BaseUrl + "/v1/admin/namespaces/{namespace}/progress/evaluate")
                .WithBearerAuth(AuthToken)
                .WithContentType(MediaType.ApplicationJson)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", ServerConfig.Namespace)
                .WithBody(requestBody.ToUtf8Json())
                .GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParse();

                callback?.Try(result);
            });
        }

        public void GetChallenge(string challengeCode
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeResponseInfo>();

                callback?.Try(result);
            });
        }

        public void GetChallengeGoal(string challengeCode
            , string goalCode
            , ResultCallback<GoalResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
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
            Report.GetFunctionLog(GetType().Name);

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
                .WithPathParam("challengeCode", challengeCode)
                .WithQueryParam("sortBy", ConverterUtils.EnumToDescription(challengeSortBy));

            if (offset >= 0)
            {
                requestBuilder.WithQueryParam("offset", offset.ToString());
            }
            if (limit >= 0)
            {
                requestBuilder.WithQueryParam("limit", limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
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
            Report.GetFunctionLog(GetType().Name);

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

            if (offset >= 0)
            {
                requestBuilder.WithQueryParam("offset", offset.ToString());
            }
            if (limit >= 0)
            {
                requestBuilder.WithQueryParam("limit", limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengePeriodResponse>();

                callback?.Try(result);
            });
        }

        public void GetChallenges(ResultCallback<ChallengeResponse> callback
            , ChallengeStatus status = ChallengeStatus.None
            , ChallengeSortBy sortBy = ChallengeSortBy.UpdatedAtDesc
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            var error = ApiHelperUtils.CheckForNullOrEmpty(AuthToken
                , Namespace_);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var requestBuilder = HttpRequestBuilder.CreateGet(BaseUrl + "/v1/admin/namespaces/{namespace}/challenges")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .WithPathParam("namespace", Namespace_);

            if (status != ChallengeStatus.None)
            {
                requestBuilder.WithQueryParam("status", status.ToString());
            }
            requestBuilder.WithQueryParam("sortBy", ConverterUtils.EnumToDescription(sortBy));
            if (offset >= 0)
            {
                requestBuilder.WithQueryParam("offset", offset.ToString());
            }
            if (limit >= 0)
            {
                requestBuilder.WithQueryParam("limit", limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
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
            Report.GetFunctionLog(GetType().Name);

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

            if (challengeRewardStatus != ChallengeRewardStatus.None)
            {
                requestBuilder.WithQueryParam("status", challengeRewardStatus.ToString());
            }
            requestBuilder.WithQueryParam("sortBy", ConverterUtils.EnumToDescription(challengeSortBy));
            if (offset >= 0)
            {
                requestBuilder.WithQueryParam("offset", offset.ToString());
            }
            if (limit >= 0)
            {
                requestBuilder.WithQueryParam("limit", limit.ToString());
            }

            var request = requestBuilder.GetResult();

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<UserRewards>();

                callback?.Try(result);
            });
        }

        public void RandomizeChallengeGoals(string challengeCode
            , ResultCallback<ChallengeResponseInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<ChallengeResponseInfo[]>();

                callback?.Try(result);
            });
        }

        public void UpdateChallenge(string challengeCode
            , UpdateChallengeRequest updateChallengeRequest
            , ResultCallback<ChallengeResponseInfo> callback)
        {
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
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
            Report.GetFunctionLog(GetType().Name);

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

            HttpOperator.SendRequest(request, response =>
            {
                var result = response.TryParseJson<GoalResponseInfo>();

                callback?.Try(result);
            });
        }
    }
}