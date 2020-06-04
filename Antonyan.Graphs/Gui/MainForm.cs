using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;

using Antonyan.Graphs.Gui;
using Antonyan.Graphs.Gui.Forms;

using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend.Algorithms;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Backend.Commands;
using System.IO;

namespace Antonyan.Graphs.Gui
{


    public partial class MainForm : Form, UserInterface
    {
        public event EventHandler<ACommandArgs> CommandEntered;

        private readonly float R = GlobalParameters.Radius;
        private readonly float left = 30f;
        private readonly float right = 250f;
        private readonly float top = 50f;
        private readonly float bottom = 50f;
        private readonly vec2 max, min;
        private readonly vec2 Wc, W;


        private bool oriented, weighted;
        private AVertexModel sourceModel, stockModel;
        private string header = $"Visual Graph";


        private int i = 0;
        private bool mouseDownFL = false;
        private string selectedKey;
        private vec2 lastVertexPos;
        private vec2 middlePos;

        private Painter _painter;
        private IModelField _field;

        public MainForm()
        {
            min = new vec2(); max = new vec2();
            Wc = new vec2(); W = new vec2();
            _painter = new Painter();
            InitializeComponent();
            statusStrip.Items.Add(toolStripStatusLabel);
            Text = header;

            listBoxAdjList.MouseDoubleClick += ListBoxAdjList_MouseDoubleClick;
        }





        public void AttachField(IModelField field) => _field = field;

        private void RetCalc()
        {
            max.x = ClientRectangle.Width - right;
            max.y = ClientRectangle.Height - bottom;
            min.x = left;
            min.y = top;
            Wc.y = max.y;
            Wc.x = left;
            W.x = max.x - left;
            W.y = max.y - top;

            //txtAdjList.Location.X = (int)(max.x + 10f);
            txtInfoList.Location = new Point((int)(max.x + 10f), (int)min.y);
            txtInfoList.Width = (int)right - 50;
            txtInfoList.Height = (int)max.y - 260;
            btnSaveAdjList.Location = new Point((int)(max.x + 10f), txtInfoList.Location.Y + txtInfoList.Height + 3);
            btnSaveAdjList.Width = txtInfoList.Width;
            btnSaveAdjList.Height = 43;
            btnSaveAdjList.TextAlign = ContentAlignment.MiddleCenter;
            listBoxAdjList.Location = new Point((int)(max.x + 10f), txtInfoList.Location.Y + txtInfoList.Height + 50);
            listBoxAdjList.Width = txtInfoList.Width - 30;
            listBoxAdjList.Height = (int)max.y - (int)top - txtInfoList.Height - 45;
            btnUnion.Location = new Point(listBoxAdjList.Location.X + listBoxAdjList.Width + 5, listBoxAdjList.Location.Y);
            btnUnion.TextAlign = ContentAlignment.MiddleCenter;
            btnRemoveAdjList.Width = btnUnion.Width = 25;

            btnRemoveAdjList.Location = new Point(btnUnion.Location.X, btnUnion.Location.Y + btnUnion.Height + 5);
        }

        public void ModelsFieldUpdate(object obj, EventArgs e)
        {
            Refresh();
        }

