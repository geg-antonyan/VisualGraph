using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Antonyan.Graphs.Gui;

namespace Antonyan.Graphs.Gui.Forms
{
    public partial class ResultForm : Form
    {
        public string TaskName { get; private set; }
        public string AlgorithmName { get; private set; }
        public string ResultText { get; private set; }
        public Stream Stream { get; private set; }
        public ResultForm(string task, string algorithmName, string result)
        {
            InitializeComponent();
            lblTask.Text += task;
            lblAlgorithmName.Text += algorithmName;
            txtResult.Text = result;
            ResultText = txtResult.Text;
            TaskName = lblTask.Text;
            AlgorithmName = lblAlgorithmName.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Stream = saveFileDialog.OpenFile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Stream = null;
            Close();
        }
    }
}
