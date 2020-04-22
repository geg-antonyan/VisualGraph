using Antonyan.Graphs.Backend.UICommandArgs;
using Antonyan.Graphs.Board.Models;
namespace Antonyan.Graphs.Backend.Commands
{
    public class MoveModelCommand : ICommand
    {
        private UIMoveModelArgs _args;
        public MoveModelCommand() { }

        public static readonly string Name = nameof(MoveModelCommand);
        public MoveModelCommand(UIMoveModelArgs args)
        {
            _args = args;
        }
        public ICommand Clone(UIEventArgs args)
        {
             return new MoveModelCommand((UIMoveModelArgs)args);
        }

        public void Execute()
        {
            _args.Field.ChangeVertexModelPos(_args.Represent, _args.NewPos);
        }

        public void Undo()
        {
            _args.Field.ChangeVertexModelPos(_args.Represent, _args.LastPos);
        }
    }
}
