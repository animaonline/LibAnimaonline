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
    /// <summary>
    /// Used for capturing console output.
    /// </summary>
    public static class ConsoleOutputCapture
    {
        public static void Start()
        {
            _coutStream = new MemoryStream();
            _coutWriter = new StreamWriter(_coutStream);

            if (_originalOutStream == null)
                _originalOutStream = Console.Out;

            //Redirect Console's output stream to our stream.
            Console.SetOut(_coutWriter);

            //capture console output till Dispose has been called.
        }

        public static string Stop()
        {
            if (_coutWriter != null && _coutStream != null)
            {
                //clear all buffers.
                _coutWriter.Flush();

                var coutString = _coutWriter.Encoding.GetString(_coutStream.ToArray());

                Console.SetOut(_originalOutStream);

                //dispose
                _coutStream.Dispose();
                _coutWriter.Dispose();

                return coutString;
            }

            return string.Empty;
        }

        /* fields */
        static MemoryStream _coutStream;
        static StreamWriter _coutWriter;
        private static TextWriter _originalOutStream;
    }
}
