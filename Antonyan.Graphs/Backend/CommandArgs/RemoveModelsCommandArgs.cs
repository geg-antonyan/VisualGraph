using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class RemoveModelsCommandArgs : ACommandArgs
    {
        public RemoveModelsCommandArgs(List<GraphModel> models)
            : base(nameof(RemoveModelsCommand))
        {
            Models = models;
        }

        public List<GraphModel> Models { get; private set; }
    }
}
