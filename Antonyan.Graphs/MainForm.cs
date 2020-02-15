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



namespace Antonyan.Graphs
{


    public partial class MainForm : Form, UserInterface
    {
        private Models models;
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

        private int i = 0;
        private bool algorithmProcessing = false;
        Thread algoThread;
        Thread curent;

        public MainForm()
        {
            curent = Thread.CurrentThread;
            min = new vec2(); max = new vec2();
            Wc = new vec2(); W = new vec2();
            Circle.GenerateCircle(R, 1f);
            models = new Models(this,
                new Pen(Color.Red, 2f), new Pen(Color.Blue), new Pen(Color.Green, 3f), new Pen(Color.DarkGray, 2f),
                new Font(FontFamily.GenericSansSerif, 14f), new Font(FontFamily.GenericSansSerif, 12f),
                new Font(FontFamily.GenericMonospace, 14f),
                new Font(FontFamily.GenericMonospace, 12f),
                new SolidBrush(Color.Red), new SolidBrush(Color.Blue), new SolidBrush(Color.Green), new SolidBrush(Color.DarkGray));
            InitializeComponent();
        }

        private void RetCalc()
        {
            max.x = ClientRectangle.Width - right;
            max.y = ClientRectangle.Height - bottom;
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
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {

            tsbtnAddEdge.Enabled = models.MarkedCircleCount == 2 && !algorithmProcessing ? true : false;
            var g = e.Graphics;
            models.Draw(g, min, max);
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
            CreateGraphGUI window = new CreateGraphGUI();
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
                WeightGui window = new WeightGui();
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
            models.UnmarkAll();

        }
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!fieldCreated) return;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        vec2 pos = new vec2((float)e.X, (float)e.Y);
                        int vertexHashCode, edgehashCode;
                        if (max.x - pos.x < R || pos.x - left < R || max.y - pos.y < R || pos.y - top < R)
                        {
                            models.UnmarkAll();
                        }
                        else if ((vertexHashCode = models.GetCircleHashCode(pos, R)) != 0)
                        {
                            models.Mark(vertexHashCode);
                            if (models.MarkedCircleCount == 1)
                                source = models.GetVertex(vertexHashCode);
                            else if (models.MarkedCircleCount == 2)
                                stock = models.GetVertex(vertexHashCode);
                        }
                        else if ((edgehashCode = models.GetEdgeHashCode(pos)) != 0)
                        {
                            models.Mark(edgehashCode);
                        }
                        else if (models.GetCircleHashCode(new vec2(e.X, e.Y), R + R + R / 2) == 0)
                        {
                            if (models.MarkedCircleCount > 0 || models.MarkedEdgeCount > 0)
                            {
                                models.UnmarkAll();
                                break;
                            }
                            string v = i++.ToString();
                            string x = e.X.ToString();
                            string y = e.Y.ToString();
                            string command = $"AddVertex {v} {x} {y}";
                            CommandEntered?.Invoke(this, new UICommandEventArgs(command));
                        }
                        else models.UnmarkAll();
                        break;
                    }
                default: break;
            }
        }

        public event EventHandler<UICommandEventArgs> CommandEntered;

        private void tsbtnUndo_Click(object sender, EventArgs e)
        {
            CommandEntered?.Invoke(this, new UICommandEventArgs("undo"));
        }

        public void ModelsUpdate(object obj, EventArgs e)
        {
            Refresh();

        }

        private void tsbtnRedo_Click(object sender, EventArgs e)
        {
            CommandEntered?.Invoke(this, new UICommandEventArgs("redo"));
        }

        public void FieldUpdate(object obj, EventArgs e)
        {
            var fieldEvent = (FieldUpdateArgs)e;
            switch (fieldEvent.Event)
            {
                case FieldEvents.AddVertex:
                    {
                        var args = (FieldUpdateVertexArgs)fieldEvent;
                        Circle circle = new Circle(args.Pos, args.Vertex);
                        models.AddDrawModel(args.GetHashCode(), circle);
                        break;
                    }
                case FieldEvents.AddEdge:
                    {
                        var args = (FieldUpdateEdgeArgs)fieldEvent;
                        Edge edge = new Edge(args.PosSource, args.Source, args.PosStock, args.Stock, R, args.Weight);
                        models.AddDrawModel(args.GetHashCode(), edge);
                        break;
                    }
                case FieldEvents.RemoveVertex:
                    {
                        var args = (FieldUpdateVertexArgs)fieldEvent;
                        models.RemoveDrawModel(args.GetHashCode());
                        break;
                    }
                case FieldEvents.RemoveEdge:
                    {
                        var args = (FieldUpdateEdgeArgs)fieldEvent;
                        models.RemoveDrawModel(args.GetHashCode());
                        break;
                    }
            }
            Refresh();
        }

        public void PostMessage(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void subDetoursBtnDFS_Click(object sender, EventArgs e)
        {
               algorithmProcessing = true;
               algoThread = new Thread(() =>
               {
                   Invoke((MethodInvoker)(() =>
                   {
                       CommandEntered?.Invoke(this, new UICommandEventArgs("dfs"));
                       algorithmProcessing = false;
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

        public void CheckUndoRedo(bool undoPossible, bool redoPossible)
        {
            tsbtnRedo.Enabled = redoPossible;
            tsbtnUndo.Enabled = undoPossible;
        }

        public bool MarkModel(int hashCode)
        {
            return models.Mark(hashCode);
        }

        public bool UnmarkModel(int hashCode)
        {
            return models.Unmark(hashCode);
        }

        public void UnmarkAll()
        {
            models.UnmarkAll();
        }
    }
}
