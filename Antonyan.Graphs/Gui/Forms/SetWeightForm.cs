using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Gui.Forms
{
    public partial class SetWeightForm : Form
    {
        public bool Ok { get; private set; }
        public string Weight { get; private set; }
        public SetWeightForm()
        {

            InitializeComponent();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            Ok = true;
            Weight = txtWeight.Text;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Ok = false;
            Close();
        }
    }
}
