using System.Linq;
using Locks;

namespace DiningPhilosophers
{
    public class PhilosopherWithLock: IPhilosopher
    {
        private readonly ILock[] _locks;
        private int _eaten;

        public PhilosopherWithLock(params (int, ILock)[] locks)
        {
            _locks = locks.OrderBy(l => l.Item1).Select(l => l.Item2).ToArray();
        }
        public PhilosopherWithLock(params ILock[] locks)
        {
            _locks = locks;
        }

        public PhilosopherState CurrentState { get; private set; }
        
        public void Start()
        {
            while (true)
            {
                _locks[0].Lock();
                _locks[1].Lock();
                
                CurrentState = PhilosopherState.Eating;
                _eaten++;
                CurrentState = PhilosopherState.Waiting;
                
                _locks[1].Unlock();
                _locks[0].Unlock();
            }
        }
        
        public override string ToString()
        {
            return $"{(CurrentState == PhilosopherState.Eating ? "e " : "  "),-2} eaten: {_eaten, -20}";
        }
    }
}