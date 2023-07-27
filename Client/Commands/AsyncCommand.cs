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
}
