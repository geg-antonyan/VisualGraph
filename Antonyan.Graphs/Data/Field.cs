﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Data
{
    public enum FieldEvents { AddVertex, RemoveVertex, AddEdge, RemoveEdge, RemoveVertices, RemoveEdges };
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

    public class FieldUpdateVertexArgs : FieldUpdateArgs
    {
        public FieldUpdateVertexArgs(FieldEvents e, string vertex, vec2 coord)
            : base(e)
        {
            Pos = coord;
            Vertex = vertex;
            hashCode = Vertex.GetHashCode();
        }
        public vec2 Pos { get; private set; }
        public string Vertex { get; private set; }

    }

    public class FieldUpdateVerticesArgs : FieldUpdateArgs
    {
        public FieldUpdateVerticesArgs(FieldEvents e, Tuple<string, vec2>[] vertices)
            : base(e)
        {
            Vertices = vertices;
        }
        public Tuple<string, vec2>[] Vertices { get; private set; }
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
    }

    public class FieldUpdateEdgesArgs : FieldUpdateArgs
    {
        public FieldUpdateEdgesArgs(FieldEvents e, Tuple<vec2, vec2, string, string, string>[] edges)
            : base(e)
        {
            Edges = edges;
        }

        public Tuple<vec2, vec2, string, string, string>[] Edges { get; private set; }
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

        public void RemoveVertex(TVertex v, bool raise = true)
        {
            var res = Graph.RemoveVertex(v);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                vec2 c;
                Positions.TryGetValue(v, out c);
                Positions.Remove(v);
                if (raise)
                    FieldUpdate?.Invoke(this, new FieldUpdateVertexArgs(FieldEvents.RemoveVertex, v.ToString(), c));
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public void RemoveVertices(Tuple<TVertex, vec2>[] vertices)
        {
            Tuple<string, vec2>[] args = new Tuple<string, vec2>[vertices.Length];
            int i = 0;
            foreach (var v in vertices)
            {
                RemoveVertex(v.Item1, false);
                args[i++] = new Tuple<string, vec2>(v.Item1.ToString(), v.Item2);
            }

            FieldUpdate?.Invoke(this, new FieldUpdateVerticesArgs(FieldEvents.RemoveVertices, args));
        }
        public void RemoveEdge(TVertex v, TVertex e, bool raise = true)
        {
            var res = Graph.RemoveEdge(v, e, out TWeight w);
            if (res == Graph<TVertex, TWeight>.ReturnValue.Succsess)
            {
                vec2 source = GetPos(v);
                vec2 stock = GetPos(e);
                if (raise)
                    FieldUpdate?.Invoke(this, new FieldUpdateEdgeArgs(FieldEvents.RemoveEdge, source, stock, v.ToString(), e.ToString(), w?.ToString()));
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public void RemoveEdges(Tuple<TVertex, TVertex, TWeight>[] edges)
        {
            Tuple<vec2, vec2, string, string, string>[] args = new Tuple<vec2, vec2, string, string, string>[edges.Length];
            int i = 0;
            foreach (var edge in edges)
            {
                RemoveEdge(edge.Item1, edge.Item2, false);
                vec2 source = GetPos(edge.Item1);
                vec2 stock = GetPos(edge.Item2);
                args[i++] = new Tuple<vec2, vec2, string, string, string>(
                    source, stock, edge.Item1.ToString(), edge.Item2.ToString(), edge.Item3.ToString());
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
