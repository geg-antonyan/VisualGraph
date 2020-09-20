using Antonyan.Graphs.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Util
{
    public static class GGF<TVertex>
        where TVertex : AVertex, new()
    {
        public static readonly int INF = 1_000_000_000;
        public static int W(Graph<TVertex> G, TVertex u, TVertex v)
        {
            var res = G[u].Find(m => m.Vertex.Equals(v));
            if (res == null) return INF;
            return G[u].Where(p => p.Vertex.Equals(v)).First().Weight;
        }
        public static void Relax(Graph<TVertex> G, TVertex u, TVertex v, Func<Graph<TVertex>, TVertex, TVertex, int> w)
        {
            if (u.d != INF && v.d > u.d + w(G, u, v))
            {
                v.d = u.d + w(G, u, v);
                v.p = u;
            }
        }
        public static TVertex ExtractMin(List<TVertex> Q)
        {
            var min = Q.First();
            Q.ForEach(v => min = min.d > v.d ? v : min);
            Q.Remove(min);
            return min;
        }
        public static void InitializeSingleSource(Graph<TVertex> G, TVertex s)
        {
            G.AdjList.Keys.ToList().ForEach(v =>
            {
                v.d = INF;
                v.p = null;
            });
            s.d = 0;
        }
    }
}
