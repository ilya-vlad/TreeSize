using System.Windows;

namespace WPF.Infrastructure.Command
{
    internal class CloseApllicationCommand : BaseCommand
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => Application.Current.Shutdown();
    }
}
