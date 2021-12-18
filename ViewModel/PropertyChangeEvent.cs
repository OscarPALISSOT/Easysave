using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EasySave.ViewModel
{
    class PropertyChangeEvent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}