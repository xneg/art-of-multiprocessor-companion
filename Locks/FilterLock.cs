using System.Linq;

namespace Locks
{
    public class FilterLock: ILock
    {
        private readonly int _count;
        private readonly int[] _level;
        private readonly int[] _victim;

        public FilterLock(int count)
        {
            _count = count;
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
                    Wait(me);
                }
            }
        }

        protected virtual void Wait(int me)
        {
        }

        public void Unlock()
        {
            _level[ThreadId.Get()] = 0;
        }
    }
}