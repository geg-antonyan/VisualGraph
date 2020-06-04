using System;
using System.Linq;
using System.Windows.Forms;
using System.Media;

using Antonyan.Graphs.Board;
using System.Drawing;

namespace Antonyan.Graphs.Gui
{
    partial class MainForm
    {
        private bool occupyInfoList = false;
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


        public void PostStatusMessage(string message)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                // SystemSounds.Exclamation.Play();
                toolStripStatusLabel.ForeColor = Color.Red;
                toolStripStatusLabel.Text = message;
            }));
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
                            header = "Visual Graph " + (_field.IsOrgraph ? " - Ориентриванный, " : " - Неориентированный, ") +
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
                            txtInfoList.Text = "";
                            break;
                        case FieldEvents.RemoveModels:
                        case FieldEvents.RemoveModel:
                            _field.UnmarkGraphModels();
                            sourceModel = stockModel = null;
                            break;
                        case FieldEvents.UpdateStoredGraphs:
                            {
                                listBoxAdjList.Items.Clear();
                                _field.GetStoredGraphsName().ForEach(name => listBoxAdjList.Items.Add(name));
                                break;
                            }
                        default: break;
                    }
                    if (_field.Status)
                    {
                        if (!occupyInfoList)
                            txtInfoList.Text = _field.GetAdjListToString();
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
    }
}
