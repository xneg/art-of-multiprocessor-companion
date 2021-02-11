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
            var count = 2;
            var buckets = new long[count];
            var total = 0L;
            // ILock @lock = new UnfairFilterLock(count, 3); // new Peterson();
            ILock @lock = new Peterson();
            // ILock @lock = new BakeryLock(count);
            // ILock @lock = new FilterLock(count);
            // ILock @lock = new DijkstraLock(count);
            // ILock @lock = new UnfairDijkstraLock(count, 3);

            var threads = Enumerable.Range(0, count).Select(i =>
            {
                return new Thread(() =>
                {
                    for (var j = 0; j < 100000; j++)
                    {
                        @lock.Lock();
                        buckets[i]++;
                        total++;
                        @lock.Unlock();
                    }
                    
                    // while (true)
                    // {
                    //     @lock.Lock();
                    //     buckets[i]++;
                    //     // Thread.Sleep(100);
                    //     @lock.Unlock();
                    // }
                    
                    // if (ThreadId.Get() == 3)
                    //     Thread.Sleep(10);
                    //
                    // while (buckets[3] < 10)
                    // {
                    //     @lock.Lock();
                    //     buckets[i]++;
                    //     @lock.Unlock();
                    // }
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
            
                Console.WriteLine("total:" + total);
                
                await Task.Delay(500);
            }
        }
    }
}