using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class VertexArgs<TVertex> : EventArgs
        where TVertex : AVertex
    {
        public VertexArgs(TVertex v) { Vertex = v; }
        public TVertex Vertex { get; private set; }
    }
    public class RemoveVertexEventArgs<TVertex, TWeight> : VertexArgs<TVertex>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public RemoveVertexEventArgs(TVertex v, Field<TVertex, TWeight> field)
            : base(v)
        {
            Field = field;
        }
        public Field<TVertex, TWeight> Field { get; private set; }
    }



    public class AddVertexArgs<TVertex, TWeight> : RemoveVertexEventArgs<TVertex, TWeight>
         where TVertex : AVertex, new()
         where TWeight : AWeight, new()
    {
        public AddVertexArgs(TVertex v, vec2 coord, Field<TVertex, TWeight> field)
            : base(v, field)
        {
            Pos = coord;
        }
        public vec2 Pos { get; private set; }
    } 
}
