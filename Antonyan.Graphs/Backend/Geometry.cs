using System;


namespace Antonyan.Graphs.Backend
{
    public struct vec2
    {
        public float x;
        public float y;
        public vec2(vec2 other) : this(other.x, other.y)
        {

        }
        public vec2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static vec2 operator +(vec2 a, vec2 b)
        {
            return new vec2(a.x + b.x, a.y + b.y);
        }

        public static vec2 operator *(vec2 a, vec2 b)
        {
            return new vec2(a.x * b.x, a.y * b.y);
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

    }
}
