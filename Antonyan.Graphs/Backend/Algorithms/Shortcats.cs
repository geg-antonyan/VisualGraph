using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using System.Threading;

namespace Antonyan.Graphs.Backend.Algorithms
{
    public static class Shortcats<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private static void ParentsBFS(
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

        private static void FindPath(Graph<TVertex, TWeight> G, 
            TVertex source, TVertex stock, SortedDictionary<TVertex, TVertex> parents, UserInterface ui)
        {
            if (stock.Equals(source))
            {
                ui.MarkModel(source.GetRepresentation());
                Thread.Sleep(500);
            }
            else if (parents[stock] == null)
            {
                ui.UnmarkAll();
                ui.PostMessage($"Путь из {source.ToString()} в {stock.ToString()} не существует");
                return;
            }
            else
            {
                FindPath(G, source, parents[stock], parents, ui);
                Thread.Sleep(500);
                var tmp = parents[stock];
                if (!ui.MarkModel(Representations.EdgeRepresentation(tmp?.ToString(), stock.ToString(), null)))
                    ui.MarkModel(Representations.EdgeRepresentation(stock.ToString(), tmp?.ToString(), null));
                Thread.Sleep(500);
                ui.MarkModel(stock.GetRepresentation());
          
            }
        }

        public static void ShortcutBFS(Graph<TVertex, TWeight> G,
            TVertex soruce, TVertex stock, UserInterface ui)
        {
            ui.UnmarkAll();
            SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
            var parents = new SortedDictionary<TVertex, TVertex>();
            foreach (var v in G.AdjList)
            {
                visited[v.Key] = false;
                parents[v.Key] = null;
            }
            ParentsBFS(G, soruce, visited, ref parents);
            FindPath(G, soruce, stock, parents, ui);
        }
    }
}
