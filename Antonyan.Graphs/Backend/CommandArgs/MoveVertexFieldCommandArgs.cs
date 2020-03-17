using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class MoveVertexFieldCommandArgs<TVertex, TWeight> : VertexFieldCommandArgs<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public MoveVertexFieldCommandArgs(Field<TVertex, TWeight> field, TVertex vertex, vec2 lastPos, vec2 newPos)
            : base(field, vertex, lastPos)
        {
            NewPos = newPos;
        }
        public vec2 NewPos { get; private set; }
    }
}
