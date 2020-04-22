using System;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend.Commands;
using Antonyan.Graphs.Backend.UICommandArgs;


namespace Antonyan.Graphs.Board
{
    public interface UserInterface
    {
        event EventHandler<UIEventArgs> CommandEntered;
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
