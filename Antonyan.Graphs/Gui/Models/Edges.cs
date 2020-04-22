//using System;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using Antonyan.Graphs.Backend;
//using Antonyan.Graphs.Board;


//namespace Antonyan.Graphs.Gui.Models
//{
//    public abstract class Edges : ADrawModel
//    {
//        protected static readonly Matrix mirrorX = new Matrix(-1f, 0f, 0f, 1f, 0f, 0f);
//        protected static readonly Matrix mirrorY = new Matrix(1f, 0f, 0f, -1f, 0f, 0f);

//        protected float angleWeight;
//        protected vec2 posWeight;
//        protected float R;
//        protected bool reverse = false;
//        public vec2 PosA { get; protected set; }
//        public vec2 PosB { get; protected set; }
//        public string SourceStr { get; protected set; }
//        public string StockStr { get; protected set; }
//        public vec2 PosSource { get; protected set; }
//        public vec2 PosStock { get; protected set; }
//        public string WeightStr { get; protected set; }
//        public bool Weighted { get; protected set; }

//        public Edges(vec2 posSource, string sourceStr, vec2 posStock, string stockStr, float r, string weightStr = null)
//        {
//            SourceStr = sourceStr;
//            StockStr = stockStr;
//            Weighted = weightStr == null ? false : true;
//            WeightStr = weightStr;
//            R = r;
//            SetPos(posSource, posStock);
//        }
//        public virtual void SetPos(vec2 posSource, vec2 posStock)
//        {
//            PosSource = posSource;
//            PosStock = posStock;
//            if (posSource.y > posStock.y)
//            {
//                Clip.Swap(ref posSource, ref posStock);
//                reverse = !reverse;
//            }
//            vec2 deltaV = posStock - posSource;
//            float tmp = deltaV.x / deltaV.Length();
//            angleWeight = (float)Math.Acos(tmp) * 180f / (float)Math.PI;

//            vec2 norm = deltaV.Normalize();
//            vec2 incr = norm * R;
//            PosA = posSource + incr;
//            PosB = posStock - incr;
//            if (Weighted)
//            {
//                float length = (posStock - posSource).Length();
//                float center = length / 2f;
//                vec2 dl = norm * center;
//                posWeight = posSource + dl;
//            }
//            //SetPosImpl(PosSource, PosStock);
//        }
//       // protected abstract void SetPosImpl(vec2 posSource, vec2 posStock);
//        public void Draw(Graphics graphic, Pen pen, Brush brush, Font font, vec2 min, vec2 max)
//        {
//            DrawEdge(graphic, pen, min, max);
//            if (Weighted && Clip.SimpleClip(new vec2(posWeight.x + 5f, posWeight.y + 5f), max, min, R))
//                DrawWeight(graphic, brush, font, min, max);
//        }

//        protected abstract void DrawEdge(Graphics graphic, Pen pen, vec2 min, vec2 max);
//        protected abstract void DrawWeight(Graphics graphic, Brush brush, Font font, vec2 min, vec2 max);
//    }

//    //public class NonOrientedEdges : Edges
//    //{


//    //    public NonOrientedEdges(vec2 sourcePos, string source, vec2 stockPos, string stock, float r, string weight = null)
//    //        : base(sourcePos, source, stockPos, stock, r, weight)
//    //    {

//    //    }

//    //    //protected override void SetPosImpl(vec2 posSource, vec2 posStock)
//    //    //{
//    //    //    vec2 norm = (posStock - posSource).Normalize();
//    //    //    vec2 incr = norm * R;
//    //    //    PosA = posSource + incr;
//    //    //    PosB = posStock - incr;
//    //    //    if (Weighted)
//    //    //    {
//    //    //        float length = (posStock - posSource).Length();
//    //    //        float center = length / 2f;
//    //    //        vec2 dl = norm * center;
//    //    //        posWeight = posSource + dl;
//    //    //    }
//    //    //}

