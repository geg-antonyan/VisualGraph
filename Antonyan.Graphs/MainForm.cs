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
        SortedDictionary<string, Model> models;
        private readonly float R = 20;
        private float left = 30f, right = 30f, top = 50f, bottom = 50f;
        private vec2 min = new vec2(), max = new vec2();
        private vec2 Wc = new vec2();
        private vec2 W = new vec2();
        private Field<TVertex, TWeight> field;
        private TVertex source, stock;

        private int i = 0;
        private void RetCalc()
        {
            max.x = ClientRectangle.Width - right;
            max.y = ClientRectangle.Height - bottom;
            Wc.y = max.y;
            Wc.x = left;
            W.x = max.x - left;
            W.y = max.y - top;
        }
       

        public MainForm()
        {
            Circle.GenerateCircle(R, 1f);
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RetCalc();
            tlbtnAddEdge.Enabled = false;
        }

        private readonly Pen bluePen = new Pen(Color.Blue, 2f);
        private readonly Pen redPen = new Pen(Color.Red, 2f);
        private readonly Pen darkRedPen = new Pen(Color.DarkRed, 2f);
        private readonly SolidBrush darkGreenBrush = new SolidBrush(Color.DarkGreen);
        private readonly SolidBrush blackBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush greenBrush = new SolidBrush(Color.Green);
        private readonly Font monospace = new Font(FontFamily.GenericMonospace, 16f);
        private readonly Font seintSerif = new Font(FontFamily.GenericSansSerif, 12f);
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            tlbtnAddEdge.Enabled = (source != null && stock != null) ? true : false;
            var g = e.Graphics;
            if (field != null)
            {
                foreach (var v in field.Positions)
                {
                    Pen pen; SolidBrush brush; Font font = monospace;
                    if (v.Key == source)
                    {
                        brush = greenBrush;
                        pen = redPen;
                    }
                    else if (v.Key == stock)
                    {
                        pen = darkRedPen;
                        brush = darkGreenBrush;
                    }
                    else
                    {
                        brush = blackBrush;
                        pen = bluePen;
                        font = seintSerif;
                    }
                    string str = v.Key.ToString();
                    vec2 pos = v.Value;
                    //float xstr = str.Length == 1 ? pos.x - R / 2f + 2f : pos.x - R + 6f;
                    //float ystr = pos.y - R / 2f;
                    //g.DrawString(str, seintSerif, brush, new RectangleF(xstr, ystr, R * 2f, R * 2f));
                    Circle circle = new Circle(g, pen, brush, seintSerif, pos, str);
                    circle.Draw(min, max);
                    //DrawCircle(pos.x, pos.y, pen, g);
                }
                bool[] vertices = new bool[field.Graph.Counut];
                foreach (var v in field.Graph.AdjList)
                {
                    if (v.Value.Count > 0)
                    {
                        vec2 src = field.GetPos(v.Key);
                        foreach (var edge in v.Value)
                        {
                            if (vertices[edge.Item1.Key] == true) continue;
                            vec2 stc = field.GetPos(edge.Item1);
                            Edge ed = new Edge(g, redPen, greenBrush, monospace, src, stc, R, edge.Item2.ToString());
                           ed.Draw(min, max);
                           // DrawEdge(src, stc, field.IsOrgarph, g, redPen, edge.Item2, greenBrush, monospace);
                        }
                    }
                    vertices[v.Key.Key] = true;
                }
            }

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
            string src = source.ToString();
            string stc = stock.ToString();
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
                        if (max.x - pos.x < R || pos.x - left < R || max.y - pos.y < R || pos.y - top < R)
                        {
                            source = stock = null;
                        }
                        else if (!field.HasAFreePlace(pos, R))
                        {
                            if (source == null)
                                source = field.GetVertex(pos, R);
                            else if (stock == null)
                                stock = field.GetVertex(pos, R);
                            else source = stock = null;
                        }
                        else if (field.HasAFreePlace(new vec2(e.X, e.Y), R + R + R / 2))
                        {
                            source = stock = null;
                            string v = i++.ToString();
                            string x = e.X.ToString();
                            string y = e.Y.ToString();
                            string command = $"AddVertex {v} {x} {y}";
                            CommandEntered?.Invoke(this, new UICommandEventArgs(command));
                        }
                        else source = stock = null;
                        Refresh();
                        break;
                    }
                default: break;
            }
        }

        public event EventHandler<UICommandEventArgs> CommandEntered;

        public void FieldUpdate(object obj, EventArgs e)
        {
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
    }
}
