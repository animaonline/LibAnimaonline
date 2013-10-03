using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Animaonline
{
    static class Program
    {
        private static bool KeepAlive = true;

        private readonly static Queue<Action> MainThreadTasks = new Queue<Action>();

        private static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "MainThread";

            var backgroundThread = new Thread(WorkerThread) { Name = "BackgroundThread" };
            backgroundThread.Start();

            while (KeepAlive)
            {
                Thread.Sleep(10);

                while (MainThreadTasks.IsNotEmpty())
                {
                    var t = MainThreadTasks.Dequeue();
                    t();
                }
            }
        }

        private static void KillMainThread()
        {
            KeepAlive = false;
        }

        private static void WorkerThread()
        {
            var ints = new[] { 1, 2, 3, 4, 5, 6 };

            var intsExcept = ints.ExceptParams(2, 3, 4);

            var ac = new Action<int>((seed) =>
            {
                var rnd = new Random(seed);
                int a = rnd.Next();
                int b = rnd.Next();
                int c = a + b;

                Console.WriteLine("Result: {0}", c);
            });

            intsExcept.ParallelForEach(i => ac.StartTask(i));
        }
    }
}
