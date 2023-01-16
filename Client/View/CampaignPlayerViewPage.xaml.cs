﻿using Client.Controls;
using Client.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Client.View
{
    public partial class CampaignPlayerViewPage : Page
    {
        private readonly IPageNavigator _pageNavigator;
        private readonly ISessionData _sessionData;

        public CampaignPlayerViewPage(IServiceProvider serviceProvider, IPageNavigator pageNavigator, ISessionData sessionData)
        {
            InitializeComponent();

            MapPresenter.Content = serviceProvider.GetRequiredService<MapControl>();

            _pageNavigator = pageNavigator;
            _sessionData = sessionData;
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            _sessionData.CampaignId = null;
            _pageNavigator.OpenPage<CampaignSelectionPage>();
        }
    }
}