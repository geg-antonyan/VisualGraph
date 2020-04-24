using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class MoveVertexModelCommandArgs : ACommandArgs
    {
        public MoveVertexModelCommandArgs(vec2 newPos, vec2 lastPos, string key)
            : base(nameof(MoveVertexModelCommand))
        {
            NewPos = newPos;
            LastPos = lastPos;
            VertexKey = key;
        }
        public vec2 NewPos { get; private set; }
        public vec2 LastPos { get; private set; }
        public string VertexKey { get; private set; }
    }
}
