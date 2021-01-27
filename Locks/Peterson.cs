namespace Locks
{
    public class Peterson: ILock
    {
        private volatile bool[] _flag = new bool[2];
        private volatile int _victim;
        
        public void Lock()
        {
            var i = ThreadId.Get();
            var j = 1 - i;
            _flag[i] = true;
            _victim = i;
            
            while(_flag[j] && _victim == i) {}
        }

        public void Unlock()
        {
            var i = ThreadId.Get();
            _flag[i] = false;
        }
    }
}