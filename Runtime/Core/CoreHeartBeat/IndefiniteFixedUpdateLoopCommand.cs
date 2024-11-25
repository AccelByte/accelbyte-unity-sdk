// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.
using System.Threading;

namespace AccelByte.Core
{
    internal class IndefiniteFixedUpdateLoopCommand : IWaitCommand
    {
        private System.Action<float> onUpdate;
        
        public IndefiniteFixedUpdateLoopCommand(System.Action<float> onUpdate, CancellationToken cancellationToken) : base(onDone: null, cancellationToken)
        {
            this.onUpdate = onUpdate;
            this.cancellationToken = cancellationToken;
        }
        
        public override bool Update(float dt)
        {
            onUpdate?.Invoke(dt);
            return false;
        }
    }
}