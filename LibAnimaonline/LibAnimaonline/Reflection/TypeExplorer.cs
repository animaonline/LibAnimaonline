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
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Animaonline.Reflection
{
    public static class TypeExplorer
    {
        #region Public Static Methods

        /// <summary>
        /// Gets a list of loaded types in the current AppDomain
        /// </summary>
        public static List<Type> GetLoadedTypes()
        {
            return _getLoadedTypes(AppDomain.CurrentDomain);
        }

        /// <summary>
        /// Gets a list of loaded types in the specified AppDomain
        /// </summary>
        /// <param name="appDomain"></param>
        public static List<Type> GetLoadedTypes(AppDomain appDomain)
        {
            return _getLoadedTypes(appDomain);
        }

        public static List<Type> GetPublicLoadedTypes(AppDomain appDomain)
        {
            return _getLoadedTypes(appDomain, true);
        }

        public static List<string> GetUniqueNamespaces(IEnumerable<Type> types)
        {
            var uniqueNamespaces = new ConcurrentBag<string>();

            Parallel.ForEach(types, type =>
            {
                if (!uniqueNamespaces.Contains(type.Namespace) && !string.IsNullOrEmpty(type.Namespace))
                    uniqueNamespaces.Add(type.Namespace);
            });

            var sortedList = uniqueNamespaces.OrderBy(o => o).ToList();

            return sortedList;
        }

        public static List<Type> GetNamespaceTypes(string ns, IEnumerable<Type> loadedTypes)
        {
            var plP = loadedTypes.AsParallel(); //parallelization provider

            var nsTypes = from p in plP
                          where p.Namespace == ns
                          select p;

            return nsTypes.OrderBy(o => o.Name).ToList();
        }

        public static IEnumerable<MemberInfo> GetMembers(Type type, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Static)
        {
            var constructors = type.GetConstructors(bindingAttr).Where(w => !w.IsSpecialName).ToArray();

            var properties = type.GetProperties(bindingAttr).Where(w => !w.IsSpecialName).ToArray();

            var methods = type.GetMethods(bindingAttr).Where(w => !w.IsSpecialName).ToArray();

            var events = type.GetEvents(bindingAttr).Where(w => !w.IsSpecialName).ToArray();

            var members = new List<MemberInfo>();

            members.AddRange(constructors);
            members.AddRange(properties);
            members.AddRange(events);
            members.AddRange(methods);

            return members.OrderBy(o => o.Name).ToList();
        }

        #endregion

        #region Private Static Methods

        private static List<Type> _getLoadedTypes(AppDomain appDomain, bool onlyPublicTypes = false)
        {
            var loadedAssemblies = appDomain.GetAssemblies();

            var loadedTypes = new ConcurrentBag<Type>();

            Parallel.ForEach(loadedAssemblies, asm =>
            {
                Type[] asmTypes;
                if (onlyPublicTypes)
                    asmTypes = asm.GetExportedTypes();
                else
                    asmTypes = asm.GetTypes();

                foreach (var asmType in asmTypes)
                    loadedTypes.Add(asmType);
            });

            return loadedTypes.ToList();
        }

        #endregion
    }
}
