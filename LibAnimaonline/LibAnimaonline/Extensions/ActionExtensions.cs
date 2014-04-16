using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public static class ActionExtensions
    {
        #region Start Task

        public static Task StartTask(this Action action)
        {
            return Task.Factory.StartNew(action);
        }

        public static Task StartTask<T>(this Action<T> action, T state)
        {
            var acWrapper = new Action<object>((s) => action((T)s));

            return Task.Factory.StartNew(acWrapper, state);
        }

        public static Thread CreateThread(this Action action)
        { 
            return new Thread(() => action());
        }

        #endregion

        #region Create Task

        public static Task CreateTask(this Action action)
        {
            return new Task(action);
        }

        #endregion
    }
}