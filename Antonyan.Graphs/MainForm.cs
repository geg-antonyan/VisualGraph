using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;

using Antonyan.Graphs.Gui;
using Antonyan.Graphs.Gui.Models;
using Antonyan.Graphs.Gui.Forms;

using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;

namespace Antonyan.Graphs
{


    public partial class MainForm : Form, UserInterface
    {
        private DrawModelsField modelsBoard;
        private readonly float R = 20;
        private readonly float left = 30f;
        private readonly float right = 30f;
        private readonly float top = 50f;
        private readonly float bottom = 50f;
        private readonly vec2 max, min;
        private readonly vec2 Wc, W;
        private bool fieldCreated;
        private bool oriented, weighted;
        private string source = null, stock = null;
        private readonly string header = "Visual Graph";

        private int i = 0;
        private bool algorithmProcessing = false;
        Thread algoThread;
        private bool mouseDownFL = false;
        private string selectedRepresent; 
        private vec2 lastVertexPos;
        public MainForm()
        {
            min = new vec2(); max = new vec2();
            Wc = new vec2(); W = new vec2();
            DrawVertexModel.GenerateCircle(R, 1f);
            modelsBoard = new DrawModelsField(this,
                new Pen(Color.Red, 2f), new Pen(Color.Blue), new Pen(Color.Green, 3f), new Pen(Color.DarkGray, 2f),
                new Font(FontFamily.GenericSansSerif, 14f), new Font(FontFamily.GenericSansSerif, 12f),
                new Font(FontFamily.GenericMonospace, 14f),
                new Font(FontFamily.GenericMonospace, 12f),
                new SolidBrush(Color.Red), new SolidBrush(Color.Blue), new SolidBrush(Color.Green), new SolidBrush(Color.DarkGray));
            InitializeComponent();
            Text = header;
        }

        private void RetCalc()
        {
            max.x = ClientRectangle.Width - right;
            max.y = ClientRectangle.Height - bottom;
            min.x = left;
            min.y = top;
            Wc.y = max.y;
            Wc.x = left;
            W.x = max.x - left;
            W.y = max.y - top;
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            RetCalc();
            tsbtnAddEdge.Enabled = false;
            tsbtnRedo.Enabled = false;
            tsbtnUndo.Enabled = false;
            subDetoursBtnBFS.Enabled = subDetoursBtnDFS.Enabled = false;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            tsBtnAddVertex.Enabled = fieldCreated;
            tsbtnAddEdge.Enabled = modelsBoard.MarkedCircleCount == 2 && !algorithmProcessing ? true : false;
            tsbtnRemoveElems.Enabled = modelsBoard.MarkedModelsCount > 0 && !tsBtnAddVertex.Checked;
            var g = e.Graphics;
            var pen = new Pen(Color.Black, 6);
            pen.EndCap = LineCap.ArrowAnchor;
            g.DrawArc(pen, 100f, 100f, 150f, 50f, 1f, 360f);
            modelsBoard.Draw(g, min, max);
            Pen rectPen = new Pen(Color.Black, 2);
            g.DrawRectangle(rectPen, left, top, W.x, W.y);
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            RetCalc();
            Refresh();
        }

        private void tlbtnCrtGraph_Click(object sender, EventArgs e)
        {
            CreateGraphForm window = new CreateGraphForm();
            window.Owner = this;
            window.ShowDialog();
            if (window.Ok)
            {
                oriented = window.Oriented;
                weighted = window.Weighted;
                string orGraph = oriented ? "oriented" : "noOriented";
                string weightedGraph = weighted ? "weighted" : "noWeighted";
                CommandEntered?.Invoke(this, new UICommandEventArgs($"CreateField {orGraph} {weightedGraph}"));
                tlbtnCrtGraph.Enabled = false;
                fieldCreated = true;
            }
        }

        private void tlbtnAddEdge_Click(object sender, EventArgs e)
        {
            if (source == null || stock == null) return;

            string src = source;
            string stc = stock;
            source = stock = null;
            if (weighted)
            {
                SetWeightForm window = new SetWeightForm();
                window.Owner = this;
                window.ShowDialog();
                if (window.Ok)
                {
                    CommandEntered?.Invoke(this, new UICommandEventArgs($"AddEdge {src} {stc} {window.Weight}"));
                }
            }
            else
            {
                CommandEntered?.Invoke(this, new UICommandEventArgs($"AddEdge {src} {stc}"));
            }
            modelsBoard.UnmarkAllDrawModels();

        }


