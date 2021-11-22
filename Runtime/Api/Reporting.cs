// Copyright (c) 2021 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Collections.Generic;
using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Api
{
    /// <summary>
    /// Provide APIs to access Reporting service.
    /// </summary>
    public class Reporting
    {
        private readonly ReportingApi api;
        private readonly ISession session;
        private readonly string @namespace;
        private readonly CoroutineRunner coroutineRunner;

        internal Reporting(ReportingApi api, ISession session, string @namespace, CoroutineRunner coroutineRunner)
        {
            Assert.IsNotNull(api, "api parameter can not be null. Construction is failed.");
            Assert.IsNotNull(session, "session parameter can not be null. Construction is failed.");
            Assert.IsFalse(string.IsNullOrEmpty(@namespace), "namespace paramater can not be empty. Construction is failed.");
            Assert.IsNotNull(coroutineRunner, "coroutineRunner parameter can not be null. Construction is failed.");

            this.api = api;
            this.session = session;
            this.@namespace = @namespace;
            this.coroutineRunner = coroutineRunner;
        }

        /// <summary>
        /// Get reason groups to retrieve reason group list to use for get reason by reason group.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is ReportingReasonsGroupsResponse Model.</param>
        /// <param name="offset">The offset of the reason group results. Default value is 0.</param>
        /// <param name="limit">The limit of the reason group results. Default value is 1000.</param>
        public void GetReasonGroups(ResultCallback<ReportingReasonGroupsResponse> callback, int offset = 0, int limit = 1000)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetReasonGroups(this.@namespace, this.session.AuthorizationToken, callback, offset, limit));
        }

        /// <summary>
        /// Get reason to retrieve reason list for reporting.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is ReportingReasonsResponse Model.</param>
        /// <param name="offset">The offset of the reason results. Default value is 0.</param>
        /// <param name="limit">The limit of the reason results. Default value is 1000.</param>
        public void GetReasons(string reasonGroup, ResultCallback<ReportingReasonsResponse> callback, int offset = 0, int limit = 1000)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.GetReasons(this.@namespace, this.session.AuthorizationToken, reasonGroup, callback, offset, limit));
        }

        /// <summary>
        /// Submit report to content and/or user for moderation.
        /// </summary>
        /// <param name="callback">This will be called when the operation succeeded. The result is ReportingSubmitResponse Model.</param>
        public void SubmitReport(ReportingSubmitData reportData, ResultCallback<ReportingSubmitResponse> callback)
        {
            Report.GetFunctionLog(this.GetType().Name);

            if (!this.session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);

                return;
            }

            this.coroutineRunner.Run(
                this.api.SubmitReport(this.@namespace, this.session.AuthorizationToken, reportData, callback));
        }
    }
}
