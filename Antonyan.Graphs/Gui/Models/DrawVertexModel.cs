using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Gui.Models
{

    public class DrawVertexModel : ADrawModel
    {
        private static float R;
        private static vec3[] circle;

        private vec2 vertexStrPos;
        private mat3 translate;

        public DrawVertexModel(GraphModels model, bool marked)
            : base(model, marked)
        {
            var m = (VertexModel)Model;
            SetPos(m.Pos);
        }

        public void SetPos(vec2 pos)
        {

            translate = Transforms.Translate(pos.x, pos.y);
            //((VertexModel)Model).SetPos(pos);
            vertexStrPos = new vec2(((VertexModel)Model).VertexStr.Length == 1 ? pos.x - R / 2f + 2f : pos.x - R + 6f, pos.y - R / 2f);
        }

        public override void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
        {
            var m = (VertexModel)Model;
            if (Clip.SimpleClip(vertexStrPos, max, min, R / 2f))
                graphic.DrawString(m.VertexStr, font, brush, new RectangleF(vertexStrPos.x, vertexStrPos.y, R * 2f, R * 2f));
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

        public override string PosRepresent(vec2 pos, float r)
        {
            var m = (VertexModel)Model;
            if (Math.Pow(pos.x - m.Pos.x, 2.0) + Math.Pow(pos.y - m.Pos.y, 2.0) <= r * r)
                return m.GetRepresentation();
            return null;
        }
    }
}
