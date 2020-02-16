using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class RemoveElemsArgs<TVertex, TWeight> : EventArgs
       where TVertex : AVertex, new()
       where TWeight : AWeight, new()
    {
        public RemoveElemsArgs(Tuple<TVertex, vec2>[] vertices, 
            Tuple<TVertex, TVertex, TWeight>[] edges,
            Field<TVertex, TWeight> field)
        {
            Vertices = vertices;
            Edges = edges;
            Field = field;
        }
        public Tuple<TVertex, vec2>[]  Vertices { get; private set; }
        public Tuple<TVertex, TVertex, TWeight>[] Edges { get; private set; }
        public Field<TVertex, TWeight> Field { get; private set; }
    }
}
