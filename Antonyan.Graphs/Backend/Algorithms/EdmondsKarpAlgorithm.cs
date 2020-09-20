using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Antonyan.Graphs.Backend.Algorithms
{
    public class EdmondsKarpCommandArgs : AlgorithmCommandArgs
    {
        public EdmondsKarpCommandArgs(AVertexModel source, AVertexModel stock)
            : base("EdmondsKarpAlgorithm")
        {
            Source = source;
            Stock = stock;
        }
        public AVertexModel Stock { get; private set; }
        public AVertexModel Source { get; private set; }
        public int MaxFlowOut { get; internal set; }
    }
    public class EdmondsKarpAlgorithm<TVertex> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
    {

        private EdmondsKarpCommandArgs _args;
        private Graph<TVertex> G;
        private SortedDictionary<TVertex, SortedDictionary<TVertex, int>> c;
        public EdmondsKarpAlgorithm(IModelField field)
            : base(field)
        {
        }

        public EdmondsKarpAlgorithm(EdmondsKarpCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
            G = ((ModelsField<TVertex>)Field).Graph;
            c = new SortedDictionary<TVertex, SortedDictionary<TVertex, int>>();
            G.AdjList.Keys.ToList().ForEach(v =>
            {
                c[v] = new SortedDictionary<TVertex, int>();
                G.AdjList.Keys.ToList().ForEach(u =>
                {
                    c[v][u] = 0;
                });
            });
            G.AdjList.ToList().ForEach(p =>
            {
                var v = p.Key;
                p.Value.ForEach(e =>
                {
                    var u = e.Vertex;
                    c[v][u] = e.Weight;
                });
            });
        }

        public ICommand Clone(ACommandArgs args)
        {
            return new EdmondsKarpAlgorithm<TVertex>((EdmondsKarpCommandArgs)args, Field);
        }

        public void Execute()
        {
            TVertex s = new TVertex();
            TVertex t = new TVertex();
            s.SetFromString(_args.Source.VertexStr);
            t.SetFromString(_args.Stock.VertexStr);
            s = G.AdjList.Keys.Where(v => v.Equals(s)).First();
            t = G.AdjList.Keys.Where(v => v.Equals(t)).First();
            _args.MaxFlowOut = EdmondsKarp(s, t);
            _args.SuccsessOut = true;
            _args.TaskNameOut = $"Найти максимальный поток из вершины {s} в {t}";
            _args.AlgorithmNameOut = "Эдмондс-Карп";
        }


        private int EdmondsKarp(TVertex s, TVertex t)
        {
            int sleep = 500;
            int maxFlow = 0;
            int incrFlow;
            Field.RefreshDefault();
            while ((incrFlow = BFS(s, t, out var p)) != 0)
            {
                var sink = t;
                while (sink != s)
                {
                    c[p[sink]][sink] -= incrFlow;
                    c[sink][p[sink]] += incrFlow;
                    Thread.Sleep(sleep);
                    Field.SetWeightMark(ServiceFunctions.EdgeRepresentation(p[sink].GetRepresentation(), sink.GetRepresentation()), $"{c[sink][p[sink]]}/", true);
                    Thread.Sleep(sleep);
                    sink = p[sink];
                }
                maxFlow += incrFlow;
                Field.RefreshDefault(false);
            }
            return maxFlow;
        }

        private int BFS(TVertex s, TVertex t, out SortedDictionary<TVertex, TVertex> p)
        {
            int sleep = 400;
            var color = RGBcolor.DarkGreen;
            int incrFlow = int.MaxValue;
            Field.RefreshDefault(false);
            p = new SortedDictionary<TVertex, TVertex>();
            foreach (var v in G.AdjList.Keys)
                p[v] = null;
            var Q = new Queue<TVertex>();
            Q.Enqueue(s);
            while (Q.Count != 0)
            {
                var v = Q.Dequeue();
                foreach (var uw in G[v])
                {
                    var u = uw.Vertex;
                    if (c[v][u] > 0 && p[u] == null)
                    {
                        Q.Enqueue(u);
                        p[u] = v;
                        if (u == t)
                        {
                            List<TVertex> vs = new List<TVertex>();
                            var last = t;
                            while (last != null)
                            {
                                vs.Add(last);
                                last = p[last];
                            }
                            vs.Reverse();
                            for (int i = 0; i < vs.Count - 1; i++)
                                if (c[vs[i]][vs[i + 1]] < incrFlow)
                                    incrFlow = c[vs[i]][vs[i + 1]];
                            for (int j = 0; j < vs.Count - 1; j++)
                            {
                                Field.SetColorAndWidth(vs[j].GetRepresentation(), color, 2);
                                Thread.Sleep(sleep);
                                Field.SetColorAndWidth(ServiceFunctions.EdgeRepresentation(vs[j].GetRepresentation(), vs[j + 1].GetRepresentation()), color, 2);
                                Thread.Sleep(sleep);
                            }
                            Field.SetColorAndWidth(vs[vs.Count - 1].GetRepresentation(), color, 2);
                            Thread.Sleep(sleep);
                            return incrFlow;
                        }
                    }
                }
            }
            return 0;
        }
    }
}
