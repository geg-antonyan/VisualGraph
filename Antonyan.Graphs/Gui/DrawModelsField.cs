using System;
using System.Collections.Generic;
using System.Drawing;

using Antonyan.Graphs.Gui.Models;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;

namespace Antonyan.Graphs.Gui
{
    public class DrawModelsField
    {
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

        private SortedDictionary<string, ADrawModel> models;

        public event EventHandler<EventArgs> Update;

        public int MarkedCircleCount { get; private set; } = 0;
        public int MarkedEdgeCount { get; private set; } = 0;

        public int MarkedModelsCount { get { return MarkedCircleCount + MarkedEdgeCount; } }
        public DrawModelsField(MainForm form, Pen mcp, Pen umcp, Pen me, Pen ume,
            Font mv, Font umv, Font mw, Font umw,
            Brush mvb, Brush umvb, Brush mwb, Brush umwb)
        {
            Update += form.ModelsBoardUpdate;
            markCirclePen = mcp; unmarkCirclePen = umcp;
            markEdgePen = me; unmarkEdgePen = ume;
            markVertexFont = mv; unmarkVertexFont = umv;
            markWeightFont = mw; unmarkWeightFont = umw;
            markVertexBrush = mvb; unmarkVertexBrush = umvb;
            markWeightBrush = mwb; unmarkWeightBrush = umwb;
            models = new SortedDictionary<string, ADrawModel>();
        }


        public void AddDrawModel(string present, ADrawModel drawModel)
        {
            models.Add(present, drawModel);
            Update?.Invoke(this, null);
        }
        public bool RemoveDrawModel(string represent)
        {

            if (models.TryGetValue(represent, out ADrawModel model))
            {
                if (model.Marked)
                {
                    if (model is DrawVertexModel)
                        MarkedCircleCount--;
                    //else if (model is Edges)
                    //    MarkedEdgeCount--;
                }

                models.Remove(represent);
                Update?.Invoke(this, null);
                return true;
            }
            else return false;
        }

        public string GetPosRepresent(vec2 pos, float r)
        {
            foreach (var m in models)
            {
                var concretModel = m.Value as DrawVertexModel;
                if (concretModel != null)
                {
                    var v = (VertexModel)concretModel.Model;
                    if (Math.Pow(pos.x - v.Pos.x, 2.0) + Math.Pow(pos.y - v.Pos.y, 2.0) <= r * r)
                        return v.GetRepresentation();
                }
            }
            return null;
        }


        public vec2 GetDrawVertexModelPos(string represent)
        {
            ADrawModel dv;
            if (models.ContainsKey(represent) && (dv = models[represent]) is DrawVertexModel)
            {
                var vertexModel = (DrawVertexModel)dv;
                return ((VertexModel)vertexModel.Model).Pos;
            }
            return null;
        }

        public void ChangeDrawVertexModelPos(string represent, vec2 newPos)
        {
            ADrawModel dm;
            if (models.ContainsKey(represent) && (dm = models[represent]) is DrawVertexModel)
            {
                ((DrawVertexModel)dm).SetPos(newPos);
                Update(this, null);
            }
        }

        public bool MarkDrawModel(string represent)
        {
            if (models.ContainsKey(represent))
            {
                models[represent].Marked = true;
                return true;
            }
            return false;
        }

        public void UnmarkAllDrawModels()
        {
            foreach (var m in models)
                m.Value.Marked = false;
            MarkedCircleCount = 0;
            MarkedEdgeCount = 0;
            Update?.Invoke(this, null);
        }


        public void Draw(Graphics g, vec2 min, vec2 max)
        {
            foreach (var m in models)
            {
                Pen pen; Brush brush; Font font;
                if (m.Value is DrawVertexModel)
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
                m.Value.Draw(g, pen, brush, font, min, max);
            }
        }

    }

}
