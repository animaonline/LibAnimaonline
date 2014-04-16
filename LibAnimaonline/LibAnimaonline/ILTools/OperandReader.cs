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

namespace Animaonline.ILTools
{
    internal static class OperandReader
    {
        public static int ReadInt16(byte[] il, ref int position)
        {
            var i1 = il[position];
            var i2 = il[position + 1] << 8;

            position += sizeof(Int16);

            return i1 | i2;
        }

        public static ushort ReadUInt16(byte[] il, ref int position)
        {
            var i1 = il[position];
            var i2 = il[position + 1] << 8;

            position += sizeof(ushort);

            return (ushort)(i1 | i2);
        }

        public static int ReadInt32(byte[] il, ref int position)
        {
            var i1 = il[position];
            var i2 = il[position + 1] << 8;
            var i3 = il[position + 2] << 16;
            var i4 = il[position + 3] << 24;

            position += sizeof(int);

            return i1 | i2 | i3 | i4;
        }

        public static ulong ReadInt64(byte[] il, ref int position)
        {
            var i1 = il[position];
            var i2 = il[position + 1] << 8;
            var i3 = il[position + 2] << 16;
            var i4 = il[position + 3] << 24;
            var i5 = il[position + 4] << 32;
            var i6 = il[position + 5] << 40;
            var i7 = il[position + 6] << 48;
            var i8 = il[position + 7] << 56;

            position += sizeof(ulong);

            return (ulong)(i1 | i2 | i3 | i4 | i5 | i6 | i7 | i8);
        }

        public static double ReadDouble(byte[] il, ref int position)
        {
            var i1 = il[position];
            var i2 = il[position + 1] << 8;
            var i3 = il[position + 2] << 16;
            var i4 = il[position + 3] << 24;
            var i5 = il[position + 4] << 32;
            var i6 = il[position + 5] << 40;
            var i7 = il[position + 6] << 48;
            var i8 = il[position + 7] << 56;

            position += sizeof(double);

            return i1 | i2 | i3 | i4 | i5 | i6 | i7 | i8;
        }

        public static sbyte ReadSByte(byte[] il, ref int position)
        {
            return (sbyte)il[position++];
        }

        public static byte ReadByte(byte[] il, ref int position)
        {
            return il[position++];
        }

        public static Single ReadSingle(byte[] il, ref int position)
        {
            return ((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18);
        }
    }
}