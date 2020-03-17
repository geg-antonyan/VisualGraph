﻿using System;
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

        public string Vertex { get; private set; } 
        public vec2 Pos { get; private set; }
        private vec2 vertexPos;
        private static float R;
        private  mat3 translate;
        private static vec3[] circle;
        public Circle(vec2 pos, string mark)
        {
            Pos = pos;
            this.Vertex = mark;
            translate = Transforms.Translate(pos.x, pos.y);
            vertexPos = new vec2(mark.Length == 1 ? pos.x - R / 2f + 2f : pos.x - R + 6f,  pos.y - R / 2f);
        }

        public void ChangePos(vec2 pos)
        {
            Pos = pos;
            translate = Transforms.Translate(Pos.x, Pos.y);
            vertexPos = new vec2(Vertex.Length == 1 ? Pos.x - R / 2f + 2f : Pos.x - R + 6f, Pos.y - R / 2f);
        }
        public override int GetHashCode()
        {
            return Vertex.GetHashCode();
        }
        public void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
        {
            if (Clip.SimpleClip(vertexPos, max, min, R / 2f))
                graphic.DrawString(Vertex, font, brush, new RectangleF(vertexPos.x, vertexPos.y, R * 2f, R * 2f));
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
