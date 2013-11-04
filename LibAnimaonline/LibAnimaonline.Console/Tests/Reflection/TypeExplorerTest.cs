using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
