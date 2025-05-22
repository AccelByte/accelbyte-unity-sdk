// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerSeasonPass : WrapperBase
    {
        private readonly CoroutineRunner coroutineRunner;
        private readonly ServerSeasonPassApi api;
        private readonly ISession session;

        [UnityEngine.Scripting.Preserve]
        internal ServerSeasonPass( ServerSeasonPassApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull( inApi, nameof( inApi ) + " is null.");
            Assert.IsNotNull( inCoroutineRunner, nameof( inCoroutineRunner ) + " is null.");

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
        internal ServerSeasonPass( ServerSeasonPassApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Grant exp to user by UserId. After reaching to the next level, currentExp will be reset to 0, remainder will be added.
        /// </summary>
        /// <param name="userId">The User ID will be granted the exp.</param>
        /// <param name="exp">Total of the exp will be granted to user.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfoWithoutReward via callback when completed.</param>
        /// <param name="source">Granted tier source. Default value SWEAT.</param>
        /// <param name="tags">Grant for reason. Default value null.</param>
        public void GrantExpToUser( string userId
            , int exp
            , ResultCallback<UserSeasonInfoWithoutReward> callback 
            , SeasonPassSource source = SeasonPassSource.SWEAT
            , string[] tags = null)
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

            coroutineRunner.Run(
                api.GrantExpToUser(userId, exp, callback, source, tags));
        }

        /// <summary>
        /// Grant tier to user by UserId. used to grant tier to user, it will auto enroll if there's no user season but active published season exist, season only located in non-publisher namespace, otherwise ignore.
        /// </summary>
        /// <param name="userId">The User ID will be granted the exp.</param>
        /// <param name="count">Total of the count will be granted to user.</param>
        /// <param name="source">Granted tier source.</param>
        /// <param name="tags">Grant for reason.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfoWithoutReward via callback when completed.</param>
        public void GrantTierToUser(string userId
            , int count
            , ResultCallback<UserSeasonInfoWithoutReward> callback
            , SeasonPassSource source = SeasonPassSource.SWEAT
            , string[] tags = null)
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

            coroutineRunner.Run(
                api.GrantTierToUser(userId, count, callback, source, tags));
        }

        /// <summary>
        /// Get User Season Data  by UserId. Used for get user season data, season only located in non-publisher namespace.
        /// </summary>
        /// <param name="userId">The User ID will be granted the exp.</param>
        /// <param name="exp">Total of the exp will be granted to user.</param>
        /// <param name="source">Granted tier source.</param>
        /// <param name="tags">Grant for reason.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfo via callback when completed.</param>
        public void GetUserSeasonData(string userId
            , string seasonId
            , ResultCallback<UserSeasonInfo> callback)
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

            coroutineRunner.Run(
                api.GetUserSeasonData(userId, seasonId, callback));
        }

        /// <summary>
        /// Get Current User Season History  by UserId. used to get user exp acquisition history, season only located in non-publisher namespace.
        /// </summary>
        /// <param name="userId">The User ID will be granted the exp.</param>
        /// <param name="seasonId">The Id of the Season.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonExpHistory via callback when completed.</param>
        public void GetCurrentUserSeasonHistory(string userId
            , string seasonId
            , ResultCallback<UserSeasonExpHistory> callback)
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

            coroutineRunner.Run(
                api.GetCurrentUserSeasonHistory(userId, seasonId, callback));
        }

        /// <summary>
        /// Get Query User Season Exp Season History  by UserId. After reaching to the next level, used to get user exp acquisition history's tag list.
        /// </summary>
        /// <param name="userId">The User ID will be granted the exp.</param>
        /// <param name="seasonId">The Id of the Season.</param>
        /// <param name="callback">Returns a Result that contains QueryUserSeasonExp via callback when completed.</param>
        public void QueryUserSeasonExp(string userId
            , string seasonId
            , ResultCallback<QueryUserSeasonExp> callback)
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

            coroutineRunner.Run(
                api.QueryUserSeasonExp(userId, seasonId, callback));
        }

        /// <summary>
        /// Get current user season progression, season only located in non-publisher namespace.
        /// </summary>
        /// <param name="userId">The User ID to check user season progression.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfoWithoutReward via callback when completed.</param>
        public void GetCurrentUserSeasonProgression( string userId
            , ResultCallback<UserSeasonInfoWithoutReward> callback )
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

            coroutineRunner.Run(
                api.GetCurrentUserSeasonProgression(userId, callback));
        }


        /// <summary>
        /// Bulk get current user session progression.
        /// </summary>
        /// <param name="userId">The User IDs to get user session progression.</param>
        /// <param name="callback">Returns a Result that contains UserSeasonInfo via callback when completed.</param>
        public void BulkGetUserSessionProgression(string[] userIds
            , ResultCallback<UserSeasonInfo[]> callback)
        {
            Report.GetFunctionLog(GetType().Name);
            Assert.IsNotNull(userIds, "Can't check user progression; UserIds parameter is null!");
            Assert.IsTrue(userIds.Length > 0, " Given validation, you need to at least specify one element on the list");

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var requestModel = new BulkGetUserSessionProgressionRequest()
            {
                UserIds = userIds
            };

            coroutineRunner.Run(api.BulkGetUserSessionProgression(requestModel, callback));
        }

    }
}