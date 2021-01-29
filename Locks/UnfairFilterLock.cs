using System.Linq;
using System.Threading;

namespace Locks
{
    public class UnfairFilterLock: ILock
    {
        private readonly int _count;
        private readonly int _victimThread;
        private readonly int[] _level;
        private readonly int[] _victim;

        public UnfairFilterLock(int count, int victimThread)
        {
            _count = count;
            _victimThread = victimThread;
            _level = new int[_count];
            _victim = new int[_count];
        }
        public void Lock()
        {
            var me = ThreadId.Get();
            for (var i = 1; i < _count; i++)
            {
                _level[me] = i;
                _victim[i] = me;

                while (_level.Where((_, k) => k != me).Any(l => l >= i)
                       && _victim[i] == me)
                {
                    if (me == _victimThread)
                    {
                        Thread.Sleep(3000);
                    }
                }
            }
        }

        public void Unlock()
        {
            _level[ThreadId.Get()] = 0;
        }
    }
}