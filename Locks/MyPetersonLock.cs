using System.Threading;

namespace Locks
{
    public class MyPetersonLock: ILock
    {
        private volatile int _victim;
        private int _firstThreadId;
        private readonly bool[] _flag = new bool[2];

        public void SetFirstThreadId()
        {
            _firstThreadId = ThreadId.Get();
        }
        
        public void Lock()
        {
            var i = _firstThreadId == ThreadId.Get() ? 0 : 1;
            var j = 1 - i;
            
            Volatile.Write(ref _flag[i], true);
            Thread.MemoryBarrier();
            _victim = i;
            
            while(Volatile.Read(ref _flag[j]) && _victim == i) {}
        }

        public void Unlock()
        {
            var i = _firstThreadId == ThreadId.Get() ? 0 : 1;
            Volatile.Write(ref _flag[i], false);
        }
    }
}