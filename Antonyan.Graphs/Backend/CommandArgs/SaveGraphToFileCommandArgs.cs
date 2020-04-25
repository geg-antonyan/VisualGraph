using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.CommandArgs
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
}
