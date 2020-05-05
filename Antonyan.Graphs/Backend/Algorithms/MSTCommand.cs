using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
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
    internal class Edge<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        internal Edge(TVertex source, TVertex stock, TWeight weight)
        {
            Source = source;
            Stock = stock;
            Weight = weight;
        }
        internal TVertex Source { get; private set; }
        internal TVertex Stock { get; private set; }
        internal TWeight Weight { get; private set; }

        public bool IsReverseEdge(Edge<TVertex, TWeight> other)
        {
            return Source.Equals(other.Stock) && Stock.Equals(other.Source);
        }
    }

    internal class Set
    {
        internal Set P { get; set; }
    }

    public class MSTCommandArgs : ACommandArgs
    {
        public List<string> MstOut { get; internal set; }
        public string AlgorithmNameOut { get; internal set; }
        public int TimeOutMSec { get; private set; }
        public MSTCommandArgs(int timout)
            : base("MSTCommand")
        {
            TimeOutMSec = timout;
        }
    }
    public class MSTCommand<TVertex, TWeight> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private MSTCommandArgs _args;

        public MSTCommand(IModelField field)
            : base(field)
        { }
        public MSTCommand(MSTCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }

        public ICommand Clone(ACommandArgs args)
        {
            return new MSTCommand<TVertex, TWeight>((MSTCommandArgs)args, Field);
        }

        public void Execute()
        {
            var G = ((ModelsField<TVertex, TWeight>)Field).Graph;
            if (G.IsOrgraph || !G.IsWeighted)
            {
                _args.SuccsessOut = false;
                throw new Exception("Для выполнении алгоритма Крускала (МОД - минимальный Каркас) нужен взвещанный неориетированный граф");
            }
            var res = MstKruskal(G);
            _args.AlgorithmNameOut = "Крускал";
            var outs = _args.MstOut = new List<string>();
            res.ForEach(edge =>
            {
                outs.Add($"{edge.Source} - {edge.Stock} = {edge.Weight}");
            });
            _args.SuccsessOut = true;
        }


        List<Edge<TVertex, TWeight>> Distinct(List<Edge<TVertex, TWeight>> list)
        {
            var res = new List<Edge<TVertex, TWeight>>();
            list.ForEach(el =>
            {
                if (res.Find(m => m.IsReverseEdge(el)) == null)
                    res.Add(el);
            });
            return res;
        }

        void Union(Set u, Set v) => u.P = v;

        Set FindSet(Set u)
        {
            if (u != u.P)
                u.P = FindSet(u.P);
            return u.P;
        }

        Set MakeSet(TVertex v)
        {
            var s = new Set();
            s.P = s;
            return s;
        }



        private List<Edge<TVertex, TWeight>> MstKruskal(Graph<TVertex, TWeight> G)
        {
            SendMessage("Создаем массив, где будем хранить минимальный каркас графа");

            var mst = new List<Edge<TVertex, TWeight>>();

            SendMessage("Создаем непересикающие множества для каждой вершины");

            SortedDictionary<TVertex, Set> set = new SortedDictionary<TVertex, Set>();
            G.AdjList.Keys.ToList().ForEach(v =>
            {
                set.Add(v, MakeSet(v));
            });


            List<Edge<TVertex, TWeight>> edges = new List<Edge<TVertex, TWeight>>();

            SendMessage("Сортируем ребра по возрастанию веса");

            G.AdjList.ToList().ForEach(v =>
            {
                v.Value.ForEach(e => edges.Add(new Edge<TVertex, TWeight>(v.Key, e.Vertex, e.Weight)));
            });
            var sortEdges = Distinct(edges).OrderBy(e => e.Weight).ToList();

            SendMessage("Начинаем проход по сортированным ребрам, пока не пройдем их всех");

            for (int i = 0; i < sortEdges.Count && mst.Count < G.Count - 1; i++)
            {
                var edge = sortEdges[i];


                SendMessage($"Смотрим ребро соединяющий вершину {edge.Source} с вершиной {edge.Stock}");
                MarkVertices(edge.Source, edge.Stock, RGBcolor.Orange, 2);
                Thread.Sleep(500);
                MarkEdge(edge.Source, edge.Stock, RGBcolor.Orange, 2);

                var u = FindSet(set[edge.Source]);
                var v = FindSet(set[edge.Stock]);
                if (u != v)
                {
                    SendMessage($"Отмеченные вершины находятся в разных множествах");

                    mst.Add(edge);

                    MarkEdge(edge.Source, edge.Stock, RGBcolor.DarkGreen, 3);
                    SendMessage($"Добавляем отмеченное ребро в МОД, и соединяем множества");

                    Union(u, v);
                }
                else
                {
                    SendMessage($"Отмеченные вершины находятся в одном множестве, пропускаем");
                    MarkDefaultEdge(edge.Source, edge.Stock);
                }

                MarkDefaultVertices(edge.Source, edge.Stock);
            }
            return mst;
        }


        private void SendMessage(string message)
        {
            int sleep = _args.TimeOutMSec / 6;
            for (int i = 0; i < 6; i++)
            {
                message += ".";
                Field.UserInterface.PostStatusMessage(message);
                Thread.Sleep(sleep);
            }
        }


        private void MarkEdge(TVertex src, TVertex stc, RGBcolor color, int width)
        {

            if (!Field.SetColorAndWidth(ServiceFunctions.EdgeRepresentation(src.GetRepresentation(), stc.GetRepresentation()), color, width))
                Field.SetColorAndWidth(ServiceFunctions.EdgeRepresentation(stc.GetRepresentation(), src.GetRepresentation()), color, width);
        }

        private void MarkVertices(TVertex a, TVertex b, RGBcolor color, int width)
        {
            Field.SetColorAndWidth(a.GetRepresentation(), color, width, false);
            Field.SetColorAndWidth(b.GetRepresentation(), color, width);
        }

        private void MarkDefaultVertices(TVertex a, TVertex b)
        {
            Field.SetModelDefaultOptions(a.GetRepresentation(), false);
            Field.SetModelDefaultOptions(b.GetRepresentation());
        }

        private void MarkDefaultEdge(TVertex src, TVertex stc)
        {
            if (!Field.SetModelDefaultOptions(ServiceFunctions.EdgeRepresentation(src.GetRepresentation(), stc.GetRepresentation())))
                Field.SetModelDefaultOptions(ServiceFunctions.EdgeRepresentation(stc.GetRepresentation(), src.GetRepresentation()));
        }
    }
}
