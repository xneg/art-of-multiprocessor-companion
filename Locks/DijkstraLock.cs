using System.Linq;
using System.Threading;

namespace Locks
{
    public class DijkstraLock: ILock
    {
        private readonly bool[] _first;
        private readonly bool[] _second;
        private volatile int _k;

        public DijkstraLock(int count)
        {
            _first = new bool[count];
            _second = new bool[count];
        }
        public void Lock()
        {
            var me = ThreadId.Get();
            Volatile.Write(ref _first[me], true);

            while (!Volatile.Read(ref _second[me]) || OthersCondition(me))
            {
                if (_k != me)
                {
                    Volatile.Write(ref _second[me], false);
                    if (!Volatile.Read(ref _first[_k])) { _k = me; }
                }
                else
                {
                    Volatile.Write(ref _second[me], true);
                }
                Wait(me);
            } 
        }

        private bool OthersCondition(int me)
        {
            return Enumerable.Range(0, _second.Length).Where(k => k != me)
                .Any(k => Volatile.Read(ref _second[k]));
        }

        public void Unlock()
        {
            var me = ThreadId.Get();
            Volatile.Write(ref _second[me], false);
            Volatile.Write(ref _first[me], false);
        }
        
        protected virtual void Wait(int me)
        {
        }
    }
}