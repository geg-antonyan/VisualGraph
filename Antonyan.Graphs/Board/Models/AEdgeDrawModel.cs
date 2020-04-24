using Antonyan.Graphs.Util;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Board.Models
{
    public abstract class AEdgeDrawModel : AEdgeModel
    {


        public vec2 PosA { get; private set; }
        public vec2 PosB { get; private set; }
        public vec2 WeightPos { get; private set; }
        public bool Weighted { get; private set; }
        public float WeightAngle { get; private set; }

        protected float R = GlobalParameters.Radius;
        protected float length;
        protected float weightPosOffsetKoef;
        protected vec2 normDirection;
        public AEdgeDrawModel(AVertexModel source, AVertexModel stock, string weight)
            : base(source, stock, weight)
        {
            Weighted = weight != null;
            StringRepresent = weight;
            RefreshPos();
        }
        public abstract override string PosKey(vec2 pos, float r);
        public override void RefreshPos()
        {
            vec2 sourcePos = Source.Pos;
            vec2 stockPos = Stock.Pos;
            vec2 direction = stockPos - sourcePos;
            normDirection = direction.Normalize();
            vec2 incr = normDirection * R;
            PosA = sourcePos + incr;
            PosB = stockPos - incr;
            length = (PosB - PosA).Length();
            if (Weighted)
            {
                bool reverse = sourcePos.y > stockPos.y;
                vec2 delta = reverse ? sourcePos - stockPos : stockPos - sourcePos;
                float len = delta.Length();
                vec2 normDelta = delta.Normalize();
                float koef = delta.x / delta.Length();
                WeightAngle = (float)(Math.Acos(koef) * 180 / Math.PI);
                float center = len / 2f;
                vec2 dl = normDelta * center;
                WeightPos = reverse ? sourcePos - dl : sourcePos + dl;
            }
        }
    }
}
