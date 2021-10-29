using System;

namespace WPF.Infrastructure.Command
{
    internal class ActionCommand : BaseCommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        public ActionCommand(Action<object> Execute, Func<object, bool> CanExecute = null)
        {
            execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            canExecute = CanExecute;
        }

        public override bool CanExecute(object p) => canExecute?.Invoke(p) ?? true;
        public override void Execute(object p) => execute(p);   
    }
}
