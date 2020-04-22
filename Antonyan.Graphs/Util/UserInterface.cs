using System;

using Antonyan.Graphs.Backend;
namespace Antonyan.Graphs.Util
{


    //public class UICommand 

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
        event EventHandler<UICommandEventArgs> CommandEntered;
        void PostMessage(string message);
        void PostWarningMessage(string warningMessage);
        void PostErrorMessage(string errorMessage);
        void CheckUndoRedo(bool undoPossible, bool redoPossible);
        void SetFieldStatus(bool status);
        bool MarkModel(string represantion);
        bool UnmarkModel(string represantion);
        void UnmarkAll();
        void FieldUpdate(object obj, EventArgs e);
    }

}
