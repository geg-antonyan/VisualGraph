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
    
    public class AddVertexFieldCommand<TVertex, TWeight> : ICommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private VertexFieldCommandArgs<TVertex, TWeight> args;
        public static readonly string Name = "AddVertex"; 
        public AddVertexFieldCommand() { }
        public AddVertexFieldCommand(VertexFieldCommandArgs<TVertex, TWeight> args = null)
        {
            this.args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new AddVertexFieldCommand<TVertex, TWeight>((VertexFieldCommandArgs<TVertex, TWeight>)args);
        }

        public void Execute()
        {
            args.Field.AddVertex(args.Vertex, args.Pos);
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
