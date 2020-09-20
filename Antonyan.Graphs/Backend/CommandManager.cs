using Antonyan.Graphs.Backend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Antonyan.Graphs.Backend
{
    public class CommandManager
    {
        private readonly Stack<ICommand> undoStore, redoStore;

        public CommandManager()
        {
            undoStore = new Stack<ICommand>();
            redoStore = new Stack<ICommand>();
        }

        public bool[] CheckPosiible()
        {
            bool[] res = new bool[2];
            res[0] = undoStore.Count > 0 ? true : false;
            res[1] = redoStore.Count > 0 ? true : false;
            return res;
        }
        public bool[] Undo()
        {

            if (undoStore.Count > 0)
            {
                IStoredCommand cmd = (IStoredCommand)undoStore.Pop();
                cmd.Undo();
                redoStore.Push(cmd);
            }
            return CheckPosiible();
        }
        public bool[] Redo()
        {
            if (redoStore.Count > 0)
            {
                ICommand cmd = redoStore.Pop();
                cmd.Execute();
                undoStore.Push(cmd);
            }
            return CheckPosiible();
        }
        public bool[] CommandExecute(ICommand command)
        {
            command.Execute();
            if (command is CreateGraphCommand || command is RemoveGraphCommand || command is OpenGraphInFileCommand)
            {
                undoStore.Clear();
                redoStore.Clear();
            }    
            if (command is IStoredCommand)
            {
                undoStore.Push(command);
                if (!(redoStore.Count == 0))
                    redoStore.Clear();
            }
            return CheckPosiible();
        }
    }
}
