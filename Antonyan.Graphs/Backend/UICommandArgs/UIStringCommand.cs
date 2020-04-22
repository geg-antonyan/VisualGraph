using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.UICommandArgs
{

    public class UIStringArgs : UIEventArgs
    {
        public UIStringArgs(string message)
            : base("StringCommand")
        {
            Message = message;
        }
        public string Message { get; private set; }
    }

}
