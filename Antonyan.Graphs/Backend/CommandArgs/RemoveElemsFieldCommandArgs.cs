using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class RemoveElemsFieldCommandArgs<TVertex, TWeight> : AFieldCommandArgs<TVertex, TWeight>
       where TVertex : AVertex, new()
       where TWeight : AWeight, new()
    {
        public RemoveElemsFieldCommandArgs(Tuple<TVertex, vec2>[] vertices, 
            Tuple<TVertex, TVertex, TWeight>[] edges,
            Field<TVertex, TWeight> field)
            : base(field)
        {
            Vertices = vertices;
            Edges = edges;
        }
        public Tuple<TVertex, vec2>[]  Vertices { get; private set; }
        public Tuple<TVertex, TVertex, TWeight>[] Edges { get; private set; }
    }
}
