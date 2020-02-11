using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Geometry
{
    public static class Clip
    {
        public static bool SimpleClip(Vec2 v, Vec2 max, Vec2 min, float r)
        {
            if (v.x + r > max.x) return false;
            if (v.x - r < min.x) return false;
            if (v.y + r > max.y) return false;
            if (v.y - r < min.y) return false;
            return true;
        }

        private static void Code_XX01_XX10(ref Vec2 A, Vec2 B, float x)
        {
            A.y += (B.y - A.y) * (x - A.x) / (B.x - A.x);
            A.x = x;
        }
        private static void Code_01XX_10XX(ref Vec2 A, Vec2 B, float y)
        {
            A.x += (B.x - A.x) * (y - A.y) / (B.y - A.y);
            A.y = y;
        }
        private static byte CodeKS(Vec2 P, Vec2 min, Vec2 max)
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
        public static bool RectangleClip(ref Vec2 A, ref Vec2 B, Vec2 min, Vec2 max)
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
