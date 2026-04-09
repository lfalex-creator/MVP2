using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Tema2.Commands
{
    class IndependentExecutionCommand : ICommand
    {
        private Action commandTask;

        public IndependentExecutionCommand(Action workToDo) => commandTask = workToDo;

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter) => commandTask();
    }
}
