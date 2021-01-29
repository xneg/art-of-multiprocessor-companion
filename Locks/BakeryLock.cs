using System.Linq;

namespace Locks
{
    public class BakeryLock: ILock
    {
        private readonly int _count;
        private readonly bool[] _flag;
        private readonly long[] _label;
        
        public BakeryLock(int count)
        {
            _count = count;
            _flag = new bool[_count];
            _label = new long[_count];
        }
        public void Lock()
        {
            var me = ThreadId.Get();
            _flag[me] = true;
            _label[me] = _label.Max() + 1;
            while (Condition(me))
            {
                Wait(me);
            }
        }

        protected virtual void Wait(int me)
        {
        }

        private bool Condition(int me)
        {
            return Enumerable.Range(0, _count).Where(i => i != me)
                .Any(el => _flag[el] && 
                           (_label[el] < _label[me] || _label[el] == _label[me] && el < me));
        }

        public void Unlock()
        {
            _flag[ThreadId.Get()] = false;
        }
    }
}