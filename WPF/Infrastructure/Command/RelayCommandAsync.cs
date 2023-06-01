using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using WPF.ViewModel;

namespace WPF.Infrastructure.Command
{
    public class RelayCommandAsync : AsyncBaseCommand
    {
        private readonly Func<object, Task<bool>> _canExecuted;
        private readonly Func<object, Task> _execute;

        public RelayCommandAsync(Func<object, Task<bool>> canExecuted, Func<object, Task> execute)
        {
            _canExecuted = canExecuted;
            _execute = execute;
        }

        public override bool CanExecute(object parameter)
        { 
            return _canExecuted(parameter).Result;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _execute(parameter);
        }
    }
}
