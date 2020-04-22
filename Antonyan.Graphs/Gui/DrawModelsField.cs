using System;
using System.Collections.Generic;
using System.Drawing;

using Antonyan.Graphs.Gui.Models;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Data;

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

        private SortedDictionary<string, ADrawModel> drawModels;

        public event EventHandler<EventArgs> Update;

        public int MarkedVertexCount { get; private set; } = 0;
        public int MarkedEdgeCount { get; private set; } = 0;

        public int MarkedModelsCount => MarkedEdgeCount + MarkedVertexCount; 
        public DrawModelsField(MainForm form, Pen mcp, Pen umcp, Pen me, Pen ume,
            Font mv, Font umv, Font mw, Font umw,
            Brush mvb, Brush umvb, Brush mwb, Brush umwb)
        {
            Update += form.ModelsFieldUpdate;
            markCirclePen = mcp; unmarkCirclePen = umcp;
            markEdgePen = me; unmarkEdgePen = ume;
            markVertexFont = mv; unmarkVertexFont = umv;
            markWeightFont = mw; unmarkWeightFont = umw;
            markVertexBrush = mvb; unmarkVertexBrush = umvb;
            markWeightBrush = mwb; unmarkWeightBrush = umwb;
            drawModels = new SortedDictionary<string, ADrawModel>();
        }

        public GraphModels GetGraphModel(string represent)
        {
            return drawModels[represent].Model;
        }
        public void AddDrawModel(string present, ADrawModel drawModel)
        {
            drawModels.Add(present, drawModel);
            Update?.Invoke(this, null);
        }
        public bool RemoveDrawModel(string represent)
        {

            if (drawModels.TryGetValue(represent, out ADrawModel model))
            {
                if (model.Marked)
                {
                    if (model is DrawVertexModel)
                        MarkedVertexCount--;
                    else 
                        MarkedEdgeCount--;
                }

                drawModels.Remove(represent);
                Update?.Invoke(this, null);
                return true;
            }
            else return false;
        }

        public  ADrawModel this[string key]
        {
            get { return drawModels[key]; }
            //set { }
        }

        public string GetPosRepresent(vec2 pos, float r)
        {
            foreach (var m in drawModels)
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
            if (drawModels.ContainsKey(represent) && (dv = drawModels[represent]) is DrawVertexModel)
            {
                var vertexModel = (DrawVertexModel)dv;
                return ((VertexModel)vertexModel.Model).Pos;
            }
            return null;
        }

        public void ChangeDrawVertexModelPos(string represent, vec2 newPos)
        {
            ADrawModel dm;
            if (drawModels.ContainsKey(represent) && (dm = drawModels[represent]) is DrawVertexModel)
            {
                ((DrawVertexModel)dm).SetPos(newPos);
                Update(this, null);
            }
        }

        public bool MarkDrawModel(string represent)
        {
            ADrawModel drawModel;
            if (drawModels.ContainsKey(represent) && !(drawModel = drawModels[represent]).Marked)
            {
                drawModel.Marked = true;
                if (drawModel is DrawVertexModel)
                    MarkedVertexCount++;
                else
                    MarkedEdgeCount++;
                Update?.Invoke(this, null);
                return true;
            }
            return false;
        }

        public void UnmarkAllDrawModels()
        {
            foreach (var m in drawModels)
                m.Value.Marked = false;
            MarkedVertexCount = 0;
            MarkedEdgeCount = 0;
            Update?.Invoke(this, null);
        }


        public void Draw(Graphics g, vec2 min, vec2 max)
        {
            foreach (var m in drawModels)
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
