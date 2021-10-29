using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
