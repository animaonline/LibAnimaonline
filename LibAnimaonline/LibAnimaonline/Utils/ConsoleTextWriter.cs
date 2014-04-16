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
using System.Text;

namespace Animaonline.Utils
{
    public class ConsoleTextWriter : TextWriter
    {
        #region Public Constructor

        public ConsoleTextWriter(TextWriter wrap, Action<string> onWrite, Action<string> onWriteLine)
        {
            _writer = wrap;
            _buffer = new StringBuilder();

            this._onWrite = onWrite;
            this._onWriteLine = onWriteLine;
        }

        #endregion

        #region Private Fields

        private readonly TextWriter _writer;
        private readonly StringBuilder _buffer;
        private readonly Action<string> _onWrite;
        private readonly Action<string> _onWriteLine;

        #endregion

        #region Overridden Methods

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(string value)
        {
            _writer.Write(value);
            _buffer.Append(value);

            if (_onWrite != null)
                _onWrite(value);
        }

        public override void WriteLine(string value)
        {
            _writer.WriteLine(value);
            _buffer.AppendLine(value);

            if (_onWriteLine != null)
                _onWriteLine(value);
        }

        public override void WriteLine()
        {
            _writer.WriteLine();
            _buffer.AppendLine();

            if (_onWriteLine != null)
                _onWriteLine(null);
        }

        #endregion

        #region Public Properties

        public string Buffer
        {
            get
            {
                return _buffer.ToString();
            }
        }

        #endregion
    }
}