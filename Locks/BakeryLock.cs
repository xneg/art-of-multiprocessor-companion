using System.Linq;
using System.Threading;

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
            Volatile.Write(ref _flag[me], true);

            var max = VolatileMax(_label);
            Volatile.Write(ref _label[me], max + 1);
            // _label[me] = _label.Max() + 1;
            
            while (Condition(me))
            {
                Wait(me);
            }
        }

        private long VolatileMax(long[] array)
        {
            return Enumerable.Range(0, array.Length).Aggregate(0L, (acc, i) =>
            {
                var x = Volatile.Read(ref array[i]);
                return x > acc ? x : acc;
            });
        }

        protected virtual void Wait(int me)
        {
        }

        private bool Condition(int me)
        {
            return Enumerable.Range(0, _count).Where(i => i != me)
                .Any(el =>
                {
                    if (Volatile.Read(ref _flag[el])) return true;

                    var labelEl = Volatile.Read(ref _label[el]);
                    var labelMe = Volatile.Read(ref _label[me]);
                    
                    return labelEl < labelMe || labelEl == labelMe && el < me;
                });
        }

        public void Unlock()
        {
            Volatile.Write(ref _flag[ThreadId.Get()], false);
        }
    }
}