// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
using System.Collections;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class LeaderboardApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==LeaderboardServerUrl</param>
        /// <param name="session"></param>
        [UnityEngine.Scripting.Preserve]
        internal LeaderboardApi( IHttpClient httpClient
            , Config config
            , ISession session )
            : base( httpClient, config, config.LeaderboardServerUrl, session )
        {
        }

        public IEnumerator GetRankings( string leaderboardCode
            , LeaderboardTimeFrame timeFrame
            , int offset
            , int limit
            , ResultCallback<LeaderboardRankingResult> callback )
        {
            Report.GetFunctionLog( GetType().Name );

            var optionalParameters = new GetRankingsOptionalParameters() { Offset = offset, Limit = limit, TimeFrame = timeFrame};

            GetRankings(leaderboardCode, optionalParameters, callback);
            
            yield break;
        }

        internal void GetRankings(string leaderboardCode
            , GetRankingsOptionalParameters optionalParams
            , ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, leaderboardCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new GetRankingsOptionalParameters();
            }

            int offset = optionalParams.Offset;
            int limit = optionalParams.Limit;
            
            string timeFrameString = "";
            switch( optionalParams.TimeFrame )
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
            }

            IHttpRequest request = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v1/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/{timeFrame}" )
                .WithPathParam( "namespace", Namespace_ )
                .WithPathParam( "leaderboardCode", leaderboardCode )
                .WithPathParam( "timeFrame", timeFrameString )
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit >= 0 ) ? limit.ToString() : "" )
                .Accepts( MediaType.ApplicationJson )
                .GetResult();

            var additionalParameters = Models.AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);
            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<LeaderboardRankingResult>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetUserRanking( string leaderboardCode
            , string userId
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog( GetType().Name );

            GetUserRanking(leaderboardCode, userId, optionalParams: null, callback);
            
            yield break;
        }

        internal void GetUserRanking(string leaderboardCode
            , string userId
            , GetUserRankingOptionalParameters optionalParams
            , ResultCallback<UserRankingData> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            string targetUserId = userId;
            if (optionalParams != null && optionalParams.AdditionalKey != null)
            {
                targetUserId = $"{userId}_{optionalParams.AdditionalKey}";
            }

            IHttpRequest request = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v1/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/users/{userId}" )
                .WithPathParam( "namespace", Namespace_ )
                .WithPathParam( "leaderboardCode", leaderboardCode )
                .WithPathParam( "userId", targetUserId )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson )
                .GetResult();

            var additionalParameters = Models.AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);
            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserRankingData>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetLeaderboardList( int offset
            , int limit
            , ResultCallback<LeaderboardPagedList> callback )
        {
            Report.GetFunctionLog( GetType().Name );

            var optionalParameters = new GetLeaderboardListOptionalParameters() { Offset = offset, Limit = limit };

            GetLeaderboardList(optionalParameters, callback);
            
            yield break;
        }

        internal void GetLeaderboardList(GetLeaderboardListOptionalParameters optionalParams, ResultCallback<LeaderboardPagedList> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new GetLeaderboardListOptionalParameters();
            }

            int offset = optionalParams.Offset;
            int limit = optionalParams.Limit;

            var request = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v1/public/namespaces/{namespace}/leaderboards" )
                .WithPathParam( "namespace", Namespace_ )
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit > 0 ) ? limit.ToString() : "" )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson )
                .GetResult();
            
            var additionalParameters = Models.AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);
            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<LeaderboardPagedList>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetLeaderboardListV3(int offset, int limit, ResultCallback<LeaderboardPagedListV3> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            GetLeaderboardPagedListV3OptionalParameters optionalParameters =
                new GetLeaderboardPagedListV3OptionalParameters() { Offset = offset, Limit = limit };
            
            GetLeaderboardListV3(optionalParameters, callback);
            
            yield break;
        }

        internal void GetLeaderboardListV3(GetLeaderboardPagedListV3OptionalParameters optionalParams, ResultCallback<LeaderboardPagedListV3> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new GetLeaderboardPagedListV3OptionalParameters();
            }

            int offset = optionalParams.Offset;
            int limit = optionalParams.Limit;

            IHttpRequest request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/leaderboards")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit > 0 ) ? limit.ToString() : "" )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson )
                .GetResult();
            
            var additionalParameters = Models.AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);
            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<LeaderboardPagedListV3>();
                callback?.Try(result);
            });
        }

        [System.Obsolete("Use GetRankingsV3")]
        public IEnumerator GetRangkingsV3(string leaderboardCode, int offset, int limit,
            ResultCallback<LeaderboardRankingResult> callback)
        {
            return GetRankingsV3(leaderboardCode, offset, limit, callback);
        }

        public IEnumerator GetRankingsV3(string leaderboardCode, int offset, int limit,
            ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            var optionalParameters = new GetRankingV3OptionalParameters()
            {
                Offset = offset,
                Limit = limit
            };
            GetRankingsV3(leaderboardCode, optionalParameters, callback);
            
            yield break;
        }
        
        internal void GetRankingsV3(string leaderboardCode, GetRankingV3OptionalParameters optionalParams, ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new GetRankingV3OptionalParameters();
            }

            int offset = optionalParams.Offset;
            int limit = optionalParams.Limit;

            IHttpRequest request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/alltime")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("leaderboardCode", leaderboardCode)
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();
            
            var additionalParameters = Models.AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);
            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<LeaderboardRankingResult>();
                callback?.Try(result);
            });
        }

        public IEnumerator GetRankingsByCycle(string leaderboardCode, string cycleId, int offset, int limit,
            ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var optionalParameters = new GetRankingsByCycleOptionalParameters()
            {
                Offset = offset,
                Limit = limit
            };
            GetRankingsByCycle(leaderboardCode, cycleId, optionalParameters, callback);
            
            yield break;
        }
        
        internal void GetRankingsByCycle(string leaderboardCode, string cycleId, GetRankingsByCycleOptionalParameters optionalParams, ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode, cycleId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            if (optionalParams == null)
            {
                optionalParams = new GetRankingsByCycleOptionalParameters();
            }

            int offset = optionalParams.Offset;
            int limit = optionalParams.Limit;

            IHttpRequest request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/cycles/{cycleId}")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("leaderboardCode", leaderboardCode)
                .WithPathParam("cycleId", cycleId)
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit > 0 ) ? limit.ToString() : "" )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson )
                .GetResult();
            
            var additionalParameters = Models.AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);
            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<LeaderboardRankingResult>();
                callback?.Try(result);
            });
        }
        
        public IEnumerator GetUserRankingV3( string leaderboardCode
            , string userId
            , ResultCallback<UserRankingDataV3> callback )
        {
            Report.GetFunctionLog( GetType().Name );

            GetUserRankingV3(leaderboardCode, userId, optionalParams: null, callback);
            
            yield break;
        }
        
        internal void GetUserRankingV3(string leaderboardCode, string userId, GetUserRankingV3OptionalParameters optionalParams, ResultCallback<UserRankingDataV3> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode, userId);
            if (error != null)
            {
                callback?.TryError(error);
                return;
            }

            var request = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v3/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/users/{userId}" )
                .WithPathParam( "namespace", Namespace_ )
                .WithPathParam( "leaderboardCode", leaderboardCode )
                .WithPathParam( "userId", userId )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson )
                .GetResult();
            
            var additionalParameters = Models.AdditionalHttpParameters.CreateFromOptionalParameters(optionalParams);
            HttpOperator.SendRequest(additionalParameters, request, response =>
            {
                var result = response.TryParseJson<UserRankingDataV3>();
                callback?.Try(result);
            });
        }
    }
}
