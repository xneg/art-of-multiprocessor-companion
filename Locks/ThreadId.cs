using System.Threading;

namespace Locks
{
    public static class ThreadId
    {
        // private static readonly ThreadLocal<int> ThreadId = new(() => Thread.CurrentThread.ManagedThreadId);
        private static readonly ThreadLocal<int> MyId = 
            new(() => Interlocked.Increment(ref _counter) - 1);
        private static int _counter;
        
        public static int Get() => MyId.Value;
    }
}