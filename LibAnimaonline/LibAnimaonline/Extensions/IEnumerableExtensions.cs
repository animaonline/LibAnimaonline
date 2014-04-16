using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System
{
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
}