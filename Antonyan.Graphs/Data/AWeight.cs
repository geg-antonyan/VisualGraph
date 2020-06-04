using System;
using System.Collections.Generic;
using System.Text;

namespace Antonyan.Graphs.Data
{
    public abstract class AWeight : AType
    {
        public AWeight() { DefaultInit(); }
        public AWeight(string str) { SetFromString(str); }

        public abstract AWeight Plus(AWeight other);
        public abstract AWeight Minus(AWeight other);
        public abstract AWeight NULL();
        public abstract bool LessThan(AWeight other);
        public abstract bool MoreThan(AWeight other);
    }
}
