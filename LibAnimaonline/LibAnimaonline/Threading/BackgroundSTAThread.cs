using System.Threading;

namespace Animaonline.Threading
{
    public class BackgroundSTAThread
    {
        public static Thread Create(ParameterizedThreadStart start)
        {
            var thread = new Thread(start);

            thread = Prepare(thread);

            return thread;
        }

        public static Thread Create(ThreadStart start)
        {
            var thread = new Thread(start);

            thread = Prepare(thread);

            return thread;
        }

        public static Thread Create(ParameterizedThreadStart start, int maxStackSize)
        {
            var thread = new Thread(start, maxStackSize);

            thread = Prepare(thread);

            return thread;
        }

        public static Thread Create(ThreadStart start, int maxStackSize)
        {
            var thread = new Thread(start, maxStackSize);

            thread = Prepare(thread);

            return thread;
        }

        private static Thread Prepare(Thread thread)
        {
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            return thread;
        }
    }
}
