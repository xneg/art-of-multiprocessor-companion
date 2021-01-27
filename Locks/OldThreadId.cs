using System;
using System.Threading;

namespace Locks
{
    public static class OldThreadId
    {
        [ThreadStatic] private static int _myId;
        private static int _counter;

        public static int Get()
        {
            if (_myId == 0)
            {
                _myId = Interlocked.Increment(ref _counter);
            }

            return _myId - 1;
        }
    }
}