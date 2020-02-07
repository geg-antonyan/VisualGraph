using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Data
{
    class GraphAndField<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
       
        private static GraphAndField<TVertex, TWeight> instance;
        public static Graph<TVertex, TWeight> Graph { get; private set; }

    
    }
}
