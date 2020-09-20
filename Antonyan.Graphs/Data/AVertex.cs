using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Antonyan.Graphs.Data
{

    public abstract class AVertex : IComparable<AVertex>, IEquatable<AVertex>
    {
        public AVertex() { DefaultInit(); }
        public AVertex(string str) { SetFromString(str); }
        public abstract int CompareTo(AVertex other);
        public abstract bool Equals(AVertex other);
        public abstract override string ToString();
        public abstract override int GetHashCode();
        public abstract void SetFromString(string str);
        public abstract void DefaultInit();
        public abstract string GetRepresentation();

        // для Дейыктра
        public int d;
        public AVertex p;
    }
}
