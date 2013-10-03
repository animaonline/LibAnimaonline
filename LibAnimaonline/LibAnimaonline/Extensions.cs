using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static IEnumerable<T> Except<T>(this IEnumerable<T> list, T except)
        {
            return list.Except(new T[] { except });
        }

        public static bool IsNullOrEmpty(this IEnumerable<string> list)
        {
            var enumerable = list as string[] ?? list.ToArray();

            if (list == null || !enumerable.Any())
                return false;

            return enumerable.GroupBy(string.IsNullOrEmpty).Any(w => w.Key);
        }
    }
}

#endregion