        public event EventHandler<UICommandEventArgs> CommandEntered;

        private void tsbtnUndo_Click(object sender, EventArgs e)
        {
            CommandEntered?.Invoke(this, new UICommandEventArgs("undo"));
        }

        public void ModelsBoardUpdate(object obj, EventArgs e)
        {
            Refresh();
        }

        private void tsbtnRedo_Click(object sender, EventArgs e)
        {
            CommandEntered?.Invoke(this, new UICommandEventArgs("redo"));
        }


        //public Edges makeEdges(vec2 posSource, string source, vec2 posStock, string stock, float r, string weight)
        //{
        //    if (oriented) return new OrientedEdges(posSource, source, posStock, stock, r, weight);
        //    else return new NonOrientedEdges(posSource, source, posStock, stock, r, weight);
        //}

        public void FieldUpdate(object obj, EventArgs e)
        {
            var fieldEvents = (FieldUpdateArgs)e;
            foreach (var fldEvent in fieldEvents.Pairs)
            {
                switch (fldEvent.Event)
                {
                    case FieldEvents.AddVertex:
                        {
                            var model = fieldEvents.Models[fldEvent.Representation];
                            var drawModel = new DrawVertexModel(model, false);
                            modelsBoard.AddDrawModel(model.GetRepresentation(), drawModel);
                            break;
                        }
                    case FieldEvents.RemoveVertex:
                        {
                            modelsBoard.RemoveDrawModel(fldEvent.Representation);
                            break;
                        }
                    case FieldEvents.ChangeVertexPos:
                        {
                            modelsBoard.ChangeDrawVertexModelPos(fldEvent.Representation, ((VertexModel)fieldEvents.Models[fldEvent.Representation]).Pos);
                            break;
                        }
                    default: break;
                }
            }

        }

