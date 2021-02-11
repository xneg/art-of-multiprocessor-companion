using System.Threading;

namespace Locks
{
    public class Peterson: ILock
    {
        private readonly bool[] _flag = new bool[2];
        private volatile int _victim;
        
        public void Lock()
        {
            var i = ThreadId.Get();
            var j = 1 - i;
            Volatile.Write(ref _flag[i], true);
            Thread.MemoryBarrier();
            _victim = i;
            
            while(Volatile.Read(ref _flag[j]) && _victim == i) {}
        }

        public void Unlock()
        {
            Volatile.Write(ref _flag[ThreadId.Get()], false);
        }
    }
}