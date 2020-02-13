using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Gui
{
    public class Edge
    {
        private Graphics graphics;
        private Pen pen;
        private vec2 start;
        private vec2 end;
        private float angle;
        private string weight;
        private readonly Matrix mirrorX = new Matrix(-1f, 0f, 0f, 1f, 0f, 0f);
        private readonly Matrix mirrorY = new Matrix(1f, 0f, 0f, -1f, 0f, 0f);
        private vec2 pos;
        private Brush brush;
        private Font font;
        private float R;
        public Edge(vec2 start, vec2 end, Pen pen, Graphics graphics, float r, Brush brush = null, Font font = null, string weight = null)
        {
            if (r != 0)
            {
                if (start.y > end.y)
                    Clip.Swap(ref start, ref end);
                vec2 v = end - start;
                vec2 norm = v.Norm();
                vec2 incr = norm * r;
                start += incr;
                end -= incr;
                this.start = start;
                this.end = end;
                this.weight = weight;
                this.font = font;
                this.brush = brush;
                R = r;
                if (weight != null)
                {
                    float tmp = v.x / v.Length();
                    float angle = (float)Math.Acos(tmp) * 180f / (float)Math.PI;
                    float length = (end - start).Length();
                    float koef = (angle > 90f) ? 1.8f : 2.2f;
                    float center = length / koef;
                    vec2 dl = norm * center;
                    pos = start + dl;
                }
            }
        }
        public void Draw(vec2 min, vec2 max)
        {
            if (Clip.RectangleClip(ref start, ref end, min, max))
                graphics.DrawLine(pen, start.x, start.y, end.x, end.y);
            if (weight == null || !Clip.SimpleClip(new vec2(pos.x + 5f, pos.y + 5f), max, min, R))
                return;
            Matrix matrix = new Matrix();
            vec2 A = end - start;
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
            graphics.MultiplyTransform(matrix);
            graphics.DrawString(weight, font, brush, B.x, B.y, stringFormat);
            matrix.Reset();
            if (angle > 90f)
            {
                matrix.Multiply(mirrorX);
                matrix.Multiply(mirrorY);
            }
            matrix.Rotate(-angle);
            matrix.Translate(-pos.x, -pos.y);
            graphics.MultiplyTransform(matrix);
        }
    }
}
