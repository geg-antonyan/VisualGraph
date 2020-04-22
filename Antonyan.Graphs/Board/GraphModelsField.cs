using System;
using System.Collections.Generic;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board
{
    public enum FieldEvents { AddVertex, RemoveVertex, AddEdge, RemoveEdge, RemoveVertices, RemoveEdges, ChangeVertexPos };

    public class RepresentationFieldEventPair
    {
        public RepresentationFieldEventPair(string representation, FieldEvents e)
        {
            Representation = representation;
            Event = e;
        }
        
        public string Representation { get; set; }
        public FieldEvents Event { get; set; }
    }
    public class FieldUpdateArgs : EventArgs
    {
        public List<RepresentationFieldEventPair> Pairs { get; private set; }
        public FieldUpdateArgs(List<RepresentationFieldEventPair> modelEventPairs, SortedDictionary<string, GraphModels> models)
        {
            Pairs = modelEventPairs;
            Models = models;
        }

        public SortedDictionary<string, GraphModels> Models { get; private set; }
    }




    public class GraphModelsField<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public event EventHandler<FieldUpdateArgs> FieldUpdate;

        public SortedDictionary<string, GraphModels> Models { get; private set; }
        public Graph<TVertex, TWeight> Graph { get; private set; }

        public GraphModelsField(bool oriented, bool weighted, UserInterface ui)
        {
            UI = ui;
            Models = new SortedDictionary<string, GraphModels>();
            Graph = new Graph<TVertex, TWeight>(oriented, weighted);
            FieldUpdate += UI.FieldUpdate;
        }
        public UserInterface UI { get; private set; }
        public bool IsOrgarph { get { return Graph.IsOrgraph; } }
        public bool IsWeighted { get { return Graph.IsWeighted; } }

        public bool HasAFreePlace(vec2 pos, float r)
        {
            foreach (var m in Models)
            {
                var v = m.Value as VertexModel;
                if (v != null && (Math.Pow(pos.x - v.Pos.x, 2.0) + Math.Pow(pos.y - v.Pos.y, 2.0) <= r * r))
                        return false;
            }
            return true;
        }

        public vec2 GetVertexModelPos(string representation)
        {
            vec2 res = null;
            GraphModels model;
            if (Models.ContainsKey(representation) && (model = Models[representation]) is VertexModel)
                res = ((VertexModel)model).Pos;
            return res;
        }

        public void ChangeVertexModelPos(string representation, vec2 newPos)
        {
            GraphModels model;
            if (Models.ContainsKey(representation) && (model = Models[representation]) is VertexModel)
            {
                ((VertexModel)model).SetPos(newPos);
                FieldUpdate?.Invoke(this, new 
                    FieldUpdateArgs(new List<RepresentationFieldEventPair> 
                                       { new RepresentationFieldEventPair(representation, FieldEvents.ChangeVertexPos) }, Models));
            }
        }
        public void AddVertexModel(TVertex v, vec2 pos)
        {
            var res = Graph.AddVertex(v);
            if (res == ReturnValue.Succsess)
            {
                var model = new VertexModel(v.ToString(), pos);
                Models.Add(model.GetRepresentation(), model);
                FieldUpdate?.Invoke(this, new FieldUpdateArgs(new List<RepresentationFieldEventPair>
                                       { new RepresentationFieldEventPair(model.GetRepresentation(), FieldEvents.AddVertex) }, Models));
            }
            else
                throw new Exception(res.ToString());
        }
        public void AddEdgeModel(TVertex v, TVertex e, TWeight w = null)
        {
            var res = Graph.AddEdge(v, e, w);
            if (res != ReturnValue.Succsess)
                throw new Exception(res.ToString());
            else
            {
                var sourceVertexModel = (VertexModel)Models[v.GetRepresentation()];
                var stockVertexModel = (VertexModel)Models[e.GetRepresentation()];
                EdgeModel edge = new EdgeModel(sourceVertexModel, stockVertexModel, w.ToString(), Graph.IsOrgraph);
                FieldUpdate?.Invoke(this,
                    new FieldUpdateArgs(new List<RepresentationFieldEventPair> 
                        { new RepresentationFieldEventPair(edge.GetRepresentation(), FieldEvents.AddEdge) }, Models));
            }
        }

        public List<GraphModels> RemoveVertexModel(TVertex v, bool raise = true)
        {

            var retVal = Graph.RemoveVertex(v);
            List<GraphModels> removeModels = new List<GraphModels>();
            if (retVal.Item1 == ReturnValue.Succsess)
            {
                string vertRepresent = v.GetRepresentation();
                GraphModels vertM = Models[vertRepresent];
                removeModels.Add(vertM);
                Models.Remove(vertRepresent);
                var raiseArgs = new List<RepresentationFieldEventPair>() { new RepresentationFieldEventPair(vertRepresent, FieldEvents.RemoveVertex) };
                foreach (var represent in retVal.Item2)
                {
                    if (Models.ContainsKey(represent)) // при неориентированном графе удаляются два ребра, но в моделе один
                    {
                        GraphModels edge = Models[represent];
                        removeModels.Add(edge);
                        raiseArgs.Add(new RepresentationFieldEventPair(represent, FieldEvents.RemoveEdge));
                        Models.Remove(represent);
                    }
                }
                if (raise)
                    FieldUpdate?.Invoke(this, new FieldUpdateArgs(raiseArgs, Models));
                return removeModels;
            }
            else
            {
                throw new Exception(retVal.Item1.ToString());
            }
        }

        public GraphModels RemoveEdgeModel(TVertex v, TVertex e, bool raise = true)
        {
            var res = Graph.RemoveEdge(v, e, out TWeight w);
            string represent = Representations.EdgeRepresentation(v.ToString(), e.ToString(), w?.ToString());
            string reverseRepresent = Representations.EdgeRepresentation(e.ToString(), v.ToString(), w?.ToString());
            if (res == ReturnValue.Succsess)
            {
                var removeModel = Models.ContainsKey(represent) ? Models[represent] : Models[reverseRepresent];
                Models.Remove(removeModel.GetRepresentation());
                if (raise)
                    FieldUpdate?.Invoke(this,  new FieldUpdateArgs(new List<RepresentationFieldEventPair>() 
                                                                        { new RepresentationFieldEventPair(removeModel.GetRepresentation(), FieldEvents.RemoveEdge) }, Models));
                return removeModel;
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }



    }
}
