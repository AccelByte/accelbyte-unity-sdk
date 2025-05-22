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

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, leaderboardCode);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            string timeFrameString = "";
            switch( timeFrame )
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

            var builder = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v1/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/{timeFrame}" )
                .WithPathParam( "namespace", Namespace_ )
                .WithPathParam( "leaderboardCode", leaderboardCode )
                .WithPathParam( "timeFrame", timeFrameString )
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit >= 0 ) ? limit.ToString() : "" )
                .Accepts( MediaType.ApplicationJson );

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest( request,
                rsp => response = rsp );

            var result = response.TryParseJson<LeaderboardRankingResult>();

            callback?.Try( result );
        }

        public IEnumerator GetUserRanking( string leaderboardCode
            , string userId
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v1/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/users/{userId}" )
                .WithPathParam( "namespace", Namespace_ )
                .WithPathParam( "leaderboardCode", leaderboardCode )
                .WithPathParam( "userId", userId )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson );

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest( request,
                rsp => response = rsp );

            var result = response.TryParseJson<UserRankingData>();

            callback?.Try( result );
        }

        public IEnumerator GetLeaderboardList( int offset
            , int limit
            , ResultCallback<LeaderboardPagedList> callback )
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v1/public/namespaces/{namespace}/leaderboards" )
                .WithPathParam( "namespace", Namespace_ )
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit > 0 ) ? limit.ToString() : "" )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson );

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest( request,
                rsp => response = rsp );

            var result = response.TryParseJson<LeaderboardPagedList>();

            callback?.Try( result );
        }

        public IEnumerator GetLeaderboardListV3(int offset, int limit, ResultCallback<LeaderboardPagedListV3> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            IHttpRequest request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/leaderboards")
                .WithPathParam("namespace", Namespace_)
                .WithQueryParam( "offset", ( offset >= 0 ) ? offset.ToString() : "" )
                .WithQueryParam( "limit", ( limit > 0 ) ? limit.ToString() : "" )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson )
                .GetResult();
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest( request,
                rsp =>
                {
                    response = rsp;
                });

            Result<LeaderboardPagedListV3> result = response.TryParseJson<LeaderboardPagedListV3>();

            callback?.Try( result );
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

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            IHttpRequest request = HttpRequestBuilder
                .CreateGet(BaseUrl + "/v3/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/alltime")
                .WithPathParam("namespace", Namespace_)
                .WithPathParam("leaderboardCode", leaderboardCode)
                .WithQueryParam("offset", (offset >= 0) ? offset.ToString() : "")
                .WithQueryParam("limit", (limit > 0) ? limit.ToString() : "")
                .WithBearerAuth(AuthToken)
                .Accepts(MediaType.ApplicationJson)
                .GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest(request,
                rsp =>
                {
                    response = rsp;
                });

            Result<LeaderboardRankingResult> result = response.TryParseJson<LeaderboardRankingResult>();

            callback?.Try(result);
        }

        public IEnumerator GetRankingsByCycle(string leaderboardCode, string cycleId, int offset, int limit,
            ResultCallback<LeaderboardRankingResult> callback)
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode, cycleId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

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
            
            IHttpResponse response = null;

            yield return HttpClient.SendRequest( request,
                rsp =>
                {
                    response = rsp;
                });

            Result<LeaderboardRankingResult> result = response.TryParseJson<LeaderboardRankingResult>();

            callback?.Try( result );
        }
        
        public IEnumerator GetUserRankingV3( string leaderboardCode
            , string userId
            , ResultCallback<UserRankingDataV3> callback )
        {
            Report.GetFunctionLog( GetType().Name );

            var error = ApiHelperUtils.CheckForNullOrEmpty(Namespace_, AuthToken, leaderboardCode, userId);
            if (error != null)
            {
                callback?.TryError(error);
                yield break;
            }

            var builder = HttpRequestBuilder
                .CreateGet( BaseUrl + "/v3/public/namespaces/{namespace}/leaderboards/{leaderboardCode}/users/{userId}" )
                .WithPathParam( "namespace", Namespace_ )
                .WithPathParam( "leaderboardCode", leaderboardCode )
                .WithPathParam( "userId", userId )
                .WithBearerAuth( AuthToken )
                .Accepts( MediaType.ApplicationJson );

            var request = builder.GetResult();

            IHttpResponse response = null;

            yield return HttpClient.SendRequest( request,
                rsp =>
                {
                    response = rsp;
                });

            var result = response.TryParseJson<UserRankingDataV3>();

            callback?.Try( result );
        }
    }
}
