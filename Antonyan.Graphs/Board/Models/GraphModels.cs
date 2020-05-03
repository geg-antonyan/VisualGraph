using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board.Models
{
    public enum ModelColor { Red, Green, Blue, Black }
    public abstract class GraphModel
    {
        protected readonly string _key;
        public static readonly RGBcolor DefaultColor = RGBcolor.Blue;
        private RGBcolor _color = DefaultColor;
        public string StringRepresent { get; protected set; }
        public RGBcolor Color
        {
            get
            {
                if (Marked)
                    return RGBcolor.Red;
                else return _color;
            }
            set
            {
                _color = value;
            }
        }
        public GraphModel(string key, bool marked = false)
        {
            _key = key;
            Marked = marked;
        }
        public string Key => _key;
       // public ModelColor { get;
        public bool Marked { get; set; }
        
        public void SetStringPresent(string str) => StringRepresent = str;
        public abstract string PosKey(vec2 pos, float r);
    }
}
