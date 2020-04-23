using Antonyan.Graphs.Backend.UICommandArgs;
using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class RemoveModelsCommand : ICommand
    {
        public static readonly string Name = nameof(RemoveModelsCommand);
        private UIRemoveModelsArgs _args;
        private List<EdgeModel> _edgeModel;
        private List<VertexModel> _vertexModels;
        private bool _executed;
        public RemoveModelsCommand() { }
        public RemoveModelsCommand(UIRemoveModelsArgs args)
        {
            _executed = false;
            _args = args;
            _edgeModel = new List<EdgeModel>();
            _vertexModels = new List<VertexModel>();
        }
        public ICommand Clone(UIEventArgs args)
        {
            return new RemoveModelsCommand((UIRemoveModelsArgs)args);
        }

        public void Execute()
        {
            var removes = _args.Field.RemoveGraphModels(_args.Models, true);
            if (!_executed)
            {
                _executed = true;
                removes.ForEach(elem =>
                {
                    var m = elem as VertexModel;
                    if (m != null) _vertexModels.Add(m);
                    else _edgeModel.Add((EdgeModel)elem);
    
                });
            }
        }

        public void Undo()
        {
            _vertexModels.ForEach(elem => _args.Field.AddVertexModel((VertexModel)elem));
            _edgeModel.ForEach(elem => _args.Field.AddEdgeModel((EdgeModel)elem));
        }
    }
}
