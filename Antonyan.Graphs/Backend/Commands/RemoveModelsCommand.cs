
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{

    public class RemoveModelsCommandArgs : ACommandArgs
    {
        public RemoveModelsCommandArgs(List<GraphModel> models)
            : base(nameof(RemoveModelsCommand))
        {
            Models = models;
        }

        public List<GraphModel> Models { get; private set; }
    }
    public class RemoveModelsCommand : AFieldCommand, IStoredCommand
    {
        private RemoveModelsCommandArgs _args;
        private List<AEdgeModel> _edges;
        private List<AVertexModel> _vertices;
        private bool _exec;
        public RemoveModelsCommand(IModelField field)
            : base(field)
        {

        }
        public RemoveModelsCommand(RemoveModelsCommandArgs args, IModelField field)
            : base(field)
        {
            _exec = false;
            _args = args;
            _edges = new List<AEdgeModel>();
            _vertices = new List<AVertexModel>();
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new RemoveModelsCommand((RemoveModelsCommandArgs)args, Field);
        }

        public void Execute()
        {
            var removeModels = Field.RemoveGraphModels(_args.Models);
            if (!_exec)
            {
                _exec = true;
                removeModels.ForEach(m =>
                {
                    var v = m as AVertexModel;
                    if (v != null)
                        _vertices.Add(v);
                    else _edges.Add((AEdgeModel)m);
                });
            }
        }

        public void Undo()
        {
            _vertices.ForEach(v => Field.AddVertexModel(v, false));
            _edges.ForEach(e => Field.AddEdgeModel(e, false));
            Field.Refresh();
        }
    }
}
