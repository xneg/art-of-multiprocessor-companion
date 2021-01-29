using System.Threading;

namespace Locks
{
    public class UnfairBakeryLock: BakeryLock
    {
        private readonly int _victimThread;

        public UnfairBakeryLock(int count, int victimThread) : base(count)
        {
            _victimThread = victimThread;
        }

        protected override void Wait(int me)
        {
            if (me == _victimThread)
            {
                Thread.Sleep(3000);
            }
        }
    }
}