using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using System.IO;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board
{
    public enum FieldEvents
    {
        InitGraph, RemoveGraph, AddModel, RemoveModel,
        RemoveModels,
        SaveGrapToFile,
        UpdateStoredGraphs
    }
    public class ModelFieldUpdateArgs : EventArgs
    {
        public ModelFieldUpdateArgs(FieldEvents e) => Event = e;
        public FieldEvents Event { get; private set; }
    }
    public class ModelsField<TVertex> : IModelField
        where TVertex : AVertex, new()
    {
        public event EventHandler<ModelFieldUpdateArgs> FieldUpdate;

        private SortedDictionary<string, GraphModel> _models;
        private Dictionary<string, Graph<TVertex>> _storedGraphs;
        public Graph<TVertex> Graph { get; private set; }

        public bool IsOrgraph { get { return Graph.IsOrgraph; } }
        public bool IsWeighted { get { return Graph.IsWeighted; } }

        public int MarkedModelsCount => _models.Values.ToList().Count(m => m.Marked);

        public int MarkedVertexModelCount => _models.Values.ToList().Count(m => m.Marked && m is AVertexModel);

        public int MarkedEdgeModelCount => _models.Values.ToList().Count(m => m.Marked && m is AEdgeModel);

        public List<GraphModel> Models => _models.Values.ToList();

        public bool Status { get; private set; } = false;

        public UserInterface UserInterface { get; private set; }

        public ModelsField(UserInterface ui)
        {
            _storedGraphs = new Dictionary<string, Graph<TVertex>>();
            UserInterface = ui;
            _models = new SortedDictionary<string, GraphModel>();
            FieldUpdate += ui.FieldUpdate;
        }

        public void CreateGraph(bool oriented, bool weighted, bool raise = true)
        {
            Graph = new Graph<TVertex>(oriented, weighted);
            Status = true;
            _models = new SortedDictionary<string, GraphModel>();
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.InitGraph));
        }


        public GraphModel this[string key]
        {
            get
            {
                if (_models.ContainsKey(key))
                    return _models[key];
                return null;
            }
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
            var weight = 0;
            if (edgeModel.Weight != null && edgeModel.Weight != "")
                weight = int.Parse(edgeModel.Weight);
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
            int weight = 0;
            if (edgeModel.Weight != null)
            {
                weight = int.Parse(edgeModel.Weight);
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
            Status = false;
            _models = null;
            Graph = null;
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.RemoveGraph));
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

        public void RefreshDefault(bool removeAddMark = true, bool raise = true)
        {
            UnmarkGraphModels(false);
            _models.Values.ToList().ForEach(m => 
            { 
                m.Color = GraphModel.DefaultColor;
                m.Width = GraphModel.DefaultWidth;
                var e = m as AEdgeModel;
                if (e != null && removeAddMark)
                    e.AddMark = null;
            });
            if (raise)
                FieldUpdate?.Invoke(null, null);
        }

        public string GetVertexPosKey(vec2 pos, float r)
        {
            foreach (var m in _models.Values)
                if (m is AVertexModel && (m.PosKey(pos, r) != null))
                    return m.Key;
            return null;
        }

        public void SaveGraphToFile(Stream stream, bool raise = true)
        {
            using (StreamWriter sw = new StreamWriter(stream, Encoding.UTF8))
            {
                string oriented = IsOrgraph ? "orgraph" : "graph";
                string weighted = IsWeighted ? "weighted" : "nonweighted";
                sw.WriteLine($"graphmodel {oriented} {weighted}");
                _models.Values.ToList().ForEach(m =>
                {
                    var e = m as AEdgeModel;
                    if (e == null)
                    {
                        var v = m as AVertexModel;
                        sw.WriteLine($"[v] {v.Key} {v.Pos.x} {v.Pos.y}");
                    }
                    else
                    {
                        sw.WriteLine($"[e] {e.Source.VertexStr} {e.Stock.VertexStr} {e.Weight}");
                    }
                });
                sw.WriteLine("graphmodel end");
                Graph.SaveGraphToFile(sw);
            }
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.SaveGrapToFile));
        }

        public void OpenGraphInFile(List<GraphModel> models, string graphDataText, bool raise = true)
        {
            bool res = true;
            Status = false;
            if (Graph != null)
            {
                res = UserInterface.AnswerTheQuestion("Граф уже создан!!! Вы действительно хотите открыть граф из файла? Текущий граф будет безвозвратно утерен.");
            }
            if (res)
            {
                _models = new SortedDictionary<string, GraphModel>();
                models.ForEach(m => _models.Add(m.Key, m));
                Graph = new Graph<TVertex>(graphDataText);
                Status = true;
            }
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.InitGraph));
        }

        public int GetHalfLifeDegree(AVertexModel vertex)
        {
            TVertex v = new TVertex();
            v.SetFromString(vertex.VertexStr);
            return Graph.GetHalfLifeDegree(v);
        }

        public string GetAdjListToString()
        {
            return Graph.GetAdjListToString();
        }

        public void AddCurrentGraphInStoredGraphs(string name, bool raise = true)
        {
            if (Graph == null) throw new Exception("Текущий граф еще не создан");
            _storedGraphs.Add(name, new Graph<TVertex>(Graph));
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.UpdateStoredGraphs));
        }

        public List<string> GetStoredGraphsName()
        {
            return _storedGraphs.Keys.ToList();
        }

        public void UnionGraphs(string[] graphsNames, string newGrapName, bool raise = true)
        {
            if (graphsNames.Length < 2)
                throw new Exception("количество графов для соединении должно быть не меньше двух");
            if (!_storedGraphs.ContainsKey(graphsNames[0]) || !_storedGraphs.ContainsKey(graphsNames[1]))
            {
                throw new Exception("Некорректное имя/имена грфа");
            }
            var res = _storedGraphs[graphsNames[0]].Union(_storedGraphs[graphsNames[1]]);
            for (int i = 2; i < graphsNames.Length; i++)
            {
                if (!_storedGraphs.ContainsKey(graphsNames[i]))
                    throw new Exception("Некорректное имя/имена грфа");
                res = res.Union(_storedGraphs[graphsNames[i]]);
            }
            _storedGraphs.Add(newGrapName, res);
            if (raise)
                FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.UpdateStoredGraphs));
        }

        public void RemoveStoredGraph(string name, bool raise = true)
        {
            if (_storedGraphs.ContainsKey(name))
            {
                _storedGraphs.Remove(name);
                if (raise)
                    FieldUpdate?.Invoke(this, new ModelFieldUpdateArgs(FieldEvents.UpdateStoredGraphs));
            }
        }

        public string GetStoredGraphAdjList(string name)
        {
            if (_storedGraphs.ContainsKey(name))
            {
                return _storedGraphs[name].GetAdjListToString();
            }
            else return null;
        }

        public bool SetColor(string key, RGBcolor color, bool raise = true)
        {
            GraphModel m;
            if (_models.ContainsKey(key) && !(m = _models[key]).Marked)
            {
                m.Color = color;
                if (raise)
                    FieldUpdate?.Invoke(null, null);
                return true;
            }
            return false;
        }

        public void Refresh()
        {
            FieldUpdate?.Invoke(null, null);
        }

        public bool SetWidth(string key, int width, bool raise = true)
        {
            GraphModel m;
            if (_models.ContainsKey(key) && !(m = _models[key]).Marked)
            {
                m.Width = width;
                if (raise)
                    FieldUpdate?.Invoke(null, null);
                return true;
            }
            return false;
        }

        public bool SetColorAndWidth(string key, RGBcolor color, int width, bool raise = true)
        {
            GraphModel m;
            if (_models.ContainsKey(key) && !(m = _models[key]).Marked)
            {
                m.Color = color;
                m.Width = width;
                if (raise)
                    FieldUpdate?.Invoke(null, null);
                return true;
            }
            return false;
        }

        public bool SetModelDefaultOptions(string key, bool raise = true)
        {
            GraphModel m;
            if (_models.ContainsKey(key) && !(m = _models[key]).Marked)
            {
                m.Color = GraphModel.DefaultColor;
                m.Width = GraphModel.DefaultWidth;
                if (raise)
                    FieldUpdate?.Invoke(null, null);
                return true;
            }
            return false;
        }

        public bool SetWeightMark(string key, string mark, bool raise = true)
        {
            AEdgeModel edge;
            if (_models.ContainsKey(key) && (edge = _models[key] as AEdgeModel) != null)
            {
                edge.AddMark = mark;
                if (raise)
                    FieldUpdate?.Invoke(null, null);
                return true;
            }
            return false;
        }
    }
}
