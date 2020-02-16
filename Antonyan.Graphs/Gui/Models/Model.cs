using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs;

namespace Antonyan.Graphs.Gui.Models
{
    public interface DrawModel
    {
        void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max);
    }

    public class Model
    {
        public Model(DrawModel model, bool mark = false)
        {
            DrawModel = model;
            Marked = mark;
        }
        public DrawModel DrawModel { get; private set; }
        public bool Marked { get; set; }
    }

    public class Models
    {
        private SortedDictionary<int, Model> models;
       
        private Pen markCirclePen;
        private Pen unmarkCirclePen;
        private Pen markEdgePen;
        private Pen unmarkEdgePen;

        private Font markVertexFont;
        private Font unmarkVertexFont;
        private Font markWeightFont;
        private Font unmarkWeightFont;

        private Brush markVertexBrush;
        private Brush unmarkVertexBrush;
        private Brush markWeightBrush;
        private Brush unmarkWeightBrush;

        public event EventHandler<EventArgs> Update;

        public int MarkedCircleCount { get; private set; } = 0;
        public int MarkedEdgeCount { get; private set; } = 0;
        public Models(MainForm form, Pen mcp, Pen umcp, Pen me, Pen ume,
            Font mv, Font umv, Font mw, Font umw,
            Brush mvb, Brush umvb, Brush mwb, Brush umwb)
        {
            Update += form.ModelsUpdate;
            markCirclePen = mcp; unmarkCirclePen = umcp;
            markEdgePen = me; unmarkEdgePen = ume;
            markVertexFont = mv; unmarkVertexFont = umv;
            markWeightFont = mw; unmarkWeightFont = umw;
            markVertexBrush = mvb; unmarkVertexBrush = umvb;
            markWeightBrush = mwb; unmarkWeightBrush = umwb;
            models = new SortedDictionary<int, Model>();
        }

        public List<Model> GetMarkedModels()
        {
            List<Model> res = new List<Model>();
            foreach (var m in models)
                if (m.Value.Marked)
                    res.Add(m.Value);
            return res;

        }
        public string GetVertex(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
            {
                if (model.DrawModel is Circle)
                    return ((Circle)model.DrawModel).Vertex;
            }
            return null;
        }
        public void AddDrawModel(int hashCode, DrawModel drawModel)
        {
            models.Add(hashCode, new Model(drawModel));
            Update?.Invoke(this, null);
        }

        public void RemoveDrawModel(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
            {
                if (model.Marked)
                {
                    if (model.DrawModel is Circle)
                        MarkedCircleCount--;
                    else if (model.DrawModel is Edge)
                        MarkedEdgeCount--;
                }

                models.Remove(hashCode);
                Update?.Invoke(this, null);
            }
            else throw new Exception("Некорректный хеш код");
        }

        public bool Mark(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
            {
                if (model.Marked) return false;
                    model.Marked = true;
                if (model.DrawModel is Circle)
                    MarkedCircleCount++;
                else if (model.DrawModel is Edge)
                    MarkedEdgeCount++;
                Update?.Invoke(this, null);
                return true;
            }
            return false;
        }

        public bool Unmark(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
            {
                if (!model.Marked) return false;
                    model.Marked = false;
                if (model.DrawModel is Circle)
                    MarkedCircleCount--;
                else if (model.DrawModel is Edge)
                    MarkedEdgeCount--;
                Update?.Invoke(this, null);
                return true;
            }
            return false;
        }

        public void UnmarkAll()
        {
            foreach (var m in models)
                m.Value.Marked = false;
            MarkedCircleCount = 0;
            MarkedEdgeCount = 0;
            Update?.Invoke(this, null);
        }

        public bool Marked(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
                return model.Marked;
            throw new Exception("Некорректный хеш код");
        }


        public bool Unmarked(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
                return model.Marked;
            throw new Exception("Некорректный хеш код");
        }

        public Circle[] GetMarkedCircle(int count = 0)
        {
             
            if (count == 0) count = models.Count;
            Circle[] res = new Circle[count];
            int i = 0;
            foreach (var m in models)
            {
                if (i == count) break;
                if (m.Value.DrawModel is Circle)
                {
                    res[i] = (Circle)m.Value.DrawModel;
                    i++;
                }
            }
            return res;
        }

        public int GetCircleHashCode(vec2 pos, float r)
        {
            foreach (var m in models)
            {
                if (m.Value.DrawModel is Circle)
                {
                    Circle circle = (Circle)m.Value.DrawModel;
                    vec2 c = circle.Pos;
                    if (Math.Pow(pos.x - c.x, 2.0) + Math.Pow(pos.y - c.y, 2.0) <= r * r)
                        return circle.GetHashCode();
                }
            }
            return 0;
        }

        public int GetEdgeHashCode(vec2 pos)
        {
            foreach (var m in models)
                if (m.Value.DrawModel is Edge)
                {
                    Edge edge = (Edge)(m.Value.DrawModel);
                    vec2 a = edge.SourcePos;
                    vec2 b = edge.StockPos;
                    float bigX = a.x > b.x ? a.x : b.x;
                    float bigY = a.y > b.y ? a.y : b.y;
                    float smallX = b.x < a.x ? b.x : a.x;
                    float smallY = b.y < a.y ? b.y : a.y;
                    if (pos.y > bigY + 15f || pos.y < smallY - 15f)
                        continue;
                    if (pos.x > bigX + 15f || pos.x < smallX - 15f)
                        continue;
                    float x = pos.x;
                    float y = pos.y;
                    float eps = 0.1f;
                    if (b.y - a.y == 0f) b.y += 1f;
                    if (b.x - a.x == 0f) b.x += 1f;
                    if (Math.Abs(b.y - a.y) < 30f) eps = 1.0f;
                    if (Math.Abs(b.x - a.x) < 30f) eps = 1.0f;
                    float res = ((x - a.x) / (b.x - a.x)) - ((y - a.y) / (b.y - a.y));
                    if (Math.Abs(res) <= eps)
                        return edge.GetHashCode();
                }
            return 0;
        }

        public void Draw(Graphics g, vec2 min, vec2 max)
        {
            foreach (var m in models)
            {
                Pen pen; Brush brush; Font font;
                if (m.Value.DrawModel is Circle)
                {
                    if (m.Value.Marked)
                    {
                        pen = markCirclePen;
                        brush = markVertexBrush;
                        font = markVertexFont;
                    }
                    else
                    {
                        pen = unmarkCirclePen;
                        brush = unmarkVertexBrush;
                        font = unmarkVertexFont;
                    }
                }
                else //if (m.Value.DrawModel is Edge)
                {

                    if (m.Value.Marked)
                    {
                        pen = markEdgePen;
                        brush = markWeightBrush;
                        font = markWeightFont;
                    }
                    else
                    {
                        pen = unmarkEdgePen;
                        brush = unmarkWeightBrush;
                        font = unmarkWeightFont;
                    }
                }
                m.Value.DrawModel.Draw(g, pen, brush, font, min, max);
            }
        }

    }

}
