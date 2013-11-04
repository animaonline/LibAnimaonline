using System;
using System.Reflection;
using System.Text;
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
