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
using System.Globalization;
using System.Reflection.Emit;

namespace Animaonline.ILTools
{
    public class ILInstruction
    {
        #region Public Constructor

        public ILInstruction(OpCode opCode)
        {
            OpCodeInfo = opCode;
            OpCode = (EnumOpCode)opCode.Value;
        }

        #endregion

        #region Public Properties

        public EnumOpCode OpCode { get; private set; }

        public OpCode OpCodeInfo { get; private set; }

        public object Operand { get; set; }

        public byte[] OperandData { get; set; }

        public int Offset { get; set; }

        #endregion

        #region Public Methods

        public string GetCode()
        {
            string result = "";
            result += GetExpandedOffset(Offset) + " : " + OpCodeInfo;
            if (Operand != null)
            {
                switch (OpCodeInfo.OperandType)
                {
                    case OperandType.InlineField:
                        var fOperand = ((System.Reflection.FieldInfo)Operand);
                        result += " " + ProcessSpecialTypes(fOperand.FieldType.ToString()) + " " +
                            ProcessSpecialTypes(fOperand.ReflectedType.ToString()) +
                            "::" + fOperand.Name + "";
                        break;
                    case OperandType.InlineMethod:
                        try
                        {
                            var mOperand = (System.Reflection.MethodInfo)Operand;
                            result += " ";
                            if (!mOperand.IsStatic) result += "instance ";
                            result += ProcessSpecialTypes(mOperand.ReturnType.ToString()) +
                                " " + ProcessSpecialTypes(mOperand.ReflectedType.ToString()) +
                                "::" + mOperand.Name + "()";
                        }
                        catch
                        {
                            try
                            {
                                var mOperand = (System.Reflection.ConstructorInfo)Operand;
                                result += " ";
                                if (!mOperand.IsStatic) result += "instance ";
                                result += "void " +
                                    ProcessSpecialTypes(mOperand.ReflectedType.ToString()) +
                                    "::" + mOperand.Name + "()";
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case OperandType.ShortInlineBrTarget:
                    case OperandType.InlineBrTarget:
                        result += " " + GetExpandedOffset((int)Operand);
                        break;
                    case OperandType.InlineType:
                        result += " " + ProcessSpecialTypes(Operand.ToString());
                        break;
                    case OperandType.InlineString:
                        if (Operand.ToString() == "\r\n") result += " \"\\r\\n\"";
                        else result += " \"" + Operand + "\"";
                        break;
                    case OperandType.ShortInlineVar:
                        result += Operand.ToString();
                        break;
                    case OperandType.InlineI:
                    case OperandType.InlineI8:
                    case OperandType.InlineR:
                    case OperandType.ShortInlineI:
                    case OperandType.ShortInlineR:
                        result += Operand.ToString();
                        break;
                    case OperandType.InlineTok:
                        if (Operand is Type)
                            result += ((Type)Operand).FullName;
                        else
                            result += "not supported";
                        break;

                    default: result += "not supported"; break;
                }
            }
            return result;

        }

        #endregion

        #region Private Static Methods

        private static string ProcessSpecialTypes(string typeName)
        {
            string result = typeName;
            switch (typeName)
            {
                case "System.string":
                case "System.String":
                case "String":
                    result = "string"; break;
                case "System.Int32":
                case "Int":
                case "Int32":
                    result = "int"; break;
            }
            return result;
        }

        private string GetExpandedOffset(long offset)
        {
            string result = offset.ToString(CultureInfo.InvariantCulture);
            for (int i = 0; result.Length < 4; i++)
            {
                result = "0" + result;
            }
            return result;
        }

        #endregion

        #region Overridden Methods

        public override string ToString()
        {
            return GetCode();
        }

        #endregion
    }
}