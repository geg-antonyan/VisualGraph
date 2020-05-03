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

            //var g = new Graph<Vertex, Weight>(false, false);
            //var v1 = new Vertex("1");
            //var v2 = new Vertex("2");
            //var v3 = new Vertex("3");
            //var v4 = new Vertex("4");
            //var v5 = new Vertex("5");
            //g.AddVertex(v1);
            //g.AddVertex(v2);
            //g.AddVertex(v3);
            //g.AddVertex(v4);
            //g.AddVertex(v5);

            //g.AddEdge(v1, v3);
            //g.AddEdge(v1, v2);
            //g.AddEdge(v4, v5);
            //var res = g.Transpose();
            //ConnectedComponentsAlgorithmCommand<Vertex, Weight> cm = new ConnectedComponentsAlgorithmCommand<Vertex, Weight>(null, null);
           // cm.Execute(g);

            //a.AddVertex(new Vertex("0"));
            //a.AddVertex(new Vertex("1"));
            //a.AddVertex(new Vertex("2"));
            //a.AddEdge(new Vertex("1"), new Vertex("0")); 
            //a.AddEdge(new Vertex("1"), new Vertex("2"));
            //b.AddVertex(new Vertex("1"));
            //b.AddVertex(new Vertex("2"));
            //b.AddVertex(new Vertex("3"));
            //b.AddEdge(new Vertex("1"), new Vertex("2"));
            //b.AddEdge(new Vertex("2"), new Vertex("1"));
            //b.AddEdge(new Vertex("1"), new Vertex("3"));
            //a.Union(b);

            var GUI = new MainForm();
            var field = new ModelsField<Vertex, Weight>(GUI);
            GUI.AttachField(field);


            CommandRepository.AddCommand(nameof(HalfLifeDegreeCommand), new HalfLifeDegreeCommand(field));
            CommandRepository.AddCommand(nameof(AddCurrentGraphInStoredGraphsCommand), new AddCurrentGraphInStoredGraphsCommand(field));
            CommandRepository.AddCommand(nameof(GraphsUnionCommand), new GraphsUnionCommand(field));
            CommandRepository.AddCommand(nameof(RemoveStoredGraphCommand), new RemoveStoredGraphCommand(field));
            CommandRepository.AddCommand("ConnectedComponentsCommand", new ConnectedComponentsCommand<Vertex, Weight>(field));

            CommandRepository.AddCommand(nameof(SaveGraphToFileCommand), new SaveGraphToFileCommand(field));
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
