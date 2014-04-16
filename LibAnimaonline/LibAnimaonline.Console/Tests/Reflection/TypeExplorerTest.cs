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
using System.Reflection;
using Animaonline.Reflection;

namespace LibAnimaonline.Console.Tests.Reflection
{
    public class TypeExplorerTest : ITest
    {
        public void StartTest()
        {
            //get all loaded types
            var loadedTypes = TypeExplorer.GetLoadedTypes(AppDomain.CurrentDomain);
            var loadedMembers = new ConcurrentBag<MemberInfo>();

            //get all loaded members
            loadedTypes.ParallelForEach(type =>
            {
                var typeMembers = TypeExplorer.GetMembers(type);

                typeMembers.ParallelForEach(loadedMembers.Add);
            });


            //get a list of loaded types that belong to the given namespace
            var systemNsTypes = TypeExplorer.GetNamespaceTypes("System", loadedTypes);

            //get a list of unique namespaces from the list of loaded types
            var uniqueNamespaces = TypeExplorer.GetUniqueNamespaces(loadedTypes);
        }
    }
}
