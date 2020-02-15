using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Antonyan.Graphs.Data
{
    public abstract class AType : IComparable<AType>, IEquatable<AType>
    {
        public AType() { DefaultInit(); }
        public AType(string str) { StringInit(str); }

        protected int hashCode;

        public override int GetHashCode()
        {
            return hashCode;
        }
        public int CompareTo(AType other)
        {
            return CompareToImpl(other);
        }

        public bool Equals(AType other)
        {
            return EqualsImpl(other);
        }

        public void SetFromString(string str)
        {
            StringInit(str);
        }

        public override string ToString()
        {
            return ToStringImpl();
        }
        protected abstract int CompareToImpl(AType other);
        protected abstract bool EqualsImpl(AType other);
        protected abstract void DefaultInit();
        protected abstract void StringInit(string str);
        protected abstract string ToStringImpl();

    }
}
