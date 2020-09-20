
using Antonyan.Graphs.Board;
using System.IO;

namespace Antonyan.Graphs.Backend.Commands
{
    public class SaveGraphToFileCommandArgs : ACommandArgs
    {
        public Stream Stream { get; private set; }
        public SaveGraphToFileCommandArgs(Stream stream)
            : base("SaveGraphToFileCommand")
        {
            Stream = stream;
        }
    }

    public class SaveGraphToFileCommand: AFieldCommand, INonStoredCommand
    {
        private readonly SaveGraphToFileCommandArgs _args;
        public SaveGraphToFileCommand(IModelField field)
            : base(field)
        { }
        public SaveGraphToFileCommand(SaveGraphToFileCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new SaveGraphToFileCommand((SaveGraphToFileCommandArgs)args, Field);
        }

        public void Execute()
        {
            Field.SaveGraphToFile(_args.Stream);
        }
    }
}
