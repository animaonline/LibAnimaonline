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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Animaonline.Reflection
{
    public static class ChangeWatcher
    {
        #region Constants

        private const bool IncludeAllProperties = false; // <- evil

        #endregion

        #region Child Classes

        public class ChangeWatchAttribute : Attribute { }

        public class ChangeWatchProperty
        {
            #region Public Constructors

            public ChangeWatchProperty() { }

            public ChangeWatchProperty(object owner, PropertyInfo property, dynamic previousValue, dynamic currentValue)
            {
                _owner = owner;
                Property = property;
                PreviousValue = previousValue;
                CurrentValue = currentValue;
            }

            #endregion

            #region Private Fields

            private readonly object _owner;

            #endregion

            #region Public Properties

            /// <summary>
            /// Owner object
            /// </summary>
            public object Owner { get { return _owner; } }

            /// <summary>
            /// Property that was changed
            /// </summary>
            public PropertyInfo Property { get; internal set; }

            /// <summary>
            /// Value before the change
            /// </summary>
            public dynamic PreviousValue { get; internal set; }

            /// <summary>
            /// Current value
            /// </summary>
            public dynamic CurrentValue { get; internal set; }

            /// <summary>
            /// Provides direct access to the property value
            /// </summary>
            public dynamic DirectValue
            {
                get { return Property != null && _owner != null ? Property.GetValue(_owner, null) : null; }
                set
                {
                    try
                    {
                        if (CurrentValue == value)
                            return;

                        dynamic targetValue;

                        if (value.GetType() != Property.PropertyType)
                            targetValue = Convert.ChangeType(value, Property.PropertyType);
                        else
                            targetValue = value;

                        Property.SetValue(_owner, targetValue, null);
                        //PreviousValue = CurrentValue;
                        CurrentValue = value;
                    }
                    catch (Exception e)
                    {
                        if (e is OverflowException ||
                            e is FormatException ||
                            e is ArgumentException ||
                            e is InvalidCastException)
                            return;
                        throw;
                    }
                }
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Restores the previous value (if any)
            /// </summary>
            public void RestorePrevious()
            {
                if (CurrentValue != PreviousValue)
                    DirectValue = PreviousValue;
            }

            public override string ToString()
            {
                if (Property == null)
                    return base.ToString();

                string previousString = PreviousValue == null ? "null" : PreviousValue.ToString();
                string currentString = CurrentValue == null ? "null" : CurrentValue.ToString();

                return string.Format("{0} - (Previous: '{1}', Current: '{2}')", Property.Name, previousString, currentString);
            }

            #endregion
        }

        /// <summary>
        /// A collection of ChangeWatchProperty objects
        /// </summary>
        public class ChangedPropertiesCollection : List<ChangeWatchProperty>
        {
            /// <summary>
            /// Gets a property by name
            /// </summary>
            public ChangeWatchProperty this[string name]
            {
                get
                {
                    var property = (from prop in this
                                    where prop.Property != null && prop.Property.Name == name
                                    select prop).FirstOrDefault();

                    return property;
                }
            }

            public void RestoreAll()
            {
                foreach (var item in this)
                {
                    item.RestorePrevious();
                }
            }
        }

        #endregion

        #region Private Static Fields

        private static readonly Dictionary<object, Dictionary<PropertyInfo, ChangeWatchProperty>> ChangeCache = new Dictionary<object, Dictionary<PropertyInfo, ChangeWatchProperty>>();

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Commits all the changes made to public properties in this object since the last time AcceptChanges() was called.
        /// </summary>
        public static void AcceptChanges(object obj)
        {
            lock (obj)
            {
                if (!ChangeCache.ContainsKey(obj)) //First time AcceptChanges
                {
                    //Get Type properties

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    // ReSharper disable once UnreachableCode
                    // ReSharper disable once CSharpWarnings::CS0162
                    var properties = IncludeAllProperties ? obj.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static) : obj.GetType().GetProperties();

                    //Create a Dictionary entry
                    ChangeCache[obj] = new Dictionary<PropertyInfo, ChangeWatchProperty>();

                    //Store current values
                    foreach (var propertyInfo in properties)
                    {
                        try
                        {
                            //ignore properties without getters
                            if (!propertyInfo.CanRead)
                                continue;

                            var indexParams = propertyInfo.GetIndexParameters();

                            if (indexParams.Length > 0)
                                continue;

                            //Get current value
                            var propertyValue = propertyInfo.GetValue(obj, null);

                            //Store it
                            ChangeCache[obj].Add(propertyInfo, new ChangeWatchProperty(obj, propertyInfo, propertyValue, propertyValue));
                        }
                        catch (TargetInvocationException)
                        {
                            //Ignore internal / weird property implementations
                        }
                    }
                }
                else
                    foreach (var propertyInfo in ChangeCache[obj].Keys) //Re-use cached Properties
                    {
                        //Get current value
                        var propertyValue = propertyInfo.GetValue(obj, null);

                        //Update Cache
                        ChangeCache[obj][propertyInfo].CurrentValue = propertyValue;
                    }
            }

        }

        /// <summary>
        /// Indicates whether this object has changed or not since AcceptChanges() was called.
        /// </summary>
        public static bool HasChanges(object obj)
        {
            lock (obj)
            {
                var changes = GetChangedProperties(obj);

                return changes.Any();
            }
        }

        /// <summary>
        ///  Gets a list of all public properties that were changed since AcceptChanges() was called.
        /// </summary>
        public static ChangedPropertiesCollection GetChangedProperties(object obj, Type requiredAttribute = null)
        {
            lock (obj)
            {
                //Call AcceptChanges if there is no cached data available
                if (!ChangeCache.ContainsKey(obj))
                    AcceptChanges(obj);

                var changesCache = ChangeCache[obj];

                //A list of changed properties
                var changes = new ChangedPropertiesCollection();

                foreach (var propertyInfo in changesCache.Keys)
                {
                    //Ignore properties without the requiredAttribute (if any)
                    if (requiredAttribute != null && !Attribute.IsDefined(propertyInfo, requiredAttribute))
                        continue;

                    //Get current value
                    dynamic currentPropertyValue = propertyInfo.GetValue(obj, null);

                    //The value hasn't changed
                    if (currentPropertyValue is bool && changesCache[propertyInfo].PreviousValue == (bool)currentPropertyValue)
                        continue;
                    if (changesCache[propertyInfo].PreviousValue != null && changesCache[propertyInfo].PreviousValue.Equals(currentPropertyValue))
                        continue;
                    if (changesCache[propertyInfo].PreviousValue == null && currentPropertyValue == null)
                        continue;

                    //The value has changed, store it
                    changesCache[propertyInfo].CurrentValue = currentPropertyValue;

                    /*yield return changesCache[propertyInfo];*/
                    changes.Add(changesCache[propertyInfo]);
                }

                return changes;
            }
        }

        #endregion
    }
}
