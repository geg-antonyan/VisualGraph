using System;
using System.Collections.Generic;



namespace Antonyan.Graphs.Data
{
    public class RemoveVertexReturnValues<TVertex, TWeight> : EventArgs
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public void SetReturnValue(Graph<TVertex, TWeight>.ReturnValue value) => Return = value;
        public void AddEdge(TVertex source, TVertex stock, TWeight weight, bool orented = false)
        {

            //if (orented)
            Edges.Add(new Tuple<TVertex, TVertex, TWeight>(source, stock, weight));
            // else
            //     foreach (var edge in Edges)
            //      {
            //          if (edge.Item1.Equals(source) && edge.Item2.Equals(stock) ||
            //              edge.Item1.Equals(stock) && edge.Item2.Equals(source))
            //             continue;
            //          Edges.Add(new Tuple<TVertex, TVertex, TWeight>(source, stock, weight));
            //      }
        }
        public Graph<TVertex, TWeight>.ReturnValue Return { get; private set; }
        public List<Tuple<TVertex, TVertex, TWeight>> Edges { get; private set; } = new List<Tuple<TVertex, TVertex, TWeight>>();
    }

    public class Graph<TVertex, TWeight>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public enum ReturnValue { VertexExist, VertexDontExist, EdgeExist, EdgeDontExist, Succsess };
        private readonly SortedDictionary<TVertex, List<Tuple<TVertex, TWeight>>> data;
        public bool IsOrgraph { get; private set; }
        public bool IsWeighted { get; private set; }


        public Graph()
        {
            IsOrgraph = false;
            IsWeighted = false;
            data = new SortedDictionary<TVertex, List<Tuple<TVertex, TWeight>>>();
        }
        public Graph(bool orgraph, bool weighted)
        {
            IsOrgraph = orgraph;
            IsWeighted = weighted;
            data = new SortedDictionary<TVertex, List<Tuple<TVertex, TWeight>>>();
        }
        public Graph(string text)
        {
            text.Replace('\r', ' ');
            string[] lines = text.Replace('\r', ' ').Split('\n');

            IsOrgraph = (lines.Length >= 1 && lines[0].TrimEnd(' ').ToLower() == "orgraph") ? true : false;
            IsWeighted = (lines.Length >= 2 && lines[1].TrimEnd(' ').ToLower() == "weighted") ? true : false;
            data = new SortedDictionary<TVertex, List<Tuple<TVertex, TWeight>>>();
            for (int i = 2; i < lines.Length; i++)
            {
                if (lines[i].Length == 0) continue;
                while (lines[i].Contains("  "))
                    lines[i] = lines[i].Replace("  ", " ");
                lines[i] = lines[i].Trim(' ');
                int k = lines[i].IndexOf(' ');
                TVertex v = new TVertex();
                v.SetFromString(lines[i].Substring(0, k == -1 ? lines[i].Length : k));
                AddVertex(v);
            }
            for (int i = 2; i < lines.Length; i++)
            {
                if (lines[i].Length == 0) continue;
                string[] elem = lines[i].Split(' ');
                TVertex v = new TVertex();
                v.SetFromString(elem[0]);
                for (int j = 1; j < elem.Length; j++)
                {
                    var e = new TVertex();
                    var w = new TWeight();
                    if (IsWeighted)
                    {
                        string[] pair = elem[j].Split('/');
                        e.SetFromString(pair[0]);
                        w.SetFromString(pair[1]);
                    }
                    else e.SetFromString(elem[j]);
                    AddEdge(v, e, w);
                }
            }
        }

        public TWeight WeightEdge(TVertex source, TVertex stock)
        {
            return null;
        }
        public Graph(Graph<TVertex, TWeight> other)
        {
            data = new SortedDictionary<TVertex, List<Tuple<TVertex, TWeight>>>();
            foreach (var pair in other.data)
            {
                data.Add(pair.Key, new List<Tuple<TVertex, TWeight>>());
                foreach (var adj in pair.Value)
                    data[pair.Key].Add(new Tuple<TVertex, TWeight>(adj.Item1, adj.Item2));
            }
        }


        public int Count { get { return AdjList.Count; } }
        public SortedDictionary<TVertex, List<Tuple<TVertex, TWeight>>> AdjList
        {
            get { return data;/*return new Graph<TVertex, TWeight>(this).data;*/ }
        }

        public List<Tuple<TVertex, TWeight>> this[TVertex v]
        {
            get
            {
                return AdjList[v];
            }
            set
            {
                AdjList[v] = value;
            }
        }
        public ReturnValue AddVertex(TVertex v)
        {
            if (data.ContainsKey(v))
                return ReturnValue.VertexExist;
            data.Add(v, new List<Tuple<TVertex, TWeight>>());
            return ReturnValue.Succsess;
        }
        public ReturnValue AddEdge(TVertex v, TVertex e, TWeight w)
        {
            if (!IsWeighted) w = null;
            bool find_v = data.ContainsKey(v);
            bool find_e = data.ContainsKey(e);
            if (!find_v || !find_e)
                return ReturnValue.VertexDontExist;
            foreach (var adj in data[v])
            {
                if (adj.Item1.Equals(e))
                    return ReturnValue.EdgeExist;
            }
            if (IsOrgraph)
            {
                data[v].Add(new Tuple<TVertex, TWeight>(e, w));
                return ReturnValue.Succsess;
            }
            else
            {
                data[v].Add(new Tuple<TVertex, TWeight>(e, w));
                if (!v.Equals(e)) // если не петля
                    data[e].Add(new Tuple<TVertex, TWeight>(v, w));
                return ReturnValue.Succsess;
            }

        }
        public RemoveVertexReturnValues<TVertex, TWeight> RemoveVertex(TVertex v)
        {
            var res = new RemoveVertexReturnValues<TVertex, TWeight>();
            if (!data.ContainsKey(v))
            {
                res.SetReturnValue(ReturnValue.VertexDontExist);
                return res;
            }
            foreach (var adj in this[v])
                res.AddEdge(v, adj.Item1, adj.Item2);
            data.Remove(v);
            res.SetReturnValue(ReturnValue.Succsess);
            foreach (var adjs in data)
                foreach (var pair in adjs.Value)
                {
                    if (pair.Item1.Equals(v))
                    {
                        if (IsOrgraph)
                        {
                            res.AddEdge(pair.Item1, v, pair.Item2);
                        }
                        adjs.Value.Remove(pair);
                        break;
                    }
                }
            return res;
        }
        public ReturnValue RemoveEdge(TVertex v, TVertex e, out TWeight w)
        {
            w = null;
            bool find_v = data.ContainsKey(v);
            bool find_e = data.ContainsKey(e);
            if (!find_v || !find_e)
            {
                return ReturnValue.VertexDontExist;
            }
            ReturnValue value = ReturnValue.EdgeDontExist;
            foreach (var adj in data[v])
                if (adj.Item1.Equals(e))
                {
                    value = ReturnValue.EdgeExist;
                    w = adj.Item2;
                    data[v].Remove(adj);
                    break;
                }
            if (value == ReturnValue.EdgeDontExist)
            {
                return value;
            }
            if (!IsOrgraph)
            {
                foreach (var adj in data[e])
                    if (adj.Item1.Equals(v))
                    {
                        data[e].Remove(adj);
                        break;
                    }
            }
            return ReturnValue.Succsess;
        }
    }
}
