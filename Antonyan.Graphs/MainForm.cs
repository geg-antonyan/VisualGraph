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

using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;

using Antonyan.Graphs.Gui;
using Antonyan.Graphs.Gui.Models;



namespace Antonyan.Graphs
{


    public partial class MainForm<TVertex, TWeight> : Form, UserInterface
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private Models models;
        private readonly float R = 20;
        private readonly float left = 30f;
        private readonly float right = 30f;
        private readonly float top = 50f;
        private readonly float bottom = 50f;
        private readonly vec2 max, min;
        private readonly vec2 Wc, W;
        private Field<TVertex, TWeight> field;
        private string source = null, stock = null;

        private int i = 0;



        public MainForm()
        {
            min = new vec2(); max = new vec2();
            Wc = new vec2(); W = new vec2();
            Circle.GenerateCircle(R, 1f);
            models = new Models(new Pen(Color.Red), new Pen(Color.Blue), new Pen(Color.Green), new Pen(Color.DarkGray, 2f),
                new Font(FontFamily.GenericSansSerif, 14f), new Font(FontFamily.GenericSansSerif, 12f), 
                new Font(FontFamily.GenericMonospace, 12f),
                new Font(FontFamily.GenericMonospace, 10f),
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
            tlbtnAddEdge.Enabled = false;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            tlbtnAddEdge.Enabled = models.MarkedCircleCount == 2 ? true : false;
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
                string orientd = window.Oriented ? "oriented" : "noOriented";
                string weighted = window.Weighted ? "weighted" : "noWeighted";
                CommandEntered?.Invoke(this, new UICommandEventArgs($"CreateField {orientd} {weighted}"));
                tlbtnCrtGraph.Enabled = false;
            }
        }

        private void tlbtnAddEdge_Click(object sender, EventArgs e)
        {
            if (source == null || stock == null) return;
            models.UnmarkAll();
            string src = source;
            string stc = stock;
            source = stock = null;
            if (field.IsWeighted)
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

        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (field == null) return;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        vec2 pos = new vec2((float)e.X, (float)e.Y);
                        int edgehashCode = models.GetEdgeHashCode(pos);
                        if (max.x - pos.x < R || pos.x - left < R || max.y - pos.y < R || pos.y - top < R)
                        {
                            models.UnmarkAll();
                        }
                        else if (!field.HasAFreePlace(pos, R))
                        {
                            string mark = field.GetVertex(pos, R).ToString();
                            int hasCode = mark.GetHashCode();
                            models.Mark(hasCode);
                            if (models.MarkedCircleCount == 1)
                                source = mark;
                            else if (models.MarkedCircleCount == 2)
                                stock = mark;
                        }
                        else if (edgehashCode != 0)
                        {
                            models.Mark(edgehashCode);
                        }
                        else if (field.HasAFreePlace(new vec2(e.X, e.Y), R + R + R / 2))
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
            Refresh();
        }

        public event EventHandler<UICommandEventArgs> CommandEntered;

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
                        Edge edge = new Edge(args.PosSource, args.PosStock, R, args.Weight);
                        models.AddDrawModel(args.GetHashCode(), edge);
                        break;
                    }
                case FieldEvents.RemoveVertex:
                    {
                        break;
                    }
                case FieldEvents.RemoveEdge:
                    {
                        break;
                    }
            }
            Refresh();
        }

        public void PostMessage(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void AttachField(object field)
        {
            this.field = (Field<TVertex, TWeight>)field;
        }

        public void CheckUndoRedo(bool undoPossible, bool redoPossible)
        {
            throw new NotImplementedException();
        }
    }
}
