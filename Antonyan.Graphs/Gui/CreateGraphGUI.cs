using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antonyan.Graphs.Gui
{
    public partial class CreateGraphGUI : Form
    {
        public bool Ok { get; private set; }
        public bool Oriented { get; private set; }
        public bool Weighted { get; private set; }
        public CreateGraphGUI()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Ok = true;
            Oriented = rbGraph.Checked ? false : true;
            Weighted = rbWeighted.Checked ? true : false;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Ok = false;
            Close();
        }
    }
}
