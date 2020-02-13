using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;

namespace Antonyan.Graphs.Backend.Commands
{
    class AddEdgeCommand<TVertex, TWeight> : ICommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private AddEdgeArgs<TVertex, TWeight> args;
        public static string Name { get; } = "AddEdge";
        public AddEdgeCommand() { }
        public AddEdgeCommand(AddEdgeArgs<TVertex, TWeight> args = null)
        {
            this.args = args;
        }
        public ICommand Clone(EventArgs args)
        {
            return new AddEdgeCommand<TVertex, TWeight>((AddEdgeArgs<TVertex, TWeight>)args);
        }

        public void Execute()
        {
            args.Field.AddEdge(args.Source, args.Stock, args.Weight);
        }

        public string HelpMessage()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            args.Field.RemoveEdge(args.Source, args.Stock);
        }
    }
}
