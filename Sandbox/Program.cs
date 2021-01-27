using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Locks;

namespace Sandbox
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var buckets = new long[2];
            ILock @lock = new Peterson();

            var threads = Enumerable.Range(0, 2).Select(i =>
            {
                return new Thread(() =>
                {
                    while (true)
                    {
                        @lock.Lock();
                        buckets[i]++;
                        // Thread.Sleep(100);
                        @lock.Unlock();
                    }
                });
            });
            
            foreach (var thread in threads)
            {
                thread.Start();
            }
            
            while (true)
            {
                Console.Clear();
                foreach (var bucket in buckets)
                {
                    Console.WriteLine(bucket);
                }
                
                await Task.Delay(500);
            }
        }
    }
}