using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;

using Antonyan.Graphs.Board;


namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class MoveVertexFieldCommandArgs<TVertex, TWeight> : AFieldCommandArgs<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public MoveVertexFieldCommandArgs(GraphModelsField<TVertex, TWeight> field, string represent, vec2 lastPos, vec2 newPos)
            : base(field)
        {
            Represent = represent;
            NewPos = newPos;
            LastPos = lastPos;
            NewPos = newPos;
        }
        public vec2 LastPos { get; private set; }
        public vec2 NewPos { get; private set; }
        public string Represent { get; private set; }
    }
}
