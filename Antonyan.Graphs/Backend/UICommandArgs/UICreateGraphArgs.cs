using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.UICommandArgs
{
    public class UICreateGraphArgs : UIEventArgs
    {
        public UICreateGraphArgs(bool oriented, bool weighted)
            : base("CreateGraph")
        {
            Oriented = oriented;
            Weighted = weighted;
        }
        public bool Oriented { get; private set; }
        public bool Weighted { get; private set; }
    }
}
