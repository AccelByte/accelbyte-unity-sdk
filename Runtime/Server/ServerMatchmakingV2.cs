// Copyright (c) 2022 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using AccelByte.Models;
using UnityEngine.Assertions;

namespace AccelByte.Server
{
    public class ServerMatchmakingV2 : WrapperBase
    {
        private readonly ServerMatchmakingV2Api _matchmakingV2Api;
        private readonly ISession _session;
        private readonly CoroutineRunner _coroutineRunner;

        internal ServerMatchmakingV2(ServerMatchmakingV2Api inApi
            , ISession inSession
            , CoroutineRunner inCoroutineRunner)
        {
            Assert.IsNotNull(inApi, "Cannot construct MatchmakingV2 manager; inApi is null!");
            Assert.IsNotNull(inCoroutineRunner, "Cannot construct MatchmakingV2 manager; inCoroutineRunner is null!");

            _matchmakingV2Api = inApi;
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
                _matchmakingV2Api.AcceptBackfillProposal(backfillProposal, stopBackfilling, callback));
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
                _matchmakingV2Api.RejectBackfillProposal(backfillProposal, stopBackfilling, callback));
        }
    }
}