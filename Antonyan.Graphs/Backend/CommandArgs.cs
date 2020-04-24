using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend
{
    public class ACommandArgs : EventArgs
    {
        public ACommandArgs(string commandName) => CommandName = commandName;
        public string CommandName { get; private set; }
    }
}
