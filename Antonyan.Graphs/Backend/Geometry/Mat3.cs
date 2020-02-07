using System;
using System.Collections.Generic;
using System.Text;

namespace Antonyan.Graphs.Backend.Geometry
{
    public class Mat3
    {
        public Vec3 row1, row2, row3;
        public Mat3() { }
        public Mat3(Vec3 a, Vec3 b, Vec3 c)
        {
            row1 = a;
            row2 = b;
            row3 = c;
        }
        public Mat3(float val)
        {
            row1 = new Vec3(val, 0f, 0f);
            row2 = new Vec3(0f, val, 0f);
            row3 = new Vec3(0f, 0f, val);
        }

        public Vec3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return row1;
                    case 1: return row2;
                    default: return row3;
                }
            }
            set
            {
                switch(i)
                {
                    case 0: row1 = value; break;
                    case 1: row2 = value; break;
                    default: row3 = value; break;
                }
            }
        }
        public Mat3 Transpose()
        {
            Mat3 res = new Mat3();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    res[i][j] = this[j][i];
            return res;
        }
        public static Vec3 operator* (Mat3 m, Vec3 v)
        {
            Vec3 res = new Vec3();
            for (int i = 0; i < 3; i++)
                res[i] = Vec3.Dot(m[i], v);
            return res;
        }
        public static Mat3 operator* (Mat3 a, Mat3 b)
        {
            Mat3 res = new Mat3(), trans = b.Transpose();
            for (int i = 0; i < 3; i++)
                res[i] = a * trans[i];
            return res;
        }
    }
}
