using Client.Commands;
using Client.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class DebugViewModel : BaseViewModel
    {
        private readonly IUpdateNotifier _updateNotifier;

        public ICommand UpdateEventSubscribersCommand { get; init; }

        public ObservableCollection<Delegate> EventSubscribers { get; set; } = [];

        public DebugViewModel(IUpdateNotifier updateNotifier)
        {
            _updateNotifier = updateNotifier;

            UpdateEventSubscribersCommand = new RelayCommand(UpdateEventSubscribers);
        }

        private void UpdateEventSubscribers()
        {
            var subscribers = _updateNotifier.GetSubscribers();
            EventSubscribers.ReplaceWith(subscribers);
        }
    }
}
