// Copyright (c) 2023 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;

namespace AccelByte.Core
{
    public class AccelByteHeartBeat
    {
        internal const int DefaultIntervalInMs = 10000;

        internal Action OnHeartbeatTrigger;

        private int intervalInMs;
        private IHeartBeatState state = null;

        private IDebugger logger;

        public bool IsHeartBeatEnabled
        {
            get
            {
                return state.IsHeartBeatEnabled();
            }
        }

        public bool IsHeartBeatJobRunning
        {
            get
            {
                return state.IsHeartBeatJobRunning();
            }
        }

        public bool IsHeartBeatPaused
        {
            get
            {
                return state.IsHeartBeatPaused();
            }
        }

        public int IntervalInMs
        {
            get
            {
                return intervalInMs;
            }
        }

        public AccelByteHeartBeat(int heartBeatIntervalMs, IDebugger logger = null)
        {
            this.logger = logger;
            if (heartBeatIntervalMs <= 0)
            {
                this.intervalInMs = DefaultIntervalInMs;
                logger?.LogWarning($"The inputted interval {heartBeatIntervalMs} is not a valid interval");
            }
            else
            {
                this.intervalInMs = heartBeatIntervalMs;
            }

            this.state = new IdleState();
        }

        public void SetLogger(IDebugger newLogger)
        {
            logger = newLogger;
        }

        ~AccelByteHeartBeat()
        {
            if (state != null)
            {
                state.Stop();
            }

            OnHeartbeatTrigger = null;
        }

        public void Start()
        {
            if (!(state is RunningState))
            {
                TransitionTo(new RunningState(intervalInMs, () =>
                {
                    OnHeartbeatTrigger?.Invoke();
                }));
            }
        }

        public void Stop()
        {
            if (!(state is IdleState))
            {
                TransitionTo(new IdleState());
            }
        }

        public void Pause()
        {
            if (!(state is PauseState))
            {
                TransitionTo(new PauseState());
            }
        }

        public void UnPause()
        {
            Start();
        }

        private void TransitionTo(IHeartBeatState state)
        {
            this.state.Stop();
            this.state = state;
            this.state.Run();
        }
    }
}