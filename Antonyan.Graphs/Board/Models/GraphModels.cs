using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board.Models
{
    public abstract class GraphModel
    {
        protected readonly string _key;
        public string StringRepresent { get; protected set; }

        public GraphModel(string key, bool marked = false)
        {
            _key = key;
            Marked = marked;
        }
        public string Key => _key;
        public bool Marked { get; set; }
        public void SetStringPresent(string str) => StringRepresent = str;
        public abstract string PosKey(vec2 pos, float r);
    }
}
