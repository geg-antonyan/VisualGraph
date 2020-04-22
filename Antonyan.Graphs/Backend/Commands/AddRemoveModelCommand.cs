
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.UICommandArgs;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Backend.Commands
{
    public class AddRemoveModelCommand : ICommand
    {
        private List<GraphModels> graphModels;
        private UIAddRemoveModelArgs _args;
        private readonly bool raise;

        public static readonly string Name = nameof(AddRemoveModelCommand); 
        public AddRemoveModelCommand() { }
        public AddRemoveModelCommand(UIAddRemoveModelArgs args, bool raise = true)
        {
            this.raise = raise;
            _args = args;
        }

        public ICommand Clone(UIEventArgs args)
        {
            return new AddRemoveModelCommand((UIAddRemoveModelArgs)args);
        }

        public void Execute()
        {
            switch (_args.CommandOption)
            {
                case "AddGraphModel":
                    _args.Field.AddGraphModel(_args.GraphModel, raise);
                    break;
                case "RemoveGraphModel":
                    graphModels = _args.Field.RemoveModels(_args.GraphModel, raise);
                    break;
                default: break;
            }
        }

        public void Undo()
        {
            switch (_args.CommandOption)
            {
                case "AddGraphModel":
                    graphModels = _args.Field.RemoveModels(_args.GraphModel, raise);
                    break;
                case "RemoveGraphModel":
                    _args.Field.AddGraphModel(_args.GraphModel, raise);
                    foreach (var m in graphModels)
                    {
                        _args.Field.AddGraphModel(m);
                    }
                    break;
                default: break;
            }
        }
    }
}
