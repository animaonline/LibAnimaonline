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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class EnumerableExtensions
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
            return list.Except(new[] { except });
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