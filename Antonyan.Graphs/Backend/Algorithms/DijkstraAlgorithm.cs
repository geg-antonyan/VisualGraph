using Antonyan.Graphs.Backend.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using System.Threading;

namespace Antonyan.Graphs.Backend.Algorithms
{
    public class DijkstraCommandArgs : AlgorithmCommandArgs
    {
        public DijkstraCommandArgs(AVertexModel model)
            : base("DijkstraAlgorithm")
        {
            StockModel = model;
        }

        public AVertexModel StockModel { get; private set; }
        public List<string> OutWays { get; internal set; } = new List<string>();
    }

    public class DijkstraAlgorithm<TVertex> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
    {
        
        private DijkstraCommandArgs _args;
        private readonly Graph<TVertex> G;
        public DijkstraAlgorithm(IModelField field)
            : base(field)
        {

        }

        public DijkstraAlgorithm(DijkstraCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
            G = ((ModelsField<TVertex>)field).Graph;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new DijkstraAlgorithm<TVertex>((DijkstraCommandArgs)args, Field);
        }

        public void Execute()
        {
            TVertex temp = new TVertex();
            temp.SetFromString(_args.StockModel.VertexStr);
            var stock = G.AdjList.Keys.Where(k => k.Equals(temp)).First();
            Field.RefreshDefault();
            G.AdjList.Keys.ToList().ForEach(s =>
            {
                GGF<TVertex>.InitializeSingleSource(G, s);
                var S = new SortedDictionary<TVertex, TVertex>();
                var Q = new List<TVertex>(G.AdjList.Keys.ToList());
                while (Q.Count != 0)
                {
                    var u = GGF<TVertex>.ExtractMin(Q);
                    S.Add(u, u);
                    G[u].ForEach(v => GGF<TVertex>.Relax(G, u, v.Vertex, GGF<TVertex>.W));
                }
                Field.RefreshDefault();
                MarkedWay(S, s, stock);
                Thread.Sleep(800);
            });
            _args.TaskNameOut = $"Найти все кратчайщие пути до вершины {stock}!";
            _args.AlgorithmNameOut = "Дейкстра!";
            _args.SuccsessOut = true;
        }

        private void MarkedWay(SortedDictionary<TVertex, TVertex> S, TVertex source, TVertex stock)
        {
            var way = new List<TVertex>();
            var cost = stock.d;
            while (stock != null)
            {
                way.Add(stock);
                stock = (TVertex)stock.p;
            }
            if (way.Count < 2) return;
            way.Reverse();
            StringBuilder sb = new StringBuilder();
            way.ForEach(v => sb.Append($"{v} -> "));
            _args.OutWays.Add(sb.ToString().TrimEnd(' ', '>', '-') + $" = {cost}");
            for (int i = 0; i < way.Count - 1; i++)
            {
                Field.SetColorAndWidth(way[i].GetRepresentation(), RGBcolor.Green, 2);
                Thread.Sleep(500);
                if  (!Field.SetColorAndWidth(ServiceFunctions.EdgeRepresentation(way[i].GetRepresentation(),
                    way[i + 1].GetRepresentation()), RGBcolor.Green, 2) && !Field.IsOrgraph)
                    Field.SetColorAndWidth(ServiceFunctions.EdgeRepresentation(way[i + 1].GetRepresentation(),
                    way[i].GetRepresentation()), RGBcolor.Green, 2);
                Thread.Sleep(500);
                Field.SetColorAndWidth(way[i + 1].GetRepresentation(), RGBcolor.Green, 2);
            }
        }

       




        

    }
}
