using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Gui.Models
{
    public class Circle :  DrawModel
    {

        private readonly string mark;
        private readonly vec2 posMark;
        private static float R;
        private readonly mat3 translate;
        private static vec3[] circle;
        public Circle(vec2 pos, string mark)
        {
            this.mark = mark;
            translate = Transforms.Translate(pos.x, pos.y);
            posMark = new vec2(mark.Length == 1 ? pos.x - R / 2f + 2f : pos.x - R + 6f,  pos.y - R / 2f);
        }
        public void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
        {
            graphic.DrawString(mark, font, brush, new RectangleF(posMark.x, posMark.y, R * 2f, R * 2f));
            vec3 A = translate * circle[0];
            for (int i = 1; i < circle.Length; i++)
            {
                vec3 B = translate * circle[i];
                vec2 a = (vec2)A, b = new vec2(B);
                if (Clip.RectangleClip(ref a, ref b, min, max))
                    graphic.DrawLine(pen, a.x, a.y, b.x, b.y);
                A = B;
            }
        }
        public static void GenerateCircle(float r, float dx)
        {
            R = r;
            circle = new vec3[(int)(r / dx * 4f + 2)];
            float x = -r, y = 0f;
            circle[0] = new vec3(x, y);
            int j = 1;
            x += dx;
            while (x <= r)
            {
                float y2 = r * r - x * x;
                if (y2 < 0) break;
                y = (float)Math.Sqrt(y2);
                circle[j++] = new vec3(x, y);
                x += dx;
            }
            x -= dx;
            while (x >= -r)
            {
                float y2 = r * r - x * x;
                if (y2 < 0) break;
                y = -(float)Math.Sqrt(y2);
                circle[j++] = new vec3(x, y);
                x -= dx;
            }
        }


    }
}
