using System;
using System.Collections.Generic;
using System.Text;

namespace Antonyan.Graphs.Desk.Geometry
{
    public class Vec3
    {
        public float x, y, z;
        public Vec3() { }
        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vec3 operator * (Vec3 a, Vec3 b)
        {
            return new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public float this[int i]
        {
            get 
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    default: return y;
                }
            }
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: y = value; break;
                }
            }
        }
        public static float Dot(Vec3 a, Vec3 b)
        {
            Vec3 c = a * b;
            return c.x + c.y + c.z;
        }

        public Vec2 Normalize()
        {
            return new Vec2(x / z, y / z);
        }
    }
}
