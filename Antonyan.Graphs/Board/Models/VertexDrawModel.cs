using Antonyan.Graphs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Board.Models
{

    public class Circle
    {
        private readonly vec3[] _circle;
        public Circle(float r, float dx)
        {
            vec3[] circle = new vec3[(int)(r / dx * 4f + 2)];
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
            _circle = circle;
        }
        public vec3[] Lines => _circle;
    }

    public class VertexDrawModel : AVertexModel
    {
        public vec2[] Lines { get; private set; }
        public vec2 VertexStrPos { get; private set; }
        public float R => GlobalParameters.Radius;
        
        public static readonly Circle _circle = new Circle(GlobalParameters.Radius, 1f);
        private mat3 _translate;
        

        public VertexDrawModel(string vertex, vec2 pos)
            : base(vertex, pos)
        {
            UpdatePos(pos);
        }
        public override void UpdatePos(vec2 pos)
        {
            base.UpdatePos(pos);
            float r = R;
            _translate = Transforms.Translate(pos.x, pos.y);
            VertexStrPos = new vec2(VertexStr.Length == 1 ? pos.x - r / 2f + 2f : pos.x - r + 6f, pos.y - r / 2f);
            Lines = _circle.Lines.Select(p => (vec2)(_translate * p)).ToArray();
        }
        public override string PosKey(vec2 pos, float r)
        {
            if (Math.Pow(pos.x - Pos.x, 2.0) + Math.Pow(pos.y - Pos.y, 2.0) <= r * r)
                return Key;
            return null;
        }
    }
}
