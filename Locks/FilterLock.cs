using System.Linq;
using System.Threading;

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
                Volatile.Write(ref _level[me], i);
                Volatile.Write(ref _victim[i], me);

                while (LevelCondition(me, i) && Volatile.Read(ref _victim[i]) == me)
                {
                    Wait(me);
                }
            }
        }

        private bool LevelCondition(int me, int i)
        {
            return Enumerable.Range(0, _level.Length).Where(j => j != me)
                .Any(j => Volatile.Read(ref _level[j]) >= i);
        }

        protected virtual void Wait(int me)
        {
        }

        public void Unlock()
        {
            Volatile.Write(ref _level[ThreadId.Get()], 0);
        }
    }
}