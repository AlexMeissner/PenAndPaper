using Client.Helper;
using Client.Services;
using System.Windows.Controls;

namespace Client.View
{
    public partial class CampaignSelectionPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;

        public CampaignSelectionPage(IPageNavigator pageNavigator, ISessionData sessionData)
        {
            InitializeComponent();
            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
            BackgroundImage.ImageSource = RandomBackgroundImage.GetImageFromResource();
        }
    }
}