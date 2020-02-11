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
            CommandRepository.AddCommand(AddVertexCommand<Vertex, Weight>.Name, new AddVertexCommand<Vertex, Weight>());
            CommandRepository.AddCommand(CreateFieldCommand<Vertex, Weight>.Name, new CreateFieldCommand<Vertex, Weight>());
            var field = new Field<Vertex, Weight>(false, false, GUI);
            CommandDispetcher<Vertex, Weight> cd = new CommandDispetcher<Vertex, Weight>(GUI);

            Application.Run(GUI);
        }
    }
}
