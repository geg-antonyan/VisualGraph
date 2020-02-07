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
        public void Undo()
        {
            if (undoStore.Count == 0) return;
            ICommand cmd = undoStore.Pop();
            cmd.Undo();
            redoStore.Push(cmd);
        }
        public void Redo()
        {
            if (redoStore.Count == 0) return;
            ICommand cmd = redoStore.Pop();
            cmd.Execute();
            undoStore.Push(cmd);
        }
        public void CommandExecute(ICommand command)
        {
            command.Execute();
            undoStore.Push(command);
            if (!(redoStore.Count == 0))
                redoStore.Clear();
        }
    }
}
