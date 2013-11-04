using System.Diagnostics;
using System.Reflection;
using System.Threading;
using LibAnimaonline.Console.Tests;
using LibAnimaonline.Console.Tests.ILTools;
using System.Linq;
using LibAnimaonline.Console.Tests.Reflection;

namespace LibAnimaonline.Console
{
    static class Program
    {
        static void Main(string[] args)
        { 
            ITest test;

            //test= new BlockingContextTest();

            //test = new TypeExplorerTest();

            test = new ILToolsTest();

            test.StartTest();
        }
    }
}
