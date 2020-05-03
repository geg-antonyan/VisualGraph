using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Antonyan.Graphs.Data
{
    public abstract class AType : IComparable<AType>, IEquatable<AType>
    {
        public abstract int CompareTo(AType other);
        public abstract bool Equals(AType other);
        public abstract override string ToString();
        public abstract override int GetHashCode();
        public abstract void SetFromString(string str);
        public abstract void DefaultInit();
    }
}
