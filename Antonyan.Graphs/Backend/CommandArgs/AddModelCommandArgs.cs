using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class AddModelCommandArgs : ACommandArgs
    {
        public AddModelCommandArgs(GraphModel model) :
            base(nameof(AddModelCommand))
            => Model = model;
        public GraphModel Model { get; private set; }
    }
}
