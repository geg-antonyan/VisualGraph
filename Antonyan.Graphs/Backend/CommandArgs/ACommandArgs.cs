using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
namespace Antonyan.Graphs.Backend.CommandArgs
{

    public class ACommandArgs
    {

    }
    public abstract class AFieldCommandArgs<TVertex, TWeight> : ACommandArgs
        where TVertex : AVertex, new()
        where TWeight  : AWeight, new()
    {
        public AFieldCommandArgs(Field<TVertex, TWeight> field)
        {
            Field = field;
        }
        public Field<TVertex, TWeight> Field;
    }
}
