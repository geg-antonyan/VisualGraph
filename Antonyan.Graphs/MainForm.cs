using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Antonyan.Graphs.Backend.Geometry;
using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs
{
    public partial class MainForm : Form, UserInterface
    {  
        private readonly float R = 20;
        private float left = 30f, right = 100f, top = 30f, bottom = 50f;
        private Vec2 min = new Vec2(), max = new Vec2();
        private Vec2 Wc = new Vec2();
        private Vec2 W = new Vec2();
        private Field<Vertex, Weight> field;
        private int i = 0;

        private readonly Vec2[] circle;

        private void RetCalc()
        {
            max.x = ClientRectangle.Width - right;
            max.y = ClientRectangle.Height - bottom;
            Wc.y = max.y;
            Wc.x = left;
            W.x = max.x - left;
            W.y = max.y - top;
        }

        private Vec2[] GenerateCircle(float r, float dx)
        {
            var res = new Vec2[(int)(r / dx * 4f + 2)];
            float x = -r, y = 0f;
            res[0] = new Vec2(x, y);
            int j = 1;
            x += dx;
            while (x <= r)
            {
                float y2 = r * r - x * x;
                if (y2 < 0) break;
                y = (float)Math.Sqrt(y2);
                res[j++] = new Vec2(x, y);
                x += dx;
            }
            x -= dx;
            while (x >= -r)
            {
                float y2 = r * r - x * x;
                if (y2 < 0) break;
                y = -(float)Math.Sqrt(y2);
                res[j++] = new Vec2(x, y);
                x -= dx;
            }
            return res;
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

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            Pen bluePen2 = new Pen(Color.Blue, 2f);
            Font fontVertex = new Font(FontFamily.GenericSansSerif, 12f);
            SolidBrush brushVertex = new SolidBrush(Color.Black);
            if (field != null)
            {
                foreach (var v in field.Coords)
                {
                   // if (!Clip.SimpleClip(v.Value, max, min, R)) continue;
                   // float xstr = (v.Key.ToString().Length < 2) ? R / 2f : R / 1.1f;
                   // g.DrawString(v.Key.ToString(), fontVertex, brushVertex, v.Value.x - xstr, v.Value.y - R / 1.5f);
                    Vec2 A = new Vec2(circle[0].x + v.Value.x, circle[0].y + v.Value.y);
                    for (int i = 1; i < circle.Length; i++)
                    {
                        Vec2 B = new Vec2(circle[i].x + v.Value.x, circle[i].y + v.Value.y);
                        if (Clip.RectangleClip(ref A, ref B, min, max))
                            g.DrawLine(bluePen2, A.x, A.y, B.x, B.y);
                        A = new Vec2(circle[i].x + v.Value.x, circle[i].y + v.Value.y);
                    }
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
        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        if (field != null)
                            if (!field.HasAFreePlace(new Vec2(e.X, e.Y), R + R + R / 2))
                                return;
                        if (max.x - (float)e.X < R || (float)e.X - left < R || max.y - (float)e.Y < R || e.Y - top < R) return;
                        string v = i++.ToString();
                        string x = e.X.ToString();
                        string y = e.Y.ToString();
                        string command = $"AddVertex {v} {x} {y}";
                        CommandEnterd?.Invoke(this, new UICommandEventArgs(command));
                    break;
                    }
                default: break;
            }
        }

        public event EventHandler<UICommandEventArgs> CommandEnterd;

        public void FieldUpdate(object obj, EventArgs e)
        {
            if (field == null)
                field = (Field<Vertex, Weight>)obj;
            Refresh();
        }

        public void PostMessage(string message)
        {
        }

       
    }
}
