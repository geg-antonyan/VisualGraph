using System;
using System.Windows.Forms;
using System.Threading;

using Antonyan.Graphs.Gui.Forms;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend.Algorithms;
using Antonyan.Graphs.Backend;
using Antonyan.Graphs.Backend.Commands;
using System.IO;

namespace Antonyan.Graphs.Gui
{
    partial class MainForm
    {
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


        private void tsBtnRefresh_Click(object sender, EventArgs e)
        {
            if (_field.Status)
                _field.RefreshDefault();
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
            if (!_field.Status) return;
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
            toolStripStatusLabel.Text = "Выполяется алгоритм обхода в глубину";
            _field.UnmarkGraphModels();
            new Thread(() =>
            CommandEntered?.Invoke(this, new DFScommandArgs(sourceModel))).Start();
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
            subShortcutBtnBFS.Enabled = _field.MarkedVertexModelCount == 2 && !_field.IsWeighted;
        }

        private void subShortcutBtnBFS_Click(object sender, EventArgs e)
        {
            Text += " - Выполяется алгоритм нахождение кратчайшего пути с помошью построение родительского дерева";
            new Thread(() =>
            {
                CommandEntered?.Invoke(this, new ShortcutBFSCommandArgs(sourceModel, stockModel));
            }).Start();
        }
    }
}
