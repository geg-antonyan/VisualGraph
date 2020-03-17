using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;

namespace Antonyan.Graphs.Backend.Commands
{
    public class RemoveElemsCommand<TVertex, TWeight> : ICommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private List<EdgeFieldCommandArgs<TVertex, TWeight>> noMarkedEdge;
        private RemoveElemsFieldCommandArgs<TVertex, TWeight> args;
        public static string Name { get { return "RemoveElements"; } }

        public RemoveElemsCommand() { }
        public RemoveElemsCommand(RemoveElemsFieldCommandArgs<TVertex, TWeight> args)
        {
            this.args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new RemoveElemsCommand<TVertex, TWeight>((RemoveElemsFieldCommandArgs<TVertex, TWeight>)args);
        }

        public void Execute()
        {
            args.Field.RemoveEdges(args.Edges);
            noMarkedEdge = args.Field.RemoveVertices(args.Vertices);
        }

        public string HelpMessage()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            foreach (var v in args.Vertices)
            {
                args.Field.AddVertex(v.Item1, v.Item2);
            }
            foreach (var edge in args.Edges)
            {
                args.Field.AddEdge(edge.Item1, edge.Item2, edge.Item3);
            }
            foreach (var edge in noMarkedEdge)
            {
                args.Field.AddEdge(edge.Source, edge.Stock, edge.Weight);
            }
        }
    }
}
