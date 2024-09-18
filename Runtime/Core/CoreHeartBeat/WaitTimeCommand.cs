using System.Threading;

namespace AccelByte.Core
{
    internal class WaitTimeCommand : IWaitCommand
    {
        private float waitTime;

        public WaitTimeCommand(float waitTime, System.Action onDone, CancellationToken cancellationToken) : base(onDone, cancellationToken)
        {
            this.waitTime = waitTime;
            this.onDone = onDone;
            this.cancellationToken = cancellationToken;
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