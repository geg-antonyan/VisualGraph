using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Gui.Models
{
    public class Edge : DrawModel
    {
        private static readonly Matrix mirrorX = new Matrix(-1f, 0f, 0f, 1f, 0f, 0f);
        private static readonly Matrix mirrorY = new Matrix(1f, 0f, 0f, -1f, 0f, 0f);
        private readonly float angle;
        private readonly vec2 a, b;
        private readonly vec2 pos;
        private readonly float r;

        public override int GetHashCode()
        {
            return (SourcePos.x.ToString() + SourcePos.y.ToString() +
                StockPos.x.ToString() + StockPos.y.ToString() + Weight).GetHashCode(); ;
        }
        public string Weight { get; private set; }
        public vec2 SourcePos { get; private set; }
        public vec2 StockPos { get; private set; }
        public Edge(vec2 soucePos, vec2 stockPos, float r, string weight = null)
        {
            if (r < 0) r = 0;
            if (r != 0)
            {
                SourcePos = soucePos;
                StockPos = stockPos;
                if (soucePos.y > stockPos.y)
                    Clip.Swap(ref soucePos, ref stockPos);
                vec2 deltaV = stockPos - soucePos;
                vec2 norm = deltaV.Norm();
                vec2 incr = norm * r;
                a = soucePos + incr;
                b = stockPos - incr;
                this.Weight = weight;
                this.r = r;
                if (weight != null)
                {
                    float tmp = deltaV.x / deltaV.Length();
                    angle = (float)Math.Acos(tmp) * 180f / (float)Math.PI;
                    float length = (stockPos - soucePos).Length();
                    float koef = (angle > 90f) ? 1.8f : 2.2f;
                    float center = length / koef;
                    vec2 dl = norm * center;
                    pos = soucePos + dl;
                }
            }
            else
            {
                SourcePos = soucePos;
                StockPos = stockPos;
                this.Weight = weight;
            }
        }
        public void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
        {
            vec2 start = new vec2(a);
            vec2 end = new vec2(b);
            if (Clip.RectangleClip(ref start, ref end, min, max))
                graphic.DrawLine(pen, start.x, start.y, end.x, end.y);
            if (Weight == null || !Clip.SimpleClip(new vec2(pos.x + 5f, pos.y + 5f), max, min, r))
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
            graphic.MultiplyTransform(matrix);
            graphic.DrawString(Weight.ToString(), font, brush, B.x, B.y, stringFormat);
            matrix.Reset();
            if (angle > 90f)
            {
                matrix.Multiply(mirrorX);
                matrix.Multiply(mirrorY);
            }
            matrix.Rotate(-angle);
            matrix.Translate(-pos.x, -pos.y);
            graphic.MultiplyTransform(matrix);
        }
    }
}
