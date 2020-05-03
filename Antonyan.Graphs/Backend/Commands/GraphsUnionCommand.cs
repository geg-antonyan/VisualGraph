using Antonyan.Graphs.Board;
using Antonyan.Graphs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class GraphUnionCommandArgs : ACommandArgs
    {
        public string[] Names { get; private set; }
        public string NewName { get; private set; }
        public GraphUnionCommandArgs(string[] names, string newName)
            : base(nameof(GraphsUnionCommand))
        {
            Names = names;
            NewName = newName;
        }
    }

    public class GraphsUnionCommand : AFieldCommand, INonStoredCommand
    {
        private readonly GraphUnionCommandArgs _args;
        public GraphsUnionCommand(IModelField field)
            : base(field)
        { }
        public GraphsUnionCommand(GraphUnionCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new GraphsUnionCommand((GraphUnionCommandArgs)args, Field);
        }

        public void Execute()
        {
            Field.UnionGraphs(_args.Names, _args.NewName);
        }

        //public void Undo()
        //{
        //    Field.RemoveStoredGraph(_args.NewName, true);
        //}
    }
}
