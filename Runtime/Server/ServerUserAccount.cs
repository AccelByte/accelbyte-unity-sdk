// Copyright (c) 2021 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections.Generic;
using AccelByte.Api;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerUserAccount : WrapperBase
    {
        internal readonly ISession Session;
        private readonly CoroutineRunner coroutineRunner;
        internal readonly ServerUserAccountApi Api;

        [UnityEngine.Scripting.Preserve]
        internal ServerUserAccount( ServerUserAccountApi inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, nameof(inApi) + " is null.");
            Assert.IsNotNull(inCoroutineRunner, nameof(inCoroutineRunner) + " is null.");

            Api = inApi;
            Session = inSession;
            coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// </summary>
        /// <param name="inApi"></param>
        /// <param name="inSession"></param>
        /// <param name="inNamespace">DEPRECATED - Now passed to Api from Config</param>
        /// <param name="inCoroutineRunner"></param>
        [Obsolete("namespace param is deprecated (now passed to Api from Config): Use the overload without it"), UnityEngine.Scripting.Preserve]
        internal ServerUserAccount( ServerUserAccountApi inApi
            , ISession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner )
        {
        }

        /// <summary>
        /// Get User Data by Authorization Token
        /// </summary>
        /// <param name="userAuthToken">User's authorization token</param>
        /// <param name="callback">Returns a UserData via callback when completed.</param>
        public void GetUserData( string userAuthToken
            , ResultCallback<UserData> callback )
        {
            coroutineRunner.Run(Api.GetUserData(userAuthToken, callback));
        }

        /// <summary>
        /// This function will search user by their third party Display Name.
        /// The query will be used to find the user with the most approximate display name.
        /// </summary>
        /// <param name="platformDisplayName">Targeted user's third party by Display Name.</param>
        /// <param name="platformType">The platform type want to use to search user</param>
        /// <param name="callback">Returns a PagedUserOtherPlatformInfo via callback when completed.</param>
        /// <param name="limit">The limit of the users data result. Default value is 20.</param>
        /// <param name="offset">The offset of the users data result. Default value is 0.</param>
        public void SearchUserOtherPlatformDisplayName( string platformDisplayName
            , PlatformType platformType
            , ResultCallback<PagedUserOtherPlatformInfo> callback
            , int limit = 20
            , int offset = 0 )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.SearchUserOtherPlatformDisplayName(
                    platformDisplayName,
                    platformType,
                    callback,
                    limit,
                    offset));
        }

        /// <summary>
        /// This function will search user by their third party Platform User ID.
        /// </summary>
        /// <param name="platformUserId">Targeted user's third party user id.</param>
        /// <param name="platformType">The platform type want to use to search user</param>
        /// <param name="callback">Returns a UserOtherPlatformInfo via callback when completed.</param>
        public void SearchUserOtherPlatformUserId( string platformUserId
            , PlatformType platformType
            , ResultCallback<UserOtherPlatformInfo> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                Api.SearchUserOtherPlatformUserId(
                    platformUserId,
                    platformType,
                    callback));
        }

        /// <summary>
        /// Ban a Single User.
        /// Only Moderator that can ban other user.
        /// </summary>
        /// <param name="userId">Ban user's user ID</param>
        /// <param name="banType">The type of Ban</param>
        /// <param name="reason">The reason of Banning</param>
        /// <param name="endDate">The date when the ban is lifted</param>
        /// <param name="comment">The detail or comment about the banning</param>
        /// <param name="notifyUser">Notify user via email or not</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void BanUser(string userId
            , BanType banType
            , BanReason reason
            , DateTime endDate
            , string comment, bool notifyUser
            , ResultCallback<UserBanResponseV3> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            BanUser(userId,
                banType,
                reason,
                endDate,
                new BanUserOptionalParameters()
                {
                    SkipNotif = !notifyUser,
                    Comment = comment
                },
                callback);
        }
        
        internal void BanUser(string userId
            , BanType banType
            , BanReason reason
            , DateTime endDate
            , BanUserOptionalParameters optionalParameters
            , ResultCallback<UserBanResponseV3> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }
            
            this.Api.BanUser(userId,
                banType,
                reason,
                endDate,
                optionalParameters,
                callback);
        }

        /// <summary>
        /// Change Ban Status of a Single User (enabled/disabled).
        /// Only Moderator that can change ban status.
        /// </summary>
        /// <param name="userId">Banned user's user ID</param>
        /// <param name="banId">Banned user's ban ID</param>
        /// <param name="enabled">Banned Status, false to disabled</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void ChangeUserBanStatus(string userId
            , string banId
            , bool enabled
            , ResultCallback<UserBanResponseV3> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!ValidateAccelByteId(banId, Utils.AccelByteIdValidator.HypensRule.NoRule, Utils.AccelByteIdValidator.GetBanIdInvalidMessage(banId), callback))
            {
                return;
            }

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.Api.ChangeUserBanStatus(
                    userId,
                    banId,
                    enabled,
                    callback));
        }

        /// <summary>
        /// Get User Ban Information
        /// Only Moderator that can get the ban information.
        /// </summary>
        /// <param name="userId">Banned user's user ID</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetUserBanInfo(string userId
            , ResultCallback<UserBanPagedList> callback
            , bool activeOnly = true)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.Api.GetUserBanInfo(
                    userId,
                    activeOnly,
                    callback));
        }

        /// <summary>
        /// Get User Banned List
        /// Only Moderator that can get the banned list.
        /// </summary>
        /// <param name="banType">The type of Ban</param>
        /// <param name="offset">Offset of the list that has been sliced based on Limit parameter (optional, default = 0) </param>
        /// <param name="limit">The limit of item on page (optional) </param>
        /// <param name="callback">Returns a result via callback when completed</param>
        /// <param name="activeOnly">true to only get the enabled banned list</param>
        public void GetUserBannedList(BanType banType
            , int offset
            , int limit
            , ResultCallback<UserBanPagedList> callback
            , bool activeOnly = true)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.Api.GetUserBannedList(
                    activeOnly,
                    banType,
                    offset,
                    limit,
                    callback));
        }

        /// <summary>
        /// Get user data from another user by user id
        /// </summary>
        /// <param name="userId"> user id that needed to get user data</param>
        public void GetUserByUserId(string userId
            , ResultCallback<UserData> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!ValidateAccelByteId(userId, Utils.AccelByteIdValidator.HypensRule.NoHypens, Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.Api.GetUserByUserId(userId, callback));
        }

        /// <summary>
        /// This function to List user by user ids.
        /// </summary>
        /// <param name="listUserDataRequest">Struct request containing user ids</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void ListUserByUserId(ListUserDataRequest listUserDataRequest
            , ResultCallback<ListUserDataResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            this.coroutineRunner.Run(
                this.Api.ListUserByUserId(listUserDataRequest, callback));
        }

        /// <summary>
        /// Search user by the list of email addresses
        /// </summary>
        /// <param name="emailAddresses">List of user email address to search</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetBulkUserByEmailAddress(IEnumerable<string> emailAddresses
            , ResultCallback<GetBulkUserByEmailAddressResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.GetBulkUserByEmailAddress(emailAddresses, callback);
        }

        /// <summary>
        /// Retrieves platform accounts linked to user.
        /// </summary>
        /// <param name="userId">User id to get the linked platform accounts</param>
        /// <param name="optionalParameters">Optional params to get linked platform accounts</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetLinkedPlatformAccounts(string userId
            , GetLinkedPlatformAccountsOptionalParams optionalParameters
            , ResultCallback<PagedPlatformLinks> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!ValidateAccelByteId(userId
                , Utils.AccelByteIdValidator.HypensRule.NoHypens
                , Utils.AccelByteIdValidator.GetUserIdInvalidMessage(userId), callback))
            {
                return;
            }

            if (!this.Session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            Api.GetLinkedPlatformAccounts(userId, optionalParameters, callback);
        }

        /// <summary>
        /// Retrieves platform accounts linked to user.
        /// </summary>
        /// <param name="userId">User id to get the linked platform accounts</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void GetLinkedPlatformAccounts(string userId, ResultCallback<PagedPlatformLinks> callback)
        {
            GetLinkedPlatformAccounts(userId, null , callback);
        }
    }
}
