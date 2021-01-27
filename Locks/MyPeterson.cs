namespace Locks
{
    public class MyPeterson: ILock
    {
        private volatile int _victim;
        private int _firstThreadId;
        private volatile bool[] _flag = new bool[2];

        public void SetFirstThreadId()
        {
            _firstThreadId = ThreadId.Get();
        }
        
        public void Lock()
        {
            var i = _firstThreadId == ThreadId.Get() ? 0 : 1;
            var j = 1 - i;
            _flag[i] = true;
            _victim = i;
            
            while(_flag[j] && _victim == i) {}
        }

        public void Unlock()
        {
            var i = _firstThreadId == ThreadId.Get() ? 0 : 1;
            _flag[i] = false;
        }
    }
}