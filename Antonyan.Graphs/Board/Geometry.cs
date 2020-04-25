using System;


namespace Antonyan.Graphs.Board
{
    public class vec2
    {
        public float x;
        public float y;
        public vec2() { }
        public vec2(vec2 other) : this(other.x, other.y)
        {

        }
        public vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public vec2(vec3 v)
        {
            x = v.x;
            y = v.y;
        }

        public static vec2 operator -(vec2 a, vec2 b)
        {
            return new vec2(a.x - b.x, a.y - b.y);
        }

        public static vec2 operator +(vec2 a, vec2 b)
        {
            return new vec2(a.x + b.x, a.y + b.y);
        }

        public static vec2 operator *(vec2 a, vec2 b)
        {
            return new vec2(a.x * b.x, a.y * b.y);
        }

        public static vec2 operator *(vec2 v, float s)
        {
            return new vec2(v.x * s, v.y * s);
        }

        public override bool Equals(object obj)
        {
            var b = (vec2)obj;
            return x == b.x && y == b.y;
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

        public static float Dot(vec2 a, vec2 b)
        {
            vec2 c = a * b;
            return c.x + c.y;
        }

        public static explicit operator vec2(vec3 v)
        {
            return new vec2(v.x, v.y);
        }

        public float Length()
        {
            return (float)Math.Sqrt(x * x + y * y);
        }
        public vec2 Normalize()
        {
            float m = this.Length();
            return new vec2(x / m, y / m);
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return x.ToString() + " " + y.ToString();
        }

    }

    public class vec3
    {
        public float x, y, z;
        public vec3() { }
        public vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 1f;
        }
        public vec3(float x, float y, float z)
            : this(x, y)
        {
            this.z = z;
        }
        public static vec3 operator *(vec3 a, vec3 b)
        {
            return new vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static vec3 operator +(vec3 a, vec3 b)
        {
            return new vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public float this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return x;
                    case 1: return y;
                    default: return z;
                }
            }
            set
            {
                switch (i)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: z = value; break;
                }
            }
        }
        public static float Dot(vec3 a, vec3 b)
        {
            vec3 c = a * b;
            return c.x + c.y + c.z;
        }

        public vec2 Normalize()
        {
            return new vec2(x / z, y / z);
        }
    }

    public class mat3
    {
        public vec3 row1, row2, row3;
        public mat3() { }
        public mat3(vec3 a, vec3 b, vec3 c)
        {
            row1 = a;
            row2 = b;
            row3 = c;
        }
        public mat3(float val)
        {
            row1 = new vec3(val, 0f, 0f);
            row2 = new vec3(0f, val, 0f);
            row3 = new vec3(0f, 0f, val);
        }

        public vec3 this[int i]
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
                switch (i)
                {
                    case 0: row1 = value; break;
                    case 1: row2 = value; break;
                    default: row3 = value; break;
                }
            }
        }
        public mat3 Transpose()
        {
            mat3 res = new mat3();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    res[i][j] = this[j][i];
            return res;
        }
        public static vec3 operator *(mat3 m, vec3 v)
        {
            vec3 res = new vec3();
            for (int i = 0; i < 3; i++)
                res[i] = vec3.Dot(m[i], v);
            return res;
        }
        public static mat3 operator *(mat3 a, mat3 b)
        {
            mat3 res = new mat3(), trans = b.Transpose();
            for (int i = 0; i < 3; i++)
                res[i] = a * trans[i];
            return res;
        }


    }

    public static class Transforms
    {
        public static mat3 Translate(float x, float y)
        {
            return new mat3(new vec3(1f, 0f, x),
                            new vec3(0f, 1f, y),
                            new vec3(0f, 0f, 1f));
        }


    }

    public static class Clip
    {
        public static bool SimpleClip(vec2 v, vec2 max, vec2 min, float r)
        {
            if (v.x + r > max.x) return false;
            if (v.x - r < min.x) return false;
            if (v.y + r > max.y) return false;
            if (v.y - r < min.y) return false;
            return true;
        }

        private static void Code_XX01_XX10(ref vec2 A, vec2 B, float x)
        {
            A.y += (B.y - A.y) * (x - A.x) / (B.x - A.x);
            A.x = x;
        }
        private static void Code_01XX_10XX(ref vec2 A, vec2 B, float y)
        {
            A.x += (B.x - A.x) * (y - A.y) / (B.y - A.y);
            A.y = y;
        }
        private static byte CodeKS(vec2 P, vec2 min, vec2 max)
        {
            byte code = 0b_0000;
            if (P.x < min.x)
                code += 0b_0001;
            else if (P.x > max.x)
                code += 0b_0010;
            if (P.y < min.y)
                code += 0b_0100;
            else if (P.y > max.y)
                code += 0b_1000;
            return code;
        }
        public static bool RectangleClip(ref vec2 A, ref vec2 B, vec2 min, vec2 max)
        {
            byte codeA = CodeKS(A, min, max);
            byte codeB = CodeKS(B, min, max);
            while ((codeA | codeB) != 0b_0000)
            {
                if ((codeA & codeB) != 0b_0000)
                    return false;
                if (codeA == 0b_0000)
                {
                    Swap(ref A, ref B);
                    Swap(ref codeA, ref codeB);
                }
                if ((codeA & 0b_0001) != 0b_0000)
                    Code_XX01_XX10(ref A, B, min.x);
                else if ((codeA & 0b_0010) != 0b_0000)
                    Code_XX01_XX10(ref A, B, max.x);
                else if ((codeA & 0b_0100) != 0b_0000)
                    Code_01XX_10XX(ref A, B, min.y);
                else Code_01XX_10XX(ref A, B, max.y);
                codeA = CodeKS(A, min, max);
            }
            return true;
        }
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

    }
}
