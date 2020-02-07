using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Desk.Geometry;

namespace Antonyan.Graphs.Backend.Commands
{
    public class AddVertexEventArgs<TVertex> : EventArgs
        where TVertex : AVertex, new()
    {
        public TVertex Vertex { get; private set; }
        public Vec2 Coord { get; private set; }
        public AddVertexEventArgs(UICommandEventArgs args)
        {
            string[] arr = args.Message.Split(' ');
            if (arr.Length != 3)
                throw new Exception("Некорректные количесто аргументов для AddVertexEventArgs");
            TVertex v = new TVertex();
            v.SetFromString(arr[0]);
            float x, y;
            bool x_exec = float.TryParse(arr[1], out x);
            bool y_exec = float.TryParse(arr[2], out y);
            if (!x_exec || !y_exec)
                throw new Exception("Некорректные аргументы координатов для AddVertexEventArgs");
        }
    }
    public class AddVertexCommand<TVertex> : ICommand
        where TVertex : AVertex, new()
    {
        private AddVertexEventArgs<TVertex> args;

        public AddVertexCommand() { }
        public AddVertexCommand(AddVertexEventArgs<TVertex> args)
        {
            this.args = args;
        }
        public ICommand Clone(EventArgs args)
        {
            return new AddVertexCommand<TVertex>((AddVertexEventArgs<TVertex>)args);
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public string HelpMessage()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
