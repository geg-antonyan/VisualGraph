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
    public enum FieldEvents { AddVertex, RemoveVertex, AddEdge, RemoveEdge, RemoveVertices, RemoveEdges, ChangeVertexPos };
    public class FieldUpdateArgs : EventArgs
    {
        protected int hashCode;
        public FieldUpdateArgs(FieldEvents e)
        {
            Event = e;
        }
        public FieldEvents Event { get; private set; }
        public override int GetHashCode()
        {
            return hashCode;
        }
    }

    public class FieldUpdateVertexPos : FieldUpdateArgs
    {
        public FieldUpdateVertexPos(FieldEvents e, string vertex, vec2 pos)
            : base(e)
        {
            Vertex = vertex;
            Pos = pos;
            hashCode = Vertex.GetHashCode();
        }

        public string Vertex { get; private set; }
        public vec2 Pos { get; private set; }
    }

    public class FieldUpdateVertexArgs : FieldUpdateVertexPos
    {
        public FieldUpdateVertexArgs(FieldEvents e, string vertex, vec2 coord, List<FieldUpdateEdgeArgs> edges = null)
            : base(e, vertex, coord)
        {
            Edges = edges;
        }
        public List<FieldUpdateEdgeArgs> Edges { get; private set; }

    }

    public class FieldUpdateVerticesArgs : FieldUpdateArgs
    {
        public FieldUpdateVerticesArgs(FieldEvents e, List<FieldUpdateVertexArgs> vertices)
            : base(e)
        {
            Vertices = vertices;
        }
        public List<FieldUpdateVertexArgs> Vertices { get; private set; }
    }


    public class FieldUpdateEdgeArgs : FieldUpdateArgs
    {
        public FieldUpdateEdgeArgs(FieldEvents e, vec2 posSource, vec2 posStock, string source, string stock, string w)
            : base(e)
        {
            PosSource = posSource;
            PosStock = posStock;
            Source = source;
            Stock = stock;
            Weight = w;
            hashCode = (Source + " " + Stock + " " + Weight).GetHashCode();
        }
        public vec2 PosSource { get; private set; }
        public vec2 PosStock { get; private set; }
        public string Source { get; private set; }
        public string Stock { get; private set; }
        public string Weight { get; private set; }

        public int GetReverseHashCode() => (Stock + " " + Source + " " + Weight).GetHashCode();
    }

    public class FieldUpdateEdgesArgs : FieldUpdateArgs
    {
        public FieldUpdateEdgesArgs(FieldEvents e, List<FieldUpdateEdgeArgs> vertices)
            : base(e)
        {
            Edges = vertices;
        }
        public List<FieldUpdateEdgeArgs> Edges { get; private set; }
    }



    public class Field<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public event EventHandler<FieldUpdateArgs> FieldUpdate;

        public SortedDictionary<TVertex, vec2> Positions { get; private set; }
        public Graph<TVertex, TWeight> Graph { get; private set; }

        public Field(bool oriented, bool weighted, UserInterface ui)
        {
            UI = ui;
            Positions = new SortedDictionary<TVertex, vec2>();
            Graph = new Graph<TVertex, TWeight>(oriented, weighted);
            FieldUpdate += UI.FieldUpdate;
        }
        public UserInterface UI { get; private set; }
        public bool IsOrgarph { get { return Graph.IsOrgraph; } }
        public bool IsWeighted { get { return Graph.IsWeighted; } }
        public TVertex GetVertex(vec2 coord, float R)
        {
            foreach (var v in Positions)
            {
                vec2 c = v.Value;
                if (Math.Pow(coord.x - c.x, 2f) + Math.Pow(coord.y - c.y, 2f) <= R * R)
                    return v.Key;
            }
            return null;
        }

        public vec2 GetPos(TVertex v)
        {
            vec2 pos;
            if (Positions.TryGetValue(v, out pos))
                return pos;
            else return null;
        }

        public void ChangeVertexPos(TVertex v, vec2 newPos)
        {
            if (Positions.ContainsKey(v))
            {
                Positions[v] = newPos;
                FieldUpdate?.Invoke(this, new FieldUpdateVertexPos(FieldEvents.ChangeVertexPos, v.ToString(), newPos));
            }
        }
        public void AddVertex(TVertex v, vec2 coord)
        {
            var res = Graph.AddVertex(v);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                Positions.Add(v, coord);
                FieldUpdate?.Invoke(this, new FieldUpdateVertexArgs(FieldEvents.AddVertex, v.ToString(), coord));
            }
            else
                throw new Exception(res.ToString());
        }
        public void AddEdge(TVertex v, TVertex e, TWeight w = null)
        {
            var res = Graph.AddEdge(v, e, w);
            if (res != Graph<TVertex, TWeight>.ReturnValue.Succsess)
                throw new Exception(res.ToString());
            else
            {
                vec2 source = GetPos(v);
                vec2 stock = GetPos(e);
                FieldUpdate?.Invoke(this, new FieldUpdateEdgeArgs(FieldEvents.AddEdge, source, stock, v.ToString(), e.ToString(), w?.ToString()));
            }
        }

        public FieldUpdateVertexArgs RemoveVertex(TVertex v, bool raise = true)
        {
            
            var res = Graph.RemoveVertex(v);
            if (res.Return == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                var edges = new List<FieldUpdateEdgeArgs>();
                vec2 c;
                Positions.TryGetValue(v, out c);
                Positions.Remove(v);
                foreach (var edge in res.Edges)
                {
                    vec2 sourcePos, stockPos;
                    Positions.TryGetValue(edge.Item1, out sourcePos);
                    Positions.TryGetValue(edge.Item2, out stockPos);
                    if (sourcePos == null) sourcePos = c;
                    if (stockPos == null) stockPos = c;
                    edges.Add(new  FieldUpdateEdgeArgs(FieldEvents.RemoveEdge,  sourcePos, stockPos, edge.Item1.ToString(), edge.Item2.ToString(), edge.Item3?.ToString()));
                }
                var removeArgs = new FieldUpdateVertexArgs(FieldEvents.RemoveVertex, v.ToString(), c, edges);
                if (raise)
                    FieldUpdate?.Invoke(this, removeArgs);
                return removeArgs;
            }
            else
            {
                throw new Exception(res.Return.ToString());
            }
        }

        public List<EdgeFieldCommandArgs<TVertex, TWeight>> RemoveVertices(Tuple<TVertex, vec2>[] vertices)
        {
            List<FieldUpdateVertexArgs> args = new List<FieldUpdateVertexArgs>();
            List<EdgeFieldCommandArgs<TVertex, TWeight>> res = new List<EdgeFieldCommandArgs<TVertex, TWeight>>();
            foreach (var v in vertices)
            {
                var retEdges = RemoveVertex(v.Item1, false);
                args.Add(retEdges);
                foreach (var edge in retEdges.Edges)
                {
                    TVertex soruce = new TVertex(), stock = new TVertex();
                    TWeight weight = new TWeight(); ;
                    soruce.SetFromString(edge.Source);
                    stock.SetFromString(edge.Stock);
                    if (edge.Weight != null)
                    {
                        weight.SetFromString(edge.Weight);
                    }
                    else weight = null;
                    res.Add(new EdgeFieldCommandArgs<TVertex, TWeight>(this, soruce, stock, weight));
                    
                }
            }
            FieldUpdate?.Invoke(this, new FieldUpdateVerticesArgs(FieldEvents.RemoveVertices, args));
            return res;
        }
        public FieldUpdateEdgeArgs RemoveEdge(TVertex v, TVertex e, bool raise = true)
        {
            var res = Graph.RemoveEdge(v, e, out TWeight w);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                vec2 source = GetPos(v);
                vec2 stock = GetPos(e);
                var removeArgs = new FieldUpdateEdgeArgs(FieldEvents.RemoveEdge, source, stock, v.ToString(), e.ToString(), w?.ToString());
                if (raise)
                    FieldUpdate?.Invoke(this, removeArgs);
                return removeArgs;
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public void RemoveEdges(Tuple<TVertex, TVertex, TWeight>[] edges)
        {
            List<FieldUpdateEdgeArgs> args = new List<FieldUpdateEdgeArgs>();
            foreach (var edge in edges)
            {
                args.Add(RemoveEdge(edge.Item1, edge.Item2, false));
            }
            FieldUpdate?.Invoke(this, new FieldUpdateEdgesArgs(FieldEvents.RemoveEdges, args));
        }

        public bool HasAFreePlace(vec2 coord, float r)
        {
            foreach (var c in Positions)
            {
                if (Math.Pow(coord.x - c.Value.x, 2.0) + Math.Pow(coord.y - c.Value.y, 2.0) <= r * r)
                    return false;
            }
            return true;
        }

    }
}
