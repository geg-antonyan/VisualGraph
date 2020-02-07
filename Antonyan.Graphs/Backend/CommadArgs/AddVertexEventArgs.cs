using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.Geometry;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class AddVertexEventArgs<TVertex, TWeight> : EventArgs
         where TVertex : AVertex, new()
         where TWeight : AWeight, new()
    {
        private FieldVertexEventArgs<TVertex> vertex;
        
        public AddVertexEventArgs(TVertex v, Vec2 coord, Field<TVertex, TWeight> f)
        {
            vertex = new FieldVertexEventArgs<TVertex>(FieldEvents.AddVertex, v, coord);
            Field = f;
        }
        public Field<TVertex, TWeight> Field { get; private set; }
        public Vec2 Coord { get { return vertex.Coord; } } 
        public TVertex Vertex { get { return vertex.Vertex; } }
        
    } 
}
