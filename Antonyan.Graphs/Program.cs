using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Gui;
using Antonyan.Graphs.Backend.Algorithms;

namespace Antonyan.Graphs
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            var GUI = new MainForm();
            var field = new ModelsField<Vertex, Weight>(GUI);
            GUI.AttachField(field);


            CommandRepository.AddCommand(nameof(HalfLifeDegreeCommand), new HalfLifeDegreeCommand(field));

            CommandRepository.AddCommand("SaveGraphToFileCommand", new SaveGraphToFileCommand(field));
            CommandRepository.AddCommand(nameof(OpenGraphInFileCommand), new OpenGraphInFileCommand(field));

            CommandRepository.AddCommand(nameof(AddModelCommand), new AddModelCommand(field));
            CommandRepository.AddCommand(nameof(CreateGraphCommand), new CreateGraphCommand(field));
            CommandRepository.AddCommand(nameof(MoveVertexModelCommand), new MoveVertexModelCommand(field));
            CommandRepository.AddCommand(nameof(RemoveModelsCommand), new RemoveModelsCommand(field));
            CommandRepository.AddCommand(nameof(RemoveGraphCommand), new RemoveGraphCommand(field));

            CommandRepository.AddCommand("DFSalgorithm", new DFSalgorithmCommand<Vertex, Weight>(field));
            CommandRepository.AddCommand("ShortcutBFSalgorithm", new ShortcutBFSalgorithmCommand<Vertex, Weight>(field));

            _ = new CommandDispetcher(GUI);
            Application.Run(GUI);
        }
    }
}