//    //    protected override void DrawEdge(Graphics graphic, Pen pen, vec2 min, vec2 max)
//    //    {
//    //        vec2 start = new vec2(PosA);
//    //        vec2 end = new vec2(PosB);
//    //        if (Clip.RectangleClip(ref start, ref end, min, max))
//    //            graphic.DrawLine(pen, start.x, start.y, end.x, end.y);
//    //    }

//    //    protected override void DrawWeight(Graphics graphic, Brush brush, Font font, vec2 min, vec2 max)
//    //    {
//    //        Matrix matrix = new Matrix();
//    //        vec2 A = PosB - PosA;
//    //        vec2 B = new vec2(10f, 0f);
//    //        B.y = -(A.x * A.y) / B.x;
//    //        B = B.Normalize();
//    //        B *= 20f;
//    //        StringFormat stringFormat = new StringFormat();
//    //        matrix.Translate(posWeight.x, posWeight.y);
//    //        matrix.Rotate(angleWeight);
//    //        if (angleWeight > 90f)
//    //        {
//    //            B *= -1f;
//    //            matrix.Multiply(mirrorY);
//    //            matrix.Multiply(mirrorX);
//    //        }
//    //        if (angleWeight == 90f) B = new vec2(0f, 0f);
//    //        graphic.MultiplyTransform(matrix);
//    //        graphic.DrawString(WeightStr.ToString(), font, brush, B.x, B.y, stringFormat);
//    //        matrix.Reset();
//    //        if (angleWeight > 90f)
//    //        {
//    //            matrix.Multiply(mirrorX);
//    //            matrix.Multiply(mirrorY);
//    //        }
//    //        matrix.Rotate(-angleWeight);
//    //        matrix.Translate(-posWeight.x, -posWeight.y);
//    //        graphic.MultiplyTransform(matrix);
//    //    }
//    //}

//    //public class OrientedEdges : Edges
//    //{
//    //    private vec2 norm;


//    //    public OrientedEdges(vec2 sourcePos, string source, vec2 stockPos, string stock, float r, string weight = null)
//    //        : base(sourcePos, source, stockPos, stock, r, weight)
//    //    {

//    //    }

//    //    public override void SetPos(vec2 posSource, vec2 posStock)
//    //    {
//    //        base.SetPos(posSource, posStock);
//    //        float x, y;
//    //        vec2 AB = posStock - posSource;
//    //        if (posSource.x != posStock.y)
//    //        {
//    //            if (posStock.x > posSource.x)
//    //                y = -10f;
//    //            else y = 10f;
//    //            vec2 v = new vec2(0f, y);
//    //            x = -(AB.y * v.y) / AB.x;
//    //        }
//    //        else
//    //        {
//    //            if (posStock.y > posSource.y)
//    //                x = -10f;
//    //            else x = 10f;
//    //            vec2 v = new vec2(x, 0f);
//    //            y = (-AB.x * v.x) / AB.y;
//    //        }
//    //        norm = new vec2(x, y).Normalize();

//    //    }
//    //    //protected override void SetPosImpl(vec2 posSource, vec2 posStock)
//    //    //{
//    //    //    vec2 offset;
//    //    //    float x, y;
//    //    //    vec2 AB = posStock - posSource;
//    //    //    if (posSource.x != posStock.y)
//    //    //    {
//    //    //        if (posStock.x > posSource.x)
//    //    //            y = -10f;
//    //    //        else y = 10f;
//    //    //        vec2 v = new vec2(0f, y);
//    //    //        x = -(AB.y * v.y) / AB.x;
//    //    //    }
//    //    //    else 
//    //    //    {
//    //    //        if (posStock.y > posSource.y)
//    //    //            x = -10f;
//    //    //        else x = 10f;
//    //    //        vec2 v = new vec2(x, 0f);
//    //    //        y = (-AB.x * v.x) / AB.y;
//    //    //    }

