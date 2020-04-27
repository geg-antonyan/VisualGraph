using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Gui
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
            this.tsbtnSaveGraph = new System.Windows.Forms.ToolStripButton();
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
            this.subShortcutBtnBFS = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGraphFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openGraphFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolStripTask = new System.Windows.Forms.ToolStrip();
            this.tsBtnHalfWayTop = new System.Windows.Forms.ToolStripButton();
            this.toolStripMain.SuspendLayout();
            this.toolStripTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtnOpen,
            this.tsbtnSaveGraph,
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
            this.tsbtnOpen.Click += new System.EventHandler(this.tsbtnOpen_Click);
            // 
            // tsbtnSaveGraph
            // 
            this.tsbtnSaveGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSaveGraph.Image = global::Antonyan.Graphs.Properties.Resources.save;
            this.tsbtnSaveGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveGraph.Name = "tsbtnSaveGraph";
            this.tsbtnSaveGraph.Size = new System.Drawing.Size(29, 29);
            this.tsbtnSaveGraph.Text = "Сохранить";
            this.tsbtnSaveGraph.Click += new System.EventHandler(this.tsbtnSaveGraph_Click);
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
            this.tsbtnDeleteGraph.Text = "tsBtnRemoveGraph";
            this.tsbtnDeleteGraph.ToolTipText = "Удалить Граф";
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
            this.subDetoursBtnDFS.Size = new System.Drawing.Size(186, 28);
            this.subDetoursBtnDFS.Text = "Обход в глубину";
            this.subDetoursBtnDFS.Click += new System.EventHandler(this.subDetoursBtnDFS_Click);
            // 
            // subDetoursBtnBFS
            // 
            this.subDetoursBtnBFS.Image = global::Antonyan.Graphs.Properties.Resources.bfs;
            this.subDetoursBtnBFS.Name = "subDetoursBtnBFS";
            this.subDetoursBtnBFS.Size = new System.Drawing.Size(186, 28);
            this.subDetoursBtnBFS.Text = "Обход в ширину";
            this.subDetoursBtnBFS.Click += new System.EventHandler(this.subDetoursBtnBFS_Click);
            // 
            // tsbtnShortcats
            // 
            this.tsbtnShortcats.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnShortcats.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.subShortcutBtnBFS});
            this.tsbtnShortcats.Image = global::Antonyan.Graphs.Properties.Resources.shortcat4;
            this.tsbtnShortcats.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnShortcats.Name = "tsbtnShortcats";
            this.tsbtnShortcats.Size = new System.Drawing.Size(33, 29);
            this.tsbtnShortcats.Text = "Кратчайшие пути";
            this.tsbtnShortcats.Click += new System.EventHandler(this.tsBtnShortcats_Click);
            // 
            // subShortcutBtnBFS
            // 
            this.subShortcutBtnBFS.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.subShortcutBtnBFS.Image = global::Antonyan.Graphs.Properties.Resources.mark;
            this.subShortcutBtnBFS.Name = "subShortcutBtnBFS";
            this.subShortcutBtnBFS.ShowShortcutKeys = false;
            this.subShortcutBtnBFS.Size = new System.Drawing.Size(418, 28);
            this.subShortcutBtnBFS.Text = "Кратчайшый путь методом построение родительского дерево";
            this.subShortcutBtnBFS.Click += new System.EventHandler(this.subShortcutBtnBFS_Click);
            // 
            // saveGraphFileDialog
            // 
            this.saveGraphFileDialog.Filter = "graph files (*.graph)|*.graph";
            // 
            // openGraphFileDialog
            // 
            this.openGraphFileDialog.Filter = "graph files (*.graph)|*.graph";
            // 
            // toolStripTask
            // 
            this.toolStripTask.Dock = System.Windows.Forms.DockStyle.Right;
            this.toolStripTask.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.toolStripTask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnHalfWayTop});
            this.toolStripTask.Location = new System.Drawing.Point(962, 32);
            this.toolStripTask.Name = "toolStripTask";
            this.toolStripTask.Size = new System.Drawing.Size(32, 607);
            this.toolStripTask.TabIndex = 1;
            this.toolStripTask.Text = "toolStripTasks";
            // 
            // tsBtnHalfWayTop
            // 
            this.tsBtnHalfWayTop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnHalfWayTop.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tsBtnHalfWayTop.Image = ((System.Drawing.Image)(resources.GetObject("tsBtnHalfWayTop.Image")));
            this.tsBtnHalfWayTop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnHalfWayTop.Name = "tsBtnHalfWayTop";
            this.tsBtnHalfWayTop.Size = new System.Drawing.Size(29, 26);
            this.tsBtnHalfWayTop.Text = "Полуисход вершины";
            this.tsBtnHalfWayTop.Click += new System.EventHandler(this.tsBtnHalfWayTop_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 639);
            this.Controls.Add(this.toolStripTask);
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
            this.toolStripTask.ResumeLayout(false);
            this.toolStripTask.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem subShortcutBtnBFS;
        private System.Windows.Forms.ToolStripButton tsBtnAddVertex;
        private System.Windows.Forms.ToolStripButton tsbtnSaveGraph;
        private System.Windows.Forms.ToolStripButton tsbtnOpen;
        private System.Windows.Forms.ToolStripButton tsbtnDeleteGraph;
        private System.Windows.Forms.ToolStripButton tsbtnRemoveElems;
        private System.Windows.Forms.ToolStripButton tsbtnMove;
        private System.Windows.Forms.SaveFileDialog saveGraphFileDialog;
        private System.Windows.Forms.OpenFileDialog openGraphFileDialog;
        private System.Windows.Forms.ToolStrip toolStripTask;
        private System.Windows.Forms.ToolStripButton tsBtnHalfWayTop;
    }
}

