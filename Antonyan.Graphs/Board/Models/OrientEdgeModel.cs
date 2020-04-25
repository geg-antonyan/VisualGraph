using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board.Models
{
    public class OrientEdgeModel : AEdgeModel
    {
        public vec2 PosA { get; private set; }
        public vec2 PosB { get; private set; }
        public vec2 PosC { get; private set; }
        public vec2 PosD { get; private set; }
        public OrientEdgeModel(AVertexModel source, AVertexModel stock, string weight)
            : base(source, stock, weight)
        {
            RefreshPos();
        }

        public override string PosKey(vec2 pos, float r)
        {
            return null;
        }
        public override void RefreshPos()

        {
            vec2 sourcePos = Source.Pos;
            vec2 stockPos = Stock.Pos;
            vec2 direction = stockPos - sourcePos;
            vec2 normDirection = direction.Normalize();
            vec2 incr = normDirection * GlobalParameters.Radius;
            PosA = sourcePos + incr;
            PosD = stockPos - incr;
            var length = (PosD - PosA).Length();

            float x, y;
            vec2 AB = stockPos - sourcePos;
            if (AB.x == 0f) AB.x = 0.0001f;
            if (AB.y == 0f) AB.y = 0.0001f;
            if (sourcePos.x != stockPos.y)
            {
                if (stockPos.x > sourcePos.x)
                    y = -10f;
                else y = 10f;
                vec2 v = new vec2(0f, y);
                x = -(AB.y * v.y) / AB.x;
            }
            else
            {
                if (stockPos.y > sourcePos.y)
                    x = -10f;
                else x = 10f;
                vec2 v = new vec2(x, 0f);
                y = (-AB.x * v.x) / AB.y;
            }

            vec2 norm = new vec2(x, y).Normalize();

            float step = length / 3f;
            PosB = PosA + (normDirection * step);
            PosB += (norm * 20f);
            PosC = PosA + (normDirection * (step * 2f));
            PosC += (norm * 20f);

            if (Weighted)
            {
                vec2 delta = sourcePos.y > stockPos.y ? sourcePos - stockPos : stockPos - sourcePos;
                float koef = delta.x / delta.Length();
                WeightAngle = (float)(Math.Acos(koef) * 180 / Math.PI);
                
                delta = PosC - PosB;
                var ln = delta.Length();
                delta = delta.Normalize();
                float charPX = 5f;
                float strLength = charPX * StringRepresent.Length;
                if (sourcePos.x < stockPos.x)
                {
                    strLength *= -1f;
                    norm *= 20f;
                }    

                float offset = (ln / 2f) + (strLength / 2);
                WeightPos = PosB + (delta * offset);
                WeightPos += norm;
            }
        }
    }
}
