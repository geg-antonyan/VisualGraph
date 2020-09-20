using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Algorithms
{

    public class NPeripheryCommandArgs : AlgorithmCommandArgs
    {
        public NPeripheryCommandArgs(AVertexModel vertex, int n)
            : base("NPeripheryCommand")
        {
            Vertex = vertex;
            N = n;
        }
        public AVertexModel Vertex { get; private set; }
        public int N { get; private set; }
        public List<string> NPeripheryOut { get; internal set; } = new List<string>();

    }


    public class NPeripheryCommand<TVertex> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
    {
        private Graph<TVertex> G;
        private NPeripheryCommandArgs _args;
        public NPeripheryCommand(IModelField field) 
            : base(field)
        {
        }

        public NPeripheryCommand(NPeripheryCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
            G = ((ModelsField<TVertex>)Field).Graph;
        }

        public ICommand Clone(ACommandArgs args)
        {
            return new NPeripheryCommand<TVertex>((NPeripheryCommandArgs)args, Field);
        }

        public void Execute()
        {
            TVertex u = new TVertex();
            u.SetFromString(_args.Vertex.VertexStr);
            u = G.AdjList.Keys.ToList().Where(v => v.Equals(u)).First();
            SortedDictionary<TVertex, int> nPeiphery = new SortedDictionary<TVertex, int>();
            G.AdjList.Keys.ToList().ForEach(v =>
            {
                if (!v.Equals(u))
                {
                    BellmanFord(v);
                    if (u.d < GGF<TVertex>.INF &&  !(u.d > v.d + GGF<TVertex>.W(G, v, u)) && u.d > _args.N)
                        nPeiphery[v] = u.d;
                }
            });
            nPeiphery.ToList().ForEach(v =>
            {
                _args.NPeripheryOut.Add($"{v.Key.GetRepresentation()} = {v.Value}");
                Field.SetColorAndWidth(v.Key.GetRepresentation(), RGBcolor.DarkGreen, 2);
                Thread.Sleep(500);
            });
            _args.AlgorithmNameOut = "Форд-Беллман";
            _args.TaskNameOut = $"{_args.N}-периферия вершины {u}";
            _args.SuccsessOut = true;
        }

        private void BellmanFord(TVertex s)
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


            //foreach (var a in G.AdjList)
            //{
            //    var u = a.Key;
            //    foreach (var b in a.Value)
            //    {
            //        var v = b.Vertex;
            //        if (v.d > u.d + GGF<TVertex>.W(G, u, v))
            //            return false;
            //    }
            //}
            //return true;
        }
    }




}
