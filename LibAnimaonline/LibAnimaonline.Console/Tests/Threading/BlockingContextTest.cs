/*
LibAnimaonline - A set of useful cross platform helper classes to use with .NET, written in C#
Copyright (C) 2007-2014  Roman Alifanov - animaonline@gmail.com - http://www.animaonline.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see http://www.gnu.org/licenses/
 */
using System;
using Animaonline.Threading;

namespace LibAnimaonline.Console.Tests.Threading
{
    public class BlockingContextTest : ITest
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
                    case "kill3s":
                        blockingContext.ScheduleUnblock(3000);
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
