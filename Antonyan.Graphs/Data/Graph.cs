using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Antonyan.Graphs.Util;


namespace Antonyan.Graphs.Data
{
    public interface IGraph
    {

    }
    public class VertexWeightPair<TVertex, TWeight> : IEquatable<VertexWeightPair<TVertex, TWeight>>
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public VertexWeightPair(TVertex v, TWeight w)
        {
            Vertex = v;
            Weight = w;
        }
        public TVertex Vertex { get; private set; }
        public TWeight Weight { get; private set; }
        public bool Equals(VertexWeightPair<TVertex, TWeight> other)
        {
            return Vertex.Equals(other.Vertex) && (Weight != null ? Weight.Equals(other.Weight) : true);
        }

        public override int GetHashCode()
        {
            return Vertex.GetHashCode() ^ ((Weight != null) ? Weight.GetHashCode() : 0);
        }
    }


    public enum ReturnValue { VertexExist, VertexDontExist, EdgeExist, EdgeDontExist, Succsess };
    public class Graph<TVertex, TWeight> : IGraph
        where TVertex : AVertex, new()
        where TWeight : AWeight, new()
    {
        public bool IsOrgraph { get; private set; }
        public bool IsWeighted { get; private set; }


        public Graph()
        {
            IsOrgraph = false;
            IsWeighted = false;
            AdjList = new SortedDictionary<TVertex, List<VertexWeightPair<TVertex, TWeight>>>();
        }
        public Graph(bool orgraph, bool weighted)
        {
            IsOrgraph = orgraph;
            IsWeighted = weighted;
            AdjList = new SortedDictionary<TVertex, List<VertexWeightPair<TVertex, TWeight>>>();
        }

        public Graph(Stream stream)
        {
            using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
            {
                ReadInText(sr.ReadToEnd());
            }
        }
        public Graph(string text)
        {
            ReadInText(text);
        }

        private void ReadInText(string text)
        {
            text.Replace('\r', ' ');
            string[] lines = text.Replace('\r', ' ').Split('\n');

            IsOrgraph = (lines.Length >= 1 && lines[0].TrimEnd(' ').ToLower() == "orgraph") ? true : false;
            IsWeighted = (lines.Length >= 2 && lines[1].TrimEnd(' ').ToLower() == "weighted") ? true : false;
            AdjList = new SortedDictionary<TVertex, List<VertexWeightPair<TVertex, TWeight>>>();
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

        public string GetAdjListToString()
        {
            StringBuilder res = new StringBuilder();
            res.AppendLine(IsOrgraph ? "orgraph" : "graph");
            res.AppendLine(IsWeighted ? "weighted" : "notweighted");
            foreach (var adjs in AdjList)
            {
                res.Append(adjs.Key.ToString() + " ");
                foreach (var pair in adjs.Value)
                {
                    res.Append(pair.Vertex.ToString());
                    if (IsWeighted)
                        res.Append("/" + pair.Weight.ToString());
                    res.Append(" ");
                }
                res.AppendLine();
            }
            return res.ToString();
        }
        public void SaveGraphToFile(StreamWriter sw)
        {
            sw.Write(GetAdjListToString());
        }


        public Graph(Graph<TVertex, TWeight> other)
        {
            AdjList = new SortedDictionary<TVertex, List<VertexWeightPair<TVertex, TWeight>>>();
            foreach (var pair in other.AdjList)
            {
                AdjList.Add(pair.Key, new List<VertexWeightPair<TVertex, TWeight>>());
                foreach (var adj in pair.Value)
                    AdjList[pair.Key].Add(new VertexWeightPair<TVertex, TWeight>(adj.Vertex, adj.Weight));
            }
            IsWeighted = other.IsWeighted;
            IsOrgraph = other.IsOrgraph;
        }


        public int Count { get { return AdjList.Count; } }
        public SortedDictionary<TVertex, List<VertexWeightPair<TVertex, TWeight>>> AdjList { get; private set; }

        public List<VertexWeightPair<TVertex, TWeight>> this[TVertex v]
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
            if (AdjList.ContainsKey(v))
                return ReturnValue.VertexExist;
            AdjList.Add(v, new List<VertexWeightPair<TVertex, TWeight>>());
            return ReturnValue.Succsess;
        }
        public ReturnValue AddEdge(TVertex v, TVertex e, TWeight w = null)
        {
            if (!IsWeighted) w = null;
            bool find_v = AdjList.ContainsKey(v);
            bool find_e = AdjList.ContainsKey(e);
            if (!find_v || !find_e)
                return ReturnValue.VertexDontExist;
            foreach (var adj in AdjList[v])
            {
                if (adj.Vertex.Equals(e))
                    return ReturnValue.EdgeExist;
            }
            if (IsOrgraph)
            {
                AdjList[v].Add(new VertexWeightPair<TVertex, TWeight>(e, w));
                return ReturnValue.Succsess;
            }
            else
            {
                AdjList[v].Add(new VertexWeightPair<TVertex, TWeight>(e, w));
                if (!v.Equals(e)) // если не петля
                    AdjList[e].Add(new VertexWeightPair<TVertex, TWeight>(v, w));
                return ReturnValue.Succsess;
            }

        }

        // List<int> hashcode for edges
        public ReturnValue RemoveVertex(TVertex v)
        {
            if (!AdjList.ContainsKey(v))
            {
                return ReturnValue.VertexDontExist;
            }
            AdjList.Remove(v);
            foreach (var adjs in AdjList)
                foreach (var pair in adjs.Value)
                {
                    if (pair.Vertex.Equals(v))
                    {
                        adjs.Value.Remove(pair);
                        break;
                    }
                }
            return ReturnValue.Succsess;
        }
        public ReturnValue RemoveEdge(TVertex v, TVertex e)
        {
            bool find_v = AdjList.ContainsKey(v);
            bool find_e = AdjList.ContainsKey(e);
            if (!find_v || !find_e)
            {
                return ReturnValue.VertexDontExist;
            }
            ReturnValue value = ReturnValue.EdgeDontExist;
            foreach (var adj in AdjList[v])
                if (adj.Vertex.Equals(e))
                {
                    value = ReturnValue.EdgeExist;
                    AdjList[v].Remove(adj);
                    break;
                }
            if (value == ReturnValue.EdgeDontExist)
            {
                return value;
            }
            if (!IsOrgraph)
            {
                foreach (var adj in AdjList[e])
                    if (adj.Vertex.Equals(v))
                    {
                        AdjList[e].Remove(adj);
                        break;
                    }
            }
            return ReturnValue.Succsess;
        }


        public int GetHalfLifeDegree(TVertex v)
        {
            return AdjList[v].Count;
        }


        public Graph<TVertex, TWeight> Union(Graph<TVertex, TWeight> other)
        {
            if (IsOrgraph != other.IsOrgraph || IsWeighted != other.IsWeighted)
                throw new Exception("Нельзя соединить разные по свойству графы");
            var union = new Graph<TVertex, TWeight>(other);
            var pats = new SortedDictionary<TVertex, bool>();

            var tmp = new SortedDictionary<TVertex, List<VertexWeightPair<TVertex, TWeight>>>();


            AdjList.ToList().ForEach(v =>
            {
                if (union.AdjList.ContainsKey(v.Key))
                {
                    tmp[v.Key] = v.Value.Union(union.AdjList[v.Key]).Distinct().ToList();
                    pats[v.Key] = true;
                    union.AdjList.Remove(v.Key);
                }
                else
                    pats[v.Key] = false;
            });
            AdjList.ToList().ForEach(v =>
            {
                if (!pats[v.Key])
                    tmp.Add(v.Key, v.Value);
            });
            union.AdjList.ToList().ForEach(v => tmp.Add(v.Key, v.Value));

            var res = new Graph<TVertex, TWeight>(IsOrgraph, IsWeighted);
            res.AdjList = tmp;
            return res;
        }

        public Graph<TVertex, TWeight> Transpose()
        {
            if (!IsOrgraph) return new Graph<TVertex, TWeight>(this);
            var trans = new Graph<TVertex, TWeight>(true, IsWeighted);
            AdjList.Keys.ToList().ForEach(v => trans.AddVertex(v));
            AdjList.ToList().ForEach(pair =>
            {
                pair.Value.ForEach(adj =>
                {
                    trans.AddEdge(adj.Vertex, pair.Key, adj.Weight);
                });
            });
            return trans;
        }

    }
}
