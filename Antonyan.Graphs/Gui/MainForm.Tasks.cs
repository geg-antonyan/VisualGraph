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
        private void tsBtnHalfWayTop_Click(object sender, EventArgs e)
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
            MessageBox.Show( this, _field.GetStoredGraphAdjList(name), name, MessageBoxButtons.OK, MessageBoxIcon.None);
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

        private void tsBtnExplorerConnComponent_Click(object sender, EventArgs e)
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

    }
}
