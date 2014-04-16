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

using System.Collections.Generic;

namespace Animaonline.ILTools.vCLR
{
    public class VCLRExecContext
    {
        #region Public Constructor

        public VCLRExecContext(MethodILInfo methodILInfo, int localsSize = 1024)
        {
            this.MethodIL = methodILInfo;
            this.EvaluationStack = new Stack<object>();
            this.MethodLocals = new object[localsSize];
        }

        #endregion

        #region Public Properties

        public MethodILInfo MethodIL { get; set; }
        public object[] Arguments { get; set; }
        public object ObjectInstance;
        public Stack<object> EvaluationStack { get; set; }
        public object[] MethodLocals { get; set; }

        public bool HasArguments
        {
            get
            {
                return Arguments != null;
            }
        }

        public bool HasObjectInstance
        {
            get
            {
                return ObjectInstance != null;
            }
        }

        #endregion

        #region Public Methods

        public void StackPush(object obj)
        {
            EvaluationStack.Push(obj);
        }

        public void StackPushRef(ref object obj)
        {
            EvaluationStack.Push(obj);
        }

        public object StackPop()
        {
            return EvaluationStack.Pop();
        }

        /// <summary>
        /// Pushes the local at the given index onto the evaluation stack
        /// </summary>
        /// <param name="index"></param>
        public void Ldloc(int index)
        {
            StackPush(MethodLocals[index]);
        }

        public void Stloc(int index)
        {
            //pop the stack and set the local at the given index
            MethodLocals[index] = StackPop();
        }

        #endregion
    }
}