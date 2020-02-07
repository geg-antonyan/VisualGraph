using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Backend.Geometry;

namespace Antonyan.Graphs.Backend
{
    public class CommandDispetcher
    {
        private readonly UserInterface ui;
        private readonly CommandManager cm;
        private readonly Field<Vertex, Weight> field;
        public CommandDispetcher(UserInterface u)
        {
            ui = u;
            field = new Field<Vertex, Weight>(false, false, ui);
            cm = new CommandManager();
            ui.CommandEnterd += CommandEntered;
        }
        private void CommandEntered(object obj, UICommandEventArgs args)
        {
            try
            {
                var cmd = GenerateCommand(args);
                
                cm.CommandExecute(cmd);
            }
            catch (Exception ex)
            {
                ui.PostMessage(ex.Message);
            }
        }

        private ICommand GenerateCommand(UICommandEventArgs args)
        {
            var message = args.Message;
            while (message.Contains("  "))
                message = message.Replace("  ", " ");
            string[] splits = message.Split(' ');
            if (splits.Length == 0) throw new Exception($"Некорректная комманда -- \"{message}\"");
            var cmdName = splits[0];
            if (cmdName == AddVertexCommand<Vertex, Weight>.Name)
            {
                if (splits.Length != 4) throw new Exception($"Некорректная количество аргументов для команды {AddVertexCommand<Vertex, Weight>.Name}");
                Vertex v = new Vertex(splits[1]);
                float x, y;
                bool succsess_x = float.TryParse(splits[2], out x);
                bool succsess_y = float.TryParse(splits[3], out y);
                if (!succsess_x || !succsess_y) throw new Exception($"Некорректные координаты --- \"{message}\"");
                return CommandRepository.AllocateCommand(cmdName, new AddVertexEventArgs<Vertex, Weight>(v, new Vec2(x, y), field));
            }
            else throw new Exception($"Некорректная имя комманды -- \"{message}\""); ;

        }
    }
}
