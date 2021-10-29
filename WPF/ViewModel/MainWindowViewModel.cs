using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Infrastructure.Command;

namespace WPF.ViewModel
{
    internal class MainWindowViewModel : ViewModel
    {
        private string title = "TreeSize";
        public string Title
        {
            get => title;
            set => Set(ref title, value);
        }

        //public ICommand CloseApplicationCommand { get; }

        //private void OnCloseApplicationCommandExecuted(object p) 
        //{
        //    Application.Current.Shutdown();
        //}
        //private bool CanCloseApplicationCommandExecute(object p) => true;
        public MainWindowViewModel()
        {
            //CloseApplicationCommand = new ActionCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);
        }
    }
}
