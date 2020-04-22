using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;

using Antonyan.Graphs.Backend.CommandArgs;

namespace Antonyan.Graphs.Backend.Commands
{
    public class MoveVertexFieldCommand<TVertex, TWeight> : 
        AFieldCommand<MoveVertexFieldCommandArgs<TVertex, TWeight>, TVertex, TWeight>,
        ICommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public static readonly string Name = "MoveVertexPos";
        public MoveVertexFieldCommand() : base() { }
        public MoveVertexFieldCommand(MoveVertexFieldCommandArgs<TVertex, TWeight> args)
            : base(args)
        {

        }
        public ICommand Clone(ACommandArgs args)
        {
            return new MoveVertexFieldCommand<TVertex, TWeight>((MoveVertexFieldCommandArgs<TVertex, TWeight>) args);
        }

        public void Execute()
        {
            args.Field.ChangeVertexModelPos(args.Represent, args.NewPos);
        }

        public string HelpMessage()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            args.Field.ChangeVertexModelPos(args.Represent, args.LastPos);
        }
    }
}
