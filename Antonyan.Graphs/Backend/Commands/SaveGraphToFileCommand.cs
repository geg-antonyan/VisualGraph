using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Data;


using System.IO;
using System.Text;
using System;

namespace Antonyan.Graphs.Backend.Commands
{
    public class SaveGraphToFileCommand<TVertex, TWeight> : AFieldCommand, INonStoredCommand
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        private readonly SaveGraphToFileCommandArgs _args;
        public SaveGraphToFileCommand(IModelField field)
            : base(field)
        { }
        public SaveGraphToFileCommand(SaveGraphToFileCommandArgs args, IModelField field)
            : base(field)
        {
            _args = args;
        }
        public ICommand Clone(ACommandArgs args)
        {
            return new SaveGraphToFileCommand<TVertex, TWeight>((SaveGraphToFileCommandArgs)args, Field);
        }

        public void Execute()
        {
            var graph = ((ModelsField<TVertex, TWeight>)Field).Graph;
            try
            {
                using (StreamWriter sw = new StreamWriter(_args.Stream, Encoding.UTF8))
                {
                    sw.WriteLine(graph.IsOrgraph ? "orgraph" : "graph");
                    sw.WriteLine(graph.IsWeighted ? "weighted" : "notweighted");
                    foreach (var adjs in graph.AdjList)
                    {
                        sw.Write(adjs.Key.ToString() + " ");
                        foreach (var pair in adjs.Value)
                        {
                            sw.Write(pair.Item1.ToString());
                            if (graph.IsWeighted)
                                sw.Write("/" + pair.Item2.ToString());
                            sw.Write(" ");
                        }
                        sw.WriteLine();
                    }
                }

            }
            catch (Exception ex)
            {
                Field.UserInterface.PostErrorMessage(ex.Message);
            }
            finally
            {
                _args.Stream.Close();
            }
            Field.UserInterface.PostMessage("Файл успешно сохранен");
        }
    }
}
