using System;
using System.Windows.Input;

namespace DragonAge2CameraTools.ViewLogic.Commands
{
    public class RelayCommand: ICommand
    {
        private readonly Action<object> _action;
 
        public RelayCommand(Action<object> action)
        {
            _action = action;
        }
 
        public bool CanExecute(object parameter)
        {
            return true;
        }
 
        public void Execute(object parameter)
        {
            _action(parameter);
        }
 
#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67
    }
}