using System.Linq;

namespace Locks
{
    public class DijkstraLock: ILock
    {
        private readonly bool[] _b;
        private readonly bool[] _c;
        private int _k;

        public DijkstraLock(int count)
        {
            _b = Enumerable.Range(0, count).Select(_ => true).ToArray();
            _c = Enumerable.Range(0, count).Select(_ => true).ToArray();
        }
        public void Lock()
        {
            var me = ThreadId.Get();
            _b[me] = false;

            do
            {
                if (_k != me)
                {
                    _c[me] = true;
                    if (_b[_k]) { _k = me; }
                }
                else
                {
                    _c[me] = false;
                }
            } while (_k != me || _c[me] || _c.Where((_, k) => k != me).Any(v => !v));
        }

        public void Unlock()
        {
            var me = ThreadId.Get();
            _c[me] = true;
            _b[me] = true;
        }
    }
}