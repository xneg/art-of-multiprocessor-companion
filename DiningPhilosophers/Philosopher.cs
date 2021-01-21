using System;
using System.Threading;

namespace DiningPhilosophers
{
    public class Philosopher
    {
        private int delayMultiplier = 1;
        private readonly Random _rnd = new();
        private readonly Fork[] _forks;
        public State CurrentState { get; private set; } = State.Waiting;

        public Philosopher(Fork[] forks)
        {
            _forks = forks;
        }

        public void Start()
        {
            while (true)
            {
                lock (_forks[0])
                {
                    //uncomment this for deadlock
                    // Thread.Sleep(TimeSpan.FromMilliseconds(DelayDuration()));
                    lock (_forks[1])
                    {
                        CurrentState = State.Eating;
                        Thread.Sleep(TimeSpan.FromMilliseconds(DelayDuration()));
                        CurrentState = State.Waiting;
                    }
                }
                //comment this for deadlock
                Thread.Sleep(TimeSpan.FromMilliseconds(DelayDuration()));
            }
        }

        private int DelayDuration() => _rnd.Next(delayMultiplier, delayMultiplier * 10);

        public override string ToString()
        {
            return CurrentState == State.Eating ? "x" : "_";
        }
        
        public enum State
        {
            Waiting = 0,
            Eating = 1
        }
    }
}