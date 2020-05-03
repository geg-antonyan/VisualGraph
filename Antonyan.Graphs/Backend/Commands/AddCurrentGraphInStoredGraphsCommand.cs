using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class AddCurrentGraphInStoredGraphsCommandArgs : ACommandArgs
    {
        public string AdjListName { get; private set; }

        public AddCurrentGraphInStoredGraphsCommandArgs(string adjListName)
            : base(nameof(AddCurrentGraphInStoredGraphsCommand))
        {
            AdjListName = adjListName;
        }
    }

    public class AddCurrentGraphInStoredGraphsCommand : AFieldCommand, INonStoredCommand
    {
        private readonly AddCurrentGraphInStoredGraphsCommandArgs _args;

        public AddCurrentGraphInStoredGraphsCommand(IModelField field)
            : base(field)
        { }
        public AddCurrentGraphInStoredGraphsCommand(
            AddCurrentGraphInStoredGraphsCommandArgs args,
            IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new AddCurrentGraphInStoredGraphsCommand(
               (AddCurrentGraphInStoredGraphsCommandArgs)args, Field);
        }

        public void Execute()
        {
            Field.AddCurrentGraphInStoredGraphs(_args.AdjListName);
        }

        public void Undo()
        {
            Field.RemoveStoredGraph(_args.AdjListName);
        }
    }
}
