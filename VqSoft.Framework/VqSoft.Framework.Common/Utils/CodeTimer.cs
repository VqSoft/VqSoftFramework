using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VqSoft.Framework.Common.Utils
{
    /// <summary>
    /// Requirement: NET 4.0+
    /// Used to test code performance
    /// Examples: CodeTimer.Time("Thread Sleep", 1, () => { Thread.Sleep(3000); });
    /// </summary>
    public static class CodeTimer
    {
        /// <summary>
        /// Should be called before the test
        /// </summary>
        public static void Initialize()
        {
            //Reduce the OS's dispatch effect
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            //Pre hot so that JIT can compile IL to native code.
            Time("", 1, () => { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Test name ,can not been empty</param>
        /// <param name="iteration">Code execution times</param>
        /// <param name="action">Function to be tested</param>
        public static void Time(string name, int iteration, Action action)
        {
            if (String.IsNullOrEmpty(name)) return;

            // 1.
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);

            // 2.
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 3.
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong cycleCount = GetCycleCount();
            for (int i = 0; i < iteration; i++) action();
            ulong cpuCycles = GetCycleCount() - cycleCount;
            watch.Stop();

            // 4.
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTime Elapsed:\t" + watch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tCPU Cycles:\t" + cpuCycles.ToString("N0"));

            // 5.
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t" + count);
            }

            Console.WriteLine();
        }

        private static ulong GetCycleCount()
        {
            ulong cycleCount = 0;
            QueryThreadCycleTime(GetCurrentThread(), ref cycleCount);
            return cycleCount;
        }

        /// <summary>
        /// Only Vista and Windows 2008 support
        /// </summary>
        /// <param name="threadHandle"></param>
        /// <param name="cycleTime"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();
    }//end of class
}
