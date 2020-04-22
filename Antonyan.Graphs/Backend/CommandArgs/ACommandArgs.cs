using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board;

namespace Antonyan.Graphs.Backend.CommandArgs
{

    public abstract class ACommandArgs
    {

    }

    public abstract class AFieldCommandArgs<TVertex, TWeight> : ACommandArgs
        where TVertex : AVertex, new()
        where TWeight  : AWeight, new()
    {
        public AFieldCommandArgs(GraphModelsField<TVertex, TWeight> field)
        {
            Field = field;
        }
        public GraphModelsField<TVertex, TWeight> Field;
    }
}
