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
            this.tsBtnRefresh = new System.Windows.Forms.ToolStripButton();
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
            this.tsddBtnTasks = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiPowVertex = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConnectedComponents = new System.Windows.Forms.ToolStripMenuItem();
            this.мОДКаркасToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiKruskalAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShortestPaths = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDijkstraAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFordBellmanAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNPeriohery = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMaxFlow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEdmondsKarp = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGraphFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openGraphFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtInfoList = new System.Windows.Forms.RichTextBox();
            this.listBoxAdjList = new System.Windows.Forms.ListBox();
            this.btnSaveAdjList = new System.Windows.Forms.Button();
            this.btnUnion = new System.Windows.Forms.Button();
            this.btnRemoveAdjList = new System.Windows.Forms.Button();
            this.toolStripMain.SuspendLayout();
            this.statusStrip.SuspendLayout();
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
            this.tsBtnRefresh,
            this.tlbtnCrtGraph,
            this.tsbtnDeleteGraph,
            this.tsBtnAddVertex,
            this.tsbtnRemoveElems,
            this.tsbtnMove,
            this.tsBtnAddEdge,
            this.tsbtnDetours,
            this.tsbtnShortcats,
            this.tsddBtnTasks});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Margin = new System.Windows.Forms.Padding(2, 4, 0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.toolStripMain.Size = new System.Drawing.Size(994, 29);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "Меню";
            // 
            // tsbtnOpen
            // 
            this.tsbtnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnOpen.Image = global::Antonyan.Graphs.Properties.Resources.open;
            this.tsbtnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnOpen.Name = "tsbtnOpen";
            this.tsbtnOpen.Size = new System.Drawing.Size(26, 26);
            this.tsbtnOpen.Text = "Открыть";
            this.tsbtnOpen.Click += new System.EventHandler(this.tsbtnOpen_Click);
            // 
            // tsbtnSaveGraph
            // 
            this.tsbtnSaveGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnSaveGraph.Image = global::Antonyan.Graphs.Properties.Resources.save;
            this.tsbtnSaveGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnSaveGraph.Name = "tsbtnSaveGraph";
            this.tsbtnSaveGraph.Size = new System.Drawing.Size(26, 26);
            this.tsbtnSaveGraph.Text = "Сохранить";
            this.tsbtnSaveGraph.Click += new System.EventHandler(this.tsbtnSaveGraph_Click);
            // 
            // tsBtnUndo
            // 
            this.tsBtnUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnUndo.Image = global::Antonyan.Graphs.Properties.Resources.Undo;
            this.tsBtnUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnUndo.Name = "tsBtnUndo";
            this.tsBtnUndo.Size = new System.Drawing.Size(26, 26);
            this.tsBtnUndo.Text = "Назад";
            this.tsBtnUndo.Click += new System.EventHandler(this.tsbtnUndo_Click);
            // 
            // tsBtnRedo
            // 
            this.tsBtnRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnRedo.Image = global::Antonyan.Graphs.Properties.Resources.redo;
            this.tsBtnRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnRedo.Name = "tsBtnRedo";
            this.tsBtnRedo.Size = new System.Drawing.Size(26, 26);
            this.tsBtnRedo.Text = "Вперед";
            this.tsBtnRedo.Click += new System.EventHandler(this.tsbtnRedo_Click);
            // 
            // tsBtnRefresh
            // 
            this.tsBtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnRefresh.Image = global::Antonyan.Graphs.Properties.Resources.refresh;
            this.tsBtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnRefresh.Name = "tsBtnRefresh";
            this.tsBtnRefresh.Size = new System.Drawing.Size(26, 26);
            this.tsBtnRefresh.Text = "Обнавить";
            this.tsBtnRefresh.Click += new System.EventHandler(this.tsBtnRefresh_Click);
            // 
            // tlbtnCrtGraph
            // 
            this.tlbtnCrtGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlbtnCrtGraph.Image = global::Antonyan.Graphs.Properties.Resources.AddGraph;
            this.tlbtnCrtGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlbtnCrtGraph.Name = "tlbtnCrtGraph";
            this.tlbtnCrtGraph.Size = new System.Drawing.Size(26, 26);
            this.tlbtnCrtGraph.Text = "Создать граф";
            this.tlbtnCrtGraph.Click += new System.EventHandler(this.tsBtnCrtGraph_Click);
            // 
            // tsbtnDeleteGraph
            // 
            this.tsbtnDeleteGraph.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnDeleteGraph.Image = global::Antonyan.Graphs.Properties.Resources.removeGraph3;
            this.tsbtnDeleteGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnDeleteGraph.Name = "tsbtnDeleteGraph";
            this.tsbtnDeleteGraph.Size = new System.Drawing.Size(26, 26);
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
            this.tsBtnAddVertex.Size = new System.Drawing.Size(26, 26);
            this.tsBtnAddVertex.Text = "Добаить вершины";
            this.tsBtnAddVertex.Click += new System.EventHandler(this.tsbtnAddVertex_Click);
            // 
            // tsbtnRemoveElems
            // 
            this.tsbtnRemoveElems.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnRemoveElems.Image = global::Antonyan.Graphs.Properties.Resources.removeElems;
            this.tsbtnRemoveElems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnRemoveElems.Name = "tsbtnRemoveElems";
            this.tsbtnRemoveElems.Size = new System.Drawing.Size(26, 26);
            this.tsbtnRemoveElems.Text = "Удалить выбранные элементы";
            this.tsbtnRemoveElems.Click += new System.EventHandler(this.tsbtnRemoveElems_Click);
            // 
            // tsbtnMove
            // 
            this.tsbtnMove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtnMove.Image = global::Antonyan.Graphs.Properties.Resources.move;
            this.tsbtnMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnMove.Name = "tsbtnMove";
            this.tsbtnMove.Size = new System.Drawing.Size(26, 26);
            this.tsbtnMove.Text = "Перемещение";
            this.tsbtnMove.Click += new System.EventHandler(this.tsBtnMove_Click);
            // 
            // tsBtnAddEdge
            // 
            this.tsBtnAddEdge.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnAddEdge.Image = global::Antonyan.Graphs.Properties.Resources.AddEdge2;
            this.tsBtnAddEdge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnAddEdge.Name = "tsBtnAddEdge";
            this.tsBtnAddEdge.Size = new System.Drawing.Size(26, 26);
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
            this.tsbtnDetours.Size = new System.Drawing.Size(35, 26);
            this.tsbtnDetours.Text = "Обходы";
            this.tsbtnDetours.Click += new System.EventHandler(this.tsBtnDetours_Click);
            // 
            // subDetoursBtnDFS
            // 
            this.subDetoursBtnDFS.Image = global::Antonyan.Graphs.Properties.Resources.dfs;
            this.subDetoursBtnDFS.Name = "subDetoursBtnDFS";
            this.subDetoursBtnDFS.Size = new System.Drawing.Size(182, 28);
            this.subDetoursBtnDFS.Text = "Обход в глубину";
            this.subDetoursBtnDFS.Click += new System.EventHandler(this.subDetoursBtnDFS_Click);
            // 
            // subDetoursBtnBFS
            // 
            this.subDetoursBtnBFS.Image = global::Antonyan.Graphs.Properties.Resources.bfs;
            this.subDetoursBtnBFS.Name = "subDetoursBtnBFS";
            this.subDetoursBtnBFS.Size = new System.Drawing.Size(182, 28);
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
            this.tsbtnShortcats.Size = new System.Drawing.Size(35, 26);
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
            // tsddBtnTasks
            // 
            this.tsddBtnTasks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddBtnTasks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPowVertex,
            this.tsmiConnectedComponents,
            this.мОДКаркасToolStripMenuItem,
            this.tsmiShortestPaths,
            this.tsmiMaxFlow});
            this.tsddBtnTasks.Image = ((System.Drawing.Image)(resources.GetObject("tsddBtnTasks.Image")));
            this.tsddBtnTasks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddBtnTasks.Name = "tsddBtnTasks";
            this.tsddBtnTasks.Size = new System.Drawing.Size(70, 26);
            this.tsddBtnTasks.Text = "Задание";
            // 
            // tsmiPowVertex
            // 
            this.tsmiPowVertex.Name = "tsmiPowVertex";
            this.tsmiPowVertex.Size = new System.Drawing.Size(205, 22);
            this.tsmiPowVertex.Text = "Степень вершины";
            this.tsmiPowVertex.Click += new System.EventHandler(this.tsmiPowVertex_Click);
            // 
            // tsmiConnectedComponents
            // 
            this.tsmiConnectedComponents.Name = "tsmiConnectedComponents";
            this.tsmiConnectedComponents.Size = new System.Drawing.Size(205, 22);
            this.tsmiConnectedComponents.Text = "Связные компоненты";
            this.tsmiConnectedComponents.Click += new System.EventHandler(this.tsmiConnectedComponents_Click);
            // 
            // мОДКаркасToolStripMenuItem
            // 
            this.мОДКаркасToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiKruskalAlgorithm});
            this.мОДКаркасToolStripMenuItem.Name = "мОДКаркасToolStripMenuItem";
            this.мОДКаркасToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.мОДКаркасToolStripMenuItem.Text = "МОД (Каркас)";
            // 
            // tsmiKruskalAlgorithm
            // 
            this.tsmiKruskalAlgorithm.Name = "tsmiKruskalAlgorithm";
            this.tsmiKruskalAlgorithm.Size = new System.Drawing.Size(192, 22);
            this.tsmiKruskalAlgorithm.Text = "Алгоритм Крускала";
            this.tsmiKruskalAlgorithm.Click += new System.EventHandler(this.tsmiKruskalAlgorithm_Click);
            // 
            // tsmiShortestPaths
            // 
            this.tsmiShortestPaths.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDijkstraAlgorithm,
            this.tsmiFordBellmanAlgorithm,
            this.tsmiNPeriohery});
            this.tsmiShortestPaths.Name = "tsmiShortestPaths";
            this.tsmiShortestPaths.Size = new System.Drawing.Size(205, 22);
            this.tsmiShortestPaths.Text = "Кратчайшие пути";
            // 
            // tsmiDijkstraAlgorithm
            // 
            this.tsmiDijkstraAlgorithm.Name = "tsmiDijkstraAlgorithm";
            this.tsmiDijkstraAlgorithm.Size = new System.Drawing.Size(194, 22);
            this.tsmiDijkstraAlgorithm.Text = "Алгоритм Дейкстры";
            this.tsmiDijkstraAlgorithm.Click += new System.EventHandler(this.tsmiDijkstraAlgorithm_Click);
            // 
            // tsmiFordBellmanAlgorithm
            // 
            this.tsmiFordBellmanAlgorithm.Name = "tsmiFordBellmanAlgorithm";
            this.tsmiFordBellmanAlgorithm.Size = new System.Drawing.Size(194, 22);
            this.tsmiFordBellmanAlgorithm.Text = "Путь не больше L";
            this.tsmiFordBellmanAlgorithm.Click += new System.EventHandler(this.tsmiFordBellmanAlgorithm_Click);
            // 
            // tsmiNPeriohery
            // 
            this.tsmiNPeriohery.Name = "tsmiNPeriohery";
            this.tsmiNPeriohery.Size = new System.Drawing.Size(194, 22);
            this.tsmiNPeriohery.Text = "N-периферия";
            this.tsmiNPeriohery.Click += new System.EventHandler(this.tsmiNPeriohery_Click);
            // 
            // tsmiMaxFlow
            // 
            this.tsmiMaxFlow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEdmondsKarp});
            this.tsmiMaxFlow.Name = "tsmiMaxFlow";
            this.tsmiMaxFlow.Size = new System.Drawing.Size(205, 22);
            this.tsmiMaxFlow.Text = "Максимальный поток";
            // 
            // tsmiEdmondsKarp
            // 
            this.tsmiEdmondsKarp.Name = "tsmiEdmondsKarp";
            this.tsmiEdmondsKarp.Size = new System.Drawing.Size(238, 22);
            this.tsmiEdmondsKarp.Text = "Алгоритм Эдмондса-Карпа";
            this.tsmiEdmondsKarp.Click += new System.EventHandler(this.tsmiEdmondsKarp_Click);
            // 
            // saveGraphFileDialog
            // 
            this.saveGraphFileDialog.Filter = "graph files (*.graph)|*.graph";
            // 
            // openGraphFileDialog
            // 
            this.openGraphFileDialog.Filter = "graph files (*.graph)|*.graph";
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 617);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(994, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // txtInfoList
            // 
            this.txtInfoList.BackColor = System.Drawing.Color.LightGray;
            this.txtInfoList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInfoList.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtInfoList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtInfoList.HideSelection = false;
            this.txtInfoList.Location = new System.Drawing.Point(800, 50);
            this.txtInfoList.Name = "txtInfoList";
            this.txtInfoList.ReadOnly = true;
            this.txtInfoList.Size = new System.Drawing.Size(150, 399);
            this.txtInfoList.TabIndex = 3;
            this.txtInfoList.Text = "";
            this.txtInfoList.WordWrap = false;
            // 
            // listBoxAdjList
            // 
            this.listBoxAdjList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBoxAdjList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxAdjList.FormattingEnabled = true;
            this.listBoxAdjList.ItemHeight = 18;
            this.listBoxAdjList.Location = new System.Drawing.Point(800, 492);
            this.listBoxAdjList.Name = "listBoxAdjList";
            this.listBoxAdjList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxAdjList.Size = new System.Drawing.Size(121, 56);
            this.listBoxAdjList.TabIndex = 4;
            // 
            // btnSaveAdjList
            // 
            this.btnSaveAdjList.BackColor = System.Drawing.SystemColors.Menu;
            this.btnSaveAdjList.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSaveAdjList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSaveAdjList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnSaveAdjList.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnSaveAdjList.Location = new System.Drawing.Point(800, 451);
            this.btnSaveAdjList.Name = "btnSaveAdjList";
            this.btnSaveAdjList.Size = new System.Drawing.Size(150, 35);
            this.btnSaveAdjList.TabIndex = 5;
            this.btnSaveAdjList.Text = "Сохранить список смежности";
            this.btnSaveAdjList.UseVisualStyleBackColor = false;
            this.btnSaveAdjList.Click += new System.EventHandler(this.btnSaveAdjList_Click);
            // 
            // btnUnion
            // 
            this.btnUnion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnUnion.Location = new System.Drawing.Point(928, 493);
            this.btnUnion.Name = "btnUnion";
            this.btnUnion.Size = new System.Drawing.Size(22, 27);
            this.btnUnion.TabIndex = 6;
            this.btnUnion.Text = "U";
            this.btnUnion.UseVisualStyleBackColor = true;
            this.btnUnion.Click += new System.EventHandler(this.btnUnion_Click);
            // 
            // btnRemoveAdjList
            // 
            this.btnRemoveAdjList.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnRemoveAdjList.Location = new System.Drawing.Point(927, 526);
            this.btnRemoveAdjList.Name = "btnRemoveAdjList";
            this.btnRemoveAdjList.Size = new System.Drawing.Size(22, 27);
            this.btnRemoveAdjList.TabIndex = 7;
            this.btnRemoveAdjList.Text = "X";
            this.btnRemoveAdjList.UseVisualStyleBackColor = true;
            this.btnRemoveAdjList.Click += new System.EventHandler(this.btnRemoveAdjList_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 639);
            this.Controls.Add(this.btnRemoveAdjList);
            this.Controls.Add(this.btnUnion);
            this.Controls.Add(this.btnSaveAdjList);
            this.Controls.Add(this.listBoxAdjList);
            this.Controls.Add(this.txtInfoList);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.toolStripMain);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
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
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.RichTextBox txtInfoList;
        private System.Windows.Forms.ListBox listBoxAdjList;
        private System.Windows.Forms.Button btnSaveAdjList;
        private System.Windows.Forms.Button btnUnion;
        private System.Windows.Forms.Button btnRemoveAdjList;
        private System.Windows.Forms.ToolStripButton tsBtnRefresh;
        private System.Windows.Forms.ToolStripDropDownButton tsddBtnTasks;
        private System.Windows.Forms.ToolStripMenuItem tsmiPowVertex;
        private System.Windows.Forms.ToolStripMenuItem tsmiConnectedComponents;
        private System.Windows.Forms.ToolStripMenuItem мОДКаркасToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiKruskalAlgorithm;
        private System.Windows.Forms.ToolStripMenuItem tsmiShortestPaths;
        private System.Windows.Forms.ToolStripMenuItem tsmiDijkstraAlgorithm;
        private System.Windows.Forms.ToolStripMenuItem tsmiFordBellmanAlgorithm;
        private System.Windows.Forms.ToolStripMenuItem tsmiMaxFlow;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdmondsKarp;
        private System.Windows.Forms.ToolStripMenuItem tsmiNPeriohery;
    }
}

