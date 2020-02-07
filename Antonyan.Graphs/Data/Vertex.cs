using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Antonyan.Graphs.Data
{
    public class Vertex : AVertex
    {
        private int data;

        protected override int CompareToImpl(AType other)
        {
            return data.CompareTo(((Vertex)other).data);
        }

        protected override void DefaultInit()
        {
            data = 0;
        }

        protected override bool EqualsImpl(AType other)
        {
            return CompareToImpl(other) == 0;
        } 

        protected override void StringInit(string str)
        {
            if (!int.TryParse(str, out data))
                throw new Exception($"Don't convert {str} to int in method Vertex.StringInit()");
        }

        protected override string ToStringImpl()
        {
            return data.ToString();
        }
    }
}
