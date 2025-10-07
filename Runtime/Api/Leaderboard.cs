// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Leaderboard : WrapperBase
    {
        private readonly LeaderboardApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Leaderboard( LeaderboardApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner==null (@ constructor)");

            api = inApi;
            session = inSession;
            coroutineRunner = inCoroutineRunner;
        }
        
        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal Leaderboard( LeaderboardApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get leaderboard ranking data from the beginning.
        /// </summary>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="timeFrame">The time frame of leaderboard</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        /// <param name="callback">
        /// Returns a Result that contains LeaderboardRankingResult via callback when completed
        /// </param>
        public void GetRankings( string leaderboardCode
            , LeaderboardTimeFrame timeFrame
            , int offset
            , int limit
            , ResultCallback<LeaderboardRankingResult> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            GetRankings(leaderboardCode, optionalParameters: new GetRankingsOptionalParameters()
            {
                Limit = limit,
                Offset = offset,
                TimeFrame = timeFrame
            }, callback);
        }
        
        internal void GetRankings( string leaderboardCode
            , GetRankingsOptionalParameters optionalParameters
            , ResultCallback<LeaderboardRankingResult> callback )
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetRankings(
                leaderboardCode,
                optionalParameters,
                cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        var param = new LeaderboardPredefinedParameters()
                        {
                            LeaderboardCode = leaderboardCode,
                            UserId = session.UserId
                        };
                        SendPredefinedEvent(param, EventMode.GetRankings);
                    }
                    HandleCallback(cb, callback);
                });
        }

        /// <summary>
        /// Get user's ranking from leaderboard.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">
        /// Returns a Result that contains UserRankingData via callback when completed
        /// </param>
        public void GetUserRanking( string userId
            , string leaderboardCode
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            GetUserRanking(userId, leaderboardCode, optionalParameters: null, callback);
        }

        /// <summary>
        /// Get user's ranking from leaderboard with additional key. The additional key will be
        /// suffixed to the userId to access multi level user ranking, such as character ranking.
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="additionalKey">
        /// To identify multi level user ranking, such as character ranking
        /// </param>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">
        /// Returns a Result that contains UserRankingData via callback when completed
        /// </param>
        public void GetUserRanking( string userId
            , string additionalKey
            , string leaderboardCode
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            GetUserRanking(userId, leaderboardCode, optionalParameters: new GetUserRankingOptionalParameters()
            {
                AdditionalKey = additionalKey
            }, callback);
        }
        
        internal void GetUserRanking( string userId
            , string leaderboardCode
            , GetUserRankingOptionalParameters optionalParameters
            , ResultCallback<UserRankingData> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserRanking(
                leaderboardCode,
                userId,
                optionalParameters,
                cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        var param = new LeaderboardPredefinedParameters()
                        {
                            LeaderboardCode = leaderboardCode,
                            UserId = userId
                        };
                        SendPredefinedEvent(param, EventMode.GetUserRanking);
                    }
                    HandleCallback(cb, callback);
                });
        }

        /// <summary>
        /// List all leaderboard by given namespace
        /// </summary>
        /// <param name="callback">
        /// Returns a Result that contains LeaderboardPagedList via callback when completed</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        public void GetLeaderboardList( ResultCallback<LeaderboardPagedList> callback
            , int offset = 0
            , int limit = 0 )
        {
            Report.GetFunctionLog(GetType().Name);

            GetLeaderboardList(new GetLeaderboardListOptionalParameters()
                {
                    Limit = limit,
                    Offset = offset
                },
                callback);
        }
        
        internal void GetLeaderboardList(GetLeaderboardListOptionalParameters optionalParameters, ResultCallback<LeaderboardPagedList> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetLeaderboardList(
                optionalParameters,
                cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        var param = new LeaderboardPredefinedParameters()
                        {
                            UserId = session.UserId
                        };
                        SendPredefinedEvent(param, EventMode.GetLeaderboards);
                    }
                    HandleCallback(cb, callback);
                });
        }

        /// <summary>
        /// List all leaderboard by given namespace
        /// </summary>
        /// <param name="callback">Returns a Result that contains LeaderboardPagedListV3 via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional) </param>
        public void GetLeaderboardListV3(ResultCallback<LeaderboardPagedListV3> callback, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            GetLeaderboardListV3(new GetLeaderboardPagedListV3OptionalParameters()
                {
                    Limit = limit,
                    Offset = offset
                },
                callback);
        }
        
        internal void GetLeaderboardListV3(GetLeaderboardPagedListV3OptionalParameters optionalParameters, ResultCallback<LeaderboardPagedListV3> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetLeaderboardListV3(optionalParameters, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    var param = new LeaderboardPredefinedParameters() { UserId = session.UserId };
                    SendPredefinedEvent(param, EventMode.GetLeaderboards);
                }

                HandleCallback(cb, callback);
            });
        }
        
        /// <summary>
        /// Get leaderboard configuration by given leaderboard code
        /// </summary>
        /// <param name="callback">Returns a Result that contains leaderboard configuration</param>
        /// <param name="leaderboardCode"></param>
        public void GetLeaderboardV3(string leaderboardCode, ResultCallback<LeaderboardDataV3> callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            api.GetLeaderboardV3(leaderboardCode, null, callback);
        }
        
        /// <summary>
        /// Get leaderboard configuration by given leaderboard code
        /// </summary>
        /// <param name="callback">Returns a Result that contains leaderboard configuration</param>
        /// <param name="optionalParams">Returns a Result that contains leaderboard configuration</param>
        /// <param name="leaderboardCode"></param>
        internal void GetLeaderboardV3(string leaderboardCode, GetLeaderboardOptionalParameters optionalParams, ResultCallback<LeaderboardDataV3> callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            api.GetLeaderboardV3(leaderboardCode, optionalParams, callback);
        }

        /// <summary>
        /// Get leaderboard ranking data from the beginning.
        /// </summary>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">Returns a Result that contains LeaderboardRankingResult via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        [Obsolete("Use GetRankingsV3")]
        public void GetRangkingsV3(string leaderboardCode, ResultCallback<LeaderboardRankingResult> callback,
            int offset = 0, int limit = 20)
        {
            GetRankingsV3(leaderboardCode, callback, offset, limit);
        }

        /// <summary>
        /// Get leaderboard ranking data from the beginning.
        /// </summary>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">Returns a Result that contains LeaderboardRankingResult via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        public void GetRankingsV3(string leaderboardCode, ResultCallback<LeaderboardRankingResult> callback,
            int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            GetRankingsV3(leaderboardCode, new GetRankingV3OptionalParameters()
            {
                Offset = offset,
                Limit = limit
            }, callback);
        }
        
        internal void GetRankingsV3(string leaderboardCode, GetRankingV3OptionalParameters optionalParameters, ResultCallback<LeaderboardRankingResult> callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetRankingsV3(leaderboardCode, optionalParameters, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    var param = new LeaderboardPredefinedParameters()
                    {
                        LeaderboardCode = leaderboardCode,
                        UserId = session.UserId
                    };
                    SendPredefinedEvent(param, EventMode.GetRankings);
                }
                HandleCallback(cb, callback);
            });
        }

        /// <summary>
        /// Get leaderboard's ranking list for specific cycle
        /// </summary>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="cycleId">The id of leaderboard cycle</param>
        /// <param name="callback">Returns a Result that contains LeaderboardRankingResult via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        public void GetRankingsByCycle(string leaderboardCode, string cycleId,
            ResultCallback<LeaderboardRankingResult> callback, int offset = 0, int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            GetRankingsByCycle(leaderboardCode, cycleId, new GetRankingsByCycleOptionalParameters()
            {
                Offset = offset,
                Limit = limit
            }, callback);
        }
        
        internal void GetRankingsByCycle(string leaderboardCode, string cycleId, GetRankingsByCycleOptionalParameters optionalParameters, ResultCallback<LeaderboardRankingResult> callback)
        {
            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetRankingsByCycle(leaderboardCode, cycleId, optionalParameters, cb =>
            {
                if (!cb.IsError && cb.Value != null)
                {
                    var param = new LeaderboardPredefinedParameters()
                    {
                        LeaderboardCode = leaderboardCode,
                        CycleId = cycleId,
                        UserId = session.UserId
                    };
                    SendPredefinedEvent(param, EventMode.GetRankingByCycleId);
                }
                HandleCallback(cb, callback);
            });
        }
        
        /// <summary>
        /// Get user's ranking from leaderboard with additional key. The additional key will be
        /// suffixed to the userId to access multi level user ranking, such as character ranking. 
        /// </summary>
        /// <param name="userId">The id of the user</param>
        /// <param name="leaderboardCode">The id of the leaderboard</param>
        /// <param name="callback">Returns a Result that contains LeaderboardRankingResult via callback when completed</param>
        public void GetUserRankingV3( string userId
            , string leaderboardCode
            , ResultCallback<UserRankingDataV3> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            GetUserRankingV3(userId, leaderboardCode, optionalParameters: null, callback);
        }
        
        internal void GetUserRankingV3(string userId, string leaderboardCode, GetUserRankingV3OptionalParameters optionalParameters, ResultCallback<UserRankingDataV3> callback)
        {
            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserRankingV3(
                leaderboardCode,
                userId,
                optionalParameters,
                cb =>
                {
                    if (!cb.IsError && cb.Value != null)
                    {
                        var param = new LeaderboardPredefinedParameters()
                        {
                            LeaderboardCode = leaderboardCode, UserId = userId
                        };
                        SendPredefinedEvent(param, EventMode.GetUserRanking);
                    }

                    HandleCallback(cb, callback);
                });
        }

        #region PredefinedEvents
        private enum EventMode
        {
            GetRankings,
            GetUserRanking,
            GetLeaderboards,
            GetRankingByCycleId,
            GetUsersRankings
        }

        private IAccelByteTelemetryPayload CreatePayload(LeaderboardPredefinedParameters parameters, EventMode mode)
        {
            IAccelByteTelemetryPayload payload = null;

            switch (mode)
            {
                case EventMode.GetRankings:
                    payload = new PredefinedLeaderboardGetRankingsPayload(parameters.LeaderboardCode, parameters.UserId);
                    break;

                case EventMode.GetUserRanking:
                    payload = new PredefinedLeaderboardGetUserRankingPayload(parameters.LeaderboardCode, parameters.UserId);
                    break;

                case EventMode.GetLeaderboards:
                    payload = new PredefinedLeaderboardGetLeaderboardsPayload(parameters.UserId);
                    break;

                case EventMode.GetRankingByCycleId:
                    payload = new PredefinedLeaderboardGetRankingByCycleIdPayload(parameters.LeaderboardCode, parameters.UserId, parameters.CycleId);
                    break;

                case EventMode.GetUsersRankings:
                    payload = new PredefinedLeaderboardGetUserRankingsPayload(parameters.LeaderboardCode, parameters.UserIds, parameters.RequesterUserId);
                    break;
            }

            return payload;
        }

        private void SendPredefinedEvent(LeaderboardPredefinedParameters parameters, EventMode mode)
        {
            IAccelByteTelemetryPayload payload = CreatePayload(parameters, mode);
            SendPredefinedEvent(payload);
        }

        private void SendPredefinedEvent(IAccelByteTelemetryPayload payload)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (payload == null)
            {
                return;
            }

            if (predefinedEventScheduler == null)
            {
                return;
            }

            AccelByteTelemetryEvent predefinedEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(predefinedEvent, null);
        }

        private void HandleCallback<T>(Result<T> result, ResultCallback<T> callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }

        private void HandleCallback(Result result, ResultCallback callback)
        {
            if (result.IsError)
            {
                callback?.TryError(result.Error);
                return;
            }

            callback?.Try(result);
        }

        private struct LeaderboardPredefinedParameters
        {
            public string LeaderboardCode;
            public string CycleId;
            public string UserId;
            public string[] UserIds;
            public string RequesterUserId;
        }

        #endregion
    }
}
