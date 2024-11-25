// Copyright (c) 2024 AccelByte Inc. All Rights Reserved.
// This is licensed software from AccelByte Inc, for limitations
// and restrictions contact your company contract manager.

using System;
using System.Threading;

namespace AccelByte.Core
{
    internal class WaitAFrameCommand : IWaitCommand
    {
        public WaitAFrameCommand(Action onDone, CancellationToken cancellationToken) : base(onDone, cancellationToken)
        {
        }
        
        public override bool Update(float dt)
        {
            return true;
        }
    }
}