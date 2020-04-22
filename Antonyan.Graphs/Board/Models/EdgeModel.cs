using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Board;


namespace Antonyan.Graphs.Board.Models
{
    public class EdgeModel : GraphModels
  
    {
        public EdgeModel(VertexModel source, VertexModel stock, string weight, bool oriented)
        {
            Source = source;
            Stock = stock;
            Weight = weight;
            Oriented = oriented;
        }

       // public static 
        public bool Oriented { get; private set; }
        public VertexModel Source { get; private set; }
        public VertexModel Stock { get; private set; }
        public string Weight { get; private set; }


        public void SetSoruce(VertexModel source) => Source = source;
        public void SetStock(VertexModel stock) => Stock = stock;
    }
}
