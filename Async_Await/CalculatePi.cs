using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//--created by Paul Zänker

namespace Async_Await
{
    class CalculatePi
    {
        public static Stopwatch stopWatch = new Stopwatch();

        public CalculatePi()
        { }

        public double calculatePi()
        {
            object lockObject = new object();

            double pi = 0.0;
            double n = Math.Pow(10, 9);
            double step = 1.0 / n;

            stopWatch.Start();
            Parallel.For(1, Convert.ToInt32(n + 1), () => 0.0, (i, loopState, parallelLoopResult) =>
            {
                double x = (i - 0.5) * step;
                return parallelLoopResult + 4.0 / (1.0 + x * x);
            },

           localPartialSum =>
           {
               lock (lockObject)
               {
                   pi += localPartialSum;
               }
           });

            stopWatch.Stop();
            Console.WriteLine("{0}", pi *= step);
            Console.WriteLine(">> Pi berechnet in " + stopWatch.ElapsedMilliseconds + " ms\n");
            return pi *= step;
        }
    }
}
