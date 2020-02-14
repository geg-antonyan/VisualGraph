using Antonyan.Graphs.Data;

namespace Antonyan.Graphs
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tlbtnCrtGraph = new System.Windows.Forms.ToolStripButton();
            this.tlbtnAddEdge = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbtnCrtGraph,
            this.tlbtnAddEdge});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(800, 27);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip4";
            // 
            // tlbtnCrtGraph
            // 
            this.tlbtnCrtGraph.Image = global::Antonyan.Graphs.Properties.Resources.plus;
            this.tlbtnCrtGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnCrtGraph.Name = "tlbtnCrtGraph";
            this.tlbtnCrtGraph.Size = new System.Drawing.Size(125, 24);
            this.tlbtnCrtGraph.Text = "Создать граф";
            this.tlbtnCrtGraph.Click += new System.EventHandler(this.tlbtnCrtGraph_Click);
            // 
            // tlbtnAddEdge
            // 
            this.tlbtnAddEdge.Image = global::Antonyan.Graphs.Properties.Resources.arrow;
            this.tlbtnAddEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnAddEdge.Name = "tlbtnAddEdge";
            this.tlbtnAddEdge.Size = new System.Drawing.Size(178, 24);
            this.tlbtnAddEdge.Text = "Соединить вершины";
            this.tlbtnAddEdge.Click += new System.EventHandler(this.tlbtnAddEdge_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripMain);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseClick);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
       
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton tlbtnCrtGraph;
        private System.Windows.Forms.ToolStripButton tlbtnCreatEdge;
        private System.Windows.Forms.ToolStripButton tlbtnAddEdge;
    }
}

