using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#region Extensions

// ReSharper disable CheckNamespace
namespace System
// ReSharper restore CheckNamespace
{
    public static class StringExtensions
    {
        public static string FormatThis(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var listItem in list)
                action(listItem);
        }

        public static void ParallelForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            Parallel.ForEach(list, action);
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return !(list != null && list.Any());
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, T except)
        {
            return list.Except(new T[] { except });
        }

        public static IEnumerable<T> ExceptParams<T>(this IEnumerable<T> list, params T[] except)
        {
            return list.Except(except);
        }

        public static bool IsNullOrEmpty(this IEnumerable<string> list)
        {
            var enumerable = list as string[] ?? list.ToArray();

            if (list == null || !enumerable.Any())
                return false;

            return enumerable.GroupBy(string.IsNullOrEmpty).Any(w => w.Key);
        }
    }

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

#endregion