using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Data;
using Antonyan.Graphs.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antonyan.Graphs.Gui
{
    public class Painter
    {
        protected Pen markCirclePen;
        protected Pen unmarkCirclePen;
        protected Pen markEdgePen;
        protected Pen unmarkEdgePen;

        protected Font markVertexFont;
        protected Font unmarkVertexFont;
        protected Font markWeightFont;
        protected Font unmarkWeightFont;

        protected Brush markVertexBrush;
        protected Brush unmarkVertexBrush;
        protected Brush markWeightBrush;
        protected Brush unmarkWeightBrush;

        protected Font font;
        protected SolidBrush brush;
        protected Pen pen;

        private static readonly Pen endPen = new Pen(Color.Black)
        {
            CustomEndCap = new CustomLineCap(new GraphicsPath(new PointF[] { new PointF(0f, 0f), new PointF(-3f, -15f),
                    new PointF(0f, -10f), new PointF(3f, -15f), new PointF(0f, 0f) }, new byte[] { 1, 1, 1, 1, 1 }), null)
        };

        protected static readonly Matrix mirrorX = new Matrix(-1f, 0f, 0f, 1f, 0f, 0f);
        protected static readonly Matrix mirrorY = new Matrix(1f, 0f, 0f, -1f, 0f, 0f);

        public Painter(Pen mcp, Pen umcp, Pen me, Pen ume,
            Font mv, Font umv, Font mw, Font umw,
            Brush mvb, Brush umvb, Brush mwb, Brush umwb)
        {
            markCirclePen = mcp; unmarkCirclePen = umcp;
            markEdgePen = me; unmarkEdgePen = ume;
            markVertexFont = mv; unmarkVertexFont = umv;
            markWeightFont = mw; unmarkWeightFont = umw;
            markVertexBrush = mvb; unmarkVertexBrush = umvb;
            markWeightBrush = mwb; unmarkWeightBrush = umwb;

            font = new Font(FontFamily.GenericSansSerif, 12f);
            brush = new SolidBrush(Color.Blue);
            pen = new Pen(Color.Blue, 1);
        }
        public void Draw(Graphics g, GraphModel m, vec2 min, vec2 max)
        {
            //if (model.Marked)
            //{
            //    pen = markCirclePen;
            //    brush = markVertexBrush;
            //    font = markVertexFont;
            //}
            //else
            //{
            //    pen = unmarkCirclePen;
            //    brush = unmarkVertexBrush;
            //    font = unmarkVertexFont;
            //}
            Color current = Color.FromArgb(m.Color.R, m.Color.G, m.Color.B);
            pen.Color = current;
            brush.Color = current;
            pen.Width = m.Marked ? 2 : m.Width;
            var vertex = m as VertexDrawModel;
            if (vertex != null)
                DrawVertex(g, vertex, min, max);
            else
            {
                var nonOrientEdge = m as NonOrientEdgeModel;
                if (nonOrientEdge != null)
                    DrawNonOrientEdge(g, nonOrientEdge, min, max);
                else DrawOrientedEdge(g, (OrientEdgeModel)m, min, max);
            }
            
        }

        private void DrawVertex(Graphics g, VertexDrawModel v, vec2 min, vec2 max)
        {
            if (Clip.SimpleClip(v.VertexStrPos, max, min, v.R / 2f))
                g.DrawString(v.VertexStr, font, brush, new RectangleF(v.VertexStrPos.x, v.VertexStrPos.y, v.R * 2f, v.R * 2f));
            int i = 0;
            vec2 A = new vec2(v.Lines[i++]);
            for (; i < v.Lines.Length; i++)
            {
                vec2 B = new vec2(v.Lines[i]);
                if (Clip.RectangleClip(ref A, ref B, min, max))
                    g.DrawLine(pen, A.x, A.y, B.x, B.y);
                A = new vec2(v.Lines[i]);
            }
        }

        private void DrawNonOrientEdge(Graphics g, NonOrientEdgeModel edge, vec2 min, vec2 max)
        {
            vec2 start = new vec2(edge.PosA);
            vec2 end = new vec2(edge.PosB);
            if (Clip.RectangleClip(ref start, ref end, min, max))
                g.DrawLine(pen, start.x, start.y, end.x, end.y);
            if (edge.Weighted && Clip.SimpleClip(new vec2(edge.WeightPos.x + 5f, edge.WeightPos.y + 5f), max, min, GlobalParameters.Radius))
                DrawWeight(g, edge, min, max);
            
        }

        private void DrawOrientedEdge(Graphics g, OrientEdgeModel edge, vec2 min, vec2 max)
        {
            vec2 A = new vec2(edge.PosA);
            vec2 B = new vec2(edge.PosB);
            if (Clip.RectangleClip(ref A, ref B, min, max))
                g.DrawLine(pen, A.x, A.y, B.x, B.y);
            B = new vec2(edge.PosB);
            vec2 C = new vec2(edge.PosC);
            if (Clip.RectangleClip(ref B, ref C, min, max))
                g.DrawLine(pen, B.x, B.y, C.x, C.y);
            C = new vec2(edge.PosC);
            vec2 D = new vec2(edge.PosD);
            if (Clip.RectangleClip(ref C, ref D, min, max) && (C - D).Length() > 6)
            {
                endPen.Color = pen.Color;
                endPen.Width = pen.Width;
                if (!D.Equals(edge.PosD)) ServiceFunctions.Swap(ref D, ref C);
                g.DrawLine(endPen, C.x, C.y, D.x, D.y);
            }
            if (edge.Weighted && Clip.SimpleClip(new vec2(edge.WeightPos.x + 5f, edge.WeightPos.y + 5f), max, min, GlobalParameters.Radius))
            {
                DrawWeight(g, edge, min, max);
            }
        }


        private void DrawWeight(Graphics g, AEdgeModel edge, vec2 min, vec2 max)
        {
            Matrix matrix = new Matrix();
            StringFormat stringFormat = new StringFormat();
            matrix.Translate(edge.WeightPos.x, edge.WeightPos.y);
            matrix.Rotate(edge.WeightAngle);
            if (edge.WeightAngle > 90f)
            {
                matrix.Multiply(mirrorY);
                matrix.Multiply(mirrorX);
            }
            g.MultiplyTransform(matrix);
            g.DrawString(edge.StringRepresent, font, brush, 0, 0, stringFormat);
            matrix.Reset();
            if (edge.WeightAngle > 90f)
            {
                matrix.Multiply(mirrorX);
                matrix.Multiply(mirrorY);
            }
            matrix.Rotate(-edge.WeightAngle);
            matrix.Translate(-edge.WeightPos.x, -edge.WeightPos.y);
            g.MultiplyTransform(matrix);
        }
    }
}
