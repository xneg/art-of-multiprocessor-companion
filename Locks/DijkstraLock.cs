using System.Linq;

namespace Locks
{
    public class DijkstraLock: ILock
    {
        private readonly bool[] _first;
        private readonly bool[] _second;
        private int _k;

        public DijkstraLock(int count)
        {
            _first = new bool[count];
            _second = new bool[count];
        }
        public void Lock()
        {
            var me = ThreadId.Get();
            _first[me] = true;

            while (!_second[me] || _second.Where((_, k) => k != me).Any(v => v))
            {
                if (_k != me)
                {
                    _second[me] = false;
                    if (!_first[_k]) { _k = me; }
                }
                else
                {
                    _second[me] = true;
                }
            } 
        }

        public void Unlock()
        {
            var me = ThreadId.Get();
            _second[me] = false;
            _first[me] = false;
        }
    }
}