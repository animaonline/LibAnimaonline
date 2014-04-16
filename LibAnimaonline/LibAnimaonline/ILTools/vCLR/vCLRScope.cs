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

namespace Animaonline.ILTools.vCLR
{
    public enum vCLRScope
    {
        /// <summary>
        /// Execution is limited to the input method
        /// </summary>
        Method,
        /// <summary>
        /// Execution is limited to all methods in the input method's declaring type.
        /// </summary>
        Class,
        /// <summary>
        /// Executes all methods virtually
        /// </summary>
        Global
    }
}
