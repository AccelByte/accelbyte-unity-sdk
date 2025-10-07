// Copyright (c) 2020 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using AccelByte.Core;
using AccelByte.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerStatistic : WrapperBase
    {
        internal readonly ServerStatisticApi Api;
        private readonly ISession session;
        private readonly CoroutineRunner coroutineRunner;

        private enum PredefinedUserStatItemMode
        {
            Created,
            Updated,
            Deleted
        }

        [UnityEngine.Scripting.Preserve]
        internal ServerStatistic( ServerStatisticApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "Cannot construct Statistic manager; inApi is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct Statistic manager; inCoroutineRunner is null!");

            Api = inApi;
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
        internal ServerStatistic( ServerStatisticApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner)
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Create stat items of a user.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statItems">List of statCodes to be created for a user</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void CreateUserStatItems( string userId
            , CreateStatItemRequest[] statItems
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new CreateUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            CreateUserStatItems(userId, statItems, optionalParameters, callback);
        }

        /// <summary>
        /// Create stat items of a user.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statItems">List of statCodes to be created for a user</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        internal void CreateUserStatItems(string userId
            , CreateStatItemRequest[] statItems
            , CreateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            List<string> collectedStatCodes = new List<string>();
            foreach (var item in statItems)
            {
                if (item != null && item.statCode != null)
                {
                    collectedStatCodes.Add(item.statCode.ToString());
                }
            }
            SendPredefinedEvent(userId, collectedStatCodes, PredefinedUserStatItemMode.Created);

            Api.CreateUserStatItems(userId, statItems, optionalParameters, callback);
        }

        private void SendPredefinedEvent(string userId, List<string> statCodes, PredefinedUserStatItemMode mode)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler != null)
            {
                IAccelByteTelemetryPayload payload;
                switch (mode)
                {
                    case PredefinedUserStatItemMode.Created:
                        payload = new PredefinedUserStatItemCreatedPayload(userId, statCodes);
                        break;
                    case PredefinedUserStatItemMode.Updated:
                        payload = new PredefinedUserStatItemUpdatedPayload(userId, statCodes);
                        break;
                    case PredefinedUserStatItemMode.Deleted:
                        payload = new PredefinedUserStatItemDeletedPayload(userId, statCodes);
                        break;
                    default:
                        return;
                }

                var userProfileEvent = new AccelByteTelemetryEvent(payload);
                predefinedEventScheduler.SendEvent(userProfileEvent, null);
            }
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new GetUserStatItemsOptionalParam()
            {
                Limit = limit,
                Logger = SharedMemory?.Logger,
                Offset = offset,
                sortBy = sortBy
            };

            GetAllUserStatItems(userId, optionalParameters, callback);
        }

        /// <summary>
        /// Get all stat items of a user.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        internal void GetAllUserStatItems(string userId
            , GetUserStatItemsOptionalParam optionalParameters
            , ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.GetUserStatItems(userId, optionalParameters, callback);
        }

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void GetUserStatItems(string userId, ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            GetUserStatItems(userId:userId, callback: callback, optionalParam: null);
        }

        /// <summary>
        /// Get stat items of a user, filter by statCodes and tags
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="optionalParam">Optional parameters to be sent</param>
        /// <param name="callback">Returns all profile's StatItems via callback when completed</param>
        public void GetUserStatItems(string userId, GetUserStatItemsOptionalParam optionalParam, ResultCallback<PagedStatItems> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParam?.Logger);

            GetAllUserStatItems(userId, optionalParam, callback);
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

            GetUserStatItems(userId, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new IncrementUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            IncrementUserStatItems(userId, increments, optionalParameters, callback);
        }

        /// <summary>
        /// Increment stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="increments">Consist of one or more statCode and value to update</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void IncrementUserStatItems(string userId
            , StatItemIncrement[] increments
            , IncrementUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            List<string> collectedStatCodes = new List<string>();
            foreach (var item in increments)
            {
                if (item != null && item.statCode != null)
                {
                    collectedStatCodes.Add(item.statCode.ToString());
                }
            }
            SendPredefinedEvent(userId, collectedStatCodes, PredefinedUserStatItemMode.Updated);

            Api.IncrementUserStatItems(userId, increments, optionalParameters, callback);
        }

        /// <summary>
        /// Increment stat items for many users
        /// </summary>
        /// <param name="increments">Consist of one or more statCode and value to update</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void IncrementManyUsersStatItems( UserStatItemIncrement[] increments
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new IncrementManyUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            IncrementManyUsersStatItems(increments, optionalParameters, callback);
        }

        /// <summary>
        /// Increment stat items for many users
        /// </summary>
        /// <param name="increments">Consist of one or more statCode and value to update</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void IncrementManyUsersStatItems(UserStatItemIncrement[] increments
            , IncrementManyUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.IncrementManyUsersStatItems(increments, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ResetUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ResetUserStatItems(userId, resets, optionalParameters, callback);
        }

        /// <summary>
        /// Reset stat items for a user
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="resets">Consist of one or more statCode</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void ResetUserStatItems(string userId
            , StatItemReset[] resets
            , ResetUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.ResetUserStatItems(userId, resets, optionalParameters, callback);
        }

        /// <summary>
        /// Reset stat items for many users
        /// </summary>
        /// <param name="resets">Consist of one or more userId and statCode to reset</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        public void ResetManyUsersStatItems( UserStatItemReset[] resets
            , ResultCallback<StatItemOperationResult[]> callback )
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new ResetManyUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            ResetManyUsersStatItems(resets, optionalParameters, callback);
        }

        /// <summary>
        /// Reset stat items for many users
        /// </summary>
        /// <param name="resets">Consist of one or more userId and statCode to reset</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void ResetManyUsersStatItems(UserStatItemReset[] resets
            , ResetManyUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.ResetManyUsersStatItems(resets, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger,
                AdditionalKey = additionalKey
            };

            UpdateUserStatItems(userId, updates, optionalParameters, callback);
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
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void UpdateUserStatItems(string userId
            , StatItemUpdate[] updates
            , UpdateUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            List<string> collectedStatCodes = new List<string>();
            foreach (var item in updates)
            {
                if (item != null && item.statCode != null)
                {
                    collectedStatCodes.Add(item.statCode.ToString());
                }
            }
            SendPredefinedEvent(userId, collectedStatCodes, PredefinedUserStatItemMode.Updated);

            Api.UpdateUserStatItems(userId, updates, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.UpdateManyUsersStatItems(updates, callback));
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
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of BulkStatItemOperationResult via callback when completed</param>
        internal void UpdateManyUsersStatItems(UserStatItemUpdate[] updates
            , UpdateManyUserStatItemsOptionalParameters optionalParameters
            , ResultCallback<StatItemOperationResult[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.UpdateManyUsersStatItems(updates, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkFetchUserStatItemsValueOptionalParameters()
            {
                AdditionalKey = additionalKey,
                Logger = SharedMemory?.Logger
            };

            BulkFetchUserStatItemValues(statCode, userIds, optionalParameters, callback);
        }

        /// <summary>
        /// Bulk fetch multiple user's stat item values for a given namespace and statCode.
        /// If stat item does not exist, will return default value.
        /// </summary>
        /// <param name="statCode">This is the StatCode that will be stored in the slot.</param>
        /// <param name="userIds"> This is the UserId array that will be stored in the slot.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of FetchUser via callback when completed</param>
        internal void BulkFetchUserStatItemValues(string statCode
           , string[] userIds
           , BulkFetchUserStatItemsValueOptionalParameters optionalParameters
           , ResultCallback<FetchUser[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.BulkFetchUserStatItemValues(statCode, userIds, optionalParameters, callback);
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

            Api.BulkFetchStatItemsValue(statCode, userIds, optionalParameters, callback);
        }

        /// <summary>
        /// Bulk update multiple user's statitems value with specific update strategy.
        /// </summary>
        /// <param name="bulkUpdateMultipleUserStatItem">This is the BulkUpdateMultipleUserStatItem array that will be stored in the slot.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemsResponse via callback when completed</param>
        public void BulkUpdateMultipleUserStatItemsValue(UpdateUserStatItem[] bulkUpdateMultipleUserStatItem
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkUpdateMultipleUserStatItemsValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkUpdateMultipleUserStatItemsValue(bulkUpdateMultipleUserStatItem, optionalParameters, callback);
        }

        /// <summary>
        /// Bulk update multiple user's statitems value with specific update strategy.
        /// </summary>
        /// <param name="bulkUpdateMultipleUserStatItem">This is the BulkUpdateMultipleUserStatItem array that will be stored in the slot.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemsResponse via callback when completed</param>
        internal void BulkUpdateMultipleUserStatItemsValue(UpdateUserStatItem[] bulkUpdateMultipleUserStatItem
            , BulkUpdateMultipleUserStatItemsValueOptionalParameters optionalParameters
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.BulkUpdateMultipleUserStatItemsValue(bulkUpdateMultipleUserStatItem, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkResetUserStatItemsValuesOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkResetUserStatItemsValues(userId, additionalKey, bulkUserStatItems, optionalParameters, callback);
        }

        /// <summary>
        /// Bulk reset user's statitem values for given namespace and user.
        /// </summary>
        /// <param name="userId">This is the UserId that will be stored in the slot.</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="bulkUserStatItems">This is the BulkUserStatItem array that will be stored in the slot.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemsResponse via callback when completed</param>
        internal void BulkResetUserStatItemsValues(string userId
            , string additionalKey
            , UserStatItem[] bulkUserStatItems
            , BulkResetUserStatItemsValuesOptionalParameters optionalParameters
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.BulkResetUserStatItemsValues(userId, additionalKey, bulkUserStatItems, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new BulkUpdateUserStatItemValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            BulkUpdateUserStatItemValue(userId, additionalKey, bulkUpdateUserStatItem, optionalParameters, callback);
        }

        /// <summary>
        /// Bulk update user's statitems value for given namespace and user with specific update strategy.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="bulkUpdateUserStatItem">This is the BulkUpdateUserStatItem array that will be stored in the slot.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemsResponse via callback when completed</param>
        internal void BulkUpdateUserStatItemValue(string userId
            , string additionalKey
            , UpdateUserStatItemWithStatCode[] bulkUpdateUserStatItem
            , BulkUpdateUserStatItemValueOptionalParameters optionalParameters
            , ResultCallback<UpdateUserStatItemsResponse[]> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.BulkUpdateUserStatItemValue(userId, additionalKey, bulkUpdateUserStatItem, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new UpdateUserStatItemValueOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            UpdateUserStatItemValue(userId, statCode, additionalKey, updateUserStatItem, optionalParameters, callback);
        }

        /// <summary>
        /// Update user's statitem value for a given namespace and user with a certain update strategy.
        /// </summary>
        /// <param name="userId">UserId of a user</param>
        /// <param name="statCode">StatCode</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="updateUserStatItem">This is the FAccelByteModelsUpdateUserStatItem that will be stored in the slot.
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns an array of UpdateUserStatItemValueResponse via callback when completed</param>
        internal void UpdateUserStatItemValue(string userId, string statCode
            , string additionalKey
            , UpdateUserStatItem updateUserStatItem
            , UpdateUserStatItemValueOptionalParameters optionalParameters
            , ResultCallback<UpdateUserStatItemValueResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.UpdateUserStatItemValue(userId, statCode, additionalKey, updateUserStatItem, optionalParameters, callback);
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
            Report.GetFunctionLog(GetType().Name, logger: SharedMemory?.Logger);

            var optionalParameters = new DeleteUserStatItemsOptionalParameters()
            {
                Logger = SharedMemory?.Logger
            };

            DeleteUserStatItems(userId, statCode, additionalKey, optionalParameters, callback);
        }

        /// <summary>
        /// Delete user's stat items for given namespace, statCode, and user Id.
        ///     If query param additionalKey is provided, it will delete user stat items of specific key (i.e. characterName).
        ///     Otherwise, it will delete all stat items related to the user Id.
        /// </summary>
        /// <param name="userId">This is the UserId that will be stored in the slot.</param>
        /// <param name="statCode">This is the StatCode that will be stored in the slot.</param>
        /// <param name="additionalKey">This is the AdditionalKey that will be stored in the slot.</param>
        /// <param name="optionalParameters">Endpoint optional parameters. Can be null.</param>
        /// <param name="callback">Returns via callback when completed</param>
        internal void DeleteUserStatItems(string userId, string statCode
           , string additionalKey
           , DeleteUserStatItemsOptionalParameters optionalParameters
           , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            List<string> collectedStatCodes = new List<string>
            {
                statCode.ToString()
            };

            SendPredefinedEvent(userId, collectedStatCodes, PredefinedUserStatItemMode.Deleted);

            Api.DeleteUserStatItems(userId, statCode, additionalKey, optionalParameters, callback);
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

            Api.GetGlobalStatItemsByStatCode(
                statCode
                , optionalParameters
                , callback);
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
        /// Get list of statistic cycle config
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

            Api.GetListStatCycleConfigs(optionalParameters, callback);
        }

        /// <summary>
        /// Fetch the list of user's stat cycle items
        /// </summary>
        public void ListUserStatCycleItems(string userId, string cycleId,
            ResultCallback<ListUserStatCycleItemsResult> callback)
        {
            ListUserStatCycleItems(userId, cycleId, null, callback);
        }

        /// <summary>
        /// Fetch the list of user's stat cycle items
        /// </summary>
        public void ListUserStatCycleItems(string userId, string cycleId,
            ListUserStatCycleItemsOptionalParameters optionalParameters,
            ResultCallback<ListUserStatCycleItemsResult> callback)
        {
            Report.GetFunctionLog(GetType().Name, logger: optionalParameters?.Logger);

            Api.ListUserStatCycleItems(userId, cycleId, optionalParameters, callback);
        }
    }
}