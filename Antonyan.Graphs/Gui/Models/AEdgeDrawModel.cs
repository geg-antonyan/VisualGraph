using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Gui.Models
{
    public abstract class AEdgeDrawModel : ADrawModel
    {
        protected static readonly Matrix mirrorX = new Matrix(-1f, 0f, 0f, 1f, 0f, 0f);
        protected static readonly Matrix mirrorY = new Matrix(1f, 0f, 0f, -1f, 0f, 0f);

        public bool Weighted { get; private set; }
        protected float R = 20;
        protected float weightAngle;
        protected float length;
        protected vec2 weightPos;
        protected float weightPosOffsetKoef;
        protected vec2 posA, posB;
        protected vec2 normDirection;
        public AEdgeDrawModel(GraphModels model, float weightPosOffsetKoef, bool marked = false)
            : base(model, marked)
        {
            this.weightPosOffsetKoef = weightPosOffsetKoef;
            Weighted = ((EdgeModel)Model).Weight == null ? false : true;
            SetPos();
        }

        public void SetObservableVertices(VertexDrawModel a, VertexDrawModel b)
        {
            a.PosChanged += UpdatePos;
            b.PosChanged += UpdatePos;
        }

        public virtual void SetPos(vec2 sourcePos = null, vec2 stockPos = null)
        {
            EdgeModel edge = (EdgeModel)Model;
            if (sourcePos == null) sourcePos = edge.Source.Pos;
            if (stockPos == null) stockPos = edge.Stock.Pos;
            vec2 direction = stockPos - sourcePos;
            normDirection = direction.Normalize();
            vec2 incr = normDirection * R;
            posA = sourcePos + incr;
            posB = stockPos - incr;
            length = (posB - posA).Length();
            if (Weighted)
            {
                bool reverse = sourcePos.y > stockPos.y;
                vec2 delta = reverse ? sourcePos - stockPos : stockPos - sourcePos;
                float len = delta.Length();
                vec2 normDelta = delta.Normalize();
                float koef = delta.x / delta.Length();
                weightAngle = (float)(Math.Acos(koef) * 180 / Math.PI);
                float center = len / 2f;
                vec2 dl = normDelta * center;
                weightPos = reverse ? sourcePos - dl : sourcePos + dl;
            }

        }
        public void UpdatePos(object ob, EventArgs args)
        {
            SetPos();
        }
        public override void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
        {
            DrawEdge(graphic, pen, min, max);
            if (Weighted && Clip.SimpleClip(new vec2(weightPos.x + 5f, weightPos.y + 5f), max, min, R))
                DrawWeight(graphic, brush, font, min, max);
        }
        public abstract override string PosRepresent(vec2 pos, float r);
        protected abstract void DrawEdge(Graphics graphic, Pen pen, vec2 min, vec2 max);
        protected virtual void DrawWeight(Graphics graphic, Brush brush, Font font, vec2 min, vec2 max)
        {
            Matrix matrix = new Matrix();
            vec2 A = posB - posA;
            vec2 B = new vec2(10f, 0f);
            B.y = -(A.x * A.y) / B.x;
            B = B.Normalize();
            B *= weightPosOffsetKoef;
            StringFormat stringFormat = new StringFormat();
            matrix.Translate(weightPos.x, weightPos.y);
            matrix.Rotate(weightAngle);
            if (weightAngle == 90f) B = new vec2(0f, 0f); 
            if (weightAngle > 90f)
            {
                B *= -1f;
                matrix.Multiply(mirrorY);
                matrix.Multiply(mirrorX);
            }
            graphic.MultiplyTransform(matrix);
            graphic.DrawString(((EdgeModel)Model).Weight, font, brush, B.x, B.y, stringFormat);
            matrix.Reset();
            if (weightAngle > 90f)
            {
                matrix.Multiply(mirrorX);
                matrix.Multiply(mirrorY);
            }
            matrix.Rotate(-weightAngle);
            matrix.Translate(-weightPos.x, -weightPos.y);
            graphic.MultiplyTransform(matrix);
        }
    }

}

