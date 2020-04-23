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
            throw new NotImplementedException();
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
