// Copyright (c) 2019 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using System.Linq;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Statistic : WrapperBase
    {
        private readonly StatisticApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Statistic( StatisticApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "Cannot construct Statistic manager; api is null!");
            Assert.IsNotNull(inSession, "Cannot construct Statistic manager; session parameter can not be null");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct Statistic manager; coroutineRunner is null!");

            this.api = inApi;
            this.session = inSession;
            this.coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal Statistic( StatisticApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Create stat items of a user. Before a user can have any data in a stat item, he/she needs to have that stat item created.
        /// </summary>
        /// <param name="statItems">List of statCodes to be created for a user</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void CreateUserStatItems(CreateStatItemRequest[] statItems
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateUserStatItems(
                statItems
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Create stat items of a user. Before a user can have any data in a stat item, he/she needs to have that stat item created.
        /// </summary>
        /// <param name="statItems">List of statCodes to be created for a user</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        internal void CreateUserStatItems(CreateStatItemRequest[] statItems
            , CreateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.CreateUserStatItems(
                session.UserId
                , session.AuthorizationToken
                , statItems
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Get all stat items of a user.
        /// </summary>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional, default = 20) </param>
        /// <param name="sortBy">The sorting method of item on page (optional, default = updated at and ascending) </param>
        public void GetAllUserStatItems( ResultCallback<PagedStatItems> callback
            , int offset = 0
            , int limit = 20
            , StatisticSortBy sortBy = StatisticSortBy.UpdatedAtAsc)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var optionalParameters = new GetAllUserStatItemsOptionalParam()
            {
                Limit = limit,
                Offset = offset,
                sortBy = sortBy
            };

            GetAllUserStatItems(optionalParameters, callback);
        }
        
        internal void GetAllUserStatItems(
            GetAllUserStatItemsOptionalParam optionalParam
            , ResultCallback<PagedStatItems> callback)
        {
            var getUserStatItemsOptionalParameters = new GetUserStatItemsOptionalParam();
            if (optionalParam != null)
            {
                getUserStatItemsOptionalParameters.Logger = optionalParam.Logger;
                getUserStatItemsOptionalParameters.ApiTracker = optionalParam.ApiTracker;
                getUserStatItemsOptionalParameters.Limit = optionalParam.Limit;
                getUserStatItemsOptionalParameters.Offset = optionalParam.Offset;
                getUserStatItemsOptionalParameters.sortBy = optionalParam.sortBy;
            }

            GetUserStatItems(getUserStatItemsOptionalParameters, callback);
        }

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void GetUserStatItems(ResultCallback<PagedStatItems> callback)
        {
            GetUserStatItems(callback: callback, optionalParam: null);
        }

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="optionalParam">Optional parameters to be sent</param>
        /// <param name="callback">>Returns all profile's StatItems via callback when completed</param>
        public void GetUserStatItems(GetUserStatItemsOptionalParam optionalParam, ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParam?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetUserStatItems(session.UserId, session.AuthorizationToken, optionalParam, callback);
        }      

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="statCodes">List of statCodes that will be included in the result</param>
        /// <param name="tags">List of tags that will be included in the result</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional, default = 20) </param>
        /// <param name="sortBy">The sorting method of item on page (optional, default = updated at and ascending) </param>
        public void GetUserStatItems( ICollection<string> statCodes
            , ICollection<string> tags
            , ResultCallback<PagedStatItems> callback
            , int offset = 0
            , int limit = 20
            , StatisticSortBy sortBy = StatisticSortBy.UpdatedAtAsc)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserStatItemsOptionalParam()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset,
                sortBy = sortBy,
                StatCodes = statCodes?.ToArray(),
                Tags = tags?.ToArray()
            };

            GetUserStatItems(optionalParameters, callback);
        }

        /// <summary>
        /// Update stat items for a users
        /// </summary>
        /// <param name="increments">Consist of one or more statCode with its increament value.
        ///     Positive increament value means it will increase the previous statCode value.
        ///     Negative increament value means it will decrease the previous statCode value. </param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void IncrementUserStatItems( StatItemIncrement[] increments
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new IncrementUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            IncrementUserStatItems(increments, optionalParameters, callback);
        }

        /// <summary>
        /// Update stat items for a users
        /// </summary>
        /// <param name="increments">Consist of one or more statCode with its increament value.
        ///     Positive increament value means it will increase the previous statCode value.
        ///     Negative increament value means it will decrease the previous statCode value. </param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void IncrementUserStatItems(StatItemIncrement[] increments
            , IncrementUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.IncrementUserStatItems(
                session.UserId
                , increments
                , session.AuthorizationToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Reset stat items for a user
        /// </summary>
        /// <param name="resets">Consist of one or more statCode.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void ResetUserStatItems( StatItemReset[] resets
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ResetUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ResetUserStatItems(
                resets
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Reset stat items for a user
        /// </summary>
        /// <param name="resets">Consist of one or more statCode.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void ResetUserStatItems(StatItemReset[] resets
            , ResetUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ResetUserStatItems(
                session.UserId
                , resets
                , session.AuthorizationToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Update stat items with the specified update strategy for a user
        /// </summary>
        /// <param name="updates">Consist of one or more statCode with its udpate value and update strategy.
        ///     OVERRIDE update strategy means it will replace the previous statCode value with the new value.
        ///     INCREMENT update strategy with positive value means it will increase the previous statCode value.
        ///     INCREMENT update strategy with negative value means it will decrease the previous statCode value.
        ///     MAX update strategy means it will replace the previous statCode value with the new value if it's larger than the previous statCode value. 
        ///     MIN update strategy means it will replace the previous statCode value with the new value if it's lower than the previous statCode value. </param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void UpdateUserStatItems( StatItemUpdate[] updates
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            UpdateUserStatItems("", updates, callback);
        }

        /// <summary> 
        /// Public bulk update user's statitems value for given namespace and user with specific update strategy. 
        /// </summary>
        /// <param name="additionalKey">To identify multi level user statItem, such as character</param>
        /// <param name="updates">Consist of one or more statCode with its udpate value and update strategy.
        ///     OVERRIDE update strategy means it will replace the previous statCode value with the new value.
        ///     INCREMENT update strategy with positive value means it will increase the previous statCode value.
        ///     INCREMENT update strategy with negative value means it will decrease the previous statCode value.
        ///     MAX update strategy means it will replace the previous statCode value with the new value if it's larger than the previous statCode value. 
        ///     MIN update strategy means it will replace the previous statCode value with the new value if it's lower than the previous statCode value. </param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void UpdateUserStatItems( string additionalKey
            , StatItemUpdate[] updates
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemsOptionalParameters()
            {
                AdditionalKey = additionalKey,
                Logger = SharedMemory?.Logger
            };

            UpdateUserStatItems(updates, optionalParameters, callback);
        }

        /// <summary> 
        /// Public bulk update user's statitems value for given namespace and user with specific update strategy. 
        /// </summary>
        /// <param name="updates">Consist of one or more statCode with its udpate value and update strategy.
        ///     OVERRIDE update strategy means it will replace the previous statCode value with the new value.
        ///     INCREMENT update strategy with positive value means it will increase the previous statCode value.
        ///     INCREMENT update strategy with negative value means it will decrease the previous statCode value.
        ///     MAX update strategy means it will replace the previous statCode value with the new value if it's larger than the previous statCode value. 
        ///     MIN update strategy means it will replace the previous statCode value with the new value if it's lower than the previous statCode value. </param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void UpdateUserStatItems(StatItemUpdate[] updates
            , UpdateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateUserStatItems(
                session.UserId,
                updates,
                session.AuthorizationToken,
                optionalParameters,
                callback);
        }

        /// <summary>
        /// Public list all statItems of user.
        /// NOTE:
        ///     If stat code does not exist, will ignore this stat code.
        ///     If stat item does not exist, will return default value
        /// </summary>
        /// <param name="statCodes">StatCodes</param>
        /// <param name="tags">This is the Tag array that will be stored in the slot.</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of FetchUser via callback when completed</param>
        public void ListUserStatItems(string[] statCodes
            , string[] tags
            , string additionalKey
            , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ListUserStatItemsOptionalParameters()
            {
                AdditionalKey = additionalKey,
                Logger = SharedMemory?.Logger,
                StatCodes = statCodes,
                Tags = tags
            };

            ListUserStatItems(optionalParameters, callback);
        }

        /// <summary>
        /// Public list all statItems of user.
        /// NOTE:
        ///     If stat code does not exist, will ignore this stat code.
        ///     If stat item does not exist, will return default value
        /// </summary>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of FetchUser via callback when completed</param>
        internal void ListUserStatItems(ListUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.ListUserStatItems(
                session.UserId
                , session.AuthorizationToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Public update user's statitem value for a given namespace and user with a certain update strategy.
        /// </summary>
        /// <param name="statCode">StatCode.</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="updateUserStatItem">This is the UpdateUserStatItem that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemValueResponse via callback when completed</param>
        public void UpdateUserStatItemsValue(string statCode
            , string additionalKey
            , PublicUpdateUserStatItem updateUserStatItem
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemsValueOptionalParameters()
            {
                AdditionalKey = additionalKey,
                Logger = SharedMemory?.Logger
            };

            UpdateUserStatItemsValue(statCode, updateUserStatItem, optionalParameters, callback);
        }

        /// <summary>
        /// Public update user's statitem value for a given namespace and user with a certain update strategy.
        /// </summary>
        /// <param name="statCode">StatCode.</param>
        /// <param name="updateUserStatItem">This is the UpdateUserStatItem that will be stored in the slot.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemValueResponse via callback when completed</param>
        internal void UpdateUserStatItemsValue(string statCode
            , PublicUpdateUserStatItem updateUserStatItem
            , UpdateUserStatItemsValueOptionalParameters optionalParameters
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.UpdateUserStatItemsValue(
                session.UserId
                , statCode
                , updateUserStatItem
                , session.AuthorizationToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Bulk fetch multiple user's stat item values for a given namespace and statCode.
        /// </summary>
        /// <param name="statCode">This is the StatCode that will be stored in the slot.</param>
        /// <param name="userIds"> This is the UserId array that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of FetchUserStatistic via callback when completed</param>
        public void BulkFetchStatItemsValue(string statCode
           , string[] userIds
           , ResultCallback<FetchUserStatistic> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkFetchStatItemsValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkFetchStatItemsValue(statCode, userIds, optionalParameters, callback);
        }

        /// <summary>
        /// Bulk fetch multiple user's stat item values for a given namespace and statCode.
        /// </summary>
        /// <param name="statCode">This is the StatCode that will be stored in the slot.</param>
        /// <param name="userIds"> This is the UserId array that will be stored in the slot.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of FetchUserStatistic via callback when completed</param>
        internal void BulkFetchStatItemsValue(string statCode
           , string[] userIds
            , BulkFetchStatItemsValueOptionalParameters optionalParameters
           , ResultCallback<FetchUserStatistic> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.BulkFetchStatItemsValue(statCode, userIds, optionalParameters, callback);
        }

        /// <summary>
        /// Get global statistic item by statistic code.
        /// </summary>
        /// <param name="statCode">StatCode.</param>
        /// <param name="callback">Returns GlobalStatItem via callback when completed</param>
        public void GetGlobalStatItemsByStatCode(string statCode, ResultCallback<GlobalStatItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetGlobalStatItemsByStatCodeOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetGlobalStatItemsByStatCode(statCode, optionalParameters, callback);
        }

        /// <summary>
        /// Get global statistic item by statistic code.
        /// </summary>
        /// <param name="statCode">StatCode.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns GlobalStatItem via callback when completed</param>
        internal void GetGlobalStatItemsByStatCode(string statCode, GetGlobalStatItemsByStatCodeOptionalParameters optionalParameters, ResultCallback<GlobalStatItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetGlobalStatItemsByStatCode(
                statCode
                , session.AuthorizationToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Get statistic cycle configuration data.
        /// </summary>
        /// <param name="cycleId">The id of the config data</param>
        /// <param name="callback">Returns StatCycleConfig via callback</param>
        public void GetStatCycleConfig(string cycleId, ResultCallback<StatCycleConfig> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetStatCycleConfigOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            GetStatCycleConfig(cycleId, optionalParameters, callback);
        }

        /// <summary>
        /// Get statistic cycle configuration data.
        /// </summary>
        /// <param name="cycleId">The id of the config data</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns StatCycleConfig via callback</param>
        internal void GetStatCycleConfig(string cycleId, GetStatCycleConfigOptionalParameters optionalParameters, ResultCallback<StatCycleConfig> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(cycleId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetCycleIdInvalidMessage(cycleId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetStatCycleConfig(cycleId, session.AuthorizationToken, optionalParameters, callback);
        }

        /// <summary>
        /// Get the list of statistic cycle configuration data that belongs to the current namespace
        /// </summary>
        /// <param name="callback">Returns PagedStatCycleConfigs via callback</param>
        /// <param name="type">Statistic cycle type</param>
        /// <param name="status">Statistic cycle status</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        public void GetListStatCycleConfigs(
            ResultCallback<PagedStatCycleConfigs> callback,
            StatisticCycleType type = StatisticCycleType.None, 
            StatisticCycleStatus status = StatisticCycleStatus.None,
            int offset = 0,
            int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetListStatCycleConfigsOptionalParameters()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset,
                Status = status,
                Type = type
            };

            GetListStatCycleConfigs(optionalParameters, callback);
        }

        /// <summary>
        /// Get the list of statistic cycle configuration data that belongs to the current namespace
        /// </summary>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns PagedStatCycleConfigs via callback</param>
        internal void GetListStatCycleConfigs(GetListStatCycleConfigsOptionalParameters optionalParameters, ResultCallback<PagedStatCycleConfigs> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetListStatCycleConfigs(
                session.AuthorizationToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Get list of user's statistic cycle item  
        /// </summary>
        /// <param name="cycleId">The cycle id where the statistic item belongs to</param>
        /// <param name="callback">Returns PagedStatCycleItem via callback</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        /// <param name="statCodes">List of specific stat codes to be retrieved. Optional</param>
        public void GetListUserStatCycleItem(
            string cycleId,
            ResultCallback<PagedStatCycleItem> callback,
            int offset = 0,
            int limit = 20,
            string[] statCodes = null)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetListUserStatCycleItemOptionalParameters()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset,
                StatCodes = statCodes
            };

            GetListUserStatCycleItem(cycleId, optionalParameters, callback);
        }

        /// <summary>
        /// Get list of user's statistic cycle item  
        /// </summary>
        /// <param name="cycleId">The cycle id where the statistic item belongs to</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns PagedStatCycleItem via callback</param>
        internal void GetListUserStatCycleItem(
            string cycleId
            , GetListUserStatCycleItemOptionalParameters optionalParameters
            , ResultCallback<PagedStatCycleItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(cycleId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetCycleIdInvalidMessage(cycleId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetListUserStatCycleItem(cycleId
                , session.UserId
                , session.AuthorizationToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Get user's own statistic item
        /// </summary>
        /// <param name="callback">Returns PagedStatItems via callback</param>
        public void GetMyStatItems(ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            GetMyStatItems(callback: callback, optionalParam: null);
        }

        /// <summary>
        /// Get user's own statistic item
        /// </summary>
        /// <param name="optionalParam">Optional Parameters to be sent</param>
        /// <param name="callback">Returns PagedStatItems via callback</param>
        public void GetMyStatItems(GetMyStatItemsOptionalParam optionalParam, ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParam?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetMyStatItems(session.AuthorizationToken, optionalParam, callback);
        }
        
        /// <summary>
        /// Get user's own statistic item
        /// </summary>
        /// <param name="statCodes">Array of statistic codes to be retrieved</param>
        /// <param name="tags">Array of tags which the statistic items to be retrieved have</param>
        /// <param name="callback">Returns PagedStatItems via callback</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        public void GetMyStatItems(
            IEnumerable<string> statCodes,
            IEnumerable<string> tags,
            ResultCallback<PagedStatItems> callback,
            int limit = 20,
            int offset = 0)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMyStatItemsOptionalParam()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset,
                StatCodes = statCodes?.ToArray(),
                Tags = tags?.ToArray()
            };

            GetMyStatItems(optionalParameters, callback);
        }

        /// <summary>
        /// Get user's own statistic value
        /// </summary>
        /// <param name="statCodes">Array of statistic codes of statistic value to be retrieved</param>
        /// <param name="tags">Array of tags that statistic item to be retrieved has</param>
        /// <param name="additionalKey"></param>
        /// <param name="callback">Returns FetchUser[] via callback</param>
        public void GetMyStatItemValues(
            string[] statCodes,
            string[] tags,
            string additionalKey,
            ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMyStatItemValuesOptionalParameters()
            {
                AdditionalKey = additionalKey,
                Logger = SharedMemory?.Logger,
                StatCodes = statCodes,
                Tags = tags
            };

            GetMyStatItemValues(optionalParameters, callback);
        }

        /// <summary>
        /// Get user's own statistic value
        /// </summary>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns FetchUser[] via callback</param>
        internal void GetMyStatItemValues(
            GetMyStatItemValuesOptionalParameters optionalParameters
            , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetMyStatItemValues(session.AuthorizationToken, optionalParameters, callback);
        }

        /// <summary>
        /// Get user's own statistic cycle item
        /// </summary>
        /// <param name="cycleId">The cycle id to which the stat item belong</param>
        /// <param name="callback">Returns PagedStatCycleItem via callback</param>
        public void GetMyStatCycleItems(string cycleId, ResultCallback<PagedStatCycleItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            GetMyStatCycleItems(cycleId: cycleId, callback: callback, optionalParam: null);
        }

        /// <summary>
        /// Get user's own statistic cycle item
        /// </summary>
        /// <param name="cycleId">The cycle id to which the stat item belong</param>
        /// <param name="optionalParam">Optional parameter be sent</param>
        /// <param name="callback">Returns PagedStatCycleItem via callback</param>
        public void GetMyStatCycleItems(string cycleId, GetMyStatCycleItemsOptionalParam optionalParam, 
            ResultCallback<PagedStatCycleItem> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParam?.Logger);

            if (!ValidateAccelByteId(cycleId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetCycleIdInvalidMessage(cycleId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            api.GetMyStatCycleItems(cycleId, session.AuthorizationToken, optionalParam, callback);
        }

        /// <summary>
        /// Get user's own statistic cycle item
        /// </summary>
        /// <param name="cycleId">The cycle id to which the stat item belong</param>
        /// <param name="statCodes">Array of statistic codes for statistic cycle items to be retrieved</param>
        /// <param name="callback">Returns PagedStatCycleItem via callback</param>
        /// <param name="limit">The limit of item on page (optional)</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        public void GetMyStatCycleItems(
            string cycleId,
            IEnumerable<string> statCodes,
            ResultCallback<PagedStatCycleItem> callback,
            int limit = 20,
            int offset = 0
        )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetMyStatCycleItemsOptionalParam()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset,
                StatCodes = statCodes?.ToArray()
            };

            GetMyStatCycleItems(cycleId, optionalParameters, callback);
        }
    }
}