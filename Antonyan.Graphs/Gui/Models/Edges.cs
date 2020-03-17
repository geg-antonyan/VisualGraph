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

    public abstract class Edges : DrawModel
    {
        protected static readonly Matrix mirrorX = new Matrix(-1f, 0f, 0f, 1f, 0f, 0f);
        protected static readonly Matrix mirrorY = new Matrix(1f, 0f, 0f, -1f, 0f, 0f);
        protected vec2 posA, posB;
        protected float angleWeight;
        protected vec2 posWeight;
        protected float R;
       
        public string Source { get; protected set; }
        public string Stock { get; protected set; }
        public vec2 SourcePos { get; protected set; }
        public vec2 StockPos { get; protected set; }
        public string Weight { get; protected set; }
        public bool Weighted { get; protected set; }

        public Edges(vec2 sourcePos, string source, vec2 stockPos, string stock, float r, string weight = null)
        {
            Source = source;
            Stock = stock;
            Weighted = weight == null ? false : true;
            Weight = weight;
            R = r;
            ChangePos(sourcePos, stockPos);
        }
        public void ChangePos(vec2 sourcePos, vec2 stockPos)
        {
            SourcePos = sourcePos;
            StockPos = stockPos;
            if (sourcePos.y > stockPos.y)
                Clip.Swap(ref sourcePos, ref stockPos);
            vec2 deltaV = stockPos - sourcePos;
            vec2 norm = deltaV.Norm();
            float tmp = deltaV.x / deltaV.Length();
            angleWeight = (float)Math.Acos(tmp) * 180f / (float)Math.PI;
            if (R != 0)
            {
                vec2 incr = norm * R;
                posA = sourcePos + incr;
                posB = stockPos - incr;
            }
            else
            {
                SourcePos = posA = sourcePos;
                StockPos = posB = stockPos;
            }
            if (Weighted)
            {
                float length = (stockPos - sourcePos).Length();
                float koef = (angleWeight > 90f) ? 1.8f : 2.2f;
                float center = length / koef;
                vec2 dl = norm * center;
                posWeight = sourcePos + dl;
            }

        }
        public abstract void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max);
    }

    public class NonOrientedEdges : Edges
    {

        public override int GetHashCode()
        {
            return (Source + " " + Stock + " " + Weight).GetHashCode();
        }

        public NonOrientedEdges(vec2 sourcePos, string source, vec2 stockPos, string stock, float r, string weight = null)
            : base(sourcePos, source, stockPos, stock, r, weight)
        {
            
        }

        public override void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
        {
            vec2 start = new vec2(posA);
            vec2 end = new vec2(posB);
            if (Clip.RectangleClip(ref start, ref end, min, max))
                graphic.DrawLine(pen, start.x, start.y, end.x, end.y);
            if (Weight == null || !Clip.SimpleClip(new vec2(posWeight.x + 5f, posWeight.y + 5f), max, min, R))
                return;
            Matrix matrix = new Matrix();
            vec2 A = end - start;
            vec2 B = new vec2(10f, 0f);
            B.y = -(A.x * A.y) / B.x;
            B = B.Norm();
            B *= 20f;
            StringFormat stringFormat = new StringFormat();
            matrix.Translate(posWeight.x, posWeight.y);
            matrix.Rotate(angleWeight);
            if (angleWeight > 90f)
            {
                B *= -1f;
                matrix.Multiply(mirrorY);
                matrix.Multiply(mirrorX);
            }
            if (angleWeight == 90f) B = new vec2(0f, 0f);
            graphic.MultiplyTransform(matrix);
            graphic.DrawString(Weight.ToString(), font, brush, B.x, B.y, stringFormat);
            matrix.Reset();
            if (angleWeight > 90f)
            {
                matrix.Multiply(mirrorX);
                matrix.Multiply(mirrorY);
            }
            matrix.Rotate(-angleWeight);
            matrix.Translate(-posWeight.x, -posWeight.y);
            graphic.MultiplyTransform(matrix);
        }
    }

    public class OrientedEdges : Edges
    {

        public override int GetHashCode()
        {
            return (Source + " " + Stock + " " + Weight).GetHashCode();
        }

        public OrientedEdges(vec2 sourcePos, string source, vec2 stockPos, string stock, float r, string weight = null)
            : base(sourcePos, source, stockPos, stock, r, weight)
        {

        }

        public override void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
        {
            vec2 start = new vec2(SourcePos);
            vec2 end = new vec2(StockPos);
            if (Clip.RectangleClip(ref start, ref end, min, max))
            {
                pen.EndCap = LineCap.ArrowAnchor;
                pen.Width = 4;
                graphic.TranslateTransform(start.x, start.y);
                graphic.RotateTransform(angleWeight);
                graphic.DrawArc(pen, 0f, 0f, Math.Abs(end.Length() - start.Length()), 30f, 0, 180f);
                graphic.RotateTransform(-angleWeight);
                graphic.TranslateTransform(-start.x, -start.y);
            }
            //if (Weight == null || !Clip.SimpleClip(new vec2(posWeight.x + 5f, posWeight.y + 5f), max, min, R))
            //    return;
            //Matrix matrix = new Matrix();
            //vec2 A = end - start;
            //vec2 B = new vec2(10f, 0f);
            //B.y = -(A.x * A.y) / B.x;
            //B = B.Norm();
            //B *= 20f;
            //StringFormat stringFormat = new StringFormat();
            //matrix.Translate(posWeight.x, posWeight.y);
            //matrix.Rotate(angleWeight);
            //if (angleWeight > 90f)
            //{
            //    B *= -1f;
            //    matrix.Multiply(mirrorY);
            //    matrix.Multiply(mirrorX);
            //}
            //if (angleWeight == 90f) B = new vec2(0f, 0f);
            //graphic.MultiplyTransform(matrix);
            //graphic.DrawString(Weight.ToString(), font, brush, B.x, B.y, stringFormat);
            //matrix.Reset();
            //if (angleWeight > 90f)
            //{
            //    matrix.Multiply(mirrorX);
            //    matrix.Multiply(mirrorY);
            //}
            //matrix.Rotate(-angleWeight);
            //matrix.Translate(-posWeight.x, -posWeight.y);
            //graphic.MultiplyTransform(matrix);
        }
    }
}
