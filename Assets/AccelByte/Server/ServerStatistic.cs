// Copyright (c) 2020 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerStatistic
    {
        private readonly ServerStatisticApi api;
        private readonly IServerSession session;
        private readonly string namespace_;
        private readonly CoroutineRunner coroutineRunner;

        internal ServerStatistic(ServerStatisticApi api, IServerSession session, string namespace_,
            CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can not construct Statistic manager; api is null!");
            Assert.IsNotNull(session, "Can not construct Statistic manager; session parameter can not be null");
            Assert.IsFalse(
                string.IsNullOrEmpty(namespace_),
                "Can not construct Statistic manager; ns paramater couldn't be empty");

            Assert.IsNotNull(coroutineRunner, "Can not construct Statistic manager; coroutineRunner is null!");

            this.api = api;
            this.session = session;
            this.namespace_ = namespace_;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Create stat items of a user. Before a user can have any data in a stat item, he/she needs to have that stat item created.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statItems">List of statCodes to be created for a user</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void CreateUserStatItems(string userId, CreateStatItemRequest[] statItems,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.CreateUserStatItems(
                    this.namespace_,
                    userId,
                    this.session.AuthorizationToken,
                    statItems,
                    callback));
        }

        /// <summary>
        /// Get all stat items of a user.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void GetAllUserStatItems(string userId, ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserStatItems(
                    this.namespace_,
                    userId,
                    this.session.AuthorizationToken,
                    null,
                    null,
                    callback));
        }

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statCodes">List of statCodes that will be included in the result</param>
        /// <param name="tags">List of tags that will be included in the result</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void GetUserStatItems(string userId, ICollection<string> statCodes, ICollection<string> tags,
            ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserStatItems(
                    this.namespace_,
                    userId,
                    this.session.AuthorizationToken,
                    statCodes,
                    tags,
                    callback));
        }

        /// <summary>
        /// Increment stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="increments">Consist of one or more statCode and value to update</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void IncrementUserStatItems(string userId, StatItemIncrement[] increments,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.IncrementUserStatItems(
                    this.namespace_,
                    userId,
                    increments,
                    this.session.AuthorizationToken,
                    callback));
        }

        /// <summary>
        /// Increment stat items for many users
        /// </summary>
        /// <param name="increments">Consist of one or more statCode and value to update</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void IncrementManyUsersStatItems(UserStatItemIncrement[] increments,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.IncrementManyUsersStatItems(
                    this.namespace_,
                    increments,
                    this.session.AuthorizationToken,
                    callback));
        }

         /// <summary>
        /// Reset stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="resets">Consist of one or more statCode</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void ResetUserStatItems(string userId, StatItemReset[] resets,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ResetUserStatItems(
                    this.namespace_,
                    userId,
                    resets,
                    this.session.AuthorizationToken,
                    callback));
        }

        /// <summary>
        /// Reset stat items for many users
        /// </summary>
        /// <param name="resets">Consist of one or more userId and statCode to reset</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void ResetManyUsersStatItems(UserStatItemReset[] resets,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.ResetManyUsersStatItems(
                    this.namespace_,
                    resets,
                    this.session.AuthorizationToken,
                    callback));
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
        public void UpdateUserStatItems(string userId, StatItemUpdate[] updates,
            ResultCallback<StatItemOperationResult[]> callback)
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
        public void UpdateUserStatItems(string userId, string additionalKey, StatItemUpdate[] updates,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateUserStatItems(
                    this.namespace_,
                    userId,
                    additionalKey,
                    updates,
                    this.session.AuthorizationToken,
                    callback));
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
        public void UpdateManyUsersStatItems(UserStatItemUpdate[] updates,
            ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.UpdateManyUsersStatItems(
                    this.namespace_,
                    updates,
                    this.session.AuthorizationToken,
                    callback));
        }
    }
}