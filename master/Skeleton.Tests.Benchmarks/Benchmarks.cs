using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Skeleton.Tests.Benchmarks
{
    public class BenchmarkCollection : List<Benchmark>
    {
        public void Add(Action executor, string name)
        {
            Add(Benchmark.Create(executor, name));
        }

        public void Run()
        {
            // warmup
            foreach (var test in this)
            {
                test.Watch = new Stopwatch();
                test.Watch.Reset();
            }

            foreach (var test in this)
            {
                test.Watch.Start();
                test.Executor();
                test.Watch.Stop();
            }

            foreach (var test in this.OrderBy(t => t.Watch.ElapsedMilliseconds))
                Trace.WriteLine(test.Name + " :: " + test.Watch.ElapsedMilliseconds + "ms");
        }
    }
}