// Copyright (c) 2019 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Statistic
    {
        private readonly StatisticApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal Statistic(StatisticApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "Can not construct Statistic manager; api is null!");
            Assert.IsNotNull(session, "Can not construct Statistic manager; session parameter can not be null");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "Can not construct Statistic manager; ns paramater couldn't be empty");
            Assert.IsNotNull(coroutineRunner, "Can not construct Statistic manager; coroutineRunner is null!");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get All StatItems of specified user
        /// </summary>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void GetAllUserStatItems(ResultCallback<StatItemPagingSlicedResult> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserStatItems(this.@namespace, this.session.UserId, this.session.AuthorizationToken, null, null, callback));
        }

        /// <summary>
        /// Get All StatItems of specified user search by statCode(s)
        /// </summary>
        /// <param name="statCodes">One or more statCode(s) that about to get</param>
        /// <param name="callback">Returns an array of statItemInfo via callback when completed</param>
        public void GetUserStatItemsByStatCodes(ICollection<string> statCodes, ResultCallback<StatItemPagingSlicedResult> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserStatItems(this.@namespace, this.session.UserId, this.session.AuthorizationToken, statCodes, null, callback));
        }

        /// <summary>
        /// Get All StatItems of specified user search by tag(s)
        /// </summary>
        /// <param name="tag">One or more tag(s) that about to get</param>
        /// <param name="callback">Returns an array of statItemInfo via callback when completed</param>
        public void GetUserStatItemsByTags(ICollection<string> tags, ResultCallback<StatItemPagingSlicedResult> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetUserStatItems(this.@namespace, this.session.UserId, this.session.AuthorizationToken, null, tags, callback));
        }

        /// <summary>
        /// Bulk update statItem(s) by profileId and statCode
        /// </summary>
        /// <param name="data">Consist of one or more profileId, statCode, and value to update</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void BulkAddStatItemValue(BulkUserStatItemInc[] data, ResultCallback<BulkStatItemOperationResult[]> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.BulkAddStatItemValue(this.@namespace, data, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Bulk update statItem(s) by userId, and statCode
        /// </summary>
        /// <param name="data">Consist of one or more statCode and value to update</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void BulkAddUserStatItemValue(BulkStatItemInc[] data, ResultCallback<BulkStatItemOperationResult[]> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.BulkAddUserStatItemValue(this.@namespace, this.session.UserId, data, this.session.AuthorizationToken, callback));
        }

        /// <summary>
        /// Update a statItem from specific userId and statCode
        /// </summary>
        /// <param name="statCode">StatCode to update</param>
        /// <param name="inc">Value to be added to the statItem</param>
        /// <param name="callback">Returns an array of StatItemIncResult via callback when completed</param>
        public void AddUserStatItemValue(string statCode, float inc, ResultCallback<StatItemIncResult> callback)
        {
            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.AddUserStatItemValue(this.@namespace, this.session.UserId, statCode, inc, this.session.AuthorizationToken, callback));
        }
    }
}