        public void PostMessage(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void subDetoursBtnDFS_Click(object sender, EventArgs e)
        {
            Text += " - Выполяется алгоритм обхода в глубину";
            algorithmProcessing = true;
            algoThread = new Thread(() =>
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    CommandEntered?.Invoke(this, new UICommandEventArgs($"algorithm dfs {source}"));
                    algorithmProcessing = false;
                    Text = header;
                }));
            });
            algoThread.Start();

        }


        private void subDetoursBtnBFS_Click(object sender, EventArgs e)
        {
            Text += " - Выполяется алгоритм обхода в ширину";
            algorithmProcessing = true;
            algoThread = new Thread(() =>
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    CommandEntered?.Invoke(this, new UICommandEventArgs($"algorithm bfs {source}"));
                    algorithmProcessing = false;
                    Text = header;
                }));
            });
            algoThread.Start();
        }


        public void SetFieldStatus(bool status)
        {
            if (!status) i = 0;
            fieldCreated = status;
            tlbtnCrtGraph.Enabled = !status;
        }

        private void tsbtnDetours_Click(object sender, EventArgs e)
        {
            string dfs = "Обход в глубину", bfs = "Обход в ширину";
            bool res = subDetoursBtnBFS.Enabled = subDetoursBtnDFS.Enabled = modelsBoard.MarkedCircleCount == 1 ? true : false;
            if (res)
            {
                subDetoursBtnBFS.Text = $"{bfs} начиная с вершины \"{source}\"";
                subDetoursBtnDFS.Text = $"{dfs} начиная с вершины \"{source}\"";
            }
            else
            {
                subDetoursBtnBFS.Text = bfs;
                subDetoursBtnDFS.Text = dfs;
            }
        }

        private void tsbtnShortcats_Click(object sender, EventArgs e)
        {
            subSortcatBtnBFS.Enabled = modelsBoard.MarkedCircleCount == 2 && !weighted;
        }

        private void subSortcatBtnBFS_Click(object sender, EventArgs e)
        {

            Text += " - Выполяется алгоритм нахождение кратчайшего пути с помошью построение родительского дерева";
            algorithmProcessing = true;
            algoThread = new Thread(() =>
            {
                BeginInvoke((MethodInvoker)(() =>
                {
                    CommandEntered?.Invoke(this, new UICommandEventArgs($"algorithm shortcatdfs {source} {stock}"));
                    algorithmProcessing = false;
                    Text = header;
                }));
            });
            algoThread.Start();
        }

        private void tsbtnAddVertex_Click(object sender, EventArgs e)
        {
            tsBtnAddVertex.Checked = !tsBtnAddVertex.Checked;
            if (tsBtnAddVertex.Checked)
            {
                Text = header + " - кликните внутри рабочего прямоугольника, чтобы добавить вершину";
                tsbtnMove.Checked = false;
            }
            else Text = header;
        }

        private void tsbtnRemoveElems_Click(object sender, EventArgs e)
        {
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!fieldCreated || tsbtnMove.Checked) return;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        vec2 pos = new vec2((float)e.X, (float)e.Y);
                        //int vertexHashCode, edgehashCode;
                        if (max.x - pos.x < R || pos.x - left < R || max.y - pos.y < R || pos.y - top < R)
                        {
                            modelsBoard.UnmarkAllDrawModels();
                        }
                        //else if ((vertexHashCode = models.GetCircleHashCode(pos, R)) != 0 && !tsbtnAddVertecxFL.Checked)
                        //{
                        //    models.Mark(vertexHashCode);
                        //    if (models.MarkedCircleCount == 1)
                        //        source = models.GetVertexMark(vertexHashCode);
                        //    else if (models.MarkedCircleCount == 2)
                        //        stock = models.GetVertexMark(vertexHashCode);
                        //}
                        //else if ((edgehashCode = models.GetEdgeHashCode(pos)) != 0 && !tsbtnAddVertecxFL.Checked)
                        //{
                        //    models.Mark(edgehashCode);
                        //}
                        if (modelsBoard.GetPosRepresent(new vec2(e.X, e.Y), R + R + R / 2) == null && tsBtnAddVertex.Checked)
                        {

                            if (modelsBoard.MarkedCircleCount > 0 || modelsBoard.MarkedEdgeCount > 0)
                            {
                                modelsBoard.UnmarkAllDrawModels();
                                break;
                            }
                            string v = i++.ToString();
                            string x = e.X.ToString();
                            string y = e.Y.ToString();
                            string command = $"AddVertex {v} {x} {y}";
                            CommandEntered?.Invoke(this, new UICommandEventArgs(command));
                        }
                        else modelsBoard.UnmarkAllDrawModels();
                        break;
                    }
                default: break;
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (tsbtnMove.Checked)
            {
                mouseDownFL = true;
                vec2 pos = new vec2((float)e.X, (float)e.Y);
                if (selectedRepresent == null)
                {
                    modelsBoard.UnmarkAllDrawModels();
                    selectedRepresent = modelsBoard.GetPosRepresent(pos, R);
                    lastVertexPos = modelsBoard.GetDrawVertexModelPos(selectedRepresent);
                }
            }
        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDownFL || !tsbtnMove.Checked) return;
            vec2 pos = new vec2((float)e.X, (float)e.Y);
            modelsBoard.MarkDrawModel(selectedRepresent);
            modelsBoard.ChangeDrawVertexModelPos(selectedRepresent, pos);
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDownFL && lastVertexPos != null)
            {
                vec2 pos = new vec2(e.X, e.Y);
                CommandEntered?.Invoke(this, new UICommandEventArgs($"MoveVertexPos {selectedRepresent} {lastVertexPos} {pos}"));
                selectedRepresent = null;
                lastVertexPos = null;
                modelsBoard.UnmarkAllDrawModels();
            }
            mouseDownFL = false;
        }

        private void MainForm_MouseEnter(object sender, EventArgs e)
        {

        }


        private void tsbtnMove_Click(object sender, EventArgs e)
        {
            tsbtnMove.Checked = !tsbtnMove.Checked;
            if (tsbtnMove.Checked)
                tsBtnAddVertex.Checked = false;
        }

        public void CheckUndoRedo(bool undoPossible, bool redoPossible)
        {
            tsbtnRedo.Enabled = redoPossible;
            tsbtnUndo.Enabled = undoPossible;
        }

        public bool MarkModel(string represent)
        {
            // return models.Mark(hashCode);
            return false;
        }

        public bool UnmarkModel(string represent)
        {
            //return models.Unmark(hashCode);
            return false;
        }

        public void UnmarkAll()
        {
            modelsBoard.UnmarkAllDrawModels();
        }

        public void PostWarningMessage(string warningMessage)
        {
            throw new NotImplementedException();
        }

        public void PostErrorMessage(string errorMessage)
        {
            throw new NotImplementedException();
        }
    }
}
