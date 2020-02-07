using System;
using System.Collections.Generic;
using System.Text;

namespace Antonyan.Graphs.Backend.Geometry
{
    public class Vec2
    {
        public float x;
        public float y;
        public Vec2()
        {

        }
        public Vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vec2 operator + (Vec2 a, Vec2 b)
        {
            return new Vec2(a.x + b.x, a.y + b.y);
        }

        public static Vec2 operator * (Vec2 a, Vec2 b)
        {
            return new Vec2(a.x * b.x, a.y * b.y);
        }

        public float this[int i]
        {
            get { return i == 0 ? x : y; }
            set 
            { 
                if (i == 0)
                    x = value;
                else y = value;
            }
        }

        public static float Dot(Vec2 a, Vec2 b)
        {
            Vec2 c = a * b;
            return c.x + c.y;
        }

    }
}
