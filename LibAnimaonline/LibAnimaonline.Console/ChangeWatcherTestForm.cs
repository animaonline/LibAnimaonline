using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LibAnimaonline.Console
{
    public partial class ChangeWatcherTestForm : Form
    {
        public ChangeWatcherTestForm()
        {
            InitializeComponent();

            Application.EnableVisualStyles();

            propertyGrid1.SelectedObject = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var changes = this.GetChangedProperties();

            changes.RestoreAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.AcceptChanges();
            Invalidate();
            propertyGrid1.Refresh();
        }
    }
}
