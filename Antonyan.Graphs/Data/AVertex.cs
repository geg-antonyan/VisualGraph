using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Antonyan.Graphs.Data
{

    public abstract class AVertex : AType
    {
        public AVertex() : base() { }
        public AVertex(string str) : base(str) { }
   
    }
}
