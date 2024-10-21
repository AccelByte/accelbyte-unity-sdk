// Copyright (c) 2020 - 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Threading;

namespace AccelByte.Core
{
    internal class IndefiniteLoopCommand : IWaitCommand
    {
        private System.Action onUpdate;
        private double? expectedInterval;
        private double? currentIntervalTimer;
        
        public IndefiniteLoopCommand(System.Action onUpdate, CancellationToken cancellationToken) : base(onDone: null, cancellationToken)
        {
            this.onUpdate = onUpdate;
            this.cancellationToken = cancellationToken;
        }
        
        public IndefiniteLoopCommand(double interval, System.Action onUpdate, CancellationToken cancellationToken) : base(onDone: null, cancellationToken)
        {
            this.onUpdate = onUpdate;
            this.cancellationToken = cancellationToken;

            expectedInterval = interval;
            currentIntervalTimer = interval;
        }

        public override bool Update(float dt)
        {
            if (currentIntervalTimer != null)
            {
                currentIntervalTimer -= dt;
                if (currentIntervalTimer <= 0)
                {
                    currentIntervalTimer = expectedInterval;
                }
                else
                {
                    return false;
                }
            }
            onUpdate?.Invoke();
            return false;
        }
    }
}