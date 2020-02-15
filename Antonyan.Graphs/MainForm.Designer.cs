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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbtnUndo = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRedo = new System.Windows.Forms.ToolStripButton();
            this.tlbtnCrtGraph = new System.Windows.Forms.ToolStripButton();
            this.tsbtnAddEdge = new System.Windows.Forms.ToolStripButton();
            this.tsbtnDetours = new System.Windows.Forms.ToolStripDropDownButton();
            this.subDetoursBtnDFS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnUndo,
            this.tsbtnRedo,
            this.tlbtnCrtGraph,
            this.tsbtnAddEdge,
            this.tsbtnDetours});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(600, 27);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip4";
            // 
            // tsbtnUndo
            // 
            this.tsbtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnUndo.Image = global::Antonyan.Graphs.Properties.Resources.Undo;
            this.tsbtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnUndo.Name = "tsbtnUndo";
            this.tsbtnUndo.Size = new System.Drawing.Size(24, 24);
            this.tsbtnUndo.Text = "Назад";
            this.tsbtnUndo.Click += new System.EventHandler(this.tsbtnUndo_Click);
            // 
            // tsbtnRedo
            // 
            this.tsbtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRedo.Image = global::Antonyan.Graphs.Properties.Resources.redo;
            this.tsbtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRedo.Name = "tsbtnRedo";
            this.tsbtnRedo.Size = new System.Drawing.Size(24, 24);
            this.tsbtnRedo.Text = "Вперед";
            this.tsbtnRedo.Click += new System.EventHandler(this.tsbtnRedo_Click);
            // 
            // tlbtnCrtGraph
            // 
            this.tlbtnCrtGraph.Image = global::Antonyan.Graphs.Properties.Resources.plus;
            this.tlbtnCrtGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnCrtGraph.Name = "tlbtnCrtGraph";
            this.tlbtnCrtGraph.Size = new System.Drawing.Size(104, 24);
            this.tlbtnCrtGraph.Text = "Создать граф";
            this.tlbtnCrtGraph.Click += new System.EventHandler(this.tlbtnCrtGraph_Click);
            // 
            // tsbtnAddEdge
            // 
            this.tsbtnAddEdge.Image = global::Antonyan.Graphs.Properties.Resources.arrow;
            this.tsbtnAddEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnAddEdge.Name = "tsbtnAddEdge";
            this.tsbtnAddEdge.Size = new System.Drawing.Size(146, 24);
            this.tsbtnAddEdge.Text = "Соединить вершины";
            this.tsbtnAddEdge.Click += new System.EventHandler(this.tlbtnAddEdge_Click);
            // 
            // tsbtnDetours
            // 
            this.tsbtnDetours.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnDetours.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subDetoursBtnDFS});
            this.tsbtnDetours.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnDetours.Image")));
            this.tsbtnDetours.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDetours.Name = "tsbtnDetours";
            this.tsbtnDetours.Size = new System.Drawing.Size(64, 24);
            this.tsbtnDetours.Text = "Обходы";
            // 
            // subDetoursBtnDFS
            // 
            this.subDetoursBtnDFS.Name = "subDetoursBtnDFS";
            this.subDetoursBtnDFS.Size = new System.Drawing.Size(180, 22);
            this.subDetoursBtnDFS.Text = "Обход в глубину";
            this.subDetoursBtnDFS.Click += new System.EventHandler(this.subDetoursBtnDFS_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.toolStripMain);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.ToolStripButton tsbtnAddEdge;
        private System.Windows.Forms.ToolStripButton tsbtnUndo;
        private System.Windows.Forms.ToolStripButton tsbtnRedo;
        private System.Windows.Forms.ToolStripDropDownButton tsbtnDetours;
        private System.Windows.Forms.ToolStripMenuItem subDetoursBtnDFS;
    }
}

