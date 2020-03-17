using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;
namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class EdgeFieldCommandArgs<TVertex, TWeight> : AFieldCommandArgs<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public EdgeFieldCommandArgs(Field<TVertex, TWeight> field, TVertex source, TVertex stock, TWeight weight)
            : base(field)
        {
            Source = source;
            Stock = stock;
            Weight = weight;
        }
        public TVertex Source { get; private set; }
        public TVertex Stock { get; private set; }
        public TWeight Weight { get; private set; }
    }
}
