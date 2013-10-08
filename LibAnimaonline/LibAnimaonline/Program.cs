using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Animaonline.Threading;

namespace Animaonline
{
    static class Program
    {
        private static void Main(string[] args)
        {

        }

        private static void BlockingContextTest()
        {
            var blockingContext = new BlockingContext();

            var newThread = new Thread(() =>
            {
                Console.WriteLine("Thread blocked");

                //blocks the current thread
                blockingContext.Block();

                Console.WriteLine("Thread unblocked");
            });

            newThread.Start();

            //do some work, then unblock the newThread

            var rnd = new Random();

            const int target = 1024;
            const int rndMax = 4096;

            int r = rnd.Next(rndMax);

            while (r != target)
            {
                //keep executing
                r = rnd.Next(rndMax);
            }

            //unblock the newThread
            blockingContext.Unblock();

            Console.WriteLine("BlockingContextTest:Done\r\nPress any key to continue...");
            Console.ReadLine();
        }
    }
}