//    //    //    vec2 norm = new vec2(x, y).Normalize();
//    //    //    offset = norm * R;
//    //    //    PosA = posSource + offset;
//    //    //    PosB = posStock + offset;

//    //    //}
//    //    protected override void DrawEdge(Graphics graphic, Pen pen, vec2 min, vec2 max)
//    //    {
//    //        //    vec2 start = new vec2(PosA);
//    //        //    vec2 end = new vec2(PosB);
//    //        //vec2 direction = PosB - PosA;
//    //        //float length = direction.Length();
//    //        //vec2 normDirection = direction.Normalize(); 
//    //        //float step = R / (length / 2f);
//    //        ////vec2 a = new vec2(PosA), b = new vec2(PosB);
//    //        ////if (reverse) 
//    //        //vec2 start = new vec2(PosA);
//    //        //vec2 startCurve = start;
//    //        //for (float i = 1, j = 1f; i < length / 2f; i++, j += step)
//    //        //{
//    //        //    vec2 end = PosA + (normDirection * i);
//    //        //    vec2 endCurve = end + (norm * j);
//    //        //    if (Clip.RectangleClip(ref startCurve, ref endCurve, min, max))
//    //        //        graphic.DrawLine(pen, startCurve.x, startCurve.y, endCurve.x, endCurve.y);
//    //        //    st art = end;
//    //        //    startCurve = end + (norm * j);
//    //        //}
//    //        //for (float i = length / 2f; i >=  1f; i--)
//    //        //{
//    //        //    vec2 end = PosA + (normDirection * i);
//    //        //    vec2 endCurve = end + (norm * j);
//    //        //    if (Clip.RectangleClip(ref startCurve, ref endCurve, min, max))
//    //        //        graphic.DrawLine(pen, startCurve.x, startCurve.y, endCurve.x, endCurve.y);
//    //        //    start = end;
//    //        //    startCurve = end + (norm * j);
//    //        //}

//    //        //{
//    //        //    pen.EndCap = LineCap.ArrowAnchor;
//    //        //    pen.Width = 4;
//    //        //    graphic.TranslateTransform(start.x, start.y);
//    //        //    graphic.RotateTransform(angleWeight);
//    //        //    graphic.DrawArc(pen, 0f, 0f, Math.Abs(end.Length() - start.Length()), 30f, 0, 180f);
//    //        //    graphic.RotateTransform(-angleWeight);
//    //        //    graphic.TranslateTransform(-start.x, -start.y);
//    //        //}
//    //        vec2 direction = PosB - PosA;
//    //        float length = direction.Length();
//    //        float r = R / 2f + (length * length) / (8 * R);
//    //        vec2 normDirection = direction.Normalize();
//    //        vec2 start = new vec2(PosA);
//    //        float delta_x = PosB.x - PosA.x;
//    //        float dx = delta_x / length;
//    //        float x = start.x + dx;
            
//    //        while (x <= PosB.x)
//    //        {
//    //            float y2 = r * r - x * x;
//    //            if (y2 < 0) { x += dx; continue; }
//    //            float y = (float)Math.Sqrt(y2);
//    //            vec2 end = new vec2(x, y);
//    //            vec2 tmpEnd = new vec2(end);
//    //            if (Clip.RectangleClip(ref start, ref end, min, max))
//    //                graphic.DrawLine(pen, start.x, start.y, end.x, end.y);
//    //            start = tmpEnd;
//    //            x += dx;
//    //        }

//    //        //vec2 start = new vec2(PosA);
//    //        //for (float i = 1f; i <= length; i++)
//    //        //{
//    //        //    vec2 end = PosA + (normDirection * i);
//    //        //    vec2 tmpEnd = end;
                 
//    //        //}
//    //    }

//    //    protected override void DrawWeight(Graphics graphic, Brush brush, Font font, vec2 min, vec2 max)
//    //    {
//    //        throw new NotImplementedException();
//    //    }
//    //}
//}
