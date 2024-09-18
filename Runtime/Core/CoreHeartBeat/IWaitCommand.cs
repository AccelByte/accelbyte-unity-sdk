using System.Threading;

namespace AccelByte.Core
{
    internal abstract class IWaitCommand
    {
        protected System.Action onDone;
        protected CancellationToken cancellationToken;
        public abstract bool Update(float dt);
        
        public IWaitCommand(System.Action onDone, CancellationToken cancellationToken)
        {
            this.onDone = onDone;
            this.cancellationToken = cancellationToken;
        }

        public bool IsCancelled()
        {
            return cancellationToken.IsCancellationRequested;
        }

        public void TriggerDone()
        {
            onDone?.Invoke();
        }
    }
}