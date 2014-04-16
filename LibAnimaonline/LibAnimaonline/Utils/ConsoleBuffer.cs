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
using System.Text;

namespace Animaonline.Utils
{
    public class ConsoleBuffer
    {
        public ConsoleBuffer(int maxLines = 200)
        {
            _buffer = new StringBuilder();
            _maxLines = maxLines;
        }

        private StringBuilder _buffer;
        private readonly int _maxLines;
        private int _lineCount;
        private readonly object _syncLock = new object();

        public string Buffer
        {
            get
            {
                _maintainConstraints();
                return _buffer.ToString();
            }
        }

        public void Clear()
        {
            lock (_syncLock)
            {
                _buffer.Clear();
                _lineCount = 0;
            }
        }

        public void Write(string value)
        {
            lock (_syncLock)
            {
                _maintainConstraints();

                _buffer.Append(value);

                _lineCount += value.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length;
            }
        }

        public void WriteLine(string line)
        {
            lock (_syncLock)
            {
                _maintainConstraints();

                _buffer.AppendFormat("{0}{1}", line, Environment.NewLine);

                _lineCount++;
            }
        }

        public void WriteLine()
        {
            lock (_syncLock)
            {
                _maintainConstraints();

                _buffer.AppendLine();

                _lineCount++;
            }
        }

        private void _maintainConstraints()
        {
            lock (_syncLock)
            {
                if (_lineCount > _maxLines)
                {
                    _buffer = _buffer.Remove(0, _buffer.Length / 2);
                    _lineCount = _lineCount / 2; 
                }
            }
        }
    }
}
