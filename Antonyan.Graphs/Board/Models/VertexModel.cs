using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board;

namespace Antonyan.Graphs.Board.Models
{
    public class VertexModel : GraphModels
    {
        public VertexModel(string vertex, vec2 pos)
        {
            Pos = pos;
            VertexStr = vertex;
        }
        public vec2 Pos { get; private set; }
        public string VertexStr { get; private set; }

        public void SetPos(vec2 pos)
        {
            Pos = pos;
        }
    }
}
