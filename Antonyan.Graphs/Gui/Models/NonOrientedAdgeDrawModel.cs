using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Board;
namespace Antonyan.Graphs.Gui.Models
{
    public class NonOrientedEdgeDrawModel : AEdgeDrawModel
    {
        public NonOrientedEdgeDrawModel(GraphModels model, bool marked = false)
            : base(model, 20f, marked)
        {

        }
        public override string PosRepresent(vec2 pos, float r)
        {
            vec2 a = posA;
            vec2 b = posB;
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
                return GetRepresent();
            return null;
        }

        protected override void DrawEdge(Graphics graphic, Pen pen, vec2 min, vec2 max)
        {
            vec2 start = new vec2(posA);
            vec2 end = new vec2(posB);
            if (Clip.RectangleClip(ref start, ref end, min, max))
                graphic.DrawLine(pen, start.x, start.y, end.x, end.y);
        }

    }
}
