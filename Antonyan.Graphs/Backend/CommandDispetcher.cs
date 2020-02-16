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
                    field = new Field<TVertex, TWeight>(oriented, weighted, ui);
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
            else if (cmdName == AddEdgeCommand<TVertex, TWeight>.Name)
            {
                if (splits.Length < 3 || splits.Length > 4)
                    throw new Exception($"Некорректная количество аргументов для команды {AddEdgeCommand<TVertex, TWeight>.Name}");
                TVertex source = new TVertex(), stock = new TVertex();
                source.SetFromString(splits[1]);
                stock.SetFromString(splits[2]);
                TWeight weight = null;
                if (splits.Length == 4)
                {
                    weight = new TWeight();
                    weight.SetFromString(splits[3]);
                }
                return CommandRepository.AllocateCommand(cmdName, new AddEdgeArgs<TVertex, TWeight>(source, stock, weight, field));
            }
            else if (cmdName == RemoveElemsCommand<TVertex, TWeight>.Name)
            {
                int edgeCount = Convert.ToInt32(splits[1]);
                Tuple<TVertex, TVertex, TWeight>[] edges = new Tuple<TVertex, TVertex, TWeight>[edgeCount];
                int n = edgeCount * 3 + 2;
                int k = 0;
                for (int i = 2; i < n; i += 3)
                {
                    TVertex source = new TVertex(), stock = new TVertex();
                    source.SetFromString(splits[i]);
                    stock.SetFromString(splits[i + 1]);
                    TWeight weight = null;
                    if (splits[i + 2] != "null")
                        weight.SetFromString(splits[i + 2]);
                    edges[k++] = new Tuple<TVertex, TVertex, TWeight>(source, stock, weight);
                }
                int circlCount = Convert.ToInt32(splits[n]);
                k = 0;
                Tuple<TVertex, vec2>[] vertices = new Tuple<TVertex, vec2>[circlCount]; 
                for (int i = n + 1; i < splits.Length; i += 3)
                {
                    TVertex vertex = new TVertex();
                    vertex.SetFromString(splits[i]);
                    bool flX, flY;
                    flX = float.TryParse(splits[i + 1], out float x);
                    flY = float.TryParse(splits[i + 2], out float y);
                    if (!flY || !flX)
                        throw new Exception($"Некорректные данные для команды {RemoveElemsCommand<TVertex, TWeight>.Name}");
                    vec2 pos = new vec2(x, y);
                    vertices[k++] = new Tuple<TVertex, vec2>(vertex, pos);
                }
                return CommandRepository.AllocateCommand(cmdName, new RemoveElemsArgs<TVertex, TWeight>(vertices, edges, field));
            }
            else throw new Exception($"Некорректная имя комманды -- \"{message}\""); ;

        }
    }
}
