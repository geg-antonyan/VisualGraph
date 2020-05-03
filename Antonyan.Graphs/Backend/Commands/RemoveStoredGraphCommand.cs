using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class RemoveStoredGraphCommandArgs : ACommandArgs
    {
        public string[] AdjListNames { get; private set; }

        public RemoveStoredGraphCommandArgs(string[] adjListNames)
            : base(nameof(RemoveStoredGraphCommand))
        {
            AdjListNames = adjListNames;
        }
    }
    public class RemoveStoredGraphCommand : AFieldCommand, INonStoredCommand
    {
        private readonly RemoveStoredGraphCommandArgs _args;

        public RemoveStoredGraphCommand(IModelField field) 
            : base(field)
        { }
        public RemoveStoredGraphCommand(RemoveStoredGraphCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }

        public ICommand Clone(ACommandArgs args)
        {
            return new RemoveStoredGraphCommand((RemoveStoredGraphCommandArgs)args, Field);
        }

        public void Execute()
        {
            for (int i = 0; i < _args.AdjListNames.Length - 1; i++)
                Field.RemoveStoredGraph(_args.AdjListNames[i], false);
            if (_args.AdjListNames.Length > 0)
                Field.RemoveStoredGraph(_args.AdjListNames[_args.AdjListNames.Length - 1], true);
        }
    }
}
