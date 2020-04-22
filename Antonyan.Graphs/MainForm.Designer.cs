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
            this.tsbtnOpen = new System.Windows.Forms.ToolStripButton();
            this.tsbtnSave = new System.Windows.Forms.ToolStripButton();
            this.tsBtnUndo = new System.Windows.Forms.ToolStripButton();
            this.tsBtnRedo = new System.Windows.Forms.ToolStripButton();
            this.tlbtnCrtGraph = new System.Windows.Forms.ToolStripButton();
            this.tsbtnDeleteGraph = new System.Windows.Forms.ToolStripButton();
            this.tsBtnAddVertex = new System.Windows.Forms.ToolStripButton();
            this.tsbtnRemoveElems = new System.Windows.Forms.ToolStripButton();
            this.tsbtnMove = new System.Windows.Forms.ToolStripButton();
            this.tsBtnAddEdge = new System.Windows.Forms.ToolStripButton();
            this.tsbtnDetours = new System.Windows.Forms.ToolStripDropDownButton();
            this.subDetoursBtnDFS = new System.Windows.Forms.ToolStripMenuItem();
            this.subDetoursBtnBFS = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbtnShortcats = new System.Windows.Forms.ToolStripDropDownButton();
            this.subSortcatBtnBFS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMain.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnOpen,
            this.tsbtnSave,
            this.tsBtnUndo,
            this.tsBtnRedo,
            this.tlbtnCrtGraph,
            this.tsbtnDeleteGraph,
            this.tsBtnAddVertex,
            this.tsbtnRemoveElems,
            this.tsbtnMove,
            this.tsBtnAddEdge,
            this.tsbtnDetours,
            this.tsbtnShortcats});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Margin = new System.Windows.Forms.Padding(2, 4, 0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStripMain.Size = new System.Drawing.Size(994, 32);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "Меню";
            // 
            // tsbtnOpen
            // 
            this.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpen.Image = global::Antonyan.Graphs.Properties.Resources.open;
            this.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnOpen.Name = "tsbtnOpen";
            this.tsbtnOpen.Size = new System.Drawing.Size(29, 29);
            this.tsbtnOpen.Text = "Открыть";
            // 
            // tsbtnSave
            // 
            this.tsbtnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSave.Image = global::Antonyan.Graphs.Properties.Resources.save;
            this.tsbtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSave.Name = "tsbtnSave";
            this.tsbtnSave.Size = new System.Drawing.Size(29, 29);
            this.tsbtnSave.Text = "Сохранить";
            // 
            // tsBtnUndo
            // 
            this.tsBtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnUndo.Image = global::Antonyan.Graphs.Properties.Resources.Undo;
            this.tsBtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnUndo.Name = "tsBtnUndo";
            this.tsBtnUndo.Size = new System.Drawing.Size(29, 29);
            this.tsBtnUndo.Text = "Назад";
            this.tsBtnUndo.Click += new System.EventHandler(this.tsbtnUndo_Click);
            // 
            // tsBtnRedo
            // 
            this.tsBtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnRedo.Image = global::Antonyan.Graphs.Properties.Resources.redo;
            this.tsBtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnRedo.Name = "tsBtnRedo";
            this.tsBtnRedo.Size = new System.Drawing.Size(29, 29);
            this.tsBtnRedo.Text = "Вперед";
            this.tsBtnRedo.Click += new System.EventHandler(this.tsbtnRedo_Click);
            // 
            // tlbtnCrtGraph
            // 
            this.tlbtnCrtGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbtnCrtGraph.Image = global::Antonyan.Graphs.Properties.Resources.AddGraph;
            this.tlbtnCrtGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnCrtGraph.Name = "tlbtnCrtGraph";
            this.tlbtnCrtGraph.Size = new System.Drawing.Size(29, 29);
            this.tlbtnCrtGraph.Text = "Создать граф";
            this.tlbtnCrtGraph.Click += new System.EventHandler(this.tsBtnCrtGraph_Click);
            // 
            // tsbtnDeleteGraph
            // 
            this.tsbtnDeleteGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDeleteGraph.Image = global::Antonyan.Graphs.Properties.Resources.removeGraph3;
            this.tsbtnDeleteGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDeleteGraph.Name = "tsbtnDeleteGraph";
            this.tsbtnDeleteGraph.Size = new System.Drawing.Size(29, 29);
            this.tsbtnDeleteGraph.Text = "toolStripButton1";
            this.tsbtnDeleteGraph.Click += new System.EventHandler(this.tsBtnDeleteGraph_Click);
            // 
            // tsBtnAddVertex
            // 
            this.tsBtnAddVertex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnAddVertex.Image = global::Antonyan.Graphs.Properties.Resources.AddVertex;
            this.tsBtnAddVertex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnAddVertex.Name = "tsBtnAddVertex";
            this.tsBtnAddVertex.Size = new System.Drawing.Size(29, 29);
            this.tsBtnAddVertex.Text = "Добаить вершины";
            this.tsBtnAddVertex.Click += new System.EventHandler(this.tsbtnAddVertex_Click);
            // 
            // tsbtnRemoveElems
            // 
            this.tsbtnRemoveElems.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRemoveElems.Image = global::Antonyan.Graphs.Properties.Resources.removeElems;
            this.tsbtnRemoveElems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRemoveElems.Name = "tsbtnRemoveElems";
            this.tsbtnRemoveElems.Size = new System.Drawing.Size(29, 29);
            this.tsbtnRemoveElems.Text = "Удалить выбранные элементы";
            this.tsbtnRemoveElems.Click += new System.EventHandler(this.tsbtnRemoveElems_Click);
            // 
            // tsbtnMove
            // 
            this.tsbtnMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnMove.Image = global::Antonyan.Graphs.Properties.Resources.move;
            this.tsbtnMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMove.Name = "tsbtnMove";
            this.tsbtnMove.Size = new System.Drawing.Size(29, 29);
            this.tsbtnMove.Text = "Перемещение";
            this.tsbtnMove.Click += new System.EventHandler(this.tsBtnMove_Click);
            // 
            // tsBtnAddEdge
            // 
            this.tsBtnAddEdge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnAddEdge.Image = global::Antonyan.Graphs.Properties.Resources.AddEdge2;
            this.tsBtnAddEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnAddEdge.Name = "tsBtnAddEdge";
            this.tsBtnAddEdge.Size = new System.Drawing.Size(29, 29);
            this.tsBtnAddEdge.Text = "Соединить вершины";
            this.tsBtnAddEdge.Click += new System.EventHandler(this.tsBtnAddEdge_Click);
            // 
            // tsbtnDetours
            // 
            this.tsbtnDetours.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDetours.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subDetoursBtnDFS,
            this.subDetoursBtnBFS});
            this.tsbtnDetours.Image = global::Antonyan.Graphs.Properties.Resources.detours1;
            this.tsbtnDetours.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDetours.Name = "tsbtnDetours";
            this.tsbtnDetours.Size = new System.Drawing.Size(38, 29);
            this.tsbtnDetours.Text = "Обходы";
            this.tsbtnDetours.Click += new System.EventHandler(this.tsBtnDetours_Click);
            // 
            // subDetoursBtnDFS
            // 
            this.subDetoursBtnDFS.Image = global::Antonyan.Graphs.Properties.Resources.dfs;
            this.subDetoursBtnDFS.Name = "subDetoursBtnDFS";
            this.subDetoursBtnDFS.Size = new System.Drawing.Size(208, 32);
            this.subDetoursBtnDFS.Text = "Обход в глубину";
            this.subDetoursBtnDFS.Click += new System.EventHandler(this.subDetoursBtnDFS_Click);
            // 
            // subDetoursBtnBFS
            // 
            this.subDetoursBtnBFS.Image = global::Antonyan.Graphs.Properties.Resources.bfs;
            this.subDetoursBtnBFS.Name = "subDetoursBtnBFS";
            this.subDetoursBtnBFS.Size = new System.Drawing.Size(208, 32);
            this.subDetoursBtnBFS.Text = "Обход в ширину";
            this.subDetoursBtnBFS.Click += new System.EventHandler(this.subDetoursBtnBFS_Click);
            // 
            // tsbtnShortcats
            // 
            this.tsbtnShortcats.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnShortcats.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subSortcatBtnBFS});
            this.tsbtnShortcats.Image = global::Antonyan.Graphs.Properties.Resources.shortcat4;
            this.tsbtnShortcats.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnShortcats.Name = "tsbtnShortcats";
            this.tsbtnShortcats.Size = new System.Drawing.Size(38, 29);
            this.tsbtnShortcats.Text = "Кратчайшие пути";
            this.tsbtnShortcats.Click += new System.EventHandler(this.tsBtnShortcats_Click);
            // 
            // subSortcatBtnBFS
            // 
            this.subSortcatBtnBFS.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.subSortcatBtnBFS.Image = global::Antonyan.Graphs.Properties.Resources.mark;
            this.subSortcatBtnBFS.Name = "subSortcatBtnBFS";
            this.subSortcatBtnBFS.ShowShortcutKeys = false;
            this.subSortcatBtnBFS.Size = new System.Drawing.Size(454, 32);
            this.subSortcatBtnBFS.Text = "Кратчайшый путь методом построение родительского дерево";
            this.subSortcatBtnBFS.Click += new System.EventHandler(this.subSortcatBtnBFS_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 639);
            this.Controls.Add(this.toolStripMain);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Visual Graph";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseDown);
            this.MouseEnter += new System.EventHandler(this.MainForm_MouseEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseUp);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
       
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton tlbtnCrtGraph;
        private System.Windows.Forms.ToolStripButton tsBtnAddEdge;
        private System.Windows.Forms.ToolStripButton tsBtnUndo;
        private System.Windows.Forms.ToolStripButton tsBtnRedo;
        private System.Windows.Forms.ToolStripDropDownButton tsbtnDetours;
        private System.Windows.Forms.ToolStripMenuItem subDetoursBtnDFS;
        private System.Windows.Forms.ToolStripMenuItem subDetoursBtnBFS;
        private System.Windows.Forms.ToolStripDropDownButton tsbtnShortcats;
        private System.Windows.Forms.ToolStripMenuItem subSortcatBtnBFS;
        private System.Windows.Forms.ToolStripButton tsBtnAddVertex;
        private System.Windows.Forms.ToolStripButton tsbtnSave;
        private System.Windows.Forms.ToolStripButton tsbtnOpen;
        private System.Windows.Forms.ToolStripButton tsbtnDeleteGraph;
        private System.Windows.Forms.ToolStripButton tsbtnRemoveElems;
        private System.Windows.Forms.ToolStripButton tsbtnMove;
    }
}

