using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Util;

namespace Antonyan.Graphs.Backend
{
    public class CommandRepository
    {
        public static SortedDictionary<string, ICommand> repos = new SortedDictionary<string, ICommand>();
        private static CommandRepository instance;
        public static CommandRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    repos = new SortedDictionary<string, ICommand>();
                    instance = new CommandRepository();
                    return instance;
                }
                else
                    return instance;
            }
        }
        public static void AddCommand(string name, ICommand command)
        {
            if (!repos.ContainsKey(name))
                repos.Add(name, command);
        }
        public static void ReomoveCommand(string name)
        {
            if (repos.ContainsKey(name))
                repos.Remove(name);
        }
        public static ICommand AllocateCommand(string name, ACommandArgs args)
        {
            ICommand command;
            repos.TryGetValue(name, out command);
            if (command != null)
                return command.Clone(args);
            else return null;
        }
    }
}
