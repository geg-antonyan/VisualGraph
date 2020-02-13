using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;
namespace Antonyan.Graphs.Backend.CommandArgs
{
    class RemoveEdgeArgs<TVertex, TWeight> : EventArgs
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public RemoveEdgeArgs(TVertex source, TVertex stock, Field<TVertex, TWeight> field)
        {
            Source = source;
            Stock = stock;
            Field = field;
        }
        public Field<TVertex, TWeight> Field { get; private set; }
        public TVertex Source { get; private set; }
        public TVertex Stock { get; private set; }
    }

   class AddEdgeArgs<TVertex, TWeight> : RemoveEdgeArgs<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public AddEdgeArgs(TVertex source, TVertex stock, TWeight weight, Field<TVertex, TWeight> field)
            : base(source, stock, field)
        {
            Weight = weight;
        }
        public TWeight Weight { get; private set; } = new TWeight();
    }
}
