using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.Geometry;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class VertexEventArgs<TVertex> : EventArgs
        where TVertex : AVertex
    {
        public VertexEventArgs(TVertex v) { Vertex = v; }
        public TVertex Vertex { get; private set; }
    }

    //public class VertexEventArgsWithCoord<TVertex> : VertexEventArgs<TVertex>
    //    where TVertex : AVertex
    //{
    //    public VertexEventArgsWithCoord(TVertex v, Vec2 coord) : base(v) 
    //    {
    //        Coord = coord;
    //    }
    //    public Vec2 Coord { get; private set; }
    //}
    public class RemoveVertexEventArgs<TVertex, TWeight> : VertexEventArgs<TVertex>
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

    public class AddVertexEventArgs<TVertex, TWeight> : RemoveVertexEventArgs<TVertex, TWeight>
         where TVertex : AVertex, new()
         where TWeight : AWeight, new()
    {
        public AddVertexEventArgs(TVertex v, Vec2 coord, Field<TVertex, TWeight> field)
            : base(v, field)
        {
            Coord = coord;
        }
        public Vec2 Coord { get; private set; }
    } 
}
