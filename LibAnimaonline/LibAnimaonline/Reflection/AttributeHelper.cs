using System;
using System.Linq;
using System.Reflection;

namespace Animaonline.Reflection
{
    public class AttributeHelper
    {
        public static bool HasCustomAttribute<T>(PropertyInfo property, bool inherit)
        {
            return property != null && property.GetCustomAttributes(typeof(T), inherit).Any();
        }


        public static T GetCustomAttribute<T>(PropertyInfo property, bool inherit) where T : class
        {
            return property == null ? null : property.GetCustomAttributes(typeof(T), inherit).Cast<T>().FirstOrDefault();
        }
    }
}
