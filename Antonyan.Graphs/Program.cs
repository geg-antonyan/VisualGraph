using System;
using System.Runtime.InteropServices;
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
            var field = new ModelsField<Vertex>(GUI);
            GUI.AttachField(field);

            CommandRepository.AddCommand(nameof(HalfLifeDegreeCommand), new HalfLifeDegreeCommand(field));
            CommandRepository.AddCommand(nameof(AddCurrentGraphInStoredGraphsCommand), new AddCurrentGraphInStoredGraphsCommand(field));
            CommandRepository.AddCommand(nameof(GraphsUnionCommand), new GraphsUnionCommand(field));
            CommandRepository.AddCommand(nameof(RemoveStoredGraphCommand), new RemoveStoredGraphCommand(field));
            CommandRepository.AddCommand(nameof(SaveGraphToFileCommand), new SaveGraphToFileCommand(field));
            CommandRepository.AddCommand(nameof(OpenGraphInFileCommand), new OpenGraphInFileCommand(field));

            CommandRepository.AddCommand(nameof(AddModelCommand), new AddModelCommand(field));
            CommandRepository.AddCommand(nameof(CreateGraphCommand), new CreateGraphCommand(field));
            CommandRepository.AddCommand(nameof(MoveVertexModelCommand), new MoveVertexModelCommand(field));
            CommandRepository.AddCommand(nameof(RemoveModelsCommand), new RemoveModelsCommand(field));
            CommandRepository.AddCommand(nameof(RemoveGraphCommand), new RemoveGraphCommand(field));
            CommandRepository.AddCommand(nameof(SaveAlgorithmResultCommand), new SaveAlgorithmResultCommand(field));

            CommandRepository.AddCommand("DFSalgorithm", new DFSalgorithmCommand<Vertex>(field));
            CommandRepository.AddCommand("ShortcutBFSalgorithm", new ShortcutBFSalgorithmCommand<Vertex>(field));
            CommandRepository.AddCommand("ConnectedComponentsCommand", new ConnectedComponentsCommand<Vertex>(field));
            CommandRepository.AddCommand("MSTCommand", new MSTCommand<Vertex>(field));
            CommandRepository.AddCommand("DijkstraAlgorithm", new DijkstraAlgorithm<Vertex>(field));
            CommandRepository.AddCommand("WayNoMoreThenLCommand", new WayNoMoreThenLCommand<Vertex>(field));
            CommandRepository.AddCommand("EdmondsKarpAlgorithm", new EdmondsKarpAlgorithm<Vertex>(field));
            CommandRepository.AddCommand("NPeripheryCommand", new NPeripheryCommand<Vertex>(field));

            _ = new CommandDispetcher(GUI);
            Application.Run(GUI);
        }
        
    }
}
