using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    public class Philosopher
    {
        private int delayMultiplier = 100;
        private readonly Random _rnd = new Random();
        private readonly Fork[] _forks;
        public State CurrentState { get; private set; } = State.Waiting;

        public Philosopher(Fork[] forks)
        {
            _forks = forks;
        }

        public async Task Start()
        {
            while (true)
            {
                TryTakeFork(0);
                TryTakeFork(1);
                await TryToEat();
                await Task.Delay(DelayDuration());
            }
        }
        
        private void TryTakeFork(int index)
        {
            _forks[index].Owner ??= this;
        }

        private async Task TryToEat()
        {
            if (_forks[0].Owner == this && _forks[1].Owner == this)
            {
                CurrentState = State.Eating;
                await Task.Delay(DelayDuration());
                _forks[0].Owner = null;
                _forks[1].Owner = null;
            }
            CurrentState = State.Waiting;
        }

        private int DelayDuration() => _rnd.Next(delayMultiplier, delayMultiplier * 10);

        public override string ToString()
        {
            if (CurrentState == State.Eating)
                return "x";

            var ownedFork = _forks.FirstOrDefault(f => f.Owner == this);
            return ownedFork is null ? "_" : ownedFork.ToString();
        }
        
        public enum State
        {
            Waiting = 0,
            Eating = 1
        }
    }
}