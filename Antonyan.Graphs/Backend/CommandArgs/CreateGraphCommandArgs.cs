using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.CommandArgs
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
}
