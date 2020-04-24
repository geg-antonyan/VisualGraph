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

using Antonyan.Graphs.Backend.CommandArgs;
using Antonyan.Graphs.Util;
using Antonyan.Graphs.Data;

using Antonyan.Graphs.Gui;
using Antonyan.Graphs.Gui.Forms;

using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend.Algorithms;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Backend.Commands;

namespace Antonyan.Graphs.Gui
{


    public partial class MainForm : Form, UserInterface
    {
        public event EventHandler<ACommandArgs> CommandEntered;

        private readonly float R = GlobalParameters.Radius;
        private readonly float left = 30f;
        private readonly float right = 30f;
        private readonly float top = 50f;
        private readonly float bottom = 50f;
        private readonly vec2 max, min;
        private readonly vec2 Wc, W;


        private bool oriented, weighted;
        private VertexDrawModel sourceModel, stockModel;
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

            _painter = new Painter(
                new Pen(Color.Red, 2f), new Pen(Color.Blue), new Pen(Color.Green, 3f), new Pen(Color.DarkGray, 2f),
                new Font(FontFamily.GenericSansSerif, 14f), new Font(FontFamily.GenericSansSerif, 12f),
                new Font(FontFamily.GenericMonospace, 14f),
                new Font(FontFamily.GenericMonospace, 12f),
                new SolidBrush(Color.Red), new SolidBrush(Color.Blue), new SolidBrush(Color.Green), new SolidBrush(Color.DarkGray));
            InitializeComponent();
            Text = header;
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
        }

        public void ModelsFieldUpdate(object obj, EventArgs e)
        {
            Refresh();
        }

