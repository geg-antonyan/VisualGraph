﻿using System;

using Antonyan.Graphs.Backend;
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
        event EventHandler<UICommandEventArgs> CommandEntered;
        void PostMessage(string message);
        void FieldUpdate(object obj, EventArgs e);
        void CheckUndoRedo(bool undoPossible, bool redoPossible);
        void AttachField(object field);
    }

}
