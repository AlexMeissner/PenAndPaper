using Client.Commands;
using Client.Services;
using Client.Services.API;
using DataTransfer.Script;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class ScriptViewModel : BaseViewModel
    {
        public string Script { get; set; } = string.Empty;
        public string ScriptBackup { get; set; } = string.Empty;

        public Visibility EditVisibility { get; set; } = Visibility.Collapsed;
        public Visibility RenderVisibility { get; set; } = Visibility.Visible;

        public ICommand CancelCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        private readonly IActiveMapApi _activeMapApi;
        private readonly ISessionData _sessionData;
        private readonly IScriptApi _scriptApi;
        private readonly IUpdateNotifier _updateNotifier;


        public ScriptViewModel(IScriptApi scriptApi, IActiveMapApi activeMapApi, ISessionData sessionData, IUpdateNotifier updateNotifier)
        {
            _activeMapApi = activeMapApi;
            _sessionData = sessionData;
            _scriptApi = scriptApi;
            _updateNotifier = updateNotifier;

            CancelCommand = new RelayCommand(OnCancel);
            EditCommand = new RelayCommand(OnEdit);
            SaveCommand = new AsyncCommand(OnSave);

            _updateNotifier.MapChanged += OnMapChanged;
        }

        public void UnsubscribeEventHandlers()
        {
            _updateNotifier.MapChanged -= OnMapChanged;
        }

        public void OnEdit()
        {
            ScriptBackup = Script;
            EditVisibility = Visibility.Visible;
            RenderVisibility = Visibility.Collapsed;
        }

        public void OnCancel()
        {
            Script = ScriptBackup;
            EditVisibility = Visibility.Collapsed;
            RenderVisibility = Visibility.Visible;
        }

        public async Task OnLoaded()
        {
            await UpdateScript();
        }

        public async Task OnSave()
        {
            EditVisibility = Visibility.Collapsed;
            RenderVisibility = Visibility.Visible;

            if (await GetMapId() is int mapId)
            {
                var payload = new ScriptDto(mapId, Script);
                await _scriptApi.Update(payload);
            }
        }

        private async void OnMapChanged(object? sender, EventArgs e)
        {
            await UpdateScript();
        }

        private async Task<int?> GetMapId()
        {
            var activeMapResponse = await _activeMapApi.GetAsync(_sessionData.CampaignId);

            int? mapId = null;

            activeMapResponse.Match(
                success => mapId = success.MapId,
                fail => throw new Exception("Could not retreive active map."));

            return mapId;
        }

        private async Task UpdateScript()
        {
            if (await GetMapId() is int mapId)
            {
                var script = await _scriptApi.Get(mapId);

                script.Match(
                    s => Script = s.Text,
                    f => throw new Exception("Could not retreive script."));
            }
        }
    }
}
