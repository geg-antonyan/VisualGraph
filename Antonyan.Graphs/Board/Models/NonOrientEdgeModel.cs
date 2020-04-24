using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Board.Models
{
    public class NonOrientEdgeModel : AEdgeDrawModel
    {
        public NonOrientEdgeModel(AVertexModel source, AVertexModel stock, string weight) 
            : base(source, stock, weight)
        {
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
