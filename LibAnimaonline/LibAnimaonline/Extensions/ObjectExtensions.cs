using Animaonline.Reflection;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class ObjectExtensions
    {

        /// <summary>
        /// Commits all the changes made to public properties in this object since the last time AcceptChanges() was called.
        /// </summary>
        public static void AcceptChanges(this object o)
        {
            ChangeWatcher.AcceptChanges(o);
        }

        /// <summary>
        /// Indicates whether this object has changed or not since AcceptChanges() was called.
        /// </summary>
        public static bool HasChanges(this object o)
        {
            return ChangeWatcher.HasChanges(o);
        }

        /// <summary>
        ///  Gets a list of all public properties that were changed since AcceptChanges() was called.
        /// </summary>
        public static ChangeWatcher.ChangedPropertiesCollection GetChangedProperties(this object o, Type requiredAttribute = null)
        {
            return ChangeWatcher.GetChangedProperties(o);
        }
    }
}
