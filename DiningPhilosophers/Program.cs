using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var forks = Enumerable.Range(0, 5).Select(i => new Fork(i)).ToArray();

            var philosophers = Enumerable.Range(0, forks.Length)
                .Select(i => new Philosopher(new[] {forks[i], forks[(i + 1) % forks.Length]})).ToArray();

            foreach (var philosopher in philosophers.SkipLast(0))
            {
                philosopher.Start();
            }

            while (true)
            {
                Console.Clear();

                if (CheckCorrectness(philosophers))
                {
                    foreach (var philosopher in philosophers)
                    {
                        Console.Write(philosopher.ToString());
                    }
                    await Task.Delay(5);
                }
                else
                {
                    Console.WriteLine("INCORRECT");
                    break;
                }
            }
        }

        private static bool CheckCorrectness(Philosopher[] philosophers)
        {
            return !philosophers
                .Where((t, i) => t.CurrentState == Philosopher.State.Eating
                                 && philosophers[(i + 1) % philosophers.Length].CurrentState == Philosopher.State.Eating)
                .Any();
        }
    }
}