using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Locks;

namespace DiningPhilosophers
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            // var forks = Enumerable.Range(0, 5).Select(i => new Fork(i)).ToArray();
            //
            // var philosophers = Enumerable.Range(0, forks.Length)
            //     .Select(i => new Philosopher(new[] {forks[i], forks[(i + 1) % forks.Length]})).ToArray();

            var locks = Enumerable.Range(0, 5).Select(i => new MyPeterson()).ToArray();
            
            // var philosophers = Enumerable.Range(0, locks.Length)
            //     .Select(i => new PhilosopherWithLock(locks[i], locks[(i + 1) % locks.Length])).ToArray();
            
            var philosophers = Enumerable.Range(0, locks.Length)
                .Select(i => 
                    new PhilosopherWithLock(
                        (i,locks[i]),
                        ((i + 1) % locks.Length, locks[(i + 1) % locks.Length]))).ToArray();

            var threads = Enumerable.Range(0, 5).Select(i =>
            {
                return new Thread(() =>
                {
                    locks[i].SetFirstThreadId();
                    Thread.Sleep(100);
                    philosophers[i].Start();
                });
            });

            foreach (var thread in threads.SkipLast(0))
            {
                thread.Start();
            }

            while (true)
            {
                Console.Clear();

                if (CheckCorrectness(philosophers))
                {
                    foreach (var philosopher in philosophers)
                    {
                        Console.WriteLine(philosopher.ToString());
                    }
                    await Task.Delay(500);
                }
                else
                {
                    Console.WriteLine("INCORRECT");
                    foreach (var philosopher in philosophers)
                    {
                        Console.WriteLine(philosopher.ToString());
                    }
                    break;
                }
            }
        }

        private static bool CheckCorrectness(IPhilosopher[] philosophers)
        {
            return !philosophers
                .Where((t, i) => t.CurrentState == PhilosopherState.Eating
                                 && philosophers[(i + 1) % philosophers.Length].CurrentState == PhilosopherState.Eating)
                .Any();
        }
    }
}