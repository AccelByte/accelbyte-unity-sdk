﻿// Copyright (c) 2019 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using AccelByte.Utils;
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateUserStatItems(
                    session.UserId,
                    session.AuthorizationToken,
                    statItems,
                    callback));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserStatItems(
                    session.UserId,
                    session.AuthorizationToken,
                    null,
                    null,
                    callback,
                    offset,
                    limit,
                    sortBy));
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
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        public void GetUserStatItems(GetUserStatItemsOptionalParam optionalParam, ResultCallback<PagedStatItems> callback)
        {
            ICollection<string> inStatCode = null;
            ICollection<string> inTags = null;
            int offset;
            int limit;
            StatisticSortBy sortBy;

            if (optionalParam != null)
            {
                if (optionalParam.StatCodes != null)
                {
                    inStatCode = optionalParam.StatCodes;
                }
                if (optionalParam.Tags != null)
                {
                    inTags = optionalParam.Tags;
                }

                offset = optionalParam.Offset > 0 ? optionalParam.Offset : 0;
                limit = optionalParam.Limit > 0 ? optionalParam.Limit : 0;

                sortBy = optionalParam.sortBy;
            }
            else
            {
                var optParam = new GetUserStatItemsOptionalParam();
                offset = optParam.Offset;
                limit = optParam.Limit;
                sortBy = optParam.sortBy;
            }
            
            GetUserStatItems(statCodes: inStatCode
                , tags: inTags
                , callback: callback
                , offset: offset
                , limit: limit
                , sortBy: sortBy);
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserStatItems(
                    session.UserId,
                    session.AuthorizationToken,
                    statCodes,
                    tags,
                    callback,
                    offset,
                    limit,
                    sortBy));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.IncrementUserStatItems(
                    session.UserId,
                    increments,
                    session.AuthorizationToken,
                    callback));
        }

        /// <summary>
        /// Reset stat items for a user
        /// </summary>
        /// <param name="resets">Consist of one or more statCode.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void ResetUserStatItems( StatItemReset[] resets
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ResetUserStatItems(
                    session.UserId,
                    resets,
                    session.AuthorizationToken,
                    callback));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateUserStatItems(
                    session.UserId,
                    additionalKey,
                    updates,
                    session.AuthorizationToken,
                    callback));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ListUserStatItems(
                    session.UserId,
                    statCodes,
                    tags,
                    additionalKey,
                    session.AuthorizationToken,
                    callback));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateUserStatItemsValue(
                    session.UserId,
                    statCode, 
                    additionalKey,
                    updateUserStatItem,
                    session.AuthorizationToken,
                    callback));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkFetchStatItemsValue(statCode, userIds, callback));
        }

        /// <summary>
        /// Get global statistic item by statistic code.
        /// </summary>
        /// <param name="statCode">StatCode.</param>
        /// <param name="callback">Returns GlobalStatItem via callback when completed</param>
        public void GetGlobalStatItemsByStatCode(string statCode, ResultCallback<GlobalStatItem> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            
            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetGlobalStatItemsByStatCode(
                    statCode, 
                    session.AuthorizationToken, 
                    callback)
                );
        }
        
        /// <summary>
        /// Get statistic cycle configuration data.
        /// </summary>
        /// <param name="cycleId">The id of the config data</param>
        /// <param name="callback">Returns StatCycleConfig via callback</param>
        public void GetStatCycleConfig(string cycleId, ResultCallback<StatCycleConfig> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(cycleId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetCycleIdInvalidMessage(cycleId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetStatCycleConfig(cycleId, session.AuthorizationToken, callback));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetListStatCycleConfigs(
                type, 
                status,
                session.AuthorizationToken, 
                callback, 
                offset, 
                limit));
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
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(cycleId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetCycleIdInvalidMessage(cycleId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            if (statCodes == null)
            {
                statCodes = Array.Empty<string>();
            }

            coroutineRunner.Run(api.GetListUserStatCycleItem(cycleId, statCodes, this.session.UserId,
                this.session.AuthorizationToken, callback, offset, limit));
        }

        /// <summary>
        /// Get user's own statistic item
        /// </summary>
        /// <param name="callback">Returns PagedStatItems via callback</param>
        public void GetMyStatItems(ResultCallback<PagedStatItems> callback)
        {
            GetMyStatItems(callback: callback, optionalParam: null);
        }

        /// <summary>
        /// Get user's own statistic item
        /// </summary>
        /// <param name="optionalParam">Optional Parameters to be sent</param>
        /// <param name="callback">Returns PagedStatItems via callback</param>
        public void GetMyStatItems(GetMyStatItemsOptionalParam optionalParam, ResultCallback<PagedStatItems> callback)
        {
            IEnumerable<string> inStatCodes = null;
            IEnumerable<string> inTags = null;
            int limit;
            int offset;
            if (optionalParam != null)
            {
                if (optionalParam.StatCodes != null)
                {
                    inStatCodes = optionalParam.StatCodes;
                }
                if (optionalParam.Tags != null)
                {
                    inTags = optionalParam.Tags;
                }

                limit = optionalParam.Limit > 0 ? optionalParam.Limit: 0;
                offset = optionalParam.Offset > 0 ? optionalParam.Offset: 0;
            }
            else
            {
                var defaultOptionalParam = new GetMyStatItemsOptionalParam();
                limit = defaultOptionalParam.Limit;
                offset = defaultOptionalParam.Offset;
            }
            GetMyStatItems(inStatCodes, inTags, callback, limit, offset);
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetMyStatItems(statCodes, tags, session.AuthorizationToken, callback, limit, offset));
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
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(api.GetMyStatItemValues(statCodes, tags, additionalKey,session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Get user's own statistic cycle item
        /// </summary>
        /// <param name="cycleId">The cycle id to which the stat item belong</param>
        /// <param name="callback">Returns PagedStatCycleItem via callback</param>
        public void GetMyStatCycleItems(string cycleId, ResultCallback<PagedStatCycleItem> callback)
        {
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
            IEnumerable<string> inStatCodes = null;
            int limit;
            int offset;
            if (optionalParam != null)
            {
                if (optionalParam.StatCodes != null)
                {
                    inStatCodes = optionalParam.StatCodes;
                }

                limit = optionalParam.Limit > 0 ? optionalParam.Limit : 0;
                offset = optionalParam.Offset > 0 ? optionalParam.Offset : 0;
            }
            else
            {
                var optParam = new GetMyStatCycleItemsOptionalParam();
                limit = optParam.Limit;
                offset = optParam.Offset;
            }

            GetMyStatCycleItems(cycleId, inStatCodes, callback, limit, offset);
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
            Report.GetFunctionLog(GetType().Name);

            if (!ValidateAccelByteId(cycleId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetCycleIdInvalidMessage(cycleId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetMyStatCycleItems(cycleId, statCodes, session.AuthorizationToken, limit, offset, callback));
        }
    }
}