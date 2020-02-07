using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.Geometry;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Data
{

    public class FieldVertexEventArgs<TVertex> : EventArgs
        where TVertex : AVertex, new()
    {
        public FieldEvents Event { get; private set; }
        public TVertex Vertex { get; private set; }
        public Vec2 Coord { get; private set; }
        public FieldVertexEventArgs(FieldEvents e, TVertex v, Vec2 coord)
        {
            Event = e;
            Vertex = v;
            Coord = coord;
        }
    }

    public class FieldEdgeEventArgs<TVertex, TWeight> : EventArgs
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public FieldEvents Event { get; private set; }
        public TVertex Vertex1 { get; private set; }
        public TVertex Vertex2 { get; private set; }
        public TWeight Weight { get; private set; }
        public FieldEdgeEventArgs(FieldEvents e, TVertex v1, TVertex v2, TWeight w = null)
        {
            Event = e;
            Vertex1 = v1;
            Vertex2 = v2;
            Weight = w;
        }
    }

    public enum FieldEvents { AddVertex, RemoveVertex, AddEdge, RemoveEdge };

    public class Field<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public  event EventHandler<FieldVertexEventArgs<TVertex>> VertexUpdate;
        public event EventHandler<FieldEdgeEventArgs<TVertex, TWeight>> EdgeUpdate;

        public SortedDictionary<TVertex, Vec2> Coords { get; private set; }
        public Graph<TVertex, TWeight> Graph { get; private set; }

        public Field(bool oriented, bool weighted, UserInterface ui)
        {
            UI = ui;
            Coords = new SortedDictionary<TVertex, Vec2>();
            Graph = new Graph<TVertex, TWeight>(oriented, weighted);
            VertexUpdate += UI.FieldUpdate;
            EdgeUpdate += UI.FieldUpdate;
        }
        public  UserInterface UI { get; private set; }
        public bool IsOrgarph { get { return Graph.IsOrgraph; } }
        public bool IsWeight { get { return Graph.IsWeighted; } }
        
        public void AddVertex(TVertex v, Vec2 coord)
        {
            var res = Graph.AddVertex(v);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                Coords.Add(v, coord);
                VertexUpdate?.Invoke(this, new FieldVertexEventArgs<TVertex>(FieldEvents.AddVertex, v, coord));
            }
            else
                throw new Exception(res.ToString());
        }
        public void AddEdge(TVertex v, TVertex e, TWeight w)
        {
            var res = Graph.AddEdge(v, e, w);
            if (res != Graph<TVertex, TWeight>.ReturnValue.Succsess)
                throw new Exception(res.ToString());
            else
                EdgeUpdate?.Invoke(this, new FieldEdgeEventArgs<TVertex, TWeight>(FieldEvents.AddEdge, v, e, w));
        }
        public void RemoveVertex(TVertex v)
        {
            var res = Graph.RemoveVertex(v);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                Vec2 c;
                Coords.TryGetValue(v, out c);
                Coords.Remove(v);
                VertexUpdate?.Invoke(this, new FieldVertexEventArgs<TVertex>(FieldEvents.RemoveVertex, v, c));
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public void RemoveEdge(TVertex v, TVertex e)
        {
            var res = Graph.RemoveEdge(v, e);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                Graph.RemoveEdge(v, e);
                EdgeUpdate?.Invoke(this, new FieldEdgeEventArgs<TVertex, TWeight>(FieldEvents.RemoveEdge, v, e));
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }
    }
}
