using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Board.Models
{
    public abstract class AEdgeModel : GraphModel
    {
        public AEdgeModel(AVertexModel source, AVertexModel stock, string weight)
            : base(ServiceFunctions.EdgeRepresentation(stock.VertexStr, source.VertexStr, weight))
        {
            StringRepresent = weight;
            Source = source;
            Stock = stock;
            Weight = weight;
        }
        public AVertexModel Source { get; private set; }
        public AVertexModel Stock { get; private set; }
        public string Weight { get; private set; }
        public abstract void RefreshPos();
    }
}
