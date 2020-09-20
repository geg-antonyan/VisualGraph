using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Util
{
    public class RGBcolor
    {
        private int _r, _g, _b;
        public static readonly RGBcolor Black = new RGBcolor(0, 0, 0);
        public static readonly RGBcolor White = new RGBcolor(255, 255, 255);
        public static readonly RGBcolor Red = new RGBcolor(255, 0, 0);
        public static readonly RGBcolor DarkGreen = new RGBcolor(0, 100, 0);
        public static readonly RGBcolor Blue = new RGBcolor(0, 0, 255);
        public static readonly RGBcolor Green = new RGBcolor(0, 255, 0);
        public static readonly RGBcolor Olive = new RGBcolor(128, 128, 0);
        public static readonly RGBcolor DeepPink = new RGBcolor(255, 20, 147);
        public static readonly RGBcolor SaddleBrown = new RGBcolor(136, 69, 19);
        public static readonly RGBcolor Orange = new RGBcolor(255, 69, 0);

        public static List<RGBcolor> Collection = new List<RGBcolor>()
        {
             Green, SaddleBrown, DeepPink, Red, DarkGreen,  Orange, Olive, 
        };
        public RGBcolor()
        {
            _r = 0;
            _g = 0;
            _b = 0;
        }

        public RGBcolor(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public int R 
        { 
            get { return _r; } 
            set { _r = CorrectValue(value); }
        }
        public int G
        {
            get { return _g; }
            set { _g = CorrectValue(value); } 
        }
        public int B
        {
            get { return _b; }
            set { _b = CorrectValue(value); }
        }
        private int CorrectValue(int value)
        {
            if (value > 255) return 255;
            else if (value < 0) return 0;
            else return value;
        }
    }
}
