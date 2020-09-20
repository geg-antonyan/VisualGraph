using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
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

    public class WayNoMoreThenLCommandArgs : AlgorithmCommandArgs
    {
        public WayNoMoreThenLCommandArgs(AVertexModel source, AVertexModel stock, int l) 
            : base("WayNoMoreThenLCommand")
        {
            Source = source;
            Stock = stock;
            L = l;
        }

        public AVertexModel Source { get; private set; }
        public AVertexModel Stock { get; private set; }
        public int L { get; private set; }
        public List<string> OutWays { get; internal set; } = new List<string>();
        public bool Exist { get; internal set; } = false;
    }
    public class WayNoMoreThenLCommand<TVertex> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
    {
        private WayNoMoreThenLCommandArgs _args;
        private Graph<TVertex> G;

        public WayNoMoreThenLCommand(IModelField field)
            : base(field)
        {

        }
        public WayNoMoreThenLCommand(WayNoMoreThenLCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
            G = ((ModelsField<TVertex>)Field).Graph;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new WayNoMoreThenLCommand<TVertex>((WayNoMoreThenLCommandArgs)args, Field);
        }

        public void Execute()
        {
            Field.RefreshDefault();
            TVertex s1 = new TVertex();
            TVertex s2 = new TVertex();
            s1.SetFromString(_args.Source.VertexStr);
            s2.SetFromString(_args.Stock.VertexStr);
            s1 = G.AdjList.Keys.Where(k => k.Equals(s1)).First();
            s2 = G.AdjList.Keys.Where(k => k.Equals(s2)).First();
            if (!BellmanFord(s1))
            {
                Field.UserInterface.PostWarningMessage("В графе обноружен отрицательный цикл!!!");
                return;
            }
            _args.TaskNameOut = $"Определить, существует ли путь длиной не более {_args.L} между вершинами {s1} и {s2}";
            _args.AlgorithmNameOut = "Форд-Беллман";
            if (s2.d != GGF<TVertex>.INF && s2.d <= _args.L)
            {
                _args.Exist = true;
                var way = new List<TVertex>();
                var cost = s2.d;
                var tmpS = s2;
                while (tmpS != null)
                {
                    way.Add(tmpS);
                    tmpS = (TVertex)tmpS.p;
                }
                way.Reverse();
                MarkedWay(way, cost);
            }
            BellmanFord(s2);
            if (s1.d != GGF<TVertex>.INF && s1.d <= _args.L)
            {

                _args.Exist = true;
                var way = new List<TVertex>();
                var cost = s1.d;
                var tmpS = s1;
                while (tmpS != null)
                {
                    way.Add(tmpS);
                    tmpS = (TVertex)tmpS.p;
                }
                way.Reverse();
                Field.RefreshDefault();
                Thread.Sleep(1300);
                MarkedWay(way, cost);
            }
            _args.SuccsessOut = true;
        }


        private bool BellmanFord(TVertex s)
        {
            GGF<TVertex>.InitializeSingleSource(G, s);
            for (int i = 1; i < G.Count; i++)
            {
                G.AdjList.ToList().ForEach(u =>
                {
                    u.Value.ForEach(v =>
                    {
                        GGF<TVertex>.Relax(G, u.Key, v.Vertex, GGF<TVertex>.W);
                    });
                });
            }
            foreach (var a in G.AdjList)
            {
                var u = a.Key;
                foreach (var b in a.Value)
                {
                    var v = b.Vertex;
                    if (v.d > u.d + GGF<TVertex>.W(G, u, v))
                        return false;
                }
            }
            return true;
        }

        private void MarkedWay(List<TVertex> way, int cost)
        {
            StringBuilder sb = new StringBuilder();
            way.ForEach(v => sb.Append($"{v} -> "));
            _args.OutWays.Add(sb.ToString().TrimEnd(' ', '>', '-') + $" = {cost}");
            for (int i = 0; i < way.Count - 1; i++)
            {
                Field.SetColorAndWidth(way[i].GetRepresentation(), RGBcolor.Green, 2);
                Thread.Sleep(500);
                if (!Field.SetColorAndWidth(ServiceFunctions.EdgeRepresentation(way[i].GetRepresentation(),
                    way[i + 1].GetRepresentation()), RGBcolor.Green, 2) && !Field.IsOrgraph)
                    Field.SetColorAndWidth(ServiceFunctions.EdgeRepresentation(way[i + 1].GetRepresentation(),
                    way[i].GetRepresentation()), RGBcolor.Green, 2);
                Thread.Sleep(500);
                Field.SetColorAndWidth(way[i + 1].GetRepresentation(), RGBcolor.Green, 2);
            }
        }
    }
}
