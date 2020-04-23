using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.UICommandArgs
{
    public class UIRemoveModelsArgs : UIFieldUpdateArgs
    {
        public UIRemoveModelsArgs(List<GraphModels> models) 
            : base(nameof(RemoveModelsCommand))
        {
            Models = models;
        }
        public List<GraphModels> Models { get; private set; }
    }
}
