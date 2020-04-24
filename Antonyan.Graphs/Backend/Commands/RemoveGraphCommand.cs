using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class RemoveGraphCommand : AFieldCommand, INonStoredCommand
    {
        public RemoveGraphCommand(IModelField field)
            : base(field)
        {

        }
        public ICommand Clone(ACommandArgs args)
        {
            return new RemoveGraphCommand(Field);
        }

        public void Execute()
        {
            Field.Clear();
        }
    }
}
