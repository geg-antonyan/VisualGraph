using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Antonyan.Graphs.Data
{
    public class Weight : AWeight
    {
        private int data;
        public Weight() : base() { }
        public Weight(string str) : base(str) { }
        

        public override int CompareTo(AType other)
        {
            return data.CompareTo(((Weight)other).data);
        }
        public override bool Equals(AType other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString()
        {
            return data.ToString();
        }


        public override void DefaultInit()
        {
            data = 0;
        }
      
        public override void SetFromString(string str)
        {
            if (!int.TryParse(str, out data))
                throw new Exception($"Don't convert {str} to int in method Vertex.StringInit()");
        }

    }
}
