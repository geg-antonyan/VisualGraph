using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs;

namespace Antonyan.Graphs.Gui.Models
{
    public abstract class ADrawModel
    {
        public ADrawModel(GraphModels model, bool marked = false)
        {
            Model = model;
            Marked = marked;
        }
        public bool Marked { get; set; }
        public GraphModels Model { get; private set; }
        public abstract string PosRepresent(vec2 pos, float r);
        public abstract void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max);
    }



}
