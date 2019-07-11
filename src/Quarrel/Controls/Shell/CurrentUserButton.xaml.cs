﻿using DiscordAPI.Models;
using GalaSoft.MvvmLight.Messaging;
using Quarrel.Messages.Gateway;
using Quarrel.Models.Bindables;
using Quarrel.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UICompositionAnimations.Helpers;
using DiscordAPI.Gateway;
using Quarrel.Messages.Posts.Requests;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Quarrel.Controls.Shell
{
    public sealed partial class CurrentUserButton : UserControl
    {
        public CurrentUserButton()
        {
            this.InitializeComponent();

            Messenger.Default.Register<GatewayReadyMessage>(this, async m =>
            {
                await DispatcherHelper.RunAsync(() => 
                {
                    DataContext = new BindableUser(new GuildMember() { User = m.EventData.User });
                });
            });

            Messenger.Default.Register<GatewayGuildSyncMessage>(this, async _ =>
            {
                await DispatcherHelper.RunAsync(() => { this.Bindings.Update(); });
            });

            Messenger.Default.Register<GatewayPresenceUpdatedMessage>(this, async m => 
            {
                await DispatcherHelper.RunAsync(() => 
                {
                    if (ViewModel != null && ViewModel.Model.User.Id == m.UserId)
                        this.Bindings.Update();
                });
            });

            Messenger.Default.Register<GatewayUserSettingsUpdatedMessage>(this, async m => 
            {
                await DispatcherHelper.RunAsync(() => 
                {
                    this.Bindings.Update();
                });
            });

            Messenger.Default.Register<CurrentUserRequestMessage>(this, async m =>
            {
                await DispatcherHelper.RunAsync(() => m.ReportResult(ViewModel));
            });

            this.DataContextChanged += (s, e) =>
            {
                this.Bindings.Update();
            };
        }

        public BindableUser ViewModel => DataContext as BindableUser;

        private async void StatusSelected(object sender, RoutedEventArgs e)
        {
            string status = (sender as RadioButton).Tag.ToString();
            ServicesManager.Discord.Gateway.Gateway.UpdateStatus(status, 0, null);
            await ServicesManager.Discord.UserService.UpdateSettings("{\"status\":\"" + status + "\"}");
        }
    }
}
