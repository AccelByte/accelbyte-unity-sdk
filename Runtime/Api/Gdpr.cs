// Copyright (c) 2023 - 2025 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using System.Collections;
using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    public class Gdpr : WrapperBase
    {
        private readonly GdprApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Gdpr(GdprApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "api==null (@ constructor)");
            Assert.IsNotNull(inSession, "session==null (@ constructor)");
            Assert.IsNotNull(inCoroutineRunner, "coroutineRunner==null (@ constructor)");

            this.api = inApi;
            this.session = inSession;
            this.coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Submit user's account deletion request 
        /// </summary>
        /// <param name="password">IAM password of the user</param>
        /// <param name="callback">Returns Delete Account Response via callback when completed</param>
        public void SubmitAccountDeletion(string password
            , ResultCallback<SubmitAccountDeletionResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SubmitAccountDeletion(
                    session.AuthorizationToken,
                    session.UserId,
                    password,
                    callback));
        }

        /// <summary>
        /// Submit headless account deletion request 
        /// </summary>
        /// <param name="platformType">PlatformType which used to generate current token</param>
        /// <param name="platformToken">Platform token of current logged platform</param>
        /// <param name="callback">Returns Delete Account Response via callback when completed</param>
        public void SubmitAccountDeletionOtherPlatform(PlatformType platformType
            , string platformToken
            , ResultCallback<SubmitAccountDeletionResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SubmitAccountDeletionPlatformId(
                    session.AuthorizationToken,
                    platformType.ToString().ToLower(),
                    platformToken,
                    callback));
        } 

        /// <summary>
        /// Submit headless account deletion request by platformId 
        /// </summary>
        /// <param name="platformId">Platform ID which used to generate current token</param>
        /// <param name="platformToken">Platform token of current logged platform</param>
        /// <param name="callback">Returns Delete Account Response via callback when completed</param>
        public void SubmitAccountDeletionPlatformId(string platformId
            , string platformToken
            , ResultCallback<SubmitAccountDeletionResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SubmitAccountDeletionPlatformId(
                    session.AuthorizationToken,
                    platformId,
                    platformToken,
                    callback));
        }

        /// <summary>
        /// Retrievte account deletion status, applies to the game namespace and publisher namespace 
        /// </summary>
        /// <param name="platformId">Platform ID which used to generate current token</param>
        /// <param name="platformToken">Platform token of current logged platform</param>
        /// <param name="callback">Returns Delete Account Response via callback when completed</param>
        public void GetAccountDeletionStatus(ResultCallback<AccountDeletionStatusResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetAccountDeletionStatus(
                    session.AuthorizationToken,
                    callback));
        }

        /// <summary>
        /// Cancel user's account deletion request, applies to the game namespace and publisher namespace 
        /// </summary>
        /// <param name="password">IAM password of the user</param>
        /// <param name="callback">Returns Delete Account Response via callback when completed</param>
        public void CancelAccountDeletion(ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.CancelAccountDeletion(
                    session.AuthorizationToken,
                    callback));
        }
    }
}