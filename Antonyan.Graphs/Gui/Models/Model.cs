using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Antonyan.Graphs.Backend;

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

        public int MarkedCircleCount { get; private set; } = 0;
        public int MarkedEdgeCount { get; private set; } = 0;
        public Models(Pen mcp, Pen umcp, Pen me, Pen ume,
            Font mv, Font umv, Font mw, Font umw,
            Brush mvb, Brush umvb, Brush mwb, Brush umwb)
        {
            markCirclePen = mcp; unmarkCirclePen = umcp;
            markEdgePen = me; unmarkEdgePen = ume;
            markVertexFont = mv; unmarkVertexFont = umv;
            markWeightFont = mw; unmarkWeightFont = umw;
            markVertexBrush = mvb; unmarkVertexBrush = umvb;
            markWeightBrush = mwb; unmarkWeightBrush = umwb;
            models = new SortedDictionary<int, Model>();
        }

        public void AddDrawModel(int hashCode, DrawModel drawModel)
        {
            models.Add(hashCode, new Model(drawModel));
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
            }
            throw new Exception("Некорректный хеш код");
            
        }

        public void Mark(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
            {
                if (model.Marked) return;
                    model.Marked = true;
                if (model.DrawModel is Circle)
                    MarkedCircleCount++;
                else if (model.DrawModel is Edge)
                    MarkedEdgeCount++;
            }

        }

        public void Unmark(int hashCode)
        {
            if (models.TryGetValue(hashCode, out Model model))
            {
                if (!model.Marked) return;
                    model.Marked = false;
                if (model.DrawModel is Circle)
                    MarkedCircleCount--;
                else if (model.DrawModel is Edge)
                    MarkedEdgeCount--;
            }
        }

        public void UnmarkAll()
        {
            foreach (var m in models)
                m.Value.Marked = false;
            MarkedCircleCount = 0;
            MarkedEdgeCount = 0;
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


        public int GetEdgeHashCode(vec2 pos)
        {
            foreach (var m in models)
                if (m.Value.DrawModel is Edge)
                {
                    Edge edge = (Edge)(m.Value.DrawModel);
                    vec2 a = edge.SourcePos;
                    vec2 b = edge.StockPos;
                    float x = pos.x;
                    float y = pos.y;
                    float res = ((x - a.x) / (b.x - a.x)) - ((y - a.y) / (b.y - a.y));
                    if (res <= 0.1 && res >= -0.1)
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
