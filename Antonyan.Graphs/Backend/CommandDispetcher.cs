using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Backend.Algorithms;
using Antonyan.Graphs.Board;

namespace Antonyan.Graphs.Backend
{
    public class CommandDispetcher<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private readonly UserInterface ui;
        private readonly CommandManager cm;
        private GraphModelsField<TVertex, TWeight> field;
        public CommandDispetcher(UserInterface u)
        {
            ui = u;
            cm = new CommandManager();
            ui.CommandEntered += CommandEntered;
        }
        private void CommandEntered(object obj, UICommandEventArgs args)
        {
            try
            {

                bool[] undoRedoPossible;
                var splits = args.Message.Split(' ');
                if (splits[0].ToLower() == "algorithm")
                {
                    AlgorithmExecute(splits);
                    undoRedoPossible = cm.CheckPosiible();
                }
                
                else if (splits[0] == "CreateField")
                {
                    if (splits.Length != 3)
                        throw new Exception($"Некорректная количество аргументов для команды  {splits[0]}");
                    bool oriented = splits[1].ToLower() == "oriented" ? true : false;
                    bool weighted = splits[2].ToLower() == "weighted" ? true : false;
                    field = new GraphModelsField<TVertex, TWeight>(oriented, weighted, ui);
                    ui.SetFieldStatus(true);
                    undoRedoPossible = cm.CheckPosiible();
                }
                else if (args.Message.ToLower() == "undo")
                    undoRedoPossible = cm.Undo();
                else if (args.Message.ToLower() == "redo")
                    undoRedoPossible = cm.Redo();
                else
                {
                    var cmd = GenerateCommand(args);
                    undoRedoPossible = cm.CommandExecute(cmd);
                }
                ui.CheckUndoRedo(undoRedoPossible[0], undoRedoPossible[1]);
            }
            catch (Exception ex)
            {
                ui.PostMessage(ex.Message);
            }
        }

        private void AlgorithmExecute(string[] arrCommand)
        {
            if (arrCommand[1].ToLower() == "dfs" || arrCommand[1].ToLower() == "bfs")
            {
                if (arrCommand.Length != 3)
                    throw new Exception($"Некорректная комманда -- \"{arrCommand}\"");
                ui.UnmarkAll();
                TVertex vertex = new TVertex();
                SortedDictionary<TVertex, bool> visited = new SortedDictionary<TVertex, bool>();
                foreach (var v in field.Graph.AdjList)
                    visited[v.Key] = false;
                vertex.SetFromString(arrCommand[2]);
                if (arrCommand[1] == "dfs")
                    Detours<TVertex, TWeight>.DFS(field.Graph, vertex, visited, ui);
                else Detours<TVertex, TWeight>.BFS(field.Graph, vertex, visited, ui);
            }
            else if (arrCommand[1].ToLower() == "shortcatdfs")
            {
                if (arrCommand.Length != 4)
                    throw new Exception($"Некорректная комманда -- \"{arrCommand}\"");
                TVertex source = new TVertex(), stock = new TVertex();
                source.SetFromString(arrCommand[2]);
                stock.SetFromString(arrCommand[3]);
                Shortcats<TVertex, TWeight>.ShortcutBFS(field.Graph, source, stock, ui);
            }
            else
            {
                throw new Exception($"Некорректная комманда -- \"{arrCommand}\"");
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
            if (field == null)
                throw new Exception("Граф еще не создан");
            else if (cmdName == AddVertexFieldCommand<TVertex, TWeight>.Name)
            {
                if (splits.Length != 4) throw new Exception($"Некорректная количество аргументов для команды {AddVertexFieldCommand<TVertex, TWeight>.Name}");
                TVertex v = new TVertex();
                v.SetFromString(splits[1]);
                float x, y;
                bool succsess_x = float.TryParse(splits[2], out x);
                bool succsess_y = float.TryParse(splits[3], out y);
                if (!succsess_x || !succsess_y) throw new Exception($"Некорректные координаты --- \"{message}\"");
                return CommandRepository.AllocateCommand(cmdName, new VertexFieldCommandArgs<TVertex, TWeight>(field, v, new vec2(x, y)));
            }
            else if (cmdName == AddEdgeFieldCommand<TVertex, TWeight>.Name)
            {
                if (splits.Length < 3 || splits.Length > 4)
                    throw new Exception($"Некорректная количество аргументов для команды {AddEdgeFieldCommand<TVertex, TWeight>.Name}");
                TVertex source = new TVertex(), stock = new TVertex();
                source.SetFromString(splits[1]);
                stock.SetFromString(splits[2]);
                TWeight weight = null;
                if (splits.Length == 4)
                {
                    weight = new TWeight();
                    weight.SetFromString(splits[3]);
                }
                return CommandRepository.AllocateCommand(cmdName, new EdgeFieldCommandArgs<TVertex, TWeight>(field, source, stock, weight));
            }
            else if (cmdName == MoveVertexFieldCommand<TVertex, TWeight>.Name)
            {
                if (splits.Length != 6)
                    throw new Exception($"Некорректные данные для команды { MoveVertexFieldCommand<TVertex, TWeight>.Name}");
                string represent = splits[1];
                bool flX1 = float.TryParse(splits[2], out float x1);
                bool flY1 = float.TryParse(splits[3], out float y1);
                bool flX2 = float.TryParse(splits[4], out float x2);
                bool flY2 = float.TryParse(splits[5], out float y2);
                if (!flX1 || !flX2 || !flY1 || !flY2)
                    throw new Exception($"Некорректные данные для команды { MoveVertexFieldCommand<TVertex, TWeight>.Name}");
                vec2 pos = new vec2(x1, y1);
                vec2 newPos = new vec2(x2, y2);
                return CommandRepository.AllocateCommand(cmdName, new MoveVertexFieldCommandArgs<TVertex, TWeight>(field, represent, pos, newPos));
            }
            else throw new Exception($"Некорректная имя комманды -- \"{message}\""); ;

        }
    }
}
