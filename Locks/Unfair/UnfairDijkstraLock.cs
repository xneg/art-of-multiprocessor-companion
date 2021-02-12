using System.Threading;

namespace Locks.Unfair
{
    public class UnfairDijkstraLock: DijkstraLock
    {
        private readonly int _victimThread;
        
        public UnfairDijkstraLock(int count, int victimThread) : base(count)
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