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

        public vec2 WeightPos { get; protected set; }
        public bool Weighted { get; protected set; }
        public float WeightAngle { get; protected set; }
        public AVertexModel Source { get; private set; }
        public AVertexModel Stock { get; private set; }
        public string Weight { get; protected set; }
        public AEdgeModel(AVertexModel source, AVertexModel stock, string weight)
            : base(ServiceFunctions.EdgeRepresentation(source.VertexStr, stock.VertexStr))
        {
            StringRepresent = weight;
            Source = source;
            Stock = stock;
            Weight = weight;
            Weighted = weight != null;
        }
        public abstract void RefreshPos();
    }
}
