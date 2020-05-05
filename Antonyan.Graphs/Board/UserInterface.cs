using System;
using Antonyan.Graphs.Board;
using Antonyan.Graphs.Board.Models;
using Antonyan.Graphs.Backend.Commands;

using Antonyan.Graphs.Backend;

namespace Antonyan.Graphs.Board
{
    public interface UserInterface
    {
        event EventHandler<ACommandArgs> CommandEntered;
        void PostMessage(string message);
        void PostWarningMessage(string warningMessage);
        void PostErrorMessage(string errorMessage);
        void CheckUndoRedo(bool undoPossible, bool redoPossible);
        bool AnswerTheQuestion(string question);
        //void SetFieldStatus(bool status);
        //bool MarkModel(string represantion);
        //bool UnmarkModel(string represantion);
        //void UnmarkAll();
        void PostStatusMessage(string message);
        void FieldUpdate(object obj, ModelFieldUpdateArgs e);
    }

}
