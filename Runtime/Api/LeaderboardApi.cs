// Copyright (c) 2020-2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System.Collections;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    internal class LeaderboardApi : ApiBase
    {
        /// <summary>
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="config">baseUrl==LeaderboardServerUrl</param>
        /// <param name="session"></param>
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
            Assert.IsNotNull( Namespace_, "Can't get ranking! Namespace parameter is null!" );
            Assert.IsNotNull( leaderboardCode, "Can't get ranking! Leaderboard Code parameter is null!" );

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

            callback.Try( result );
        }

        public IEnumerator GetUserRanking( string leaderboardCode
            , string userId
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog( GetType().Name );
            Assert.IsNotNull( Namespace_, "Can't get item! Namespace parameter is null!" );
            Assert.IsNotNull( AuthToken, "Can't get item! AccessToken parameter is null!" );
            Assert.IsNotNull( leaderboardCode, "Can't get item! Leaderboard Code parameter is null!" );
            Assert.IsNotNull( userId, "Can't get item! UserId parameter is null!" );

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

            callback.Try( result );
        }

        public IEnumerator GetLeaderboardList( int offset
            , int limit
            , ResultCallback<LeaderboardPagedList> callback )
        {
            Report.GetFunctionLog( GetType().Name );
            Assert.IsNotNull( Namespace_, "Can't get leaderboard! Namespace parameter is null!" );

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

            callback.Try( result );
        }

    }
}
