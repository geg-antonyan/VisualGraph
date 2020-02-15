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
        public static void Visit(TVertex v, UserInterface ui)
        {
            ui.MarkModel(v.GetHashCode());
            Thread.Sleep(1000);
        }
        public static void DFS(Graph<TVertex, TWeight> G, TVertex v, SortedDictionary<TVertex, bool> visited, UserInterface ui)
        {
            Visit(v, ui);
            visited[v] = true;
            foreach (var adj in G[v])
            {

                if (!visited[adj.Item1])
                {
                    if (!ui.MarkModel((v.ToString() + adj.Item1.ToString() + adj.Item2?.ToString()).GetHashCode()))
                        ui.MarkModel((adj.Item1.ToString() + v.ToString() + adj.Item2?.ToString()).GetHashCode());
                    Thread.Sleep(1000);
                    DFS(G, adj.Item1, visited, ui);
                }
            }
        }
    }
}
