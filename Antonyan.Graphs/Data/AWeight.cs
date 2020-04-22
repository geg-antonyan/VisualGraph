using System;
using System.Collections.Generic;
using System.Text;

namespace Antonyan.Graphs.Data
{
    public abstract class AWeight : AType
    {
        public AWeight()  { DefaultInit(); }
        public AWeight(string str)  { SetFromString(str); }

    }
}
