using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;

namespace Antonyan.Graphs.Backend.Commands
{
    
    public class AddVertexCommand<TVertex, TWeight> : ICommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private AddVertexArgs<TVertex, TWeight> args;
        public static string Name { get { return "AddVertex"; } }
        public AddVertexCommand() { }
        public AddVertexCommand(AddVertexArgs<TVertex, TWeight> args = null)
        {
            this.args = args;
        }
        public ICommand Clone(EventArgs args)
        {
            return new AddVertexCommand<TVertex, TWeight>((AddVertexArgs<TVertex, TWeight>)args);
        }

        public void Execute()
        {
            args.Field.AddVertex(args.Vertex, args.Coord);
        }

        public string HelpMessage()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            args.Field.RemoveVertex(args.Vertex);
        }
    }
}
