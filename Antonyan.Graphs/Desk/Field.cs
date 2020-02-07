using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Desk.Geometry;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Desk
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

    class Field<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public static event EventHandler<FieldVertexEventArgs<TVertex>> VertexUpdate;
        public static event EventHandler<FieldEdgeEventArgs<TVertex, TWeight>> EdgeUpdate;
        
        public static Field<TVertex, TWeight> instance;
        public static Field<TVertex, TWeight> Instance
        {
            get
            {
                if (instance == null)
                {
                    Graph = new Graph<TVertex, TWeight>(IsOrgarph == null ? false : IsOrgarph.Value, IsWeight == null ? false : IsWeight.Value);
                    instance = new Field<TVertex, TWeight>();
                    return instance;
                }
                return instance;
            }
        }

        public static UserInterface UI { get; private set; }
        public static bool? IsOrgarph { get { return IsOrgarph; } set { if (IsOrgarph == null) IsOrgarph = value; } }
        public static bool? IsWeight { get { return IsWeight; } set { if (IsWeight == null) IsWeight = value; } }
        
        public static void AddObserved(UserInterface ui)
        {
            UI = ui;
            VertexUpdate += UI.FieldUpdate;
            EdgeUpdate += UI.FieldUpdate;
        }
        public static SortedDictionary<TVertex, Vec2> Coords { get; private set; }
        public static Graph<TVertex, TWeight> Graph { get; private set; }
       
        public static void AddVertexAndCoord(TVertex v, Vec2 coord)
        {
            var res = Graph.AddVertex(v);
            if (res == Graph<TVertex, TWeight>.RetrunValue.Succsess)
            {
                Coords.Add(v, coord);
                VertexUpdate?.Invoke(Instance, new FieldVertexEventArgs<TVertex>(FieldEvents.AddVertex, v, coord));
            }
            else
                throw new Exception(res.ToString());
        }
        public static void AddEdge(TVertex v, TVertex e, TWeight w)
        {
            var res = Graph.AddEdge(v, e, w);
            if (res != Graph<TVertex, TWeight>.RetrunValue.Succsess)
                throw new Exception(res.ToString());
            else
                EdgeUpdate?.Invoke(Instance, new FieldEdgeEventArgs<TVertex, TWeight>(FieldEvents.AddEdge, v, e, w));
        }
        public static void RemoveVertex(TVertex v)
        {
            var res = Graph.RemoveVertex(v);
            if (res == Graph<TVertex, TWeight>.RetrunValue.Succsess)
            {
                Vec2 c;
                Coords.TryGetValue(v, out c);
                Coords.Remove(v);
                VertexUpdate?.Invoke(Instance, new FieldVertexEventArgs<TVertex>(FieldEvents.RemoveVertex, v, c));
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public static void RemoveEdge(TVertex v, TVertex e)
        {
            var res = Graph.RemoveEdge(v, e);
            if (res == Graph<TVertex, TWeight>.RetrunValue.Succsess)
            {

            }
        }
    }
}
