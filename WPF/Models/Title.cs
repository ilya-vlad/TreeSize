using WPF.Infrastructure;

namespace WPF.Models
{
    public class Title : BasePropertyChanged
    {
        private string name;

        public string Name
        {
            get => name;
            set => Set(ref name, value);
        }
    }
}
