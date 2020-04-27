using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antonyan.Graphs.Backend.Algorithms;
using Antonyan.Graphs.Board;

namespace Antonyan.Graphs.Backend
{
    public class CommandDispetcher
    {
        private readonly UserInterface _ui;
        private readonly CommandManager _cm;
        public CommandDispetcher(UserInterface ui)
        {
            _ui = ui;
            _cm = new CommandManager();
            ui.CommandEntered += CommandEntered;
        }
        private void CommandEntered(object obj, ACommandArgs args)
        {
            try
            {
                bool[] undoRedoPossible = new bool[2];
                if (args.CommandName == "undo")
                {
                    _cm.Undo();
                }
                else if (args.CommandName == "redo")
                {
                    _cm.Redo();
                }
                else
                {
                    var cmd = CommandRepository.AllocateCommand(args.CommandName, args);
                    _cm.CommandExecute(cmd);
                }
                undoRedoPossible = _cm.CheckPosiible();
                _ui.CheckUndoRedo(undoRedoPossible[0], undoRedoPossible[1]);
            }
            catch (Exception ex)
            {
                _ui.PostErrorMessage(ex.Message);
            }
        }

    }
}
