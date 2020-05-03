using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Algorithms
{

    public class ConnectedComponentsCommandArgs : ACommandArgs
    {
        public ConnectedComponentsCommandArgs()
            : base("ConnectedComponentsCommand")
        {

        }

        public string ConnectedTypeOut { get; set; }
        public string AlgorithmNameOut { get; set; }
        public List<List<string>> ComponentsOut { get; set; }
    }

    public class ConnectedComponentsCommand<TVertex, TWeight> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private ConnectedComponentsCommandArgs _args;
        public ConnectedComponentsCommand(IModelField field)
            : base(field)
        {

        }

        public ConnectedComponentsCommand(ConnectedComponentsCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new ConnectedComponentsCommand<TVertex, TWeight>(
                (ConnectedComponentsCommandArgs)args, Field);
        }

        public void Execute()
        {
            var G = ((ModelsField<TVertex, TWeight>)Field).Graph;
            SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
            G.AdjList.Keys.ToList().ForEach(el => visited[el] = false);
            List<List<TVertex>> components = new List<List<TVertex>>();
            if (!G.IsOrgraph)
            {
                int i = 0;
                G.AdjList.Keys.ToList().ForEach(k =>
                {
                    if (!visited[k])
                    {
                        components.Add(new List<TVertex>());
                        var cmp = components.Last();
                        if (i >= RGBcolor.Collection.Count) i = 0;
                        var color = RGBcolor.Collection[i++];
                        BFS(G, k, visited, cmp, color);
                        cmp.ForEach(el =>
                        {
                            G[el].ForEach(edge =>
                            {
                                if (cmp.Exists(e => e.Equals(edge.Vertex)))
                                {
                                    string key1 = ServiceFunctions.EdgeRepresentation(el.GetRepresentation(), edge.Vertex.GetRepresentation());
                                    Field.SetColor(key1, color, false);
                                }
                            });
                        });
                        Field.Refresh();
                    }
                });
                if (components.Count == 1)
                    _args.ConnectedTypeOut = "Связынй граф";
                else _args.ConnectedTypeOut = $"Граф содержит {components.Count} копанента(ов) связности";

                _args.AlgorithmNameOut = "Обход в ширину";
            }
            else
            {
                Stack<TVertex> oredrs = new Stack<TVertex>();
                var Gt = G.Transpose();
                G.AdjList.Keys.ToList().ForEach(k =>
                {
                    if (!visited[k])
                    DFS_G(G, k, visited, oredrs);
                });
                G.AdjList.Keys.ToList().ForEach(el => visited[el] = false);
                int i = 0;
                while (oredrs.Count != 0)
                {
                    var vert = oredrs.Pop();
                    if (!visited[vert])
                    {
                        components.Add(new List<TVertex>());
                        var cmp = components.Last();
                        if (i >= RGBcolor.Collection.Count) i = 0;
                        var color = RGBcolor.Collection[i++];
                        DFS_Gt(Gt, vert, visited, cmp, color);
                        cmp.ForEach(el => // оптимизировать
                        {
                            Gt[el].ForEach(edge =>
                            {
                                if (cmp.Exists(e => e.Equals(edge.Vertex)))
                                {
                                    string key = ServiceFunctions.EdgeRepresentation(edge.Vertex.GetRepresentation(), el.GetRepresentation());
                                    Field.SetColor(key, color, false);
                                }
                            });
                        });
                        Field.Refresh();
                    }
                }
                components.ForEach(conn =>
                {
                    if (i >= RGBcolor.Collection.Count) i = 0;
                    var color = RGBcolor.Collection[i++];
                });
                _args.AlgorithmNameOut = "Обход в глубину с использованием транспонированного Орграфа";
                if (components.Count == 1)
                    _args.ConnectedTypeOut = "Сильно-связный Орграф";
                else _args.ConnectedTypeOut = $"Орграф содержит {components.Count} сильно-связных компонента(ов)";
            }

            _args.ComponentsOut = new List<List<string>>();
            components.ForEach(conn =>
            {
                _args.ComponentsOut.Add(new List<string>());
                var current = _args.ComponentsOut.Last();
                conn.ForEach(vertex =>
                {
                    current.Add(vertex.GetRepresentation());
                });
            });

        }

        private void BFS(Graph<TVertex, TWeight> G, TVertex v, SortedDictionary<TVertex, bool> visited, List<TVertex> components, RGBcolor color)
        {
            visited[v] = true;
            components.Add(v);
            Field.SetColor(v.GetRepresentation(), color);
            Thread.Sleep(600);
            Queue<TVertex> Q = new Queue<TVertex>();
            Q.Enqueue(v);
            while (Q.Count != 0)
            {
                var x = Q.Dequeue();
                foreach (var w in G[x])
                {
                    if (!visited[w.Vertex])
                    {
                        visited[w.Vertex] = true;
                        Field.SetColor(w.Vertex.GetRepresentation(), color);
                        Thread.Sleep(600);
                        components.Add(w.Vertex);
                        Q.Enqueue(w.Vertex);
                    }
                }
            }
        }

        private void DFS_G(Graph<TVertex, TWeight> G, TVertex v, SortedDictionary<TVertex, bool> visited, Stack<TVertex> orders)
        {
            visited[v] = true;
            foreach (var adj in G[v])
            {
                if (!visited[adj.Vertex])
                {
                    DFS_G(G, adj.Vertex, visited, orders);
                }
            }
            orders.Push(v);
        }

        private void DFS_Gt(Graph<TVertex, TWeight> G, TVertex v, SortedDictionary<TVertex, bool> visited, List<TVertex> components, RGBcolor color)
        {
            visited[v] = true;
            Field.SetColor(v.GetRepresentation(), color);
            Thread.Sleep(600);
            components.Add(v);
            foreach (var adj in G[v])
            {
                if (!visited[adj.Vertex])
                {
                    Thread.Sleep(500);
                    DFS_Gt(G, adj.Vertex, visited, components, color);
                }
            }
        }
    }
}
