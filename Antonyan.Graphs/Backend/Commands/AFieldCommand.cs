using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public abstract class AFieldCommand
    {
        public AFieldCommand(IModelField field) => Field = field;
        public IModelField Field { get; private set; }
    }
}
