using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DiningPhilosophers
{
    public class Philosopher
    {
        private const int DelayMultiplier = 1;
        private readonly Random _rnd = new();
        private readonly Fork[] _forks;
        private string _capturedForksIndexes;
        private int _eaten;
        public State CurrentState { get; private set; } = State.Waiting;

        public Philosopher(IEnumerable<Fork> forks)
        {
            _forks = forks.OrderBy(f => f.Index).ToArray();
        }

        public void Start()
        {
            while (true)
            {
                if (Monitor.TryEnter(_forks[0]))
                {
                    _capturedForksIndexes = _forks[0].Index.ToString();

                    if (Monitor.TryEnter(_forks[1]))
                    {
                        _capturedForksIndexes += ' ' + _forks[1].Index.ToString();
                        
                        CurrentState = State.Eating;
                        Thread.Sleep(TimeSpan.FromMilliseconds(100 * DelayDuration()));
                        _eaten++;
                        CurrentState = State.Waiting;
                        _capturedForksIndexes = "";
                        
                        Monitor.Exit(_forks[1]);
                    }
                    Thread.Sleep(TimeSpan.FromMilliseconds(DelayDuration()));
                    Monitor.Exit(_forks[0]);
                }
                _capturedForksIndexes = "";
            }
        }

        private int DelayDuration() => _rnd.Next(DelayMultiplier, DelayMultiplier * 10);

        public override string ToString()
        {
            return $"{(CurrentState == State.Eating ? "e " : "  "),-2} {_capturedForksIndexes,-3} eaten: {_eaten, -20}";
        }
        
        public enum State
        {
            Waiting = 0,
            Eating = 1
        }
    }
}