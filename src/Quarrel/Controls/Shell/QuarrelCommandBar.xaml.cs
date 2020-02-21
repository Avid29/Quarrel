﻿// Copyright (c) Quarrel. All rights reserved.

using DiscordAPI.Models;
using GalaSoft.MvvmLight.Ioc;
using Quarrel.ViewModels;
using Quarrel.ViewModels.Services.Navigation;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Quarrel.Controls.Shell
{
    /// <summary>
    /// Custom CommandBar instance used in shell.
    /// </summary>
    public sealed partial class QuarrelCommandBar : CommandBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuarrelCommandBar"/> class.
        /// </summary>
        public QuarrelCommandBar()
        {
            this.InitializeComponent();

            this.DataContextChanged += (s, e) =>
            {
                this.Bindings.Update();
            };
        }

        /// <summary>
        /// Invoked when Hamburger button is clicked
        /// </summary>
        public event EventHandler HamburgerClicked;

        /// <summary>
        /// Invoked when MemberListToggle button is clicked
        /// </summary>
        public event EventHandler MemberListButtonClicked;

        /// <summary>
        /// Gets the MainViewModel for the app.
        /// </summary>
        public MainViewModel ViewModel => App.ViewModelLocator.Main;

        /// <summary>
        /// Gets or sets a value indicating whether or not the Hamburger button should be shown by this CommandBar.
        /// </summary>
        public bool ShowHamburger { get; set; }

        /// <summary>
        /// Gets Current channel as GuildChannel.
        /// </summary>
        private GuildChannel GuildChannel { get => ViewModel.CurrentChannel != null ? ViewModel.CurrentChannel.Model as GuildChannel : null; }

        /// <summary>
        /// Gets the topic of the current channel (if GuildChannel).
        /// </summary>
        private string ChannelTopic { get => GuildChannel != null ? GuildChannel.Topic : string.Empty; }

        /// <summary>
        /// Changes the VisualStateManager to confirm open down.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var layoutRoot = GetTemplateChild("LayoutRoot") as Grid;
            if (layoutRoot != null)
            {
                VisualStateManager.SetCustomVisualStateManager(layoutRoot, new OpenDownCommandBarVisualStateManager());
            }
        }

        /// <summary>
        /// Invokes the HumburgerClicked event.
        /// </summary>
        private void InvokeHumburgerClick(object sender, RoutedEventArgs e)
        {
            HamburgerClicked(this, null);
        }

        /// <summary>
        /// Invokes the MemberListButtonClicked event.
        /// </summary>
        private void InvokeMemberListToggleClick(object sender, RoutedEventArgs e)
        {
            MemberListButtonClicked(this, null);
        }

        /// <summary>
        /// Opens Channel TopicPage for current channel.
        /// </summary>
        private void ChannelNameTapped(object sender, TappedRoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<ISubFrameNavigationService>().NavigateTo("TopicPage", ViewModel.CurrentChannel);
        }
    }
}
