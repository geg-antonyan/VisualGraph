using Antonyan.Graphs.Backend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend.Algorithms
{
    public class DijkstraCommandArgs : ACommandArgs
    {
        public DijkstraCommandArgs(AVertexModel model)
            : base("DFSalgorithm")
        {
            VertexModel = model;
        }

        public AVertexModel VertexModel { get; private set; }
    }

    public class DijkstraAlgorithm<TVertex, TWeight> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private DijkstraCommandArgs _args;
        private readonly Graph<TVertex, TWeight> G;
        public DijkstraAlgorithm(IModelField field)
            : base(field)
        {

        }

        public DijkstraAlgorithm(DijkstraCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
            G = ((ModelsField<TVertex, TWeight>)field).Graph;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new DijkstraAlgorithm<TVertex, TWeight>((DijkstraCommandArgs)args, Field);
        }

        public void Execute()
        {
            const int INF = int.MaxValue;
            TVertex temp = new TVertex();
            temp.SetFromString(_args.VertexModel.VertexStr);
            G.AdjList.Keys.ToList().ForEach(v => 
            { 
                v.d = null; 
                v.p = null;
                if (v.Equals(temp))
                    v.d = ;
            });

        }


        private TWeight w(TVertex u, TVertex v)
        {
            return G[u].Where(p => p.Vertex.Equals(v)).First().Weight;
        }
    }
}
