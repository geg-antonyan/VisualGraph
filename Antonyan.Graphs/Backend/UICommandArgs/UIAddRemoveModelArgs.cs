using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend.Commands;

namespace Antonyan.Graphs.Backend.UICommandArgs
{
    public class UIAddRemoveModelArgs : UIFieldUpdateArgs
    {
        public UIAddRemoveModelArgs(string cmdNameOption, GraphModels model)
            : base(nameof(AddRemoveModelCommand))
        {
            CommandOption = cmdNameOption;
            GraphModel = model;
        }
        public string CommandOption { get; private set; }
        public GraphModels GraphModel { get; private set; }
    }
}
