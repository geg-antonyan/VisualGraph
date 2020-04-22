using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Util
{
    public static class Representations
    {
        public static string VertexRepresentation(string vertex)
        {
            return vertex;
        }

        public static string ModelRepresentation(GraphModels model)
        {
            if (model is VertexModel)
                return ((VertexModel)model).VertexStr;
            else
            {
                var edge = (EdgeModel)model;
                return edge.Source.VertexStr + "->" +
                       edge.Stock.VertexStr + "=" +
                       edge.Weight;
            }
        }

        public static string EdgeRepresentation(string source, string stock, string weight)
        {
            return source + "->" + stock + "=" + weight;
        }
    }
}
