using System.Reflection;
using Animaonline.Reflection;

public static class PropertyInfoExtensions
{
    public static bool HasCustomAttribute<T>(this PropertyInfo property, bool inherit = false) where T : class
    {
        return AttributeHelper.HasCustomAttribute<T>(property, inherit);
    }

    public static T GetCustomAttribute<T>(this PropertyInfo property, bool inherit = false) where T : class
    {
        return AttributeHelper.GetCustomAttribute<T>(property, inherit);
    }
}
