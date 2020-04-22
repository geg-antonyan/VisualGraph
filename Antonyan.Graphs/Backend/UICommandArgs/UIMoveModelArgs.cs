using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;

namespace Antonyan.Graphs.Backend.UICommandArgs
{
    public class UIMoveModelArgs : UIFieldUpdateArgs
    {
        public UIMoveModelArgs(string represent, vec2 newPos, vec2 lastPos) 
            : base(nameof(MoveModelCommand))
        {
            Represent = represent;
            NewPos = newPos;
            LastPos = lastPos;
        }

        public string Represent { get; private set; }
        public vec2 NewPos { get; private set; }
        public vec2 LastPos { get; private set; }
    }
}
