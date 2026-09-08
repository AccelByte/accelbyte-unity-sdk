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

        /// <summary>
        /// Submit a personal data request. Used when the account is a headless account.
        /// </summary>
        /// <param name="platformType">PlatformType which used to generate current token</param>
        /// <param name="platformToken">Platform token of current logged platform</param>
        /// <param name="callback">Returns Submit Personal Data Request Response via callback when completed</param>
        /// <param name="email">Optional email address that receives the notification and the download link once the
        /// request completes. There is no other way to retrieve the completed download link for this flow, so a
        /// request submitted without an email has no retrieval path once it completes.</param>
        /// <param name="languageTag">Optional language tag for the notification email</param>
        public void SubmitMyPersonalDataRequest(PlatformType platformType
            , string platformToken
            , ResultCallback<SubmitPersonalDataRequestResponse> callback
            , string email = null
            , string languageTag = null)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var optionalParameters = new SubmitMyPersonalDataRequestOptionalParameters()
            {
                Email = email,
                LanguageTag = languageTag
            };

            api.SubmitMyPersonalDataRequest(platformType.ToString().ToLower()
                , platformToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Submit a personal data request by platformId. Used when the account is a headless account.
        /// </summary>
        /// <param name="platformId">Platform ID which used to generate current token, string type of this field
        /// makes it support OpenID Connect (OIDC)</param>
        /// <param name="platformToken">Platform token of current logged platform</param>
        /// <param name="callback">Returns Submit Personal Data Request Response via callback when completed</param>
        /// <param name="email">Optional email address that receives the notification and the download link once the
        /// request completes. See SubmitMyPersonalDataRequest for why omitting it leaves the download link
        /// unrecoverable.</param>
        /// <param name="languageTag">Optional language tag for the notification email</param>
        public void SubmitMyPersonalDataRequestPlatformId(string platformId
            , string platformToken
            , ResultCallback<SubmitPersonalDataRequestResponse> callback
            , string email = null
            , string languageTag = null)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var optionalParameters = new SubmitMyPersonalDataRequestOptionalParameters()
            {
                Email = email,
                LanguageTag = languageTag
            };

            api.SubmitMyPersonalDataRequest(platformId
                , platformToken
                , optionalParameters
                , callback);
        }

        /// <summary>
        /// Retrieve the caller's personal data requests.
        /// </summary>
        /// <param name="callback">Returns Personal Data Requests Response via callback when completed</param>
        /// <param name="offset">The offset of the request data</param>
        /// <param name="limit">The limit of the request data</param>
        public void GetMyPersonalDataRequests(ResultCallback<PersonalDataRequestsResponse> callback
            , int offset = 0
            , int limit = 20)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var optionalParameters = new GetMyPersonalDataRequestsOptionalParameters()
            {
                Offset = offset,
                Limit = limit
            };

            api.GetMyPersonalDataRequests(optionalParameters, callback);
        }

        /// <summary>
        /// Cancel a pending personal data request.
        /// </summary>
        /// <param name="requestDate">The request date identifying which request to cancel, as returned by
        /// SubmitMyPersonalDataRequest or GetMyPersonalDataRequests</param>
        /// <param name="callback">Returns a result via callback when completed</param>
        public void CancelMyPersonalDataRequest(DateTime requestDate
            , ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback?.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            var optionalParameters = new CancelMyPersonalDataRequestOptionalParameters();

            api.CancelMyPersonalDataRequest(requestDate, optionalParameters, callback);
        }
    }
}