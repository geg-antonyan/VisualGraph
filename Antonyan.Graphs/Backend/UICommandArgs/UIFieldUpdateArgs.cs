using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Board;

namespace Antonyan.Graphs.Backend.UICommandArgs
{
    public class UIFieldUpdateArgs : UIEventArgs
    {
        public UIFieldUpdateArgs(string cmdName)
            : base(cmdName)
        {

        }
        public void SetField(IField field) => Field = field;
        public IField Field { get; private set; }
    }
}
