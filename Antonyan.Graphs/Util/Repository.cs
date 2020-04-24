using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Util
{
    public interface RepositoryArgs
    {

    }
    public interface RepositoryItem
    {
        RepositoryItem Clone(RepositoryArgs args);
    }


    public class Repository
    {
        private SortedDictionary<string, RepositoryItem> repos;

        public Repository() => repos = new SortedDictionary<string, RepositoryItem>();
        public void AddItem(string name, RepositoryItem item)
        {
            if (!repos.ContainsKey(name))
                repos.Add(name, item);
        }
        public void ReomoveItem(string name)
        {
            if (repos.ContainsKey(name))
                repos.Remove(name);
        }
        public RepositoryItem GetItemWithArgs(string name, RepositoryArgs args)
        {
            RepositoryItem item;
            repos.TryGetValue(name, out item);
            if (item != null)
                return item.Clone(args);
            else return null;
        }

    }
}
