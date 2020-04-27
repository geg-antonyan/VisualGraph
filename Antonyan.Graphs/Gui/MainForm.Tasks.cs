using Antonyan.Graphs.Backend.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                string halfWay = oriented ? "полуисхода" : "";
                PostMessage($"Степень {halfWay} вершины {sourceModel.VertexStr} равно {args.OutHalfLifeDegree}");
            }
            else PostWarningMessage("Отметьте одну и только одну вершину !!!");
        }
    }
}
