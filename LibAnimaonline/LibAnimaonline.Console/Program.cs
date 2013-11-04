using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Animaonline.Threading;

namespace LibAnimaonline.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            var blockingContextTest = new BlockingContextTest();

            blockingContextTest.StartTest();
        }
    }
}
