using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Backend.Commands
{
    public class HalfLifeDegreeCommandArgs : ACommandArgs
    {
        public HalfLifeDegreeCommandArgs(AVertexModel vertex)
            : base(nameof(HalfLifeDegreeCommand))
        {
            Vertex = vertex;
        }
        public AVertexModel Vertex { get; private set; }
        public int OutHalfLifeDegree { get; private set; }
        public void SetHalfLifeDegree(int count)
        {
            OutHalfLifeDegree = count;
        }
    }
    public class HalfLifeDegreeCommand : AFieldCommand, INonStoredCommand
    {
        private HalfLifeDegreeCommandArgs _args;
        public HalfLifeDegreeCommand(IModelField field) 
            : base(field)
        {

        }
        public HalfLifeDegreeCommand(HalfLifeDegreeCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new HalfLifeDegreeCommand((HalfLifeDegreeCommandArgs)args, Field);
        }

        public void Execute()
        {
            _args.SetHalfLifeDegree(Field.GetHalfLifeDegree(_args.Vertex));
        }
    }
}
