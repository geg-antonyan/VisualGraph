using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board.Models
{
    public abstract class GraphModels
    {
        public string GetRepresentation()
        {
            return ServiceFunctions.ModelRepresentation(this);
        }
    }
}
