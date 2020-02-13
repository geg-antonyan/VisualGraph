using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Backend
{
    public class CommandDispetcher<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private readonly UserInterface ui;
        private readonly CommandManager cm;
        private Field<TVertex, TWeight> field;
        public CommandDispetcher(UserInterface u)
        {
            ui = u;
            cm = new CommandManager();
            ui.CommandEntered += CommandEntered;
        }

        public void AttachField(Field<TVertex, TWeight> fld) => field = fld;
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
            if (field == null && cmdName != CreateFieldCommand<TVertex, TWeight>.Name)
                throw new Exception("Граф еще не создан");
            else if (cmdName == AddVertexCommand<TVertex, TWeight>.Name)
            {
                if (splits.Length != 4) throw new Exception($"Некорректная количество аргументов для команды {AddVertexCommand<TVertex, TWeight>.Name}");
                TVertex v = new TVertex();
                v.SetFromString(splits[1]);
                float x, y;
                bool succsess_x = float.TryParse(splits[2], out x);
                bool succsess_y = float.TryParse(splits[3], out y);
                if (!succsess_x || !succsess_y) throw new Exception($"Некорректные координаты --- \"{message}\"");
                return CommandRepository.AllocateCommand(cmdName, new AddVertexArgs<TVertex, TWeight>(v, new vec2(x, y), field));
            }
            else if (cmdName == CreateFieldCommand<TVertex, TWeight>.Name)
            {
                if (splits.Length != 3)
                    throw new Exception($"Некорректная количество аргументов для команды {CreateFieldCommand<TVertex, TWeight>.Name}");
                bool oriented = splits[1].ToLower() == "oriented" ? true : false;
                bool weighted = splits[2].ToLower() == "weighted" ? true : false;
                return CommandRepository.AllocateCommand(cmdName, new CreatFieldArgs<TVertex, TWeight>(ui, this, oriented, weighted));
            }
            else if (cmdName == AddEdgeCommand<TVertex, TWeight>.Name)
            {
                if (splits.Length < 3 || splits.Length > 4)
                    throw new Exception($"Некорректная количество аргументов для команды {AddEdgeCommand<TVertex, TWeight>.Name}");
                TVertex source = new TVertex(), stock = new TVertex();
                source.SetFromString(splits[1]);
                stock.SetFromString(splits[2]);
                TWeight weight = new TWeight();
                if (splits.Length == 4)
                    weight.SetFromString(splits[3]);
                return CommandRepository.AllocateCommand(cmdName, new AddEdgeArgs<TVertex, TWeight>(source, stock, weight, field));
            }
            else throw new Exception($"Некорректная имя комманды -- \"{message}\""); ;

        }
    }
}
