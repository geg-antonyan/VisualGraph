using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antonyan.Graphs.Gui.Forms
{
    public partial class StoredGraphNameForm : Form
    {
        public bool Ok { get; private set; }
        public string ListName { get; private set; }
        public StoredGraphNameForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Ok = true;
            ListName = txtName.Text;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Ok = false;
            Close();
        }
    }
}
