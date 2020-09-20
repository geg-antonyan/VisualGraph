using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class AlgorithmCommandArgs : ACommandArgs
    {
        public AlgorithmCommandArgs(string commandName) 
            : base(commandName)
        {
        }
        public string TaskNameOut { get; internal set; }
        public string AlgorithmNameOut { get; internal set; }
        public bool SuccsessOut { get; internal set; }
    }
}
