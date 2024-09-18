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