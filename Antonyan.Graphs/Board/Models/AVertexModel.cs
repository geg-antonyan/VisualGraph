using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board.Models
{
    public abstract class AVertexModel : GraphModel
    {
        public AVertexModel(string vertex, vec2 pos)
            : base(vertex)
        {
            Pos = pos;
            VertexStr = vertex;
            StringRepresent = vertex;
        }

        public vec2 Pos { get; private set; }
        public string VertexStr { get; private set; }
        public virtual void UpdatePos(vec2 pos)
        {
            Pos = pos;
        }
    }
}
