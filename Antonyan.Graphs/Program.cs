using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Board;

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
            CommandRepository.AddCommand(AddVertexFieldCommand<Vertex, Weight>.Name, new AddVertexFieldCommand<Vertex, Weight>());
            CommandRepository.AddCommand(AddEdgeFieldCommand<Vertex, Weight>.Name, new AddEdgeFieldCommand<Vertex, Weight>());
            CommandRepository.AddCommand(MoveVertexFieldCommand<Vertex, Weight>.Name, new MoveVertexFieldCommand<Vertex, Weight>());
            _ = new GraphModelsField<Vertex, Weight>(false, false, GUI);
            _ = new CommandDispetcher<Vertex, Weight>(GUI);
            Application.Run(GUI);
        }
    }
}
