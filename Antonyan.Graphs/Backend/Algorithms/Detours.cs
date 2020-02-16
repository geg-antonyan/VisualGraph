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
    public static class Detours<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public static void Visit(TVertex v, UserInterface ui, SortedDictionary<TVertex, TVertex> prev = null)
        {
            ui.MarkModel(v.GetHashCode());
            Thread.Sleep(700);
        }
        public static void DFS(Graph<TVertex, TWeight> G, TVertex v, SortedDictionary<TVertex, bool> visited, UserInterface ui)
        {
            Visit(v, ui);
            visited[v] = true;
            foreach (var adj in G[v])
            {
                if (!visited[adj.Item1])
                {
                    Thread.Sleep(500);
                    if (!ui.MarkModel((v.ToString() + " " + adj.Item1.ToString() +  " " + adj.Item2?.ToString()).GetHashCode()))
                        ui.MarkModel((adj.Item1.ToString() + " " + v.ToString() + " " + adj.Item2?.ToString()).GetHashCode());
                   
                    DFS(G, adj.Item1, visited, ui);
                }
            }
        }
        public static void BFS(Graph<TVertex, TWeight> G, TVertex v, SortedDictionary<TVertex, bool> visited,
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
                        if (!ui.MarkModel((x.ToString() + " " + w.Item1.ToString() + " " + w.Item2?.ToString()).GetHashCode()))
                            ui.MarkModel((w.Item1.ToString() + " " + x.ToString() + " " + w.Item2?.ToString()).GetHashCode());
                        Visit(w.Item1, ui);
                        visited[w.Item1] = true;
                        Q.Enqueue(w.Item1);
                    }
                }
            }
        }

    }
}
