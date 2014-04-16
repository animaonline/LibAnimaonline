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
using System.IO;

namespace Animaonline.Utils
{
    public class ConsoleInputCapture
    {
        /// <summary>
        /// Will trigger the provided callbacks when Console's Input stream requires it.
        /// </summary>
        public static void Start(Func<string> readLine, Func<int> read)
        {
            if (_originalCinHandler == null)
                _originalCinHandler = Console.In;

            var cinHandler = new ConsoleInputHandler(readLine, read);

            Console.SetIn(cinHandler);
        }

        static TextReader _originalCinHandler;

        public static void Stop()
        {
            Console.SetIn(_originalCinHandler);
        }

        private class ConsoleInputHandler : TextReader
        {
            readonly Func<string> _readLine;
            readonly Func<int> _read;

            public ConsoleInputHandler(Func<string> readLine = null, Func<int> read = null)
            {
                _readLine = readLine;
                _read = read;
            }

            public override int Read(char[] buffer, int index, int count)
            {
                return base.Read(buffer, index, count);
            }

            public override int Read()
            {
                return _read != null ? _read() : base.Read();
            }

            public override string ReadLine()
            {
                return _readLine != null ? _readLine() : base.ReadLine();
            }
        }
    }
}
