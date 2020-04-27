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
        private readonly float right = 60f;
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

        private event EventHandler<EventArgs> myEvent;

        private void MyEventFun(object sender, EventArgs e)
        {
            Refresh();
        }
        public MainForm()
        {
            myEvent += MyEventFun;

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
                    _field.Models.ForEach(m => _painter.Draw(g, m, min, max));
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
        private void tsbtnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (openGraphFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = openGraphFileDialog.OpenFile())
                    {
                        CommandEntered?.Invoke(this, new OpenGraphInFileCommandArgs(stream));
                    }
                }
            }
            catch (Exception ex)
            {
                PostErrorMessage(ex.Message);
            }
        }
        private void tsbtnSaveGraph_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveGraphFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = saveGraphFileDialog.OpenFile())
                    {
                        CommandEntered?.Invoke(this, new SaveGraphToFileCommandArgs(stream));
                        PostMessage("Граф успешно сохранен");
                    }
                }
            }
            catch (Exception ex)
            {
                PostErrorMessage(ex.Message);
            }

        }

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
            sourceModel = stockModel = null;
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
            AEdgeModel edgeModel;
            if (_field.IsOrgraph)
                edgeModel = new OrientEdgeModel(sourceModel, stockModel, weight);
            else edgeModel = new NonOrientEdgeModel(sourceModel, stockModel, weight);

            CommandEntered?.Invoke(this, new AddModelCommandArgs(edgeModel));
            _field.UnmarkGraphModels();
        }


        private void tsBtnDetours_Click(object sender, EventArgs e)
        {
            string dfs = "Обход в глубину", bfs = "Обход в ширину";
            bool res = subDetoursBtnBFS.Enabled = subDetoursBtnDFS.Enabled = _field.MarkedVertexModelCount == 1 ? true : false;
            if (res)
            {
                subDetoursBtnBFS.Text = $"{bfs} начиная с вершины \"{sourceModel.StringRepresent}\"";
                subDetoursBtnDFS.Text = $"{dfs} начиная с вершины \"{sourceModel.StringRepresent}\"";
            }
            else
            {
                subDetoursBtnBFS.Text = bfs;
                subDetoursBtnDFS.Text = dfs;
            }
        }

        private void subDetoursBtnDFS_Click(object sender, EventArgs e)
        {
            Text += " - Выполяется алгоритм обхода в глубину";
            _field.UnmarkGraphModels();
            new Thread(() =>
            CommandEntered?.Invoke(this, new DFScommandArgs(sourceModel))).Start();
            Text = header;
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
            subShortcutBtnBFS.Enabled = _field.MarkedVertexModelCount == 2 && !weighted;
        }

        private void subShortcutBtnBFS_Click(object sender, EventArgs e)
        {
            Text += " - Выполяется алгоритм нахождение кратчайшего пути с помошью построение родительского дерева";
            new Thread(() =>
            {
                CommandEntered?.Invoke(this, new ShortcutBFSCommandArgs(sourceModel, stockModel));
            }).Start();
        }
        // -----------------------------!MainToolStrip ----------------------------------- //

        // ****************** *********************** ************************************ //

        // ---------------------------- Override Methods --------------------------------- //

        public void PostMessage(string message)
        {
            MessageBox.Show(message, "Информация!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void PostWarningMessage(string warningMessage)
        {
            MessageBox.Show(warningMessage, "Предупреждение!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }



        public void PostErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Ошибка!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            BeginInvoke((MethodInvoker)(() =>
           {
               try
               {
                   switch (e?.Event)
                   {
                       case FieldEvents.InitGraph:
                           header += (_field.IsOrgraph ? " - Ориентриванный, " : " - Неориентированный, ") +
                                   (_field.IsWeighted ? "Взвещанный." : "Невзвещанный.");
                           Text = header;
                           tlbtnCrtGraph.Enabled = false;
                           tsbtnDeleteGraph.Enabled = true;
                           tsbtnMove.Enabled = true;
                           tsBtnAddVertex.Enabled = true;
                           tsbtnSaveGraph.Enabled = true;
                           break;
                       case FieldEvents.RemoveGraph:
                           header = "Visual Graph";
                           Text = header;
                           i = 0;
                           tlbtnCrtGraph.Enabled = true;
                           tsbtnDeleteGraph.Enabled = false;
                           tsbtnRemoveElems.Enabled = false;
                           tsbtnMove.Enabled = false;
                           tsBtnAddVertex.Enabled = false;
                           tsbtnSaveGraph.Enabled = false;
                           break;
                       case FieldEvents.RemoveModels:
                       case FieldEvents.RemoveModel:
                           _field.UnmarkGraphModels();
                           sourceModel = stockModel = null;
                           break;
                       default: break;
                   }
                   if (_field.Status)
                   {
                       tsbtnRemoveElems.Enabled = _field.MarkedModelsCount > 0;
                       tsBtnAddEdge.Enabled = _field.MarkedVertexModelCount == 2 && _field.MarkedModelsCount == 2;
                   }
                   Refresh();
               }
               catch (Exception ex)
               {
                   PostErrorMessage(ex.Message);
               }
           }));
        }


        public bool AnswerTheQuestion(string question)
        {
            DialogResult result = MessageBox.Show(this, question, "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes ? true : false;
        }

        // ---------------------------- !Override Methods --------------------------------- //

    }


}
