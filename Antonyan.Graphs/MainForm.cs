using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    public partial class MainForm : Form, UserInterface
    {
        private readonly float R = 20;
        private float left = 30f, right = 100f, top = 30f, bottom = 50f;
        private vec2 min = new vec2(), max = new vec2();
        private vec2 Wc = new vec2();
        private vec2 W = new vec2();
        private Field<Vertex, Weight> field;
        private Vertex source, stock;

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


        public MainForm()
        {
            circle = GenerateCircle(R, 1f);

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RetCalc();
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
            var g = e.Graphics; 
            if (field != null)
            {
                foreach (var v in field.Coords)
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
            }

            Pen rectPen = new Pen(Color.Black, 2);
            g.DrawRectangle(rectPen, left, top, W.x, W.y);
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            RetCalc();
            Refresh();
        }

        private void createGraph_file_Click(object sender, EventArgs e)
        {
            
            CreateGraphGUI crt = new CreateGraphGUI();
            crt.Owner = this;
            crt.ShowDialog();
            if (crt.Ok)
            {
                string orientd = crt.Oriented ? "oriented" : "noOriented";
                string weighted = crt.Weighted ? "weighted" : "noWeighted";
                CommandEntered?.Invoke(this, new UICommandEventArgs($"CreateField {orientd} {weighted}"));
                createGraph_file.Enabled = false;
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
        }

        public void AttachField(object field)
        {
            this.field = (Field<Vertex, Weight>)field;
        }
    }
}
