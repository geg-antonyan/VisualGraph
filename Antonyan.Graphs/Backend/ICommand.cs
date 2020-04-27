using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Backend
{
    public interface ICommand
    {
        void Execute();
        ICommand Clone(ACommandArgs args);
    }

    public interface IStoredCommand : ICommand
    {
        void Undo();
    }

    public interface INonStoredCommand : ICommand
    {

    }
}
