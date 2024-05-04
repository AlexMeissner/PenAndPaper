using System.Windows.Input;

namespace MonsterManager.ViewModels
{
    internal class RelayCommand(Action action) : ICommand
    {
        public event EventHandler? CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            action?.Invoke();
        }
    }

    internal class RelayCommandWithParameter(Action<object?> action) : ICommand
    {
        public event EventHandler? CanExecuteChanged = (sender, e) => { };

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            action?.Invoke(parameter);
        }
    }
}
