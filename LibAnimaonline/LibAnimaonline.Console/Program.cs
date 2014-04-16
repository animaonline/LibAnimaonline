﻿/*
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
using LibAnimaonline.Console.Tests;
using LibAnimaonline.Console.Tests.Events;

namespace LibAnimaonline.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            ITest test;

            //test= new BlockingContextTest();

            //test = new TypeExplorerTest();

            //test = new ILToolsTest();

            test = new SmartEventTest();

            test.StartTest();
        }
    }
}
