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
            Field<Vertex, Weight> field = new Field<Vertex, Weight>(false, false, GUI);
            CommandDispetcher cd = new CommandDispetcher(GUI);
           // UserInterface ui = new UI();
           // Observer ob = new Observer(ui);
           // ui.PassCommand("Hello");
            
            Application.Run(GUI);
        }
    }
}
