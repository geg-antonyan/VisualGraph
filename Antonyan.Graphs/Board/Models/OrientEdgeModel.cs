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
    public class OrientEdgeModel : AEdgeDrawModel
    {
        public vec2 PosA1 { get; private set; }
        public vec2 PosA2 { get; private set; }
        
        public OrientEdgeModel(AVertexModel source, AVertexModel stock, string weight)
            : base(source, stock, weight)
        {
        }

        public override string PosKey(vec2 pos, float r)
        {
            return null;
        }
        public override void RefreshPos()
        {
            base.RefreshPos();
            vec2 posStock = Stock.Pos, posSource = Source.Pos;
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
            PosA1 = PosA + (normDirection * step);
            PosA1 += (norm * 20f);
            PosA2 = PosA + (normDirection * (step * 2f));
            PosA2 += (norm * 20f);
        }
    }
}
