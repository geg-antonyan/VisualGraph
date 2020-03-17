using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend.CommandArgs
{

    public class VertexFieldCommandArgs<TVertex, TWeight> : AFieldCommandArgs<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public VertexFieldCommandArgs(Field<TVertex, TWeight> field, TVertex vertex, vec2 pos)
            : base(field)
        {
            Vertex = vertex;
            Pos = pos;
        }
        public TVertex Vertex { get; private set; }
        public vec2 Pos { get; private set; }
    }
}
