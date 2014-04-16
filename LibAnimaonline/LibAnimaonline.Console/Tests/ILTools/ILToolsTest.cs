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
using System.Reflection;
using System.Threading;
using Animaonline.ILTools.vCLR;

namespace LibAnimaonline.Console.Tests.ILTools
{
    public class ILToolsTest : ITest
    {
        public void StartTest()
        {
            var targetMethod = typeof(ILToolsTest).GetMethod("TestMethod", BindingFlags.NonPublic | BindingFlags.Instance);

            var methodIl = Animaonline.ILTools.ILTools.GetMethodIL(targetMethod);

            var virtualCLR = new VirtualCLR(vCLRScope.Class);

            virtualCLR.ExecuteILMethod(methodIl);

            System.Console.WriteLine("Test completed\r\nPress any key to continue...");
            System.Console.Read();
        }

        private void TestMethod()
        {
            for (int i = 0; i < 3; i++)
                System.Console.WriteLine(i);

            var t = new Thread(() => System.Console.WriteLine("Hello, from a thread!"));

            t.Start();

            var lambda = new Action(() => System.Console.WriteLine("Hello, from a lambda!"));

            lambda();

            var addFunc = new Func<int, int, int>((a, b) => a + b);

            var addResult = addFunc(5, 8);

            System.Console.WriteLine("Result: " + addResult);
        }
    }
}