        // --------------------------------- From events --------------------------------- //
        private void MainForm_Load(object sender, EventArgs e)
        {
            RetCalc();
            tsBtnAddEdge.Enabled = false;
            tsBtnRedo.Enabled = false;
            tsBtnUndo.Enabled = false;
            subDetoursBtnBFS.Enabled = subDetoursBtnDFS.Enabled = false;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            tsBtnAddVertex.Enabled = _field.Status;
            var g = e.Graphics;
            _field.Models.ForEach(m => _painter.Draw(g, m, min, max));
            Pen rectPen = new Pen(Color.Black, 2);
            g.DrawRectangle(rectPen, left, top, W.x, W.y);
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
            if (!_field.Status || tsbtnMove.Checked) return;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    {
                        vec2 pos = new vec2((float)e.X, (float)e.Y);

                        //int vertexHashCode, edgehashCode;
                        if (max.x - pos.x < R || pos.x - left < R || max.y - pos.y < R || pos.y - top < R)
                        {
                            _field.UnmarkGraphModels();
                        }
                        else if ((selectedKey = _field.GetPosKey(pos, R)) != null && !tsBtnAddVertex.Checked)
                        {
                            _field.MarkGraphModel(selectedKey);
                            var model = _field[selectedKey];
                            if (model is VertexDrawModel && _field.MarkedModelsCount <= 2)
                            {
                                if (_field.MarkedVertexModelCount == 1)
                                    sourceModel = (VertexDrawModel)model;
                                else if (_field.MarkedVertexModelCount == 2)
                                {
                                    stockModel = (VertexDrawModel)model;
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
                            VertexDrawModel model = new VertexDrawModel(v, pos);
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
            tsBtnAddEdge.Enabled = _field.MarkedVertexModelCount == 2 && _field.MarkedModelsCount == 2;
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
                if ((pos - middlePos).Length() > 20f)
                {
                    middlePos = pos;
                    _field.MoveVertexModel(selectedKey, pos);
                }
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseDownFL && lastVertexPos != null)
            {
                vec2 pos = new vec2(e.X, e.Y);
                CommandEntered?.Invoke(this, new MoveVertexModelCommandArgs(pos, lastVertexPos, selectedKey));
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

        private void tsbtnUndo_Click(object sender, EventArgs e)
        {
            CommandEntered?.Invoke(this, new ACommandArgs("undo"));
        }
        private void tsbtnRedo_Click(object sender, EventArgs e)
        {
            CommandEntered?.Invoke(this, new ACommandArgs("redo"));
        }
        private void tsBtnCrtGraph_Click(object sender, EventArgs e)
        {
            CreateGraphForm window = new CreateGraphForm();
            window.Owner = this;
            window.ShowDialog();
            if (window.Ok)
            {
                oriented = window.Oriented;
                weighted = window.Weighted;
                CommandEntered?.Invoke(this, new CreateGraphCommandArgs(oriented, weighted));
            }
        }

        private void tsBtnDeleteGraph_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Вы действительно хотите безвозвратно удалить этот граф?", "Удаление Графа", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                CommandEntered?.Invoke(null, new ACommandArgs(nameof(RemoveGraphCommand)));
            }    
        }

        private void tsbtnAddVertex_Click(object sender, EventArgs e)
        {
            tsBtnAddVertex.Checked = !tsBtnAddVertex.Checked;
            if (tsBtnAddVertex.Checked)
            {
                Text = header + " - кликните внутри рабочего прямоугольника, чтобы добавить вершину";
                tsbtnMove.Checked = false;
            }
            else Text = header;
        }

        private void tsbtnRemoveElems_Click(object sender, EventArgs e)
        {
            CommandEntered?.Invoke(this, new RemoveModelsCommandArgs(_field.GetMarkedModels()));
        }


        private void tsBtnMove_Click(object sender, EventArgs e)
        {
            tsbtnMove.Checked = !tsbtnMove.Checked;
            if (tsbtnMove.Checked)
                tsBtnAddVertex.Checked = false;
        }


        private void tsBtnAddEdge_Click(object sender, EventArgs e)
        {
            if (sourceModel == null || stockModel == null
                || sourceModel.Key == stockModel.Key) return;

            string weight = null;
            if (_field.IsWeighted)
            {
                SetWeightForm window = new SetWeightForm();
                window.Owner = this;
                window.ShowDialog();
                if (window.Ok)
                {
                    weight = window.Weight;
                }
            }
            AEdgeDrawModel edgeModel;
            if (_field.IsOrgraph)
                edgeModel = new OrientEdgeModel(sourceModel, stockModel, weight);
            else edgeModel = new NonOrientEdgeModel(sourceModel, stockModel, weight);

            CommandEntered?.Invoke(this, new AddModelCommandArgs(edgeModel));
            _field.UnmarkGraphModels();
            sourceModel = stockModel = null;
        }


        private void tsBtnDetours_Click(object sender, EventArgs e)
        {
            //string dfs = "Обход в глубину", bfs = "Обход в ширину";
            //bool res = subDetoursBtnBFS.Enabled = subDetoursBtnDFS.Enabled = modelsField.MarkedVertexCount == 1 ? true : false;
            //if (res)
            //{
            //    subDetoursBtnBFS.Text = $"{bfs} начиная с вершины \"{source}\"";
            //    subDetoursBtnDFS.Text = $"{dfs} начиная с вершины \"{source}\"";
            //}
            //else
            //{
            //    subDetoursBtnBFS.Text = bfs;
            //    subDetoursBtnDFS.Text = dfs;
            //}
        }

        private void subDetoursBtnDFS_Click(object sender, EventArgs e)
        {
            //Text += " - Выполяется алгоритм обхода в глубину";

            //   CommandEntered?.Invoke(this, new UIDFSargs(sourceModel));
            //        algorithmProcessing = false;
            //        Text = header;

        }

        private void subDetoursBtnBFS_Click(object sender, EventArgs e)
        {
            //Text += " - Выполяется алгоритм обхода в ширину";
            //algorithmProcessing = true;
            //algoThread = new Thread(() =>
            //{
            //    BeginInvoke((MethodInvoker)(() =>
            //    {
            //        CommandEntered?.Invoke(this, new UIStringArgs($"algorithm bfs {source}"));
            //        algorithmProcessing = false;
            //        Text = header;
            //    }));
            //});
            //algoThread.Start();
        }

        private void tsBtnShortcats_Click(object sender, EventArgs e)
        {
            // subSortcatBtnBFS.Enabled = modelsField.MarkedVertexCount == 2 && !weighted;
        }

        private void subSortcatBtnBFS_Click(object sender, EventArgs e)
        {
            //Text += " - Выполяется алгоритм нахождение кратчайшего пути с помошью построение родительского дерева";
            //algorithmProcessing = true;
            //algoThread = new Thread(() =>
            //{
            //    BeginInvoke((MethodInvoker)(() =>
            //    {
            //        CommandEntered?.Invoke(this, new UIStringArgs($"algorithm shortcatdfs {source} {stock}"));
            //        algorithmProcessing = false;
            //        Text = header;
            //    }));
            //});
            //algoThread.Start();
        }
        // -----------------------------!MainToolStrip ----------------------------------- //

        // ****************** *********************** ************************************ //

        // ---------------------------- Override Methods --------------------------------- //

        public void PostMessage(string message)
        {
            MessageBox.Show(message, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public void PostWarningMessage(string warningMessage)
        {
            throw new NotImplementedException();
        }

        public void PostErrorMessage(string errorMessage)
        {
            throw new NotImplementedException();
        }
        public void CheckUndoRedo(bool undoPossible, bool redoPossible)
        {
            tsBtnRedo.Enabled = redoPossible;
            tsBtnUndo.Enabled = undoPossible;
        }

        public void SetFieldStatus(bool status)
        {
            if (!status) i = 0;
            tlbtnCrtGraph.Enabled = !status;
        }


        public void FieldUpdate(object obj, ModelFieldUpdateArgs e)
        {
            switch (e?.Event)
            {
                case FieldEvents.InitGraph:
                    header += (_field.IsOrgraph ? " - Ориентриванный, " : " - Неориентированный, ") +
                        (_field.IsWeighted ? "Взвещанный." : "Невзвещанный.");
                    Text = header;
                    tlbtnCrtGraph.Enabled = false;
                    tsbtnDeleteGraph.Enabled = true;
                    break;
                case FieldEvents.RemoveGraph:
                    header = "Visual Graph";
                    Text = header;
                    i = 0;
                    tlbtnCrtGraph.Enabled = true;
                    tsbtnDeleteGraph.Enabled = false;
                    break;
                default: break;
            }
            tsbtnRemoveElems.Enabled = _field.MarkedModelsCount > 0;
            Refresh();

        }

        // ---------------------------- !Override Methods --------------------------------- //

    }


}
