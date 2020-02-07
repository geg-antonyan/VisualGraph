using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        ICommand Clone(EventArgs args);
        string HelpMessage();
    }
}
