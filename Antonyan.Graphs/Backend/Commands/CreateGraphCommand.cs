
using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class CreateGraphCommandArgs : ACommandArgs
    {
        public CreateGraphCommandArgs(bool oriented, bool weighted)
            : base(nameof(CreateGraphCommand))
        {
            Oriented = oriented;
            Weighted = weighted;
        }
        public bool Oriented { get; private set; }
        public bool Weighted { get; private set; }
    }


    public class CreateGraphCommand : AFieldCommand, INonStoredCommand
    {
        private CreateGraphCommandArgs _args;
        public CreateGraphCommand(IModelField field) : base(field)
        { }

        public CreateGraphCommand(CreateGraphCommandArgs args, IModelField field) 
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new CreateGraphCommand((CreateGraphCommandArgs)args, Field);
        }

        public void Execute()
        {
            Field.CreateGraph(_args.Oriented, _args.Weighted);
        }
    }
}
