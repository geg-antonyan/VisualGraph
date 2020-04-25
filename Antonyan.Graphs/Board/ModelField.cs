using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Board
{
    public enum FieldEvents { InitGraph, RemoveGraph, AddModel, RemoveModel,
        RemoveModels
    }
    public class ModelFieldUpdateArgs : EventArgs
    {
        public ModelFieldUpdateArgs(FieldEvents e) => Event = e;
        public FieldEvents Event { get; private set; }
    }
    public class ModelsField<TVertex, TWeight> : IModelField
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()  
    {
        public event EventHandler<ModelFieldUpdateArgs> FieldUpdate;
        private readonly SortedDictionary<string, GraphModel> _models;
        private bool _status = false;
        private UserInterface _ui;
        public Graph<TVertex, TWeight> Graph { get; private set; }
        
        public bool IsOrgraph { get { return Graph.IsOrgraph; } }
        public bool IsWeighted { get { return Graph.IsWeighted; } }

        public int MarkedModelsCount => _models.Values.ToList().Count(m => m.Marked);

        public int MarkedVertexModelCount => _models.Values.ToList().Count(m => m.Marked && m is AVertexModel);

        public int MarkedEdgeModelCount => _models.Values.ToList().Count(m => m.Marked && m is AEdgeModel);

        public List<GraphModel> Models => _models.Values.ToList();

        public bool Status => _status;

        public UserInterface UserInterface => _ui;

        public GraphModel this[string key]
        {
            get
            {
                if (_models.ContainsKey(key))
                    return _models[key];
                return null;
            } 
        }

        public ModelsField(UserInterface ui)
        {
            _ui = ui;
            _models = new SortedDictionary<string, GraphModel>();
            FieldUpdate += _ui.FieldUpdate;
            Graph = new Graph<TVertex, TWeight>();
        }

        public void AddVertexModel(AVertexModel vertexModel, bool raise = true)
        {
            var vertex = new TVertex();
            vertex.SetFromString(vertexModel.VertexStr);
            var res = Graph.AddVertex(vertex);
            if (res == ReturnValue.Succsess)
            {
                _models.Add(vertexModel.Key, vertexModel);
                if (raise)
                    FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.AddModel));
            }
        }

        public void AddEdgeModel(AEdgeModel edgeModel, bool raise = true)
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
                _models.Add(edgeModel.Key, edgeModel);
                if (raise)
                    FieldUpdate?.Invoke(this, null);
            }
        }

        public void AddGraphModel(GraphModel model, bool raise = true)
        {
            var vertexModel = model as AVertexModel;
            if (vertexModel != null)
                AddVertexModel(vertexModel, raise);
            else
                AddEdgeModel((AEdgeModel)model, raise);
        }

        public List<GraphModel> RemoveVertexModel(AVertexModel vertexModel, bool raise = true)
        {
            var vertex = new TVertex();
            vertex.SetFromString(vertexModel.VertexStr);
            var retVal = Graph.RemoveVertex(vertex);
            List<GraphModel> removeModels = new List<GraphModel>();
            if (retVal == ReturnValue.Succsess)
            {
                removeModels.Add(vertexModel);
                _models.Remove(vertexModel.Key);
                _models.Values.ToList().ForEach(m =>
                {
                    var edge = m as AEdgeModel;
                    if (edge != null &&
                       (edge.Source.Key == vertexModel.Key ||
                        edge.Stock.Key == vertexModel.Key))
                    {
                        removeModels.Add(edge);
                        _models.Remove(edge.Key);
                    }
                });
                if (raise)
                    FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.RemoveModel));
                return removeModels;
            }
            else
            {
                throw new Exception(retVal.ToString());
            }
        }

        public GraphModel RemoveEdgeModel(AEdgeModel edgeModel, bool raise = true)
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
            var res = Graph.RemoveEdge(source, stock);
            string key = edgeModel.Key;
            if (res == ReturnValue.Succsess)
            {
                _models.Remove(key);
                if (raise)
                    FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.RemoveModel));
                return edgeModel;
            }
            else
            {
                throw new Exception(res.ToString());
            }
        }

        public List<GraphModel> RemoveGraphModel(GraphModel model, bool raise = true)
        {
            var vertexModel = model as AVertexModel;
            if (vertexModel != null)
                return RemoveVertexModel(vertexModel, raise);
            else
            {
                return new List<GraphModel>() { RemoveEdgeModel((AEdgeModel)model, raise) };
            }
        }

        public List<GraphModel> RemoveGraphModels(List<GraphModel> models, bool raise = true)
        {
            List<AVertexModel> vertices = new List<AVertexModel>();
            List<AEdgeModel> edges = new List<AEdgeModel>();
            models.ForEach(m =>
            {
                var v = m as AVertexModel;
                if (v != null)
                    vertices.Add(v);
                else edges.Add((AEdgeModel)m);
            });
            List<GraphModel> res = new List<GraphModel>();
            res.AddRange(edges.Select(e => RemoveEdgeModel(e, false)));
            res.AddRange(vertices.SelectMany(v => RemoveVertexModel(v, false)));
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.RemoveModels));
            return res;
        }

        public void Clear(bool raise = true)
        {
            _status = false;
            _models.Clear();
            Graph.AdjList.Clear();
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.RemoveGraph));
        }

        public void SetGraphOptions(bool oriented, bool weighted, bool raise = true)
        {
            _status = true;
            Graph.SetOptions(oriented, weighted);
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.InitGraph));
        }

        public void MoveVertexModel(string key, vec2 newPos, bool raise = true)
        {

            GraphModel model;
            if (_models.ContainsKey(key) && (model = _models[key]) is AVertexModel)
            {
                ((AVertexModel)model).UpdatePos(newPos);
                _models.Values.ToList().ForEach(m =>
               {
                   AEdgeModel edge = m as AEdgeModel;
                   if (edge != null && (edge.Source.Key == key || edge.Stock.Key == key))
                       edge.RefreshPos();
               });
                if (raise)
                    FieldUpdate?.Invoke(this, null);
            }
        }

        public bool MarkGraphModel(string key, bool raise = true)
        {
            GraphModel m;
            if (_models.ContainsKey(key) && !(m = _models[key]).Marked)
            {
                m.Marked = true;
                if (raise)
                    FieldUpdate?.Invoke(null, null);
                return true;
            }
            return false;
        }

        public void UnmarkGraphModels(bool raise = true)
        {
            _models.Values.ToList().ForEach(m => m.Marked = false);
            if (raise)
                FieldUpdate?.Invoke(null, null);
        }

        public string GetPosKey(vec2 pos, float r)
        {
            foreach (var m in _models)
                if (m.Value.PosKey(pos, r) != null)
                    return m.Value.Key;
           return null;
        }

        public vec2 GetVertexModelPos(string key)
        {
            vec2 res = null;
            GraphModel model;
            if (_models.ContainsKey(key) && (model = _models[key]) is AVertexModel)
                res = ((AVertexModel)model).Pos;
            return res;
        }

        

        public List<GraphModel> GetMarkedModels()
        {
            return _models.Values.ToList()
                .Where(m => m.Marked)
                .ToList();
        }

        public void Refresh()
        {
            FieldUpdate?.Invoke(null, null);
        }

        public string GetVertexPosKey(vec2 pos, float r)
        {
            foreach (var m in _models.Values)
                if (m is AVertexModel && (m.PosKey(pos, r) != null))
                    return m.Key;
            return null;
        }
    }
}
