using Antonyan.Graphs.Board;
using System;
using System.IO;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Util;
using System.Drawing.Drawing2D;

namespace Antonyan.Graphs.Gui.Models
{
    public class OrientedEdgeDrawModel : AEdgeDrawModel
    {
        private vec2 A1, A2;
        private static readonly Pen endPen = new Pen(Color.Black)
        {
            CustomEndCap = new CustomLineCap(new GraphicsPath(new PointF[] { new PointF(0f, 0f), new PointF(-3f, -15f),
                    new PointF(0f, -10f), new PointF(3f, -15f), new PointF(0f, 0f) }, new byte[] { 1, 1, 1, 1, 1 }), null)
        };
        public OrientedEdgeDrawModel(GraphModels model, bool marked = false)
            : base(model, 40, marked)
        {
            SetPos();
        }

        public override void SetPos(vec2 sourcePos = null, vec2 stockPos = null)
        {
            base.SetPos(sourcePos, stockPos);
            ServiceFunctions.WriteLine($"{weightAngle}");
            var edgeModel = (EdgeModel)Model;
            vec2 posStock = edgeModel.Stock.Pos, posSource = edgeModel.Source.Pos;
            float x, y;
            vec2 AB = posStock - posSource;
            if (AB.x == 0f) AB.x = 0.0001f;
            if (AB.y == 0f) AB.y = 0.0001f;
            if (posSource.x != posStock.y)
            {
                if (posStock.x > posSource.x)
                    y = -10f;
                else y = 10f;
                vec2 v = new vec2(0f, y);
                x = -(AB.y * v.y) / AB.x;
            }
            else
            {
                if (posStock.y > posSource.y)
                    x = -10f;
                else x = 10f;
                vec2 v = new vec2(x, 0f);
                y = (-AB.x * v.x) / AB.y;
            }
            
            vec2 norm = new vec2(x, y).Normalize();
            
            float step = length / 3f;
            A1 = posA + (normDirection * step);
            A1 = A1 + (norm * 20f);
            A2 = posA + (normDirection * (step * 2f));
            A2 = A2 + (norm * 20f);
           // weightPosOffsetKoef = posA.x > posB.x && weightAngle > 90f ? weightPosOffsetKoef * -1f : Math.Abs(weightPosOffsetKoef);

        }

        protected override void DrawWeight(Graphics graphic, Brush brush, Font font, vec2 min, vec2 max)
        {
            if (posA.x > posB.x)
                weightPosOffsetKoef = -30f;
            else weightPosOffsetKoef = 40f;
            base.DrawWeight(graphic, brush, font, min, max);
        }

        public override string PosRepresent(vec2 pos, float r)
        {
            throw new NotImplementedException();
        }

        protected override void DrawEdge(Graphics graphic, Pen pen, vec2 min, vec2 max)
        {
            vec2 A = new vec2(posA);
            vec2 B = new vec2(A1);
            if (Clip.RectangleClip(ref A, ref B, min, max))
                graphic.DrawLine(pen, A.x, A.y, B.x, B.y);
            B = new vec2(A1);
            vec2 C = new vec2(A2);
            if (Clip.RectangleClip(ref B, ref C, min, max))
                graphic.DrawLine(pen, B.x, B.y, C.x, C.y);
            C = new vec2(A2);
            vec2 D = new vec2(posB);
            if (Clip.RectangleClip(ref C, ref D, min, max) && (C-D).Length() > 6)
            {
                endPen.Color = pen.Color;
                endPen.Width = pen.Width;
                if (!D.Equals(posB)) ServiceFunctions.Swap(ref D, ref C);
                var p = posB.Equals(D) ? endPen : pen;
                graphic.DrawLine(endPen, C.x, C.y, D.x, D.y);
            }
        }
    }

 
}