        // --------------------------------- From events --------------------------------- //
        private void MainForm_Load(object sender, EventArgs e)
        {
            RetCalc();
            tsbtnDeleteGraph.Enabled = false;
            tsbtnRemoveElems.Enabled = false;
            tsBtnAddVertex.Enabled = false;
            tsbtnMove.Enabled = false;
            tsBtnAddEdge.Enabled = false;
            tsBtnRedo.Enabled = false;
            tsBtnUndo.Enabled = false;
            subDetoursBtnBFS.Enabled = subDetoursBtnDFS.Enabled = false;
            tsbtnSaveGraph.Enabled = false;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // tsBtnAddVertex.Enabled = _field.Status;
            try
            {
                var g = e.Graphics;
                if (_field.Status)
                    _field.Models?.ForEach(m => _painter.Draw(g, m, min, max));
                Pen rectPen = new Pen(Color.Black, 2);
                g.DrawRectangle(rectPen, left, top, W.x, W.y);
            }
            catch (Exception ex)
            {
                PostErrorMessage(ex.Message);
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            RetCalc();
            Refresh();
        }

        // --------------------------------- !From events --------------------------------- //
        //public Edges makeEdges(vec2 posSource, string source, vec2 posStock, string stock, float r, string weight)
        //{
        //    if (oriented) return new OrientedEdges(posSource, source, posStock, stock, r, weight);
        //    else return new NonOrientedEdges(posSource, source, posStock, stock, r, weight);
        //}



        // ----------------------------- Mouse Events ----------------------------- //

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_field.Status || tsbtnMove.Checked) return;
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        {
                            vec2 pos = new vec2((float)e.X, (float)e.Y);
                            if (max.x - pos.x < R || pos.x - left < R || max.y - pos.y < R || pos.y - top < R)
                            {
                                _field.UnmarkGraphModels();
                            }
                            else if ((selectedKey = _field.GetPosKey(pos, R)) != null && !tsBtnAddVertex.Checked)
                            {
                                _field.MarkGraphModel(selectedKey);
                                var model = _field[selectedKey];
                                if (model is AVertexModel && _field.MarkedModelsCount <= 2)
                                {
                                    if (_field.MarkedVertexModelCount == 1)
                                        sourceModel = (AVertexModel)model;
                                    else if (_field.MarkedVertexModelCount == 2)
                                    {
                                        stockModel = (AVertexModel)model;
                                    }
                                }

                            }

                            else if (_field.GetPosKey(pos, R + R + R / 2) == null && tsBtnAddVertex.Checked)
                            {

                                if (_field.MarkedVertexModelCount > 0 || _field.MarkedEdgeModelCount > 0)
                                {
                                    _field.UnmarkGraphModels();
                                    break;
                                }

                                string v = i++.ToString();
                                var model = new VertexDrawModel(v, pos);
                                AddModelCommandArgs command = new AddModelCommandArgs(model);
                                CommandEntered?.Invoke(this, command);

                            }
                            else
                            {
                                _field.UnmarkGraphModels();
                                sourceModel = stockModel = null;
                            }
                            break;
                        }
                    default: break;
                }
            }
            catch (Exception ex)
            {
                PostErrorMessage(ex.Message);
            }
        }
        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (tsbtnMove.Checked)
            {
                mouseDownFL = true;
                vec2 pos = new vec2(e.X, e.Y);
                _field.UnmarkGraphModels();
                selectedKey = _field.GetVertexPosKey(pos, R);
                if (selectedKey != null)
                {
                    _field.MarkGraphModel(selectedKey);
                    middlePos = lastVertexPos = _field.GetVertexModelPos(selectedKey);
                }
            }
        }
        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDownFL || !tsbtnMove.Checked) return;
            vec2 pos = new vec2(e.X, e.Y);
            if (selectedKey != null && !selectedKey.Contains("->"))
            {
                if (Clip.SimpleClip(pos, max, min, GlobalParameters.Radius))
                {
                    middlePos = pos;
                    _field.MoveVertexModel(selectedKey, pos);
                }
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            vec2 pos = new vec2(e.X, e.Y);
            if (mouseDownFL && lastVertexPos != null && middlePos != null)
            {
                vec2 helpVector = new vec2(min);
                Clip.RectangleClip(ref helpVector, ref pos, min, max);
                CommandEntered?.Invoke(this, new MoveVertexModelCommandArgs(middlePos, lastVertexPos, selectedKey));
                selectedKey = null;
                lastVertexPos = null;
                _field.UnmarkGraphModels();
            }
            mouseDownFL = false;
        }

        private void MainForm_MouseEnter(object sender, EventArgs e)
        {

        }



        // ----------------------------- !Mouse Events ----------------------------------- //

        // *************** ****************** *************** **************************** //

        // ----------------------------- MainToolStrip ----------------------------------- //

        // -----------------------------!MainToolStrip ----------------------------------- //

        // ****************** *********************** ************************************ //

    }


}
