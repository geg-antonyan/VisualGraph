using Antonyan.Graphs.Board;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class SaveAlgorithmResultCommandArgs : ACommandArgs
    {
        public Stream Stream { get; private set; }
        public string ResultText { get; private set; }
        public SaveAlgorithmResultCommandArgs(Stream stream, string result)
            : base(nameof(SaveAlgorithmResultCommand))
        {
            Stream = stream;
            ResultText = result;
        }
    }

    public class SaveAlgorithmResultCommand : AFieldCommand, INonStoredCommand
    {
        private readonly SaveAlgorithmResultCommandArgs _args;
        public SaveAlgorithmResultCommand(IModelField field)
            : base(field)
        { }
        public SaveAlgorithmResultCommand(SaveAlgorithmResultCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new SaveAlgorithmResultCommand((SaveAlgorithmResultCommandArgs)args, Field);
        }

        public void Execute()
        {
            using (var wr = new StreamWriter(_args.Stream, Encoding.UTF8))
            {
                wr.WriteLine(Field.GetAdjListToString());
                wr.WriteLine(_args.ResultText);
            }
        }
    }
}
