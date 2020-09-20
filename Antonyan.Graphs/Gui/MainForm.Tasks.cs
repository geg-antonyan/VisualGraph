using Antonyan.Graphs.Backend.Algorithms;
using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Gui.Forms;


using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Antonyan.Graphs.Gui
{
    partial class MainForm
    {

        private void btnSaveAdjList_Click(object sender, EventArgs e)
        {
            var window = new StoredGraphNameForm();
            window.Owner = this;
            window.ShowDialog();
            if (window.Ok)
            {
                CommandEntered?.Invoke(this, new AddCurrentGraphInStoredGraphsCommandArgs(window.ListName));
            }
        }

        private void ListBoxAdjList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string name = (string)((ListBox)sender).SelectedItem;
            if (name == null) return;
            MessageBox.Show(this, _field.GetStoredGraphAdjList(name), name, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void btnUnion_Click(object sender, EventArgs e)
        {
            int count;
            if ((count = listBoxAdjList.SelectedItems.Count) == 0) return;
            string[] names = new string[count];
            int i = 0;
            foreach (var elem in listBoxAdjList.SelectedItems)
            {
                names[i++] = (string)elem;
            }
            var window = new StoredGraphNameForm();
            window.Owner = this;
            window.ShowDialog();
            if (window.Ok)
                CommandEntered?.Invoke(this, new GraphUnionCommandArgs(names, window.ListName));
        }


        private void btnRemoveAdjList_Click(object sender, EventArgs e)
        {
            int count;
            if ((count = listBoxAdjList.SelectedItems.Count) == 0) return;
            string[] names = new string[count];
            int i = 0;
            foreach (var elem in listBoxAdjList.SelectedItems)
            {
                names[i++] = (string)elem;
            }
            CommandEntered?.Invoke(this, new RemoveStoredGraphCommandArgs(names));
        }


        private void tsmiPowVertex_Click(object sender, EventArgs e)
        {
            if (_field.MarkedVertexModelCount == 1)
            {
                HalfLifeDegreeCommandArgs args = new HalfLifeDegreeCommandArgs(sourceModel);
                CommandEntered?.Invoke(this, args);
                string halfWay = _field.IsOrgraph ? "полуисхода" : "";
                PostMessage($"Степень {halfWay} вершины {sourceModel.VertexStr} равно {args.OutHalfLifeDegree}");
            }
            else PostWarningMessage("Отметьте одну и только одну вершину !!!");
        }

        private void tsmiConnectedComponents_Click(object sender, EventArgs e)
        {
            if (_field.Status == false) return;
            new Thread(() =>
            {
                var args = new ConnectedComponentsCommandArgs();
                CommandEntered?.Invoke(this, args);
                StringBuilder components = new StringBuilder();
                components.AppendLine($"Алгоритм: \"{args.AlgorithmNameOut}\"");
                for (int i = 0; i < args.ComponentsOut.Count; i++)
                {
                    components.Append($"Компонента {i + 1}: ");
                    args.ComponentsOut[i].ForEach(v => components.Append($"{v}, "));
                    components.Append("\r\n");
                }

                MessageBox.Show(components.ToString(), args.ConnectedTypeOut, MessageBoxButtons.OK, MessageBoxIcon.None);
            }).Start();

        }

        private void tsmiKruskalAlgorithm_Click(object sender, EventArgs e)
        {
            if (_field.Status == false) return;
            var args = new MSTCommandArgs(500);
            new Thread(() =>
            {
                CommandEntered?.Invoke(this, args);
                if (args.SuccsessOut)
                {
                    StringBuilder mst = new StringBuilder();
                    args.MstOut.ForEach(str => mst.AppendLine(str));
                    mst.AppendLine($"Суммарный вес: {args.SummWeightOut}");
                    ResultForm result = new ResultForm(args.TaskNameOut, args.AlgorithmNameOut, mst.ToString());
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        result.Owner = this;
                        result.ShowDialog();
                        if (result.Stream != null)
                        {
                            mst.Clear();
                            mst.AppendLine(result.TaskName);
                            mst.AppendLine(result.AlgorithmName);
                            mst.AppendLine(result.ResultText);
                            CommandEntered?.Invoke(this, new SaveAlgorithmResultCommandArgs(result.Stream, mst.ToString()));
                        }
                    }));
                }
            }).Start();
        }

        private void tsmiDijkstraAlgorithm_Click(object sender, EventArgs e)
        {
            if (_field.Status == false) return;
            new Thread(() =>
            {
                DijkstraCommandArgs args = null;
                if (_field.MarkedVertexModelCount == 1)
                {
                    args = new DijkstraCommandArgs(sourceModel);
                    CommandEntered.Invoke(this, args);
                    if (args.SuccsessOut)
                    {
                        if (args.OutWays.Count == 0)
                            PostMessage("Пути к данной вершини отсутсвуют!!!");
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            args.OutWays.ForEach(w => sb.AppendLine(w));
                            ResultForm resform = new ResultForm(args.TaskNameOut, args.AlgorithmNameOut, sb.ToString());
                            BeginInvoke((MethodInvoker)(() =>
                            {
                                resform.Owner = this;
                                resform.ShowDialog();
                                if (resform.Stream != null)
                                {
                                    sb.Clear();
                                    sb.AppendLine(resform.TaskName);
                                    sb.AppendLine(resform.AlgorithmName);
                                    sb.AppendLine(resform.ResultText);
                                    CommandEntered?.Invoke(this, new SaveAlgorithmResultCommandArgs(resform.Stream, sb.ToString()));
                                }
                            }));
                        }
                    }

                }
                else
                {
                    PostWarningMessage("Надо вбрать одну вершину!!!");
                }
            }).Start();
        }

        private void tsmiFordBellmanAlgorithm_Click(object sender, EventArgs e)
        {
            if (_field.Status == false) return;
            if (_field.MarkedVertexModelCount == 2)
            {
                SetWeightForm form = new SetWeightForm("Укажите макс длину L", "Найти");
                form.Owner = this;
                form.ShowDialog();
                if (!form.Ok) return;
                bool succsess = int.TryParse(form.Weight, out var l);
                if (!succsess)
                {
                    PostErrorMessage("Некорректное число L");
                    return;
                }
                new Thread(() =>
                {
                    var args = new WayNoMoreThenLCommandArgs(sourceModel, stockModel, l);
                    CommandEntered.Invoke(this, args);
                    if (args.SuccsessOut)
                    {
                        if (!args.Exist)
                            PostMessage($"Пути не больше {l} не существуют!!");
                        else
                        {
                            StringBuilder sb = new StringBuilder();
                            args.OutWays.ForEach(w => sb.AppendLine(w));
                            ResultForm resform = new ResultForm(args.TaskNameOut, args.AlgorithmNameOut, sb.ToString());
                            BeginInvoke((MethodInvoker)(() =>
                            {
                                resform.Owner = this;
                                resform.ShowDialog();
                                if (resform.Stream != null)
                                {
                                    sb.Clear();
                                    sb.AppendLine(resform.TaskName);
                                    sb.AppendLine(resform.AlgorithmName);
                                    sb.AppendLine(resform.ResultText);
                                    CommandEntered?.Invoke(this, new SaveAlgorithmResultCommandArgs(resform.Stream, sb.ToString()));
                                }
                            }));
                        }
                    }
                }).Start();
            }
            else
            {
                PostWarningMessage("Надо вбрать две вершини!!!");
            }
        }

        private void tsmiEdmondsKarp_Click(object sender, EventArgs e)
        {
            if (_field.Status == false) return;
            if (_field.MarkedVertexModelCount == 2)
            {
                new Thread(() =>
                {
                    try
                    {
                        var args = new EdmondsKarpCommandArgs(sourceModel, stockModel);
                        CommandEntered.Invoke(this, args);
                        if (args.SuccsessOut)
                        {
                            ResultForm resform = new ResultForm(args.TaskNameOut, args.AlgorithmNameOut, args.MaxFlowOut.ToString());
                            BeginInvoke((MethodInvoker)(() =>
                            {
                                resform.Owner = this;
                                resform.ShowDialog();
                                if (resform.Stream != null)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine(resform.TaskName);
                                    sb.AppendLine(resform.AlgorithmName);
                                    sb.AppendLine(resform.ResultText);
                                    CommandEntered?.Invoke(this, new SaveAlgorithmResultCommandArgs(resform.Stream, sb.ToString()));
                                }
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        PostErrorMessage(ex.Message);
                    }
                }).Start();
            }
            else
            {
                PostWarningMessage("Надо вбрать две вершини!!!");
            }
        }



        private void tsmiNPeriohery_Click(object sender, EventArgs e)
        {
            if (_field.Status == false) return;
            if (_field.MarkedVertexModelCount == 1)
            {
                SetWeightForm form = new SetWeightForm("Укажите N", "Найти");
                form.Owner = this;
                form.ShowDialog();
                if (!form.Ok) return;
                bool succsess = int.TryParse(form.Weight, out var N);
                if (!succsess)
                {
                    PostErrorMessage("Некорректное число N");
                    return;
                }
                new Thread(() =>
                {
                    var args = new NPeripheryCommandArgs(sourceModel, N);
                    CommandEntered.Invoke(this, args);
                    if (args.SuccsessOut)
                    {
                            StringBuilder sb = new StringBuilder();
                            args.NPeripheryOut.ForEach(w => sb.AppendLine(w));
                            ResultForm resform = new ResultForm(args.TaskNameOut, args.AlgorithmNameOut, sb.ToString());
                            BeginInvoke((MethodInvoker)(() =>
                            {
                                resform.Owner = this;
                                resform.ShowDialog();
                                if (resform.Stream != null)
                                {
                                    sb.Clear();
                                    sb.AppendLine(resform.TaskName);
                                    sb.AppendLine(resform.AlgorithmName);
                                    sb.AppendLine(resform.ResultText);
                                    CommandEntered?.Invoke(this, new SaveAlgorithmResultCommandArgs(resform.Stream, sb.ToString()));
                                }
                            }));
                    }
                }).Start();
            }
            else
            {
                PostWarningMessage("Надо вбрать одну вершину!!!");
            }

        }

    }

}

