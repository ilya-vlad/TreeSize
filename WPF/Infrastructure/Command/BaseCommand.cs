using System;
using System.Windows.Input;

namespace WPF.Infrastructure.Command
{
    internal abstract class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
