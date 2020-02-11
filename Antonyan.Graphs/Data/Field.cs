using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Data
{
    public enum FieldEvents { AddVertex, RemoveVertex, AddEdge, RemoveEdge };
    public class FieldUpdateArgs<TVertex> : VertexEventArgs<TVertex>
        where TVertex : AVertex, new()
    {
        public FieldUpdateArgs(FieldEvents e, TVertex v) 
            : base(v)
        {
            Event = e;
        }
        public FieldEvents Event { get; private set; }
    }

    public class FieldUpdateVertexArgs<TVertex> : FieldUpdateArgs<TVertex>
        where TVertex : AVertex, new()
    {
        public FieldUpdateVertexArgs(FieldEvents e, TVertex v, vec2 coord)
            : base(e, v)
        {
            Coord = coord;
        }
        public vec2 Coord { get; private set; }
    }


    public class FieldUpdateEdgeArgs<TVertex, TWeight> : FieldUpdateArgs<TVertex>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public FieldUpdateEdgeArgs(FieldEvents e, TVertex v, TVertex edge, TWeight w = null) 
            : base(e, v)
        {
            Edge = edge;
            Weight = w;
        }
        public TVertex Edge { get; private set; }
        public TWeight Weight { get; private set; }
    }

   

    public class Field<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public event EventHandler<FieldUpdateArgs<TVertex>> VertexUpdate;
        public event EventHandler<FieldUpdateEdgeArgs<TVertex, TWeight>> EdgeUpdate;

        public SortedDictionary<TVertex, vec2> Coords { get; private set; }
        public Graph<TVertex, TWeight> Graph { get; private set; }

        public Field(bool oriented, bool weighted, UserInterface ui)
        {
            UI = ui;
            Coords = new SortedDictionary<TVertex, vec2>();
            Graph = new Graph<TVertex, TWeight>(oriented, weighted);
            VertexUpdate += UI.FieldUpdate;
            EdgeUpdate += UI.FieldUpdate;
            
        }
        public  UserInterface UI { get; private set; }
        public bool IsOrgarph { get { return Graph.IsOrgraph; } }
        public bool IsWeight { get { return Graph.IsWeighted; } }
        public TVertex GetVertex(vec2 coord, float R)
        {
            foreach (var v in Coords)
            {
                vec2 c = v.Value;
                if (Math.Pow(coord.x - c.x, 2f) + Math.Pow(coord.y - c.y, 2f) <= R * R)
                    return v.Key;
            }
            return null;
        }
        public void AddVertex(TVertex v, vec2 coord)
        {
            var res = Graph.AddVertex(v);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                Coords.Add(v, coord);
                VertexUpdate?.Invoke(this, new FieldUpdateVertexArgs<TVertex>(FieldEvents.AddVertex, v, coord));
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
                EdgeUpdate?.Invoke(this, new FieldUpdateEdgeArgs<TVertex, TWeight>(FieldEvents.AddEdge, v, e, w));
        }
        public void RemoveVertex(TVertex v)
        {
            var res = Graph.RemoveVertex(v);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                vec2 c;
                Coords.TryGetValue(v, out c);
                Coords.Remove(v);
                VertexUpdate?.Invoke(this, new FieldUpdateVertexArgs<TVertex>(FieldEvents.RemoveVertex, v, c));
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
                EdgeUpdate?.Invoke(this, new FieldUpdateEdgeArgs<TVertex, TWeight>(FieldEvents.RemoveEdge, v, e));
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public bool HasAFreePlace(vec2 coord, float r)
        {
            foreach (var c in Coords)
            {
                //var res = 
                if (Math.Pow(coord.x - c.Value.x, 2.0) + Math.Pow(coord.y - c.Value.y, 2.0) <= r * r)
                    return false;
            }
            return true;
        }
    }
}
