// Copyright (c) 2021 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access Reporting service.
    /// </summary>
    public class Reporting : WrapperBase
    {
        private readonly ReportingApi api;
        private readonly UserSession session;
        private readonly CoroutineRunner coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal Reporting( ReportingApi inApi
            , UserSession inSession
            , CoroutineRunner inCoroutineRunner )
        {
            Assert.IsNotNull(inApi, "inApi parameter can not be null. Construction is failed.");
            Assert.IsNotNull(inCoroutineRunner, "inCoroutineRunner parameter can not be null. Construction is failed.");

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
        internal Reporting( ReportingApi inApi
            , UserSession inSession
            , string inNamespace
            , CoroutineRunner inCoroutineRunner )
            : this( inApi, inSession, inCoroutineRunner ) // Curry this obsolete data to the new overload ->
        {
        }

        /// <summary>
        /// Get reason groups to retrieve reason group list to use for get reason by reason group.
        /// </summary>
        /// <param name="callback">
        /// This will be called when the operation succeeded. The result is ReportingReasonsGroupsResponse Model.
        /// </param>
        /// <param name="offset">The offset of the reason group results. Default value is 0.</param>
        /// <param name="limit">The limit of the reason group results. Default value is 1000.</param>
        public void GetReasonGroups( ResultCallback<ReportingReasonGroupsResponse> callback
            , int offset = 0
            , int limit = 1000 )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetReasonGroups(callback, offset, limit));
        }

        /// <summary>
        /// Get reason to retrieve reason list for reporting.
        /// </summary>
        /// <param name="reasonGroup">Specified reason group</param>
        /// <param name="callback">This will be called when the operation succeeded. The result is ReportingReasonsResponse Model.</param>
        /// <param name="offset">The offset of the reason results. Default value is 0.</param>
        /// <param name="limit">The limit of the reason results. Default value is 1000.</param>
        /// <param name="title">Query reason(s) by title</param>
        public void GetReasons( string reasonGroup
            , ResultCallback<ReportingReasonsResponse> callback
            , int offset = 0
            , int limit = 1000
            , string title = "")
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.GetReasons(reasonGroup, callback, offset, limit, title));
        }

        /// <summary>
        /// Submit report to content and/or user for moderation.
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="callback">
        /// This will be called when the operation succeeded. The result is ReportingSubmitResponse Model.
        /// </param>
        public void SubmitReport( ReportingSubmitData reportData
            , ResultCallback<ReportingSubmitResponse> callback )
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SubmitReport(reportData, callback));
        }

        public void SubmitChatReport(ReportingSubmitDataChat reportData,
            ResultCallback<ReportingSubmitResponse> callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            coroutineRunner.Run(
                api.SubmitChatReport(reportData, callback));
        }
    }
}
