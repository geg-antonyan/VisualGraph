using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Data
{
    public class Vertex : AVertex
    {
        private int data;
        public Vertex() : base() { }
        public Vertex(string str) : base(str) { }

        public override int CompareTo(AType other)
        {
            return data.CompareTo(((Vertex)other).data);
        }

        public override bool Equals(AType other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            return data.ToString();
        }

        public override void SetFromString(string str)
        {
            if (!int.TryParse(str, out data))
                throw new Exception($"Don't convert {str} to int in method Vertex.StringInit()");
        }

        public override void DefaultInit()
        {
            data = 0;
        }

        public override string GetRepresentation()
        {
            return ServiceFunctions.VertexRepresentation(data.ToString());
        }

        public override int GetHashCode()
        {
            return data.GetHashCode();
        }
    }
}
