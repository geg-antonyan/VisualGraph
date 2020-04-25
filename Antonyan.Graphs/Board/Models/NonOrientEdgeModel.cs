using Antonyan.Graphs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Board.Models
{
    public class NonOrientEdgeModel : AEdgeModel
    {

        public vec2 PosA { get; private set; }
        public vec2 PosB { get; private set; }
        public NonOrientEdgeModel(AVertexModel source, AVertexModel stock, string weight) 
            : base(source, stock, weight)
        {
            RefreshPos();
        }

        public override void RefreshPos()
        {

            vec2 sourcePos = Source.Pos;
            vec2 stockPos = Stock.Pos;
            vec2 direction = stockPos - sourcePos;
            vec2 normDirection = direction.Normalize();
            vec2 incr = normDirection * GlobalParameters.Radius;
            PosA = sourcePos + incr;
            PosB = stockPos - incr;

            if (Weighted)
            {
                vec2 delta = sourcePos.y > stockPos.y ? sourcePos - stockPos : stockPos - sourcePos;
                float koef = delta.x / delta.Length();
                WeightAngle = (float)(Math.Acos(koef) * 180 / Math.PI);

                float x, y;
                vec2 AB = sourcePos - stockPos;
                if (AB.x == 0f) AB.x = 0.0001f;
                if (AB.y == 0f) AB.y = 0.0001f;
                if (stockPos.x != sourcePos.y)
                {
                    if (sourcePos.x > stockPos.x)
                        y = -10f;
                    else y = 10f;
                    vec2 v = new vec2(0f, y);
                    x = -(AB.y * v.y) / AB.x;
                }
                else
                {
                    if (sourcePos.y > stockPos.y)
                        x = -10f;
                    else x = 10f;
                    vec2 v = new vec2(x, 0f);
                    y = (-AB.x * v.x) / AB.y;
                }

                vec2 norm = new vec2(x, y).Normalize();


                delta = PosB - PosA;
                var ln = delta.Length();
                delta = delta.Normalize();
                float charPX = 5f;
                float strLength = charPX * StringRepresent.Length;
                if (sourcePos.x < stockPos.x)
                {
                    strLength *= -1f;
                    norm *= -20f;
                }
                else norm *= 20;
                float offset = (ln / 2f) + (strLength / 2);
                WeightPos = PosA + (delta * offset);
                WeightPos += norm;
            }
        }
        public override string PosKey(vec2 pos, float r)
        {
            vec2 a = PosA;
            vec2 b = PosB;
            float bigX = a.x > b.x ? a.x : b.x;
            float bigY = a.y > b.y ? a.y : b.y;
            float smallX = b.x < a.x ? b.x : a.x;
            float smallY = b.y < a.y ? b.y : a.y;
            if (pos.y > bigY + 15f || pos.y < smallY - 15f)
                return null;
            if (pos.x > bigX + 15f || pos.x < smallX - 15f)
                return null;
            float x = pos.x;
            float y = pos.y;
            float eps = 0.1f;
            if (b.y - a.y == 0f) b.y += 1f;
            if (b.x - a.x == 0f) b.x += 1f;
            if (Math.Abs(b.y - a.y) < 30f) eps = 1.0f;
            if (Math.Abs(b.x - a.x) < 30f) eps = 1.0f;
            float res = ((x - a.x) / (b.x - a.x)) - ((y - a.y) / (b.y - a.y));
            if (Math.Abs(res) <= eps)
                return Key;
            return null;
        }
    }
}
