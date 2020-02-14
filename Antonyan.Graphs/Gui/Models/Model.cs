using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Gui.Models
{
    public interface Model
    {
        void Draw(vec2 min, vec2 max);
    }

    public abstract class DrawObject
    {
        protected readonly Graphics graphic;
        protected readonly Pen pen;
        protected readonly Brush brush;
        protected readonly Font font;
        public DrawObject(Graphics graphic, Pen pen, Brush brush, Font font)
        {
            this.graphic = graphic;
            this.pen = pen;
            this.brush = brush;
            this.font = font;
        }
    }
}
