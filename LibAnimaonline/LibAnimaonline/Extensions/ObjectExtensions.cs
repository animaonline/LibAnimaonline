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
using Animaonline.Reflection;

// ReSharper disable once CheckNamespace

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
    /// Frees used resources
    /// </summary>
    /// <param name="o"></param>
    public static void DisposeChangeWatcher(this object o)
    {
        ChangeWatcher.Dispose(o);
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

    public static void ToConsole(this object o)
    {
        Console.WriteLine(o);
    }
    public static void ToConsole(this object o, params object[] args)
    {
        Console.WriteLine(o.ToString(), args);
    }
}
