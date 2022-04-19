﻿// Quarrel © 2022

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using Quarrel.Messages;
using Quarrel.Messages.Navigation.SubPages;
using Quarrel.Messages.Panel;
using Quarrel.Services.Windows;
using Quarrel.ViewModels.Panels;
using Quarrel.ViewModels.SubPages.DiscordStatus;
using Quarrel.ViewModels.SubPages.Meta;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Quarrel.Controls.Shell
{
    public sealed partial class QuarrelCommandBar : CommandBar
    {
        private readonly IWindowService _windowService;
        private readonly IMessenger _messenger;

        public QuarrelCommandBar()
        {
            this.InitializeComponent();
            _messenger = App.Current.Services.GetRequiredService<IMessenger>();
            _windowService = App.Current.Services.GetRequiredService<IWindowService>();
        }

        private void ToggleMemberList(object sender, RoutedEventArgs e)
            => _messenger.Send(new TogglePanelMessage(PanelSide.Right, PanelState.Toggle));

        private void HamburgerClicked(object sender, RoutedEventArgs e)
            => _messenger.Send(new TogglePanelMessage(PanelSide.Left, PanelState.Toggle));

        private void GoToDiscordStatus(object sender, RoutedEventArgs e)
            => _messenger.Send(new NavigateToSubPageMessage(typeof(DiscordStatusViewModel)));

        private void GoToAbout(object sender, RoutedEventArgs e)
            => _messenger.Send(new NavigateToSubPageMessage(typeof(AboutPageViewModel)));

        private void OpenInNewWindow(object sender, RoutedEventArgs e)
        {
            _windowService.OpenSecondaryWindow();
        }
    }
}
