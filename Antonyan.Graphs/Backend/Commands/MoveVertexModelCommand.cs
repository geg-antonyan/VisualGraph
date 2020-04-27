
using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
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
    public class MoveVertexModelCommand : AFieldCommand, IStoredCommand 
    {
        MoveVertexModelCommandArgs _args;
        public MoveVertexModelCommand(IModelField field)
            : base(field)
        {

        }
        public MoveVertexModelCommand(MoveVertexModelCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }

        public ICommand Clone(ACommandArgs args)
        {
            return new MoveVertexModelCommand((MoveVertexModelCommandArgs)args, Field);
        }

        public void Execute()
        {
            Field.MoveVertexModel(_args.VertexKey, _args.NewPos);
        }

        public void Undo()
        {
            Field.MoveVertexModel(_args.VertexKey, _args.LastPos);
        }
    }
}
