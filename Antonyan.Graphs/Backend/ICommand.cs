using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.UICommandArgs;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board;

namespace Antonyan.Graphs.Backend
{
    public interface ICommand
    {
        ICommand Clone(UIEventArgs args);
        void Execute();
        void Undo();
    }
}
