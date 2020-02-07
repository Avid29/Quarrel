﻿using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Quarrel.ViewModels.Helpers;
using Quarrel.ViewModels.Messages;
using Quarrel.ViewModels.Messages.Gateway;
using Quarrel.ViewModels.Services.Cache;
using Quarrel.ViewModels.Services.Discord.Rest;
using Quarrel.ViewModels.Services.Navigation;
using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Quarrel.Controls.Shell
{
    public sealed partial class ExtendedSplashScreen : UserControl
    {
        public ExtendedSplashScreen()
        {
            this.InitializeComponent();
            
            // Status changed
            // Updates status text and ther appropiate actions
            Messenger.Default.Register<ConnectionStatusMessage>(this, async m =>
            {
                await DispatcherHelper.RunAsync(() =>
                {
                    StatusBlock.Text = m.Status.ToString().ToUpper();
                });

                if (m.Status == Status.Failed || m.Status == Status.Offline)
                {
                    // Stops animation on failed or offline
                    await DispatcherHelper.RunAsync(() =>
                    {
                        Animation.Stop();
                    });

                    // Opens status page
                    if (!_Retry)
                    {
                        SimpleIoc.Default.GetInstance<ISubFrameNavigationService>().NavigateTo("DiscordStatusPage");
                    }
                }

                // Shows Retry button 
                if (m.Status == Status.Failed)
                {
                    await DispatcherHelper.RunAsync(() =>
                    {
                        RetryButton.Visibility = Visibility.Visible;
                    });
                }

                // Reshow Splash
                if (m.Status == Status.Disconnected)
                {
                    await DispatcherHelper.RunAsync(() =>
                    {
                        Visibility = Visibility.Visible;
                        LoadIn.Begin();
                        RetryConnecting(null, null);
                    });
                }
            });
            
            // Finished loading
            // Begins hiding splash
            Messenger.Default.Register<GatewayReadyMessage>(this, async _ => 
            {
                await DispatcherHelper.RunAsync(() => 
                {
                    LoadOut.Begin();
                });
            });

            LoadQuote();
        }

        /// <summary>
        /// Gets a random splash text quote
        /// </summary>
        public async void LoadQuote()
        {
            var splash = await Helpers.Constants.FromFile.GetRandomSplash();
            MessageBlock.Text = splash.Text;
            CreditBlock.Text = splash.Credit;
        }

        /// <summary>
        /// Alligns icon with old splash icon and begins animation
        /// </summary>
        /// <param name="ogSplash">Static splash screen data</param>
        public void InitializeAnimation(SplashScreen ogSplash)
        {
            // Setup icon
            AdjustSize(ogSplash);
            LoadIn.Begin();
        }

        /// <summary>
        /// Adjust ViewBox size
        /// </summary>
        /// <param name="ogSplash">Static splash screen data</param>
        public void AdjustSize(SplashScreen ogSplash)
        {
            try
            {
                var location = ogSplash.ImageLocation;
                viewbox.Width = location.Width;
                viewbox.Height = location.Height;

                //this.Focus(FocusState.Pointer);
                stack.Margin = new Thickness(0, location.Bottom, 0, 0);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// Finish Load in
        /// </summary>
        private void LoadIn_Completed(object sender, object e)
        {
            Animation.Begin();
        }

        /// <summary>
        /// Finish Load out
        /// </summary>
        private void LoadOut_Completed(object sender, object e)
        {
            Visibility = Visibility.Collapsed;
            Animation.Stop();
        }

        /// <summary>
        /// Attempts to open a connection to Discord (again)
        /// </summary>
        private async void RetryConnecting(object sender, RoutedEventArgs e)
        {
            // Retrying
            _Retry = true;

            // Reset View State
            RetryButton.Visibility = Visibility.Collapsed;
            Animation.Begin();

            // Login
            string token = (string)await SimpleIoc.Default.GetInstance<ICacheService>()
                .Persistent.Roaming.TryGetValueAsync<object>(Constants.Cache.Keys.AccessToken);

            await SimpleIoc.Default.GetInstance<IDiscordService>().Login(token);
        }

        /// <summary>
        /// Indicates that a connection attempt is a retry
        /// </summary>
        private bool _Retry = false;

        /// <summary>
        /// Shows the Discord Status
        /// </summary>
        private void ShowDiscordStatus(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<ISubFrameNavigationService>().NavigateTo("DiscordStatusPage");
        }
    }
}
