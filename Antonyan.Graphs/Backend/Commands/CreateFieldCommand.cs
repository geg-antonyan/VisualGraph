using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend.Commands
{
    class CreateFieldCommand<TVertex, TWeight> : ICommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private CreatFieldArgs<TVertex, TWeight> args;
        public static string Name { get { return "CreateField"; } }
        public CreateFieldCommand() { }
        public CreateFieldCommand(CreatFieldArgs<TVertex, TWeight> args = null)
        {
            this.args = args;
        }
        public ICommand Clone(EventArgs args)
        {
            return new CreateFieldCommand<TVertex, TWeight>((CreatFieldArgs<TVertex, TWeight>)args);
        }

        public void Execute()
        {
            Field<TVertex, TWeight> field = new Field<TVertex, TWeight>(args.Oriented, args.Weighted, args.UI);
            args.UI.AttachField(field);
            args.CommandDispetcher.AttachField(field);
        }

        public string HelpMessage()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            args.UI.AttachField(null);
            args.CommandDispetcher.AttachField(null);
        }
    }
}
