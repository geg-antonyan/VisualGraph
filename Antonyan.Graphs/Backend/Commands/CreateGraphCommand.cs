using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
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
            Field.SetGraphOptions(_args.Oriented, _args.Weighted);
        }

        //public void Undo()
        //{
        //    Field.Clear();
        //}
    }
}
