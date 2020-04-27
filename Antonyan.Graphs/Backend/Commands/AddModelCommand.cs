using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Board;

namespace Antonyan.Graphs.Backend.Commands
{

    public class AddModelCommandArgs : ACommandArgs
    {
        public AddModelCommandArgs(GraphModel model) :
            base(nameof(AddModelCommand))
            => Model = model;
        public GraphModel Model { get; private set; }
    }


    class AddModelCommand : AFieldCommand, IStoredCommand
    {
        private AddModelCommandArgs _args;

        public AddModelCommand(IModelField field) : base(field) { }

        public AddModelCommand(AddModelCommandArgs args, IModelField field) 
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new AddModelCommand((AddModelCommandArgs)args, Field);
        }

        public void Execute()
        {
            Field.AddGraphModel(_args.Model);
        }

        public void Undo()
        {
            Field.RemoveGraphModel(_args.Model);
        }
    }
}
