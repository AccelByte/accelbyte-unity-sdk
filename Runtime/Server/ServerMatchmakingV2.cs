// Copyright (c) 2022 - 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using System;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerMatchmakingV2 : WrapperBase
    {
        internal ServerMatchmakingV2Api Api;
        private readonly ISession _session;
        private readonly CoroutineRunner _coroutineRunner;

        [UnityEngine.Scripting.Preserve]
        internal ServerMatchmakingV2(ServerMatchmakingV2Api inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "Cannot construct MatchmakingV2 manager; inApi is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct MatchmakingV2 manager; inCoroutineRunner is null!");

            Api = inApi;
            _session = inSession;
            _coroutineRunner = inCoroutineRunner;
        }

        /// <summary>
        /// Accept backfill proposal
        /// </summary>
        /// <param name="backfillProposal">Backfill proposal received from notification</param>
        /// <param name="stopBackfilling">Should server stop backfilling</param>
        /// <param name="callback">
        /// Returns a Result via callback when completed.
        /// </param>
        public void AcceptBackfillProposal(MatchmakingV2BackfillProposalNotification backfillProposal,
            bool stopBackfilling,
            ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                Api.AcceptBackfillProposal(backfillProposal, stopBackfilling, cb =>
                {
                    SendPredefinedEvent(backfillProposal, RequestType.Accept);
                    HandleCallback(cb, callback);
                }));
        }

        /// <summary>
        /// Reject backfill proposal
        /// </summary>
        /// <param name="backfillProposal">Backfill proposal received from notification</param>
        /// <param name="stopBackfilling">Should server stop backfilling</param>
        /// <param name="callback">
        /// Returns a Result via callback when completed.
        /// </param>
        public void RejectBackfillProposal(MatchmakingV2BackfillProposalNotification backfillProposal, bool stopBackfilling,
            ResultCallback callback)
        {
            Report.GetFunctionLog(GetType().Name);

            if (!_session.IsValid())
            {
                callback.TryError(ErrorCode.IsNotLoggedIn);
                return;
            }

            _coroutineRunner.Run(
                Api.RejectBackfillProposal(backfillProposal, stopBackfilling, cb =>
                {
                    SendPredefinedEvent(backfillProposal, RequestType.Reject);
                    HandleCallback(cb, callback);
                }));
        }

        #region PredefinedEvents
        protected string podName;

        private enum RequestType
        {
            Accept,
            Reject
        }

        private IAccelByteTelemetryPayload CreatePayload(MatchmakingV2BackfillProposalNotification backfillNotif, RequestType requestType)
        {
            if (string.IsNullOrEmpty(podName))
            {
                podName = AccelByteSDK.GetServerRegistry().GetApi().GetDedicatedServerManager().ServerName;
            }

            IAccelByteTelemetryPayload payload = null;

            switch (requestType)
            {
                case RequestType.Accept:
                    payload = new PredefinedDSBackfillAcceptedPayload(podName, backfillNotif);
                    break;

                case RequestType.Reject:
                    payload = new PredefinedDSBackfillRejectedPayload(podName, backfillNotif);
                    break;
            }

            return payload;
        }

        private void SendPredefinedEvent(MatchmakingV2BackfillProposalNotification backfillNotif, RequestType requestType)
        {
            PredefinedEventScheduler predefinedEventScheduler = SharedMemory.PredefinedEventScheduler;
            if (predefinedEventScheduler == null)
            {
                return;
            }

            IAccelByteTelemetryPayload payload = CreatePayload(backfillNotif, requestType);

            if (payload == null)
            {
                return;
            }

            var dsHubEvent = new AccelByteTelemetryEvent(payload);
            predefinedEventScheduler.SendEvent(dsHubEvent, null);
        }

        private void HandleCallback(Result result, ResultCallback resultCallback)
        {
            if (result.IsError)
            {
                resultCallback.TryError(result.Error);
                return;
            }

            resultCallback.Try(result);
        }

        #endregion
    }
}