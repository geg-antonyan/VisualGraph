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

namespace Antonyan.Graphs
{
    public partial class MainForm<TVertex, TWeight> : Form, UserInterface
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private readonly float R = 20;
        private float left = 30f, right = 30f, top = 50f, bottom = 50f;
        private vec2 min = new vec2(), max = new vec2();
        private vec2 Wc = new vec2();
        private vec2 W = new vec2();
        private Field<TVertex, TWeight> field;
        private TVertex source, stock;

        private int i = 0;

        private readonly vec3[] circle;

        private void RetCalc()
        {
            max.x = ClientRectangle.Width - right;
            max.y = ClientRectangle.Height - bottom;
            Wc.y = max.y;
            Wc.x = left;
            W.x = max.x - left;
            W.y = max.y - top;
        }

        private vec3[] GenerateCircle(float r, float dx)
        {
            var res = new vec3[(int)(r / dx * 4f + 2)];
            float x = -r, y = 0f;
            res[0] = new vec3(x, y);
            int j = 1;
            x += dx;
            while (x <= r)
            {
                float y2 = r * r - x * x;
                if (y2 < 0) break;
                y = (float)Math.Sqrt(y2);
                res[j++] = new vec3(x, y);
                x += dx;
            }
            x -= dx;
            while (x >= -r)
            {
                float y2 = r * r - x * x;
                if (y2 < 0) break;
                y = -(float)Math.Sqrt(y2);
                res[j++] = new vec3(x, y);
                x -= dx;
            }
            return res;
        }

        private void DrawCircle(float cx, float cy, Pen pen, Graphics g)
        {
            mat3 translate = Transforms.Translate(cx, cy);
            vec3 A = translate * circle[0];
            for (int i = 1; i < circle.Length; i++)
            {
                vec3 B = translate * circle[i];
                vec2 a = (vec2)A, b = new vec2(B);
                if (Clip.RectangleClip(ref a, ref b, min, max))
                    g.DrawLine(pen, a.x, a.y, b.x, b.y);
                A = B;
            }
        }
        private readonly Matrix mirrorX = new Matrix(-1f, 0f, 0f, 1f, 0f, 0f);
        private readonly Matrix mirrorY = new Matrix(1f, 0f, 0f, -1f, 0f, 0f);
        private void DrawEdge(vec2 a, vec2 b, bool oriented, Graphics g, Pen pen, TWeight weight = null, SolidBrush brush = null, Font font = null)
        {
            if (oriented)
            {

            }
            else
            {
                if (a.y > b.y)
                    Clip.Swap(ref a, ref b);
                vec2 v = b - a;
                vec2 norm = v.Norm();
                vec2 incr = norm * R;
                vec2 start = a + incr;
                vec2 end = b - incr;

                if (Clip.RectangleClip(ref start, ref end, min, max))
                    g.DrawLine(pen, start.x, start.y, end.x, end.y);
                if (weight != null)
                {

                    float tmp = v.x / v.Length();
                    float angle = (float)Math.Acos(tmp) * 180f / (float)Math.PI;
                    float length = (end - start).Length();
                    float koef = (angle > 90f) ? 1.8f : 2.2f;
                    float center = length / koef;
                    vec2 dl = norm * center;
                    vec2 pos = start + dl;
                    if (Clip.SimpleClip(new vec2(pos.x + 5, pos.y + 5), max, min, R))
                    {
                        
                        Matrix matrix = new Matrix();
                        vec2 A = v;
                        vec2 B = new vec2(10f, 0f);
                        B.y = -(A.x * A.y) / B.x;
                        B = B.Norm();
                        B *= 23f;
                        StringFormat stringFormat = new StringFormat();
                        matrix.Translate(pos.x, pos.y);
                        matrix.Rotate(angle);
                        if (angle > 90f)
                        {
                            B *= -1f;
                            matrix.Multiply(mirrorY);
                            matrix.Multiply(mirrorX);
                        }
                        if (angle == 90f) B = new vec2(0f, 0f);
                        g.MultiplyTransform(matrix);
                        g.DrawString(weight.ToString(), font, brush, B.x, B.y, stringFormat);
                        matrix.Reset();
                        if (angle > 90f)
                        {
                            matrix.Multiply(mirrorX);
                            matrix.Multiply(mirrorY);
                        }
                        matrix.Rotate(-angle);
                        matrix.Translate(-pos.x, -pos.y);
                        g.MultiplyTransform(matrix);
                    }

                }
            }
        }


        public MainForm()
        {
            circle = GenerateCircle(R, 1f);

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
                    float xstr = str.Length == 1 ? pos.x - R / 2f + 2f : pos.x - R + 6f;
                    float ystr = pos.y - R / 2f;
                    g.DrawString(str, seintSerif, brush, new RectangleF(xstr, ystr, R * 2f, R * 2f));
                    DrawCircle(pos.x, pos.y, pen, g);
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
                            DrawEdge(src, stc, field.IsOrgarph, g, redPen, edge.Item2, greenBrush, monospace);
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
