using PropertyChanged;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MonsterManager.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    internal class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
