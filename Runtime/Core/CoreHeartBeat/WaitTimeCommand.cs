// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System.Threading;

namespace AccelByte.Core
{
    internal class WaitTimeCommand : IWaitCommand
    {
        private double originalWaitTime;
        private double waitTime;

        public WaitTimeCommand(float waitTime, System.Action onDone, CancellationToken cancellationToken) : base(onDone, cancellationToken)
        {
            originalWaitTime = waitTime;
            this.waitTime = waitTime;
            this.onDone = onDone;
            this.cancellationToken = cancellationToken;
        }
        
        public WaitTimeCommand(double waitTime, System.Action onDone, CancellationToken cancellationToken) : base(onDone, cancellationToken)
        {
            originalWaitTime = waitTime;
            this.waitTime = waitTime;
            this.onDone = onDone;
            this.cancellationToken = cancellationToken;
        }

        public void ResetTime()
        {
            waitTime = originalWaitTime;
        }
        
        public override bool Update(float dt)
        {
            waitTime -= dt;
            if (waitTime <= 0)
            {
                return true;
            }
            return false;
        }
    }
}