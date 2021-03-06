﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using Antonyan.Graphs.Board;
using System.Threading;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend.Commands;

namespace Antonyan.Graphs.Backend.Algorithms
{

    public class DFScommandArgs : AlgorithmCommandArgs
    {
        public DFScommandArgs(AVertexModel model)
            : base("DFSalgorithm")
        {
            VertexModel = model;
        }

        public AVertexModel VertexModel { get; private set; }
    }

    public class DFSalgorithmCommand<TVertex> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
    {
        private readonly DFScommandArgs _args;
        public DFSalgorithmCommand(IModelField field)
            : base(field)
        { }
        public DFSalgorithmCommand(DFScommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }

        public ICommand Clone(ACommandArgs args)
        {
            return new DFSalgorithmCommand<TVertex>((DFScommandArgs)args, Field);
        }

        public void Execute()
        {
            var graph = ((ModelsField<TVertex>)Field).Graph;
            TVertex vertex = new TVertex();
            vertex.SetFromString(_args.VertexModel.VertexStr);
            SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
            foreach (var v in graph.AdjList)
                visited[v.Key] = false;
            DFS(graph, vertex, visited);

        }
        private void Visit(TVertex v)
        {
            Thread.Sleep(700);
            Field.MarkGraphModel(v.GetRepresentation());


        }
        private void DFS(Graph<TVertex> G,  TVertex v, SortedDictionary<TVertex, bool> visited)
        {
            Visit(v);
            visited[v] = true;
            foreach (var adj in  G[v])
            {
                if (!visited[adj.Vertex])
                {
                    Thread.Sleep(500);
                    if (!Field.MarkGraphModel(ServiceFunctions.EdgeRepresentation(v.ToString(), adj.Vertex.ToString())) && !G.IsOrgraph)
                        Field.MarkGraphModel(ServiceFunctions.EdgeRepresentation(adj.Vertex.ToString(), v.ToString()));

                    DFS(G, adj.Vertex, visited);
                }
            }
            Field.UserInterface.PostStatusMessage("Выполниение обхода в глубину закончен!");
        }

        //public override RepositoryItem Clone(RepositoryArgs args)
        //{
        //    SetArgs((UIAlgorithmArgs)args);
        //    return this;
        //}

        //public override void Execute()
        //{
        //    TVertex vertex = new TVertex();
        //    vertex.SetFromString(((DFScommandArgs)_args).VertexModel.VertexStr);
        //    SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
        //    foreach (var v in _graph.AdjList)
        //        visited[v.Key] = false;
        //    DFS(vertex, visited);
        //}

    }
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
