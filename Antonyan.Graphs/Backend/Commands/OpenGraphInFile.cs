using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;

namespace Antonyan.Graphs.Backend.Commands
{
    public class OpenGraphInFileCommandArgs : ACommandArgs
    {
        public Stream Stream { get; private set; }
        public OpenGraphInFileCommandArgs(Stream stream)
            : base(nameof(OpenGraphInFileCommand))
        {
            Stream = stream;
        }
    }

    public class OpenGraphInFileCommand : AFieldCommand, INonStoredCommand
    {
        private readonly OpenGraphInFileCommandArgs _args;
        public OpenGraphInFileCommand(IModelField field)
            : base(field)
        { }
        public OpenGraphInFileCommand(OpenGraphInFileCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new OpenGraphInFileCommand((OpenGraphInFileCommandArgs)args, Field);
        }

        public void Execute()
        {
            string graphDataText;
            List<GraphModel> models = new List<GraphModel>();
            using (StreamReader reader = new StreamReader(_args.Stream, Encoding.UTF8))
            {
                string line = reader.ReadLine();
                string[] split = line.Split(' ');
                bool orgraph = split[1] == "graph" ? false : true;
                bool weighted = split[2] == "nonweighted" ? false : true;
                List<string> edgsStrings = new List<string>();
                while ((line = reader.ReadLine()) != null && line != "graphmodel end")
                {
                    string[] splitLine = line.Split(' ');
                    if (splitLine.Length == 0)
                        continue;
                    else if (splitLine[0] == "[v]")
                    {
                        var flX = float.TryParse(splitLine[2], out var x);
                        var flY = float.TryParse(splitLine[3], out var y);
                        if (!flY || !flX)
                            throw new Exception("Некорректный формат данных");
                        models.Add(new VertexDrawModel(splitLine[1], new vec2(x, y)));
                    }
                    else if (splitLine[0] == "[e]")
                    {
                        edgsStrings.Add(line);
                    }
                    else
                    {
                        throw new Exception("Некорректный формат данных");
                    }
                }
                edgsStrings.ForEach(str =>
                {
                    string[] sp = str.Split(' ');
                    var src = sp[1];
                    var stc = sp[2];
                    var w = sp.Length == 4 ? sp[3] : null;
                    var source = models.Find(v => v.Key == src);
                    var stock = models.Find(v => v.Key == stc);
                    if (source == null || stock == null)
                        throw new Exception("Некорректный формат данных");
                    if (orgraph)
                        models.Add(new OrientEdgeModel((AVertexModel)source, (AVertexModel)stock, w));
                    else models.Add(new NonOrientEdgeModel((AVertexModel)source, (AVertexModel)stock, w));
                });
                graphDataText = reader.ReadToEnd();
            }
            Field.OpenGraphInFile(models, graphDataText);
        }
    }
}