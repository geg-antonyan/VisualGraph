using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend;

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
            CommandRepository.AddCommand(RemoveElemsCommand<Vertex, Weight>.Name, new RemoveElemsCommand<Vertex, Weight>());
            CommandRepository.AddCommand(AddVertexCommand<Vertex, Weight>.Name, new AddVertexCommand<Vertex, Weight>());
            CommandRepository.AddCommand(AddEdgeCommand<Vertex, Weight>.Name, new AddEdgeCommand<Vertex, Weight>());
            _ = new Field<Vertex, Weight>(false, false, GUI);
            _ = new CommandDispetcher<Vertex, Weight>(GUI);
            Application.Run(GUI);
        }
    }
}
