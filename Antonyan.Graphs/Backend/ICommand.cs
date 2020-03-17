using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Data;

namespace Antonyan.Graphs.Backend
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        ICommand Clone(ACommandArgs args);
        string HelpMessage();
    }


    public abstract class AFieldCommand<FieldCommandArgs, TVertex, TWeigth>
        where TVertex : AVertex, new()
        where TWeigth : AWeight, new()
        where FieldCommandArgs : AFieldCommandArgs<TVertex, TWeigth>
    {
        protected FieldCommandArgs args;
        public AFieldCommand() { }
        public AFieldCommand(FieldCommandArgs args)
        {
            this.args = args;
        }
    }
}
