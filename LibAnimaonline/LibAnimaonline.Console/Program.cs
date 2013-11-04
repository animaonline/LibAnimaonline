using LibAnimaonline.Console.Tests;

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
