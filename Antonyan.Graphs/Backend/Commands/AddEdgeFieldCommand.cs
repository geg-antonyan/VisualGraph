using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;

namespace Antonyan.Graphs.Backend.Commands
{
    class AddEdgeFieldCommand<TVertex, TWeight> : 
        AFieldCommand<EdgeFieldCommandArgs<TVertex, TWeight>, TVertex, TWeight>, 
        ICommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public static string Name { get; } = "AddEdge";
        public AddEdgeFieldCommand() : base() { }
        public AddEdgeFieldCommand(EdgeFieldCommandArgs<TVertex, TWeight> args = null)
            : base(args)
        {
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new AddEdgeFieldCommand<TVertex, TWeight>((EdgeFieldCommandArgs<TVertex, TWeight>)args);
        }

        public void Execute()
        {
            args.Field.AddEdgeModel(args.Source, args.Stock, args.Weight);
        }

        public string HelpMessage()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            args.Field.RemoveEdgeModel(args.Source, args.Stock);
        }
    }
}
