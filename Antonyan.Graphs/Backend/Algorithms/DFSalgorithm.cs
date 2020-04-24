using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using Antonyan.Graphs.Board;
using System.Threading;
using Antonyan.Graphs.Board.Models;

namespace Antonyan.Graphs.Backend.Algorithms
{

    //public class UIDFSargs : UIAlgorithmArgs
    //{
    //    public UIDFSargs(AVertexModel model)
    //        : base("DFS")
    //    {
    //        VertexModel = model;
    //    }

    //    public AVertexModel VertexModel { get; private set; }
    //}

    //public  class DFSalgorithm<TVertex, TWeight> : Algorithm
    //    where TVertex : AVertex, new()
    //    where TWeight : AWeight, new()
    //{
    //    private readonly Graph<TVertex, TWeight> _graph;
    //    private readonly UserInterface _ui;

    //    public DFSalgorithm(Graph<TVertex, TWeight> graph, UserInterface ui)
    //    {
    //        _graph = graph;
    //        _ui = ui;
    //    }
    //    public void Visit(TVertex v, UserInterface ui)
    //    {
    //        Thread.Sleep(700);
    //        ui.MarkModel(v.ToString());
            
    //    }
    //    public void DFS(TVertex v, SortedDictionary<TVertex, bool> visited)
    //    {
    //        Visit(v, _ui);
    //        visited[v] = true;
    //        foreach (var adj in _graph[v])
    //        {
    //            if (!visited[adj.Item1])
    //            {
    //                Thread.Sleep(500);
    //                if (!_ui.MarkModel(ServiceFunctions.EdgeRepresentation(v.ToString(), adj.Item1.ToString(), adj.Item2?.ToString())) && !_graph.IsOrgraph)
    //                    _ui.MarkModel(ServiceFunctions.EdgeRepresentation(adj.Item1.ToString(), v.ToString(), adj.Item2?.ToString()));

    //                DFS(adj.Item1, visited);
    //            }
    //        }
    //    }
      
    //    public override RepositoryItem Clone(RepositoryArgs args)
    //    {
    //        SetArgs((UIAlgorithmArgs)args);
    //        return this;
    //    }

    //    public override void Execute()
    //    {
    //        TVertex vertex = new TVertex();
    //        vertex.SetFromString(((UIDFSargs)_args).VertexModel.VertexStr);
    //        SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
    //        foreach (var v in _graph.AdjList)
    //            visited[v.Key] = false;
    //        DFS(vertex, visited);
    //    }
    //}
}

/*
   public void BFS(Graph<TVertex, TWeight> G, TVertex v, SortedDictionary<TVertex, bool> visited,
            UserInterface ui)
        {
            Visit(v, ui, null);
            visited[v] = true;
            Queue<TVertex> Q = new Queue<TVertex>();
            Q.Enqueue(v);
            while (Q.Count != 0)
            {
                var x = Q.Dequeue();
                foreach (var w in G[x])
                {
                    if (!visited[w.Item1])
                    {
                        Thread.Sleep(500);
                        if (!ui.MarkModel(ServiceFunctions.EdgeRepresentation(x.ToString(), w.Item1.ToString(), w.Item2?.ToString())))
                            ui.MarkModel(ServiceFunctions.EdgeRepresentation(w.Item1.ToString(), x.ToString(), w.Item2?.ToString()));
                        Visit(w.Item1, ui);
                        visited[w.Item1] = true;
                        Q.Enqueue(w.Item1);
                    }
                }
            }
        }
     */
