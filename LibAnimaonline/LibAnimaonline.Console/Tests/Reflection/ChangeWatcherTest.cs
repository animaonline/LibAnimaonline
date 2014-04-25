using System.ComponentModel;

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

            System.Console.WriteLine("Following properties have changed");

            changedProperties.ForEach(p => p.ToConsole());

            var form = new ChangeWatcherTestForm();

            form.ShowDialog();
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
                R = r;
                G = g;
                B = b;
            }
        }
    }
}
