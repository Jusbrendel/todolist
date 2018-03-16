using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TodoList.Helper
{
    // ICommand MVVM implementation

    class Command : ICommand
    {

        private Action<Object> _action;
        private bool _canExecute;

        public Command(Action<object> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }


    }
}
