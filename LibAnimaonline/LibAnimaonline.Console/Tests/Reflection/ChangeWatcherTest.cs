using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LibAnimaonline.Console.Tests.Reflection
{
    public class ChangeWatcherTest : ITest
    {
        public void StartTest()
        {
            var testTarget = new TestTarget();

            //set properties
            testTarget.Set(236, 225, 15);

            //commit all the changes made to public properties in testTarget since the last time AcceptChanges() was called.
            testTarget.AcceptChanges();

            //set properties
            testTarget.Set(44, 138, 207);

            var changedProperties = testTarget.GetChangedProperties();
            changedProperties["B"].DirectValue = true;
        }

        public class TestTarget
        {
            [DisplayName("ColorR")]
            public byte R { get; set; }
            public byte G { get; set; }
            public byte B { get; set; }

            public string RGB
            {
                get
                {
                    return "#{0}{1}{2}".FormatThis(R.ToString("X2"), G.ToString("X2"), B.ToString("X2"));
                }
            }

            public void Set(byte r, byte g, byte b)
            {
                this.R = r;
                this.G = g;
                this.B = b;
            }
        }
    }
}
