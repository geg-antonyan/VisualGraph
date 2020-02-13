using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Util;
namespace Antonyan.Graphs.Backend.CommandArgs
{
    public class CreatFieldArgs<TVertex, TWeight> : EventArgs
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public CreatFieldArgs(UserInterface ui, CommandDispetcher<TVertex, TWeight> cd, bool oriented, bool weighted)
        {
            UI = ui;
            CommandDispetcher = cd;
            Oriented = oriented;
            Weighted = weighted;
        }
        public CommandDispetcher<TVertex, TWeight> CommandDispetcher { get; private set; }
        public UserInterface UI { get; private set; }
        public bool Oriented { get; private set; }
        public bool Weighted { get; private set; }
    }
}
