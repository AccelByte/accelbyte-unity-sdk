// Copyright (c) 2023 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using AccelByte.Core;
using System;

namespace AccelByte.Utils
{
	internal class AccelByteTimer
    {
        readonly float targetTimerInSecond;
        readonly Action onTimerComplete;
        float currentTimer;

        public AccelByteTimer(float timerInSecond, Action onTimerComplete)
        {
            this.targetTimerInSecond = timerInSecond;
            this.onTimerComplete = onTimerComplete;
        }

        public void Start()
        {
            currentTimer = targetTimerInSecond;
            AccelByteSDKMain.AddGameUpdateListener(OnUpdate, checkThreadSignallerIsAlive: false);
        }

        public void Stop()
        {
            AccelByteSDKMain.RemoveGameUpdateListener(OnUpdate);
        }

        void OnUpdate(float dt)
        {
            currentTimer -= dt;
            if (currentTimer <= 0)
            {
                onTimerComplete?.Invoke();
                AccelByteSDKMain.OnGameUpdate -= OnUpdate;
            }
        }
    }
}