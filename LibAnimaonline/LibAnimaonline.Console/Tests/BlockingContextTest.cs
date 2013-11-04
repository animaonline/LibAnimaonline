using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Animaonline.Threading;

namespace LibAnimaonline.Console
{
    public class BlockingContextTest
    {
        public void StartTest()
        {
            //execute the console input thread in background
            new Action(StartConsoleLoop).CreateThread().Start();

            //block the main thread while the background thread is executing
            blockingContext.Block();

            System.Console.WriteLine("Test completed\r\nPress any key to continue...");
            System.Console.Read();
        }

        private readonly BlockingContext blockingContext = new BlockingContext();

        private void StartConsoleLoop()
        {
            bool keepAlive = true;

            while (keepAlive)
            {
                System.Console.Write("$:");

                string cin = System.Console.ReadLine();

                if (cin == null || cin.IsNullOrEmpty())
                    continue;

                switch (cin.ToLower())
                {
                    case "exit":
                        keepAlive = false;
                        break;
                    default:
                        System.Console.WriteLine("Unknown command '{0}'".FormatThis(cin));
                        break;
                }
            }

            blockingContext.Unblock();
        }
    }
}
