using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs
{
    public partial class MainForm : Form, UserInterface
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public event EventHandler<UICommandEventArgs> CommandEnterd;

        public void FieldUpdate(object obj, EventArgs e)
        {
            throw new Exception(e.ToString());
        }

        public void PostMessage(string message)
        {
            testbutton.Text = message;
        }

        private void testbutton_Click(object sender, EventArgs e)
        {
            CommandEnterd.Invoke(this, new UICommandEventArgs("AddVertex 2 50 45"));
        }
    }
}
