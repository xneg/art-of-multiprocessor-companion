using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Locks;
using Xunit;

namespace Tests
{
    public class UnitTests
    {
        [Fact]
        public void ForNoLockMutualExclusionIsNotRespected()
        {
            var result = CheckMutualExclusion(new NoLock(), 5, 1000);
            Assert.True(5 * 1000 > result);
        }
        
        [Theory]
        [MemberData(nameof(GetLocks))]
        public void MutualExclusionShouldBeRespected(ILock @lock, int count)
        {
            var result = CheckMutualExclusion(@lock, count, 10000);
            Assert.Equal(count * 10000, result);
        }
        
        public static IEnumerable<object[]> GetLocks
        {
            get
            {
                return new[]
                {
                    new object[] { new Peterson(), 2 },
                    new object[] { new FilterLock(5), 5 },
                    new object[] { new BakeryLock(5), 5 },
                    new object[] { new DijkstraLock(5), 5 },
                };
            }
        }

        private static long CheckMutualExclusion(ILock @lock, int count, int cyclesCount)
        {
            var buckets = new long[count];
            var finished = new bool[count];
            var total = 0L;

            ThreadId.ZeroOut();
            
            var threads = Enumerable.Range(0, count).Select(i =>
            {
                return new Thread(() =>
                {
                    for (var j = 0; j < cyclesCount; j++)
                    {
                        @lock.Lock();
                        buckets[i]++;
                        total++;
                        @lock.Unlock();
                    }

                    finished[i] = true;
                });
            });

            foreach (var thread in threads)
            {
                thread.Start();
            }

            while (finished.Any(f => !f))
            {
            }

            return total;
        }
    }
}