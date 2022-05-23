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
        public void GetAllUserStatItems( string userId
            , ResultCallback<PagedStatItems> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserStatItems(userId, null, null, callback));
        }

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statCodes">List of statCodes that will be included in the result</param>
        /// <param name="tags">List of tags that will be included in the result</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void GetUserStatItems( string userId
            , ICollection<string> statCodes
            , ICollection<string> tags
            , ResultCallback<PagedStatItems> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetUserStatItems(userId, statCodes, tags, callback));
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
    }
}