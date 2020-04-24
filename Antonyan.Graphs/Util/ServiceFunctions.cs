using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Util
{
    public static class ServiceFunctions
    {
        //public 

        public static void WriteLine(string txt)
        {
            StreamWriter writer = new StreamWriter("log.txt", true);
            writer.WriteLine(txt);
            writer.Close();
        }
        public static string VertexRepresentation(string vertex)
        {
            return vertex;
        }

        public static string ModelRepresentation(GraphModel model)
        {
            if (model is AVertexModel)
                return ((AVertexModel)model).VertexStr;
            else
            {
                var edge = (AEdgeModel)model;
                return edge.Source.VertexStr + "->" +
                       edge.Stock.VertexStr + "=" +
                       edge.Weight;
            }
        }

        public static string EdgeRepresentation(string source, string stock, string weight)
        {
            return source + "->" + stock + "=" + weight;
        }

        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

    }
}
