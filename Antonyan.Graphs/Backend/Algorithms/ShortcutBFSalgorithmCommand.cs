using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using Antonyan.Graphs.Board;
using System.Threading;
using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board.Models;

namespace Antonyan.Graphs.Backend.Algorithms
{

    public class ShortcutBFSCommandArgs : ACommandArgs
    {
        public ShortcutBFSCommandArgs(AVertexModel source, AVertexModel stock)
            : base("ShortcutBFSalgorithm")
        {
            SourceModel = source;
            StockModel = stock;
        }

        public AVertexModel SourceModel { get; private set; }
        public AVertexModel StockModel { get; private set; }
    }

    public class ShortcutBFSalgorithmCommand<TVertex, TWeight> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private readonly ShortcutBFSCommandArgs _args;

        public ShortcutBFSalgorithmCommand(IModelField field) 
            : base(field)
        {

        }
        public ShortcutBFSalgorithmCommand(ShortcutBFSCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new ShortcutBFSalgorithmCommand<TVertex, TWeight>((ShortcutBFSCommandArgs)args, Field);
        }

        public void Execute()
        {
            TVertex source = new TVertex();
            TVertex stock = new TVertex();
            source.SetFromString(_args.SourceModel.VertexStr);
            stock.SetFromString(_args.StockModel.VertexStr);
            var graph = ((ModelsField<TVertex, TWeight>)Field).Graph;
            Field.UnmarkGraphModels();
            ShortcutBFS(graph, source, stock);
        }

        public void ShortcutBFS(Graph<TVertex, TWeight> G,
            TVertex soruce, TVertex stock)
        {
            SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
            var parents = new SortedDictionary<TVertex, TVertex>();
            foreach (var v in G.AdjList)
            {
                visited[v.Key] = false;
                parents[v.Key] = null;
            }
            ParentsBFS(G, soruce, visited, ref parents);
            FindPath(G, soruce, stock, parents);
        }
        private void ParentsBFS(
          Graph<TVertex, TWeight> G, TVertex v,
          SortedDictionary<TVertex, bool> visited,
          ref SortedDictionary<TVertex, TVertex> parents)
        {
            visited[v] = true;
            Queue<TVertex> Q = new Queue<TVertex>();
            Q.Enqueue(v);
            parents[v] = null;
            while (Q.Count != 0)
            {
                var x = Q.Dequeue();
                foreach (var w in G[x])
                {
                    if (!visited[w.Item1])
                    {
                        visited[w.Item1] = true;
                        Q.Enqueue(w.Item1);
                        parents[w.Item1] = x;
                        Q.Enqueue(w.Item1);
                    }
                }
            }
        }

        private void FindPath(Graph<TVertex, TWeight> G,
            TVertex source, TVertex stock, SortedDictionary<TVertex, TVertex> parents)
        {
            if (stock.Equals(source))
            {
                Field.MarkGraphModel(source.GetRepresentation());
                Thread.Sleep(500);
            }
            else if (parents[stock] == null)
            {
                Field.UnmarkGraphModels();
                Field.UserInterface.PostMessage($"Путь из {source} в {stock} не существует");
                return;
            }
            else
            {
                FindPath(G, source, parents[stock], parents);
                Thread.Sleep(500);
                var tmp = parents[stock];
                if (!Field.MarkGraphModel(ServiceFunctions.EdgeRepresentation(tmp?.ToString(), stock.ToString(), null)) && !G.IsOrgraph)
                    Field.MarkGraphModel(ServiceFunctions.EdgeRepresentation(stock.ToString(), tmp?.ToString(), null));
                Thread.Sleep(500);
                Field.MarkGraphModel(stock.GetRepresentation());

            }
        }
    }




    //public static class Shortcats<TVertex, TWeight>
    //    where TVertex : AVertex, new()
    //    where TWeight : AWeight, new()
    //{
    //    private static void ParentsBFS(
    //      Graph<TVertex, TWeight> G, TVertex v,
    //      SortedDictionary<TVertex, bool> visited,
    //      ref SortedDictionary<TVertex, TVertex> parents)
    //    {
    //        visited[v] = true;
    //        Queue<TVertex> Q = new Queue<TVertex>();
    //        Q.Enqueue(v);
    //        parents[v] = null;
    //        while (Q.Count != 0)
    //        {
    //            var x = Q.Dequeue();
    //            foreach (var w in G[x])
    //            {
    //                if (!visited[w.Item1])
    //                {
    //                    visited[w.Item1] = true;
    //                    Q.Enqueue(w.Item1);
    //                    parents[w.Item1] = x;
    //                    Q.Enqueue(w.Item1);
    //                }
    //            }
    //        }
    //    }

    //    private static void FindPath(Graph<TVertex, TWeight> G, 
    //        TVertex source, TVertex stock, SortedDictionary<TVertex, TVertex> parents, UserInterface ui)
    //    {
    //        if (stock.Equals(source))
    //        {
    //            ui.MarkModel(source.GetRepresentation());
    //            Thread.Sleep(500);
    //        }
    //        else if (parents[stock] == null)
    //        {
    //            ui.UnmarkAll();
    //            ui.PostMessage($"Путь из {source.ToString()} в {stock.ToString()} не существует");
    //            return;
    //        }
    //        else
    //        {
    //            FindPath(G, source, parents[stock], parents, ui);
    //            Thread.Sleep(500);
    //            var tmp = parents[stock];
    //            if (!ui.MarkModel(ServiceFunctions.EdgeRepresentation(tmp?.ToString(), stock.ToString(), null)))
    //                ui.MarkModel(ServiceFunctions.EdgeRepresentation(stock.ToString(), tmp?.ToString(), null));
    //            Thread.Sleep(500);
    //            ui.MarkModel(stock.GetRepresentation());

    //        }
    //    }

    //    public static void ShortcutBFS(Graph<TVertex, TWeight> G,
    //        TVertex soruce, TVertex stock, UserInterface ui)
    //    {
    //        ui.UnmarkAll();
    //        SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
    //        var parents = new SortedDictionary<TVertex, TVertex>();
    //        foreach (var v in G.AdjList)
    //        {
    //            visited[v.Key] = false;
    //            parents[v.Key] = null;
    //        }
    //        ParentsBFS(G, soruce, visited, ref parents);
    //        FindPath(G, soruce, stock, parents, ui);
    //    }
    //}
}
