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
            throw new NotImplementedException();
        }

        public void PassCommand(string message)
        {
            throw new NotImplementedException();
        }
    }
}
