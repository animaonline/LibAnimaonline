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
using System.Reflection.Emit;

namespace Animaonline.ILTools
{
    internal static class OpCodesTable
    {
        private static OpCode[] _singleByteOpCodes;
        private static OpCode[] _multiByteOpCodes;

        public static OpCode[] SingleByteOpCodes
        {
            get
            {
                if (_singleByteOpCodes == null || _multiByteOpCodes == null)
                    LoadOpCodes();

                return _singleByteOpCodes;
            }
        }

        public static OpCode[] MultiByteOpCodes
        {
            get
            {
                if (_singleByteOpCodes == null || _multiByteOpCodes == null)
                    LoadOpCodes();

                return _multiByteOpCodes;
            }
        }

        private static void LoadOpCodes()
        {
            _singleByteOpCodes = new OpCode[0x100];
            _multiByteOpCodes = new OpCode[0x100];

            var opCodesFields = typeof(OpCodes).GetFields();

            foreach (var fieldInfo in opCodesFields)
            {
                if (fieldInfo.FieldType != typeof(OpCode))
                    continue;

                var opCode = (OpCode)fieldInfo.GetValue(null);

                var opCodeValue = (ushort)opCode.Value;

                //1 byte
                if (opCodeValue < 0x100)
                    _singleByteOpCodes[opCodeValue] = opCode;
                //greater than 1 byte
                else
                {
                    if ((opCodeValue & 0xff00) != 0xfe00)
                        throw new Exception("Invalid OpCode.");

                    _multiByteOpCodes[opCodeValue & 0xff] = opCode;
                }
            }
        }
    }
}