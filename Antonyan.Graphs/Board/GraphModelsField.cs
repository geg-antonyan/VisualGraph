﻿using System;
using System.Collections.Generic;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Util;
using System.Linq;

namespace Antonyan.Graphs.Board
{
    public enum FieldEvents { AddVertex, RemoveVertex, AddEdge, RemoveEdge, RemoveVertices, RemoveEdges, ChangeVertexPos, RemoveGraphModel };

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



    public interface IField
    {
        void SetGraphOptions(bool oriented, bool weighted);
        vec2 GetVertexModelPos(string representation);
        void ChangeVertexModelPos(string representation, vec2 newPos);
        void AddVertexModel(VertexModel vertexModel, bool raise = true);
        void AddEdgeModel(EdgeModel edgeModel, bool raise = true);
        void AddGraphModel(GraphModels model, bool raise = true);
        List<GraphModels> RemoveVertexModel(VertexModel vertexModel, bool raise = true);
        GraphModels RemoveEdgeModel(EdgeModel edgeModel, bool raise = true);
        List<GraphModels> RemoveGraphModel(GraphModels model, bool raise = true);
        List<GraphModels> RemoveGraphModels(List<GraphModels> models, bool raise = true);
    }


    public class GraphModelsField<TVertex, TWeight> : IField
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public event EventHandler<FieldUpdateArgs> FieldUpdate;

        public SortedDictionary<string, GraphModels> Models { get; private set; }
        public Graph<TVertex, TWeight> Graph { get; private set; }

        public GraphModelsField(UserInterface ui)
        {
            UI = ui;
            Models = new SortedDictionary<string, GraphModels>();
            FieldUpdate += UI.FieldUpdate;
        }


        public void SetGraphOptions(bool oriented, bool weighted)
        {
            Graph = new Graph<TVertex, TWeight>(oriented, weighted);
        }

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

        public void AddVertexModel(VertexModel vertexModel, bool raise = true)
        {
            var vertex = new TVertex();
            vertex.SetFromString(vertexModel.VertexStr);
            var res = Graph.AddVertex(vertex);
            if (res == ReturnValue.Succsess)
            {
                Models.Add(vertexModel.GetRepresentation(), vertexModel);
                if (raise)
                    FieldUpdate?.Invoke(this, new FieldUpdateArgs(new List<RepresentationFieldEventPair>
                                       { new RepresentationFieldEventPair(vertexModel.GetRepresentation(), FieldEvents.AddVertex) }, Models));
            }
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
                Models.Add(edge.GetRepresentation(), edge);
                FieldUpdate?.Invoke(this,
                    new FieldUpdateArgs(new List<RepresentationFieldEventPair> 
                        { new RepresentationFieldEventPair(edge.GetRepresentation(), FieldEvents.AddEdge) }, Models));
            }
        }

        public void AddEdgeModel(EdgeModel edgeModel, bool raise = true)
        {

            var weight = new TWeight();
            if (edgeModel.Weight != null)
                weight.SetFromString(edgeModel.Weight);
            var source = new TVertex();
            var stock = new TVertex();
            source.SetFromString(edgeModel.Source.VertexStr);
            stock.SetFromString(edgeModel.Stock.VertexStr);
            var res = Graph.AddEdge(source, stock, weight);
            if (res != ReturnValue.Succsess)
                throw new Exception(res.ToString());
            else
            {
                Models.Add(edgeModel.GetRepresentation(), edgeModel);
                FieldUpdate?.Invoke(this,
                    new FieldUpdateArgs(new List<RepresentationFieldEventPair>
                        { new RepresentationFieldEventPair(edgeModel.GetRepresentation(), FieldEvents.AddEdge) }, Models));
            }

        }

        public void AddGraphModel(GraphModels model, bool raise = true)
        {
            var vertexModel = model as VertexModel;
            if (vertexModel != null)
                AddVertexModel(vertexModel, raise);
            else
                AddEdgeModel((EdgeModel)model, raise);
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

        public List<GraphModels> RemoveVertexModel(VertexModel vertexModel, bool raise = true)
        {
            var vertex = new TVertex();
            vertex.SetFromString(vertexModel.VertexStr);
            var retVal = Graph.RemoveVertex(vertex);
            List<GraphModels> removeModels = new List<GraphModels>();
            if (retVal.Item1 == ReturnValue.Succsess)
            {
                string vertRepresent = vertexModel.GetRepresentation();
                GraphModels vertM = Models[vertRepresent];
               // removeModels.Add(vertM);
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
            string represent = ServiceFunctions.EdgeRepresentation(v.ToString(), e.ToString(), w?.ToString());
            string reverseRepresent = ServiceFunctions.EdgeRepresentation(e.ToString(), v.ToString(), w?.ToString());
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

        public GraphModels RemoveEdgeModel(EdgeModel edgeModel, bool raise = true)
        {
            TWeight weight = null;
            if (edgeModel.Weight != null)
            {
                weight = new TWeight();
                weight.SetFromString(edgeModel.Weight);
            }
            var source = new TVertex();
            var stock = new TVertex();
            source.SetFromString(edgeModel.Source.VertexStr);
            stock.SetFromString(edgeModel.Stock.VertexStr);
            var res = Graph.RemoveEdge(source, stock, out TWeight w);
            string represent = edgeModel.GetRepresentation();
            //string reverseRepresent = edgeModel.GetReverseRepresentation();
            if (res == ReturnValue.Succsess)
            {
                Models.Remove(represent);
                if (raise)
                    FieldUpdate?.Invoke(this, new FieldUpdateArgs(new List<RepresentationFieldEventPair>()
                                                                        { new RepresentationFieldEventPair(represent, FieldEvents.RemoveEdge) }, Models));
                return edgeModel;
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public List<GraphModels> RemoveGraphModel(GraphModels model, bool raise = true)
        {
            var vertexModel = model as VertexModel;
            if (vertexModel != null)
                return RemoveVertexModel(vertexModel, raise);
            else
            {
                return new List<GraphModels>() { RemoveEdgeModel((EdgeModel)model, raise) };
            }
        }


        public List<GraphModels> RemoveGraphModels(List<GraphModels> models, bool raise = true)
        {
            List<GraphModels> res = new List<GraphModels>();
            List<VertexModel> vertices = new List<VertexModel>();
            List<RepresentationFieldEventPair> raiseArgs = new List<RepresentationFieldEventPair>();
            foreach (var m in models)
            {
                var list = RemoveGraphModel(m, false);
                list.ForEach(elem => raiseArgs.Add(new 
                    RepresentationFieldEventPair(elem.GetRepresentation(), FieldEvents.RemoveGraphModel)));
                res.AddRange(list);
            }
            if (raise)
                FieldUpdate?.Invoke(this, new FieldUpdateArgs(raiseArgs, Models));
            return res;
        }

    }
}
