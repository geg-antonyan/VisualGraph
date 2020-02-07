using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Antonyan.Graphs.Data;
using Antonyan.Graphs.Desk.Geometry;

namespace Antonyan.Graphs.Util
{

    public class UICommandEventArgs : EventArgs
    {
        public UICommandEventArgs(string message)
        {
            Message = message;
        }
        public string Message { get; private set; }
    }
    public interface UserInterface
    {
        event EventHandler<UICommandEventArgs> CommandEnterd;
        void PassCommand(string message);
        void FieldUpdate(object obj, EventArgs e);
        
    }

}
