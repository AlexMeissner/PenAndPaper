using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Commands
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> _execute;

        public event EventHandler? CanExecuteChanged = (sender, e) => { };

        public AsyncCommand(Func<Task> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        void ICommand.Execute(object? parameter)
        {
            FireAndForget(_execute());
        }

        private static async void FireAndForget(Task task)
        {
            try
            {
                await task;
            }
            catch { /* ToDo: Exception Handling */ }
        }
    }

    public class AsyncCommand<T> : ICommand
    {
        private readonly Func<T, Task> _execute;

        public event EventHandler? CanExecuteChanged = (sender, e) => { };

        public AsyncCommand(Func<T, Task> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        void ICommand.Execute(object? parameter)
        {
            if (parameter is T param)
            {
                FireAndForget(_execute(param));
            }
        }

        private static async void FireAndForget(Task task)
        {
            try
            {
                await task;
            }
            catch { /* ToDo: Exception Handling */ }
        }
    }
}
