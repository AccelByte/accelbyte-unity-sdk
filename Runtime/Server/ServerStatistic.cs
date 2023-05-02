// Copyright (c) 2020 - 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerStatistic : WrapperBase
    {
        private readonly ServerStatisticApi api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        internal ServerStatistic( ServerStatisticApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "Cannot construct Statistic manager; inApi is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct Statistic manager; inCoroutineRunner is null!");

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
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it")]
        internal ServerStatistic( ServerStatisticApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner)
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Create stat items of a user. Before a user can have any data in a stat item, he/she needs to have that stat item created.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statItems">List of statCodes to be created for a user</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void CreateUserStatItems( string userId
            , CreateStatItemRequest[] statItems
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CreateUserStatItems(userId, statItems, callback));
        }

        /// <summary>
        /// Get all stat items of a user.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional, default = 20) </param>
        /// <param name="sortBy">The sorting method of item on page (optional, default = updated at and ascending) </param>
        public void GetAllUserStatItems( string userId
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
                api.GetUserStatItems(userId, null, null, callback, offset, limit, sortBy));
        }

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statCodes">List of statCodes that will be included in the result</param>
        /// <param name="tags">List of tags that will be included in the result</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0)</param>
        /// <param name="limit">The limit of item on page (optional, default = 20) </param>
        /// <param name="sortBy">The sorting method of item on page (optional, default = updated at and ascending) </param>
        public void GetUserStatItems( string userId
            , ICollection<string> statCodes
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
                api.GetUserStatItems(userId, statCodes, tags, callback, offset, limit, sortBy));
        }

        /// <summary>
        /// Increment stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="increments">Consist of one or more statCode and value to update</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void IncrementUserStatItems( string userId
            , StatItemIncrement[] increments
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.IncrementUserStatItems(userId, increments, callback));
        }

        /// <summary>
        /// Increment stat items for many users
        /// </summary>
        /// <param name="increments">Consist of one or more statCode and value to update</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void IncrementManyUsersStatItems( UserStatItemIncrement[] increments
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.IncrementManyUsersStatItems(increments, callback));
        }

         /// <summary>
        /// Reset stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="resets">Consist of one or more statCode</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void ResetUserStatItems( string userId
            , StatItemReset[] resets
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ResetUserStatItems(userId, resets, callback));
        }

        /// <summary>
        /// Reset stat items for many users
        /// </summary>
        /// <param name="resets">Consist of one or more userId and statCode to reset</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void ResetManyUsersStatItems( UserStatItemReset[] resets
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.ResetManyUsersStatItems(resets, callback));
        }

        /// <summary>
        /// Update stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="updates">Consist of one or more statCode with its udpate value and update strategy.
        ///     OVERRIDE update strategy means it will replace the previous statCode value with the new value.
        ///     INCREMENT update strategy with positive value means it will increase the previous statCode value.
        ///     INCREMENT update strategy with negative value means it will decrease the previous statCode value.
        ///     MAX update strategy means it will replace the previous statCode value with the new value if it's larger than the previous statCode value. 
        ///     MIN update strategy means it will replace the previous statCode value with the new value if it's lower than the previous statCode value. </param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void UpdateUserStatItems( string userId
            , StatItemUpdate[] updates
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            UpdateUserStatItems(userId, "", updates, callback);
        }

        /// <summary>
        /// Update stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="additionalKey">To identify multi level user statItem, such as character</param>
        /// <param name="updates">Consist of one or more statCode with its udpate value and update strategy.
        ///     OVERRIDE update strategy means it will replace the previous statCode value with the new value.
        ///     INCREMENT update strategy with positive value means it will increase the previous statCode value.
        ///     INCREMENT update strategy with negative value means it will decrease the previous statCode value.
        ///     MAX update strategy means it will replace the previous statCode value with the new value if it's larger than the previous statCode value. 
        ///     MIN update strategy means it will replace the previous statCode value with the new value if it's lower than the previous statCode value. </param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void UpdateUserStatItems( string userId
            , string additionalKey
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
                api.UpdateUserStatItems(userId, additionalKey, updates, callback));
        }

        /// <summary>
        /// Update stat items for many users
        /// </summary>
        /// <param name="updates">Consist of one or more userId and statCode with its udpate value and update strategy.
        ///     OVERRIDE update strategy means it will replace the previous statCode value with the new value.
        ///     INCREMENT update strategy with positive value means it will increase the previous statCode value.
        ///     INCREMENT update strategy with negative value means it will decrease the previous statCode value.
        ///     MAX update strategy means it will replace the previous statCode value with the new value if it's larger than the previous statCode value. 
        ///     MIN update strategy means it will replace the previous statCode value with the new value if it's lower than the previous statCode value. </param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void UpdateManyUsersStatItems( UserStatItemUpdate[] updates
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateManyUsersStatItems(updates, callback));
        }

        /// <summary>
        /// Bulk fetch multiple user's stat item values for a given namespace and statCode.
        /// If stat item does not exist, will return default value.
        /// </summary>
        /// <param name="statCode">This is the StatCode that will be stored in the slot.</param>
        /// <param name="userIds"> This is the UserId array that will be stored in the slot.</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of FetchUser via callback when completed</param>
        public void BulkFetchUserStatItemValues(string statCode
           , string[] userIds
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
                api.BulkFetchUserStatItemValues(statCode, userIds, additionalKey, callback));
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
        /// Bulk update multiple user's statitems value with specific update strategy.
        /// </summary>
        /// <param name="bulkUpdateMultipleUserStatItem">This is the BulkUpdateMultipleUserStatItem array that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemsResponse via callback when completed</param>
        public void BulkUpdateMultipleUserStatItemsValue(UpdateUserStatItem[] bulkUpdateMultipleUserStatItem
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkUpdateMultipleUserStatItemsValue(bulkUpdateMultipleUserStatItem, callback));
        }

        /// <summary>
        /// Bulk reset user's statitem values for given namespace and user.
        /// </summary>
        /// <param name="userId">This is the UserId that will be stored in the slot.</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="bulkUserStatItems">This is the BulkUserStatItem array that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemsResponse via callback when completed</param>
        public void BulkResetUserStatItemsValues(string userId
            , string additionalKey
            , UserStatItem[] bulkUserStatItems
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkResetUserStatItemsValues(userId, additionalKey, bulkUserStatItems, callback));
        }

        /// <summary>
        /// Bulk update user's statitems value for given namespace and user with specific update strategy.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="bulkUpdateUserStatItem">This is the BulkUpdateUserStatItem array that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemsResponse via callback when completed</param>
        public void BulkUpdateUserStatItemValue(string userId
            , string additionalKey
            , UpdateUserStatItemWithStatCode[] bulkUpdateUserStatItem
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.BulkUpdateUserStatItemValue(userId, additionalKey, bulkUpdateUserStatItem, callback));
        }

        /// <summary>
        /// Update user's statitem value for a given namespace and user with a certain update strategy.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statCode">StatCode</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="updateUserStatItem">This is the FAccelByteModelsUpdateUserStatItem that will be stored in the slot.
        /// <param name="callback">Returns an array of UpdateUserStatItemValueResponse via callback when completed</param>
        public void UpdateUserStatItemValue(string userId, string statCode
            , string additionalKey
            , UpdateUserStatItem updateUserStatItem
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.UpdateUserStatItemValue(userId, statCode, additionalKey, updateUserStatItem, callback));
        }

        /// <summary>
        /// Delete user's stat items for given namespace, statCode, and user Id.
        ///     If query param additionalKey is provided, it will delete user stat items of specific key (i.e. characterName).
        ///     Otherwise, it will delete all stat items related to the user Id.
        /// </summary>
        /// <param name="userId">This is the UserId that will be stored in the slot.</param>
        /// <param name="statCode">This is the StatCode that will be stored in the slot.</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="callback">Returns via callback when completed</param>
        public void DeleteUserStatItems(string userId, string statCode
           , string additionalKey
           , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.DeleteUserStatItems(userId, statCode, additionalKey, callback));
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
                    callback)
            );
        }

        /// <summary>
        /// Get list of statistic cycle config
        /// </summary>
        /// <param name="callback">Returns PagedStatCycleConfigs via callback</param>
        /// <param name="type">Statistic cycle type</param>
        /// <param name="status">Statistic cycle status</param>
        /// <param name="offset">
        /// Offset of the list that has been sliced based on Limit parameter (optional, default = 0)
        /// </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        public void GetListStatCycleConfigs(ResultCallback<PagedStatCycleConfigs> callback,
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

            coroutineRunner.Run(api.GetListStatCycleConfigs(type, status, callback, offset, limit));
        }
    }
}