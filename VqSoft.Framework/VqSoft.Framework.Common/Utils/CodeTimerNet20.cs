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
    public interface IAction
    {
        void Action();
    }

    /// <summary>
    /// For net 2.0
    /// Example: CodeTimer.Time("Thread Sleep", 10000000, new TestEmptyMethod());
    /// Notice: TestEmptyMethod must implement IAction
    /// </summary>
    public static class CodeTimerNet20
    {
        /// <summary>
        /// The accuracy is less than QueryThreadCycleTime, but it 
        /// can been used on lower version windows.
        /// </summary>
        /// <param name="hThread"></param>
        /// <param name="lpCreationTime"></param>
        /// <param name="lpExitTime"></param>
        /// <param name="lpKernelTime"></param>
        /// <param name="lpUserTime"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime,
          out long lpExitTime, out long lpKernelTime, out long lpUserTime);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        public delegate void ActionDelegate();

        private static long GetCurrentThreadTimes()
        {
            long l;
            long kernelTime, userTimer;
            GetThreadTimes(GetCurrentThread(), out l, out l, out kernelTime,
               out userTimer);
            return kernelTime + userTimer;
        }

        static CodeTimerNet20()
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

        }

        public static void Time(string name, int iteration, ActionDelegate action)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }

            if (action == null)
            {
                return;
            }

            //1. Print name
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);


            // 2. Record the latest GC counts
            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.Collect(GC.MaxGeneration);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 3. Run action
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long ticksFst = GetCurrentThreadTimes(); //100 nanosecond one tick

            for (int i = 0; i < iteration; i++) action();
            long ticks = GetCurrentThreadTimes() - ticksFst;
            watch.Stop();

            // 4. Print CPU
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTime Elapsed:\t\t" +
               watch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tTime Elapsed (one time):" +
               (watch.ElapsedMilliseconds / iteration).ToString("N0") + "ms");

            Console.WriteLine("\tCPU time:\t\t" + (ticks * 100).ToString("N0")
               + "ns");
            Console.WriteLine("\tCPU time (one time):\t" + (ticks * 100 /
               iteration).ToString("N0") + "ns");

            // 5. Print GC
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t\t" + count);
            }

            Console.WriteLine();

        }

        public static void Time(string name, int iteration, IAction action)
        {
            if (String.IsNullOrEmpty(name))
            {
                return;
            }

            if (action == null)
            {
                return;
            }

            //1. Print name
            ConsoleColor currentForeColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);


            // 2. Record the latest GC counts
            //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
            GC.Collect(GC.MaxGeneration);
            int[] gcCounts = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                gcCounts[i] = GC.CollectionCount(i);
            }

            // 3. Run action
            Stopwatch watch = new Stopwatch();
            watch.Start();
            long ticksFst = GetCurrentThreadTimes(); //100 nanosecond one tick

            for (int i = 0; i < iteration; i++) action.Action();
            long ticks = GetCurrentThreadTimes() - ticksFst;
            watch.Stop();

            // 4. Print CPU
            Console.ForegroundColor = currentForeColor;
            Console.WriteLine("\tTime Elapsed:\t\t" +
               watch.ElapsedMilliseconds.ToString("N0") + "ms");
            Console.WriteLine("\tTime Elapsed (one time):" +
               (watch.ElapsedMilliseconds / iteration).ToString("N0") + "ms");

            Console.WriteLine("\tCPU time:\t\t" + (ticks * 100).ToString("N0")
                + "ns");
            Console.WriteLine("\tCPU time (one time):\t" + (ticks * 100 /
                iteration).ToString("N0") + "ns");

            // 5. Print GC
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                int count = GC.CollectionCount(i) - gcCounts[i];
                Console.WriteLine("\tGen " + i + ": \t\t\t" + count);
            }

            Console.WriteLine();

        }
    }//end of class
}
