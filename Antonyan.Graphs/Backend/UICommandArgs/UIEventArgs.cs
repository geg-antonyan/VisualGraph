using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.UICommandArgs
{
    public class UIEventArgs : EventArgs
    {
        public UIEventArgs(string cmdName)
        {
            CommandName = cmdName;
        }
        public string CommandName { get; private set; }
    }

}
